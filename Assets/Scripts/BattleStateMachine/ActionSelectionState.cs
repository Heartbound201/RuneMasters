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

        if (owner.party.units.TrueForAll(u => u.hasActed) || owner.party.AvailableMana <= 0)
        {
            owner.ChangeState<TurnSelectionState>();
        }

        if (owner.ActingUnit == null)
        {
            owner.turnMenuController.SelectCharacter(owner.party.units.First());
        }
        else if (owner.ActingUnit.hasActed && owner.ActingUnit.movement <= 0)
        {
            owner.turnMenuController.SelectCharacter(owner.party.units.Find(u => !u.hasActed || u.movement > 0));
        }
        else
        {
            owner.turnMenuController.SelectCharacter((PlayerUnit) owner.ActingUnit);
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