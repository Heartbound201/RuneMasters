using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class PlayerUnit : Unit
{
    public List<Rune> runes = new List<Rune>();

    public Party party;

    public override bool ExpandSearch(HexTile<Tile> from, HexTile<Tile> to)
    {
        return (from.Data._distance + 1) <= movement && (from.Data._distance + 1) <= party.AvailableMana &&
               to.Data.isPassable;
    }

    public override IEnumerator Move(List<HexTile<Tile>> tiles)
    {
        for (int i = 1; i < tiles.Count; ++i)
        {
            HexTile<Tile> from = tiles[i - 1];
            HexTile<Tile> to = tiles[i];
            transform.position = to.CartesianPosition;
            from.Data.unit = null;
            tile = to;
            tile.Data.unit = this;

            movement = Mathf.Clamp(movement - 1, 0, movementMax);
            party.SpendMana(1);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public override IEnumerator MoveRune(List<HexTile<Tile>> tiles)
    {
        foreach (HexTile<Tile> to in tiles)
        {
            transform.position = to.CartesianPosition;
            tile.Data.unit = null;
            tile = to;
            tile.Data.unit = this;

            movement = Mathf.Clamp(movement - 1, 0, movementMax);
            party.SpendMana(1);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        party.TakeDamage(amount - defense);
        Debug.Log(name + " is hit for " + (amount - defense));
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        party.Heal(amount);
        Debug.Log(name + " is healed for " + amount);
    }
}