using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public float transitionDuration;
    public CameraMode currentMode;

    public enum CameraMode
    {
        Isometric,
        Topdown
    }

    public void GoIsometric()
    {
        StartCoroutine(Transition(new Vector3(), Quaternion.Euler(35, 30, 0)));
    }

    public void GoTopDown()
    {
        StartCoroutine(Transition(new Vector3(), Quaternion.Euler(35, 30, 0)));
    }

    private IEnumerator Transition(Vector3 targetPos, Quaternion targetRot)
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.position = Vector3.Lerp(startingPos, targetPos, t);
            transform.rotation = Quaternion.Lerp(startingRot, targetRot, t);
            yield return 0;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            switch (currentMode)
            {
                case CameraMode.Isometric:
                    GoTopDown();
                    break;
                case CameraMode.Topdown:
                default:
                    GoIsometric();
                    break;
            }
        }
    }
}