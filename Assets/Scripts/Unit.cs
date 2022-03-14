using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Wunderwunsch.HexMapLibrary.Generic;

public class Unit : MonoBehaviour
{
    public string name;
    public int movement;
    public int movementMax;
    public bool isPassable;
    public bool hasActed;
    public HexTile<Tile> tile;
    public List<Status> statuses = new List<Status>();

    public int strength;
    public int defense;
    public int intelligence;
    public int dexterity;

    [HideInInspector] public int strengthStart;
    [HideInInspector] public int defenseStart;
    [HideInInspector] public int intelligenceStart;
    [HideInInspector] public int dexterityStart;

    private void Start()
    {
        strengthStart = strength;
        defenseStart = defense;
        intelligenceStart = intelligence;
        dexterityStart = dexterity;
    }

    public virtual bool ExpandSearch(HexTile<Tile> from, HexTile<Tile> to)
    {
        return (from.Data._distance + 1) <= movement && to.Data.isPassable;
    }

    public virtual IEnumerator Move(List<HexTile<Tile>> tiles)
    {
        foreach (HexTile<Tile> tile in tiles)
        {
            transform.position = tile.CartesianPosition;
            this.tile.Data.unit = null;
            this.tile = tile;
            tile.Data.unit = this;

            movement--;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public virtual List<HexTile<Tile>> GetTilesInRange()
    {
        List<HexTile<Tile>> retValue = tile.Data.board.SearchRange(tile, ExpandSearch);
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

    public virtual void TakeDamage(int amount)
    {
    }
    public virtual void Heal(int amount)
    {
    }
}