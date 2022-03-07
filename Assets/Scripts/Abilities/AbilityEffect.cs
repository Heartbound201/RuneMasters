using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public abstract class AbilityEffect : MonoBehaviour
{
    public abstract void Apply(HexTile<Tile> target);
}