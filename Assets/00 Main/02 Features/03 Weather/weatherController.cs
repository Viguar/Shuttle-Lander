using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Aircraft;

#if UNITY_EDITOR
using UnityEditor;
using System.Net;
#endif

namespace Viguar.WeatherDynamics
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]  
    public class weatherController : MonoBehaviour
    {
        public int m_AirTemperature = 15;
        public int m_AirPressure = 1013;
        public int m_RelativeHumidity = 50;
        public float m_TemperatureLapse = 1f;
        public float m_HumidityLapse = 0.25f;

        public float baseWindStrength = 5;
        public float baseWindTurbulenceStrength = 4;
        public float baseWindGustStrength = 20;
        public float baseWindHeading = 0;
        public float windGustsPerMinute = 0;
        public float windGustsDurationSeconds = 0;
        public AnimationCurve windGustShape = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

        public int m_maxCloudsPerZone = 5;
        public GameObject[] LowLevelCloudPrefabs;
        public int m_lowLevelCloudsAltitude = 200;
        public int m_midLevelCloudsAltitude = 1000;
        public int m_highLevelCloudsAltitude = 5000;
        public PrecipitationConfig[] pConfig;

        public bool m_dynamicWeather = false;
        public GameObject m_weatherZonePrefab;
        public GameObject m_weatherZoneSpawnPrefab;

        public float m_dynWeatherZoneRandomizer = 0.2f;
        public int m_weatherZoneGenerationDistance = 1000;
        

        public int m_weatherZoneSize = 75;
        public float m_weatherZoneSpeed = 5;

        public GameObject ClosestWeatherZone { get; set; }
        public float shortestDistance { get; set; }
        private int m_maxTilesInRow;
        public bool m_generateCloudFormation = false;
        public bool m_generateFog = false;
        public bool m_generatePrecipitation = false;
        public bool m_generateParticles = false;

        private bool clustercreated = false;
        void Start()
        {
            if (m_dynamicWeather) { configureDynamicWeather(); }
            else { configureStaticWeather(); }
            shortestDistance = Mathf.Infinity;
        }

        void Update()
        {
            if(m_dynamicWeather) { runDynamicWeather(); }                                     
        }

        void configureStaticWeather()
        {
            //m_aeroplane.UpdateWeatherZoneValues(m_AirTemperature, m_AirPressure, m_RelativeHumidity, wwz, m_TemperatureLapse, m_HumidityLapse);
        }
        void configureDynamicWeather()
        {
            shortestDistance = Mathf.Infinity;
            if (m_weatherZoneSize > m_weatherZoneGenerationDistance)
            {
                m_weatherZoneGenerationDistance = m_weatherZoneSize;
            }
            if(GetComponent<WindZone>() != null)
            {
                Destroy(GetComponent<WindZone>());
            }
            Vector3 colliderSize = new Vector3(m_weatherZoneGenerationDistance, m_weatherZoneGenerationDistance, m_weatherZoneGenerationDistance);
            Vector3 spawnPositionSet = new Vector3(0, 0, -m_weatherZoneGenerationDistance);
            m_weatherZoneSpawnPrefab.transform.localPosition = spawnPositionSet;
            GetComponent<BoxCollider>().size = colliderSize;
            GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<Rigidbody>().useGravity = false;
            m_maxTilesInRow = Mathf.RoundToInt(m_weatherZoneGenerationDistance / m_weatherZoneSize);
            fillWeatherAtPosition(m_maxTilesInRow, transform.position);
            //fillWeatherAtPosition(m_maxTilesInRow, m_weatherZoneSpawnPrefab.transform.position);            
        }
        void runDynamicWeather()
        {
            GameObject[] zones;
            zones = GameObject.FindGameObjectsWithTag("weatherZone");
            if (zones.Length <= m_maxTilesInRow * m_maxTilesInRow)
            {
                fillWeatherAtPosition(m_maxTilesInRow, m_weatherZoneSpawnPrefab.transform.localPosition);
            }
        }               
        void fillWeatherAtPosition(int rowAmount, Vector3 spawnPos)
        {
                GameObject cluster;
                cluster = Instantiate(new GameObject("Cluster"), spawnPos, Quaternion.identity);
                cluster.transform.parent = transform;
                for (int y = 0; y <= rowAmount; y++)
                {
                    for (int x = 0; x <= rowAmount; x++)
                    {
                        GameObject weatherZone;
                        Vector3 pos = new Vector3(x, 0, y) * m_weatherZoneSize;
                        pos += Vector3.back * m_weatherZoneGenerationDistance / 2;
                        pos += Vector3.left * m_weatherZoneGenerationDistance / 2;
                        pos += transform.position / 2;
                        weatherZone = Instantiate(m_weatherZonePrefab, pos, Quaternion.identity);
                        weatherZone.transform.parent = cluster.transform;
                    }
                }
                cluster.transform.rotation = transform.rotation;
         }      

    }

    [Serializable]
    public class PrecipitationConfig
    {
        public weatherCalculator.PrecipitationTypes precipitation;
        public GameObject precipitationPrefab;
        public int particlesPerUnit;
    }
}



