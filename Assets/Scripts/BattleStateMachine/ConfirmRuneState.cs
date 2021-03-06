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
        
        owner.ShowHint("Confirm your unit's rune movement");
    }

    public override void Exit()
    {
        base.Exit();
        owner.SelectedRuneSteps = selectedRuneTiles
            .Select(selectedRuneTile => Board.GetTile(selectedRuneTile)).ToList();
        
        owner.HideHint();
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
        InputController.CommandPause += owner.PauseOrResumeGame;
        TurnMenuController.SelectRune += SelectRune;
    }

    private void SelectRune(Rune obj)
    {
        owner.SelectedRune = obj;
        selectedRuneDirection = owner.SelectedRune.RunePrototype.steps[0];
        selectedRuneTiles = Board.GetPathTiles(owner.ActingUnit.tile.Position, owner.SelectedRune.RunePrototype.steps);
        HighlightRune();
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
        InputController.CommandPause -= owner.PauseOrResumeGame;
        TurnMenuController.SelectRune -= SelectRune;
    }

    private void HighlightRune()
    {
        Board.ClearHighlight();
        List<HexTile<Tile>> hexTiles = selectedRuneTiles
            .Select(selectedRuneTile => Board.GetTile(selectedRuneTile)).ToList();
        hexTiles.Add(owner.ActingUnit.tile);
        Board.HighlightTiles(hexTiles);
    }

    private void Undo()
    {
        // TODO on char button click, undo + select unit
        // TODO select unit with tile click
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
        // all tiles must be either passable, empty or containing only player units
        if (!hexTiles.TrueForAll(t =>
            t != null && (t.Data.IsPassable || (t.Data.content.Count > 0 && t.Data.content.TrueForAll(u => u is PlayerUnit))))) return;
        // the last tile must be empty
        if(hexTiles.Last().Data.content.Count > 0 && hexTiles.Last().Data.Unit != owner.ActingUnit) return;
        // the number of tiles must be equal to the number of steps required by the rune
        if (selectedRuneTiles.Count != owner.SelectedRune.RunePrototype.steps.Count) return;
        StartCoroutine(ExecuteRune());
    }

    private IEnumerator ExecuteRune()
    {
        yield return null;
        owner.ChangeState<RuneExecutionState>();
    }
}