using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "CircleRange", menuName = "Create Circle Ability Range")]
public class CircleAbilityRange : AbilityRange
{
    public int range = 1;
    public override List<HexTile<Tile>> GetTilesInRange(Unit unit, Board board)
    {
        return GetTilesInRange(unit.tile, board);
    }
    public override List<HexTile<Tile>> GetTilesInRange(HexTile<Tile> tile, Board board)
    {
        return board.SearchRange(tile, ExpandSearch);
    }
    bool ExpandSearch (HexTile<Tile> from, HexTile<Tile> to)
    {
        return (from.Data._distance + 1) <= range;
    }
    public override string Summary()
    {
		if (range == 0)
		{
			return $"Self";
		}

		return $"<b>{range}</b>";
    }
}