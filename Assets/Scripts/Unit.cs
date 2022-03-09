using System;
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

    public Party party;

    public bool ExpandSearch(HexTile<Tile> from, HexTile<Tile> to)
    {
        return (from.Data._distance + 1) <= movement && (from.Data._distance + 1) <= party.AvailableMana;
    }
    
    public IEnumerator Move(List<HexTile<Tile>> tiles)
    {
        foreach (HexTile<Tile> tile in tiles)
        {
            transform.position = tile.CartesianPosition;
            standingTile.Data.unit = null;
            standingTile = tile;
            tile.Data.unit = this;
            // lower movement
            movement--;
            // lower mana
            party.mana--;
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    public virtual List<HexTile<Tile>> GetTilesInRange()
    {
        List<HexTile<Tile>> retValue = standingTile.Data.board.SearchRange(standingTile, ExpandSearch);
        Filter(retValue);
        return retValue;
    }
    
    protected virtual void Filter(List<HexTile<Tile>> tiles)
    {
        for (int i = tiles.Count - 1; i > 0; --i)
            if (tiles[i].Data.unit != null)
                tiles.RemoveAt(i);
    }
    
    public virtual List<HexTile<Tile>> FindPath(HexTile<Tile> tile)
    {
        List<HexTile<Tile>> targets = new List<HexTile<Tile>>();
        while (tile != null)
        {
            targets.Insert(0, tile);
            tile = tile.Data._prev;
        }
        return targets;
    }

    public void Reset()
    {
        movement = movementMax;
        hasActed = false;
    }
}