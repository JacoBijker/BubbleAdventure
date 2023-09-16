using UnityEngine;
using System.Collections;

public class Bomb : ItemBase {

    protected override void OnPickup(Collider2D player)
    {
        BubbleManager.BlowUp();
    }
}
