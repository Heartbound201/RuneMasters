using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class EnemyUnit : Unit
{
    public int health;
    [HideInInspector] public int currentHealth;
    public HealthBar healthBar;

    public List<Ability> abilities = new List<Ability>();

    void Start()
    {
        currentHealth = health;
        healthBar.UpdateHealth(currentHealth, health);
    }

    public AIPlan PlanAction()
    {
        AIPlan plan = new AIPlan(this);
        // pick random ability
        Ability ability = abilities[Random.Range(0, abilities.Count)];
        // pick random target
        Board board = tile.Data.board;
        List<HexTile<Tile>> validAttackLocations = new List<HexTile<Tile>>();

        // get nearest enemy
        Unit nearestEnemy = GetNearestEnemy(this);
        Debug.Log(nearestEnemy.tile);
        Debug.Log(tile);
        // current distance from nearest enemy
        int distance = board.hexMap.GetTileDistance.Grid(tile.Position, nearestEnemy.tile.Position);
        HexTile<Tile> closestTarget = null;

        foreach (var hexTileInRange in ability.abilityRange.GetTilesInRange(this, board))
        {
            int d = board.hexMap.GetTileDistance.Grid(hexTileInRange.Position, nearestEnemy.tile.Position);
            if (d < distance)
            {
                distance = d;
                closestTarget = hexTileInRange;
            }

            foreach (var hexTileInArea in ability.abilityArea.GetTilesInArea(board, hexTileInRange))
            {
                if (hexTileInArea.Data.unit != null && hexTileInArea.Data.unit is PlayerUnit)
                {
                    validAttackLocations.Add(hexTileInRange);
                }
            }
        }

        if (validAttackLocations.Count <= 0)
        {
            validAttackLocations.Add(closestTarget);
        }

        plan.ability = ability;
        plan.attackLocations = new List<HexTile<Tile>>()
            {validAttackLocations[Random.Range(0, validAttackLocations.Count)]};
        return plan;
    }

    public Unit GetNearestEnemy(Unit from)
    {
        Unit nearestEnemy = null;
        tile.Data.board.SearchRange(from.tile, delegate(HexTile<Tile> arg1, HexTile<Tile> arg2)
        {
            if (nearestEnemy == null && arg2.Data.unit != null)
            {
                Unit other = arg2.Data.unit;
                if (other != null && other is PlayerUnit)
                {
                    Unit unit = other.GetComponent<Unit>();
                    nearestEnemy = unit;
                    return true;
                }
            }

            return nearestEnemy == null;
        });
        return nearestEnemy;
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        currentHealth = Mathf.Clamp(currentHealth - (amount - defense), 0, health);
        healthBar.UpdateHealth(currentHealth, health);
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, health);
        healthBar.UpdateHealth(currentHealth, health);
    }
}