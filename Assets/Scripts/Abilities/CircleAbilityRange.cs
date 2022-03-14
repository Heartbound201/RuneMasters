using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "CircleRange", menuName = "Create Circle Ability Range")]
public class CircleAbilityRange : AbilityRange
{
    public int range = 1;
    public override List<HexTile<Tile>> GetTilesInRange(Unit unit, Board board)
    {
        return board.SearchRange(unit.tile, ExpandSearch);
    }
    bool ExpandSearch (HexTile<Tile> from, HexTile<Tile> to)
    {
        return (from.Data._distance + 1) <= range;
    }
}