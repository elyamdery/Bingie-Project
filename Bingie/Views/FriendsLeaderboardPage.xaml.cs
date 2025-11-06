using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bingie.Models;
using Bingie.Services;
using Microsoft.Maui.Controls;

namespace Bingie.Views;

public partial class FriendsLeaderboardPage : ContentPage
{
    private readonly FriendsLeaderboardService _leaderboardService;
    private readonly string _username;
    private bool _isRefreshing;
    private bool _suppressShareEvents;
    private FriendsLeaderboardState? _latestState;

    public FriendsLeaderboardPage(FriendsLeaderboardService leaderboardService, string username)
    {
        ArgumentNullException.ThrowIfNull(leaderboardService);
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        InitializeComponent();
        _leaderboardService = leaderboardService;
        _username = username.Trim();

        MemberPicker.ItemDisplayBinding = new Binding(nameof(LeaderboardEntry.Nickname));
        SupportTemplatePicker.ItemDisplayBinding = new Binding(nameof(SupportTokenTemplate.Title));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        if (_isRefreshing) return;
        _isRefreshing = true;

        try
        {
            var state = await _leaderboardService.GetStateAsync(_username, DateTime.UtcNow);
            _latestState = state;

            FeatureDisabledCard.IsVisible = !_leaderboardService.IsEnabled;
            OptInForm.IsVisible = _leaderboardService.IsEnabled && !state.OptedIn;
            CircleSection.IsVisible = _leaderboardService.IsEnabled && state.OptedIn;

            SupportTemplatePicker.ItemsSource = state.Templates.ToList();

            if (!_leaderboardService.IsEnabled)
            {
                return;
            }

            if (!state.OptedIn)
            {
                return;
            }

            if (state.Circle != null)
            {
                CircleNameLabel.Text = state.Circle.Name;
                CircleInviteLabel.Text = $"Invite code: {state.Circle.InviteCode}";
                CircleMembersLabel.Text =
                    $"{state.Circle.MemberCount}/{state.Circle.MemberCapacity} members";
                CircleMembersLabel.IsVisible = true;
            }
            else
            {
                CircleMembersLabel.IsVisible = false;
            }

            WeekRangeLabel.Text = $"{state.WeekStartUtc:MMM d} – {state.WeekStartUtc.AddDays(6):MMM d}";

            if (state.Membership != null)
            {
                _suppressShareEvents = true;
                ShareXpSwitch.IsToggled = state.Membership.ShareXp;
                ShareStreakSwitch.IsToggled = state.Membership.ShareStreak;
                ShareCopingSwitch.IsToggled = state.Membership.ShareCopingCount;
                MuteCircleSwitch.IsToggled = state.Membership.Muted;
                _suppressShareEvents = false;
            }

            LeaderboardList.ItemsSource = state.Entries;
            EmptyLeaderboardLabel.IsVisible = state.Entries.Count == 0;

            MemberPicker.ItemsSource = state.Entries.Where(entry => !entry.IsSelf).ToList();

            var feed = BuildSupportFeed(state);
            SupportFeedList.ItemsSource = feed;
            SupportFeedList.IsVisible = feed.Any();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "We couldn't refresh the leaderboard. Please try again.", "OK");
            System.Diagnostics.Debug.WriteLine($"Leaderboard refresh failed: {ex}");
        }
        finally
        {
            _isRefreshing = false;
        }
    }

    private async void OnOptInClicked(object sender, EventArgs e)
    {
        try
        {
            var nickname = OptInNicknameEntry.Text?.Trim() ?? string.Empty;
            var shareXp = OptInShareXpSwitch.IsToggled;
            var shareStreak = OptInShareStreakSwitch.IsToggled;
            var shareCoping = OptInShareCopingSwitch.IsToggled;
            var inviteCode = string.IsNullOrWhiteSpace(OptInInviteCodeEntry.Text)
                ? null
                : OptInInviteCodeEntry.Text.Trim();
            var circleName = string.IsNullOrWhiteSpace(inviteCode)
                ? OptInCircleNameEntry.Text
                : null;

            await _leaderboardService.OptInAsync(
                _username,
                nickname,
                shareXp,
                shareStreak,
                shareCoping,
                inviteCode,
                circleName,
                DateTime.UtcNow);

            await DisplayAlert("Joined", "You're ready to support each other. Refreshing the board now.", "Great");
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Unable to join", ex.Message, "OK");
        }
    }

    private async void OnSharePreferenceToggled(object sender, ToggledEventArgs e)
    {
        if (_suppressShareEvents || _latestState?.Membership == null) return;

        try
        {
            await _leaderboardService.UpdateSharingAsync(
                _username,
                ShareXpSwitch.IsToggled,
                ShareStreakSwitch.IsToggled,
                ShareCopingSwitch.IsToggled,
                DateTime.UtcNow);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Update failed", ex.Message, "OK");
            _suppressShareEvents = true;
            ShareXpSwitch.IsToggled = _latestState.Membership.ShareXp;
            ShareStreakSwitch.IsToggled = _latestState.Membership.ShareStreak;
            ShareCopingSwitch.IsToggled = _latestState.Membership.ShareCopingCount;
            _suppressShareEvents = false;
        }
    }

    private async void OnMuteToggled(object sender, ToggledEventArgs e)
    {
        if (_suppressShareEvents || _latestState?.Membership == null) return;

        try
        {
            await _leaderboardService.ToggleMuteAsync(_username, e.Value, DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Update failed", ex.Message, "OK");
            _suppressShareEvents = true;
            MuteCircleSwitch.IsToggled = !e.Value;
            _suppressShareEvents = false;
        }
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await RefreshAsync();
    }

    private async void OnSendSupportClicked(object sender, EventArgs e)
    {
        if (MemberPicker.SelectedItem is not LeaderboardEntry entry)
        {
            await DisplayAlert("Pick someone", "Choose a teammate to uplift.", "OK");
            return;
        }

        if (SupportTemplatePicker.SelectedItem is not SupportTokenTemplate template)
        {
            await DisplayAlert("Choose a token", "Select a token message before sending.", "OK");
            return;
        }

        try
        {
            await _leaderboardService.SendSupportTokenAsync(
                _username,
                entry.Username,
                template.Code,
                DateTime.UtcNow);
            await RefreshAsync();
            MemberPicker.SelectedItem = null;
            SupportTemplatePicker.SelectedItem = null;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Send failed", ex.Message, "OK");
        }
    }

    private async void OnLeaveCircleClicked(object sender, EventArgs e)
    {
        var confirm = await DisplayAlert("Leave circle?", "Leaving will remove you from the leaderboard until you join again. Continue?", "Leave", "Cancel");
        if (!confirm) return;

        try
        {
            await _leaderboardService.LeaveAsync(_username);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Leave failed", ex.Message, "OK");
        }
    }

    private static IEnumerable<SupportTokenFeedItem> BuildSupportFeed(FriendsLeaderboardState state)
    {
        Dictionary<string, string> nicknameLookup = state.Entries
            .GroupBy(entry => entry.Username, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.First().Nickname, StringComparer.OrdinalIgnoreCase);

        return state.SupportTokens
            .OrderByDescending(token => token.CreatedUtc)
            .Select(token =>
            {
                nicknameLookup.TryGetValue(token.FromUsername, out var fromNickname);
                nicknameLookup.TryGetValue(token.ToUsername, out var toNickname);
                var metadata = $"{fromNickname ?? token.FromUsername} → {toNickname ?? token.ToUsername} · {token.CreatedUtc:MMM d HH:mm}";
                return new SupportTokenFeedItem(token.Message, metadata);
            })
            .ToList();
    }

    private sealed record SupportTokenFeedItem(string Message, string Metadata);
}
