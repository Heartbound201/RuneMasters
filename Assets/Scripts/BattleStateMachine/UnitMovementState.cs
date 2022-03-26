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

    private IEnumerator Sequence(List<HexTile<Tile>> line)
    {
        yield return StartCoroutine(owner.ActingUnit.MoveTact(line));
        owner.ChangeState<ActionSelectionState>();
    }
}