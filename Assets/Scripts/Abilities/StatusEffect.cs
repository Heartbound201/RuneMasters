using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class StatusEffect : AbilityEffect
{
    public Status status;
    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        if(target.Data.unit)
        {
            status.Apply(target.Data.unit);
        }
    }
}