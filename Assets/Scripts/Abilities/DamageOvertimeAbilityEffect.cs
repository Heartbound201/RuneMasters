using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "DamageOvertimeEffect", menuName = "Create Damage Overtime Ability Effect")]
class DamageOvertimeAbilityEffect : DamageAbilityEffect
{
    public int duration;

    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        if(target.Data.unit)
        {
            DamageOvertimeStatus se = new DamageOvertimeStatus(duration, potency);
            se.Apply(target.Data.unit);
        }
    }
}