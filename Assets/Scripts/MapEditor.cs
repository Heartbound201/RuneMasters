using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class MapEditor : MonoBehaviour
{
    private HexMouse hexMouse;
    public HexMap<Tile> hexMap;
    public TileCollection tileCollection;

    public HexTile<Tile> tileHover;
    public List<HexTile<Tile>> tileSelection = new List<HexTile<Tile>>();


    [Header("Level Data")] public int radius = 7;
    public List<TileInfo> tiles = new List<TileInfo>();
    public List<SpawnInfo> unitSpawnTiles = new List<SpawnInfo>();
    public List<SpawnInfo> enemySpawnTiles = new List<SpawnInfo>();

    private void Awake()
    {
        hexMouse = gameObject.AddComponent<HexMouse>();
    }

    void Start()
    {
        StartCoroutine(GenerateBoard());
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
            RotateTileCollection(t);
        }
    }

    private void RotateTileCollection(HexTile<Tile> hexTile)
    {
        int index = tileCollection.tiles.FindIndex(t => t == hexTile.Data.prototype);
        if (index >= tileCollection.tiles.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }

        Destroy(hexTile.Data.gameObject);
        TilePrototype tilePrototype = tileCollection.tiles[index];
        GameObject instance = Instantiate(tilePrototype.prefab, transform);
        hexTile.Data = instance.GetComponent<Tile>();
        instance.transform.position = hexTile.CartesianPosition;
        instance.gameObject.name = "Hex" + hexTile.CartesianPosition;
        hexTile.Data.prototype = tilePrototype;

        TileInfo tileInfo = tiles.Find(ti => ti.index == hexTile.Index);
        tileInfo.proto = tilePrototype;
    }

    public IEnumerator GenerateBoard()
    {
        hexMap = new HexMap<Tile>(HexMapBuilder.CreateHexagonalShapedMap(radius), null);
        hexMouse.Init(hexMap);
        foreach (var tile in hexMap.Tiles)
        {
            TilePrototype tilePrototype = tileCollection.tiles[0];
            GameObject instance = Instantiate(tilePrototype.prefab, transform);
            instance.transform.position = tile.CartesianPosition;
            instance.gameObject.name = "Hex" + tile.CartesianPosition;
            tile.Data = instance.GetComponent<Tile>();
            tile.Data.prototype = tilePrototype;

            tiles.Add(new TileInfo
            {
                index = tile.Index,
                proto = tilePrototype
            });
            yield return null;
        }

        yield return null;
        SetCamera();
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
        if (tileSelection.Contains(tile)) return;
        tileSelection.Add(tile);
        tile.Data.Highlight(true);
    }

    public void SetCamera()
    {
        //put the following at the end of the start method (or in its own method called after map creation)
        Camera.main.transform.position =
            new Vector3(hexMap.MapSizeData.center.x, 4,
                hexMap.MapSizeData.center.z); // centers the camera and moves it 5 units above the XZ-plane
        Camera.main.orthographic = true; //for this example we use an orthographic camera.
        Camera.main.transform.rotation = Quaternion.Euler(35, 30, 0); //rotates the camera to it looks at the XZ-plane
        Camera.main.orthographicSize =
            hexMap.MapSizeData.extents.z * 2 * 0.8f; // sets orthographic size of the camera.]
        Camera.main.nearClipPlane = -12f;
        //this does not account for aspect ratio but for our purposes it works good enough.
    }

    public void Save()
    {
        string filePath = Application.dataPath + "/ScriptableObjects";

        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        levelData.boardRadius = radius;
        levelData.tileCollection = tileCollection;
        levelData.tiles = tiles;
        levelData.characters = unitSpawnTiles;
        levelData.enemies = enemySpawnTiles;

        string fileName = string.Format("Assets/ScriptableObjects/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(levelData, fileName);
    }

    private void OnGUI()
    {
        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = 24;
        if (GUI.Button(new Rect(100, 100, 250, 50), "Create Level", customButton))
        {
            Save();
            ClearSelection();
        }

        if (GUI.Button(new Rect(100, 150, 250, 50), "Char Spawn Tiles", customButton))
        {
            foreach (HexTile<Tile> hexTile in tileSelection)
            {
                unitSpawnTiles.Add(new SpawnInfo() {index = hexTile.Index});
            }

            ClearSelection();
        }

        if (GUI.Button(new Rect(100, 200, 250, 50), "Enemy Spawn Tiles", customButton))
        {
            foreach (HexTile<Tile> hexTile in tileSelection)
            {
                enemySpawnTiles.Add(new SpawnInfo() {index = hexTile.Index});
            }

            ClearSelection();
        }

        if (GUI.Button(new Rect(100, 250, 250, 50), "Clear Level", customButton))
        {
            foreach (HexTile<Tile> hexTile in hexMap.Tiles)
            {
                Destroy(hexTile.Data.gameObject);
                TilePrototype tilePrototype = tileCollection.tiles[0];
                GameObject instance = Instantiate(tilePrototype.prefab, transform);
                hexTile.Data = instance.GetComponent<Tile>();
                instance.transform.position = hexTile.CartesianPosition;
                instance.gameObject.name = "Hex" + hexTile.CartesianPosition;
                hexTile.Data.prototype = tilePrototype;
                TileInfo tileInfo = tiles.Find(ti => ti.index == hexTile.Index);
                tileInfo.proto = tilePrototype;
            }

            unitSpawnTiles.Clear();
            enemySpawnTiles.Clear();
            ClearSelection();
        }
    }

    private void ClearSelection()
    {
        foreach (HexTile<Tile> hexTile in tileSelection)
        {
            hexTile.Data.Highlight(false);
        }

        tileSelection.Clear();
    }
}