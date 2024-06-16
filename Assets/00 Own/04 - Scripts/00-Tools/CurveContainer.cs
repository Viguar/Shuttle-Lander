using System;
using UnityEngine;
using Unity.XR.CoreUtils.Datums;

namespace Viguar.Tooling
{
    [CreateAssetMenu(fileName = "CurveContainer", menuName = "Viguar/Tools/DataContainers/AnimationCurveContainer", order = 0)]
    public class CurveContainer : Datum<AnimationCurve>
    {
    }
}
