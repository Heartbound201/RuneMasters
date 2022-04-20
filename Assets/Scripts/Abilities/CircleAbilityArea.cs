using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "CircleArea", menuName = "Create Circle Ability Area")]
public class CircleAbilityArea : AbilityArea
{
    public int range = 1;

    public override List<HexTile<Tile>> GetTilesInArea(Board board, HexTile<Tile> start, HexTile<Tile> target)
    {
        return board.SearchRange(target, ExpandSearch);
    }

    public override IEnumerator Execute(Unit actor, HexTile<Tile> targetTile, List<AbilityEffect> abilityEffects)
    {
        List<HexTile<Tile>> tilesInArea = GetTilesInArea(targetTile.Data.board, actor.tile, targetTile);
        foreach (HexTile<Tile> tile in tilesInArea)
        {
            foreach (AbilityEffect abilityEffect in abilityEffects)
            {
                if (tile == targetTile)
                {
                    yield return abilityEffect.ApplyParticleEffectSelf(actor, tile);
                    yield return abilityEffect.ApplyParticleEffectTarget(actor, tile, 1 + range / 2);
                }
                abilityEffect.Apply(actor, tile);
            }
        }
    }

    public override string Summary()
    {
		if(range == 0)
		{
			return $"Single target";
		}

        return $"Circle radius <b>{range}</b>";
    }

    bool ExpandSearch(HexTile<Tile> from, HexTile<Tile> to)
    {
        return (from.Data._distance + 1) <= range;
    }
}