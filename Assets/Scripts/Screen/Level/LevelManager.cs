using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts.Player;
using System.Collections.Generic;
using Assets.Scripts.Behavior;

public enum GameState
{
    SlideIn,
    Play,
    SlideOut,
    None
};

public class LevelManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject EnemySpawner;
    public GameObject LevelCleared;

    private PlayerAccessor playerAccessor = new PlayerAccessor();
    private Vector3 playerSpawnPoint;
    private ScreenManager screenManager;
    private CampaignManager campaignManager;
    private LevelAccessor levelAccessor = new LevelAccessor();

    private int TotalEnemies;
    private int DeadEnemies;
    private int currentLevel;

    private GameState gameState = GameState.None;
    private List<ICustomBehaviour> levelSlideIn;
    private List<ICustomBehaviour> levelSlideOut;
    private List<GameObject> lateRunners;

    void Start()
    {
        screenManager = GetComponentInChildren<ScreenManager>();
        LevelCleared.SetActive(false);
    }

    public void LoadLevel(string levelName, int currentLevel)
    {        
        EnablePlayer(false);
        lateRunners = new List<GameObject>();
        List<GameObject> allScreenObjects = new List<GameObject>();

        playerAccessor.Player.Level = currentLevel;
        TotalEnemies = 0;
        DeadEnemies = 0;
        BubbleManager.Clear();

        var yOffset = -Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).y * 2;

        screenManager.ClearLevel();
        
        var loadedLevelInfo = screenManager.LoadLevel(levelName, new int[] { 6, 7 }, true, yOffset);

        playerSpawnPoint = loadedLevelInfo.ExcludedTiles.Find(s => s.TileIndex == 6).Position;
        ResetPosition();

        var allTunnels = loadedLevelInfo.LoadedObjects.Where(s => s.tag == "Tunnel");
        var tunnelComponents = allTunnels.Select(s => s.GetComponent<Tunnelling>() as Tunnelling);
        foreach (var tunnel in tunnelComponents)
            tunnel.LinkTunnel(tunnelComponents);

        lateRunners.AddRange(allTunnels);

        var allLevelObjects = loadedLevelInfo.LoadedObjects;
        var allEnemySpawners = loadedLevelInfo.ExcludedTiles.Where(s => s.TileIndex == 7);
        foreach (var spawner in allEnemySpawners)
            lateRunners.Add(CreateEnemySpawner(spawner, yOffset));

        allLevelObjects.AddRange(lateRunners);

        levelSlideIn = new List<ICustomBehaviour>();
        allLevelObjects.ForEach(s => levelSlideIn.Add(new SlideIn(s, yOffset)));
        levelSlideOut = new List<ICustomBehaviour>();
        allLevelObjects.ForEach(s => levelSlideOut.Add(new SlideOut(s, yOffset)));
        gameState = GameState.SlideIn;
    }

    void FixedUpdate()
    {
        if (gameState == GameState.SlideIn)
        {
            if (!RunCustomBehavior(levelSlideIn))
                return;

            gameState = GameState.Play;
            lateRunners.ForEach(s => s.SendMessage("Run"));
            EnablePlayer(true);
            ResetPosition();
        }
        else if (gameState == GameState.SlideOut)
        {
            if (!RunCustomBehavior(levelSlideOut))
                return;

            gameState = GameState.None;
            LevelCleared.SetActive(false);
            campaignManager.LoadNextLevel();
        }
    }

    private bool RunCustomBehavior(List<ICustomBehaviour> sliders)
    {
        int counter = 0;
        while (counter < sliders.Count)
        {
            var slider = sliders[counter];
            slider.Update();
            if (!slider.IsActive)
                sliders.RemoveAt(counter);
            else
                counter++;
        }

        if (sliders.Count == 0)
            return true;

        return false;
    }

    private GameObject CreateEnemySpawner(TileType spawnerInfo, float yOffset)
    {
        var newPosition = new Vector3(spawnerInfo.Position.x, spawnerInfo.Position.y + yOffset, spawnerInfo.Position.z);
        var newSpawner = Instantiate(EnemySpawner, newPosition, Quaternion.identity) as GameObject;
        var spawnerComponent = newSpawner.GetComponent<EnemySpawner>();
        spawnerComponent.Setup(spawnerInfo.internalInformation);
        newSpawner.transform.parent = screenManager.transform;
        TotalEnemies += spawnerComponent.AmountToSpawn;
        return newSpawner;
    }

    void EnablePlayer(bool enable)
    {
        Player.SetActive(enable);
    }

    void ResetPosition()
    {
        Player.transform.position = new Vector3(playerSpawnPoint.x, playerSpawnPoint.y, -1);
        Player.GetComponent<PlayerController>().Reset();
    }

    void Respawn()
    {
        playerAccessor.Player.Lives--;
        playerAccessor.Player.BubbleDuration = 0;
        playerAccessor.Player.BubbleSize = 0;

        if (playerAccessor.Player.Lives <= 0)
            GetComponentInChildren<GameHUD>().SetMenu(HUDMenus.GameOver);
        else
            ResetPosition();
    }

    void Killed_Enemy()
    {
        DeadEnemies++;

        if (DeadEnemies >= TotalEnemies)
        {
            LevelCleared.SetActive(true);
            Invoke("NextLevel", 3f);
        }
    }

    internal void SetCampaignManager(CampaignManager campaignManager)
    {
        this.campaignManager = campaignManager;
    }

    void NextLevel()
    {
        if (campaignManager.HasNextLevel())
        {
            gameState = GameState.SlideOut;
        }
        else
        {
            GetComponentInChildren<GameHUD>().SetMenu(HUDMenus.Congratulations);
        }
    }
}
