namespace Bingie.Services;

public interface IStoryGuideAnalytics
{
    void TrackTriggerLogged(string username, string triggerCode);
    void TrackExperimentSuggested(string username, string experimentCode);
    void TrackExperimentPlanned(string username, string experimentCode);
    void TrackExperimentCompleted(string username, string experimentCode, int xpAwarded);
}

internal sealed class NoopStoryGuideAnalytics : IStoryGuideAnalytics
{
    public void TrackTriggerLogged(string username, string triggerCode)
    {
    }

    public void TrackExperimentSuggested(string username, string experimentCode)
    {
    }

    public void TrackExperimentPlanned(string username, string experimentCode)
    {
    }

    public void TrackExperimentCompleted(string username, string experimentCode, int xpAwarded)
    {
    }
}
