using System.Collections;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Create Heal Ability Effect")]
public class HealEffect : AbilityEffect
{
    public int potency = 1;
    public float strengthScaling;
    public float intelligenceScaling;
    public float dexterityScaling;
    public override IEnumerator Apply(Unit actor, HexTile<Tile> target)
    {
        yield return base.Apply(actor, target);
        for (int i = target.Data.unitList.Count - 1; i >= 0; i--)
        {
            int amount = Mathf.RoundToInt(potency + actor.strength*strengthScaling + actor.intelligence*intelligenceScaling + actor.dexterity*dexterityScaling);
            target.Data.unitList[i].Heal(amount);
        }
    }
}