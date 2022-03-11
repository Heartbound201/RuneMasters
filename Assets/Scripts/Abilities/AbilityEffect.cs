using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public abstract class AbilityEffect : ScriptableObject
{
    public abstract void Apply(Unit actor, HexTile<Tile> target);
}