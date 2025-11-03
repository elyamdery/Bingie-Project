using System;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Config;
using Bingie.Models;
using Bingie.Services;

namespace Bingie.Views;

public partial class StoryGuidePromptPage : ContentPage
{
    private readonly StoryGuideService _storyGuideService;
    private readonly string _username;
    private readonly DateTime _entryDateUtc;

    private StoryTriggerDefinition? _selectedTrigger;
    private StoryExperimentSuggestion? _suggestedExperiment;
    private bool _hasSaved;

    public StoryGuidePromptPage(StoryGuideService storyGuideService, string username, DateTime entryDateUtc)
    {
        InitializeComponent();

        if (!FeatureFlags.StoryGuideEnabled)
        {
            throw new InvalidOperationException("Story guide feature flag is disabled.");
        }

        _storyGuideService = storyGuideService ?? throw new ArgumentNullException(nameof(storyGuideService));
        _username = string.IsNullOrWhiteSpace(username) ? throw new ArgumentException("Username cannot be empty.", nameof(username)) : username.Trim();
        _entryDateUtc = entryDateUtc;

        TriggerCollection.ItemsSource = _storyGuideService.GetTriggers();
    }

    private void OnTriggerSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedTrigger = e.CurrentSelection.FirstOrDefault() as StoryTriggerDefinition;
        CustomReasonPanel.IsVisible = _selectedTrigger != null && string.Equals(_selectedTrigger.Code, StoryGuideCatalogue.CustomTriggerCode, StringComparison.OrdinalIgnoreCase);
        SaveButton.IsEnabled = _selectedTrigger != null;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_hasSaved) return;
        if (_selectedTrigger == null) return;

        try
        {
            SaveButton.IsEnabled = false;
            var custom = CustomReasonPanel.IsVisible ? CustomReasonEditor.Text?.Trim() : null;

            var result = await _storyGuideService.RecordTriggerSelectionAsync(
                _username,
                _entryDateUtc,
                _selectedTrigger.Code,
                custom,
                DateTime.UtcNow);

            _hasSaved = true;
            PresentChapter(result);
        }
        catch (Exception ex)
        {
            SaveButton.IsEnabled = true;
            await DisplayAlert("Oops", $"We couldn't save that insight: {ex.Message}", "OK");
        }
    }

    private async void OnSkipClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void OnPlanExperimentClicked(object sender, EventArgs e)
    {
        if (_suggestedExperiment == null) return;

        try
        {
            PlanExperimentButton.IsEnabled = false;
            _ = await _storyGuideService.PlanExperimentAsync(_username, _suggestedExperiment.Definition.Code, DateTime.UtcNow);
            await DisplayAlert("Great choice", "Experiment added to your story plan.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Oops", $"Couldn't plan that experiment: {ex.Message}", "OK");
            PlanExperimentButton.IsEnabled = true;
        }
    }

    private async void OnDoneClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void PresentChapter(StoryChapterResult result)
    {
        ChapterTitleLabel.Text = result.ChapterTitle;
        ChapterBodyLabel.Text = result.ChapterBody;
        ChapterCard.IsVisible = true;
        DoneButton.IsVisible = true;

        if (result.Experiment is { } suggestion)
        {
            _suggestedExperiment = suggestion;
            ExperimentTitleLabel.Text = suggestion.Definition.Title;
            ExperimentPromptLabel.Text = suggestion.Definition.Prompt;
            ExperimentRewardLabel.Text = $"Reward: +{suggestion.Definition.XpReward} XP";
            PlanExperimentButton.IsEnabled = !suggestion.AlreadyPlanned;
            PlanExperimentButton.Text = suggestion.AlreadyPlanned ? "Already planned" : "I'll try this";
            ExperimentCard.IsVisible = true;
        }
        else
        {
            ExperimentCard.IsVisible = false;
        }
    }
}
