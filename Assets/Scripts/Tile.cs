using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using Wunderwunsch.HexMapLibrary.Generic;

public class Tile : MonoBehaviour
{
    public TilePrototype prototype;
    
    public Board board;
    public List<Unit> unitList;
    public bool isPassable;

    public bool IsHighlighted;
    public bool IsSelected;
    public bool IsHovered;

    public List<AIPlan> dangerList = new List<AIPlan>();
    public Vector3Int pos;
    public Vector3 posCart;
    public Vector2 posNorm;

    private MeshRenderer _renderer { get { return GetComponentInChildren<MeshRenderer>(); } }
    private Color originColor;

    [HideInInspector] public HexTile<Tile> _prev;
    [HideInInspector] public int _distance;

    void OnGUI(){
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        string text = $"{pos.x:0.00},{pos.y:0.00}";
        Vector3 textSize = GUI.skin.label.CalcSize(new GUIContent(text));
        GUIStyle guiStyle = new GUIStyle()
        {
            fontSize = 24
        };
        GUI.Label(new Rect(position.x, Screen.height - position.y, textSize.x, textSize.y), text, guiStyle);
    }
    private void Start()
    {
        originColor = _renderer.material.color;
        unitList = new List<Unit>();
    }
    public void Select(bool value)
    {
        this.IsSelected = value;
    }

    public void Endanger(AIPlan danger)
    {
        dangerList.Add(danger);
        RenderColor();
    }
    
    public void SolveDanger(AIPlan danger)
    {
        dangerList.Remove(danger);
        RenderColor();
    }
    public void Hover(bool value)
    {
        this.IsHovered = value;
        RenderColor();
    }

    public void Highlight(bool value)
    {
        this.IsHighlighted = value;
        RenderColor();
    }

    public void RenderColor()
    {
        if (IsHovered)
        {
            _renderer.material.color = Color.white;
            return;
        }

        if (IsHighlighted && dangerList.Count > 0)
        {
            _renderer.material.color = Color.Lerp(Color.red, Color.cyan, 0.2f);
            return;
        }
        
        if (IsHighlighted)
        {
            _renderer.material.color = Color.cyan;
            return;
        }

        if (dangerList.Count > 0)
        {
            _renderer.material.color = Color.red;
            return;
        }
        
        _renderer.material.color = originColor;
    }
    public bool IsPassable
    {
        get
        {
            return isPassable && unitList.TrueForAll(u => u.isPassable);
        }
    }


}
