using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool hasMoved;
    public bool hasActed;
    public List<ItemPrototype> items = new List<ItemPrototype>();
    public List<AbilityPrototype> abilities = new List<AbilityPrototype>();
}
