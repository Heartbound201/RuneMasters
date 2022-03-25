using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Wunderwunsch.HexMapLibrary.Generic;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Camera cam;
    public CameraMode currentMode;
    public float transitionDuration;
    public float zoomInSize = 12;
    public float zoomOutSize = 16;
    public float zoomSensitivity = 10f;
    public Bounds bounds;

    private bool inTransition = false;
    private Vector3 dragOrigin;

    public enum CameraMode
    {
        Isometric,
        Topdown
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void GoIsometric()
    {
        if(inTransition) return;
        StartCoroutine(Transition(cam.transform.position, Quaternion.Euler(35, 0, 0)));
        currentMode = CameraMode.Isometric;
    }

    private void GoTopDown()
    {
        if(inTransition) return;
        StartCoroutine(Transition(cam.transform.position, Quaternion.Euler(90, 0, 0)));
        currentMode = CameraMode.Topdown;
    }

    private IEnumerator Transition(Vector3 targetPos, Quaternion targetRot)
    {
        inTransition = true;
        float t = 0.0f;
        var transform1 = cam.transform;
        Vector3 startingPos = transform1.position;
        Quaternion startingRot = transform1.rotation;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            cam.transform.position = Vector3.Lerp(startingPos, targetPos, t);
            cam.transform.rotation = Quaternion.Lerp(startingRot, targetRot, t);
            yield return 0;
        }

        inTransition = false;
    }

    public static void CameraLookAt(Unit unit)
    {
        var position = unit.transform.position;
        instance.cam.transform.position = new Vector3(position.x, 0 , position.z);
    } public void CameraLookAt(HexTile<Tile> tile)
    {
        var position = tile.Data.transform.position;
        instance.cam.transform.position = new Vector3(position.x, 0 , position.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeCameraMode();
        }

        ChangeZoom();
        HandlePanning();
    }

    private void HandlePanning()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 diff = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position = bounds.ClosestPoint(cam.transform.position + diff);
        }
    }

    private void ChangeZoom()
    {
        float zoom = cam.orthographicSize;
        if (Input.mouseScrollDelta.y > 0)
        {
            zoom -= zoomSensitivity * Time.deltaTime;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoom += zoomSensitivity * Time.deltaTime;
        }

        zoom = Mathf.Clamp(zoom, zoomInSize, zoomOutSize);
        cam.orthographicSize = zoom;
    }

    private void ChangeCameraMode()
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
}