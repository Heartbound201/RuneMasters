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
            var gameObject = Instantiate(particleEffectSelf, actor.tile.Data.transform);
            gameObject.transform.localScale = Vector3.one * scale;
            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
            float totalDuration = ps.duration + ps.startLifetime;
            yield return new WaitForSeconds(totalDuration);
        }
    }
    public virtual IEnumerator ApplyParticleEffectTarget(Unit actor, HexTile<Tile> target, float scale = 1f)
    {
        if (particleEffectTarget)
        {
            var gameObject = Instantiate(particleEffectTarget, target.Data.transform);
            gameObject.transform.localScale = Vector3.one * scale;
            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
            float totalDuration = ps.duration + ps.startLifetime;
            yield return new WaitForSeconds(totalDuration);
            
        }
    }

    public abstract string Summary();
}