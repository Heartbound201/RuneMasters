using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class EnemyUnit : Unit
{
    public int health;
    
    public List<Ability> abilities = new List<Ability>();

    public AIPlan PlanAction()
    {
        AIPlan plan = new AIPlan(this);
        // pick random ability
        Ability ability = abilities[Random.Range(0, abilities.Count)];
        // pick random target
        Board board = tile.Data.board;
        List<HexTile<Tile>> validAttackLocations = new List<HexTile<Tile>>();
        foreach (var hexTileInRange in ability.abilityRange.GetTilesInRange(this, board))
        {
            foreach (var hexTileInArea in ability.abilityArea.GetTilesInArea(board, hexTileInRange))
            {
                if (hexTileInArea.Data.unit != null && hexTileInArea.Data.unit is PlayerUnit)
                {
                    validAttackLocations.Add(hexTileInRange);
                }
            }
        }
        if(validAttackLocations.Count > 0)
        {
            plan.ability = ability;
            plan.attackLocations = new List<HexTile<Tile>>()
                {validAttackLocations[Random.Range(0, validAttackLocations.Count)]};
        }
        return plan;
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        health -= amount - defense;
    }
    
    public override void Heal(int amount)
    {
        base.Heal(amount);
        health += amount;
    }
}
