using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bingie.Models;

namespace Bingie.Services;

public interface IStoryGuideRepository
{
    Task UpsertTriggerSelectionAsync(StoryTriggerSelection selection);
    Task<StoryTriggerSelection?> GetTriggerSelectionAsync(string username, DateTime entryDateUtc);
    Task<IReadOnlyList<StoryTriggerSelection>> GetTriggerSelectionsAsync(string username, DateTime fromUtc, DateTime toUtc);

    Task<StoryExperimentProgress?> GetExperimentProgressAsync(string username, string experimentCode);
    Task<IReadOnlyList<StoryExperimentProgress>> GetExperimentProgressAsync(string username);
    Task UpsertExperimentProgressAsync(StoryExperimentProgress progress);
}
