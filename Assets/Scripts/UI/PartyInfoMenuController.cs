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
        partyHealthText.text = party.Health.ToString();
        partyHealthMaxText.text = party.HealthMax.ToString();
        partyManaText.text = party.Mana.ToString();
        partyManaMaxText.text = party.ManaMax.ToString();
        partyManaReserveText.text = party.ManaReserve.ToString();
        partyManaReserveMaxText.text = party.ManaReserveMax.ToString();

        HealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(5 * party.Health, HealthBar.GetComponent<RectTransform>().rect.height);
        ManaBar.GetComponent<RectTransform>().sizeDelta = new Vector2(5 * party.Mana, ManaBar.GetComponent<RectTransform>().rect.height);
        ReserveBar.GetComponent<RectTransform>().sizeDelta = new Vector2(5 * party.ManaReserve, ReserveBar.GetComponent<RectTransform>().rect.height);
    }
    
}
