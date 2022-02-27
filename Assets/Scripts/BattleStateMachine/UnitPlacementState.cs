using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacementState : BaseState
{
    public override void EnterState()
    {
        base.EnterState();
        owner.board.SpawnAllyInRandomPosition();
        owner.board.SpawnAllyInRandomPosition();
        owner.board.SpawnAllyInRandomPosition();
        owner.board.SpawnEnemyInRandomPosition();
        owner.ChangeState(new UnitSelectionState());
    }

}
