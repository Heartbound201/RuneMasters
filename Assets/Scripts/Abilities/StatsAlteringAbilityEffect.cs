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
        if(target.Data.unit)
        {
            StatsAlteringStatus se = new StatsAlteringStatus(duration, strength, defense, intelligence, dexterity);
            se.Apply(target.Data.unit);
        }
    }
}