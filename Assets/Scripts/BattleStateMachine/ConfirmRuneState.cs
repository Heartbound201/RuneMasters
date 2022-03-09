using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class ConfirmRuneState : State
{
    private List<HexTile<Tile>> runeTiles = new List<HexTile<Tile>>();
    public override void Enter()
    {
        base.Enter();
        owner.selectedRuneSteps = owner.SelectedRune.steps;
        owner.board.canHighlightOnHover = false;
        HighlightRune();
    }

    public override void Exit()
    {
        base.Exit();
        owner.board.canHighlightOnHover = false;
    }

    private void HighlightRune()
    {
        owner.board.ClearHighlight();
        runeTiles = Board.GetRuneTiles(owner.selectedRuneSteps, owner.ActingUnit.standingTile);
        owner.board.HighlightTiles(runeTiles);
    }

    private void Update()
    {
        // rotate ccw
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateRuneCounterClockwise();
            HighlightRune();
        }

        // rotate cw
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateRuneClockwise();
            HighlightRune();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            owner.ChangeState<RuneExecutionState>();
        }
        
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            owner.ChangeState<ActionSelectionState>();
        }
    }

    public void RotateRuneClockwise()
    {
        owner.selectedRuneSteps = owner.selectedRuneSteps.Select(s => s.Clockwise()).ToList();
    }

    public void RotateRuneCounterClockwise()
    {
        owner.selectedRuneSteps = owner.selectedRuneSteps.Select(s => s.CounterClockwise()).ToList();
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        Board.SelectTileEvent += ConfirmRune;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        Board.SelectTileEvent -= ConfirmRune;
    }

    private void ConfirmRune(HexTile<Tile> obj)
    {
        if (!runeTiles.Contains(obj)) return;
        StartCoroutine(ExecuteRune());
    }

    private IEnumerator ExecuteRune()
    {
        yield return null;
        owner.ChangeState<RuneExecutionState>();
    }
}