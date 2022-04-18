using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class UnitMovementState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Sequence(owner.ActingUnit.FindPath(owner.SelectedTile)));
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
    private IEnumerator Sequence(List<HexTile<Tile>> line)
    {
        yield return StartCoroutine(owner.ActingUnit.MoveTact(line));
        owner.ChangeState<ActionSelectionState>();
    }
}