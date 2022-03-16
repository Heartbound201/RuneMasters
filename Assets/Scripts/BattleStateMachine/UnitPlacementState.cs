using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacementState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return Board.GenerateBoard(owner.levelData);
        Board.SetCamera();

        foreach (SpawnInfo spawnInfo in owner.levelData.enemies)
        {
            GameObject spawnEntity = Board.SpawnEntity(spawnInfo.obj, spawnInfo.index);
            EnemyUnit enemyUnit = spawnEntity.GetComponent<EnemyUnit>();
            owner.enemies.Add(enemyUnit);
        }

        owner.party = new Party
        {
            healthMax = 25,
            manaMax = 12,
            manaReserveMax = 6
        };
        owner.party.Init();
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
        owner.partyInfoMenuController.endTurnBtn.onClick.AddListener(() =>
        {
            if (owner.turnManager.currentTurn == TurnManager.Turn.Player)
            {
                owner.ChangeState<TurnSelectionState>();
            }
        });
        owner.ChangeState<TurnSelectionState>();
    }
}