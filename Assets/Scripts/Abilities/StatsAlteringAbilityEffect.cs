using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "StatsAlterEffect", menuName = "Create Stats Altering Ability Effect")]
class StatsAlteringAbilityEffect : AbilityEffect
{
    public int duration;
    public int strength;
    public int defense;
    public int intelligence;
    public int dexterity;
    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        
        for (int i = target.Data.content.Count - 1; i >= 0; i--)
        {
            StatsAlteringStatus se = new StatsAlteringStatus(duration, strength, defense, intelligence, dexterity);
            se.Apply(target.Data.content[i] as Unit);
        }
    }
    
    public override string Summary(Unit unit)
    {
        List<string> stats = new List<string>();
        
        if (strength != 0)
        {
            stats.Add($"{strength} STR");
        }
        if (defense != 0)
        {
            stats.Add($"{defense} DEF");
        }
        if (intelligence != 0)
        {
            stats.Add($"{intelligence} INT");
        }
        if (dexterity != 0)
        {
            stats.Add($"{dexterity:0.##\\%} DEX");
        }

        string statsString = String.Join(", ", stats);
        string text = $"+<b>{statsString}</b> ({duration} turns)";
        return text;
    }
}