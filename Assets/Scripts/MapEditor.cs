using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class MapEditor : MonoBehaviour
{
    private HexMouse hexMouse;
    private HexMap<Tile> hexMap;
    private HexTile<Tile> tileHover;
    private HexTile<Tile> tileSelected;

    public Camera cam;
    public LevelData levelData;
    public TileCollection tileCollection;
    public TilePrototype defaultTile;
    public List<Unit> charactersPrefabs;
    public List<Unit> enemiesPrefabs;
    public PlacementPanelController placementPanelController;

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
            levelData = CreateLevelData();
        }

        StartCoroutine(GenerateBoard());
        SetCamera();
        FillPlacementOptions();
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

    private void ClearTile(HexTile<Tile> hexTile)
    {
        if (levelData.characters.Exists(info => info.index == hexTile.Index))
        {
            if (hexTile.Data.unitList.Count > 0)
            {
                Unit unit = hexTile.Data.unitList[0];
                if (unit != null) Destroy(unit.gameObject);
            }

            hexTile.Data.unitList.Clear();
            SpawnInfo spawnInfo = levelData.characters.Find(info => info.index == hexTile.Index);
            levelData.characters.Remove(spawnInfo);
        }
        else if (levelData.enemies.Exists(info => info.index == hexTile.Index))
        {
            if (hexTile.Data.unitList.Count > 0)
            {
                Unit unit = hexTile.Data.unitList[0];
                if (unit != null) Destroy(unit.gameObject);
            }

            hexTile.Data.unitList.Clear();
            SpawnInfo spawnInfo = levelData.enemies.Find(info => info.index == hexTile.Index);
            levelData.enemies.Remove(spawnInfo);
        }
        else
        {
            ChangeTilePrototype(hexTile, defaultTile);
        }
    }

    private void ChangeTilePrototype(HexTile<Tile> hexTile, TilePrototype tilePrototype)
    {
        Destroy(hexTile.Data.gameObject);
        GameObject instance = Instantiate(tilePrototype.prefab, transform);
        hexTile.Data = instance.GetComponent<Tile>();
        instance.transform.position = hexTile.CartesianPosition;
        instance.gameObject.name = "Hex" + hexTile.CartesianPosition;
        hexTile.Data.prototype = tilePrototype;

        TileInfo tileInfo = levelData.tiles.Find(ti => ti.index == hexTile.Index);
        tileInfo.proto = tilePrototype;
    }

    private void PlaceCharacter(HexTile<Tile> hexTile, GameObject go)
    {
        if (levelData.characters.Exists(info => info.index == hexTile.Index))
        {
            if(hexTile.Data.unitList.Count > 0)
            {
                Unit unit = hexTile.Data.unitList[0];
                if (unit != null) Destroy(unit.gameObject);
            }
            hexTile.Data.unitList.Clear();
            SpawnInfo spawnInfo = levelData.characters.Find(info => info.index == hexTile.Index);
            spawnInfo.obj = go;
        }

        GameObject instance = Instantiate(go, transform);
        instance.transform.position = hexTile.CartesianPosition;
        Unit character = instance.GetComponent<PlayerUnit>();
        hexTile.Data.unitList.Add(character);

        if (!levelData.characters.Exists(info => info.index == hexTile.Index))
        {
            levelData.characters.Add(new SpawnInfo()
            {
                index = hexTile.Index,
                obj = go
            });
        }
    }

    private void PlaceEnemy(HexTile<Tile> hexTile, GameObject go)
    {
        if (levelData.enemies.Exists(info => info.index == hexTile.Index))
        {
            if (hexTile.Data.unitList.Count > 0)
            {
                Unit unit = hexTile.Data.unitList[0];
                if (unit != null) Destroy(unit.gameObject);
            }

            hexTile.Data.unitList.Clear();
            SpawnInfo spawnInfo = levelData.enemies.Find(info => info.index == hexTile.Index);
            spawnInfo.obj = go;
        }

        GameObject instance = Instantiate(go, transform);
        instance.transform.position = hexTile.CartesianPosition;
        Unit character = instance.GetComponent<EnemyUnit>();
        hexTile.Data.unitList.Add(character);

        if (!levelData.enemies.Exists(info => info.index == hexTile.Index))
        {
            levelData.enemies.Add(new SpawnInfo()
            {
                index = hexTile.Index,
                obj = go
            });
        }
    }

    private IEnumerator GenerateBoard()
    {
        hexMap = new HexMap<Tile>(HexMapBuilder.CreateHexagonalShapedMap(levelData.boardRadius), null);
        hexMouse.Init(hexMap);
        foreach (var tile in hexMap.Tiles)
        {
            TilePrototype tilePrototype = defaultTile;

            if (levelData.tiles.Exists(info => info.index == tile.Index))
            {
                tilePrototype = levelData.tiles.Find(info => info.index == tile.Index).proto;
            }

            GameObject instance = Instantiate(tilePrototype.prefab, transform);
            instance.transform.position = tile.CartesianPosition;
            instance.gameObject.name = "Hex" + tile.CartesianPosition;
            tile.Data = instance.GetComponent<Tile>();
            tile.Data.prototype = tilePrototype;

            if (!levelData.tiles.Exists(info => info.index == tile.Index))
            {
                levelData.tiles.Add(new TileInfo
                {
                    index = tile.Index,
                    proto = tilePrototype
                });
            }

            if (levelData.characters.Exists(info => info.index == tile.Index))
            {
                PlaceCharacter(tile, levelData.characters.Find(info => info.index == tile.Index).obj);
            }

            if (levelData.enemies.Exists(info => info.index == tile.Index))
            {
                PlaceEnemy(tile, levelData.enemies.Find(info => info.index == tile.Index).obj);
            }

            yield return null;
        }

        yield return null;
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
            $"Assets/ScriptableObjects/{name}{DateTime.Today.Year}{DateTime.Today.Month}{DateTime.Today.Day}.asset";
        AssetDatabase.CreateAsset(data, fileName);
        return data;
    }

    private void OnGUI()
    {
        GUIStyle customButton = new GUIStyle("button")
        {
            fontSize = 24
        };
        if (GUI.Button(new Rect(100, 100, 250, 50), "Reload Level", customButton))
        {
            ClearBoard();
            StartCoroutine(GenerateBoard());
        }
    }
}