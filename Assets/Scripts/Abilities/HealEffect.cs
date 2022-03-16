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
        foreach (Unit unit in target.Data.unitList)
        {
            int amount = Mathf.RoundToInt(potency + actor.strength*strengthScaling + actor.intelligence*intelligenceScaling + actor.dexterity*dexterityScaling);
            unit.Heal(amount);
        }
    }
}