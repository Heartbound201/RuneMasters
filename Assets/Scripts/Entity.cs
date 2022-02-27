using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class Entity : MonoBehaviour
{
    public HexTile<TileData> tile;
    public bool isPassable;
    public bool hasActed;
    public List<ItemPrototype> items = new List<ItemPrototype>();
    public List<AbilityPrototype> abilities = new List<AbilityPrototype>();
    
    public void MoveTo(List<HexTile<TileData>> line)
    {
        StartCoroutine(Move(line));
        // hasActed = true;
    }

    private IEnumerator Move(List<HexTile<TileData>> vector3Ints)
    {
        foreach (HexTile<TileData> vector3Int in vector3Ints)
        {
            Debug.Log("move to " + vector3Int.CartesianPosition);
            transform.position = vector3Int.CartesianPosition;
            tile.Data.content.Remove(this);
            tile = vector3Int;
            vector3Int.Data.content.Add(this);
            yield return new WaitForSeconds(0.5f);
        }
    }
}