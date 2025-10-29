using System;

namespace Bingie.Config;

/// <summary>
/// Centralized feature flags for the Bingie experience.
/// Flags check overrides first, then environment variables, then defaults.
/// </summary>
public static class FeatureFlags
{
    private static bool? _avatarFeedbackOverride;
    private static bool? _storyGuideOverride;

    public static bool AvatarFeedbackEnabled
    {
        get
        {
            if (_avatarFeedbackOverride.HasValue) return _avatarFeedbackOverride.Value;
            return ResolveFlag("BINGIE_AVATAR_FEEDBACK_ENABLED", defaultValue: true);
        }
    }

    public static bool StoryGuideEnabled
    {
        get
        {
            if (_storyGuideOverride.HasValue) return _storyGuideOverride.Value;
            return ResolveFlag("BINGIE_STORY_GUIDE_ENABLED", defaultValue: true);
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

    public static void OverrideAvatarFeedback(bool? enabled) => _avatarFeedbackOverride = enabled;

    public static void OverrideStoryGuide(bool? enabled) => _storyGuideOverride = enabled;
}
