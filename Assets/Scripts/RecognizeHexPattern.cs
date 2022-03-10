using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class RecognizeHexPattern : MonoBehaviour
{
    
    private HexMouse hexMouse;
    public HexMap<Tile> hexMap;
    public TileCollection tileCollection;
    
    public HexTile<Tile> tileHover;
    public List<HexTile<Tile>> tileSelection = new List<HexTile<Tile>>();
    
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
    }
    
    public IEnumerator GenerateBoard()
    {
        hexMap = new HexMap<Tile>(HexMapBuilder.CreateHexagonalShapedMap(5), null);
        hexMouse.Init(hexMap);
        foreach (var tile in hexMap.Tiles)
        {
            TilePrototype tilePrototype = tileCollection.GetRandomTile();
            GameObject instance = Instantiate(tilePrototype.prefab, transform);
            instance.transform.position = tile.CartesianPosition;
            instance.gameObject.name = "Hex" + tile.CartesianPosition;
            tile.Data = instance.GetComponent<Tile>();

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
        Camera.main.transform.position = new Vector3(hexMap.MapSizeData.center.x, 4, hexMap.MapSizeData.center.z); // centers the camera and moves it 5 units above the XZ-plane
        Camera.main.orthographic = true; //for this example we use an orthographic camera.
        Camera.main.transform.rotation = Quaternion.Euler(35, 30, 0); //rotates the camera to it looks at the XZ-plane
        Camera.main.orthographicSize =
            hexMap.MapSizeData.extents.z * 2 * 0.8f; // sets orthographic size of the camera.]
        Camera.main.nearClipPlane = -12f;
        //this does not account for aspect ratio but for our purposes it works good enough.
    }

    private void OnGUI()
    {
        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = 24;
        if (GUI.Button(new Rect(100, 100, 200, 50), "Create Rune", customButton))
        {
            List<Vector3Int> vector3Ints = tileSelection.Select(t => t.Position).ToList();
            List<Vector3Int> corners = hexMap.GetCornerPositions.TileBorders(vector3Ints);
            List<Vector3Int> edges = hexMap.GetEdgePositions.TileBorders(vector3Ints);
            
            Vector3Int totCorners = default;
            foreach (var vector3Int in corners)
            {
                totCorners += vector3Int;
            }
            Debug.Log("corners sum: " + totCorners + " magnitude: " + totCorners.magnitude);

            Vector3Int totEdges = default;
            foreach (var vector3Int in edges)
            {
                totEdges += vector3Int;
            }
            Debug.Log("edges sum: " + totEdges + " magnitude: " + totEdges.magnitude);

            Vector3Int totTiles = default;
            int totIndexes = 0;
            foreach (HexTile<Tile> hexTile in tileSelection)
            {
                totTiles += hexTile.Position;
                totIndexes += hexTile.Index;
            }
            Debug.Log("tiles sum: " + totTiles + " magnitude: " + totTiles.magnitude + " index: " + totIndexes);
            Debug.Log("corner / tiles " + totCorners.magnitude/totTiles.magnitude + " " + totCorners.magnitude/totIndexes);
        }

        if (GUI.Button(new Rect(100, 150, 200, 50), "Clear Rune", customButton))
        {
            foreach (HexTile<Tile> hexTile in tileSelection)
            {
                hexTile.Data.Highlight(false);
            }
            tileSelection.Clear();
        }
    }
}
