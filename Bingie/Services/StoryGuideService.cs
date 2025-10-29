using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Models;

namespace Bingie.Services;

/// <summary>
/// Coordinates trigger selections, experiment lifecycle, and story summaries.
/// </summary>
public sealed class StoryGuideService
{
    private readonly IStoryGuideRepository _repository;
    private readonly IStoryGuideAnalytics _analytics;
    private readonly IReadOnlyDictionary<string, StoryTriggerDefinition> _triggerLookup;

    public StoryGuideService(IStoryGuideRepository repository, IStoryGuideAnalytics analytics)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _analytics = analytics ?? throw new ArgumentNullException(nameof(analytics));

        _triggerLookup = StoryGuideCatalogue.GetTriggers()
            .ToDictionary(trigger => trigger.Code, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyList<StoryTriggerDefinition> GetTriggers()
    {
        return StoryGuideCatalogue.GetTriggers();
    }

    public StoryTriggerDefinition GetTriggerDefinition(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return StoryGuideCatalogue.GetTrigger(StoryGuideCatalogue.CustomTriggerCode);

        return _triggerLookup.TryGetValue(code, out var definition)
            ? definition
            : StoryGuideCatalogue.GetTrigger(StoryGuideCatalogue.CustomTriggerCode);
    }

    public async Task<StoryChapterResult> RecordTriggerSelectionAsync(
        string username,
        DateTime entryDateUtc,
        string triggerCode,
        string? customTrigger,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        }

        var normalizedUser = username.Trim();
        var normalizedTrigger = NormalizeTriggerCode(triggerCode);
        var definition = GetTriggerDefinition(normalizedTrigger);

        StoryTriggerSelection selection = new()
        {
            Username = normalizedUser,
            TriggerCode = definition.Code,
            CustomTrigger = RequiresCustomText(definition.Code) ? customTrigger?.Trim() : null,
            EntryDateUtc = NormalizeUtc(entryDateUtc),
            CreatedUtc = nowUtc
        };

        await _repository.UpsertTriggerSelectionAsync(selection);
        _analytics.TrackTriggerLogged(normalizedUser, definition.Code);

        var suggestion = await PickExperimentSuggestionAsync(normalizedUser, definition, nowUtc);

        return new StoryChapterResult
        {
            Trigger = definition,
            ChapterTitle = BuildChapterTitle(definition),
            ChapterBody = BuildChapterBody(definition, selection.CustomTrigger),
            Experiment = suggestion
        };
    }

    public async Task<StoryExperimentProgress> PlanExperimentAsync(string username, string experimentCode, DateTime nowUtc)
    {
        var normalizedUser = ValidateUser(username);
        var definition = ResolveExperiment(experimentCode);
        var progress = await _repository.GetExperimentProgressAsync(normalizedUser, definition.Code);

        var updated = (progress ?? new StoryExperimentProgress
        {
            Username = normalizedUser,
            ExperimentCode = definition.Code,
            Status = StoryExperimentStatus.None
        }).With(
            status: StoryExperimentStatus.Planned,
            plannedUtc: nowUtc);

        await _repository.UpsertExperimentProgressAsync(updated);
        _analytics.TrackExperimentPlanned(normalizedUser, definition.Code);

        return updated;
    }

    public async Task<StoryExperimentCompletionResult> CompleteExperimentAsync(string username, string experimentCode, DateTime nowUtc)
    {
        var normalizedUser = ValidateUser(username);
        var definition = ResolveExperiment(experimentCode);
        var existing = await _repository.GetExperimentProgressAsync(normalizedUser, definition.Code);
        if (existing == null)
        {
            throw new InvalidOperationException("Experiment has not been suggested yet.");
        }

        var xpAwarded = existing.XpGranted ? 0 : definition.XpReward;

        var updated = existing.With(
            status: StoryExperimentStatus.Completed,
            completedUtc: nowUtc,
            xpGranted: existing.XpGranted || xpAwarded > 0);

        await _repository.UpsertExperimentProgressAsync(updated);
        if (xpAwarded > 0)
        {
            _analytics.TrackExperimentCompleted(normalizedUser, definition.Code, xpAwarded);
        }
        else
        {
            _analytics.TrackExperimentCompleted(normalizedUser, definition.Code, 0);
        }

        return new StoryExperimentCompletionResult
        {
            Summary = MapSummary(definition, updated),
            XpAwarded = xpAwarded
        };
    }

