using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Collection", menuName = "Create Tile Collection")]
public class TileCollection : ScriptableObject
{
    public List<TilePrototype> tiles = new List<TilePrototype>();

    public TilePrototype GetRandomTile()
    {
        return tiles[Random.Range(0, tiles.Count)];
    }
}