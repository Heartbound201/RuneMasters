using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "CircleArea", menuName = "Create Circle Ability Area")]
public class CircleAbilityArea : AbilityArea
{
    public int range = 1;
    public override List<HexTile<Tile>> GetTilesInArea(Board board, HexTile<Tile> start, HexTile<Tile> target)
    {
        return board.SearchRange(target, ExpandSearch);
    }

    public override string Summary()
    {
        return $"Circle radius <b>{range}</b>";
    }

    bool ExpandSearch (HexTile<Tile> from, HexTile<Tile> to)
    {
        return (from.Data._distance + 1) <= range;
    }
}