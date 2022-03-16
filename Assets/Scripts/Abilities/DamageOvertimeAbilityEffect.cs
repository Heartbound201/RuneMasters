using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "DamageOvertimeEffect", menuName = "Create Damage Overtime Ability Effect")]
class DamageOvertimeAbilityEffect : DamageAbilityEffect
{
    public int duration;

    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        foreach (Unit unit in target.Data.unitList)
        {
            DamageOvertimeStatus se = new DamageOvertimeStatus(duration, potency);
            se.Apply(unit);
        }
    }
}