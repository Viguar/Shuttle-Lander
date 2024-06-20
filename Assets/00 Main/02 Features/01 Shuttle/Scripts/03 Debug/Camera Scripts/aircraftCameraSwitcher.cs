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

        void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            foreach (Camera camera in cameras) //Turn off every camera and their audio listener.
            {
                camera.enabled = false;
                camera.GetComponent<AudioListener>().enabled = false;
            }
            cameras[0].enabled = true; //Turn on cam 1 and its audio listerner.
            cameras[0].GetComponent<AudioListener>().enabled = true;
            _configBaseProcessor._DebugActiveCamera = cameras[0];
        }

        public void switchCameras()
        {
                foreach (Camera _camera in cameras)
                {
                    _camera.enabled = false;
                    _camera.GetComponent<AudioListener>().enabled = false;
                }
                index = index + 1 ;
                if (index >= cameras.Length)
                {
                    index = 0;
                }
                cameras[index].enabled = true;
                cameras[index].GetComponent<AudioListener>().enabled = true;
                _configBaseProcessor._DebugActiveCamera = cameras[index];
        }
    }
}
