using System;
using System.Collections;
using UnityEngine;
using Wunderwunsch.HexMapLibrary.Generic;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Camera camera;
    public CameraMode currentMode;
    public float transitionDuration;
    public float zoomInSize = 12;
    public float zoomOutSize = 16;
    public float zoomSensitivity = 10f;
    
    private bool inTransition = false;

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
        StartCoroutine(Transition(camera.transform.position, Quaternion.Euler(35, 0, 0)));
        currentMode = CameraMode.Isometric;
    }

    private void GoTopDown()
    {
        if(inTransition) return;
        StartCoroutine(Transition(camera.transform.position, Quaternion.Euler(90, 0, 0)));
        currentMode = CameraMode.Topdown;
    }

    private IEnumerator Transition(Vector3 targetPos, Quaternion targetRot)
    {
        inTransition = true;
        float t = 0.0f;
        var transform1 = camera.transform;
        Vector3 startingPos = transform1.position;
        Quaternion startingRot = transform1.rotation;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            camera.transform.position = Vector3.Lerp(startingPos, targetPos, t);
            camera.transform.rotation = Quaternion.Lerp(startingRot, targetRot, t);
            yield return 0;
        }

        inTransition = false;
    }

    public static void CameraLookAt(Unit unit)
    {
        var position = unit.transform.position;
        instance.camera.transform.position = new Vector3(position.x, 0 , position.z);
    } public void CameraLookAt(HexTile<Tile> tile)
    {
        var position = tile.Data.transform.position;
        instance.camera.transform.position = new Vector3(position.x, 0 , position.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeCameraMode();
        }

        ChangeZoom();
    }

    private void ChangeZoom()
    {
        float zoom = camera.orthographicSize;
        if (Input.mouseScrollDelta.y > 0)
        {
            zoom -= zoomSensitivity * Time.deltaTime;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoom += zoomSensitivity * Time.deltaTime;
        }

        zoom = Mathf.Clamp(zoom, zoomInSize, zoomOutSize);
        camera.orthographicSize = zoom;
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