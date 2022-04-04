using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class ConfirmRuneState : State
{
    private List<Vector3Int> selectedRuneTiles = new List<Vector3Int>();
    private TileDirection selectedRuneDirection;

    public override void Enter()
    {
        base.Enter();
        selectedRuneDirection = owner.SelectedRune.RunePrototype.steps[0];
        selectedRuneTiles = Board.GetPathTiles(owner.ActingUnit.tile.Position, owner.SelectedRune.RunePrototype.steps);
        HighlightRune();
    }

    public override void Exit()
    {
        base.Exit();
        owner.SelectedRuneSteps = selectedRuneTiles
            .Select(selectedRuneTile => Board.GetTile(selectedRuneTile)).ToList();
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        Board.SelectTileEvent += ConfirmRune;
        InputController.CommandRight += RotateRuneClockwise;
        InputController.CommandLeft += RotateRuneCounterClockwise;
        InputController.CommandConfirm += ConfirmRune;
        InputController.CommandCancel += Undo;
        InputController.CommandMirror += Mirror;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        Board.SelectTileEvent -= ConfirmRune;
        InputController.CommandRight -= RotateRuneClockwise;
        InputController.CommandLeft -= RotateRuneCounterClockwise;
        InputController.CommandConfirm -= ConfirmRune;
        InputController.CommandCancel -= Undo;
        InputController.CommandMirror -= Mirror;
    }

    private void HighlightRune()
    {
        Board.ClearHighlight();
        List<HexTile<Tile>> hexTiles = selectedRuneTiles
            .Select(selectedRuneTile => Board.GetTile(selectedRuneTile)).ToList();
        Board.HighlightTiles(hexTiles);
    }

    private void Undo()
    {
        owner.ChangeState<ActionSelectionState>();
    }

    private void RotateRuneClockwise()
    {
        selectedRuneDirection = selectedRuneDirection.Clockwise();
        selectedRuneTiles = Board.RotateClockwise(selectedRuneTiles, owner.ActingUnit.tile.Position);
        HighlightRune();
    }

    private void RotateRuneCounterClockwise()
    {
        selectedRuneDirection = selectedRuneDirection.CounterClockwise();
        selectedRuneTiles = Board.RotateCounterClockwise(selectedRuneTiles, owner.ActingUnit.tile.Position);
        HighlightRune();
    }


    private void Mirror()
    {
        selectedRuneTiles = Board.Reflect(selectedRuneTiles, owner.ActingUnit.tile.Position, selectedRuneDirection);
        HighlightRune();
    }

    private void ConfirmRune(HexTile<Tile> obj)
    {
        if (!selectedRuneTiles.Contains(obj.Position)) return;
        ConfirmRune();
    }

    private void ConfirmRune()
    {
        List<HexTile<Tile>> hexTiles = selectedRuneTiles
            .Select(selectedRuneTile => Board.GetTile(selectedRuneTile)).ToList();
        if (!hexTiles.TrueForAll(t => t != null && t.Data.IsPassable)) return;
        if (selectedRuneTiles.Count != owner.SelectedRune.RunePrototype.steps.Count) return;
        StartCoroutine(ExecuteRune());
    }

    private IEnumerator ExecuteRune()
    {
        yield return null;
        owner.ChangeState<RuneExecutionState>();
    }
}