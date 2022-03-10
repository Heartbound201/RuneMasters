using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class SelfAbilityRange : AbilityRange
{
    public override List<HexTile<Tile>> GetTilesInRange (Unit unit, Board board)
    {
        List<HexTile<Tile>> retValue = new List<HexTile<Tile>>(1);
        retValue.Add(unit.tile);
        return retValue;
    }
}