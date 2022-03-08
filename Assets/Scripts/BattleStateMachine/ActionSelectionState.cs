using System.Collections;
using UnityEngine;

public class ActionSelectionState : State
{
    public override void Enter()
    {
        base.Enter();
        if (TurnManager.currentTurn == TurnManager.Turn.Enemy)
            StartCoroutine(AITurn());
        else
            OpenMenu();
    }
    IEnumerator AITurn()
    {
        // Resolve prev turn action
        // Plan next attack
        yield return null;
        owner.ChangeState<AbilityExecutionState>();
    }
    
    protected void OpenMenu()
    {
        if(owner.turnMenuController == null)
        {
            Debug.LogError("There's no Menu Gameobject on ActionSelectionState.");
        }
        else
            owner.turnMenuController.Load();
    }
    protected override void AddListeners()
    {
        base.AddListeners();
        // Board.SelectTileEvent += SelectUnitByPosition;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        // Board.SelectTileEvent -= SelectUnitByPosition;
    }
}