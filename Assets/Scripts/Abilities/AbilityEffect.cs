using System;
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using Wunderwunsch.HexMapLibrary.Generic;

public abstract class AbilityEffect : ScriptableObject
{
    [Header("ParticleEffects")] 
    public GameObject particleEffectSelf;
    public GameObject particleEffectProjectile;
    public GameObject particleEffectTarget;

    public virtual IEnumerator Apply(Unit actor, HexTile<Tile> target)
    {
        yield return ApplyParticleEffectSelf(actor, target);
        yield return ApplyParticleEffectProjectile(actor, target);
        yield return ApplyParticleEffectTarget(actor, target);
    }

    public virtual IEnumerator ApplyParticleEffectSelf(Unit actor, HexTile<Tile> target)
    {
        if (particleEffectSelf)
        {
            var gameObject = Instantiate(particleEffectSelf, actor.tile.Data.transform.position, Quaternion.identity);
            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
            float totalDuration = ps.duration + ps.startLifetime;
            yield return new WaitForSeconds(totalDuration);
        }
    }
    public virtual IEnumerator ApplyParticleEffectProjectile(Unit actor, HexTile<Tile> target)
    {
        if (particleEffectProjectile)
        {
            var gameObject = Instantiate(particleEffectProjectile, actor.tile.Data.transform);
            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
            float totalDuration = ps.duration + ps.startLifetime;
            yield return new WaitForSeconds(totalDuration);
            
            // TODO handle trajectory
        }
    }
    public virtual IEnumerator ApplyParticleEffectTarget(Unit actor, HexTile<Tile> target)
    {
        if (particleEffectTarget)
        {
            var gameObject = Instantiate(particleEffectTarget, target.Data.transform.position, Quaternion.identity);
            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
            float totalDuration = ps.duration + ps.startLifetime;
            yield return new WaitForSeconds(totalDuration);
            
        }
    }

    public abstract string Summary();
}

class ObstacleSpawningAbilityEffect : AbilityEffect
{
    public override string Summary()
    {
        throw new NotImplementedException();
    }
}