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
    public TMP_Text text;
    public Image icon;
    public Image categoryIcon;
    public Image costIcon;
    public Button button;
    public Unit unit;
    public Rune rune;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip(rune.Summary(unit));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       Tooltip.HideTooltip();
    }
}
