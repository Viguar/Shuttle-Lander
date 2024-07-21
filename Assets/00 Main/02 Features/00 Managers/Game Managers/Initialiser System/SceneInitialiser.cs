using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using Viguar.Aircraft.Runways;
using Viguar.WeatherDynamics;

namespace Viguar.Aircraft.Management
{
    public class SceneInitialiser : MonoBehaviour
    {
        #region Feature GameObjects
        private AircraftBaseProcessor _AircraftPlayer;              
        private weatherController _WeatherController;
        //private Runwayzone
        private RunwayLocalizer _RunwayLocalizer;
        #endregion

        #region Feature Bools
        public bool _InitFoundAircraftPlayer;
        public bool _InitFoundWeatherController;
        public bool _InitFoundRunwayZone;
        public bool _InitFoundRunwayLocalizer;
        #endregion

        void Awake()
        {
            FetchFeatureObjectsInScene();
        }
        void Start()
        {

        }
        void Update()
        {

        }


        private void FetchFeatureObjectsInScene()
        {
            FetchFeatureAircraftplayer();
            FetchFeatureWeatherSystem();
            FetchFeatureRunwayZone();
            FetchFeatureRunwayLocalizer();
        }

        private void FetchFeatureAircraftplayer()
        {
            if(GameObject.FindGameObjectWithTag("aircraft") != null)
            {
                _InitFoundAircraftPlayer = true;
                _AircraftPlayer = GameObject.FindGameObjectWithTag("aircraft").GetComponent<AircraftBaseProcessor>();
            }
            else
            {
                _InitFoundAircraftPlayer = false;
            }
        }      
        private void FetchFeatureRunwayZone()
        {

        }
        private void FetchFeatureRunwayLocalizer()
        {
            if(GameObject.FindGameObjectWithTag("runwayLocalizer") != null)
            {
                _InitFoundRunwayLocalizer = true;
                _RunwayLocalizer = GameObject.FindGameObjectWithTag("runwayLocalizer").GetComponent<RunwayLocalizer>();
            }
            else
            {
                _InitFoundRunwayLocalizer = false;
            }
        }
        private void FetchFeatureWeatherSystem()
        {
            if(GameObject.FindGameObjectWithTag("weatherController") != null)
            {
                _InitFoundWeatherController = true;
                _WeatherController = GameObject.FindGameObjectWithTag("weatherController").GetComponent<weatherController>();
            }
            else
            {
                _InitFoundWeatherController = false;
            }
        }

        
    }
}
