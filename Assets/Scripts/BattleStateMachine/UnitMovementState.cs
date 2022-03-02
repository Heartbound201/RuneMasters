using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class UnitMovementState : State
{
    protected override void AddListeners()
    {
        base.AddListeners();
        Board.SelectTileEvent += SelectTile;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        Board.SelectTileEvent -= SelectTile;
    }

    private void SelectTile(HexTile<TileData> selectedTile)
    {
        HexTile<TileData> origin = owner.selectedUnit.standingTile;
        List<HexTile<TileData>> line = owner.board.hexMap.GetTiles.Line(origin.Position, selectedTile.Position, false);
        Debug.LogFormat("from {0} to {1}. {2}", origin.Position, selectedTile.Position, line);
        StartCoroutine(Sequence(line));

        owner.ChangeState<UnitSelectionState>(); //TODO must be a coroutine aswell
    }

    IEnumerator Sequence(List<HexTile<TileData>> line)
    {
        yield return StartCoroutine(owner.selectedUnit.Move(line)); // coroutine
        owner.ChangeState<UnitSelectionState>();
    }
}