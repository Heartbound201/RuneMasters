using System.Collections;
using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class AIPlanExecutionState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(AITurn());
    }

    IEnumerator AITurn()
    {
        // Resolve prev turn action
        foreach (AIPlan aiPlan in owner.enemyPlans)
        {
            yield return ExecuteAIPlan(aiPlan);
        }

        owner.enemyPlans.Clear();
        owner.IsBattleOver();

        // Plan next attack
        foreach (EnemyUnit enemy in owner.enemies)
        {
            owner.enemyPlans.Add(enemy.PlanAction());
            yield return null;
        }

        foreach (AIPlan enemyPlan in owner.enemyPlans)
        {
            AddAIPlanDangerToTiles(enemyPlan);
        }

        owner.ChangeState<TurnSelectionState>();
    }

    private IEnumerator ExecuteAIPlan(AIPlan aiPlan)
    {
        yield return StartCoroutine(aiPlan.actor.MoveTact(aiPlan.movePath));
        yield return StartCoroutine(aiPlan.actor.Act(aiPlan.ability, aiPlan.attackLocation));
        ClearAIPlanDangerFromTiles(aiPlan, aiPlan.attackLocation);
    }

    private void AddAIPlanDangerToTiles(AIPlan enemyPlan)
    {
        if (enemyPlan.ability == null) return;
        foreach (HexTile<Tile> tile in enemyPlan.ability.abilityArea.GetTilesInArea(Board, enemyPlan.attackLocation))
        {
            tile.Data.Endanger(enemyPlan);
        }
    }

    private void ClearAIPlanDangerFromTiles(AIPlan aiPlan, HexTile<Tile> aiPlanAttackLocation)
    {
        if (aiPlan.ability == null) return;
        foreach (HexTile<Tile> tile in aiPlan.ability.abilityArea.GetTilesInArea(Board, aiPlanAttackLocation))
        {
            tile.Data.SolveDanger(aiPlan);
        }
    }
}