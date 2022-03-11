using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            yield return currentTurn;
        }
    }

    public void Switch()
    {
        switch (currentTurn)
        {
            case Turn.Player:
                foreach (EnemyUnit enemy in enemies)
                {
                    enemy.Reset();
                }
                currentTurn = Turn.Enemy;
                break;
            case Turn.Enemy:
                foreach (PlayerUnit unit in units)
                {
                    unit.Reset();
                }
                party.ResetTurn();
                currentTurn = Turn.Player;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}