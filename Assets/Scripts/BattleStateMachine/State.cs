using System;
using UnityEngine;

public abstract class State : MonoBehaviour 
{
    protected BattleStateMachine owner;

    protected Board Board => owner.board;
    protected TurnManager TurnManager => owner.turnManager;
    protected LevelData LevelData => owner.levelData;

    private void Awake()
    {
        owner = GetComponent<BattleStateMachine>();
    }

    public virtual void Enter ()
    {
        AddListeners();
    }
  
    public virtual void Exit ()
    {
        RemoveListeners();
    }
    protected virtual void OnDestroy ()
    {
        RemoveListeners();
    }
    protected virtual void AddListeners ()
    {
    }
  
    protected virtual void RemoveListeners ()
    {
    }
}