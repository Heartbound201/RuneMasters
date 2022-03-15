using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public TMP_Text text;

    public void UpdateHealth(int current, int max)
    {
        text.text = $"{current}/{max}";
    }
    
    void Update()
    {
        text.transform.rotation = Camera.main.transform.rotation;
    }
}
