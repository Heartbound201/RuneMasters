using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Create Damage Ability Effect")]
public class DamageAbilityEffect : AbilityEffect
{
    public int potency = 1;
    
    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        if(target.Data.unit)
        {
            target.Data.unit.TakeDamage(potency + actor.attack);
        }
    }
}