    public async Task<StoryWeeklyEpisode> GetWeeklyEpisodeAsync(string username, DateTime todayUtc)
    {
        var normalizedUser = ValidateUser(username);
        var weekStart = StartOfWeek(NormalizeUtc(todayUtc));
        var weekEnd = weekStart.AddDays(7);

        var selections = await _repository.GetTriggerSelectionsAsync(normalizedUser, weekStart, weekEnd);
        var grouped = selections
            .GroupBy(s => NormalizeTriggerCode(s.TriggerCode))
            .Select(group => new StoryTriggerCount
            {
                Definition = GetTriggerDefinition(group.Key),
                Count = group.Count()
            })
            .OrderByDescending(item => item.Count)
            .ThenBy(item => item.Definition.Title, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var experiments = await _repository.GetExperimentProgressAsync(normalizedUser);
        var experimentSummaries = experiments
            .Select(progress => MapSummary(ResolveExperiment(progress.ExperimentCode), progress))
            .Where(summary => summary.Status != StoryExperimentStatus.None)
            .OrderBy(summary => summary.Status == StoryExperimentStatus.Completed ? 1 : 0)
            .ThenByDescending(summary => summary.LastSuggestedUtc ?? DateTime.MinValue)
            .ToList();

        return new StoryWeeklyEpisode
        {
            WeekStartUtc = weekStart,
            TriggerCounts = grouped,
            Experiments = experimentSummaries
        };
    }

    private async Task<StoryExperimentSuggestion?> PickExperimentSuggestionAsync(
        string username,
        StoryTriggerDefinition trigger,
        DateTime nowUtc)
    {
        var experiments = StoryGuideCatalogue.GetExperiments(trigger.Code);
        if (experiments.Count == 0) return null;

        var progressList = await _repository.GetExperimentProgressAsync(username);
        var candidate = SelectExperimentCandidate(experiments, progressList);
        if (candidate == null) return null;

        var existing = progressList.FirstOrDefault(p => string.Equals(p.ExperimentCode, candidate.Code, StringComparison.OrdinalIgnoreCase));
        var alreadyPlanned = existing?.Status >= StoryExperimentStatus.Planned;

        var updated = (existing ?? new StoryExperimentProgress
        {
            Username = username,
            ExperimentCode = candidate.Code,
            Status = StoryExperimentStatus.None
        }).With(
            status: StoryExperimentStatus.Suggested,
            lastSuggestedUtc: nowUtc);

        await _repository.UpsertExperimentProgressAsync(updated);
        _analytics.TrackExperimentSuggested(username, candidate.Code);

        return new StoryExperimentSuggestion
        {
            Definition = candidate,
            AlreadyPlanned = alreadyPlanned
        };
    }

    private static StoryExperimentDefinition? SelectExperimentCandidate(
        IReadOnlyList<StoryExperimentDefinition> definitions,
        IReadOnlyList<StoryExperimentProgress> progressList)
    {
        var completedCodes = new HashSet<string>(
            progressList.Where(p => p.Status == StoryExperimentStatus.Completed)
                .Select(p => p.ExperimentCode),
            StringComparer.OrdinalIgnoreCase);

        var available = definitions
            .Where(def => !completedCodes.Contains(def.Code))
            .Select(def => new
            {
                Definition = def,
                Progress = progressList.FirstOrDefault(p => string.Equals(p.ExperimentCode, def.Code, StringComparison.OrdinalIgnoreCase))
            })
            .ToList();

        if (available.Count == 0) return null;

        var lastSuggested = available
            .Select(item => item.Progress)
            .Where(p => p != null && p.LastSuggestedUtc.HasValue && p.Status != StoryExperimentStatus.Completed)
            .OrderByDescending(p => p!.LastSuggestedUtc)
            .FirstOrDefault();

        var ordered = available
            .OrderBy(item => item.Progress?.LastSuggestedUtc ?? DateTime.MinValue)
            .ToList();

        foreach (var item in ordered)
        {
            if (lastSuggested != null &&
                string.Equals(lastSuggested.ExperimentCode, item.Definition.Code, StringComparison.OrdinalIgnoreCase) &&
                lastSuggested.Status != StoryExperimentStatus.Completed &&
                ordered.Count > 1)
            {
                // Skip repeating the same suggestion when we have alternatives.
                continue;
            }

            return item.Definition;
        }

        // If all options were skipped due to repetition, allow the first one.
        return ordered[0].Definition;
    }

    private StoryExperimentDefinition ResolveExperiment(string experimentCode)
    {
        if (string.IsNullOrWhiteSpace(experimentCode))
        {
            throw new ArgumentException("Experiment code cannot be empty.", nameof(experimentCode));
        }

        foreach (var trigger in StoryGuideCatalogue.GetTriggers())
        {
            var experiments = StoryGuideCatalogue.GetExperiments(trigger.Code);
            var match = experiments.FirstOrDefault(exp =>
                string.Equals(exp.Code, experimentCode, StringComparison.OrdinalIgnoreCase));
            if (match != null) return match;
        }

        throw new ArgumentOutOfRangeException(nameof(experimentCode), $"Unknown experiment code '{experimentCode}'.");
    }

    private StoryExperimentSummary MapSummary(StoryExperimentDefinition definition, StoryExperimentProgress progress)
    {
        return new StoryExperimentSummary
        {
            Definition = definition,
            Status = progress.Status,
            LastSuggestedUtc = progress.LastSuggestedUtc,
            PlannedUtc = progress.PlannedUtc,
            CompletedUtc = progress.CompletedUtc,
            XpGranted = progress.XpGranted,
            XpReward = definition.XpReward
        };
    }

    private static string ValidateUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        }

