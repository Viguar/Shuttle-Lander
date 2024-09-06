using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    public class aircraftCameraSwitcher : MonoBehaviour
    {
        public Camera[] cameras;
        private int index = 0;
        private AircraftBaseProcessor _configBaseProcessor;
        private Camera currentCamera;

        void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            foreach (Camera camera in cameras) //Turn off every camera and their audio listener.
            {
                camera.enabled = false;
            }
            cameras[0].enabled = true; //Turn on cam 1 and add audio listerner.
            cameras[0].gameObject.AddComponent(typeof(AudioListener));
            currentCamera = cameras[0];
            _configBaseProcessor._DebugActiveCamera = currentCamera;
        }

        public void switchCameras()
        {
            Destroy(currentCamera.GetComponent<AudioListener>());
            foreach (Camera _camera in cameras)
                {
                    _camera.enabled = false;
                }
                index = index + 1 ;
                if (index >= cameras.Length)
                {
                    index = 0;
                }
            currentCamera = cameras[index];
            currentCamera.enabled = true;
                currentCamera.gameObject.AddComponent(typeof(AudioListener));
            _configBaseProcessor._DebugActiveCamera = currentCamera;
        }
    }
}
