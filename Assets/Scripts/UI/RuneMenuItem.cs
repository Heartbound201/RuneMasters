using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RuneMenuItem : MonoBehaviour
{
    [FormerlySerializedAs("runePrototype")] public Rune rune;
    public TMP_Text text;
    public Image icon;
    public Button button;
}
