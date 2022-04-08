using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    public IEnumerator Execute(Unit actor, HexTile<Tile> targetTile)
    {
        List<HexTile<Tile>> tilesInArea = abilityArea.GetTilesInArea(targetTile.Data.board, actor.tile, targetTile);
        foreach (AbilityEffect abilityEffect in abilityEffects)
        {
            foreach (HexTile<Tile> tile in tilesInArea)
            {
                yield return abilityEffect.Apply(actor, tile);
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
                    abilityValue += 50;
                }
                else if (targetTile.Data.unitList.Count > 0 && targetTile.Data.unitList[0] is EnemyUnit)
                {
                    abilityValue += 50;
                }
                else if (targetTile.Data.unitList.Count > 0 && targetTile.Data.unitList[0] is PlayerUnit)
                {
                    abilityValue -= 50;
                }
            }
            else
            {
                if (actor.tile == targetTile)
                {
                    abilityValue -= 50;
                }
                else if (targetTile.Data.unitList.Count > 0 && targetTile.Data.unitList[0] is EnemyUnit)
                {
                    abilityValue -= 50;
                }
                else if (targetTile.Data.unitList.Count > 0 && targetTile.Data.unitList[0] is PlayerUnit)
                {
                    abilityValue += 50;
                }
            }
            
        }

        return abilityValue;
    }

    public string Summary()
    {
        List<string> effectTexts = new List<string>();
        abilityEffects.ForEach(effect => effectTexts.Add(effect.Summary()));
        return $"Range: {abilityRange.Summary()}" +
               $"\nEffects: {String.Join(", ", effectTexts)}" +
               $"\nArea: {abilityArea.Summary()}";
    }
}
