using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class MapEditor : MonoBehaviour
{
    private HexMouse hexMouse;

    public HexMap<Tile, int> hexMap;
    private HexTile<Tile> tileHover;
    private HexTile<Tile> tileSelected;

    public Camera cam;
    public LevelData levelData;
    public TileCollection tileCollection;
    public Transform brushPanel;

    [Tooltip("Apply a random rotation to tiles")]
    public bool applyRandomRotation = false;

    public List<GameObject> brushes = new List<GameObject>();
    public GameObject edgePrefab;

    private GameObject[] edges;
    private GameObject[] tiles;
    private IBrush selectedBrush;

    private void Awake()
    {
        hexMouse = gameObject.AddComponent<HexMouse>();
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(levelData);
#endif
    }

    void Start()
    {
        if (levelData == null)
        {
            Debug.LogError("set a LevelData into the Board game object.");
        }

        if (levelData.tiles == null || levelData.tiles.Count <= 0)
        {
            StartCoroutine(GenerateRandomBoard());
        }
        else
        {
            StartCoroutine(GenerateBoardFromLevelData());
        }

        SetCamera();
        CreateBrushes();
    }

    private void CreateBrushes()
    {
        foreach (var brush in brushes)
        {
            var brushGO = Instantiate(brush, brushPanel);
            var brushComponent = brushGO.GetComponent<IBrush>();
            brushComponent.AddButtonListener(() => SelectBrush(brushComponent));
        }
    }

    void Update()
    {
        if (!hexMouse.CursorIsOnMap) return;
        Vector3Int mouseTilePosition = hexMouse.TileCoord;
        HexTile<Tile> t = hexMap.TilesByPosition[mouseTilePosition];
        HoverTile(t);

        if (Input.GetMouseButtonDown(0))
        {
            Paint(t);
        }

        if (Input.GetMouseButtonDown(1))
        {
            ClearTile(t);
        }
    }

    public void SelectBrush(IBrush brush)
    {
        if (selectedBrush != null)
        {
            selectedBrush.Select(false);
        }

        brush.Select(true);
        selectedBrush = brush;
    }

    private void Paint(HexTile<Tile> hexTile)
    {
        if (selectedBrush == null) return;
        if (hexTile == null || hexTile.Data == null) return;
        selectedBrush.Paint(levelData, hexTile, transform);
    }

    private void ClearTile(HexTile<Tile> hexTile)
    {
        RemoveCharacter(hexTile, null);
        RemoveEnemy(hexTile, null);
        RemoveTile(hexTile, GetRandomTileProto());
    }

    private void RemoveCharacter(HexTile<Tile> hexTile, GameObject go)
    {
        // clear previous unit on tile
        Unit unit = hexTile.Data.Unit;
        if (unit != null) Destroy(unit.gameObject);

        hexTile.Data.content.Clear();

        // place unit on leveldata
        levelData.PlaceUnit(hexTile.Index, go);
    }

    private void RemoveEnemy(HexTile<Tile> hexTile, GameObject go)
    {
        // clear previous unit on tile
        Unit unit = hexTile.Data.Unit;
        if (unit != null) Destroy(unit.gameObject);

        hexTile.Data.content.Clear();

        // place unit on leveldata
        levelData.PlaceEnemy(hexTile.Index, go);
    }

    private void RemoveTile(HexTile<Tile> hexTile, TilePrototype proto)
    {
        Destroy(hexTile.Data.gameObject);
        GameObject instance = Instantiate(proto.prefab, transform);
        hexTile.Data = instance.GetComponent<Tile>();
        instance.transform.position = hexTile.CartesianPosition;
        instance.gameObject.name = "Hex" + hexTile.CartesianPosition;
        hexTile.Data.prototype = proto;

        levelData.PlaceTilePrototype(hexTile.Index, proto);
    }

    private void PlaceCharacter(HexTile<Tile> hexTile, GameObject go)
    {
        if (go == null) return;
        // clear previous unit on tile
        if (hexTile.Data.content.Count > 0)
        {
            Unit unit = hexTile.Data.Unit;
            if (unit != null) Destroy(unit.gameObject);
        }

        hexTile.Data.content.Clear();

        // place unit on leveldata
        levelData.PlaceUnit(hexTile.Index, go);

        // instantiate graphics
        GameObject instance = Instantiate(go, transform);
        instance.transform.position = hexTile.CartesianPosition;
        Unit character = instance.GetComponent<PlayerUnit>();
        hexTile.Data.content.Add(character);
    }

    private void PlaceEnemy(HexTile<Tile> hexTile, GameObject go)
    {
        if (go == null) return;
        // clear previous unit on tile
        if (hexTile.Data.content.Count > 0)
        {
            Unit unit = hexTile.Data.Unit;
            if (unit != null) Destroy(unit.gameObject);
        }

        hexTile.Data.content.Clear();

        // place unit on leveldata
        levelData.PlaceEnemy(hexTile.Index, go);

        // instantiate graphics
        GameObject instance = Instantiate(go, transform);
        instance.transform.position = hexTile.CartesianPosition;
        Unit character = instance.GetComponent<EnemyUnit>();
        hexTile.Data.content.Add(character);
    }

    private IEnumerator GenerateBoardFromLevelData()
    {
        ClearBoard();
        hexMap = new HexMap<Tile, int>(HexMapBuilder.CreateHexagonalShapedMap(levelData.boardRadius), null);
        hexMouse.Init(hexMap);

        tiles = new GameObject[hexMap.TilesByPosition.Count];
        edges = new GameObject[hexMap.EdgesByPosition.Count];

        foreach (var tile in hexMap.Tiles)
        {
            TilePrototype tilePrototype = levelData.GetTilePrototypeAt(tile.Index);

            GameObject instance = Instantiate(tilePrototype.prefab, transform);
            instance.transform.position = tile.CartesianPosition;
            if (applyRandomRotation)
            {
                instance.transform.rotation = new Quaternion(0f, Random.Range(0, 6) * 60f, 0f, 0f);
            }

            instance.gameObject.name = "Hex" + tile.CartesianPosition;
            tile.Data = instance.GetComponent<Tile>();
            tile.Data.prototype = tilePrototype;

            PlaceEnemy(tile, levelData.GetEnemyAt(tile.Index));
            PlaceCharacter(tile, levelData.GetUnitAt(tile.Index));

            yield return null;
        }

        GenerateEdges();

        yield return null;
    }

    private IEnumerator GenerateRandomBoard()
    {
        ClearBoard();
        hexMap = new HexMap<Tile, int>(HexMapBuilder.CreateHexagonalShapedMap(levelData.boardRadius), null);
        hexMouse.Init(hexMap);

        tiles = new GameObject[hexMap.TilesByPosition.Count];
        edges = new GameObject[hexMap.EdgesByPosition.Count];

        foreach (var tile in hexMap.Tiles)
        {
            TilePrototype tilePrototype = GetRandomTileProto();

            GameObject instance = Instantiate(tilePrototype.prefab, transform);
            instance.transform.position = tile.CartesianPosition;
            if (applyRandomRotation)
            {
                instance.transform.rotation = new Quaternion(0f, Random.Range(0, 6) * 60f, 0f, 0f);
            }

            instance.gameObject.name = "Hex" + tile.CartesianPosition;
            tile.Data = instance.GetComponent<Tile>();
            tile.Data.prototype = tilePrototype;

            levelData.PlaceTilePrototype(tile.Index, tilePrototype);

            yield return null;
        }

        GenerateEdges();

        yield return null;
    }

    private void GenerateEdges()
    {
        if (edgePrefab)
        {
            foreach (var tileBorder in hexMap.Edges)
            {
                GameObject instance2 = Instantiate(edgePrefab, transform);
                instance2.name = "MapEdge_" + tileBorder.Position;
                instance2.transform.position = tileBorder.CartesianPosition;
                instance2.transform.rotation = Quaternion.Euler(0, tileBorder.EdgeAlignmentAngle, 0);
                edges[tileBorder.Index] = instance2;
                instance2.SetActive(true);
            }
        }
    }

    private void ClearBoard()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
    }

    private void HoverTile(HexTile<Tile> tile)
    {
        if (tileHover != null && tileHover != tile && tileHover.Data != null)
        {
            tileHover.Data.Hover(false);
        }

        tileHover = tile;
        tileHover.Data.Hover(true);
    }

    private void SetCamera()
    {
        //put the following at the end of the start method (or in its own method called after map creation)
        cam.transform.position =
            new Vector3(hexMap.MapSizeData.center.x, 4,
                hexMap.MapSizeData.center.z); // centers the camera and moves it 5 units above the XZ-plane
        cam.orthographic = true; //for this example we use an orthographic camera.
        cam.transform.rotation = Quaternion.Euler(35, 30, 0); //rotates the camera to it looks at the XZ-plane
        cam.orthographicSize =
            hexMap.MapSizeData.extents.z * 1.5f * 0.8f; // sets orthographic size of the camera.]
        cam.nearClipPlane = -50f;
        //this does not account for aspect ratio but for our purposes it works good enough.
    }

    private void OnGUI()
    {
        GUIStyle customButton = new GUIStyle("button")
        {
            fontSize = 24
        };
        if (GUI.Button(new Rect(100, 100, 250, 50), "Load Level", customButton))
        {
            ClearBoard();
            StartCoroutine(GenerateBoardFromLevelData());
        }

        if (GUI.Button(new Rect(100, 200, 250, 50), "Generate Random", customButton))
        {
            ClearBoard();
            StartCoroutine(GenerateRandomBoard());
        }
    }


    private TilePrototype GetRandomTileProto()
    {
        return tileCollection.tiles[Random.Range(0, tileCollection.tiles.Count)];
    }
}