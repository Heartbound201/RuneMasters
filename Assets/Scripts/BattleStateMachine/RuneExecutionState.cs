using System.Collections;
using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class RuneExecutionState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Sequence(Board.GetRuneTiles(owner.selectedRuneSteps, owner.ActingUnit.tile)));
    }

    public override void Exit()
    {
        base.Exit();
        
        owner.partyInfoMenuController.UpdatePartyInfo(owner.party);
    }

    IEnumerator Sequence(List<HexTile<Tile>> line)
    {
        yield return StartCoroutine(owner.ActingUnit.Move(line)); // coroutine
        owner.SelectedAbility = owner.SelectedRune.ability;
        owner.ChangeState<AbilityTargetState>();
    }
}