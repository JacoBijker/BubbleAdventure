using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;

public enum UpgradeType
{
    Size,
    Duration
}

public class UpgradePlayer : ItemBase
{
    public UpgradeType type;

    protected override void OnPickup(Collider2D player)
    {
        var playerAccess = (new PlayerAccessor()).Player;
        switch (type)
        {
            case UpgradeType.Size:
                playerAccess.BubbleSize = Mathf.Min(playerAccess.BubbleSize + 1, 3);
                break;
            case UpgradeType.Duration:
                playerAccess.BubbleDuration = Mathf.Min(playerAccess.BubbleDuration + 1, 3);
                break;
        }

    }
}
