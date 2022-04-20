using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using Wunderwunsch.HexMapLibrary.Generic;

public abstract class AbilityEffect : ScriptableObject
{
    [Header("ParticleEffects")] 
    public GameObject particleEffectSelf;
    public GameObject particleEffectTarget;

    public abstract void Apply(Unit actor, HexTile<Tile> target);

    public virtual IEnumerator ApplyParticleEffectSelf(Unit actor, HexTile<Tile> target, float scale = 1f)
    {
        if (particleEffectSelf)
        {
            var particleGO = Instantiate(particleEffectSelf, actor.tile.Data.transform);
            particleGO.transform.localScale = Vector3.one * scale;
            ParticleSystem ps = particleGO.GetComponent<ParticleSystem>();
            float totalDuration = ps.duration + ps.startLifetime;
            yield return null;
            // yield return new WaitForSeconds(totalDuration);
        }
    }
    public virtual IEnumerator ApplyParticleEffectTarget(Unit actor, HexTile<Tile> target, float scale = 1f)
    {
        if (particleEffectTarget)
        {
            var particleGO = Instantiate(particleEffectTarget, target.Data.transform);
            particleGO.transform.localScale = Vector3.one * scale;
            ParticleSystem ps = particleGO.GetComponent<ParticleSystem>();
            float totalDuration = ps.duration + ps.startLifetime;
            yield return null;
            // yield return new WaitForSeconds(totalDuration);
            
        }
    }

    public abstract string Summary();
}