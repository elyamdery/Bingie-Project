using System;

namespace Bingie.Config;

/// <summary>
/// Centralized feature flags for runtime and tests.
/// Flags resolve from overrides first, then environment variables, then defaults.
/// </summary>
public static class FeatureFlags
{
    private static bool? _avatarFeedbackOverride;

    /// <summary>
    /// Feature flag gating the avatar feedback experience.
    /// </summary>
    public static bool AvatarFeedbackEnabled
    {
        get
        {
            if (_avatarFeedbackOverride.HasValue) return _avatarFeedbackOverride.Value;

            var raw = Environment.GetEnvironmentVariable("BINGIE_AVATAR_FEEDBACK_ENABLED");
            if (string.IsNullOrWhiteSpace(raw)) return true;

            return raw switch
            {
                "0" => false,
                "false" => false,
                "FALSE" => false,
                _ => true
            };
        }
    }

    /// <summary>
    /// Allows tests or diagnostics to override the computed value.
    /// Passing <c>null</c> clears the override.
    /// </summary>
    public static void OverrideAvatarFeedback(bool? enabled)
    {
        _avatarFeedbackOverride = enabled;
    }
}
