using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class ConfirmAbilityTargetState : State
{
    private List<HexTile<Tile>> tilesInArea = new List<HexTile<Tile>>();

    public override void Enter()
    {
        base.Enter();
        HighlightAbilityArea();
        owner.ShowHint("Confirm your target location");
    }

    public override void Exit()
    {
        base.Exit();
        Board.ClearHighlight();
        owner.HideHint();
    }

    private void HighlightAbilityArea()
    {
        owner.board.ClearHighlight();
        tilesInArea = owner.SelectedAbility.abilityArea.GetTilesInArea(Board, owner.ActingUnit.tile, owner.SelectedTile);
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
        InputController.CommandPause += owner.PauseOrResumeGame;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        Board.SelectTileEvent -= SelectTarget;
        InputController.CommandCancel -= Undo;
        InputController.CommandPause -= owner.PauseOrResumeGame;
    }

    private void SelectTarget(HexTile<Tile> obj)
    {
        if (!tilesInArea.Contains(obj)) return;
        owner.ChangeState<AbilityExecutionState>();
    }
}