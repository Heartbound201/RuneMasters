using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacementState : State
{
    public override void Enter ()
    {
        base.Enter ();
        StartCoroutine(Init());
    }
    IEnumerator Init ()
    {
        
        yield return Board.GenerateBoard();
        Board.SetCamera();

        Board.SpawnEnemyAtIndex(67);
        owner.party.units.Clear();
        owner.party.units.Add(Board.SpawnAllyAtIndex(9));
        owner.party.units.Add(Board.SpawnAllyAtIndex(15));
        owner.party.units.Add(Board.SpawnAllyAtIndex(22));
        
        
        owner.ChangeState<UnitSelectionState>();
    }

}
