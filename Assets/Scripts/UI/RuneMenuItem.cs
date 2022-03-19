using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RuneMenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Rune rune;
    public TMP_Text text;
    public Image icon;
    public Button button;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip($"Rune: {rune.description}\nAbility: {rune.ability.description}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       Tooltip.HideTooltip();
    }
}
