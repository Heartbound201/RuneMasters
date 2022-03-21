using System;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

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
    
    public static TileDirection GetDirection(this HexTile<Tile> t1, HexTile<Tile> t2)
    {
        if (t1.CartesianPosition.z < t2.CartesianPosition.z && t1.CartesianPosition.x < t2.CartesianPosition.x)
            return TileDirection.TopRight;
        if (t1.CartesianPosition.z < t2.CartesianPosition.z && t1.CartesianPosition.x > t2.CartesianPosition.x)
            return TileDirection.TopLeft;
        if (t1.CartesianPosition.z > t2.CartesianPosition.z && t1.CartesianPosition.x < t2.CartesianPosition.x)
            return TileDirection.BottomRight;
        if (t1.CartesianPosition.z > t2.CartesianPosition.z && t1.CartesianPosition.x > t2.CartesianPosition.x)
            return TileDirection.BottomLeft;
        if (Mathf.Approximately(t1.CartesianPosition.z , t2.CartesianPosition.z) && t1.CartesianPosition.x < t2.CartesianPosition.x)
            return TileDirection.Right;
        return TileDirection.Left;
    }
    public static Vector3 ToEuler(this TileDirection d)
    {
        return new Vector3(0, d.ToDegree(), 0);
    }
    
    public static int ToDegree(this TileDirection d)
    {
        return (int) d * 60;
    }
}