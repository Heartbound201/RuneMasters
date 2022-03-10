using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public Turn currentTurn;
    public Party party;

    public List<PlayerUnit> units = new List<PlayerUnit>();
    public List<Unit> enemies = new List<Unit>();

    public enum Turn
    {
        Player,
        Enemy
    }

    public IEnumerator Round()
    {
        while (true)
        {
            if (IsOver())
            {
                Switch();
                Debug.Log(currentTurn + " turn");
            }

            yield return currentTurn;
        }
    }

    public void Switch()
    {
        switch (currentTurn)
        {
            case Turn.Player:
                currentTurn = Turn.Enemy;
                break;
            case Turn.Enemy:
                foreach (Unit unit in units)
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

    public bool IsOver()
    {
        switch (currentTurn)
        {
            case TurnManager.Turn.Player:
                return units.TrueForAll(u => u.hasActed) || party.mana <= 0;
                break;
            case TurnManager.Turn.Enemy:
                return enemies.TrueForAll(u => u.hasActed);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public Unit ServeEnemy()
    {
        return enemies.Find(u => !u.hasActed);
    }
}