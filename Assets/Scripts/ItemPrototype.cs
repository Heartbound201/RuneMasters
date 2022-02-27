using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Create Item")]
public class ItemPrototype : ScriptableObject
{
    public string itemName;
    public string itemDescription;
}
