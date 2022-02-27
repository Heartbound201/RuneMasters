using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class UnitSelectionState : BaseState
{
    public override void EnterState()
    {
        base.EnterState();
        owner.selectedEntity = null;
        // switch (owner.turnManager.currentTurn)
        // {
        //     case TurnManager.Turn.Player:
        //         break;
        //     case TurnManager.Turn.Enemy:
        //         SelectEnemyByOrder();
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }
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
        // if(owner.turnManager.currentTurn == TurnManager.Turn.Enemy) return;
        
        if (selectedTile != null)
        {
            Debug.Log("clicked tile " + selectedTile);
            if (selectedTile.Data.Entity != null)
            {
                Debug.Log("clicked entity " + selectedTile.Data.Entity);
                if (selectedTile.Data.Entity.hasActed)
                {
                    Debug.Log("clicked entity has already acted this turn");
                    return;
                }
                owner.selectedEntity = selectedTile.Data.Entity;
                owner.ChangeState(new UnitMovementState());
            }
        }
    }

    public void SelectEnemyByOrder()
    {
        
    }
}