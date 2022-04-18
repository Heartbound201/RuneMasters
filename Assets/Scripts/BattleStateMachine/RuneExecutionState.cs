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
    IEnumerator Sequence(List<HexTile<Tile>> line)
    {
        yield return StartCoroutine(owner.ActingUnit.MoveRune(line)); // coroutine
        owner.SelectedAbility = owner.SelectedRune.RunePrototype.ability;
        owner.ChangeState<AbilityTargetState>();
    }
}