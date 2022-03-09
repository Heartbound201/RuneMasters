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
        
        yield return Board.GenerateBoard(owner.levelData);
        Board.SetCamera();
        
        owner.enemies.Add(Board.SpawnEnemyAtIndex(67));

        // clear since party is a scriptable object
        owner.party.units.Clear();
        foreach (SpawnInfo spawnInfo in owner.levelData.characters)
        {
            owner.party.units.Add(Board.SpawnEntity(spawnInfo.obj, spawnInfo.index));

        }

        TurnManager.units = owner.party.units;
        TurnManager.enemies = owner.enemies;
        Debug.Log("Units spawned");
        
        owner.turnMenuController.Load();
        owner.ChangeState<TurnSelectionState>();
    }

}
