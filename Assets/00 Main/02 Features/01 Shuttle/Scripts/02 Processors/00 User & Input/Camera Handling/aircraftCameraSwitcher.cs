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
        public int StartingCam = 0;

        void Start()
        {
            if(StartingCam < 0 || StartingCam > cameras.Length) { StartingCam = 0; }
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            foreach (Camera camera in cameras) //Turn off every camera and their audio listener.
            {
                camera.enabled = false;
            }
            cameras[StartingCam].enabled = true; //Turn on cam 1 and add audio listerner.
            cameras[StartingCam].gameObject.AddComponent(typeof(AudioListener));
            currentCamera = cameras[StartingCam];
            _configBaseProcessor._DebugActiveCamera = currentCamera;
            index = StartingCam;
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
            currentCamera.gameObject.AddComponent(typeof(AudioListener));
            currentCamera.enabled = true;               
            _configBaseProcessor._DebugActiveCamera = currentCamera;
        }
    }
}
