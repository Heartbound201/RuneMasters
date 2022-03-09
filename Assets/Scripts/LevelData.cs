using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Create Level Data")]
public class LevelData : ScriptableObject
{
    public int boardRadius;
    public TileCollection tileCollection;
    public Dictionary<int, Unit> enemies = new Dictionary<int, Unit>();
    // public List<Vector3> spawnableTiles = new List<Vector3>();

    public List<SpawnInfo> characters = new List<SpawnInfo>();
}

[Serializable]
public class SpawnInfo
{
    public int index;
    public GameObject obj;
}