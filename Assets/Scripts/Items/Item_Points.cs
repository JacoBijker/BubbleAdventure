using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;

public class Item_Points : ItemBase {
    public GameObject playerPickupPopup;
    public int AmountOfPoints;

    protected override void OnPickup(Collider2D player)
    {
        (new PlayerAccessor()).Player.Score += AmountOfPoints;
        var newPickupPopup = Instantiate(playerPickupPopup) as GameObject;
        var newPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        newPickupPopup.GetComponent<PlayerPickupPopup>().Setup(newPosition, "+" + AmountOfPoints.ToString(), 6);
    }
    
}
