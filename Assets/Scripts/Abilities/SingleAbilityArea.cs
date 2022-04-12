using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "SingleTileArea", menuName = "Create Single Tile Ability Area")]
public class SingleAbilityArea : AbilityArea
{
    public override List<HexTile<Tile>> GetTilesInArea(Board board, HexTile<Tile> start, HexTile<Tile> target)
    {
        List<HexTile<Tile>> retValue = new List<HexTile<Tile>>();
        if (target != null)
            retValue.Add(target);
        return retValue;
    }

    public override IEnumerator Execute(Unit actor, HexTile<Tile> targetTile, List<AbilityEffect> abilityEffects)
    {
        List<HexTile<Tile>> tilesInArea = GetTilesInArea(targetTile.Data.board, actor.tile, targetTile);
        foreach (HexTile<Tile> tile in tilesInArea)
        {
            foreach (AbilityEffect abilityEffect in abilityEffects)
            {
                yield return abilityEffect.ApplyParticleEffectSelf(actor, tile);
                yield return abilityEffect.ApplyParticleEffectTarget(actor, tile);
                abilityEffect.Apply(actor, tile);
            }
        }
    }

    public override string Summary()
    {
        return $"Single";
    }
}