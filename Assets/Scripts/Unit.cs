using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class Unit : MonoBehaviour
{
    public string name;
    public int movement;
    public int movementMax;
    public Alliance alliance;
    public bool isPassable;
    public bool hasActed;
    public HexTile<Tile> standingTile;
    public List<RunePrototype> runes = new List<RunePrototype>();


    public bool ExpandSearch(Tile from, Tile to)
    {
        return (from._distance + 1) <= movement;
    }
    
    public IEnumerator Move(List<HexTile<Tile>> tiles)
    {
        foreach (HexTile<Tile> tile in tiles)
        {
            transform.position = tile.CartesianPosition;
            standingTile.Data.unit = null;
            standingTile = tile;
            tile.Data.unit = this;
            yield return new WaitForSeconds(0.5f);
        }
    }
}