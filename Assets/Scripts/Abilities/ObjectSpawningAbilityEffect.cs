using System;
using System.Collections;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "CreateObjectEffect", menuName = "Create Object Ability Effect")]
class ObjectSpawningAbilityEffect : AbilityEffect
{
    public GameObject boardObject;

    public override void Apply(Unit actor, HexTile<Tile> target)
    {
        if (target.Data.content.Count == 0)
        {
            GameObject instantiate = Instantiate(boardObject, target.Data.transform);
            instantiate.transform.position = target.Data.transform.position;
            BoardObject component = instantiate.GetComponent<BoardObject>();
            component.Tile = target;
            target.Data.content.Add(component);
        }
    }

    public override string Summary(Unit unit)
    {
        return $"Spawns a <b>{boardObject.name}</b> on the target tile";
    }
}