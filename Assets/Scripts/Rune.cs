using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;

[CreateAssetMenu(fileName = "New Rune", menuName = "Create Rune")]
public class Rune : ScriptableObject
{
    public Sprite icon;
    public string runeName;
    public string runeDescription;
    public List<TileDirection> steps = new List<TileDirection>();
    public Ability ability;
    public GameObject runePrefab;
    public int Cost => steps.Count;
}