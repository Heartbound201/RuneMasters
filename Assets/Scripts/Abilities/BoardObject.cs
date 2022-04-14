using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class BoardObject : MonoBehaviour
{
    public HexTile<Tile> Tile { get; set; }
    public bool isPassable;
    protected void PlaceOnTile(HexTile<Tile> target)
    {
        // Make sure old tile location is not still pointing to this unit
        if (Tile != null && Tile.Data.content.Contains(this))
            Tile.Data.content.Remove(this);

        // Link unit and Tile references
        Tile = target;

        if (target == null) return;
        
        target.Data.content.Add(this);
        transform.position = target.CartesianPosition;
    }
}