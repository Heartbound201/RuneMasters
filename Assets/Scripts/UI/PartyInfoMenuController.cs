using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyInfoMenuController : MonoBehaviour
{
    public TextMeshProUGUI partyHealthText;
    public TextMeshProUGUI partyManaText;
    public TextMeshProUGUI partyManaReserveText;
    public Button endTurnBtn;
    
    public void UpdatePartyInfo(Party party)
    {
        partyHealthText.text = party.health.ToString();
        partyManaText.text = party.mana.ToString();
        partyManaReserveText.text = party.manaReserve.ToString();
    }
    
}
