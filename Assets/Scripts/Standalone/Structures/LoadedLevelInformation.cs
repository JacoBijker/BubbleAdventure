using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Structures
{
    public class LoadedLevelInformation
    {
        public LoadedLevelInformation()
        {
            ExcludedTiles = new List<TileType>();
            LoadedObjects = new List<GameObject>();
        }
        public List<TileType> ExcludedTiles { get; set; }
        public List<GameObject> LoadedObjects { get; set; }
    }
}
