using System;
using System.Collections;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "CreateObjectEffect", menuName = "Create Object Ability Effect")]
class ObjectSpawningAbilityEffect : AbilityEffect
{
    // TODO add dedicated class with board and/or tile reference
    public BoardObject boardObject;

    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        if (target.Data.content.Count == 0)
        {
            var instantiate = Instantiate(boardObject, target.Data.transform);
            instantiate.transform.position = target.Data.transform.position;
        }
    }

    public override string Summary()
    {
        return $"Spawns a <b>{boardObject.name}</b> on the target tile";
    }
}