using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Viguar.Aircraft
{ 

 public class camToolsLookAt : MonoBehaviour
    {
        public Transform cameraTarget;

    void Start()
    {
            //cameraTarget = GetComponentInParent<Transform>();
    }


    void Update()
    {
            transform.LookAt(cameraTarget);
    }
 }
}

