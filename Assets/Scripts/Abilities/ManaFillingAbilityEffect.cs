using System.Collections;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "ManaFillEffect", menuName = "Create Mana Fill Ability Effect")]
class ManaFillingAbilityEffect : AbilityEffect
{
    public int amount;

    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        Unit unit = target.Data.Unit;
        if (unit is PlayerUnit playerUnit)
        {
            playerUnit.party.FillMana(amount);
        }
    }

    public override string Summary(Unit unit)
    {
        string text = $"+<b>{amount}</b> current Mana";
        return text;
    }
}