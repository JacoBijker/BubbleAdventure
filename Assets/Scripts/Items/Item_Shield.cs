using UnityEngine;
using System.Collections;

public class Item_Shield : ItemBase {
    public GameObject Shield;

    protected override void OnPickup(Collider2D player)
    {
        var newShield = Instantiate(Shield, Vector3.zero, Quaternion.identity) as GameObject;
        newShield.transform.parent = player.gameObject.transform;
        newShield.transform.localPosition = Vector3.zero;
    }
}
