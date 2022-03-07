using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyInfoMenuController : MonoBehaviour
{
    public Party party;

    public TextMeshProUGUI partyHealthText;
    public TextMeshProUGUI partyManaText;
    public TextMeshProUGUI partyManaReserveText;
    
    void Start()
    {
        UpdatePartyInfo();
    }

    void UpdatePartyInfo()
    {
        partyHealthText.text = party.health.ToString();
        partyManaText.text = party.mana.ToString();
        partyManaReserveText.text = party.manaReserve.ToString();
    }
    
}
