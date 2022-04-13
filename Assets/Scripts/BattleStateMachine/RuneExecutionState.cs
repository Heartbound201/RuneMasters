using System.Collections;
using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class RuneExecutionState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Sequence(owner.SelectedRuneSteps));
    }

    IEnumerator Sequence(List<HexTile<Tile>> line)
    {
        yield return StartCoroutine(owner.ActingUnit.MoveRune(line)); // coroutine
        owner.SelectedAbility = owner.SelectedRune.RunePrototype.ability;
        owner.ChangeState<AbilityTargetState>();
    }
}