        return username.Trim();
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }

    private static DateTime StartOfWeek(DateTime dateUtc)
    {
        var offset = ((int)dateUtc.DayOfWeek + 6) % 7;
        return new DateTime(dateUtc.Year, dateUtc.Month, dateUtc.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(-offset);
    }

    private static string NormalizeTriggerCode(string triggerCode)
    {
        return string.IsNullOrWhiteSpace(triggerCode)
            ? StoryGuideCatalogue.CustomTriggerCode
            : triggerCode.Trim().ToLowerInvariant();
    }

    private static bool RequiresCustomText(string triggerCode)
    {
        return string.Equals(triggerCode, StoryGuideCatalogue.CustomTriggerCode, StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildChapterTitle(StoryTriggerDefinition definition)
    {
        return definition.Code switch
        {
            "lonely" => "Your connection chapter",
            "stress" => "Easing the pressure",
            "fatigue" => "Restoring energy",
            "bored" => "Inviting a new spark",
            "celebrate" => "Keeping the glow",
            "restrict" => "Rewriting the rules",
            "social" => "Holding compassionate boundaries",
            _ => "Your story insight"
        };
    }

    private static string BuildChapterBody(StoryTriggerDefinition definition, string? customTrigger)
    {
        if (RequiresCustomText(definition.Code) && !string.IsNullOrWhiteSpace(customTrigger))
        {
            return $"{definition.ChapterIntro} You named \"{customTrigger}\"—that awareness is powerful.";
        }

        return definition.ChapterIntro;
    }
}
