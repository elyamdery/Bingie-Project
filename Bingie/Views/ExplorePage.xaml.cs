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
    private readonly string _username;

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

    public ExplorePage()
    {
        InitializeComponent();
        _username = "PreviewUser";
        var connectionFactory = new SqliteConnectionFactory();
        var storyRepository = new StoryGuideRepository(connectionFactory);
        _storyGuideService = new StoryGuideService(storyRepository, new NoopStoryGuideAnalytics());

        PopulateChallenges();
        ShowRandomQuote();
    }

    public ExplorePage(StoryGuideService storyGuideService, string username)
    {
        InitializeComponent();
        _storyGuideService = FeatureFlags.StoryGuideEnabled
            ? storyGuideService ?? throw new ArgumentNullException(nameof(storyGuideService))
            : null;
        _username = username ?? throw new ArgumentNullException(nameof(username));

        PopulateChallenges();
        ShowRandomQuote();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshStoryGuideAsync();
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
        if (!FeatureFlags.StoryGuideEnabled || _storyGuideService == null)
        {
            StoryGuideSection.IsVisible = false;
            return;
        }

        try
        {
            var summary = await _storyGuideService.GetWeeklyEpisodeAsync(_username, DateTime.UtcNow);
            if (!summary.HasData)
            {
                StoryGuideSection.IsVisible = false;
                return;
            }

            StoryGuideSection.IsVisible = true;
            StoryGuideWeekLabel.Text = $"Week of {summary.WeekStartUtc:MMM d}";

            PopulateTriggers(summary);
            PopulateExperiments(summary);
        }
        catch (Exception ex)
        {
            StoryGuideSection.IsVisible = false;
            await DisplayAlert("Story guide", $"Couldn't load your story insights: {ex.Message}", "OK");
        }
    }

    private void PopulateTriggers(StoryWeeklyEpisode summary)
    {
        StoryGuideTriggersLayout.Children.Clear();

        foreach (var trigger in summary.TriggerCounts.Take(3))
        {
            HorizontalStackLayout row = new()
            {
                Spacing = 8
            };
            var bullet = new Ellipse
            {
                WidthRequest = 10,
                HeightRequest = 10,
                Fill = new SolidColorBrush(Color.FromArgb("#7C5CFA"))
            };
            row.Children.Add(bullet);
            row.Children.Add(new Label
            {
                Text = $"{trigger.Definition.Title}: {trigger.Count}",
                TextColor = Color.FromArgb("#E6F4FF"),
                FontSize = 14
            });

            StoryGuideTriggersLayout.Children.Add(row);
        }

        if (!summary.TriggerCounts.Any())
        {
            StoryGuideTriggersLayout.Children.Add(new Label
            {
                Text = "No triggers logged yet—log your next check-in to unlock a story beat.",
                TextColor = Color.FromArgb("#E6F4FF"),
                FontSize = 14
            });
        }
    }

    private void PopulateExperiments(StoryWeeklyEpisode summary)
    {
        StoryGuideExperimentsLayout.Children.Clear();

        foreach (var experiment in summary.Experiments.Take(2))
        {
            StoryGuideExperimentsLayout.Children.Add(CreateExperimentCard(experiment));
        }

        if (!summary.Experiments.Any())
        {
            StoryGuideExperimentsLayout.Children.Add(new Label
            {
                Text = "Complete a trigger entry to receive tailored experiments.",
                TextColor = Color.FromArgb("#F2F8FF"),
                FontSize = 14
            });
        }
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
}
