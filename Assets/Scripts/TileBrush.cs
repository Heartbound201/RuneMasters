using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wunderwunsch.HexMapLibrary.Generic;

public class TileBrush : MonoBehaviour, IBrush
{
    public TilePrototype brushPrefab;
    public Button btn;
    public TMP_Text text;

    private void Start()
    {
        text.text = brushPrefab.name;
    }

    public void Select(bool value)
    {
        btn.interactable = !value;
    }
    
    public void Paint(LevelData levelData, HexTile<Tile> tile, Transform boardParent)
    {
        Destroy(tile.Data.gameObject);
        GameObject instance = Instantiate(brushPrefab.prefab, boardParent);
        tile.Data = instance.GetComponent<Tile>();
        instance.transform.position = tile.CartesianPosition;
        instance.gameObject.name = "Hex" + tile.CartesianPosition;
        tile.Data.prototype = brushPrefab;

        levelData.PlaceTilePrototype(tile.Index, brushPrefab);
    }

    public void AddButtonListener(UnityAction action)
    {
        btn.onClick.AddListener(action);
    }
}