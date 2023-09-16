using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class uniqueTileIndex
{
    private int x, y, z;
    public uniqueTileIndex(Vector2 tilePosition)
        : this(new Vector3(tilePosition.x, tilePosition.y, 0))
    { }
    public uniqueTileIndex(Vector3 tilePosition)
    {
        x = (int)Mathf.Round(tilePosition.x * 100);
        y = (int)Mathf.Round(tilePosition.y * 100);
        z = (int)Mathf.Round(tilePosition.z * 100);
    }

    public override bool Equals(object obj)
    {
        var uti = obj as uniqueTileIndex;
        if (uti != null)
            return x == uti.x && y == uti.y && z == uti.z;

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + x.GetHashCode();
        hash = hash * 23 + y.GetHashCode();
        hash = hash * 23 + z.GetHashCode();
        return hash;
    }
}

public class Level
{
    public string LevelName { get; set; }
    public Dictionary<uniqueTileIndex, TileType> tiles { get; set; }

    public LevelTypes LevelType { get; set; }

    public Level()
    {
        tiles = new Dictionary<uniqueTileIndex, TileType>();
    }

    public Level(string levelName)
        : this()
    {
        this.LevelName = levelName;
    }

    public void Load(string levelName)
    {
        tiles.Clear();

        var fileLines = FileManager.SplitOnNewlines(FileManager.LoadFile(levelName));
        LevelName = fileLines[0];
        LevelType = (LevelTypes)Enum.Parse(typeof(LevelTypes), fileLines[1]);
        for (int i = 2; i < fileLines.Length; i++)
        {
            var newTile = new TileType(fileLines[i]);
            tiles.Add(newTile.GetUniqueKey(), newTile);
        }
    }

    public void Save(string levelName)
    {
        StreamWriter SW = new StreamWriter(levelName);
        SW.WriteLine(LevelName);
        SW.WriteLine(LevelType.ToString());
        foreach (var tile in tiles.Values)
            SW.WriteLine(tile.ToString());
        SW.Close();
    }

    private bool NearZero(float one, float two)
    {
        return Mathf.Abs(two - one) < 0.01f;
    }

    public bool RemoveTileIfSameType(TileType tile)
    {
        lock (tiles)
        {
            var key = tile.GetUniqueKey();
            if (tiles.ContainsKey(key))
            {
                if (tiles[key].TileIndex != tile.TileIndex)
                    return false;

                tiles.Remove(key);
                return true;
            }

            return false;
        }
    }

    public bool RemoveTile(TileType tile)
    {
        lock (tiles)
        {
            var key = tile.GetUniqueKey();
            if (tiles.ContainsKey(key))
            {
                tiles.Remove(key);
                return true;
            }
            return false;
        }
    }

    public bool AddTile(TileType tile)
    {
        lock (tiles)
        {
            var key = tile.GetUniqueKey();
            if (tiles.ContainsKey(key))
            {
                var toChange = tiles[key];
                if (toChange.TileIndex != tile.TileIndex)
                {
                    toChange.TileIndex = tile.TileIndex;
                    return true;
                }

                return false;
            }

            tiles.Add(key, tile);
            return true;
        }
    }
}
