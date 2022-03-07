using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class UnitSelectionState : State
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

    public void SelectUnitByPosition(HexTile<Tile> selectedTile)
    {
        if(owner.turnManager.currentTurn == TurnManager.Turn.Enemy) return;
        
        if (selectedTile != null)
        {
            Debug.Log("clicked tile " + selectedTile);
            if (selectedTile.Data.unit != null)
            {
                Debug.Log("clicked entity " + selectedTile.Data.unit);
                if (selectedTile.Data.unit.hasActed)
                {
                    Debug.Log("clicked entity has already acted this turn");
                    return;
                }
                owner.ActingUnit = selectedTile.Data.unit;
                owner.ChangeState<UnitMovementState>();
            }
        }
    }

    public void SelectEnemyByOrder()
    {
        owner.ActingUnit = TurnManager.ServeEnemy();
        owner.ChangeState<AIActionState>();
    }
}