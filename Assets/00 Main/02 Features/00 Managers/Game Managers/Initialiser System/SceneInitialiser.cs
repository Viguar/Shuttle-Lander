using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public bool _FoundAircraftPlayer;
        public bool _FoundWeatherController;
        public bool _FoundRunwayZone;
        public bool _FoundRunwayLocalizer;
        #endregion

        void Awake()
        {

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
                _FoundAircraftPlayer = true;
                _AircraftPlayer = GameObject.FindGameObjectWithTag("aircraft").GetComponent<AircraftBaseProcessor>();
            }
            else
            {
                _FoundAircraftPlayer = false;
            }
        }
        

        private void FetchFeatureRunwayZone()
        {

        }
        private void FetchFeatureRunwayLocalizer()
        {
            if(GameObject.FindGameObjectWithTag("runwayLocalizer") != null)
            {
                _FoundRunwayLocalizer = true;
                _RunwayLocalizer = GameObject.FindGameObjectWithTag("runwayLocalizer").GetComponent<RunwayLocalizer>();
            }
            else
            {
                _FoundRunwayLocalizer = false;
            }
        }
        private void FetchFeatureWeatherSystem()
        {
            if(GameObject.FindGameObjectWithTag("weatherController") != null)
            {
                _FoundWeatherController = true;
                _WeatherController = GameObject.FindGameObjectWithTag("weatherController").GetComponent<weatherController>();
            }
            else
            {
                _FoundWeatherController = false;
            }
        }

        
    }
}
