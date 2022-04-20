using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class EnemyUnit : Unit
{
    public int health;
    [HideInInspector] public int currentHealth;
    public HealthBar healthBar;

    public List<Ability> abilities = new List<Ability>();

    public static event Action<Unit> KOEvent;

    void Start()
    {
        base.Start();
        currentHealth = health;
        healthBar.UpdateHealth(currentHealth, health);
    }

    public AIPlan PlanMovement()
    {
        float temp = Time.realtimeSinceStartup;
        int planCounter = 0;

        HexTile<Tile> start = tile;

        Unit nearestEnemy = GetNearestEnemy(this);
        Debug.Log("Nearest enemy is " + nearestEnemy);

        List<AIPlan> aiPlans = new List<AIPlan>();
        AIPlan firstPlan = new AIPlan(this, null, null, tile, nearestEnemy);
        aiPlans.Add(firstPlan);
        planCounter++;
        
        // Evaluate every possible movement
        foreach (HexTile<Tile> moveOpt in GetTilesInRange())
        {
            // There may not be a useful ability to cast
            AIPlan planMoveOnly = new AIPlan(this, null, null, moveOpt, nearestEnemy);
            planCounter++;
            aiPlans.Add(planMoveOnly);
        }
        PlaceOnTile(start);

        var bestPlan = aiPlans.OrderBy(plan => plan.Evaluate()).Last();
        Debug.LogFormat("[AI {0}] {1} plans explored in {2}. Best plan's score: {3}",
            this, planCounter, (Time.realtimeSinceStartup - temp), bestPlan.Evaluate());
        return bestPlan;
    }

    public AIPlan PlanAction()
    {
        float temp = Time.realtimeSinceStartup;
        int planCounter = 0;

        HexTile<Tile> start = tile;
        List<Ability> abilityOption = abilities;

        Unit nearestEnemy = GetNearestEnemy(this);
        Debug.Log("Nearest enemy is " + nearestEnemy);

        List<AIPlan> aiPlans = new List<AIPlan>();
        AIPlan firstPlan = new AIPlan(this, null, null, tile, nearestEnemy);
        aiPlans.Add(firstPlan);
        planCounter++;


        // Evaluate every possible ability
        foreach (Ability a in abilityOption)
        {
            PlaceOnTile(start);
            List<HexTile<Tile>> atkOptions = a.abilityRange.GetTilesInRange(start, start.Data.board);
            // Evaluate every possible target
            foreach (HexTile<Tile> atkOpt in atkOptions)
            {
                AIPlan plan = new AIPlan(this, a, atkOpt, start, nearestEnemy);
                planCounter++;
                aiPlans.Add(plan);
            }
        }

        PlaceOnTile(start);
        
        var bestPlan = aiPlans.OrderBy(plan => plan.Evaluate()).Last();
        Debug.Log(bestPlan);
        Debug.LogFormat("[AI {0}] {1} plans explored in {2}. Best plan's score: {3}",
            this, planCounter, (Time.realtimeSinceStartup - temp), bestPlan.Evaluate());
        return bestPlan;
    }

    public Unit GetNearestEnemy(Unit from)
    {
        Unit nearestEnemy = null;
        tile.Data.board.SearchRange(from.tile, delegate(HexTile<Tile> arg1, HexTile<Tile> arg2)
        {
            if (nearestEnemy == null && arg2.Data.content.Count > 0)
            {
                Unit other = arg2.Data.Unit;
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
        NumberDisplayManager.Instance.ShowNumber(-(amount - defense), transform, Color.red);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        base.Die();
        tile.Data.content.Remove(this);
        KOEvent?.Invoke(this);
        Destroy(gameObject, 2f);
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, health);
        healthBar.UpdateHealth(currentHealth, health);
        NumberDisplayManager.Instance.ShowNumber(amount, transform, Color.green);
    }
}