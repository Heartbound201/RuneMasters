using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class Tile : MonoBehaviour
{
    public Board board;
    public Unit unit;
    public bool isPassable;

    public bool IsHighlighted;
    public bool IsSelected;
    public bool IsWarning;
    public bool IsHovered;
    private MeshRenderer _renderer { get { return GetComponentInChildren<MeshRenderer>(); } }
    private Color originColor;

    private void Start()
    {
        originColor = _renderer.material.color;
    }

    [HideInInspector] public HexTile<Tile> _prev;
    [HideInInspector] public int _distance;
    public void Select(bool value)
    {
        this.IsSelected = value;
    }

    public void Warning(bool value)
    {
        this.IsWarning = value;
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

        if (IsHighlighted)
        {
            _renderer.material.color = Color.cyan;
            return;
        }

        if (IsWarning)
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
            return isPassable && unit.isPassable;
        }
    }


}
