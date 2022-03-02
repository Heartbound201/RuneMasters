using System;
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
        Player, Enemy
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
                
                break;
            case TurnManager.Turn.Enemy:
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }
}
