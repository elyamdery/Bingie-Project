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
    public static void OverrideAvatarFeedback(bool? enabled)
    {
        _avatarFeedbackOverride = enabled;
    }

    /// <summary>
    /// Allows tests or diagnostics to override the story guide flag; passing null clears the override.
    /// </summary>
    public static void OverrideStoryGuide(bool? enabled)
    {
        _storyGuideOverride = enabled;
    }
}
