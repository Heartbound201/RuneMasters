using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class TileData : ISerializable
{
    public Board board;
    public List<Entity> content = new List<Entity>();
    public bool isPassable;
    public Tile tile;

    public bool IsPassable
    {
        get
        {
            return isPassable && content.All(entity => entity.isPassable);
        }
    }

    public Entity Entity
    {
        get
        {
            return content.OfType<Entity>().FirstOrDefault();
        }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new System.NotImplementedException();
    }
}
