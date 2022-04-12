using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Create Heal Ability Effect")]
public class HealEffect : AbilityEffect
{
    public int potency = 1;
    public float strengthScaling;
    public float intelligenceScaling;
    public float dexterityScaling;
    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        for (int i = target.Data.unitList.Count - 1; i >= 0; i--)
        {
            int amount = Mathf.RoundToInt(potency + actor.strength*strengthScaling + actor.intelligence*intelligenceScaling + actor.dexterity*dexterityScaling);
            target.Data.unitList[i].Heal(amount);
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
            scalings.Add($"{intelligenceScaling:0.##\\%} NT");
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
        string text = $"<b>{potency} {scalingString}</b> Healing";
        return text;
    }
}