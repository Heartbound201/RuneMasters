using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Turn _currentTurn;
    public Turn currentTurn;

    public List<Unit> units = new List<Unit>();
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
            }

            yield return currentTurn;
        }
    }

    public void Switch()
    {
        _currentTurn = _currentTurn == Turn.Enemy ? Turn.Player : Turn.Enemy;
    }

    public bool IsOver()
    {
        switch (currentTurn)
        {
            case TurnManager.Turn.Player:
                return units.TrueForAll(u => u.hasActed);
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