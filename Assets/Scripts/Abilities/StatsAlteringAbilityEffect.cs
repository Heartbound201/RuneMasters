using System.Collections;
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
    public override IEnumerator Apply(Unit actor, HexTile<Tile> target)
    {
        yield return base.Apply(actor, target);
        
        for (int i = target.Data.unitList.Count - 1; i >= 0; i--)
        {
            StatsAlteringStatus se = new StatsAlteringStatus(duration, strength, defense, intelligence, dexterity);
            se.Apply(target.Data.unitList[i]);
        }
    }
}