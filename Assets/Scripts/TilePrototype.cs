using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Create Tile")]
public class TilePrototype : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
}