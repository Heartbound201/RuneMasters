using System.Collections;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

class ManaFillingAbilityEffect : AbilityEffect
{
    public int amount;
    public override IEnumerator Apply(Unit actor, HexTile<Tile> target)
    {
        yield return base.Apply(actor, target);
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