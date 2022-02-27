using System;
using System.Collections.Generic;

public class TurnManager
{

    private Turn _currentTurn;
    public Turn currentTurn;

    public List<Entity> units = new List<Entity>();
    public List<Entity> enemies = new List<Entity>();
    
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
