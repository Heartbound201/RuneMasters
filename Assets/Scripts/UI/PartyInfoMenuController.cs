using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyInfoMenuController : MonoBehaviour
{
    public TextMeshProUGUI partyHealthText;
    public TextMeshProUGUI partyHealthMaxText;
    public TextMeshProUGUI partyManaText;
    public TextMeshProUGUI partyManaMaxText;
    public TextMeshProUGUI partyManaReserveText;
    public TextMeshProUGUI partyManaReserveMaxText;
    public Image HealthBar;
    public Image ManaBar;
    public Image ReserveBar;
    public Button endTurnBtn;
    
    public void UpdatePartyInfo(Party party)
    {
        partyHealthText.text = party.health.ToString();
        partyHealthMaxText.text = party.healthMax.ToString();
        partyManaText.text = party.mana.ToString();
        partyManaMaxText.text = party.manaMax.ToString();
        partyManaReserveText.text = party.manaReserve.ToString();
        partyManaReserveMaxText.text = party.manaReserveMax.ToString();

        HealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(5 * party.health, HealthBar.GetComponent<RectTransform>().rect.height);
        ManaBar.GetComponent<RectTransform>().sizeDelta = new Vector2(5 * party.mana, ManaBar.GetComponent<RectTransform>().rect.height);
        ReserveBar.GetComponent<RectTransform>().sizeDelta = new Vector2(5 * party.manaReserve, ReserveBar.GetComponent<RectTransform>().rect.height);
    }
    
}
