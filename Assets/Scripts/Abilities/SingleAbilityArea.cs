using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class SingleAbilityArea : AbilityArea
{
    public override List<HexTile<Tile>> GetTilesInArea(Board board, HexTile<Tile> tile)
    {
        List<HexTile<Tile>> retValue = new List<HexTile<Tile>>();
        if (tile != null)
            retValue.Add(tile);
        return retValue;
    }
}