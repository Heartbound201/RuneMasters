using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public abstract class AbilityRange : MonoBehaviour
{
    public abstract List<HexTile<TileData>> GetTilesInRange (Board board);
}