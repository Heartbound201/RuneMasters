using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wunderwunsch.HexMapLibrary.Generic;

public class EnemyBrush : MonoBehaviour, IBrush
{
    public GameObject brushPrefab;
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
        // clear previous unit on tile
        if (tile.Data.content.Count > 0)
        {
            Unit unit = tile.Data.Unit;
            if (unit != null) Destroy(unit.gameObject);
        }

        tile.Data.content.Clear();

        // place unit on leveldata
        levelData.PlaceEnemy(tile.Index, brushPrefab);

        // instantiate graphics
        GameObject instance = Instantiate(brushPrefab, boardParent);
        instance.transform.position = tile.CartesianPosition;
        Unit character = instance.GetComponent<PlayerUnit>();
        tile.Data.content.Add(character);
    }
    public void AddButtonListener(UnityAction action)
    {
        btn.onClick.AddListener(action);
    }
}