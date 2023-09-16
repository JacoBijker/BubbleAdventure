using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

public class ItemSpawner : MonoBehaviour, ISetupable, IRunnable {
    private float SecondsBeforeSpawning;
    public GameObject[] Items;

    void Spawn()
    {
        var index = Random.Range(0, Items.Length);
        var newItem = Instantiate(Items[index], this.transform.position, Quaternion.identity) as GameObject;
        newItem.transform.parent = this.transform.parent;
        Destroy(this.gameObject);
    }

    public void Setup(System.Collections.Generic.Dictionary<string, string> extraInformation)
    {
        SecondsBeforeSpawning = float.Parse(extraInformation["Seconds"]);
    }

    public void Run()
    {
        Invoke("Spawn", SecondsBeforeSpawning);
    }
}
