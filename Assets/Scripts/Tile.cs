using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Board board;
    public Unit unit;
    public bool isPassable;

    public bool IsHighlighted;
    public bool IsSelected;
    private MeshRenderer _renderer { get { return GetComponentInChildren<MeshRenderer>(); } }

    [HideInInspector] public Tile _prev;
    [HideInInspector] public int _distance;
    public void Select(bool value)
    {
        this.IsSelected = value;
    }

    public void Highlight(bool value)
    {
        this.IsHighlighted = value;
        if (value)
        {
            _renderer.material.color = Color.cyan;
        }
        else
        {
            _renderer.material.color = Color.white;
        }
    }
    public bool IsPassable
    {
        get
        {
            return isPassable && unit.isPassable;
        }
    }


}
