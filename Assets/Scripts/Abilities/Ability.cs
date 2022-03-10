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
                abilityEffect.Apply(tile);
            }
        }
    }
}
