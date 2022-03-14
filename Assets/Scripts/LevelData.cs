using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Create Level Data")]
public class LevelData : ScriptableObject
{
    public int boardRadius;
    public List<TileInfo> tiles = new List<TileInfo>();
    public TileCollection tileCollection;
    public List<SpawnInfo> enemies = new  List<SpawnInfo>();
    public List<SpawnInfo> characters = new List<SpawnInfo>();
    // public List<Vector3> spawnableTiles = new List<Vector3>();
}

[Serializable]
public class SpawnInfo
{
    public int index;
    public GameObject obj;
}
[Serializable]
public class TileInfo
{
    public int index;
    public TilePrototype proto;
}