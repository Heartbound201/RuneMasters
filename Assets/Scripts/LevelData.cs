using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Create Level Data")]
public class LevelData : ScriptableObject
{
    public int boardRadius = 5;
    public List<TileInfo> tiles = new List<TileInfo>();
    public List<SpawnInfo> enemies = new List<SpawnInfo>();
    public List<SpawnInfo> characters = new List<SpawnInfo>();

    public TilePrototype GetTilePrototypeAt(int index)
    {
        if (tiles.Exists(info => info.index == index))
        {
            return tiles.Find(info => info.index == index).proto;
        }

        return null;
    }

    public GameObject GetUnitAt(int index)
    {
        if (characters.Exists(info => info.index == index))
        {
            return characters.Find(info => info.index == index).obj;
        }

        return null;
    }

    public GameObject GetEnemyAt(int index)
    {
        if (enemies.Exists(info => info.index == index))
        {
            return enemies.Find(info => info.index == index).obj;
        }

        return null;
    }

    public void PlaceUnit(int index, GameObject obj)
    {
        if (characters.Exists(info => info.index == index))
        {
            var spawnInfo = characters.Find(info => info.index == index);
            if (obj == null)
            {
                characters.Remove(spawnInfo);
                return;
            }

            spawnInfo.obj = obj;
        }
        else
        {
            if (obj == null)
            {
                return;
            }
            characters.Add(new SpawnInfo()
            {
                index = index,
                obj = obj
            });
        }
    }

    public void PlaceEnemy(int index, GameObject obj)
    {
        if (enemies.Exists(info => info.index == index))
        {
            var spawnInfo = enemies.Find(info => info.index == index);
            if (obj == null)
            {
                enemies.Remove(spawnInfo);
                return;
            }

            spawnInfo.obj = obj;
        }
        else
        {
            if (obj == null)
            {
                return;
            }
            enemies.Add(new SpawnInfo()
            {
                index = index,
                obj = obj
            });
        }
    }

    public void SwapTilePrototype(int index, TilePrototype proto)
    {
        var tileInfo = tiles.Find(info => info.index == index);
        if (tileInfo != null)
        {
            tileInfo.proto = proto;
        }
        else
        {
            tiles.Add(new TileInfo()
            {
                index = index,
                proto = proto
            });
        }
    }
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