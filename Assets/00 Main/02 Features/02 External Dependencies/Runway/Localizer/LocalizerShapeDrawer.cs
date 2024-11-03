using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft.Runways
{
    [ExecuteAlways]    
    public class LocalizerShapeDrawer : MonoBehaviour
    {
        private RunwayLocalizer rwLocalizer;
        private GameObject _CenteredLocalizerObject;
        private GameObject _OffsetLocalizerObject;
        [HideInInspector] public Vector3[] _CenteredLocalizerVertices;
        [HideInInspector] public Vector3[] _OffsetLocalizerVertices;        

        public void OnLocalizerValidate()
        {
            rwLocalizer = GetComponent<RunwayLocalizer>();

            _CenteredLocalizerObject = GameObject.FindGameObjectWithTag("runwayLocalizerOnGlideSlope");
            _OffsetLocalizerObject = GameObject.FindGameObjectWithTag("runwayLocalizerOffGlideSlope");

            if (_CenteredLocalizerObject != null)
            {
                _CenteredLocalizerObject.transform.localPosition = Vector3.zero;
                _CenteredLocalizerObject.GetComponent<LocalizerZoneDetector>().LocalizerType = LocalizerZoneDetector.LocalizerTypes.Center;
            }                      
            if (_OffsetLocalizerObject != null)
            {
                _OffsetLocalizerObject.transform.localPosition = Vector3.zero;
                _OffsetLocalizerObject.GetComponent<LocalizerZoneDetector>().LocalizerType = LocalizerZoneDetector.LocalizerTypes.Offset;
            }
            
            _CenteredLocalizerVertices = new Vector3[5];
            _OffsetLocalizerVertices = new Vector3[5];

            _CenteredLocalizerVertices[4] = new Vector3(0, 0, 0); // Initialize the apex at the game object's origin.
            _OffsetLocalizerVertices[4] = new Vector3(0, 0, 0);

            if (_CenteredLocalizerObject != null)
            {
                DrawLocalizer(_CenteredLocalizerObject, rwLocalizer._LocalizerVerticalWindowCenter, rwLocalizer._CenteredLocalizerDebugMaterial);
            }
            if (_OffsetLocalizerObject != null)
            {
                DrawLocalizer(_OffsetLocalizerObject, rwLocalizer._LocalizerVerticalWindowCenter + rwLocalizer._LocalizerVerticalWindowOffset, rwLocalizer._OffsetLocalizerDebugMaterial);
            }
        }

        public void DrawLocalizer(GameObject localizerOBJ, float vAngle, Material material)
        {
            MeshCollider meshCollider = localizerOBJ.GetComponent<MeshCollider>();
            MeshFilter meshFilter = localizerOBJ.GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = localizerOBJ.GetComponent<MeshRenderer>();
            Rigidbody rigidbody = localizerOBJ.GetComponent<Rigidbody>();
            if (meshCollider == null) { meshCollider = localizerOBJ.AddComponent<MeshCollider>(); }
            meshCollider.convex = true; meshCollider.isTrigger = true;
            if (meshFilter == null) { meshFilter = localizerOBJ.AddComponent<MeshFilter>(); }
            if (meshRenderer == null) { meshRenderer = localizerOBJ.AddComponent<MeshRenderer>(); }
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; meshRenderer.material = material; meshRenderer.receiveShadows = false;
            if (rigidbody == null) { rigidbody = localizerOBJ.AddComponent<Rigidbody>(); }
            rigidbody.useGravity = false; rigidbody.isKinematic = false;

            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;
            float halfVerticalSize = rwLocalizer._LocalizerRange * Mathf.Tan(vAngle * Mathf.Deg2Rad / 2);
            float halfHorizontalSize = rwLocalizer._LocalizerRange * Mathf.Tan(rwLocalizer._LocalizerWidth * Mathf.Deg2Rad / 2);

            // Define the vertices of a pyramid in local space, inverted along the Z-axis
            Vector3 baseVertex = new Vector3(0, 0, 0);  // Base vertex at the origin
            Vector3 bottomLeft = new Vector3(-halfHorizontalSize, -halfVerticalSize, rwLocalizer._LocalizerRange); // Bottom-left
            Vector3 topLeft = new Vector3(-halfHorizontalSize, halfVerticalSize, rwLocalizer._LocalizerRange);    // Top-left
            Vector3 topRight = new Vector3(halfHorizontalSize, halfVerticalSize, rwLocalizer._LocalizerRange);    // Top-right
            Vector3 bottomRight = new Vector3(halfHorizontalSize, -halfVerticalSize, rwLocalizer._LocalizerRange); // Bottom-right

            Quaternion rotation = Quaternion.Euler(-rwLocalizer._LocalizerGlobalAngle, 0, 0); // Rotate around the x-axis from the base vertex

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
    }
}
