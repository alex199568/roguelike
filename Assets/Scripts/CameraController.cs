using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

	public float scrollSensitivity = 1.0f;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        UpdateCameraScroll();
    }

    private void UpdateCameraScroll()
    {
        var mouseDelta = Input.GetAxis("Mouse ScrollWheel");
        if (mouseDelta > 0.0f)
        {
            _camera.orthographicSize -= scrollSensitivity;
        }
        else if (mouseDelta < 0.0f)
        {
            _camera.orthographicSize += scrollSensitivity;
        }
    }
}
