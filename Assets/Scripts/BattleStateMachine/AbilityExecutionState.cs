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

        a.Execute(owner.ActingUnit, owner.SelectedTile);
        yield return null;
        owner.ActingUnit.hasActed = true;

        //TODO check gameover
        owner.ChangeState<ActionSelectionState>();
    }
}