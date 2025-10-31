using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Config;
using Bingie.Models;
using Bingie.Services;
using Microsoft.Maui.Controls.Shapes;

namespace Bingie.Views;

public partial class ExplorePage : ContentPage
{
    private readonly StoryGuideService? _storyGuideService;
    private readonly PointsSystemService? _pointsSystemService;
    private readonly string _username;
    private readonly bool _pointsEnabled;

    private readonly Random _random = new();
    private readonly IReadOnlyList<string> _microChallenges = new List<string>
    {
        "Sip a glass of water before your next craving check-in.",
        "Text a friend one encouraging sentence.",
        "Write down three feelings you notice right now.",
        "Step outside for five breaths of fresh air.",
        "Swap one negative thought for a curious question.",
        "Play your favorite song and stretch for 60 seconds.",
        "Plan a nourishing snack you’re excited about.",
        "List two wins from this week—no matter how small.",
        "Take a mindful bite: notice flavor, texture, and gratitude.",
        "Repeat a mantra: “I deserve care and calm.”"
    };

    private bool _suppressPointsToggle;

    public ExplorePage()
    {
        InitializeComponent();
        _username = "PreviewUser";

        var connectionFactory = new SqliteConnectionFactory();
        if (FeatureFlags.StoryGuideEnabled)
        {
            var storyRepository = new StoryGuideRepository(connectionFactory);
            _storyGuideService = new StoryGuideService(storyRepository, new NoopStoryGuideAnalytics());
        }

        _pointsEnabled = FeatureFlags.PointsSystemEnabled;
        if (_pointsEnabled)
        {
            var pointsRepository = new PointsSystemRepository(connectionFactory);
            _pointsSystemService = new PointsSystemService(pointsRepository);
        }

        StoryGuideSection.IsVisible = false;
        PointsSection.IsVisible = _pointsEnabled;
        PointsQuestEmptyLabel.IsVisible = false;

        PopulateChallenges();
        ShowRandomQuote();
    }

    public ExplorePage(StoryGuideService storyGuideService, PointsSystemService pointsSystemService, string username)
    {
        InitializeComponent();
        _storyGuideService = FeatureFlags.StoryGuideEnabled
            ? storyGuideService ?? throw new ArgumentNullException(nameof(storyGuideService))
            : null;
        _pointsEnabled = FeatureFlags.PointsSystemEnabled;
        _pointsSystemService = _pointsEnabled
            ? pointsSystemService ?? throw new ArgumentNullException(nameof(pointsSystemService))
            : null;
        _username = username ?? throw new ArgumentNullException(nameof(username));

        StoryGuideSection.IsVisible = false;
        PointsSection.IsVisible = _pointsEnabled;
        PointsQuestEmptyLabel.IsVisible = false;

        PopulateChallenges();
        ShowRandomQuote();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshStoryGuideAsync();
        await RefreshPointsAsync();
    }

    private void PopulateChallenges()
    {
        ChallengesLayout.Children.Clear();

        foreach (var challenge in _microChallenges.OrderBy(_ => _random.Next()).Take(6))
        {
            ChallengesLayout.Children.Add(CreateChallengeCard(challenge));
        }
    }

