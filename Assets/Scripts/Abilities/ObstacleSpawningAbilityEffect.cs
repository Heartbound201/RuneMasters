using System;
using System.Collections;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

[CreateAssetMenu(fileName = "CreateObjectEffect", menuName = "Create Object Ability Effect")]
class ObjectSpawningAbilityEffect : AbilityEffect
{
    // TODO add dedicated class with board and/or tile reference
    public GameObject gameObject;

    public override IEnumerator Apply(Unit actor, HexTile<Tile> target)
    {
        yield return base.Apply(actor, target);
        if (target.Data.unitList.Count == 0)
        {
            var instantiate = Instantiate(gameObject, target.Data.transform);
            instantiate.transform.position = target.Data.transform.position;
        }
    }

    public override string Summary()
    {
        return $"Spawns a <b>{gameObject.name}</b> on the target tile";
    }
}