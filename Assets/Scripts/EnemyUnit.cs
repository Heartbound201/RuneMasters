using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;
using Random = UnityEngine.Random;

public class EnemyUnit : Unit
{
    public int health;
    [HideInInspector] public int currentHealth;
    public HealthBar healthBar;

    public List<Ability> abilities = new List<Ability>();

    public static event Action<Unit> KOEvent;

    void Start()
    {
        currentHealth = health;
        healthBar.UpdateHealth(currentHealth, health);
    }

    // public AIPlan PlanAction()
    // {
    //     AIPlan plan = new AIPlan(this);
    //     Board board = tile.Data.board;
    //     
    //     // get nearest enemy
    //     Unit nearestEnemy = GetNearestEnemy(this);
    //     // current distance from nearest enemy
    //     int distance = board.hexMap.GetTileDistance.Grid(tile.Position, nearestEnemy.tile.Position);
    //     
    //     // pick movement option
    //     HexTile<Tile> closestTargetMovement = tile;
    //     int distanceMovement = distance;
    //     foreach (var hexTileInRange in GetTilesInRange())
    //     {
    //         int d = board.hexMap.GetTileDistance.Grid(hexTileInRange.Position, nearestEnemy.tile.Position);
    //         if (d < distanceMovement)
    //         {
    //             distanceMovement = d;
    //             closestTargetMovement = hexTileInRange;
    //         }
    //     }
    //     plan.moveLocation = closestTargetMovement;
    //     plan.movePath = FindPath(closestTargetMovement);
    //
    //     // pick random ability
    //     Ability ability = abilities[Random.Range(0, abilities.Count)];
    //     
    //     // pick random target
    //     List<HexTile<Tile>> validAttackLocations = new List<HexTile<Tile>>();
    //     int distanceAttack = distance;
    //     HexTile<Tile> closestTargetAttack = null;
    //     foreach (var hexTileInRange in ability.abilityRange.GetTilesInRange(plan.moveLocation, board))
    //     {
    //         int d = board.hexMap.GetTileDistance.Grid(hexTileInRange.Position, nearestEnemy.tile.Position);
    //         if (d < distanceAttack)
    //         {
    //             distanceAttack = d;
    //             closestTargetAttack = hexTileInRange;
    //         }
    //
    //         foreach (var hexTileInArea in ability.abilityArea.GetTilesInArea(board, hexTileInRange))
    //         {
    //             if (hexTileInArea.Data.unitList.Count > 0 && hexTileInArea.Data.unitList is PlayerUnit)
    //             {
    //                 validAttackLocations.Add(hexTileInRange);
    //             }
    //         }
    //     }
    //
    //     if (validAttackLocations.Count <= 0)
    //     {
    //         validAttackLocations.Add(closestTargetAttack);
    //     }
    //
    //     plan.ability = ability;
    //     plan.attackLocation = validAttackLocations[Random.Range(0, validAttackLocations.Count)];
    //     StartCoroutine(Turn(plan.attackLocation));
    //     return plan;
    // }
    public AIPlan PlanAction()
    {
        float temp = Time.realtimeSinceStartup;
        int planCounter = 0;

        HexTile<Tile> start = tile;
        List<Ability> abilityOption = abilities;

        Unit nearestEnemy = GetNearestEnemy(this);
        Debug.Log("Nearest enemy is " + nearestEnemy);

        AIPlan bestPlan = new AIPlan(this, null, null, tile, nearestEnemy);
        planCounter++;

        // Evaluate every possible movement
        foreach (HexTile<Tile> moveOpt in GetTilesInRange())
        {
            // There may not be a useful ability to cast
            AIPlan planMoveOnly = new AIPlan(this, null, null, moveOpt, nearestEnemy);
            planCounter++;

            if (planMoveOnly.Evaluate() >= bestPlan.Evaluate())
            {
                bestPlan = planMoveOnly;
            }

            // Evaluate every possible ability
            foreach (Ability a in abilityOption)
            {
                PlaceOnTile(moveOpt);
                List<HexTile<Tile>> atkOptions = a.abilityRange.GetTilesInRange(moveOpt, moveOpt.Data.board);
                // Evaluate every possible target
                foreach (HexTile<Tile> atkOpt in atkOptions)
                {
                    AIPlan plan = new AIPlan(this, a, atkOpt, moveOpt, nearestEnemy);
                    planCounter++;
                    if (plan.Evaluate() > bestPlan.Evaluate())
                        bestPlan = plan;
                }
            }
        }

        PlaceOnTile(start);
        Debug.LogFormat("[AI {0}] {1} plans explored in {2}. Best plan's score: {3}",
            this, planCounter, (Time.realtimeSinceStartup - temp), bestPlan.Evaluate());
        return bestPlan;
    }

    public Unit GetNearestEnemy(Unit from)
    {
        Unit nearestEnemy = null;
        tile.Data.board.SearchRange(from.tile, delegate(HexTile<Tile> arg1, HexTile<Tile> arg2)
        {
            if (nearestEnemy == null && arg2.Data.unitList.Count > 0)
            {
                Unit other = arg2.Data.unitList[0];
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

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        tile.Data.unitList.Remove(this);
        KOEvent?.Invoke(this);
        Destroy(gameObject);
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, health);
        healthBar.UpdateHealth(currentHealth, health);
    }
}