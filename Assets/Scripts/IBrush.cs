using System;
using UnityEngine;
using UnityEngine.Events;
using Wunderwunsch.HexMapLibrary.Generic;

public interface IBrush
{
    void Select(bool value);
    void Paint(LevelData levelData, HexTile<Tile> tile, Transform boardParent);
    void AddButtonListener(UnityAction action);
}