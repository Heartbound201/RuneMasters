using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public int boardRadius;
    public TileCollection tileCollection;
    public Dictionary<Vector3, Unit> enemies = new Dictionary<Vector3, Unit>();
    public List<Vector3> spawnableTiles = new List<Vector3>();
        
}