using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "DamageOvertimeEffect", menuName = "Create Damage Overtime Ability Effect")]
class DamageOvertimeAbilityEffect : DamageAbilityEffect
{
    public int duration;

    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        for (int i = target.Data.unitList.Count - 1; i >= 0; i--)
        {
            DamageOvertimeStatus se = new DamageOvertimeStatus(duration, potency);
            se.Apply(target.Data.unitList[i]);
        }
    }
}