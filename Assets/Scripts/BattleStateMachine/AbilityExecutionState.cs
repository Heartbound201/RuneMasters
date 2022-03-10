using System.Collections;
using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary;

public class AbilityExecutionState : State
{
    public override void Enter()
    {
        base.Enter();
        if (TurnManager.currentTurn == TurnManager.Turn.Enemy)
            StartCoroutine(AITurn());
        else
            StartCoroutine(Execution());
    }
    
    public override void Exit()
    {
        base.Exit();
        owner.SelectedAbility = null;
        owner.SelectedTile = null;
        owner.SelectedRune = null;
        owner.selectedRuneSteps = new List<TileDirection>();
    }

    private IEnumerator AITurn()
    {
        throw new System.NotImplementedException();
    }
    
    IEnumerator Execution()
    {
        Ability a = owner.SelectedAbility;

        //owner.cameraRig.follow.Add(targetTile.transform);

        // if (owner.ActingUnit.standingTile != owner.SelectedTile)
        // {
        //     Directions dir = turn.actor.Tile.GetDirection(targetTile);
        //     //StartCoroutine(turn.actor.GetComponent<Movement>().Turn(dir));
        //     turn.actor.GetComponent<Movement>().Turn(dir);
        // }
        a.Execute(owner.ActingUnit, owner.SelectedTile);
        yield return null;
        owner.ActingUnit.hasActed = true;

        //owner.cameraRig.follow.Remove(targetTile.transform);

        // if (IsBattleOver())
        //     owner.ChangeState<EndBattleState>();
        owner.ActingUnit.hasActed = true;
        owner.ChangeState<TurnSelectionState>();
    }
}