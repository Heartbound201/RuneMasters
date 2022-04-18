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
    protected override void AddListeners()
    {
        base.AddListeners();
        InputController.CommandPause += owner.PauseOrResumeGame;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        InputController.CommandPause -= owner.PauseOrResumeGame;
    }
    IEnumerator AITurn()
    {
        // Resolve prev turn action
        for (int i = owner.enemyPlans.Count - 1; i >= 0; i--)
        {
            yield return ExecuteAIPlan(owner.enemyPlans[i]);
        }

        owner.enemyPlans.Clear();
        owner.IsBattleOver();

        foreach (EnemyUnit enemy in owner.enemies)
        {
            // Move
            AIPlan movePlan = enemy.PlanMovement();
            List<HexTile<Tile>> findPath = movePlan.actor.FindPath(movePlan.moveLocation);
            yield return StartCoroutine(movePlan.actor.MoveTact(findPath));

            // Plan next attack
            AIPlan actionPlan = enemy.PlanAction();
            owner.enemyPlans.Add(actionPlan);
            AddAIPlanDangerToTiles(actionPlan);
            yield return null;
        }

        owner.ChangeState<TurnSelectionState>();
    }

    private IEnumerator ExecuteAIPlan(AIPlan aiPlan)
    {
        yield return StartCoroutine(aiPlan.actor.Act(aiPlan.ability, aiPlan.attackLocation));
        ClearAIPlanDangerFromTiles(aiPlan, aiPlan.attackLocation);
    }

    private void AddAIPlanDangerToTiles(AIPlan enemyPlan)
    {
        if (enemyPlan.ability == null) return;
        foreach (HexTile<Tile> tile in enemyPlan.ability.abilityArea.GetTilesInArea(Board, enemyPlan.actor.tile,
            enemyPlan.attackLocation))
        {
            tile.Data.Endanger(enemyPlan);
        }
    }

    private void ClearAIPlanDangerFromTiles(AIPlan aiPlan, HexTile<Tile> aiPlanAttackLocation)
    {
        owner.enemyPlans.Remove(aiPlan);
        if (aiPlan.ability == null) return;
        foreach (HexTile<Tile> tile in aiPlan.ability.abilityArea.GetTilesInArea(Board, aiPlan.actor.tile,
            aiPlanAttackLocation))
        {
            tile.Data.SolveDanger(aiPlan);
        }
    }
}