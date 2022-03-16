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
    
    private MeshRenderer _renderer { get { return GetComponentInChildren<MeshRenderer>(); } }
    private Color originColor;

    private void Start()
    {
        originColor = _renderer.material.color;
        unitList = new List<Unit>();
    }

    [HideInInspector] public HexTile<Tile> _prev;
    [HideInInspector] public int _distance;
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
