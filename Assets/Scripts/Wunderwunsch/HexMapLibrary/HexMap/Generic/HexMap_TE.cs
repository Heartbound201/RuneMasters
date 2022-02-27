using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary.Generic
{
    public class HexMap<T,E> : HexMap<T> where T : new() where E : new()
    {
        public EdgeDataProvider<E> GetEdge { get; private set; }
        public EdgesDataProvider<E> GetEdges { get; private set; }
        public Dictionary<Vector3Int, HexEdge<E>> EdgesByPosition { get; private set; }
        public HexEdge<E>[] Edges { get; private set; }
        


        public HexMap(Dictionary<Vector3Int, int> tileIndexByPosition, CoordinateWrapper coordinateWrapper = null) : base(tileIndexByPosition, coordinateWrapper)
        {
            CreateEdgeData();
            GetEdge = new EdgeDataProvider<E>(EdgesByPosition, base.GetEdgePosition);
            GetEdges = new EdgesDataProvider<E>(EdgesByPosition, base.GetEdgePositions);
        }

        public void CreateEdgeData()
        {
            Vector3 center = MapSizeData.center;
            Vector3 extents = MapSizeData.extents;
            Vector3IntEqualityComparer vector3IntEqualityComparer = new Vector3IntEqualityComparer();

            float minX = center.x - extents.x;
            float maxX = center.x + extents.x;
            float minZ = center.z - extents.z;
            float maxZ = center.z + extents.z;

            EdgesByPosition = new Dictionary<Vector3Int, HexEdge<E>>(vector3IntEqualityComparer);
            Edges = new HexEdge<E>[EdgeIndexByPosition.Count];

            foreach (var kvp in EdgeIndexByPosition)
            {
                Vector2 normalizedPosition = HexConverter.TileCoordToNormalizedPosition(kvp.Key, minX, maxX, minZ, maxZ);
                HexEdge<E> hexEdge = new HexEdge<E>(kvp.Key, kvp.Value, normalizedPosition);
                EdgesByPosition.Add(kvp.Key, hexEdge);
                Edges[hexEdge.Index] = hexEdge;
            }
        }
    }
}