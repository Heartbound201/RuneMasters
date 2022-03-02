using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class TileData : ISerializable
{
    public Board board;
    public List<Unit> content = new List<Unit>();
    public bool isPassable;
    public Tile tile;

    public bool IsPassable
    {
        get
        {
            return isPassable && content.All(entity => entity.isPassable);
        }
    }

    public Unit Unit
    {
        get
        {
            return content.OfType<Unit>().FirstOrDefault();
        }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new System.NotImplementedException();
    }
}
