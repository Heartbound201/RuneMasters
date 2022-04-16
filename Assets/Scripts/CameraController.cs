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
	public float IsometricAngle = 40;
    public float transitionDuration;
    public float zoomInSize = 12;
    public float zoomOutSize = 16;
    public float zoomSensitivity = 10f;
    public Bounds bounds;

    private bool inTransition = false;
    private bool targetReached = true;
    private Vector3 targetFollow;
    private Vector3 followOrigin;
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
        StartCoroutine(Transition(cam.transform.position, Quaternion.Euler(IsometricAngle, 30, 0)));
        currentMode = CameraMode.Isometric;
    }

    private void GoTopDown()
    {
        if(inTransition) return;
        StartCoroutine(Transition(cam.transform.position, Quaternion.Euler(90, 30, 0)));
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

    public void CameraLookAt(Unit unit)
    {
        targetFollow = unit.transform.position;
        targetReached = false;
        followOrigin = cam.transform.position;
    }

    public void CameraLookAt(HexTile<Tile> tile)
    {
        targetFollow = tile.Data.transform.position;
        targetReached = false;
        followOrigin = cam.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeCameraMode();
        }

        ChangeZoom();
        HandlePanning();
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (targetReached) return;
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetFollow, Time.deltaTime * 2);
        if (Vector3.Distance(followOrigin, targetFollow) < .2f)
        {
            cam.transform.position = targetFollow;
            targetReached = true;
        }
    }

    private void HandlePanning()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            targetReached = true;
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