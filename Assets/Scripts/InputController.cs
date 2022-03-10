using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static event Action CommandLeft;
    public static event Action CommandRight;
    public static event Action CommandConfirm;
    public static event Action CommandCancel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CommandLeft?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            CommandRight?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CommandConfirm?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            CommandCancel?.Invoke();
        }
    }
}