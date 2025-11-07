using Bingie.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Bingie.Messaging;

public sealed class PointsDashboardUpdatedMessage : ValueChangedMessage<PointsDashboard?>
{
    public PointsDashboardUpdatedMessage(PointsDashboard? value) : base(value) { }
}

public sealed class BingeEntryAddedMessage : ValueChangedMessage<BingeEntry>
{
    public BingeEntryAddedMessage(BingeEntry value) : base(value) { }
}

public sealed class FriendsLeaderboardVisibilityChangedMessage : ValueChangedMessage<bool>
{
    public FriendsLeaderboardVisibilityChangedMessage(bool value) : base(value) { }
}
