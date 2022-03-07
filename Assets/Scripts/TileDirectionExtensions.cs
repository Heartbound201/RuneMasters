using System;
using Wunderwunsch.HexMapLibrary;

public static class TileDirectionExtensions
{
    public static TileDirection Clockwise(this TileDirection d)
    {
        switch (d)
        {
            case TileDirection.TopRight:
                return TileDirection.Right;
                break;
            case TileDirection.Right:
                return TileDirection.BottomRight;
                break;
            case TileDirection.BottomRight:
                return TileDirection.BottomLeft;
                break;
            case TileDirection.BottomLeft:
                return TileDirection.Left;
                break;
            case TileDirection.Left:
                return TileDirection.TopLeft;
                break;
            case TileDirection.TopLeft:
                return TileDirection.TopRight;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(d), d, null);
        }
    }
    public static TileDirection CounterClockwise(this TileDirection d)
    {
        switch (d)
        {
            case TileDirection.TopRight:
                return TileDirection.TopLeft;
                break;
            case TileDirection.Right:
                return TileDirection.TopRight;
                break;
            case TileDirection.BottomRight:
                return TileDirection.Right;
                break;
            case TileDirection.BottomLeft:
                return TileDirection.BottomRight;
                break;
            case TileDirection.Left:
                return TileDirection.BottomLeft;
                break;
            case TileDirection.TopLeft:
                return TileDirection.Left;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(d), d, null);
        }
    }
}