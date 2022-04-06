using System.Collections;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "DamageOvertimeEffect", menuName = "Create Damage Overtime OnTurnStart Ability Effect")]
class DamageOvertimeAbilityEffect : DamageAbilityEffect
{
    public int duration;

    public override IEnumerator Apply(Unit actor, HexTile<Tile> target)
    {
        yield return base.Apply(actor, target);
        for (int i = target.Data.unitList.Count - 1; i >= 0; i--)
        {
            DamageOvertimeOnTurnStartStatus se = new DamageOvertimeOnTurnStartStatus(duration, potency);
            se.Apply(target.Data.unitList[i]);
        }
    }
}