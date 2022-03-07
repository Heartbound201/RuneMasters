using System.Collections;
using UnityEngine;

public class ActionSelectionState : State
{
    public GameObject hud;
    
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
        if(hud == null)
        {
            Debug.LogError("There's no Menu Gameobject on ActionSelectionState.");
        }
        else
            hud.SetActive(true);
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