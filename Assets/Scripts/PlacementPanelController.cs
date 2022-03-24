using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlacementPanelController : MonoBehaviour
{
    public GameObject placeButtonPrefab;
    public void CreatePlacementButton(String text, UnityAction action)
    {
        GameObject instantiate = Instantiate(placeButtonPrefab, transform);
        Button button = instantiate.GetComponentInChildren<Button>();
        button.onClick.AddListener(action);
        TMP_Text tmpText = instantiate.GetComponentInChildren<TMP_Text>();
        tmpText.text = text;
    }
}