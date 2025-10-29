using System.Collections.Generic;
using Bingie.Models;

namespace Bingie.Services;

internal static class StoryGuideCatalogue
{
    public const string CustomTriggerCode = "custom";

    private static readonly IReadOnlyList<StoryTriggerDefinition> Triggers = new List<StoryTriggerDefinition>
    {
        new()
        {
            Code = "lonely",
            Title = "Feeling isolated",
            Description = "Craving connection or comfort.",
            ChapterIntro = "Moments of loneliness often invite the urge to soothe with food. Let's rewrite that chapter with gentle connection."
        },
        new()
        {
            Code = "stress",
            Title = "Stress spike",
            Description = "Overwhelmed, tense, or pressured.",
            ChapterIntro = "Stress can hijack the story fast. This episode is all about small releases that honour your limits."
        },
        new()
        {
            Code = "fatigue",
            Title = "Running on empty",
            Description = "Exhausted or sleep deprived.",
            ChapterIntro = "When energy is low, the body shouts for quick relief. We can explore chapters that add rest back in."
        },
        new()
        {
            Code = "bored",
            Title = "Need for spark",
            Description = "Unstimulated or drifting.",
            ChapterIntro = "Boredom is a powerful narrator. Together we'll seed tiny bursts of novelty that feel nourishing."
        },
        new()
        {
            Code = "celebrate",
            Title = "Celebration spillover",
            Description = "Rewarding yourself or keeping the party going.",
            ChapterIntro = "Celebrations deserve a joyful script. We'll weave in rituals that keep the glow without the crash."
        },
        new()
        {
            Code = "restrict",
            Title = "Restrictive rebound",
            Description = "Came after rigid rules or skipped meals.",
            ChapterIntro = "Restriction sets up a dramatic twist. Let's soften the chapter with steady nourishment and permission."
        },
        new()
        {
            Code = "social",
            Title = "Social pressure",
            Description = "Triggered by comments or comparison.",
            ChapterIntro = "Social scenarios can add heavy dialogue. We'll craft responses that protect your peace."
        },
        new()
        {
            Code = CustomTriggerCode,
            Title = "Something else",
            Description = "Name your own reason in the moment.",
            ChapterIntro = "Every story is unique. Naming your reason already adds clarity and care to the narrative."
        }
    };

