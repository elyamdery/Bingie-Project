using System;

namespace Bingie.Config;

/// <summary>
/// Centralized feature flags for runtime and tests.
/// Flags resolve from overrides first, then environment variables, then defaults.
/// </summary>
public static class FeatureFlags
{
    private static bool? _avatarFeedbackOverride;
    private static bool? _storyGuideOverride;
    private static bool? _pointsSystemOverride;
    private static bool? _leaderboardOverride;

    /// <summary>
    /// Feature flag gating the avatar feedback experience.
    /// </summary>
    public static bool AvatarFeedbackEnabled
    {
        get
        {
            if (_avatarFeedbackOverride.HasValue) return _avatarFeedbackOverride.Value;
            return ResolveFlag("BINGIE_AVATAR_FEEDBACK_ENABLED", defaultValue: true);
        }
    }

    /// <summary>
    /// Feature flag that toggles the story guide narrative experience.
    /// </summary>
    public static bool StoryGuideEnabled
    {
        get
        {
            if (_storyGuideOverride.HasValue) return _storyGuideOverride.Value;
            return ResolveFlag("BINGIE_STORY_GUIDE_ENABLED", defaultValue: true);
        }
    }

    /// <summary>
    /// Feature flag controlling the XP/points system.
    /// </summary>
    public static bool PointsSystemEnabled
    {
        get
        {
            if (_pointsSystemOverride.HasValue) return _pointsSystemOverride.Value;
            return ResolveFlag("BINGIE_POINTS_SYSTEM_ENABLED", defaultValue: true);
        }
    }

    /// <summary>
    /// Feature flag that enables the friends leaderboard experience.
    /// </summary>
    public static bool LeaderboardEnabled
    {
        get
        {
            if (_leaderboardOverride.HasValue) return _leaderboardOverride.Value;
            return ResolveFlag("BINGIE_LEADERBOARD_ENABLED", defaultValue: true);
        }
    }

    private static bool ResolveFlag(string environmentVariable, bool defaultValue)
    {
        var raw = Environment.GetEnvironmentVariable(environmentVariable);
        if (string.IsNullOrWhiteSpace(raw)) return defaultValue;

        return raw.ToLowerInvariant() switch
        {
            "0" or "false" or "off" or "disabled" => false,
            _ => true
        };
    }

    /// <summary>
    /// Allows tests or diagnostics to override the avatar feedback flag; passing null clears the override.
    /// </summary>
    public static void OverrideAvatarFeedback(bool? enabled) => _avatarFeedbackOverride = enabled;

    /// <summary>
    /// Allows tests or diagnostics to override the story guide flag; passing null clears the override.
    /// </summary>
    public static void OverrideStoryGuide(bool? enabled) => _storyGuideOverride = enabled;

    /// <summary>
    /// Allows tests or diagnostics to override the points system flag; passing null clears the override.
    /// </summary>
    public static void OverridePointsSystem(bool? enabled) => _pointsSystemOverride = enabled;

    /// <summary>
    /// Allows tests or diagnostics to override the friends leaderboard flag; passing null clears the override.
    /// </summary>
    public static void OverrideLeaderboard(bool? enabled) => _leaderboardOverride = enabled;
}
