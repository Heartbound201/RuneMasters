using System.Collections;
using TMPro;
using UnityEngine;

public class MessageDisplayingUIController : MonoSingleton<MessageDisplayingUIController>
{
    public TMP_Text message;
    public float duration;

    public void ShowMessage(string msg)
    {
        message.text = msg;
        StartCoroutine(Show());
    }

    public IEnumerator Show()
    {
        message.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        message.gameObject.SetActive(false);
    }
}