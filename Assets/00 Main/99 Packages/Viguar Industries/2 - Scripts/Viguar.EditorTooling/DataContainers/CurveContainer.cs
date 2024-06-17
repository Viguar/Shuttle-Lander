using UnityEngine;
namespace Viguar.EditorTooling.DataContainers.Curve
{
    [CreateAssetMenu(fileName = "Curve Container", menuName = "Viguar/DataContainers/CurveContainer", order = 0)]
    public class CurveContainer : ScriptableObject
    {
        public AnimationCurve Curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [Space(10)]
        [SerializeField] private string CurveName;
        [TextArea(15, 20)][SerializeField] private string CurveDetails;
    }
}