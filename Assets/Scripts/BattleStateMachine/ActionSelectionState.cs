using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class ActionSelectionState : State
{
    private List<HexTile<Tile>> tilesInRange;
    
    public override void Enter()
    {
        base.Enter();
        if (TurnManager.currentTurn == TurnManager.Turn.Enemy)
        {
            StartCoroutine(AITurn());
        }
        else
        {
            OpenMenu();
            // select the first character
            owner.turnMenuController.SelectCharacter(owner.party.units.First());

        }
    }

    public override void Exit()
    {
        base.Exit();
        Board.ClearHighlight();
    }

    IEnumerator AITurn()
    {
        // Resolve prev turn action
        // Plan next attack
        yield return null;
        owner.ChangeState<AbilityExecutionState>();
    }
    
    protected void OpenMenu()
    {
        if(owner.turnMenuController == null)
        {
            Debug.LogError("There's no Menu Gameobject on ActionSelectionState.");
        }
        else
        {
            //TODO scroll menu up
        }
    }
    protected override void AddListeners()
    {
        base.AddListeners();
        Board.SelectTileEvent += SelectTileForMovement;
        TurnMenuController.SelectUnit += HighlightAllowedMovement;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        Board.SelectTileEvent -= SelectTileForMovement;
        TurnMenuController.SelectUnit -= HighlightAllowedMovement;
    }

    private void SelectTileForMovement(HexTile<Tile> obj)
    {
        if (!tilesInRange.Contains(obj)) return;
        owner.SelectedTile = obj;
        owner.ChangeState<UnitMovementState>();
    }

    private void HighlightAllowedMovement(Unit unit)
    {
        tilesInRange = unit.GetTilesInRange();
        Board.ClearHighlight();
        Board.HighlightTiles(tilesInRange);
    }
}