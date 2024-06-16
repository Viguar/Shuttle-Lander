using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

namespace Viguar.Aircraft
{
    public class debugInputField : MonoBehaviour
    {
        private int minLimit = 0;
        private int maxLimit = 1000;
        private int target = 0;
        private int targetStep = 0;
        private aircraftController m_Aeroplane;
        public enum type { ATTarget, APHeading, APAltitude, APVerticalSpeed, }
        public type targetType;

        void Start()
        {
            m_Aeroplane = GameObject.FindGameObjectWithTag("aircraft").GetComponent<aircraftController>();
        }        

        public void lowerTargetType()
        {
            switch(targetType)
            {
                case type.ATTarget:
                    targetStep = 10;
                    lowerTargetAT();
                    break;
                case type.APHeading:
                    targetStep = 10;
                    lowerTargetAPHeading();
                    break;
                case type.APAltitude:
                    targetStep = 100;
                    lowerTargetAPAltitude();
                    break;
                case type.APVerticalSpeed:
                    targetStep = 1;
                    lowerTargetAPVerticalSpeed();
                    break;
            }
        }
        public void raiseTargetType()
        {
            switch (targetType)
            {
                case type.ATTarget:
                    targetStep = 10;
                    raiseTargetAT();
                    break;
                case type.APHeading:
                    targetStep = 10;
                    raiseTargetAPHeading();
                    break;
                case type.APAltitude:
                    targetStep = 100;
                    raiseTargetAPAltitude();
                    break;
                case type.APVerticalSpeed:
                    targetStep = 1;
                    raiseTargetAPVerticalSpeed();
                    break;
            }
        }

        void raiseTargetAT()
        {
            target += targetStep;
            if (target > m_Aeroplane.m_maxServiceCeiling)
            {
                target = m_Aeroplane.m_maxServiceCeiling;
            }
            m_Aeroplane.SetAutoThrottleTarget(target);
        }
        void lowerTargetAT()
        {
            target -= targetStep;
            if (target < 1)
            {
                target = 0;
            }
            m_Aeroplane.SetAutoThrottleTarget(target);
        }

        void raiseTargetAPHeading()
        {
            target += targetStep;
            if (target > 359)
            {
                target = 0;
            }
            m_Aeroplane.SetAutoPilotHeadingTarget(target);
        }
        void lowerTargetAPHeading()
        {
            target -= targetStep;
            if (target < 0)
            {
                target = 350;
            }
            m_Aeroplane.SetAutoPilotHeadingTarget(target);
        }

        void raiseTargetAPAltitude()
        {
            target += targetStep;
            if(target > m_Aeroplane.m_maxServiceCeiling)
            {
                target = m_Aeroplane.m_maxServiceCeiling;
            }
            m_Aeroplane.SetAutoPilotAltitudeTarget(target);
        }
        void lowerTargetAPAltitude()
        {
            target -= targetStep;
            if (target < 100)
            {
                target = 100;
            }
            m_Aeroplane.SetAutoPilotAltitudeTarget(target);
        }

        void raiseTargetAPVerticalSpeed()
        {
            target += targetStep;
            if (target > m_Aeroplane.ApMaxVerticalRate)
            {
                target = 10;
            }
            m_Aeroplane.SetAutoPilotVerticalSpeedTarget(target);
        }
        void lowerTargetAPVerticalSpeed()
        {
            target -= targetStep;
            if (target < 0)
            {
                target = 1;
            }
            m_Aeroplane.SetAutoPilotVerticalSpeedTarget(target);
        }
    }
}
