using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class TurnManager : MonoBehaviour
{
    public Turn currentTurn;
    public Party party;

    public List<PlayerUnit> units = new List<PlayerUnit>();
    public List<EnemyUnit> enemies = new List<EnemyUnit>();

    public enum Turn
    {
        Player,
        Enemy
    }

    public IEnumerator Round()
    {
        while (true)
        {
            Switch();
            Debug.Log(currentTurn + " turn");

            ShowTurnMessage(currentTurn);
            yield return currentTurn;
        }
    }

    private void ShowTurnMessage(Turn turn)
    {
        MessageDisplayingUIController.Instance.ShowMessage($"{turn}'s turn");
    }

    private void Start()
    {
        EnemyUnit.KOEvent += OnEnemyUnitKoEvent;
    }

    private void OnEnemyUnitKoEvent(Unit unit)
    {
        enemies.Remove((EnemyUnit) unit);
    }

    private void OnDestroy()
    {
        EnemyUnit.KOEvent -= OnEnemyUnitKoEvent;
    }

    public void Switch()
    {
        switch (currentTurn)
        {
            case Turn.Player:
                foreach (PlayerUnit unit in units)
                {
                    unit.TriggerStatusEnd();
                }
                foreach (EnemyUnit enemy in enemies)
                {
                    enemy.Reset();
                    enemy.TriggerStatusStart();
                }
                currentTurn = Turn.Enemy;
                break;
            case Turn.Enemy:
                foreach (EnemyUnit enemy in enemies)
                {
                    enemy.TriggerStatusEnd();
                }
                foreach (PlayerUnit unit in units)
                {
                    unit.Reset();
                    unit.TriggerStatusStart();
                }
                party.ResetTurn();
                currentTurn = Turn.Player;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}