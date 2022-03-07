using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public abstract class AbilityArea : MonoBehaviour
{
    public abstract List<Tile> GetTilesInArea (Board board, HexTile<Tile> tile);
}