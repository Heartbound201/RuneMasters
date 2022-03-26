using System.Collections;
using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary;

public class AbilityExecutionState : State
{
    public override void Enter()
    {
        base.Enter();
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

    IEnumerator Execution()
    {
        Ability a = owner.SelectedAbility;

        yield return StartCoroutine(owner.ActingUnit.Act(a, owner.SelectedTile));
        
        CameraController.instance.CameraLookAt(owner.SelectedTile);
        
        owner.IsBattleOver();
        owner.ChangeState<ActionSelectionState>();
    }
}