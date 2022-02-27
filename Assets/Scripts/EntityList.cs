using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New entity List", menuName = "Create Entity List")]
public class EntityList : ScriptableObject
{
    public List<Entity> entities = new List<Entity>();
}
