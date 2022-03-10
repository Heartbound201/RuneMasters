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

        owner.party = new Party
        {
            healthMax = 10,
            manaMax = 10,
            manaReserveMax = 3
        };
        owner.party.Reset();
        foreach (SpawnInfo spawnInfo in owner.levelData.characters)
        {
            GameObject spawnEntity = Board.SpawnEntity(spawnInfo.obj, spawnInfo.index);
            PlayerUnit playerUnit = spawnEntity.GetComponent<PlayerUnit>();
            playerUnit.party = owner.party; //TODO find a better solution
            owner.party.units.Add(playerUnit);

        }

        TurnManager.party = owner.party;
        TurnManager.units = owner.party.units;
        TurnManager.enemies = owner.enemies;
        Debug.Log("Units spawned");
        
        owner.turnMenuController.Load(owner.party);
        owner.ChangeState<TurnSelectionState>();
    }

}
