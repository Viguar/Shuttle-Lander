using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Viguar.Aircraft
{
    public class aircraftCameraRotator : MonoBehaviour
    {
        Transform target;
        float speed = 1f;
        float zoomSpeed = 1f;

        float RotY = 0.0f;
        float RotZ = 0.0f;
        Vector3 tmp = Vector3.forward;

        float distance = 40;
        float rotationDamping = 1;
        float heightDamping = 1;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("aircraft").GetComponent<Transform>();
        }

        public void Update()
        {
            //float CurrentDist += Input.GetAxis("Mouse ScrollWheel");
        }

        public void LateUpdate()
        {
            // Vector3 tmp;
            // tmp = Vector3.forward;
            // tmp.y = Mathf.Sin(transform.position * (Mathf.PI / 180)) * CurrentDist + target.position;
            // transform.position = Vector3.Slerp(transform.position, tmp, speed * Time.deltaTime);
            // transform.LookAt(target);

            //followSmoothly();
            zoomWithScrollWheel();
            followSmoothly();
        }

        private void zoomWithScrollWheel()
        {
            float scrollwheel = Input.GetAxis("Mouse ScrollWheel");
            tmp += Vector3.forward * scrollwheel * 10;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, tmp, zoomSpeed);

            //transform.Translate(0, 0, -scrollwheel * zoomSpeed, Space.Self);
        }

        private void rotateWithMouse()
        {
            if (Input.GetMouseButton(0))
            {                
                RotY += Input.GetAxis("Mouse X") * speed * Time.deltaTime;
                RotZ += Input.GetAxis("Mouse Y") * speed * Time.deltaTime;

                //transform.localEulerAngles = new Vector3(RotZ, RotY, 0);
            }
        }

        private void followSmoothly()
        {
            if (!target) return;

            // Calculate the current rotation angles
            float wantedRotationAngle = target.eulerAngles.y;
            float wantedHeight = target.position.y; // + height;

            float currentRotationAngle = transform.eulerAngles.y;
            float currentHeight = transform.position.y;

            // Damp the rotation around the y-axis
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            // Convert the angle into a rotation
            var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;

            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

            // Always look at the target
            transform.LookAt(target);

        }

    }
}