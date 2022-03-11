using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class AIPlan
{
    public Unit actor;
    public Ability ability;
    public List<HexTile<Tile>> attackLocations;
    
    public AIPlan(Unit unit)
    {
        this.actor = unit;
    }
    
    public AIPlan(Unit unit, Ability ability, List<HexTile<Tile>> attackPos)
    {
        this.actor = unit;
        this.ability = ability;
        this.attackLocations = attackPos;
    }
}
