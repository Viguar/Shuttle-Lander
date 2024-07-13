using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Viguar.EditorTooling.InspectorUITools.OverrideLabels;

namespace Viguar.Aircraft.Runways
{
    public class RunwayLocalizer : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;

        [Header("Localizer Beam Creator")]
        [LabelOverride("Localizer Beam Length")] public int _LocalizerRange = 1500;
        [LabelOverride("Glide Slope")] public float _LocalizerGlobalAngle = 15;
        [LabelOverride("Inner Glide Slope Margin Angle")] public float _LocalizerVerticalWindowCenter = 3;
        [LabelOverride("Outer Glide Slope Margin Angle")] public float _LocalizerVerticalWindowOffset = 12;
        [LabelOverride("Glide Slope Width Angle")]public float _LocalizerWidth = 25;
        [Space(10)]
        public Material _CenteredLocalizerDebugMaterial;
        public Material _OffsetLocalizerDebugMaterial;
        
        [HideInInspector] public bool _Centered = false;
        [HideInInspector] public bool _Offset = false;
        private float _CurrentApproachAngle;
        private bool _AboveGlideslope = false;

        private void OnValidate()
        {
            GetComponent<LocalizerShapeDrawer>().OnLocalizerValidate();
        }

        private void Start()
        {
            _configBaseProcessor = GameObject.FindGameObjectWithTag("aircraft").GetComponent<AircraftBaseProcessor>();
        }

        private void Update()
        {
            RunLocalizerLogic();
        }

        private void RunLocalizerLogic()
        {
            if(_configBaseProcessor._CockpitLocalizerPanelState == AircraftBaseProcessor.CockpitLocalizerPanelStates.On)
            {
                if (_Offset && _Centered) //Run logic for centered localizer range.
                {
                    _configBaseProcessor._LocalizerRecieverInfoState = AircraftBaseProcessor.LocalizerRecieverInfoTypes.OnGlideSlope;
                }
                else if (_Offset && !_Centered) //Run logic offset localizer range.
                {
                    CalculateCurrentApproachAngle();
                    EvaluateCurrentApproachAngle();
                    if (_AboveGlideslope) { _configBaseProcessor._LocalizerRecieverInfoState = AircraftBaseProcessor.LocalizerRecieverInfoTypes.High; }
                    else { _configBaseProcessor._LocalizerRecieverInfoState = AircraftBaseProcessor.LocalizerRecieverInfoTypes.Low; }
                }
                else //Run logic for not within localizer range.
                {
                    _configBaseProcessor._LocalizerRecieverInfoState = AircraftBaseProcessor.LocalizerRecieverInfoTypes.OutOfRange;
                }
            }
            else
            {
                _configBaseProcessor._LocalizerRecieverInfoState = AircraftBaseProcessor.LocalizerRecieverInfoTypes.Unknown;
            }
        }

        private void CalculateCurrentApproachAngle()
        {
            Vector3 pos1 = transform.position;
            Vector3 pos2 = _configBaseProcessor.gameObject.transform.position;
            Vector3 horizontalVector = new Vector3(pos2.x - pos1.x, 0, pos2.z - pos1.z);

            float heightDifference = pos2.y - pos1.y;
            float horizontalDifference = horizontalVector.magnitude;
            _CurrentApproachAngle = Mathf.Atan2(heightDifference, horizontalDifference) * Mathf.Rad2Deg;
        }

        private void EvaluateCurrentApproachAngle()
        {
            if(_CurrentApproachAngle > _LocalizerGlobalAngle) { _AboveGlideslope = true; }
            else { _AboveGlideslope = false; }
        }

    }
}