using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class AIPlan
{
    public Unit actor;
    public Ability ability;
    public HexTile<Tile> attackLocation;
    private Board board;
    
    public AIPlan(Unit unit)
    {
        this.actor = unit;
    }
    
    public AIPlan(Unit unit, Ability ability, HexTile<Tile> attackPos)
    {
        this.actor = unit;
        this.ability = ability;
        this.attackLocation = attackPos;
        if(attackLocation != null)
        {
            this.board = attackLocation.Data.board;
        }
    }
}
