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

        owner.partyInfoMenuController.UpdatePartyInfo(owner.party);

        // change turn if
        // party mana <= 0
        // all units have acted
        if (owner.party.Units.TrueForAll(u => u.hasActed) || owner.party.AvailableMana <= 0)
        {
            owner.ChangeState<TurnSelectionState>();
        }

        // if turn has just started and no unit was selected yet, select the first in line
        if (owner.ActingUnit == null)
        {
            SelectCharacter(owner.party.Units.First());
        }
        
        // if the current acting unit has acted it cannot use tact movement even if it has movement left
        // select another one that can move or act
        else if (owner.ActingUnit.hasActed)
        {
            PlayerUnit playerUnit = owner.party.Units.Find(u => !u.hasActed || u.movement > 0);
            SelectCharacter((PlayerUnit) (playerUnit != null ? playerUnit : owner.ActingUnit));
        }
        
        // if the current acting unit has only moved and can act, keep it selected
        else
        {
            SelectCharacter((PlayerUnit) owner.ActingUnit);
        }

    }

    public override void Exit()
    {
        base.Exit();
        Board.ClearHighlight();

        owner.partyInfoMenuController.UpdatePartyInfo(owner.party);
    }


    protected override void AddListeners()
    {
        base.AddListeners();
        Board.SelectTileEvent += SelectTileForMovement;
        TurnMenuController.SelectUnit += SelectCharacter;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        Board.SelectTileEvent -= SelectTileForMovement;
        TurnMenuController.SelectUnit -= SelectCharacter;
    }

    private void SelectTileForMovement(HexTile<Tile> obj)
    {
        if (!tilesInRange.Contains(obj)) return;
        owner.SelectedTile = obj;
        owner.ChangeState<UnitMovementState>();
    }

    private void SelectCharacter(PlayerUnit unit)
    {
        owner.turnMenuController.SelectCharacter(unit);
        HighlightAllowedMovement(unit);
    }
    

    private void HighlightAllowedMovement(Unit unit)
    {
        Board.ClearHighlight();
        if(!unit.hasActed)
        {
            tilesInRange = unit.GetTilesInRange();
            Board.HighlightTiles(tilesInRange);
        }
    }
}