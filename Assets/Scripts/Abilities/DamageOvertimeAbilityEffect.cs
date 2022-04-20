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

    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        for (int i = target.Data.content.Count - 1; i >= 0; i--)
        {
            DamageOvertimeOnTurnStartStatus se = new DamageOvertimeOnTurnStartStatus(duration, potency);
            se.Apply(target.Data.content[i] as Unit);
        }
    }
    public override string Summary(Unit unit)
    {
        List<string> scalings = new List<string>();
        
        if (strengthScaling != 0)
        {
			if (unit.strength != 0)
			{
				scalings.Add($"+{unit.strength * strengthScaling}");
			}
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
        string text = $"<b>{potency}{scalingString}</b> Dot Dmg ({duration} turns)";
        return text;
    }
}