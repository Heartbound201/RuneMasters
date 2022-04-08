using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
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
    public override string Summary()
    {
        List<string> scalings = new List<string>();
        
        if (strengthScaling != 0)
        {
            scalings.Add($"{strengthScaling:0.##\\%} STR");
        }
        if (intelligenceScaling != 0)
        {
            scalings.Add($"{intelligenceScaling:0.##\\%} INT");
        }
        if (dexterityScaling != 0)
        {
            scalings.Add($"{dexterityScaling:0.##\\%} DEX");
        }

        string scalingString = String.Join(", ", scalings);
        if (scalings.Count > 0)
        {
            scalingString = $"[{scalingString}]";
        }
        string text = $"<b>{potency} {scalingString}</b> Damage Overtime for {duration} turns";
        return text;
    }
}