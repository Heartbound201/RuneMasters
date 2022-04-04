using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Forewarning : MonoBehaviour
{
	public TextMeshProUGUI TotalDamageValue;
	public TextMeshProUGUI InfoText;
	public TextMeshProUGUI DamageReceivedValue;

	public GameObject DamageReceivedPanel;

	public void ShowTotalDamage(int damage)
	{
		TotalDamageValue.text = damage.ToString();
	}

	public void ShowInfoText(string Infos)
	{
		InfoText.text = Infos;
	}

	public void ShowDamageReceived(int damage)
	{
		DamageReceivedValue.text = damage.ToString();

		DamageReceivedPanel.SetActive(true);
	}
}
