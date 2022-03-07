using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    private HexMouse hexMouse;
    public static event Action<HexTile<Tile>> SelectTileEvent;

    public int radius = 5;
    public TileCollection tileCollection;

    public HexMap<Tile> hexMap;
    public GameObject genericAllyPrefab;
    public GameObject genericEnemyPrefab;


    public HexTile<Tile> tileHover;
    public HexTile<Tile> tileSelection;

    [Header("Materials")] public Material tileHoverMat;
    public Material tileSelectionMat;

    [Header("Audio")] public AudioClip tileHoverSfx;
    public AudioClip tileSelectionSfx;
    public AudioClip unitPlacementSfx;

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
        // 9 15 22 67
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
            tileHover.Data.Highlight(false);
        }

        tileHover = tile;
        // Debug.Log("Hovering " + tile.Index);

        if (tileHover == tileSelection) return;
        tileHover.Data.Highlight(true);
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

    public void TestRune()
    {
        

        List<TileDirection> steps = new List<TileDirection>
        {
            TileDirection.TopRight,
            TileDirection.TopRight,
            TileDirection.TopRight
        };
        List<HexTile<Tile>> runeTiles = GetRuneTiles(steps);
        StartCoroutine(HighlightTiles(runeTiles));

        TileDirection d = TileDirection.Left;
        d.Clockwise();
        List<TileDirection> steps1 = steps.Select(direction => d.Clockwise()).ToList();

        foreach (HexTile<Tile> runeTile in runeTiles)
        {
            runeTile.Data.Highlight(false);
        }
        
        
        StartCoroutine(HighlightTiles(GetRuneTiles(steps1)));
    }
    
    public List<HexTile<Tile>> GetRuneTiles(List<TileDirection> steps)
    {
        HexTile<Tile> current = tileSelection;

        List<HexTile<Tile>> runeTiles = new List<HexTile<Tile>>();

        foreach (TileDirection step in steps)
        {
            HexTile<Tile> nextTile =
                hexMap.TilesByPosition[current.Position + HexGrid.TileDirectionVectors[(int) step]];
            runeTiles.Add(nextTile);
            current = nextTile;
        }

        return runeTiles;
    }

    public IEnumerator HighlightTiles(List<HexTile<Tile>> tiles)
    {
        foreach (HexTile<Tile> tile in tiles)
        {
            tile.Data.Highlight(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SpawnAllyInRandomPosition()
    {
        HexTile<Tile> hexTile;
        do
        {
            hexTile = hexMap.Tiles[Random.Range(0, hexMap.Tiles.Length)];
        } while (hexTile.Data.unit != null);

        SpawnEntity(genericAllyPrefab, hexTile.Position);
    }

    public void SpawnAllyAtIndex(int index)
    {
        HexTile<Tile> hexTile = hexMap.Tiles[index];
        SpawnEntity(genericAllyPrefab, hexTile.Position);
    }

    public void SpawnEnemyAtIndex(int index)
    {
        HexTile<Tile> hexTile = hexMap.Tiles[index];
        SpawnEntity(genericEnemyPrefab, hexTile.Position);
    }

    public void SpawnEnemyInRandomPosition()
    {
        HexTile<Tile> hexTile;
        do
        {
            hexTile = hexMap.Tiles[Random.Range(0, hexMap.Tiles.Length)];
        } while (hexTile.Data.unit != null);

        SpawnEntity(genericEnemyPrefab, hexTile.Position);
    }

    public void SpawnAlly(Vector3Int pos)
    {
        SpawnEntity(genericAllyPrefab, pos);
    }

    public void SpawnEnemy(Vector3Int pos)
    {
        SpawnEntity(genericEnemyPrefab, pos);
    }

    public void SpawnEntity(GameObject obj, Vector3Int pos)
    {
        HexTile<Tile> hexTile = hexMap.TilesByPosition[pos];
        GameObject o = Instantiate(obj, hexTile.CartesianPosition, Quaternion.identity);
        Unit unit = o.GetComponent<Unit>();
        unit.standingTile = hexTile;
        hexTile.Data.unit = unit;
    }

    public IEnumerator GenerateBoard()
    {
        hexMap = new HexMap<Tile>(HexMapBuilder.CreateHexagonalShapedMap(radius), null);
        hexMouse.Init(hexMap);
        foreach (var tile in hexMap.Tiles)
        {
            TilePrototype tilePrototype = tileCollection.GetRandomTile();
            GameObject instance = Instantiate(tilePrototype.prefab, transform);
            instance.transform.position = tile.CartesianPosition;
            instance.gameObject.name = "Hex" + tile.CartesianPosition;
            tile.Data = instance.GetComponent<Tile>();
            tile.Data.board = this;

            yield return new WaitForSeconds(0.05f);
        }

        yield return null;
    }


    public void SetCamera()
    {
        //put the following at the end of the start method (or in its own method called after map creation)
        // Camera.main.transform.position = new Vector3(hexMap.MapSizeData.center.x, 4, hexMap.MapSizeData.center.z); // centers the camera and moves it 5 units above the XZ-plane
        // Camera.main.orthographic = true; //for this example we use an orthographic camera.
        // Camera.main.transform.rotation = Quaternion.Euler(35, 30, 0); //rotates the camera to it looks at the XZ-plane
        Camera.main.orthographicSize =
            hexMap.MapSizeData.extents.z * 2 * 0.8f; // sets orthographic size of the camera.]
        Camera.main.nearClipPlane = -12f;
        //this does not account for aspect ratio but for our purposes it works good enough.
    }

    #region Pathfinding

    public List<HexTile<Tile>> SearchRange(HexTile<Tile> start, Func<HexTile<Tile>, HexTile<Tile>, bool> addTile,
        bool selectTilesAtEnd = false)
    {
        ClearPathfinding();

        List<HexTile<Tile>> tilesInRange = new List<HexTile<Tile>> {start};

        Queue<HexTile<Tile>> toCheck = new Queue<HexTile<Tile>>();
        ;
        toCheck.Enqueue(start);
        Queue<HexTile<Tile>> explored = new Queue<HexTile<Tile>>();
        start.Data._distance = 0;
        while (toCheck.Count > 0)
        {
            HexTile<Tile> current = toCheck.Dequeue();
            foreach (HexTile<Tile> next in hexMap.GetTiles.AdjacentToTile(start))
            {
                // HexTile<Tile> next = GetTile(current.Position + dirs[i]);
                if (next == null || next.Data._distance <= current.Data._distance + 1)
                    continue;
                if (addTile(current, next))
                {
                    next.Data._distance = current.Data._distance + 1;
                    next.Data._prev = current.Data;
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

    public void HighlightTiles(ICollection<HexTile<Tile>> items)
    {
        foreach (HexTile<Tile> t in hexMap.Tiles)
        {
            if (items.Contains(t))
                t.Data.Highlight(true);
            else
                t.Data.Highlight(false);
        }
    }

    #endregion
}