// Custom Editor
#if UNITY_EDITOR
namespace Viguar.WeatherDynamics
{
    [CustomEditor(typeof(weatherController)), InitializeOnLoadAttribute]
    public class aircraftControllerEditor : Editor
    {
        weatherController wsys;
        SerializedObject SerWSYS;

        private void OnEnable()
        {
            wsys = (weatherController)target;
            SerWSYS = new SerializedObject(wsys);
        }
        public override void OnInspectorGUI()
        {
            SerWSYS.Update();

            #region Script Main Header
            EditorGUILayout.Space();
            GUILayout.Label("Viguar Aircraft Weather", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 });
            //GUILayout.Label("3", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 11 });
            EditorGUILayout.Space();
            #endregion

            #region Global Weather Settings
            #region Section Header
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //Header Divider Bar
            GUILayout.Label("Atmospheric Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true)); //Section Title
            EditorGUILayout.Space();
            #endregion

            GUILayout.Label("Weather Generator", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 }, GUILayout.ExpandWidth(true)); //Section SubTitle         
            wsys.m_AirTemperature = EditorGUILayout.IntSlider(new GUIContent("Sea Level Temperature", "The air temperature in celsius (°C) at sea level of the world"), wsys.m_AirTemperature, -40, 60);
            wsys.m_AirPressure = EditorGUILayout.IntSlider(new GUIContent("Sea Level Air Pressure", "The air pressure in hectopascal (hPa) at sea level of the world."), wsys.m_AirPressure, 870, 1084);
            wsys.m_RelativeHumidity = EditorGUILayout.IntSlider(new GUIContent("Sea Level Humidity", "The relative humidity (%) at sea level of the world."), wsys.m_RelativeHumidity, 1, 100);            
            EditorGUILayout.Space();

            GUILayout.Label("Altitude Lapse Rates", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 }, GUILayout.ExpandWidth(true)); //Section SubTitle         
            wsys.m_TemperatureLapse = EditorGUILayout.Slider(new GUIContent("Altitude Temperature Lapse", "The amount of °C by which the temperature reduces every 100 meters of altitude."), wsys.m_TemperatureLapse, 0.01f, 10f);
            wsys.m_HumidityLapse = EditorGUILayout.Slider(new GUIContent("Altitude Humidity Lapse", "The amount of % by which the humidity reduces every 100 meters of altitude."), wsys.m_HumidityLapse, 0.01f, 10f);
            EditorGUILayout.Space();

