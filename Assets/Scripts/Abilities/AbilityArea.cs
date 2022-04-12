using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public abstract class AbilityArea : ScriptableObject
{
    public abstract List<HexTile<Tile>> GetTilesInArea(Board board, HexTile<Tile> start, HexTile<Tile> target);
    public abstract IEnumerator Execute(Unit actor, HexTile<Tile> targetTile, List<AbilityEffect> abilityEffects);
    public abstract string Summary();
}