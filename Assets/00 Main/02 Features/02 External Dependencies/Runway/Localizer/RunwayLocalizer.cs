using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace Viguar.Aircraft.Runways
{
    [ExecuteInEditMode]
    public class RunwayLocalizer : MonoBehaviour
    {                      
        private AircraftBaseProcessor _configBaseProcessor;

        [Header("Localizer Beam Creator")]
        public int _LocalizerRange = 1000;
        public float _LocalizerGlobalAngle = 15;
        public float _LocalizerVerticalWindowCenter = 7;
        public float _LocalizerVerticalWindowOffset = 12;
        public float _LocalizerWidth = 20;
        
        [HideInInspector] public Vector3[] _CenteredLocalizerVertices;
        [HideInInspector] public Vector3[] _OffsetLocalizerVertices;
        [HideInInspector] public bool _Centered = false;
        [HideInInspector] public bool _Offset = false;
        private MeshCollider _mc;
        private Rigidbody _rb;
        private GameObject _CenteredLocalizerObject;
        private GameObject _OffsetLocalizerObject;
        


        private void Start()
        {
            _configBaseProcessor = GameObject.FindGameObjectWithTag("aircraft").GetComponent<AircraftBaseProcessor>();
            InitLocalizer();
        }

        private void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "aircraft") { print("Captured Localizer"); }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.tag == "aircraft") { print("Lost Localizer"); }
        }

        private void InitLocalizer()
        {
            _mc = GetComponent<MeshCollider>();
            _rb = GetComponent<Rigidbody>();
        }






        #region Localizer Mesh Shape Creation
        private void OnValidate()
        {
            if(transform.Find("LocalizerCenter") == null)
            {
                _CenteredLocalizerObject = new GameObject("LocalizerCenter");
                _CenteredLocalizerObject.transform.SetParent(transform);
                _CenteredLocalizerObject.AddComponent<LocalizerZoneDetector>();
                _CenteredLocalizerObject.GetComponent<LocalizerZoneDetector>().LocalizerType = LocalizerZoneDetector.LocalizerTypes.Center;
            }
            if(transform.Find("LocalizerMargin") == null)
            {
                _OffsetLocalizerObject = new GameObject("LocalizerMargin");
                _OffsetLocalizerObject.transform.SetParent(transform);
                _OffsetLocalizerObject.AddComponent<LocalizerZoneDetector>();
                _OffsetLocalizerObject.GetComponent<LocalizerZoneDetector>().LocalizerType = LocalizerZoneDetector.LocalizerTypes.Offset;
            }
            if (_CenteredLocalizerVertices == null || _CenteredLocalizerVertices.Length != 5)
            {
                _CenteredLocalizerVertices = new Vector3[5];
                _CenteredLocalizerVertices[4] = new Vector3(0, 0, 0); // Initialize the apex at the game object's origin.
            }
            if(_OffsetLocalizerVertices == null || _OffsetLocalizerVertices.Length != 5)
            {
                _OffsetLocalizerVertices = new Vector3[5];
                _OffsetLocalizerVertices[4] = new Vector3(0,0,0);
            }
            UpdateLocalizerCone(_CenteredLocalizerObject, _LocalizerVerticalWindowCenter);
            UpdateLocalizerCone(_OffsetLocalizerObject, _LocalizerVerticalWindowOffset);
        }

        public void UpdateLocalizerCone(GameObject localizerOBJ, float vAngle)
        {
            MeshCollider meshCollider = localizerOBJ.GetComponent<MeshCollider>();
            MeshFilter meshFilter = localizerOBJ.GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = localizerOBJ.GetComponent<MeshRenderer>();
            Rigidbody rigidbody = localizerOBJ.GetComponent<Rigidbody>();
            if (meshCollider == null) { meshCollider = localizerOBJ.AddComponent<MeshCollider>(); } meshCollider.convex = true; meshCollider.isTrigger = true; 
            if (meshFilter == null) { meshFilter = localizerOBJ.AddComponent<MeshFilter>(); }
            if (meshRenderer == null) { meshRenderer = localizerOBJ.AddComponent<MeshRenderer>(); } meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            if (rigidbody == null) { rigidbody = localizerOBJ.AddComponent<Rigidbody>(); } rigidbody.useGravity = false; rigidbody.isKinematic = true;

            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;
            float halfVerticalSize = _LocalizerRange * Mathf.Tan(vAngle * Mathf.Deg2Rad / 2);
            float halfHorizontalSize = _LocalizerRange * Mathf.Tan(_LocalizerWidth * Mathf.Deg2Rad / 2);
            

            // Define the vertices of a pyramid in local space, inverted along the Z-axis
            Vector3 baseVertex = new Vector3(0, 0, 0);  // Base vertex at the origin
            Vector3 bottomLeft = new Vector3(-halfHorizontalSize, -halfVerticalSize, _LocalizerRange); // Bottom-left
            Vector3 topLeft = new Vector3(-halfHorizontalSize, halfVerticalSize, _LocalizerRange);    // Top-left
            Vector3 topRight = new Vector3(halfHorizontalSize, halfVerticalSize, _LocalizerRange);    // Top-right
            Vector3 bottomRight = new Vector3(halfHorizontalSize, -halfVerticalSize, _LocalizerRange); // Bottom-right


            Quaternion rotation = Quaternion.Euler(-_LocalizerGlobalAngle, 0, 0); // Rotate around the x-axis from the base vertex

            bottomLeft = rotation * (bottomLeft - baseVertex) + baseVertex;
            topLeft = rotation * (topLeft - baseVertex) + baseVertex;
            topRight = rotation * (topRight - baseVertex) + baseVertex;
            bottomRight = rotation * (bottomRight - baseVertex) + baseVertex;
            
            Vector3 apex = baseVertex; // Apex at the origin

            Vector3[] vertices = new Vector3[]
            {
                bottomLeft, bottomRight, topLeft,  // Base
                topLeft, bottomRight, topRight,
                bottomLeft, topLeft, apex,         // Side 1                
                topLeft, topRight, apex,           // Side 2                
                topRight, bottomRight, apex,       // Side 3      
                bottomRight, bottomLeft, apex      // Side 4
            };

            int[] triangles = new int[]
            {
                0, 1, 2, // Base
                3, 4, 5,

                6, 7, 8, // Sides
                9, 10, 11,
                12, 13, 14,
                15, 16, 17
            };                       
            meshCollider.sharedMesh = null; // Reset mesh
            meshCollider.sharedMesh = mesh; // Assign the updated mesh          

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
        #endregion
    }
}