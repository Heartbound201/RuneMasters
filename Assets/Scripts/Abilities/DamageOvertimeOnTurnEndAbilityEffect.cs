using System.Collections;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;
[CreateAssetMenu(fileName = "DamageOvertimeEffect", menuName = "Create Damage Overtime OnTurnEnd Ability Effect")]
class DamageOvertimeOnTurnEndAbilityEffect : DamageAbilityEffect
{
    public int duration;

    public override IEnumerator Apply(Unit actor, HexTile<Tile> target)
    {
        yield return base.Apply(actor, target);
        for (int i = target.Data.unitList.Count - 1; i >= 0; i--)
        {
            DamageOvertimeOnTurnEndStatus se = new DamageOvertimeOnTurnEndStatus(duration, potency);
            se.Apply(target.Data.unitList[i]);
        }
    }
}