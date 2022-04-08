using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class FullAbilityArea : AbilityArea
{
    public override List<HexTile<Tile>> GetTilesInArea(Board board, HexTile<Tile> start, HexTile<Tile> target)
    {
        throw new System.NotImplementedException();
    }
    
    public override string Summary()
    {
        return $"Full range area";
    }
}