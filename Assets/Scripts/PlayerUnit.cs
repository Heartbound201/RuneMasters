using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class PlayerUnit : Unit
{
    public List<Rune> runes = new List<Rune>();

    public Party party;

    public override bool ExpandSearch(HexTile<Tile> from, HexTile<Tile> to)
    {
        return (from.Data._distance + 1) <= movement && (from.Data._distance + 1) <= party.AvailableMana &&
               to.Data.IsPassable;
    }

    public override IEnumerator Move(List<HexTile<Tile>> tiles)
    {
        for (int i = 1; i < tiles.Count; ++i)
        {
            TileDirection tileDirection = tile.GetDirection(tiles[i]);
            if (direction != tileDirection)
                yield return StartCoroutine(Turn(tileDirection));
            yield return StartCoroutine(Walk(tiles[i]));
            PlaceOnTile(tiles[i]);

            movement = Mathf.Clamp(movement - 1, 0, movementMax);
            party.SpendMana(1);
        }

        if (animator != null)
        {
            animator.Play("Idle");
        }
        yield return null;
    }

    public override IEnumerator MoveRune(List<HexTile<Tile>> tiles)
    {
        foreach (HexTile<Tile> to in tiles)
        {
            TileDirection tileDirection = tile.GetDirection(to);
            if (direction != tileDirection)
                yield return StartCoroutine(Turn(tileDirection));
            yield return StartCoroutine(Walk(to));
            PlaceOnTile(to);

            movement = Mathf.Clamp(movement - 1, 0, movementMax);
            party.SpendMana(1);
        }

        if (animator != null)
        {
            animator.Play("Idle");
        }
        yield return null;
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