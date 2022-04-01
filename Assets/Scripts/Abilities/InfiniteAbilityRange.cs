using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;
[CreateAssetMenu(fileName = "InfiniteRange", menuName = "Create Infinite Ability Range")]
public class InfiniteAbilityRange : AbilityRange
{
    public override List<HexTile<Tile>> GetTilesInRange(HexTile<Tile> tile, Board board)
    {
        return board.hexMap.Tiles.ToList();
    }

    public override List<HexTile<Tile>> GetTilesInRange(Unit unit, Board board)
    {
        return GetTilesInRange(unit.tile, board);
    }
}