using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public float transitionDuration;
    public CameraMode currentMode;
    public float zoomInSize = 12;
    public float zoomOutSize = 16;

    public enum CameraMode
    {
        Isometric,
        Topdown
    }

    public void GoIsometric()
    {
        StartCoroutine(Transition(new Vector3(), Quaternion.Euler(35, 30, 0)));
        currentMode = CameraMode.Isometric;
    }

    public void GoTopDown()
    {
        StartCoroutine(Transition(new Vector3(), Quaternion.Euler(90, 30, 0)));
        currentMode = CameraMode.Topdown;
    }

    private IEnumerator Transition(Vector3 targetPos, Quaternion targetRot)
    {
        float t = 0.0f;
        Vector3 startingPos = camera.transform.position;
        Quaternion startingRot = camera.transform.rotation;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            camera.transform.position = Vector3.Lerp(startingPos, targetPos, t);
            camera.transform.rotation = Quaternion.Lerp(startingRot, targetRot, t);
            yield return 0;
        }
    }
    private IEnumerator Transition(float targetZoom)
    {
        float t = 0.0f;
        float startingZoom = camera.orthographicSize;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);
            camera.orthographicSize = Mathf.Lerp(startingZoom, targetZoom, t);
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
                    GoIsometric();
                    break;
            }
        }

        // zoom in
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(Transition(zoomInSize));
        }
        // zoom out
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(Transition(zoomOutSize));
        }
    }
}