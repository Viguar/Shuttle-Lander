using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.WeatherDynamics
{
    public class rainRenderer : MonoBehaviour
    {
        GameObject m_aeroplane;

        void Start()
        {
            m_aeroplane = GameObject.FindGameObjectWithTag("aircraft");
        }


        void Update()
        {
            if(Vector3.Distance(transform.position, m_aeroplane.transform.position) > 1500)
            {
                GetComponent<ParticleSystem>().Play(false);
            }
            else
            {
                GetComponent<ParticleSystem>().Play(true);
            }
        }
    }
}
