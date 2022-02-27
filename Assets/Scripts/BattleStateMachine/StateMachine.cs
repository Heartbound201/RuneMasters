using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class StateMachine : MonoBehaviour
{
    // Reference to currently operating state.
    private BaseState currentState;

    public Board board;
    public TurnManager turnManager;
    public List<Entity> enemies = new List<Entity>();
    public List<Entity> units = new List<Entity>();

    public Entity selectedEntity;
    public HexTile<TileData> selectedTile;

    private void Start()
    {
        // Start game in menu state
        board.GenerateBoard();
        ChangeState(new UnitPlacementState());
    }

    private void Update()
    {
        // If we have reference to state, we should update it!
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    public void ChangeState(BaseState newState)
    {
        // If we currently have state, we need to destroy it!
        if (currentState != null)
        {
            currentState.ExitState();
        }

        // Swap reference
        currentState = newState;
        Debug.Log("State changed to " + currentState);

        // If we passed reference to new state, we should assign owner of that state and initialize it!
        // If we decided to pass null as new state, nothing will happened.
        if (currentState != null)
        {
            currentState.owner = this;
            currentState.EnterState();
        }
    }
}