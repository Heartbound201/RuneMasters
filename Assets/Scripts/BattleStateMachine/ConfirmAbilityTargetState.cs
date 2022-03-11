using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class ConfirmAbilityTargetState : State
{
    private List<HexTile<Tile>> tilesInArea = new List<HexTile<Tile>>();

    public override void Enter()
    {
        base.Enter();
        HighlightAbilityArea();
    }

    public override void Exit()
    {
        base.Exit();
        Board.ClearHighlight();
    }

    private void HighlightAbilityArea()
    {
        owner.board.ClearHighlight();
        tilesInArea = owner.SelectedAbility.abilityArea.GetTilesInArea(Board, owner.SelectedTile);
        owner.board.HighlightTiles(tilesInArea);
    }

    private void Undo()
    {
        owner.SelectedTile = null;
        owner.ChangeState<AbilityTargetState>();
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        Board.SelectTileEvent += SelectTarget;
        InputController.CommandCancel += Undo;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        Board.SelectTileEvent -= SelectTarget;
        InputController.CommandCancel -= Undo;
    }

    private void SelectTarget(HexTile<Tile> obj)
    {
        if (!tilesInArea.Contains(obj)) return;
        owner.SelectedTile = obj;
        owner.ChangeState<AbilityExecutionState>();
    }
}