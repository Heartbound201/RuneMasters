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
    public Unit nearestEnemy;

    public AIPlan(Unit unit)
    {
        this.actor = unit;
    }

    public AIPlan(Unit actor, Ability ability, HexTile<Tile> attackLocation, HexTile<Tile> moveLocation,
        Unit nearestEnemy)
    {
        this.actor = actor;
        this.ability = ability;
        this.attackLocation = attackLocation;
        this.moveLocation = moveLocation;
        this.nearestEnemy = nearestEnemy;
    }

    public int Evaluate()
    {
        int value = 0;
        var dataBoard = moveLocation.Data.board;
        int currentDistance = dataBoard.hexMap.GetTileDistance.Grid(actor.tile.Position, nearestEnemy.tile.Position);
        if (moveLocation != null)
        {
            int afterMovementDistance = dataBoard.hexMap.GetTileDistance.Grid(moveLocation.Position, nearestEnemy.tile.Position);
            // add value the closer you get to the enemy
            value += (currentDistance - afterMovementDistance) * 10;
            // subtract value if standing on endangered tile 
            value -= moveLocation.Data.dangerList.Count * 100;
        }

        if (ability != null && attackLocation != null)
        {
            int attackDistance = dataBoard.hexMap.GetTileDistance.Grid(attackLocation.Position, nearestEnemy.tile.Position);
            // add value the closer you get to the enemy
            value += (currentDistance - attackDistance) * 10;
            var tilesInArea = ability.abilityArea.GetTilesInArea(dataBoard, actor.tile, attackLocation);
            Debug.Log(tilesInArea.Count);
            foreach (var hexTileInArea in tilesInArea)
            {
                value += ability.Predict(actor, hexTileInArea);
            }
            Debug.Log("value after tiles in area: " + value);
        }

        return value;
    }
}