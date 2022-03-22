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
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        Board.SelectTileEvent -= SelectTarget;
        InputController.CommandCancel -= Undo;
    }

    private void SelectTarget(HexTile<Tile> obj)
    {
        if (!tilesInRange.Contains(obj)) return;
        owner.SelectedTile = obj;
        TileDirection tileDirection = owner.ActingUnit.tile.GetDirection(obj);
        if (owner.ActingUnit.direction != tileDirection)
            StartCoroutine(owner.ActingUnit.Turn(tileDirection));
        owner.ChangeState<ConfirmAbilityTargetState>();
    }
}