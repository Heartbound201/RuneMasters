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
        Board.SpawnAllyInRandomPosition();
        Board.SpawnAllyInRandomPosition();
        Board.SpawnAllyInRandomPosition();
        Board.SpawnEnemyInRandomPosition();
        owner.ChangeState<UnitSelectionState>();
    }

}
