using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rune", menuName = "Create Rune")]
public class RunePrototype : ScriptableObject
{
    public Sprite icon;
    public string abilityName;
    public string abilityDescription;
    public List<Vector3> steps = new List<Vector3>();
}