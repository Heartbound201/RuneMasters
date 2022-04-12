using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
[CreateAssetMenu(fileName = "ConeArea", menuName = "Create Cone Ability Area")]
class ConeAbilityArea : AbilityArea
{
    [Range(0f, 180f)]
    public float coneAngle;
    public int coneLength;
    
    public override List<HexTile<Tile>> GetTilesInArea(Board board, HexTile<Tile> start, HexTile<Tile> target)
    {
        Vector3Int directionVector = HexGrid.TileDirectionVectors[(int) start.GetDirection(target)];
        return board.hexMap.GetTiles.Cone(start, directionVector, coneAngle, coneLength);
    }

    public override IEnumerator Execute(Unit actor, HexTile<Tile> targetTile, List<AbilityEffect> abilityEffects)
    {
        List<HexTile<Tile>> tilesInArea = GetTilesInArea(targetTile.Data.board, actor.tile, targetTile);
        foreach (HexTile<Tile> tile in tilesInArea)
        {
            foreach (AbilityEffect abilityEffect in abilityEffects)
            {
                if (coneAngle > 30f)
                {
                    if(tile.Data.unitList.Count > 0 && tile.Data.unitList[0] != null)
                    {
                        yield return abilityEffect.ApplyParticleEffectSelf(actor, tile);
                        yield return abilityEffect.ApplyParticleEffectTarget(actor, tile);
                    }
                }
                else
                {
                    yield return abilityEffect.ApplyParticleEffectSelf(actor, tile);
                    yield return abilityEffect.ApplyParticleEffectTarget(actor, tile);
                }
                abilityEffect.Apply(actor, tile);
            }
        }
    }

    public override string Summary()
    {
        return $"Cone angle <b>{coneAngle}</b>, length <b>{coneLength}</b>";
    }
}