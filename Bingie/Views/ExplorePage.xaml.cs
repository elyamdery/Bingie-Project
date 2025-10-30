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
        _pointsEnabled = FeatureFlags.PointsSystemEnabled;

        if (_pointsEnabled)
        {
            var connectionFactory = new SqliteConnectionFactory();
            var repository = new PointsSystemRepository(connectionFactory);
            _pointsSystemService = new PointsSystemService(repository);
        }

        PointsSection.IsVisible = _pointsEnabled;
        PointsQuestEmptyLabel.IsVisible = false;

        PopulateChallenges();
        ShowRandomQuote();
    }

    public ExplorePage(PointsSystemService pointsSystemService, string username)
    {
        InitializeComponent();
        _pointsEnabled = FeatureFlags.PointsSystemEnabled;
        _pointsSystemService = _pointsEnabled ? pointsSystemService ?? throw new ArgumentNullException(nameof(pointsSystemService)) : null;
        _username = username ?? throw new ArgumentNullException(nameof(username));

        PointsSection.IsVisible = _pointsEnabled;
        PointsQuestEmptyLabel.IsVisible = false;

        PopulateChallenges();
        ShowRandomQuote();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
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

            _suppressPointsToggle = true;
            PointsPauseSwitch.IsToggled = dashboard.RewardsPaused;
            _suppressPointsToggle = false;

            PointsGlowProgress.Progress = Math.Clamp(dashboard.GlowFill, 0, 1);
            PointsGlowLabel.Text = $"Level {dashboard.GlowLevel} · {dashboard.TotalXp} XP";
            PointsWeeklyLabel.Text = $"Last 7 days: {dashboard.WeeklyXp} XP";

            RenderQuestBoard(dashboard);
            RenderCosmetics(dashboard);
        }
        catch (Exception ex)
        {
            PointsSection.IsVisible = false;
            await DisplayAlert("Points system", $"Unable to refresh quests right now: {ex.Message}", "OK");
        }
    }

    private void RenderQuestBoard(PointsDashboard dashboard)
    {
        PointsQuestLayout.Children.Clear();

        if (dashboard.RewardsPaused)
        {
            PointsQuestEmptyLabel.Text = "Rewards are paused. Flip the switch when you’re ready.";
            PointsQuestEmptyLabel.IsVisible = true;
            return;
        }

        if (dashboard.Quests.Count == 0)
        {
            PointsQuestEmptyLabel.Text = "No quests yet—log today to generate a board.";
            PointsQuestEmptyLabel.IsVisible = true;
            return;
        }

        PointsQuestEmptyLabel.IsVisible = false;

        foreach (var quest in dashboard.Quests)
        {
            PointsQuestLayout.Children.Add(CreateQuestView(quest));
        }
    }

    private View CreateQuestView(QuestViewModel quest)
    {
        Frame frame = new()
        {
            Padding = new Thickness(16),
            CornerRadius = 20,
            BackgroundColor = Color.FromArgb("#FFFFFF14"),
            Content = new VerticalStackLayout
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
                        TextColor = Color.FromArgb("#E6F4FF")
                    }
                }
            }
        };

        Button actionButton = new()
        {
            Text = quest.Completed ? "Completed" : $"+{quest.Xp} XP",
            IsEnabled = !quest.Completed,
            CornerRadius = 20,
            BackgroundColor = quest.Completed ? Color.FromArgb("#3A3F73") : Color.FromArgb("#FFB347"),
            TextColor = Colors.White,
            CommandParameter = quest.QuestId
        };
        actionButton.Clicked += OnQuestCompleteClicked;

        ((VerticalStackLayout)frame.Content).Children.Add(actionButton);

        return frame;
    }

    private void RenderCosmetics(PointsDashboard dashboard)
    {
        PointsCosmeticsLayout.Children.Clear();

        foreach (var cosmetic in dashboard.Cosmetics)
        {
            PointsCosmeticsLayout.Children.Add(CreateCosmeticView(cosmetic));
        }
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
