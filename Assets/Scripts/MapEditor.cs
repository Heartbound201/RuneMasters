using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
using Random = UnityEngine.Random;

public class MapEditor : MonoBehaviour
{
    private HexMouse hexMouse;

    public HexMap<Tile, int> hexMap;
    private HexTile<Tile> tileHover;
    private HexTile<Tile> tileSelected;

    public Camera cam;
    public LevelData levelData;
    public TileCollection tileCollection;
    public TilePrototype defaultTile;
    public List<Unit> charactersPrefabs;
    public List<Unit> enemiesPrefabs;

    public int radius = 5;

    [Tooltip("Apply a random rotation to tiles")]
    public bool applyRandomRotation = false;

    public PlacementPanelController placementPanelController;

    public GameObject edgePrefab;

    private GameObject[] edges;
    private GameObject[] tiles;

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
            StartCoroutine(GenerateRandomBoard());
        }
        else
        {
            StartCoroutine(GenerateBoardFromLevelData());
        }

        SetCamera();
        FillPlacementOptions();
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
        }

        if (Input.GetMouseButtonDown(1))
        {
            ClearTile(t);
        }
    }

    private void FillPlacementOptions()
    {
        foreach (TilePrototype tilePrototype in tileCollection.tiles)
        {
            placementPanelController.CreatePlacementButton(tilePrototype.name,
                () => { ChangeTilePrototype(this.tileSelected, tilePrototype); });
        }

        foreach (Unit unit in charactersPrefabs)
        {
            placementPanelController.CreatePlacementButton(unit.name,
                () => { PlaceCharacter(this.tileSelected, unit.gameObject); });
        }

        foreach (Unit unit in enemiesPrefabs)
        {
            placementPanelController.CreatePlacementButton(unit.name,
                () => { PlaceEnemy(this.tileSelected, unit.gameObject); });
        }
    }

    private void ClearTile(HexTile<Tile> hexTile)
    {
        levelData.PlaceEnemy(hexTile.Index, null);
        levelData.PlaceUnit(hexTile.Index, null);
        levelData.SwapTilePrototype(hexTile.Index, defaultTile);
    }

    private void ChangeTilePrototype(HexTile<Tile> hexTile, TilePrototype tilePrototype)
    {
        Destroy(hexTile.Data.gameObject);
        GameObject instance = Instantiate(tilePrototype.prefab, transform);
        hexTile.Data = instance.GetComponent<Tile>();
        instance.transform.position = hexTile.CartesianPosition;
        instance.gameObject.name = "Hex" + hexTile.CartesianPosition;
        hexTile.Data.prototype = tilePrototype;

        levelData.SwapTilePrototype(hexTile.Index, tilePrototype);
    }

    private void PlaceCharacter(HexTile<Tile> hexTile, GameObject go)
    {
        // clear previous unit on tile
        if (hexTile.Data.unitList.Count > 0)
        {
            Unit unit = hexTile.Data.unitList[0];
            if (unit != null) Destroy(unit.gameObject);
        }

        hexTile.Data.unitList.Clear();

        // place unit on leveldata
        levelData.PlaceUnit(hexTile.Index, go);

        // instantiate graphics
        if (go != null)
        {
            GameObject instance = Instantiate(go, transform);
            instance.transform.position = hexTile.CartesianPosition;
            Unit character = instance.GetComponent<PlayerUnit>();
            hexTile.Data.unitList.Add(character);
        }
    }

    private void PlaceEnemy(HexTile<Tile> hexTile, GameObject go)
    {
        // clear previous unit on tile
        if (hexTile.Data.unitList.Count > 0)
        {
            Unit unit = hexTile.Data.unitList[0];
            if (unit != null) Destroy(unit.gameObject);
        }

        hexTile.Data.unitList.Clear();

        // place unit on leveldata
        levelData.PlaceEnemy(hexTile.Index, go);

        // instantiate graphics
        if (go != null)
        {
            GameObject instance = Instantiate(go, transform);
            instance.transform.position = hexTile.CartesianPosition;
            Unit character = instance.GetComponent<EnemyUnit>();
            hexTile.Data.unitList.Add(character);
        }
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

            PlaceCharacter(tile, levelData.GetUnitAt(tile.Index));
            PlaceEnemy(tile, levelData.GetEnemyAt(tile.Index));

            yield return null;
        }

        GenerateEdges();

        yield return null;
    }

    private IEnumerator GenerateRandomBoard()
    {
        ClearBoard();
        hexMap = new HexMap<Tile, int>(HexMapBuilder.CreateHexagonalShapedMap(radius), null);
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

    private void SelectTile(HexTile<Tile> tile)
    {
        if (tileSelected == tile) return;
        if (tileSelected != null && tileSelected.Data != null)
        {
            tileSelected.Data.Highlight(false);
        }

        tileSelected = tile;
        tile.Data.Highlight(true);
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
            hexMap.MapSizeData.extents.z * 2 * 0.8f; // sets orthographic size of the camera.]
        cam.nearClipPlane = -12f;
        //this does not account for aspect ratio but for our purposes it works good enough.
    }

    private LevelData CreateLevelData()
    {
        LevelData data = ScriptableObject.CreateInstance<LevelData>();
        string fileName =
            $"Assets/ScriptableObjects/{name}_{DateTime.Today.Year}_{DateTime.Today.Month}_{DateTime.Today.Day}_{Guid.NewGuid()}.asset";
        AssetDatabase.CreateAsset(data, fileName);
        return data;
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

        if (GUI.Button(new Rect(100, 300, 250, 50), "Save as New", customButton))
        {
            SaveAsNewLevel();
        }
    }

    private void SaveAsNewLevel()
    {
        var data = CreateLevelData();
        data.boardRadius = radius;
        foreach (var tile in hexMap.Tiles)
        {
            if (tile.Data.unitList.Count > 0 && tile.Data.unitList[0] is PlayerUnit)
            {
                data.PlaceUnit(tile.Index, tile.Data.unitList[0].gameObject);
            }
            if (tile.Data.unitList.Count > 0 && tile.Data.unitList[0] is EnemyUnit)
            {
                data.PlaceEnemy(tile.Index, tile.Data.unitList[0].gameObject);
            }
            data.SwapTilePrototype(tile.Index, tile.Data.prototype);
        }
        EditorUtility.SetDirty(data);
    }

    private TilePrototype GetRandomTileProto()
    {
        return tileCollection.tiles[Random.Range(0, tileCollection.tiles.Count)];
    }
}