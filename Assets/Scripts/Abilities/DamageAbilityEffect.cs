using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Create Damage Ability Effect")]
public class DamageAbilityEffect : AbilityEffect
{
    public int potency = 1;
    
    public override void Apply(HexTile<Tile> target)
    {
        if(target.Data.unit)
        {
            Debug.Log(target.Data.unit + " takes " + potency + " damage");
        }
    }
}