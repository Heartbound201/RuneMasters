using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "New Ability", menuName = "Create Ability")]
public class Ability : ScriptableObject
{
    public string name;
    public string description;
    public AbilityArea abilityArea;
    public AbilityRange abilityRange;
    public List<AbilityEffect> abilityEffects = new List<AbilityEffect>();


    public void Execute(Unit actor, HexTile<Tile> targetTile)
    {
        List<HexTile<Tile>> tilesInArea = abilityArea.GetTilesInArea(targetTile.Data.board, targetTile);
        foreach (AbilityEffect abilityEffect in abilityEffects)
        {
            foreach (HexTile<Tile> tile in tilesInArea)
            {
                abilityEffect.Apply(actor, tile);
            }
        }
    }

    public int Predict(Unit actor, HexTile<Tile> targetTile)
    {
        int abilityValue = 0;
        foreach (AbilityEffect abilityEffect in abilityEffects)
        {
            // TODO add abilityEffect.predict() : int
            if (abilityEffect is HealEffect)
            {
                if (actor.tile == targetTile)
                {
                    abilityValue += 10;
                }
                else if (targetTile.Data.unitList.Count > 0 && targetTile.Data.unitList[0] is PlayerUnit)
                {
                    abilityValue -= 10;
                }
            }
            else
            {
                if (actor.tile == targetTile)
                {
                    abilityValue -= 10;
                }
                else if (targetTile.Data.unitList.Count > 0 && targetTile.Data.unitList[0] is PlayerUnit)
                {
                    abilityValue += 10;
                }
            }
            
        }

        return abilityValue;
    }
}
