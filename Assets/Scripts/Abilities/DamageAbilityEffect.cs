using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Create Damage Ability Effect")]
public class DamageAbilityEffect : AbilityEffect
{
    public int potency = 1;
    public float strengthScaling;
    public float intelligenceScaling;
    public float dexterityScaling;

    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        for (int i = target.Data.unitList.Count - 1; i >= 0; i--)
        {
            int amount = Mathf.RoundToInt(potency + actor.strength * strengthScaling +
                                          actor.intelligence * intelligenceScaling +
                                          actor.dexterity * dexterityScaling);
            target.Data.unitList[i].TakeDamage(amount);
        }
    }
}