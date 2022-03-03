using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class Unit : MonoBehaviour
{
    public int health;
    public int movement;
    public int movementLeft;
    public Alliance alliance;
    public int size;
    public bool isPassable;
    public bool hasActed;
    public HexTile<TileData> standingTile;
    public List<RunePrototype> runes = new List<RunePrototype>();
    

    public IEnumerator Move(List<HexTile<TileData>> vector3Ints)
    {
        foreach (HexTile<TileData> vector3Int in vector3Ints)
        {
            Debug.Log("move to " + vector3Int.CartesianPosition);
            transform.position = vector3Int.CartesianPosition;
            standingTile.Data.content.Remove(this);
            standingTile = vector3Int;
            vector3Int.Data.content.Add(this);
            yield return new WaitForSeconds(0.5f);
        }
    }
}