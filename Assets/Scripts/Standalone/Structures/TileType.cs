using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Globalization;

public class TileType
{
    private static Dictionary<int, string[]> RequiredInformation = new Dictionary<int, string[]>()
    {
        {5, new string [] { "LinkID" } },
        {7, new string [] { "Amount", "EnemyType", "Interval"}},
        {9, new string [] { "Seconds" }}
    };

    private static Dictionary<int, string> TileTypeNames = new Dictionary<int, string>()
    {
        {0, "Dirt" },
        {1, "Dirt" },
        {2, "Dirt" },
        {3, "Dirt" },
        {4, "Dirt" },
        {5, "Tunnel" },
        {6, "Player Spawn" },
        {7, "Enemy Spawn" },
        {8, "Border" },
        {9, "Item Spawner"},
        {10, "Spikes"},
        {11, "Spikes"}
    };

    public Dictionary<string, string> internalInformation { get; internal set; }
    public Vector3 Position { get; set; }
    private int tileIndex;
    public int TileIndex
    {
        get
        {
            return tileIndex;
        }
        set
        {
            tileIndex = value;
            internalInformation["Name"] = TileTypeNames[tileIndex];
        }
    }

    public TileType(int tileIndex)
    {
        internalInformation = new Dictionary<string, string>();
        this.TileIndex = tileIndex;
        if (RequiredInformation.ContainsKey(tileIndex))
            foreach (var str in RequiredInformation[tileIndex])
                internalInformation.Add(str, string.Empty);
    }

    public TileType(int tileIndex, Vector3 position)
        : this(tileIndex)
    {
        this.Position = position;
    }

    public TileType(string inputString)
    {
        internalInformation = new Dictionary<string, string>();
        string[] inputs = inputString.Split(',');
        TileIndex = Convert.ToInt32(inputs[0]);
        Position = new Vector3(float.Parse(inputs[1], CultureInfo.InvariantCulture.NumberFormat),
                               float.Parse(inputs[2], CultureInfo.InvariantCulture.NumberFormat),
                               float.Parse(inputs[3], CultureInfo.InvariantCulture.NumberFormat));

        int counter = 4;
        while (counter < inputs.Length)
        {
            var splits = inputs[counter].Split('=');
            internalInformation[splits[0]] = splits[1];
            counter++;
        }
    }

    /*
    public override bool Equals(object obj)
    {
        if (obj is TileType)
            return (obj as TileType).Position == this.Position;

        return base.Equals(obj);
    }
    */
    public override string ToString()
    {
        string toReturn = string.Format("{0},{1},{2},{3}", TileIndex, Position.x, Position.y, Position.z);

        var allKeys = internalInformation.Keys;
        foreach (var key in allKeys)
            toReturn += string.Format(",{0}={1}", key, internalInformation[key]);

        return toReturn;
    }

    public uniqueTileIndex GetUniqueKey()
    {
        return new uniqueTileIndex(Position);
    }

    public string this[string key]
    {
        get
        {
            if (internalInformation.ContainsKey(key))
                return internalInformation[key];
            return string.Empty;
        }
    }
}
