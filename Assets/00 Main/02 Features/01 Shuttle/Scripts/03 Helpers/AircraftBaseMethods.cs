using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Aircraft;

public class AircraftBaseMethods : MonoBehaviour
{
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _originalVelocity;
    private Vector3 _originalAngularVelocity;
    private Vector3 _velocityAtFreeze;
    private Vector3 _angularVelocityAtFreeze;

    public void RecordOriginalValues()
    {
        _originalPosition = gameObject.transform.position;
        _originalRotation = gameObject.transform.rotation;
        _originalVelocity = GetComponent<Rigidbody>().velocity;
        _originalAngularVelocity = GetComponent<Rigidbody>().angularVelocity;
    }

    #region Resetting Aircraft
    //Rigidbody.position and .rotation need to be changed to the original position, because transform.position/.rotation does not work (because kinematic stuff and velocities).
    public void ResetAircraftPosition()
    {
        GetComponent<Rigidbody>().position = _originalPosition;
    }
    public void ResetAircraftRotation()
    {
        GetComponent<Rigidbody>().rotation = _originalRotation;
    }
    public void ResetAircraftVelocity()
    {
        GetComponent<Rigidbody>().velocity = _originalVelocity;
        GetComponent<Rigidbody>().angularVelocity = _originalAngularVelocity;
    }   
    public void FullResetAircraft()
    {
        ResetAircraftPosition();
        ResetAircraftRotation();
        ResetAircraftVelocity();
    }
    #endregion
    #region Freezing Aircraft
    public void FreezeAircraft()
    {
        _velocityAtFreeze = GetComponent<Rigidbody>().velocity;
        _angularVelocityAtFreeze = GetComponent<Rigidbody>().angularVelocity;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    public void UnfreezeAircraft()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().velocity = _velocityAtFreeze;
        GetComponent<Rigidbody>().angularVelocity = _angularVelocityAtFreeze;
    }
    #endregion
    #region Changing Aircraft Transform & Velocity
    public void KillAircraftVelocity()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    public void IncreaseAltitude()
    {
        GetComponent<Rigidbody>().position += Vector3.up * 10;
    }
    public void IncreaseForwardSpeed()
    {
        GetComponent<Rigidbody>().AddForce(0,0,1000000);
    }
    public void DecreaseForwardSpeed()
    {

    }    
    #endregion    

    public void QuitApplication()
    {
        Application.Quit();
    }
}
