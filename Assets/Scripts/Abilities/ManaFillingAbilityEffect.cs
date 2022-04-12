using System.Collections;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "ManaFillEffect", menuName = "Create Mana Fill Ability Effect")]
class ManaFillingAbilityEffect : AbilityEffect
{
    public int amount;
    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        for (int i = target.Data.unitList.Count - 1; i >= 0; i--)
        {
            Unit unit = target.Data.unitList[i];
            if (unit is PlayerUnit playerUnit)
            {
                playerUnit.party.FillMana(amount);
            }
        }
    }

    public override string Summary()
    {
        string text = $"Mana filling <b>{amount}</b> effect";
        return text;
    }
}