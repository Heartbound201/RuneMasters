using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class AbilityTargetState : State
{
    private List<HexTile<Tile>> tilesInRange = new List<HexTile<Tile>>();

    public override void Enter()
    {
        base.Enter();
        HighlightAbilityRange();
        owner.ShowHint("Select a tile to target");
    }

    public override void Exit()
    {
        base.Exit();
        owner.HideHint();
    }

    private void HighlightAbilityRange()
    {
        owner.board.ClearHighlight();
        tilesInRange = owner.SelectedAbility.abilityRange.GetTilesInRange(owner.ActingUnit, Board);
        owner.board.HighlightTiles(tilesInRange);
    }

    private void Undo()
    {
        //TODO throw new System.NotImplementedException();
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
        if (!tilesInRange.Contains(obj)) return;
        owner.SelectedTile = obj;
        StartCoroutine(owner.ActingUnit.Turn(obj));
        owner.ChangeState<ConfirmAbilityTargetState>();
    }
}