using Bingie.Models;
using Bingie.Services;

namespace Bingie.Tests;

public class StoryGuideServiceTests
{
    private static readonly DateTime ReferenceDate = new(2025, 2, 17, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task RecordTriggerSelectionAsync_SavesSelectionAndSuggestsExperiment()
    {
        InMemoryStoryGuideRepository repository = new();
        RecordingStoryGuideAnalytics analytics = new();
        StoryGuideService service = new(repository, analytics);

        var result = await service.RecordTriggerSelectionAsync("alex", ReferenceDate, "stress", null, ReferenceDate);

        Assert.Equal("stress", result.Trigger.Code);
        Assert.NotNull(result.Experiment);

        var selection = await repository.GetTriggerSelectionAsync("alex", ReferenceDate);
        Assert.NotNull(selection);
        Assert.Equal("stress", selection!.TriggerCode);
        Assert.Contains("stress", analytics.TriggerEvents);
        Assert.Contains(result.Experiment!.Definition.Code, analytics.SuggestedEvents);
    }

    [Fact]
    public async Task RecordTriggerSelectionAsync_RotatesExperimentsBeforeRepeating()
    {
        InMemoryStoryGuideRepository repository = new();
        StoryGuideService service = new(repository, new RecordingStoryGuideAnalytics());

        var first = await service.RecordTriggerSelectionAsync("alex", ReferenceDate, "stress", null, ReferenceDate);
        var second = await service.RecordTriggerSelectionAsync("alex", ReferenceDate.AddMinutes(5), "stress", null, ReferenceDate.AddMinutes(5));

        Assert.NotEqual(first.Experiment?.Definition.Code, second.Experiment?.Definition.Code);
    }

    [Fact]
    public async Task CompletingExperiment_AwardsXpOnlyOnce()
    {
        InMemoryStoryGuideRepository repository = new();
        StoryGuideService service = new(repository, new RecordingStoryGuideAnalytics());

        var chapter = await service.RecordTriggerSelectionAsync("alex", ReferenceDate, "lonely", null, ReferenceDate);
        var experimentCode = chapter.Experiment!.Definition.Code;

        _ = await service.PlanExperimentAsync("alex", experimentCode, ReferenceDate.AddMinutes(30));
        var firstCompletion = await service.CompleteExperimentAsync("alex", experimentCode, ReferenceDate.AddHours(2));
        var secondCompletion = await service.CompleteExperimentAsync("alex", experimentCode, ReferenceDate.AddHours(3));

        Assert.True(firstCompletion.XpAwarded > 0);
        Assert.Equal(0, secondCompletion.XpAwarded);
    }

    [Fact]
    public async Task GetWeeklyEpisodeAsync_IncludesTriggerCountsAndExperiments()
    {
        InMemoryStoryGuideRepository repository = new();
        StoryGuideService service = new(repository, new RecordingStoryGuideAnalytics());

        await service.RecordTriggerSelectionAsync("alex", ReferenceDate, "stress", null, ReferenceDate);
        await service.RecordTriggerSelectionAsync("alex", ReferenceDate.AddDays(1), "stress", null, ReferenceDate.AddDays(1));
        await service.RecordTriggerSelectionAsync("alex", ReferenceDate.AddDays(2), "lonely", null, ReferenceDate.AddDays(2));

        var summary = await service.GetWeeklyEpisodeAsync("alex", ReferenceDate.AddDays(3));

        Assert.True(summary.HasData);
        Assert.Contains(summary.TriggerCounts, count => count.Definition.Code == "stress" && count.Count == 2);
    }

    private sealed class InMemoryStoryGuideRepository : IStoryGuideRepository
    {
        private readonly Dictionary<(string User, DateTime EntryDate), StoryTriggerSelection> _selections = new();
        private readonly Dictionary<(string User, string Experiment), StoryExperimentProgress> _experiments = new();

        public Task UpsertTriggerSelectionAsync(StoryTriggerSelection selection)
        {
            _selections[(selection.Username, selection.EntryDateUtc)] = Clone(selection);
            return Task.CompletedTask;
        }

        public Task<StoryTriggerSelection?> GetTriggerSelectionAsync(string username, DateTime entryDateUtc)
        {
            _selections.TryGetValue((username, entryDateUtc), out var selection);
            return Task.FromResult(selection is null ? null : Clone(selection));
        }

        public Task<IReadOnlyList<StoryTriggerSelection>> GetTriggerSelectionsAsync(string username, DateTime fromUtc, DateTime toUtc)
        {
            var items = _selections.Values
                .Where(s => string.Equals(s.Username, username, StringComparison.OrdinalIgnoreCase)
                            && s.EntryDateUtc >= fromUtc && s.EntryDateUtc < toUtc)
                .Select(Clone)
                .OrderBy(s => s.EntryDateUtc)
                .ToList();
            return Task.FromResult<IReadOnlyList<StoryTriggerSelection>>(items);
        }

        public Task<StoryExperimentProgress?> GetExperimentProgressAsync(string username, string experimentCode)
        {
            _experiments.TryGetValue((username, experimentCode), out var progress);
            return Task.FromResult(progress is null ? null : Clone(progress));
        }

        public Task<IReadOnlyList<StoryExperimentProgress>> GetExperimentProgressAsync(string username)
        {
            var items = _experiments.Values
                .Where(e => string.Equals(e.Username, username, StringComparison.OrdinalIgnoreCase))
                .Select(Clone)
                .ToList();
            return Task.FromResult<IReadOnlyList<StoryExperimentProgress>>(items);
        }

        public Task UpsertExperimentProgressAsync(StoryExperimentProgress progress)
        {
            _experiments[(progress.Username, progress.ExperimentCode)] = Clone(progress);
            return Task.CompletedTask;
        }

        private static StoryTriggerSelection Clone(StoryTriggerSelection selection)
        {
            return new StoryTriggerSelection
            {
                Id = selection.Id,
                Username = selection.Username,
                TriggerCode = selection.TriggerCode,
                CustomTrigger = selection.CustomTrigger,
                EntryDateUtc = selection.EntryDateUtc,
                CreatedUtc = selection.CreatedUtc
            };
        }

        private static StoryExperimentProgress Clone(StoryExperimentProgress progress)
        {
            return new StoryExperimentProgress
            {
                Id = progress.Id,
                Username = progress.Username,
                ExperimentCode = progress.ExperimentCode,
                Status = progress.Status,
                LastSuggestedUtc = progress.LastSuggestedUtc,
                PlannedUtc = progress.PlannedUtc,
                CompletedUtc = progress.CompletedUtc,
                XpGranted = progress.XpGranted
            };
        }
    }

    private sealed class RecordingStoryGuideAnalytics : IStoryGuideAnalytics
    {
        public List<string> TriggerEvents { get; } = new();
        public List<string> SuggestedEvents { get; } = new();
        public List<string> PlannedEvents { get; } = new();
        public List<(string Experiment, int Xp)> CompletedEvents { get; } = new();

        public void TrackTriggerLogged(string username, string triggerCode)
        {
            TriggerEvents.Add(triggerCode);
        }

        public void TrackExperimentSuggested(string username, string experimentCode)
        {
            SuggestedEvents.Add(experimentCode);
        }

        public void TrackExperimentPlanned(string username, string experimentCode)
        {
            PlannedEvents.Add(experimentCode);
        }

        public void TrackExperimentCompleted(string username, string experimentCode, int xpAwarded)
        {
            CompletedEvents.Add((experimentCode, xpAwarded));
        }
    }
}
