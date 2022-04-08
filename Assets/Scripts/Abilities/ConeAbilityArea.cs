using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
[CreateAssetMenu(fileName = "ConeArea", menuName = "Create Cone Ability Area")]
class ConeAbilityArea : AbilityArea
{
    [Range(0f, 180f)]
    public float coneAngle;
    public int coneLength;
    
    public override List<HexTile<Tile>> GetTilesInArea(Board board, HexTile<Tile> start, HexTile<Tile> target)
    {
        Vector3Int directionVector = HexGrid.TileDirectionVectors[(int) start.GetDirection(target)];
        return board.hexMap.GetTiles.Cone(start, directionVector, coneAngle, coneLength);
    }
    public override string Summary()
    {
        return $"Cone angle <b>{coneAngle}</b>, length <b>{coneLength}</b>";
    }
}