using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wunderwunsch.HexMapLibrary.Generic;

public class ConfirmRuneState : State
{
    private List<HexTile<Tile>> runeTiles = new List<HexTile<Tile>>();

    public override void Enter()
    {
        base.Enter();
        owner.selectedRuneSteps = owner.SelectedRune.steps;
        HighlightRune();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void HighlightRune()
    {
        owner.board.ClearHighlight();
        runeTiles = Board.GetPathTiles(owner.ActingUnit.tile, owner.selectedRuneSteps);
        owner.board.HighlightTiles(runeTiles);
    }

    private void Undo()
    {
        owner.ChangeState<ActionSelectionState>();
    }

    private void RotateRuneClockwise()
    {
        owner.selectedRuneSteps = owner.selectedRuneSteps.Select(s => s.Clockwise()).ToList();
        
        HighlightRune();
    }

    private void RotateRuneCounterClockwise()
    {
        owner.selectedRuneSteps = owner.selectedRuneSteps.Select(s => s.CounterClockwise()).ToList();
        
        HighlightRune();
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        Board.SelectTileEvent += ConfirmRune;
        InputController.CommandRight += RotateRuneClockwise;
        InputController.CommandLeft += RotateRuneCounterClockwise;
        InputController.CommandConfirm += ConfirmRune;
        InputController.CommandCancel += Undo;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        Board.SelectTileEvent -= ConfirmRune;
        InputController.CommandRight -= RotateRuneClockwise;
        InputController.CommandLeft -= RotateRuneCounterClockwise;
        InputController.CommandConfirm -= ConfirmRune;
        InputController.CommandCancel -= Undo;
    }

    private void ConfirmRune(HexTile<Tile> obj)
    {
        if (!runeTiles.Contains(obj)) return;
        if (!runeTiles.TrueForAll(t => t.Data.IsPassable)) return;
        StartCoroutine(ExecuteRune());
    }

    private void ConfirmRune()
    {
        if (!runeTiles.TrueForAll(t => t.Data.IsPassable)) return;
        StartCoroutine(ExecuteRune());
    }

    private IEnumerator ExecuteRune()
    {
        yield return null;
        owner.ChangeState<RuneExecutionState>();
    }
}