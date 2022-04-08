using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Wunderwunsch.HexMapLibrary;

[CreateAssetMenu(fileName = "New Rune", menuName = "Create Rune")]
public class RunePrototype : ScriptableObject
{
    public Sprite icon;
    public string runeName;
    public string description;
    public List<TileDirection> steps = new List<TileDirection>();
    public Ability ability;
    public int Cost => steps.Count;
    public int cooldown;
    public Category category;
    public Sprite categorySprite;
}

public enum Category
{
    Damage,
    DamageOvertime,
    Heal,
    Buff,
    Debuff,
    MapInteraction

}