    private View CreateChallengeCard(string challenge)
    {
        Border card = new()
        {
            BackgroundColor = Color.FromArgb("#FFFFFF"),
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(18)
            },
            WidthRequest = 150,
            Margin = new Thickness(6),
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Color.FromArgb("#1A000000")),
                Radius = 12,
                Offset = new Point(0, 6),
                Opacity = 0.3f
            },
            Content = new VerticalStackLayout
            {
                Padding = new Thickness(14),
                Spacing = 6,
                Children =
                {
                    new Label
                    {
                        Text = challenge,
                        TextColor = Color.FromArgb("#223355"),
                        FontSize = 14
                    }
                }
            }
        };

        card.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () =>
            {
                await card.ScaleTo(0.94, 90, Easing.CubicOut);
                await card.ScaleTo(1, 120, Easing.CubicIn);
            })
        });

        return card;
    }

    private void ShowRandomQuote()
    {
        var quote = QuotesService.GetRandomQuote(_random);
        QuoteTextLabel.Text = $"“{quote.Text}”";
        QuoteAuthorLabel.Text = $"- {quote.Author}";
        _ = QuoteCard.ScaleTo(1.02, 110, Easing.CubicOut);
        _ = QuoteCard.ScaleTo(1, 150, Easing.CubicIn);
    }

    private void OnShuffleQuoteClicked(object sender, EventArgs e)
    {
        ShowRandomQuote();
    }

    private async Task RefreshStoryGuideAsync()
    {
        if (_storyGuideService == null)
        {
            StoryGuideSection.IsVisible = false;
            return;
        }

        try
        {
            var episode = await _storyGuideService.GetWeeklyEpisodeAsync(_username, DateTime.UtcNow);
            if (episode == null)
            {
                StoryGuideSection.IsVisible = false;
                return;
            }

            StoryGuideSection.IsVisible = true;
            StoryGuideWeekLabel.Text = $"Week of {episode.WeekStartUtc:MMM d}";

            StoryGuideTriggersLayout.Children.Clear();
            foreach (var trigger in episode.TriggerCounts.Take(3))
            {
                StoryGuideTriggersLayout.Children.Add(CreateTriggerChip(trigger));
            }

            StoryGuideExperimentsLayout.Children.Clear();
            foreach (var experiment in episode.Experiments)
            {
                StoryGuideExperimentsLayout.Children.Add(CreateExperimentCard(experiment));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Story guide", $"Unable to load weekly story: {ex.Message}", "OK");
            StoryGuideSection.IsVisible = false;
        }
    }

    private View CreateTriggerChip(StoryTriggerCount trigger)
    {
        Border chip = new()
        {
            BackgroundColor = Color.FromArgb("#2B3F78"),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18) },
            Padding = new Thickness(12, 6),
            Content = new Label
            {
                Text = $"{trigger.Definition.Title} · {trigger.Count}",
                TextColor = Colors.White,
                FontSize = 14
            }
        };
        return chip;
    }

    private View CreateExperimentCard(StoryExperimentSummary summary)
    {
        Border card = new()
        {
            BackgroundColor = Color.FromArgb("#1F2859"),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18) },
            Padding = new Thickness(16),
            Content = new VerticalStackLayout
            {
                Spacing = 6,
                Children =
                {
                    new Label
                    {
                        Text = summary.Definition.Title,
                        FontSize = 18,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Colors.White
                    },
                    new Label
                    {
                        Text = summary.Definition.Prompt,
                        FontSize = 14,
                        TextColor = Color.FromArgb("#D8E0FF")
                    },
                    new Label
                    {
                        Text = summary.XpGranted ? "XP awarded" : $"+{summary.XpReward} XP available",
                        FontSize = 12,
                        TextColor = Color.FromArgb("#C5CBFF")
                    }
                }
            }
        };

        var actions = new HorizontalStackLayout
        {
            Spacing = 10
        };

        if (summary.Status < StoryExperimentStatus.Planned)
        {
            actions.Children.Add(CreateActionButton("Plan", summary.Definition.Code, OnPlanExperimentClicked));
        }
        else if (summary.Status < StoryExperimentStatus.Completed)
        {
            actions.Children.Add(CreateActionButton("Mark completed", summary.Definition.Code, OnCompleteExperimentClicked));
        }
        else
        {
            actions.Children.Add(new Label
            {
                Text = summary.XpGranted ? "Completed · XP granted" : "Completed",
                FontSize = 12,
                TextColor = Color.FromArgb("#A6B1FF")
            });
        }

        ((VerticalStackLayout)card.Content).Children.Add(actions);

        return card;
    }

    private Button CreateActionButton(string text, string experimentCode, EventHandler handler)
    {
        Button button = new()
        {
            Text = text,
            CornerRadius = 20,
            BackgroundColor = Color.FromArgb("#7C5CFA"),
            TextColor = Colors.White,
            CommandParameter = experimentCode
        };
        button.Clicked += handler;
        return button;
    }

    private async void OnPlanExperimentClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not string code) return;
        if (_storyGuideService == null) return;

        button.IsEnabled = false;
        try
        {
            _ = await _storyGuideService.PlanExperimentAsync(_username, code, DateTime.UtcNow);
            await DisplayAlert("Story guide", "Experiment planned. You've got this!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Story guide", $"Could not plan experiment: {ex.Message}", "OK");
        }
        finally
        {
            await RefreshStoryGuideAsync();
        }
    }

    private async void OnCompleteExperimentClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not string code) return;
        if (_storyGuideService == null) return;

        button.IsEnabled = false;
        try
        {
            var result = await _storyGuideService.CompleteExperimentAsync(_username, code, DateTime.UtcNow);
            var message = result.XpAwarded > 0
                ? $"Nice work! You earned {result.XpAwarded} XP."
                : "Experiment marked complete.";
            await DisplayAlert("Story guide", message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Story guide", $"Could not complete experiment: {ex.Message}", "OK");
        }
        finally
        {
            await RefreshStoryGuideAsync();
        }
    }

    private async Task RefreshPointsAsync()
    {
        if (!_pointsEnabled || _pointsSystemService == null)
        {
            PointsSection.IsVisible = false;
            return;
        }

        try
        {
            var dashboard = await _pointsSystemService.GetDashboardAsync(_username, DateTime.UtcNow);
            PointsSection.IsVisible = true;
            RenderPointsDashboard(dashboard);
        }
        catch (Exception ex)
        {
            PointsSection.IsVisible = false;
            await DisplayAlert("Points system", $"Unable to load XP dashboard: {ex.Message}", "OK");
        }
    }

    private void RenderPointsDashboard(PointsDashboard dashboard)
    {
        _suppressPointsToggle = true;
        PointsPauseSwitch.IsToggled = dashboard.RewardsPaused;
        _suppressPointsToggle = false;

        PointsGlowProgress.Progress = Math.Clamp(dashboard.GlowFill, 0, 1);
        PointsGlowLabel.Text = $"Level {dashboard.GlowLevel} · {dashboard.TotalXp} XP";
        PointsWeeklyLabel.Text = $"This week: {dashboard.WeeklyXp} XP";

        PointsQuestLayout.Children.Clear();
        if (dashboard.Quests.Count == 0)
        {
            PointsQuestEmptyLabel.IsVisible = true;
            PointsQuestEmptyLabel.Text = "New quests arrive each morning.";
        }
        else
        {
            PointsQuestEmptyLabel.IsVisible = false;
            foreach (var quest in dashboard.Quests)
            {
                PointsQuestLayout.Children.Add(BuildQuestView(quest));
            }
        }

        PointsCosmeticsLayout.Children.Clear();
        foreach (var cosmetic in dashboard.Cosmetics)
        {
            PointsCosmeticsLayout.Children.Add(CreateCosmeticView(cosmetic));
        }
    }

    private View BuildQuestView(QuestViewModel quest)
    {
        Frame frame = new()
        {
            Padding = new Thickness(16),
            CornerRadius = 18,
            BackgroundColor = Color.FromArgb("#1F2859")
        };

        VerticalStackLayout layout = new()
        {
            Spacing = 6,
            Children =
            {
                new Label
                {
                    Text = quest.Title,
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White
                },
                new Label
                {
                    Text = quest.Description,
                    FontSize = 14,
                    TextColor = Color.FromArgb("#D8E0FF")
                },
                new Label
                {
                    Text = $"+{quest.Xp} XP",
                    FontSize = 12,
                    TextColor = Color.FromArgb("#B8C2FF")
                }
            }
        };

        if (!quest.Completed)
        {
            Button completeButton = new()
            {
                Text = "Complete",
                CornerRadius = 18,
                BackgroundColor = Color.FromArgb("#7C5CFA"),
                TextColor = Colors.White,
                CommandParameter = quest.QuestId
            };
            completeButton.Clicked += OnQuestCompleteClicked;
            layout.Children.Add(completeButton);
        }
        else
        {
            layout.Children.Add(new Label
            {
                Text = "Completed",
                FontSize = 12,
                TextColor = Color.FromArgb("#9CF6FF")
            });
        }

        frame.Content = layout;
        return frame;
    }

    private View CreateCosmeticView(CosmeticStatus status)
    {
        Frame frame = new()
        {
            Padding = new Thickness(16),
            CornerRadius = 18,
            BackgroundColor = Color.FromArgb("#1F2859")
        };

        VerticalStackLayout layout = new()
        {
            Spacing = 6,
            Children =
            {
                new Label
                {
                    Text = status.Reward.Name,
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White
                },
                new Label
                {
                    Text = status.Reward.Description,
                    FontSize = 14,
                    TextColor = Color.FromArgb("#D8E0FF")
                }
            }
        };

        if (!status.IsUnlocked)
        {
            layout.Children.Add(new Label
            {
                Text = $"Unlock at {status.Reward.RequiredXp} XP",
                FontSize = 12,
                TextColor = Color.FromArgb("#B8C2FF")
            });
        }
        else if (status.IsEquipped)
        {
            layout.Children.Add(new Label
            {
                Text = "Equipped",
                FontSize = 12,
                TextColor = Color.FromArgb("#9CF6FF")
            });
        }
        else if (status.CanEquip)
        {
            Button equipButton = new()
            {
                Text = "Equip",
                CornerRadius = 18,
                BackgroundColor = Color.FromArgb("#7C5CFA"),
                TextColor = Colors.White,
                CommandParameter = status.Reward.CosmeticCode
            };
            equipButton.Clicked += OnEquipCosmeticClicked;
            layout.Children.Add(equipButton);
        }

        frame.Content = layout;
        return frame;
    }

    private async void OnPointsPauseToggled(object? sender, ToggledEventArgs e)
    {
        if (_suppressPointsToggle) return;
        if (_pointsSystemService == null) return;

        try
        {
            await _pointsSystemService.ToggleRewardsAsync(_username, e.Value, DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Points system", $"Unable to update rewards setting: {ex.Message}", "OK");
        }
        finally
        {
            await RefreshPointsAsync();
        }
    }

    private async void OnQuestCompleteClicked(object? sender, EventArgs e)
    {
        if (_pointsSystemService == null) return;
        if (sender is not Button button || button.CommandParameter is not long questId) return;

        button.IsEnabled = false;
        try
        {
            await _pointsSystemService.CompleteQuestAsync(_username, questId, DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Points system", $"Unable to complete quest: {ex.Message}", "OK");
        }
        finally
        {
            await RefreshPointsAsync();
        }
    }

    private async void OnEquipCosmeticClicked(object? sender, EventArgs e)
    {
        if (_pointsSystemService == null) return;
        if (sender is not Button button || button.CommandParameter is not string code) return;

        button.IsEnabled = false;
        try
        {
            await _pointsSystemService.EquipCosmeticAsync(_username, code, DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Points system", $"Unable to equip cosmetic: {ex.Message}", "OK");
        }
        finally
        {
            await RefreshPointsAsync();
        }
    }
}
