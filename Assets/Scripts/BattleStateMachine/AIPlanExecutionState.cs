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
            foreach (HexTile<Tile> aiPlanAttackLocation in aiPlan.attackLocations)
            {
                aiPlan.ability.Execute(aiPlan.actor, aiPlanAttackLocation);
                foreach (HexTile<Tile> tile in aiPlan.ability.abilityArea.GetTilesInArea(Board, aiPlanAttackLocation))
                {
                    tile.Data.SolveDanger(aiPlan);
                }
            }
            
            yield return null;
        }
        
        owner.enemyPlans.Clear();
        owner.IsBattleOver();

        // TODO Move

        // Plan next attack
        foreach (EnemyUnit enemy in owner.enemies)
        {
            owner.enemyPlans.Add(enemy.PlanAction());
            yield return null;
        }

        foreach (AIPlan enemyPlan in owner.enemyPlans)
        {
            foreach (var attackLocation in enemyPlan.attackLocations)
            {
                foreach (HexTile<Tile> tile in enemyPlan.ability.abilityArea.GetTilesInArea(Board, attackLocation))
                {
                    tile.Data.Endanger(enemyPlan);
                }
            }
        }

        owner.ChangeState<TurnSelectionState>();
    }
}