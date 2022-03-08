using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class ConfirmRuneState : State
{
    public override void Enter()
    {
        base.Enter();
        HighlightRune(owner.SelectedRune);
    }

    private void HighlightRune(RunePrototype runePrototype)
    {
        List<HexTile<Tile>> runeTiles = GetRuneTiles(runePrototype.steps);
        owner.board.HighlightTiles(runeTiles);

        // TileDirection d = TileDirection.Left;
        // d.Clockwise();
        // List<TileDirection> steps1 = steps.Select(direction => d.Clockwise()).ToList();
        //
        // foreach (HexTile<Tile> runeTile in runeTiles)
        // {
        //     runeTile.Data.Highlight(false);
        // }
        //
        //
        // HighlightTiles(GetRuneTiles(steps1));
    }
    public List<HexTile<Tile>> GetRuneTiles(List<TileDirection> steps)
    {
        HexTile<Tile> current = owner.ActingUnit.standingTile;

        List<HexTile<Tile>> runeTiles = new List<HexTile<Tile>>();

        foreach (TileDirection step in steps)
        {
            HexTile<Tile> nextTile =
                owner.board.hexMap.TilesByPosition[current.Position + HexGrid.TileDirectionVectors[(int) step]];
            runeTiles.Add(nextTile);
            current = nextTile;
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