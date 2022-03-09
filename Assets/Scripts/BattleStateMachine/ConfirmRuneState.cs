using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class ConfirmRuneState : State
{
    private List<TileDirection> steps;

    public override void Enter()
    {
        base.Enter();
        steps = owner.SelectedRune.steps;
        HighlightRune();
    }

    private void HighlightRune()
    {
        owner.board.ClearHighlight();
        List<HexTile<Tile>> runeTiles = GetRuneTiles(steps);
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
    }

    public void RotateRuneClockwise()
    {
        steps = steps.Select(s => s.Clockwise()).ToList();
    }

    public void RotateRuneCounterClockwise()
    {
        steps = steps.Select(s => s.CounterClockwise()).ToList();
    }

    public List<HexTile<Tile>> GetRuneTiles(List<TileDirection> steps)
    {
        HexTile<Tile> current = owner.ActingUnit.standingTile;

        List<HexTile<Tile>> runeTiles = new List<HexTile<Tile>>();
        foreach (TileDirection step in steps)
        {
            try
            {
                HexTile<Tile> nextTile =
                    owner.board.hexMap.TilesByPosition[current.Position + HexGrid.TileDirectionVectors[(int) step]];
                if (nextTile != null)
                {
                    runeTiles.Add(nextTile);
                    current = nextTile;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        return runeTiles;
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
        StartCoroutine(ExecuteRune());
    }

    private IEnumerator ExecuteRune()
    {
        yield return null;
        owner.ChangeState<RuneExecutionState>();
    }
}

public class RuneExecutionState : State
{
}