using UnityEngine;

public abstract class BaseState
{
    // Reference to our state machine.
    public StateMachine owner;

    public virtual void EnterState()
    {
        AddListeners();
    }
    public virtual void UpdateState()
    {
    }
    public virtual void ExitState()
    {
        RemoveListeners();
    }
    protected virtual void AddListeners()
    {
    }
    protected virtual void RemoveListeners()
    {
    }
}