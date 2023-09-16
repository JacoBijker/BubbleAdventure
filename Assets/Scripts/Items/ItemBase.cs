using Assets.Scripts.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    private Flicker flicker;
    void Start()
    {
        Invoke("StartFlicker", 5f);
        Invoke("Remove", 7f);
        flicker = new Flicker(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            OnPickup(other);
        }
    }

    void Update()
    {
        flicker.Update();
    }

    void Remove()
    {
        flicker.Stop();
        Destroy(this.gameObject);
    }

    void StartFlicker()
    {
        flicker.Start();
    }

    protected virtual void OnPickup(Collider2D player)
    {

    }
}
