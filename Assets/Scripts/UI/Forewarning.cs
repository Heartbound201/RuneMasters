using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Forewarning : MonoBehaviour
{
	public TextMeshProUGUI TotalDamageValue;
	public TextMeshProUGUI InfoText;
	public TextMeshProUGUI DamageReceivedValue;

	public void ShowTotalDamage(int damage)
	{
		TotalDamageValue.text = damage.ToString();
	}

	public void ShowInfoText(string Infos)
	{
		//InfoText.text = 
	}

	public void ShowDamageReceived(int damage)
	{
		DamageReceivedValue.text = damage.ToString();
	}
}
