using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class Ability : MonoBehaviour
{
    public AbilityArea abilityArea;
    public AbilityRange abilityRange;
    public List<AbilityEffect> abilityEffects = new List<AbilityEffect>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Execute(Unit actor, HexTile<Tile> targetTile)
    {
        throw new System.NotImplementedException();
    }
}
