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
        if(target.Data.unit)
        {
            int amount = Mathf.RoundToInt(potency + actor.strength*strengthScaling + actor.intelligence*intelligenceScaling + actor.dexterity*dexterityScaling);
            target.Data.unit.Heal(amount);
        }
    }
}