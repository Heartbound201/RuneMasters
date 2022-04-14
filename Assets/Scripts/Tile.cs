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
    public List<BoardObject> content;

    public List<IDamageable> Damageables
    {
        get
        {
             return content.FindAll(c => c is IDamageable).Cast<IDamageable>().ToList();
            
        }
    }
    public Unit Unit
    {
        get
        {
            var bo = content.Find(c => c is Unit);
            if (bo == null) return null;
            return (Unit) bo;
        }
    }

    public bool isPassable;
    public bool IsHighlighted;
    public bool IsSelected;
    public bool IsHovered;

    public List<AIPlan> dangerList = new List<AIPlan>();
    public Vector3Int pos;
    public Vector3 posCart;
    public Vector2 posNorm;

    public Color highlightColor;
    public Color dangerColor;
    public Color hoverColor;

    private MeshRenderer _renderer
    {
        get
        {
            try
            {
                return GetComponentInChildren<MeshRenderer>();
            }
            catch
            {
                return null;
            }
        }
    }

    private Color originColor;

    [HideInInspector] public HexTile<Tile> _prev;
    [HideInInspector] public int _distance;


    // HexTile<> .position = cube coord (q + r + s = 0)
    // pos.x = s        pos.y = r       pos.z = q
    // void OnGUI(){
    //     Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
    //     string text = $"{pos.x:0},{pos.y:0},{pos.z:0}";
    //     Vector3 textSize = GUI.skin.label.CalcSize(new GUIContent(text));
    //     GUIStyle guiStyle = new GUIStyle()
    //     {
    //         fontSize = 24
    //     };
    //     GUI.Label(new Rect(position.x, Screen.height - position.y, textSize.x, textSize.y), text, guiStyle);
    // }
    private void Start()
    {
        originColor = _renderer.material.color;
        content = new List<BoardObject>();
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
        if (_renderer == null) return;
        if (IsHovered)
        {
            _renderer.material.color = hoverColor;
            return;
        }

        if (IsHighlighted && dangerList.Count > 0)
        {
            _renderer.material.color = Color.Lerp(dangerColor, highlightColor, 0.3f);
            return;
        }

        if (IsHighlighted)
        {
            _renderer.material.color = highlightColor;
            return;
        }

        if (dangerList.Count > 0)
        {
            _renderer.material.color = dangerColor;
            return;
        }

        _renderer.material.color = originColor;
    }

    public bool IsPassable
    {
        get { return isPassable && content.TrueForAll(u => u.isPassable); }
    }
}