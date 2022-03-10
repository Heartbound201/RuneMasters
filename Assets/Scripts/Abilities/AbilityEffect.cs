using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public abstract class AbilityEffect : ScriptableObject
{
    public abstract void Apply(HexTile<Tile> target);
}