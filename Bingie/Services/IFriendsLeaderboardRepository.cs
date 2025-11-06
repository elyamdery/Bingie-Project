using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bingie.Models;

namespace Bingie.Services;

/// <summary>
/// Persistence boundary for circles, memberships, weekly snapshots, and support tokens.
/// </summary>
public interface IFriendsLeaderboardRepository
{
    Task<FriendCircle?> GetCircleByInviteCodeAsync(string inviteCode);
    Task<FriendCircle?> GetCircleByIdAsync(int circleId);
    Task<int> AddCircleAsync(FriendCircle circle);
    Task UpdateCircleAsync(FriendCircle circle);

    Task<CircleMembership?> GetMembershipAsync(string username);
    Task<CircleMembership?> GetMembershipByIdAsync(int membershipId);
    Task<IReadOnlyList<CircleMembership>> GetMembershipsForCircleAsync(int circleId);
    Task<int> AddMembershipAsync(CircleMembership membership);
    Task UpdateMembershipAsync(CircleMembership membership);
    Task RemoveMembershipAsync(int membershipId);

    Task<IReadOnlyList<CircleWeeklySnapshot>> GetSnapshotsForWeekAsync(int circleId, DateTime weekStartUtc);
    Task UpsertWeeklySnapshotAsync(CircleWeeklySnapshot snapshot);

    Task<IReadOnlyList<SupportToken>> GetSupportTokensForWeekAsync(int circleId, DateTime weekStartUtc);
    Task<int> AddSupportTokenAsync(SupportToken token);
}
