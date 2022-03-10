using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public abstract class AbilityRange : ScriptableObject
{
    public abstract List<HexTile<Tile>> GetTilesInRange (Unit unit, Board board);
}