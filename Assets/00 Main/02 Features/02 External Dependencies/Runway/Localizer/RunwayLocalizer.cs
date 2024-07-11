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
        public float _LocalizerVerticalWindow = 10;
        public float _LocalizerWidth = 20;
        public float _LocalizerAngle = 4;
        
        [HideInInspector] public Vector3[] LocalizerConeVertices;
        private MeshCollider _mc;
        private Rigidbody _rb;

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
            AssignMeshColliderProperties(_mc);
            AssignRigidbodyProperties(_rb);
        }
        private void AssignMeshColliderProperties(MeshCollider mc)
        {
            mc.convex = true;
            mc.isTrigger = true;
        }
        private void AssignRigidbodyProperties(Rigidbody rb)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }


        #region Localizer Mesh Shape Creation
        private void OnValidate()
        {
            if (LocalizerConeVertices == null || LocalizerConeVertices.Length != 8)
            {
                LocalizerConeVertices = new Vector3[5];
                LocalizerConeVertices[4] = new Vector3(0, 0, 0); // Initialize the apex at the game object's origin.
            }
            UpdateLocalizerCone();
        }

        public void UpdateLocalizerCone()
        {
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (meshCollider == null) { meshCollider = gameObject.AddComponent<MeshCollider>(); }
            if (meshFilter == null) { meshFilter = gameObject.AddComponent<MeshFilter>(); }
            if (meshRenderer == null) { meshRenderer = gameObject.AddComponent<MeshRenderer>(); meshRenderer.material = new Material(Shader.Find("Standard")); }
            if (rigidbody == null) { rigidbody = gameObject.AddComponent<Rigidbody>(); }

            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;
            float halfVerticalSize = _LocalizerRange * Mathf.Tan(_LocalizerVerticalWindow * Mathf.Deg2Rad / 2);
            float halfHorizontalSize = _LocalizerRange * Mathf.Tan(_LocalizerWidth * Mathf.Deg2Rad / 2);

            // Define the vertices of a pyramid in local space, inverted along the Z-axis
            Vector3 baseVertex = new Vector3(0, 0, 0);  // Base vertex at the origin
            Vector3 bottomLeft = new Vector3(-halfHorizontalSize, -halfVerticalSize, _LocalizerRange); // Bottom-left
            Vector3 topLeft = new Vector3(-halfHorizontalSize, halfVerticalSize, _LocalizerRange);    // Top-left
            Vector3 topRight = new Vector3(halfHorizontalSize, halfVerticalSize, _LocalizerRange);    // Top-right
            Vector3 bottomRight = new Vector3(halfHorizontalSize, -halfVerticalSize, _LocalizerRange); // Bottom-right


            Quaternion rotation = Quaternion.Euler(-_LocalizerAngle, 0, 0); // Rotate around the x-axis from the base vertex

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
            AssignMeshColliderProperties(meshCollider); //Assign the correct values

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
        #endregion
    }
}