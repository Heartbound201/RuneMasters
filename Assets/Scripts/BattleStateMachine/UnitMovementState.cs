using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class UnitMovementState : BaseState
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
        HexTile<TileData> origin = owner.selectedEntity.tile;
        List<HexTile<TileData>> line = owner.board.hexMap.GetTiles.Line(origin.Position, selectedTile.Position, false);
        Debug.LogFormat("from {0} to {1}. {2}", origin.Position, selectedTile.Position, line);
        owner.selectedEntity.MoveTo(line);
        owner.ChangeState(new UnitSelectionState());
    }
    
}