            GUILayout.Label("Wind Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 }, GUILayout.ExpandWidth(true)); //Section SubTitle
            wsys.baseWindHeading = EditorGUILayout.Slider(new GUIContent("Base Wind Heading", ""), wsys.baseWindHeading, 0f, 359f);
            wsys.baseWindStrength = EditorGUILayout.Slider(new GUIContent("Base Wind Strength", ""), wsys.baseWindStrength, 0f, 100f);
            wsys.baseWindTurbulenceStrength = EditorGUILayout.Slider(new GUIContent("Max. Turbulence Strength", ""), wsys.baseWindTurbulenceStrength, 0f, 100f);
            wsys.baseWindGustStrength = EditorGUILayout.Slider(new GUIContent("Max. Wind Gust Strength", ""), wsys.baseWindGustStrength, 0f, 100f);
            wsys.windGustsPerMinute = EditorGUILayout.Slider(new GUIContent("Max. Wind Gusts Per Minute", ""), wsys.windGustsPerMinute, 0f, 10f);
            wsys.windGustsDurationSeconds = EditorGUILayout.Slider(new GUIContent("Max. Wind Gust Duration (Seconds)", ""), wsys.windGustsDurationSeconds, 0f, 10f);
            wsys.windGustShape = EditorGUILayout.CurveField(new GUIContent("Wind Gust Intensity over time", "."), wsys.windGustShape);
            #endregion

            #region Dynamic Weather Setup
            #region Section Header
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //Header Divider Bar
            GUILayout.Label("Dynamic Weather Generator Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true)); //Section Title
            EditorGUILayout.Space();
            #endregion
            wsys.m_dynamicWeather = EditorGUILayout.ToggleLeft(new GUIContent("Dynamic Weather", "Toggles dynamic weather."), wsys.m_dynamicWeather);
            EditorGUILayout.Space();
            if (wsys.m_dynamicWeather)
            {
             GUILayout.Label("Dynamic Weather Zone Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 }, GUILayout.ExpandWidth(true)); //Section SubTitle                                        
             wsys.m_weatherZonePrefab = EditorGUILayout.ObjectField(new GUIContent("Weather Zone Prefab", "The weather zone prefab that will be instantiated."), wsys.m_weatherZonePrefab, typeof(GameObject), true) as GameObject;
             wsys.m_weatherZoneSpawnPrefab = EditorGUILayout.ObjectField(new GUIContent("Weather Zone Spawn Point", "The weather zone spawn point."), wsys.m_weatherZoneSpawnPrefab, typeof(GameObject), true) as GameObject;
                EditorGUILayout.Space();
                wsys.m_dynWeatherZoneRandomizer = EditorGUILayout.Slider(new GUIContent("Weather Zone Randomiser Intensity", "The intensity by which the global weather values are randomized in each zone."), wsys.m_dynWeatherZoneRandomizer, 0.001f, 1f);
                EditorGUILayout.Space();
                wsys.m_weatherZoneSize = EditorGUILayout.IntSlider(new GUIContent("Weather Zone Size", "The Size of the Weather Zones"), wsys.m_weatherZoneSize, 1, 10000);                   
             wsys.m_weatherZoneGenerationDistance = EditorGUILayout.IntSlider(new GUIContent("Weather Zone Generation Distance", "The distance from the weather center where dynamic weather zones occur"), wsys.m_weatherZoneGenerationDistance, 0, 100000);
             wsys.m_weatherZoneSpeed = EditorGUILayout.Slider(new GUIContent("Weather Zone Speed", "The speed of the Weather Zones"), wsys.m_weatherZoneSpeed, 1, 200);
             
             EditorGUILayout.Space();
            }
            


            #endregion

            #region Visual Effects
            #region Section Header
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //Header Divider Bar
            GUILayout.Label("Visual Effects", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true)); //Section Title
            EditorGUILayout.Space();
            #endregion

            GUILayout.Label("Visual Effects Configuration", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 }, GUILayout.ExpandWidth(true)); //Section SubTitle         
            wsys.m_generateCloudFormation = EditorGUILayout.ToggleLeft(new GUIContent("Enable Cloud Formation", "Toggles clouds being shown."), wsys.m_generateCloudFormation);
            wsys.m_generateFog = EditorGUILayout.ToggleLeft(new GUIContent("Enable Fog", "Toggles fog being shown."), wsys.m_generateFog);
            wsys.m_generatePrecipitation = EditorGUILayout.ToggleLeft(new GUIContent("Enable Precipitation", "Toggles rain/snow being shown."), wsys.m_generatePrecipitation);
            wsys.m_generateParticles = EditorGUILayout.ToggleLeft(new GUIContent("Enable Particles & Pollution", "Toggles particles & pollution being shown."), wsys.m_generateParticles);
            EditorGUILayout.Space();
           
            if (wsys.m_generateCloudFormation)
            {
                GUILayout.Label("Clouds Configuration", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 }, GUILayout.ExpandWidth(true)); //Section SubTitle
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("LowLevelCloudPrefabs"), true);
                EditorGUI.indentLevel--;
                wsys.m_maxCloudsPerZone = EditorGUILayout.IntSlider(new GUIContent("Max. Clouds per Zone", "Clouds per Zone amount. May impact performance."), wsys.m_maxCloudsPerZone, 1, 15);                
                wsys.m_lowLevelCloudsAltitude = EditorGUILayout.IntSlider(new GUIContent("Low Level Clouds Height", "Altitude of Low Level Clouds Spawning"), wsys.m_lowLevelCloudsAltitude, 0, 800);
                wsys.m_midLevelCloudsAltitude = EditorGUILayout.IntSlider(new GUIContent("Mid Level Clouds Height", "Altitude of Mid Level Clouds Spawning"), wsys.m_midLevelCloudsAltitude, 800, 3200);
                wsys.m_highLevelCloudsAltitude = EditorGUILayout.IntSlider(new GUIContent("High Level Clouds Height", "Altitude of High Level Clouds Spawning"), wsys.m_highLevelCloudsAltitude, 3200, 16000);
                EditorGUILayout.Space();
            }

            if(wsys.m_generatePrecipitation)
            {
                GUILayout.Label("Precipitation Configuration", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 }, GUILayout.ExpandWidth(true)); //Section SubTitle
                EditorGUILayout.PropertyField(SerWSYS.FindProperty("pConfig"), true);
                EditorGUILayout.Space();
            }
            #endregion

            //Sets any changes from the prefab
            if (GUI.changed)
            {
                EditorUtility.SetDirty(wsys);
                Undo.RecordObject(wsys, "FPC Change");
                SerWSYS.ApplyModifiedProperties();
            }
        }

    }
}
#endif
