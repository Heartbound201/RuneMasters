using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Ability", menuName = "Create Ability")]
public class AbilityPrototype : ScriptableObject
{
    public string abilityName;
    public string abilityDescription;
}