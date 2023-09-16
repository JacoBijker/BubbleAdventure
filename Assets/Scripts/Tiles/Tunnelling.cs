using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;

public class Tunnelling : MonoBehaviour, ISetupable, IRunnable
{

    public string LinkID;
    private GameObject linkedTunnel;
    private bool isActive = false;
    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(Dictionary<string, string> extraInformation)
    {
        LinkID = extraInformation["LinkID"];
    }

    public void LinkTunnel(IEnumerable<Tunnelling> otherTunnels)
    {
        var toLink = otherTunnels.FirstOrDefault(s => s != this && s.LinkID == LinkID);
        linkedTunnel = toLink.gameObject;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (!isActive)
            return;

        var spawnLeft = (collisionInfo.transform.position.x > this.transform.position.x);
        float spawnOffset = spawnLeft ? -0.500f : 0.500f;
        var newPos = new Vector3(linkedTunnel.transform.position.x + spawnOffset, linkedTunnel.transform.position.y, collisionInfo.transform.position.z);
        collisionInfo.transform.position = newPos;
    }

    public void Run()
    {
        isActive = true;
    }
}
