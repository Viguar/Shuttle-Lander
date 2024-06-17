using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamLimited : MonoBehaviour
{
    private float RotationSensitivity = 60.0f;
    private float ZoomSensitivity = 100.0f;
    private float maxAngle = 60.0f;    
    private float xRotate = 0.0f;
    private float yRotate = 0.0f;
    private float zRotate = 0.0f;
    private float fov = 0.0f;
    private Vector2 fovLimits = new Vector2(15, 100);
    private Camera attachedCamera;

    private void Start()
    {
        attachedCamera = GetComponent<Camera>();
        if(attachedCamera.fieldOfView < fovLimits.y && attachedCamera.fieldOfView > fovLimits.x)
        {
            fov = attachedCamera.fieldOfView;
        }
        else
        {
            fov = (fovLimits.x + fovLimits.y) / 2;
        }
    }

    private void Update()
    {
        if (attachedCamera.isActiveAndEnabled && !Cursor.visible)
        {
            RotateCamera();
            ZoomCamera();
        }
    }

    private void RotateCamera()
    {
        xRotate += Input.GetAxis("Mouse Y") * RotationSensitivity * Time.deltaTime; //X and Y are switched, because if you move the Mouse on Y axis, the camera should move on the X axis (and vice versa).
        yRotate += Input.GetAxis("Mouse X") * RotationSensitivity * Time.deltaTime;
        xRotate = Mathf.Clamp(xRotate, -maxAngle, maxAngle);
        yRotate = Mathf.Clamp(yRotate, -maxAngle, maxAngle);
        transform.localEulerAngles = new Vector3(-xRotate, yRotate, zRotate);
    }
    private void ZoomCamera()
    {
        fov -= Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity;
        fov = Mathf.Clamp(fov, fovLimits.x, fovLimits.y);
        attachedCamera.fieldOfView = Mathf.MoveTowards(attachedCamera.fieldOfView, fov, ZoomSensitivity * Time.deltaTime);
    }
}

