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
        owner.selectedUnit = null;
        if(TurnManager.IsOver()) TurnManager.Switch();
        if (TurnManager.currentTurn == TurnManager.Turn.Enemy)
        {
            SelectEnemyByOrder();
        }
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        Board.SelectTileEvent += SelectUnitByPosition;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        Board.SelectTileEvent -= SelectUnitByPosition;
    }

    public void SelectUnitByPosition(HexTile<TileData> selectedTile)
    {
        if(owner.turnManager.currentTurn == TurnManager.Turn.Enemy) return;
        
        if (selectedTile != null)
        {
            Debug.Log("clicked tile " + selectedTile);
            if (selectedTile.Data.Unit != null)
            {
                Debug.Log("clicked entity " + selectedTile.Data.Unit);
                if (selectedTile.Data.Unit.hasActed)
                {
                    Debug.Log("clicked entity has already acted this turn");
                    return;
                }
                owner.selectedUnit = selectedTile.Data.Unit;
                owner.ChangeState<UnitMovementState>();
            }
        }
    }

    public void SelectEnemyByOrder()
    {
        owner.selectedUnit = TurnManager.ServeEnemy();
        owner.ChangeState<AIActionState>();
    }
}