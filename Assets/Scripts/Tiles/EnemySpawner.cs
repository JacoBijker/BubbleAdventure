using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

public class EnemySpawner : MonoBehaviour, ISetupable, IRunnable
{
    private bool isActive;
    private int amountToSpawn;
    private int enemyType;
    private float intervalInMilliseconds;

    private float timeInterval;
    void Start()
    {

    }

    void Update()
    {
        if (!isActive)
            return;

        timeInterval += Time.deltaTime;

        if (timeInterval > intervalInMilliseconds)
        {
            timeInterval -= intervalInMilliseconds;
            if (amountToSpawn > 0)
            {
                amountToSpawn--;
                var newEnemy = Instantiate(Global.EnemyTypes[enemyType], this.transform.position, Quaternion.identity) as GameObject;
                newEnemy.transform.parent = this.transform.parent;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Setup(System.Collections.Generic.Dictionary<string, string> extraInformation)
    {
        amountToSpawn = int.Parse(extraInformation["Amount"]);
        enemyType = int.Parse(extraInformation["EnemyType"]);
        intervalInMilliseconds = int.Parse(extraInformation["Interval"]) / 1000.0f;

        GetComponent<SpriteRenderer>().sprite = Global.EnemySpawnSprites[enemyType];
    }

    public int AmountToSpawn
    {
        get
        { return amountToSpawn; }
    }

    public void Run()
    {
        isActive = true;
    }
}
