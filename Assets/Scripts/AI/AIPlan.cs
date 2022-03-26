using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class AIPlan
{
    public Unit actor;
    public Ability ability;
    public HexTile<Tile> attackLocation;
    public HexTile<Tile> moveLocation;
    public List<HexTile<Tile>> movePath;
    public Unit nearestEnemy;
    
    public int Score { get { return Evaluate(); }}
    
    public AIPlan(Unit unit)
    {
        this.actor = unit;
    }

    public AIPlan(Unit actor, Ability ability, HexTile<Tile> attackLocation, HexTile<Tile> moveLocation,
        List<HexTile<Tile>> movePath, Unit nearestEnemy)
    {
        this.actor = actor;
        this.ability = ability;
        this.attackLocation = attackLocation;
        this.moveLocation = moveLocation;
        this.movePath = movePath;
        this.nearestEnemy = nearestEnemy;
    }

    public int Evaluate()
    {
        int value = -999;

        if (moveLocation != null)
        {                
            value += movePath.Count;
            int currentDistance = moveLocation.Data.board.hexMap.GetTileDistance.Grid(actor.tile.Position, nearestEnemy.tile.Position);
            int afterMovementDistance = moveLocation.Data.board.hexMap.GetTileDistance.Grid(moveLocation.Position, nearestEnemy.tile.Position);
            value += currentDistance - afterMovementDistance;
        }
        
        if (ability != null && attackLocation != null && attackLocation.Data.unitList.Count > 0)
        {
            foreach (var hexTileInArea in ability.abilityArea.GetTilesInArea(attackLocation.Data.board, attackLocation))
            {
                value += ability.Predict(actor, hexTileInArea);
            }
        }
        return value;
    }
}
