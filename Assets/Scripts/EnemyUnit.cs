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

    protected override void Die()
    {
        base.Die();
        tile.Data.unitList.Remove(this);
        KOEvent?.Invoke(this);
        Destroy(gameObject, 2f);
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, health);
        healthBar.UpdateHealth(currentHealth, health);
    }
}