using UnityEngine;

public class RunwayLocalizer : MonoBehaviour
{
    public LocalizerBeam[] _Localizer;
    [Space(10)]
    [Header("Localizer Colors (Editor)")]
    [SerializeField] private Color32 _LocalizerColorOnGlideSlope;
    [SerializeField] private Color32 _LocalizerColorLow;
    [SerializeField] private Color32 _LocalizerColorTooLow;
    [SerializeField] private Color32 _LocalizerColorHigh;
    [SerializeField] private Color32 _LocalizerColorTooHigh;

    private void Update()
    {
        RunLocalizer();
    }

    private void RunLocalizer()
    {      
        foreach(LocalizerBeam localizer in _Localizer)
        {
            for(int i=0; i<localizer._LocalizerRayAmount; i++)
            {
                CreateLocalizer(i, localizer._LocalizerRayAmount, localizer._LocalizerWidth, localizer._LocalizerAngle, localizer._LocalizerRange, localizer);
            }
        }       
    }

    private void CreateLocalizer(int iteration, int rayAmount, float raySpread, float rayAngle, int rayRange, LocalizerBeam beam)
    {
        Vector3 localizerOrigin = transform.position; //The Localizer Beams are send from this very gameObject.
        float rayOffsetRotation = ((iteration - (rayAmount - 1) / 2) * raySpread) / (rayAmount - 1); //The spreading angle of the rays based on the amount and maximum spread angle.
        Vector3 rayDirection = Quaternion.Euler(-rayAngle, rayOffsetRotation, 0) * transform.forward; //The rays align with the direction the attached gameObject is facing with a positive angle upwards.
        Color32 rayColor;     
        switch(beam._LocalizerType)
        {
            case LocalizerBeam._LocalizerTypes.OnGlideSlope:
                rayColor = _LocalizerColorOnGlideSlope;
                DrawRayGizmo(localizerOrigin, rayDirection, rayRange, rayColor);
                break;
            case LocalizerBeam._LocalizerTypes.Low:
                rayColor = _LocalizerColorLow;
                DrawRayGizmo(localizerOrigin, rayDirection, rayRange, rayColor);
                break;
            case LocalizerBeam._LocalizerTypes.TooLow:
                rayColor = _LocalizerColorTooLow;
                DrawRayGizmo(localizerOrigin, rayDirection, rayRange, rayColor);
                break;
            case LocalizerBeam._LocalizerTypes.High:
                rayColor = _LocalizerColorHigh;
                DrawRayGizmo(localizerOrigin, rayDirection, rayRange, rayColor);
                break;
            case LocalizerBeam._LocalizerTypes.TooHigh:
                rayColor = _LocalizerColorTooHigh;
                DrawRayGizmo(localizerOrigin, rayDirection, rayRange, rayColor);
                break;
        }        
        //FOR LATER:
        /*
         *RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, detectionRange))
            {
                // Check the hit point and adjust game elements accordingly
                // You may also want to differentiate between rays (center, left, right) for specific adjustments
            }
         */
    }

    private void DrawRayGizmo(Vector3 origin, Vector3 direction, float length, Color32 color)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(origin, direction * length);
    }

    private void OnDrawGizmos()
    {     
        RunLocalizer(); // Visualize the rays in the Unity Editor scene view
    }
}

    [System.Serializable]
    public class LocalizerBeam
    {
    public enum _LocalizerTypes { OnGlideSlope, Low, TooLow, High, TooHigh, }
    public _LocalizerTypes _LocalizerType;
    public int _LocalizerRayAmount;
    public int _LocalizerRange;
    public float _LocalizerAngle;
    public float _LocalizerWidth;
    }
