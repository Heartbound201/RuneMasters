using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;
[CreateAssetMenu(fileName = "SelfRange", menuName = "Create Self Ability Range")]
public class SelfAbilityRange : AbilityRange
{
    public override List<HexTile<Tile>> GetTilesInRange (Unit unit, Board board)
    {
        List<HexTile<Tile>> retValue = new List<HexTile<Tile>>(1);
        retValue.Add(unit.tile);
        return retValue;
    }
}