using UnityEngine;
using Viguar.EditorTooling.GUITools.ConditionalPropertyDisplay;
using Viguar.Aircraft;

public class AircraftWheelPositioner : MonoBehaviour
{
    private WheelCollider thisObject;
    private Vector3 originalPos;
    private AircraftBaseProcessor aircraft;
    private float maxWheelHeightDistance;
    private Quaternion originalRotatingClusterRotation;
    private float clusterRotation;

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform wheelModel;

    [SerializeField] private bool isRearOfCluster;
    [SerializeField][DrawIf("isRearOfCluster", true)] private Transform rotatingClusterObject;
    [SerializeField][DrawIf("isRearOfCluster", true)] private Transform rotatingClusterFrontWheel;
    [SerializeField][DrawIf("isRearOfCluster", true)] private int rotatingClusterStartRotation;

    private void Start()
    {
        aircraft = GetComponentInParent<AircraftBaseProcessor>();
        thisObject = GetComponent<WheelCollider>();
        if(wheelModel != null) { originalPos = wheelModel.position; }    
        CalculateWheelHeightDistance();
    }

    private void Update()
    {
        MoveWheelCollider();
        MoveWheelModel();
        RotateWheelCluster();
    }

    private void MoveWheelCollider()
    {
        //Make the collider follow the axis. This is because the animated landing gear moves the collider, which should not be a child of the wheelmodel.
        thisObject.transform.position = targetTransform.position;
    }
    private void MoveWheelModel()
    {
        //Make the wheel model follow the collider, so that it looks like it's spinning etc.
        if (wheelModel != null) 
        {
            if (thisObject.GetGroundHit(out WheelHit hit))
            {
                thisObject.GetWorldPose(out Vector3 pos, out Quaternion quat);
                wheelModel.position = pos;
                wheelModel.rotation = quat;
            }
        }
    }
    private void RotateWheelCluster()
    {
        if (isRearOfCluster)
        {
            if (aircraft._LandingGearState == AircraftBaseProcessor.LandingGearStateTypes.Extended)
            {              
                if (thisObject.GetGroundHit(out WheelHit hit))
                {
                    clusterRotation = Mathf.InverseLerp(0, Mathf.Abs(maxWheelHeightDistance), Mathf.Abs(thisObject.transform.position.y - rotatingClusterFrontWheel.transform.position.y));
                }
                else
                {
                    clusterRotation = rotatingClusterStartRotation * Mathf.InverseLerp(0, Mathf.Abs(maxWheelHeightDistance), Mathf.Abs(thisObject.transform.position.y - rotatingClusterFrontWheel.transform.position.y));
                }
                rotatingClusterObject.localRotation = Quaternion.Slerp(rotatingClusterObject.localRotation, Quaternion.Euler(0, -90, clusterRotation), Mathf.Abs(aircraft._VerticalSpeed) * Time.deltaTime * 1.5f);
            }
            else
            {
                rotatingClusterObject.localRotation = originalRotatingClusterRotation;
                clusterRotation = 0;
            }
        }
    }
    private void CalculateWheelHeightDistance()
    {
        if (isRearOfCluster)
        {
            originalRotatingClusterRotation = rotatingClusterObject.localRotation;
            clusterRotation = rotatingClusterStartRotation;
            rotatingClusterObject.localRotation = Quaternion.Euler(0, -90, clusterRotation);
            maxWheelHeightDistance = Mathf.Abs(thisObject.transform.position.y - rotatingClusterFrontWheel.transform.position.y);
        }
    }
}
