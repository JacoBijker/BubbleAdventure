using Assets.Scripts.Structures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private LevelAccessor levelAccessor;
    private LevelAccessor LevelAccessor
    {
        get
        {
            levelAccessor = levelAccessor ?? new LevelAccessor();
            return levelAccessor;
        }
    }

    public float CubeSize = 0.692f;
    // Use this for initialization
    void Start()
    {
        LevelAccessor.Level = new Level("Default");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool CheckVectorPass(TileType newTile, Vector3 toCheck)
    {
        if ((toCheck.x == 1) != LevelAccessor.Level.tiles.ContainsKey(newTile.Left(CubeSize)))
            return false;
        if ((toCheck.y == 1) != LevelAccessor.Level.tiles.ContainsKey(newTile.Top(CubeSize)))
            return false;
        if ((toCheck.z == 1) != LevelAccessor.Level.tiles.ContainsKey(newTile.Right(CubeSize)))
            return false;

        return true;
    }

    private Dictionary<int, Vector3> dirtTiles = new Dictionary<int, Vector3>()
    {
        { 1, new Vector3(0,0,1) },
        { 2, new Vector3(1,0,1) },
        { 3, new Vector3(1,0,0) },
        { 4, new Vector3(0,0,0) }
    };

    private void CalculateNewTileType(TileType newTile, bool checkSurroundingTypes)
    {
        if (newTile.TileIndex > 4)
            return;

        bool hasChanged = false;

        foreach (var dirtKey in dirtTiles.Keys)
        {
            if (CheckVectorPass(newTile, dirtTiles[dirtKey]))
            {
                newTile.TileIndex = dirtKey;
                hasChanged = true;
            }
        }

        if (!hasChanged)
            newTile.TileIndex = 0;

        if (checkSurroundingTypes)
            RefreshSurroundingTiles(newTile);
    }

    private void RefreshSurroundingTiles(TileType newTile)
    {
        if (LevelAccessor.Level.tiles.ContainsKey(newTile.Left(CubeSize)))
            RefreshTile(LevelAccessor.Level.tiles[newTile.Left(CubeSize)], false);

        if (LevelAccessor.Level.tiles.ContainsKey(newTile.Right(CubeSize)))
            RefreshTile(LevelAccessor.Level.tiles[newTile.Right(CubeSize)], false);

        if (LevelAccessor.Level.tiles.ContainsKey(newTile.Top(CubeSize)))
            RefreshTile(LevelAccessor.Level.tiles[newTile.Top(CubeSize)], false);

        if (LevelAccessor.Level.tiles.ContainsKey(newTile.Bottom(CubeSize)))
            RefreshTile(LevelAccessor.Level.tiles[newTile.Bottom(CubeSize)], false);
    }

    public void RefreshTile(TileType newTile, bool checkSurroundingTypes = false)
    {
        CalculateNewTileType(newTile, checkSurroundingTypes);
        RemoveTileNoRefresh(newTile);
        var mustAdd = LevelAccessor.Level.AddTile(newTile);

        if (!mustAdd)
            return;

        var tile = Instantiate(Global.BlockTypes[newTile.TileIndex]) as GameObject;
        tile.transform.position = newTile.Position;
        tile.transform.parent = this.transform;
    }

    public void AddTile(TileType newTile)
    {
        CalculateNewTileType(newTile, true);
        RemoveTileNoRefresh(newTile);
        var mustAdd = LevelAccessor.Level.AddTile(newTile);

        if (!mustAdd)
            return;

        var tile = Instantiate(Global.BlockTypes[newTile.TileIndex]) as GameObject;
        tile.transform.position = newTile.Position;
        tile.transform.parent = this.transform;
    }

    private void RemoveTileNoRefresh(TileType tileToRemove)
    {
        if (!LevelAccessor.Level.RemoveTile(tileToRemove))
            return;

        RemoveComplete(tileToRemove);
    }

    public TileType GetTileAtPosition(Vector2 Position)
    {
        var index = new uniqueTileIndex(Position);
        if (levelAccessor.Level.tiles.ContainsKey(index))
            return levelAccessor.Level.tiles[index];

        return null;
    }

    public void RemoveTile(TileType tileToRemove)
    {
        if (!LevelAccessor.Level.RemoveTile(tileToRemove))
            return;

        RefreshSurroundingTiles(tileToRemove);
        RemoveComplete(tileToRemove);
    }

    private void RemoveComplete(TileType tileToRemove)
    {
        var count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = transform.GetChild(i);
            if (child.position.x.FloatEquals(tileToRemove.Position.x) && child.position.y.FloatEquals(tileToRemove.Position.y) && child.position.z.FloatEquals(tileToRemove.Position.z))
            {
                DestroyImmediate(child.gameObject);
                break;
            }
        }
    }

    public LoadedLevelInformation LoadLevel(string levelName, int[] blockIgnoreList, bool run = false, float yOffset = 0)
    {
        LoadedLevelInformation levelInfo = new LoadedLevelInformation();
        
        LevelAccessor.Level = new Level(levelName);
        LevelAccessor.Level.Load(levelName);
        Global.SwitchLevelType(LevelAccessor.Level.LevelType);
        ClearLevel();
        foreach (var newTile in levelAccessor.Level.tiles.Values)
        {
            if (blockIgnoreList.Contains(newTile.TileIndex))
            {
                levelInfo.ExcludedTiles.Add(newTile);
                continue;
            }
            var tile = Instantiate(Global.BlockTypes[newTile.TileIndex]) as GameObject;
            tile.transform.position = new Vector3(newTile.Position.x, newTile.Position.y + yOffset, newTile.Position.z);
            tile.transform.parent = this.transform;

            if (newTile.internalInformation.Keys.Count > 1)
                tile.SendMessage("Setup", newTile.internalInformation);

            if (run && newTile.TileIndex == 9) //if itemspawner
                tile.SendMessage("Run");

            levelInfo.LoadedObjects.Add(tile);
        }
        return levelInfo;
    }

    public void SaveLevel(string levelName)
    {
        LevelAccessor.Level.Save(levelName);
    }

    public void ClearLevel()
    {
        var count = transform.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void Killed_Enemy()
    {
        Debug.Log("Got it in mg");
    }
}
