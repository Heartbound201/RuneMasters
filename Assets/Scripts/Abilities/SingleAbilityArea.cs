using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "SingleTileArea", menuName = "Create Single Tile Ability Area")]
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