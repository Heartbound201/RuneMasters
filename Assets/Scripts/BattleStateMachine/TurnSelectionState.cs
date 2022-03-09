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

    protected override void AddListeners()
    {
        base.AddListeners();
        // Board.SelectTileEvent += SelectUnitByPosition;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        // Board.SelectTileEvent -= SelectUnitByPosition;
    }

    public IEnumerator ChangeCurrentTurn()
    {
        TurnManager.Round().MoveNext();
        yield return null;
        owner.ChangeState<ActionSelectionState>();
    }

}