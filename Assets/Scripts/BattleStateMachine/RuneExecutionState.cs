using System.Collections;
using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary.Generic;

public class RuneExecutionState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Sequence(Board.GetRuneTiles(owner.selectedRuneSteps, owner.ActingUnit.standingTile)));
    }
    
    IEnumerator Sequence(List<HexTile<Tile>> line)
    {
        yield return StartCoroutine(owner.ActingUnit.Move(line)); // coroutine
        owner.ActingUnit.hasActed = true;
        owner.ChangeState<TurnSelectionState>();
    }
}