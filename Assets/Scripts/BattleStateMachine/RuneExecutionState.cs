using System.Collections;
using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class RuneExecutionState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Sequence(Board.GetPathTiles(owner.ActingUnit.tile, owner.SelectedRuneSteps)));
    }

    public override void Exit()
    {
        base.Exit();
        
        owner.partyInfoMenuController.UpdatePartyInfo(owner.party);
    }

    IEnumerator Sequence(List<HexTile<Tile>> line)
    {
        yield return StartCoroutine(owner.ActingUnit.MoveRune(line)); // coroutine
        owner.SelectedAbility = owner.SelectedRune.RunePrototype.ability;
        owner.ChangeState<AbilityTargetState>();
    }
}