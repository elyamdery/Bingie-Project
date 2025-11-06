using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Bingie.Config;
using Bingie.Models;

namespace Bingie.Services;

/// <summary>
/// Coordinates circle membership, privacy preferences, weekly rankings, and support tokens.
/// </summary>
public sealed class FriendsLeaderboardService
{
    public const int CircleMemberCapacity = 15;

    private static readonly char[] InviteAlphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();
    private static readonly SupportTokenTemplate[] TokenTemplates =
    {
        new()
        {
            Code = "shine",
            Title = "Celebrate",
            Body = "Your glow lifted me today—thank you for showing up with heart."
        },
        new()
        {
            Code = "steady",
            Title = "Steady presence",
            Body = "I appreciate your steady check-ins. You remind me that slow is still progress."
        },
        new()
        {
            Code = "check_in",
            Title = "Gentle check-in",
            Body = "I noticed things have been quieter. No pressure, just here if you want to share."
        }
    };

    private readonly IFriendsLeaderboardRepository _repository;
    private readonly IPointsSystemRepository _pointsRepository;
    private readonly IDataStore<BingeEntry> _bingeStore;

    public FriendsLeaderboardService(
        IFriendsLeaderboardRepository repository,
        IPointsSystemRepository pointsRepository,
        IDataStore<BingeEntry> bingeStore)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _pointsRepository = pointsRepository ?? throw new ArgumentNullException(nameof(pointsRepository));
        _bingeStore = bingeStore ?? throw new ArgumentNullException(nameof(bingeStore));
    }

    public bool IsEnabled => FeatureFlags.LeaderboardEnabled;

    public IReadOnlyList<SupportTokenTemplate> GetSupportTemplates() => TokenTemplates;

    public async Task<FriendsLeaderboardState> GetStateAsync(string username, DateTime nowUtc)
    {
        var normalizedUser = NormalizeUsername(username);
        var weekStart = GetWeekStart(nowUtc);

        if (!IsEnabled)
        {
            return new FriendsLeaderboardState
            {
                OptedIn = false,
                WeekStartUtc = weekStart,
                Templates = TokenTemplates
            };
        }

        var membership = await _repository.GetMembershipAsync(normalizedUser);
        if (membership == null)
        {
            return new FriendsLeaderboardState
            {
                OptedIn = false,
                WeekStartUtc = weekStart,
                Templates = TokenTemplates
            };
        }

        var circle = await _repository.GetCircleByIdAsync(membership.CircleId);
        if (circle == null)
        {
            return new FriendsLeaderboardState
            {
                OptedIn = false,
                WeekStartUtc = weekStart,
                Templates = TokenTemplates
            };
        }

        await RefreshLocalSnapshotAsync(normalizedUser, nowUtc, membership);

        var members = await _repository.GetMembershipsForCircleAsync(circle.Id);
        var snapshots = await _repository.GetSnapshotsForWeekAsync(circle.Id, weekStart);
        var tokens = await _repository.GetSupportTokensForWeekAsync(circle.Id, weekStart);

        var entries = BuildEntries(members, snapshots, tokens, normalizedUser).ToList();
        AssignRanks(entries);

        CircleSummary summary = new()
        {
            Id = circle.Id,
            Name = circle.Name,
            InviteCode = circle.InviteCode,
            MemberCount = members.Count,
            MemberCapacity = CircleMemberCapacity
        };

        return new FriendsLeaderboardState
        {
            OptedIn = true,
            WeekStartUtc = weekStart,
            Circle = summary,
            Membership = membership,
            Entries = entries,
            SupportTokens = tokens,
            Templates = TokenTemplates
        };
    }

    public async Task<CircleMembership> OptInAsync(
        string username,
        string nickname,
        bool shareXp,
        bool shareStreak,
        bool shareCopingCount,
        string? inviteCode,
        string? circleName,
        DateTime nowUtc)
    {
        EnsureFeatureEnabled();
        var normalizedUser = NormalizeUsername(username);
        var sanitizedNickname = ValidateNickname(nickname);

        var existingMembership = await _repository.GetMembershipAsync(normalizedUser);
        if (existingMembership != null)
        {
            var updated = existingMembership.WithSharing(shareXp, shareStreak, shareCopingCount)
                                            .WithNickname(sanitizedNickname);
            updated.LastActiveUtc = nowUtc;
            await _repository.UpdateMembershipAsync(updated);
            await RefreshLocalSnapshotAsync(normalizedUser, nowUtc, updated);
            return updated;
        }

        FriendCircle circle;
        if (!string.IsNullOrWhiteSpace(inviteCode))
        {
            circle = await _repository.GetCircleByInviteCodeAsync(inviteCode.Trim().ToUpperInvariant())
                     ?? throw new InvalidOperationException("Circle invite code not found.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(circleName))
            {
                throw new ArgumentException("A circle name is required when creating a new circle.", nameof(circleName));
            }

            circle = new FriendCircle
            {
                Name = circleName.Trim(),
                InviteCode = await GenerateUniqueInviteCodeAsync(),
                CreatedUtc = nowUtc
            };

            var circleId = await _repository.AddCircleAsync(circle);
            circle.Id = circleId;
        }

        var members = await _repository.GetMembershipsForCircleAsync(circle.Id);
        if (members.Count >= CircleMemberCapacity)
        {
            throw new InvalidOperationException("This circle has reached its maximum capacity of 15 members.");
        }

        CircleMembership membership = new()
        {
            CircleId = circle.Id,
            Username = normalizedUser,
            Nickname = sanitizedNickname,
            ShareXp = shareXp,
            ShareStreak = shareStreak,
            ShareCopingCount = shareCopingCount,
            Muted = false,
            JoinedUtc = nowUtc,
            LastActiveUtc = nowUtc
        };

        var membershipId = await _repository.AddMembershipAsync(membership);
        membership.Id = membershipId;

        await RefreshLocalSnapshotAsync(normalizedUser, nowUtc, membership);

        return membership;
    }

    public async Task UpdateSharingAsync(string username, bool shareXp, bool shareStreak, bool shareCopingCount, DateTime nowUtc)
    {
        EnsureFeatureEnabled();
        var normalizedUser = NormalizeUsername(username);
        var membership = await _repository.GetMembershipAsync(normalizedUser)
                         ?? throw new InvalidOperationException("You must join a circle before updating preferences.");

        var updated = membership.WithSharing(shareXp, shareStreak, shareCopingCount);
        updated.LastActiveUtc = nowUtc;
        await _repository.UpdateMembershipAsync(updated);
        await RefreshLocalSnapshotAsync(normalizedUser, nowUtc, updated);
    }

    public async Task LeaveAsync(string username)
    {
        EnsureFeatureEnabled();
        var normalizedUser = NormalizeUsername(username);
        var membership = await _repository.GetMembershipAsync(normalizedUser);
        if (membership == null) return;

        await _repository.RemoveMembershipAsync(membership.Id);
    }

    public async Task ToggleMuteAsync(string username, bool muted, DateTime nowUtc)
    {
        EnsureFeatureEnabled();
        var normalizedUser = NormalizeUsername(username);
        var membership = await _repository.GetMembershipAsync(normalizedUser)
                         ?? throw new InvalidOperationException("Join a circle before updating mute preferences.");

        var updated = membership.WithMuteState(muted);
        updated.LastActiveUtc = nowUtc;
        await _repository.UpdateMembershipAsync(updated);
    }

    public async Task SendSupportTokenAsync(string username, string recipientUsername, string templateCode, DateTime nowUtc)
    {
        EnsureFeatureEnabled();
        var normalizedUser = NormalizeUsername(username);
        var normalizedRecipient = NormalizeUsername(recipientUsername);

        if (string.Equals(normalizedUser, normalizedRecipient, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("You cannot send a support token to yourself.");
        }

        var membership = await _repository.GetMembershipAsync(normalizedUser)
                         ?? throw new InvalidOperationException("Join a circle before sending support tokens.");
        var recipient = await _repository.GetMembershipAsync(normalizedRecipient)
                       ?? throw new InvalidOperationException("Recipient is not part of the circle.");

        if (membership.CircleId != recipient.CircleId)
        {
            throw new InvalidOperationException("Support tokens can only be sent to members of your circle.");
        }

        var template = TokenTemplates.FirstOrDefault(t =>
            string.Equals(t.Code, templateCode, StringComparison.OrdinalIgnoreCase));
        if (template == null)
        {
            throw new ArgumentException("Unknown support token template.", nameof(templateCode));
        }

        var weekStart = GetWeekStart(nowUtc);
        SupportToken token = new()
        {
            CircleId = membership.CircleId,
            FromUsername = normalizedUser,
            ToUsername = normalizedRecipient,
            TemplateCode = template.Code,
            Message = template.Body,
            WeekStartUtc = weekStart,
            CreatedUtc = nowUtc
        };

        _ = await _repository.AddSupportTokenAsync(token);
    }

    public async Task RefreshLocalSnapshotAsync(string username, DateTime nowUtc)
    {
        var membership = await _repository.GetMembershipAsync(NormalizeUsername(username));
        if (membership != null)
        {
            await RefreshLocalSnapshotAsync(username, nowUtc, membership);
        }
    }

    private async Task RefreshLocalSnapshotAsync(string username, DateTime nowUtc, CircleMembership membership)
    {
        var weekStart = GetWeekStart(nowUtc);
        var weekEnd = weekStart.AddDays(7);

        var transactions = await _pointsRepository.GetXpTransactionsAsync(membership.Username, weekStart, weekEnd);
        var sharedXp = transactions.Sum(tx => tx.Amount);
        var copingCount = transactions.Count(tx => !string.Equals(tx.ActionCode, "log_entry", StringComparison.OrdinalIgnoreCase));

        var allEntries = await _bingeStore.GetItemsAsync();
        var userEntries = allEntries.Where(e =>
            string.Equals(e.Username, membership.Username, StringComparison.OrdinalIgnoreCase));
        var streakDays = CalculateCurrentStreak(userEntries, nowUtc);

        CircleWeeklySnapshot snapshot = new()
        {
            CircleId = membership.CircleId,
            Username = membership.Username,
            WeekStartUtc = weekStart,
            SharedXp = sharedXp,
            SharedStreakDays = streakDays,
            SharedCopingCount = copingCount,
            CreatedUtc = nowUtc,
            RefreshedUtc = nowUtc
        };

        await _repository.UpsertWeeklySnapshotAsync(snapshot);

        membership.LastActiveUtc = nowUtc;
        await _repository.UpdateMembershipAsync(membership);
    }

    private static IEnumerable<LeaderboardEntry> BuildEntries(
        IReadOnlyList<CircleMembership> members,
        IReadOnlyList<CircleWeeklySnapshot> snapshots,
        IReadOnlyList<SupportToken> tokens,
        string currentUsername)
    {
        Dictionary<string, CircleWeeklySnapshot> snapshotLookup = snapshots
            .GroupBy(s => s.Username, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(s => s.RefreshedUtc ?? s.CreatedUtc).First(), StringComparer.OrdinalIgnoreCase);

        Dictionary<string, int> tokenCounts = tokens
            .GroupBy(t => t.ToUsername, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

        foreach (var member in members)
        {
            snapshotLookup.TryGetValue(member.Username, out var snapshot);
            var sharedXp = member.ShareXp ? snapshot?.SharedXp : null;
            var sharedStreak = member.ShareStreak ? snapshot?.SharedStreakDays : null;
            var sharedCoping = member.ShareCopingCount ? snapshot?.SharedCopingCount : null;

            var composite = ComputeCompositeScore(sharedXp, sharedStreak, sharedCoping);
            tokenCounts.TryGetValue(member.Username, out var receivedTokens);

            yield return new LeaderboardEntry
            {
                Username = member.Username,
                Nickname = member.Nickname,
                IsSelf = string.Equals(member.Username, currentUsername, StringComparison.OrdinalIgnoreCase),
                Muted = member.Muted,
                SharedXp = sharedXp,
                SharedStreakDays = sharedStreak,
                SharedCopingCount = sharedCoping,
                SupportTokensReceived = receivedTokens,
                CompositeScore = composite,
                Rank = 0 // assigned later
            };
        }
    }

    private static void AssignRanks(IList<LeaderboardEntry> entries)
    {
        var ordered = entries
            .OrderByDescending(e => e.CompositeScore)
            .ThenBy(e => e.Nickname, StringComparer.OrdinalIgnoreCase)
            .ToList();

        List<LeaderboardEntry> ranked = new(ordered.Count);
        double? lastScore = null;
        var currentRank = 0;

        for (var i = 0; i < ordered.Count; i++)
        {
            var entry = ordered[i];
            if (!lastScore.HasValue || !IsClose(lastScore.Value, entry.CompositeScore))
            {
                currentRank = i + 1;
                lastScore = entry.CompositeScore;
            }

            ranked.Add(new LeaderboardEntry
            {
                Username = entry.Username,
                Nickname = entry.Nickname,
                IsSelf = entry.IsSelf,
                Muted = entry.Muted,
                SupportTokensReceived = entry.SupportTokensReceived,
                SharedXp = entry.SharedXp,
                SharedStreakDays = entry.SharedStreakDays,
                SharedCopingCount = entry.SharedCopingCount,
                CompositeScore = entry.CompositeScore,
                Rank = currentRank
            });
        }

        if (entries is List<LeaderboardEntry> list)
        {
            list.Clear();
            list.AddRange(ranked);
        }
        else
        {
            for (var i = 0; i < ranked.Count; i++)
            {
                entries[i] = ranked[i];
            }
        }
    }

    private async Task<string> GenerateUniqueInviteCodeAsync()
    {
        for (var attempt = 0; attempt < 16; attempt++)
        {
            var code = GenerateInviteCode();
            var existing = await _repository.GetCircleByInviteCodeAsync(code);
            if (existing == null) return code;
        }

        throw new InvalidOperationException("Unable to generate a unique invite code.");
    }

    private static string GenerateInviteCode()
    {
        Span<byte> buffer = stackalloc byte[6];
        RandomNumberGenerator.Fill(buffer);

        Span<char> chars = stackalloc char[6];
        for (var i = 0; i < buffer.Length; i++)
        {
            chars[i] = InviteAlphabet[buffer[i] % InviteAlphabet.Length];
        }

        return new string(chars);
    }

    private static void EnsureFeatureEnabled()
    {
        if (!FeatureFlags.LeaderboardEnabled)
        {
            throw new InvalidOperationException("Leaderboard feature is disabled.");
        }
    }

    private static string NormalizeUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username is required.", nameof(username));
        }

        return username.Trim();
    }

    private static string ValidateNickname(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
        {
            throw new ArgumentException("A nickname is required for the leaderboard.", nameof(nickname));
        }

        return nickname.Trim();
    }

    private static DateTime GetWeekStart(DateTime nowUtc)
    {
        var date = nowUtc.Date;
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-diff);
    }

    private static int CalculateCurrentStreak(IEnumerable<BingeEntry> userEntries, DateTime nowUtc)
    {
        var lastEntry = userEntries
            .OrderByDescending(entry => entry.Date)
            .FirstOrDefault();

        if (lastEntry == null) return 0;

        var lastDate = lastEntry.Date.ToUniversalTime().Date;
        return Math.Max(0, (nowUtc.Date - lastDate).Days);
    }

    private static double ComputeCompositeScore(int? xp, int? streakDays, int? copingCount)
    {
        double score = 0;
        if (xp.HasValue)
        {
            score += xp.Value;
        }

        if (streakDays.HasValue)
        {
            score += streakDays.Value * 10d;
        }

        if (copingCount.HasValue)
        {
            score += copingCount.Value * 5d;
        }

        return score;
    }

    private static bool IsClose(double left, double right)
    {
        return Math.Abs(left - right) < 0.001;
    }
}
