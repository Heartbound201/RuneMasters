using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class InfiniteAbilityRange : AbilityRange
{
    public override List<HexTile<Tile>> GetTilesInRange(HexTile<Tile> tile, Board board)
    {
        throw new System.NotImplementedException();
    }

    public override List<HexTile<Tile>> GetTilesInRange(Unit unit, Board board)
    {
        return GetTilesInRange(unit.tile, board);
    }
}