    private static readonly IReadOnlyDictionary<string, IReadOnlyList<StoryExperimentDefinition>> Experiments =
        new Dictionary<string, IReadOnlyList<StoryExperimentDefinition>>
        {
            ["lonely"] = new[]
            {
                new StoryExperimentDefinition
                {
                    Code = "call-a-kind-voice",
                    TriggerCode = "lonely",
                    Title = "Call a kind voice",
                    Prompt = "Send a voice note or text to someone who feels safe—even if you simply say hello.",
                    XpReward = 12
                },
                new StoryExperimentDefinition
                {
                    Code = "self-soothing-kit",
                    TriggerCode = "lonely",
                    Title = "Assemble a self-soothing kit",
                    Prompt = "Gather three items (song, scent, texture) that remind you you're cared for.",
                    XpReward = 12
                }
            },
            ["stress"] = new[]
            {
                new StoryExperimentDefinition
                {
                    Code = "90-second-reset",
                    TriggerCode = "stress",
                    Title = "90-second reset",
                    Prompt = "Move or stretch for 90 seconds while naming what feels heavy and what you can pause.",
                    XpReward = 14
                },
                new StoryExperimentDefinition
                {
                    Code = "micro-boundary",
                    TriggerCode = "stress",
                    Title = "Micro boundary",
                    Prompt = "Choose one small task to defer or delegate today and note how your body responds.",
                    XpReward = 14
                }
            },
            ["fatigue"] = new[]
            {
                new StoryExperimentDefinition
                {
                    Code = "rest-midstory",
                    TriggerCode = "fatigue",
                    Title = "Interpretive rest",
                    Prompt = "Schedule a 10-minute break where you intentionally do nothing but breathe or daydream.",
                    XpReward = 10
                },
                new StoryExperimentDefinition
                {
                    Code = "evening-winddown",
                    TriggerCode = "fatigue",
                    Title = "Evening wind-down",
                    Prompt = "Pick one gentle wind-down activity tonight (warm drink, soft music) and notice the impact.",
                    XpReward = 10
                }
            },
            ["bored"] = new[]
            {
                new StoryExperimentDefinition
                {
                    Code = "novelty-five",
                    TriggerCode = "bored",
                    Title = "Novelty in five",
                    Prompt = "Try a five-minute curiosity burst—listen to a new short podcast clip or sketch something random.",
                    XpReward = 11
                },
                new StoryExperimentDefinition
                {
                    Code = "future-note",
                    TriggerCode = "bored",
                    Title = "Future you note",
                    Prompt = "Write a sticky note inviting future-you to a small plan you’re looking forward to.",
                    XpReward = 11
                }
            },
            ["celebrate"] = new[]
            {
                new StoryExperimentDefinition
                {
                    Code = "memory-toast",
                    TriggerCode = "celebrate",
                    Title = "Memory toast",
                    Prompt = "Mark the moment by naming three things you’re proud of aloud or in a journal.",
                    XpReward = 13
                },
                new StoryExperimentDefinition
                {
                    Code = "ritual-refresh",
                    TriggerCode = "celebrate",
                    Title = "Ritual refresh",
                    Prompt = "Create a celebration ritual that involves movement, music, or sharing a moment with someone.",
                    XpReward = 13
                }
            },
            ["restrict"] = new[]
            {
                new StoryExperimentDefinition
                {
                    Code = "compassionate-meal",
                    TriggerCode = "restrict",
                    Title = "Compassionate meal",
                    Prompt = "Plan or enjoy one balanced meal today with curiosity instead of rules.",
                    XpReward = 15
                },
                new StoryExperimentDefinition
                {
                    Code = "permission-note",
                    TriggerCode = "restrict",
                    Title = "Permission slip",
                    Prompt = "Write yourself a short permission slip to eat when your body whispers hunger.",
                    XpReward = 15
                }
            },
            ["social"] = new[]
            {
                new StoryExperimentDefinition
                {
                    Code = "compassion-reframe",
                    TriggerCode = "social",
                    Title = "Compassion reframe",
                    Prompt = "When a comment stings, respond internally with a compassionate phrase you pre-select.",
                    XpReward = 12
                },
                new StoryExperimentDefinition
                {
                    Code = "ally-check-in",
                    TriggerCode = "social",
                    Title = "Ally check-in",
                    Prompt = "Share what happened with someone supportive and note how validation shifts the narrative.",
                    XpReward = 12
                }
            },
            [CustomTriggerCode] = new[]
            {
                new StoryExperimentDefinition
                {
                    Code = "self-reflection",
                    TriggerCode = CustomTriggerCode,
                    Title = "Gentle reflection",
                    Prompt = "Spend five minutes writing about what you needed in that moment, without judgement.",
                    XpReward = 10
                },
                new StoryExperimentDefinition
                {
                    Code = "supportive-plan",
                    TriggerCode = CustomTriggerCode,
                    Title = "Supportive plan",
                    Prompt = "List two caring actions that could help next time this feeling surfaces.",
                    XpReward = 10
                }
            }
        };

    public static IReadOnlyList<StoryTriggerDefinition> GetTriggers() => Triggers;

    public static StoryTriggerDefinition GetTrigger(string code)
    {
        foreach (var trigger in Triggers)
        {
            if (string.Equals(trigger.Code, code, System.StringComparison.OrdinalIgnoreCase))
            {
                return trigger;
            }
        }

        return Triggers[^1]; // default to custom
    }

    public static IReadOnlyList<StoryExperimentDefinition> GetExperiments(string triggerCode)
    {
        return Experiments.TryGetValue(triggerCode, out var list)
            ? list
            : Experiments[CustomTriggerCode];
    }
}
