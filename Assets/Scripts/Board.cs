using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    private HexMouse hexMouse;
    public static event Action<HexTile<TileData>> SelectTileEvent;
    
    public int radius = 5;
    public TileCollection tileCollection;

    public HexMap<TileData> hexMap;
    public Player player;
    public GameObject genericAllyPrefab;
    public GameObject genericEnemyPrefab;

    
    public HexTile<TileData> tileHover;
    public HexTile<TileData> tileSelection;

    [Header("Materials")]
    public Material tileHoverMat;
    public Material tileSelectionMat;
    
    [Header("Audio")]
    public AudioClip tileHoverSfx;
    public AudioClip tileSelectionSfx;
    public AudioClip unitPlacementSfx;

    private void Start()
    {
        hexMouse = gameObject.AddComponent<HexMouse>();
    }
    
    void Update()
    {
        if (!hexMouse.CursorIsOnMap) return;
        Vector3Int mouseTilePosition = hexMouse.TileCoord;
        HexTile<TileData> t = hexMap.TilesByPosition[mouseTilePosition];
        HoverTile(t);
        
        if (Input.GetMouseButtonDown(0))
        {
            SelectTile(t);
            SelectTileEvent?.Invoke(t);
        }
    }

    public void HoverTile(HexTile<TileData> tile)
    {
        if(tileHover != null && tileHover != tile)
            tileHover.Data.tile.renderer.material = tileHover.Data.tile.originalMat;
        
        if(tileHover == tileSelection) return; 
        
        tileHover = tile;
        
        tileHover.Data.tile.renderer.material = tileHoverMat;
    }
    public void SelectTile(HexTile<TileData> tile)
    {
        if(tileSelection != null && tileSelection != tile)
            tileSelection.Data.tile.renderer.material = tileSelection.Data.tile.originalMat;
        
        tileSelection = tile;
        
        tileSelection.Data.tile.renderer.material = tileSelectionMat;
    }
    public void GenerateBoard()
    {
        StartCoroutine(GenerateBoardAsync());
        SetCamera();
    }

    public void SpawnAllyInRandomPosition()
    {
        HexTile<TileData> hexTile;
        do
        {
            hexTile = hexMap.Tiles[Random.Range(0, hexMap.Tiles.Length)];
        } while (hexTile.Data.Entity != null);

        SpawnEntity(genericAllyPrefab, hexTile.Position);
    }
    
    public void SpawnEnemyInRandomPosition()
    {
        HexTile<TileData> hexTile;
        do
        {
            hexTile = hexMap.Tiles[Random.Range(0, hexMap.Tiles.Length)];
        } while (hexTile.Data.Entity != null);

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
        HexTile<TileData> hexTile = hexMap.TilesByPosition[pos];
        GameObject o = Instantiate(obj, hexTile.CartesianPosition, Quaternion.identity);
        Entity entity = o.GetComponent<Entity>();
        entity.tile = hexTile;
        hexTile.Data.content.Add(entity); 
        
    }

    private IEnumerator GenerateBoardAsync()
    {
        hexMap = new HexMap<TileData>(HexMapBuilder.CreateHexagonalShapedMap(radius), null);                        
        hexMouse.Init(hexMap);
        foreach (var tile in hexMap.Tiles) 
        {
            TilePrototype tilePrototype = tileCollection.GetRandomTile();
            GameObject instance = Instantiate(tilePrototype.prefab, transform);
            instance.transform.position = tile.CartesianPosition;
            instance.gameObject.name = "Hex" + tile.CartesianPosition;
            tile.Data.board = this;
            tile.Data.tile = instance.GetComponent<Tile>();
            
            yield return new WaitForSeconds(0.05f);
        }

        yield return null;
    }
    

    private void SetCamera()
    {
        //put the following at the end of the start method (or in its own method called after map creation)
        Camera.main.transform.position = new Vector3(hexMap.MapSizeData.center.x, 4, hexMap.MapSizeData.center.z); // centers the camera and moves it 5 units above the XZ-plane
        Camera.main.orthographic = true; //for this example we use an orthographic camera.
        Camera.main.transform.rotation = Quaternion.Euler(35, 30, 0); //rotates the camera to it looks at the XZ-plane
        Camera.main.orthographicSize = hexMap.MapSizeData.extents.z * 2 * 0.8f; // sets orthographic size of the camera.]
        Camera.main.nearClipPlane = -12f;
        //this does not account for aspect ratio but for our purposes it works good enough.
    }

}