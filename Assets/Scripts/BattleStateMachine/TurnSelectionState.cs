using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class TurnSelectionState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(ChangeCurrentTurn());
    }

    public IEnumerator ChangeCurrentTurn()
    {
        TurnManager.Round().MoveNext();
        yield return null;
        if (TurnManager.currentTurn == TurnManager.Turn.Player)
        {
            owner.ChangeState<ActionSelectionState>();
        }
        else
        {
            owner.ChangeState<AIPlanExecutionState>();
        }
    }
}