using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;

public enum LevelTypes
{
    Grass,
    Snow,
    Stone
}

public static class Global
{
    public static GameObject[] grassBlockTypes { get; set; }
    public static GameObject[] snowBlockTypes { get; set; }
    public static GameObject[] stonelockTypes { get; set; }
    public static GameObject[] BlockTypes { get; set; }
    public static GameObject[] EnemyTypes { get; set; }
    public static Sprite[] EnemySpawnSprites { get; set; }
    public static Sprite[] BubbleTypes { get; set; }

    public static AudioClip[] footStepClips { get; set; }
    public static AudioClip[] dieClips { get; set; }
    public static AudioClip[] chestClips { get; set; }
    public static AudioClip[] pickupClips { get; set; }
    public static AudioClip[] attackClips { get; set; }

    public static GameObject levelManager { get; set; }

    public static void SwitchLevelType(LevelTypes levelType)
    {
        GameObject[] toSwitch = null;
        switch(levelType)
        {
            case LevelTypes.Grass:
                toSwitch = grassBlockTypes;
                break;
            case LevelTypes.Snow:
                toSwitch = snowBlockTypes;
                break;
            case LevelTypes.Stone:
                toSwitch = stonelockTypes;
                break;
        }

        for (int i = 0; i < toSwitch.Length; i++)
            BlockTypes[i] = toSwitch[i];
    }
}

public class GlobalInformation : MonoBehaviour
{
    public GameObject[] grassBlockTypes;
    public GameObject[] snowBlockTypes;
    public GameObject[] stonelockTypes;

    public GameObject[] blockTypes;
    public GameObject[] enemyTypes;

    public Sprite[] enemySpawnSprites;
    public Sprite[] bubbleTypes;

    public AudioClip[] footStepClips;
    public AudioClip[] dieClips;
    public AudioClip[] chestClips;
    public AudioClip[] pickupClips;
    public AudioClip[] attackClips;

    public GameObject levelManager;

    // Use this for initialization
    void Start()
    {
        Global.grassBlockTypes = grassBlockTypes;
        Global.snowBlockTypes = snowBlockTypes;
        Global.stonelockTypes = stonelockTypes;

        Global.BlockTypes = blockTypes;
        Global.EnemyTypes = enemyTypes;
        Global.BubbleTypes = bubbleTypes;
        Global.EnemySpawnSprites = enemySpawnSprites;

        Global.footStepClips = footStepClips;
        Global.dieClips = dieClips;
        Global.chestClips = chestClips;
        Global.pickupClips = pickupClips;
        Global.attackClips = attackClips;

        Global.levelManager = levelManager;

        PlayerAccessor playerAccessor = new PlayerAccessor();
        playerAccessor.Player = new Player();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
