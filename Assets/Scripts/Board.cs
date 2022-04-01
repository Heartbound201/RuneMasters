using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class Board : MonoBehaviour
{
    public static event Action<HexTile<Tile>> SelectTileEvent;

    public HexMouse hexMouse;
    public HexMap<Tile, int> hexMap;

    public HexTile<Tile> tileHover;
    public HexTile<Tile> tileSelection;

    [Header("Audio")] public AudioClip tileHoverSfx;
    public AudioClip tileSelectionSfx;
    public AudioClip unitPlacementSfx;

    public GameObject edgePrefab;
    private GameObject[] edges;
    private GameObject[] tiles;
    private void Awake()
    {
        hexMouse = gameObject.AddComponent<HexMouse>();
    }

    void Update()
    {
        if (!hexMouse.CursorIsOnMap) return;
        Vector3Int mouseTilePosition = hexMouse.TileCoord;
        HexTile<Tile> t = hexMap.TilesByPosition[mouseTilePosition];
        HoverTile(t);

        if (Input.GetMouseButtonDown(0))
        {
            SelectTile(t);
            SelectTileEvent?.Invoke(t);
        }
    }

    public void HoverTile(HexTile<Tile> tile)
    {
        if (tileHover != null && tileHover != tile)
        {
            tileHover.Data.Hover(false);
        }

        tileHover = tile;
        tileHover.Data.Hover(true);
    }

    public void SelectTile(HexTile<Tile> tile)
    {
        if (tileSelection != null && tileSelection != tile)
        {
            tileSelection.Data.Select(false);
        }

        tileSelection = tile;

        tileSelection.Data.Select(true);
    }

    public void HighlightTiles(List<HexTile<Tile>> tiles)
    {
        foreach (HexTile<Tile> tile in tiles)
        {
            tile.Data.Highlight(true);
        }
    }

    public GameObject SpawnEntity(GameObject obj, int index)
    {
        HexTile<Tile> hexTile = hexMap.Tiles[index];
        return Instantiate(obj, hexTile.CartesianPosition, Quaternion.identity);
    }

    public IEnumerator GenerateBoard(LevelData levelData)
    {
        hexMap = new HexMap<Tile, int>(HexMapBuilder.CreateHexagonalShapedMap(levelData.boardRadius), null);
        hexMouse.Init(hexMap);
        tiles = new GameObject[hexMap.TilesByPosition.Count];
        edges = new GameObject[hexMap.EdgesByPosition.Count];
        foreach (var tile in hexMap.Tiles)
        {
            TilePrototype tilePrototype = levelData.tiles.Find(ti => ti.index == tile.Index).proto;
            GameObject instance = Instantiate(tilePrototype.prefab, transform);
            instance.transform.position = tile.CartesianPosition;
            instance.gameObject.name = "Hex" + tile.CartesianPosition + "["+tile.Index+"]";
            tile.Data = instance.GetComponent<Tile>();
            tile.Data.board = this;
            tile.Data.pos = tile.Position;
            tile.Data.posCart = tile.CartesianPosition;
            tile.Data.posNorm = tile.NormalizedPosition;
            tile.Data.prototype = tilePrototype;
            tiles[tile.Index] = instance;
            yield return null;
        }

        foreach (var tileBorder in hexMap.Edges)
        {
            GameObject instance = Instantiate(edgePrefab, transform);
            instance.name = "MapEdge_" + tileBorder.Position;
            instance.transform.position = tileBorder.CartesianPosition;
            instance.transform.rotation = Quaternion.Euler(0, tileBorder.EdgeAlignmentAngle, 0);
            edges[tileBorder.Index] = instance;
            instance.SetActive(false);
        }
        yield return null;
    }

    public List<HexTile<Tile>> SearchRange(HexTile<Tile> start, Func<HexTile<Tile>, HexTile<Tile>, bool> addTile,
        bool selectTilesAtEnd = false)
    {
        ClearPathfinding();

        List<HexTile<Tile>> tilesInRange = new List<HexTile<Tile>> {start};

        Queue<HexTile<Tile>> toCheck = new Queue<HexTile<Tile>>();
        toCheck.Enqueue(start);
        Queue<HexTile<Tile>> explored = new Queue<HexTile<Tile>>();
        start.Data._distance = 0;
        while (toCheck.Count > 0)
        {
            HexTile<Tile> current = toCheck.Dequeue();
            foreach (HexTile<Tile> next in hexMap.GetTiles.AdjacentToTile(current))
            {
                if (next == null || next.Data._distance <= current.Data._distance + 1)
                    continue;
                if (addTile(current, next))
                {
                    next.Data._distance = current.Data._distance + 1;
                    next.Data._prev = current;
                    explored.Enqueue(next);
                    tilesInRange.Add(next);
                }
            }

            SwapReference(ref toCheck, ref explored);
        }

        if (selectTilesAtEnd) HighlightTiles(tilesInRange);
        return tilesInRange;
    }

    private void ClearPathfinding()
    {
        foreach (HexTile<Tile> t in hexMap.Tiles)
        {
            t.Data._prev = null;
            t.Data._distance = int.MaxValue;
        }
    }

    void SwapReference(ref Queue<HexTile<Tile>> a, ref Queue<HexTile<Tile>> b)
    {
        (a, b) = (b, a);
    }


    public void ClearHighlight()
    {
        foreach (HexTile<Tile> hexMapTile in hexMap.Tiles)
        {
            hexMapTile.Data.Highlight(false);
        }
    }

    public List<HexTile<Tile>> GetPathTiles(HexTile<Tile> start, List<TileDirection> steps)
    {
        HexTile<Tile> current = start;

        List<HexTile<Tile>> runeTiles = new List<HexTile<Tile>>();
        foreach (TileDirection step in steps)
        {
            try
            {
                HexTile<Tile> nextTile =
                    hexMap.TilesByPosition[current.Position + HexGrid.TileDirectionVectors[(int) step]];
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

    public void HighlightRune(List<HexTile<Tile>> tiles)
    {
        List<HexEdge<int>> hexEdges = hexMap.GetEdges.TileBorders(tiles);
        foreach (HexEdge<int> hexEdge in hexEdges)
        {
            MeshRenderer meshRenderer = edges[hexEdge.Index].GetComponentInChildren<MeshRenderer>();
            meshRenderer.material.color = Color.red;
        }
    }

}