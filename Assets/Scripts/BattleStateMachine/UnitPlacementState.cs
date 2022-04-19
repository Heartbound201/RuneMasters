using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;

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

        foreach (SpawnInfo spawnInfo in owner.levelData.enemies)
        {
            GameObject spawnEntity = Board.SpawnEnemy(spawnInfo.obj, spawnInfo.index);
            EnemyUnit enemyUnit = spawnEntity.GetComponent<EnemyUnit>();
            enemyUnit.PlaceOnTile(Board.hexMap.Tiles[spawnInfo.index], TileDirection.Left);
            owner.enemies.Add(enemyUnit);
            yield return new WaitForSeconds(0.8f);
        }

        owner.party.Init();
        foreach (SpawnInfo spawnInfo in owner.levelData.characters)
        {
            GameObject spawnEntity = Board.SpawnUnit(spawnInfo.obj, spawnInfo.index);
            PlayerUnit playerUnit = spawnEntity.GetComponent<PlayerUnit>();

            playerUnit.PlaceOnTile(Board.hexMap.Tiles[spawnInfo.index], TileDirection.Right);
            playerUnit.party = owner.party;
            owner.party.Units.Add(playerUnit);
            yield return new WaitForSeconds(0.8f);
        }

        TurnManager.party = owner.party;
        TurnManager.units = owner.party.Units;
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
        
        owner.HudFadeInTimeline.Play();
        
        
        switch (TurnManager.currentTurn)
        {
            case TurnManager.Turn.Player:
                owner.ChangeState<ActionSelectionState>();
                break;
            case TurnManager.Turn.Enemy:
                owner.ChangeState<AIPlanExecutionState>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}