using System.Collections;
using System.Collections.Generic;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class AbilityExecutionState : State
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Execution());
    }

    public override void Exit()
    {
        base.Exit();
        
        owner.SelectedAbility = null;
        owner.SelectedTile = null;
        owner.SelectedRune = null;
        owner.SelectedRuneSteps = new List<HexTile<Tile>>();
    }
    protected override void AddListeners()
    {
        base.AddListeners();
        InputController.CommandPause += owner.PauseOrResumeGame;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        InputController.CommandPause -= owner.PauseOrResumeGame;
    }
    IEnumerator Execution()
    {
        Ability a = owner.SelectedAbility;

        yield return StartCoroutine(owner.ActingUnit.Act(a, owner.SelectedTile));
        
        CameraController.instance.CameraLookAt(owner.SelectedTile);
        
        owner.IsBattleOver();
        owner.SelectedRune.SendInCooldown();
        owner.ChangeState<ActionSelectionState>();
    }
}