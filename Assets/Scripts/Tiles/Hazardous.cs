using UnityEngine;
using System.Collections;

public class Hazardous : MonoBehaviour {

    public string[] TagsToKill;

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
            collisionInfo.gameObject.SendMessage("Kill");
        else if (collisionInfo.gameObject.tag == "Bubble")
            collisionInfo.gameObject.SendMessage("SelfDestruct");
    }
}
