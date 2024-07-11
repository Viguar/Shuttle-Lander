using UnityEngine;
using UnityEditor;

namespace Viguar.Cubes
{
    [ExecuteInEditMode]
    public class CustomCube : MonoBehaviour
    {
        public Vector3[] vertices;

        private void OnValidate()
        {
            if (vertices == null || vertices.Length != 5)
            {
                InitializeVertices();
            }

            UpdateMesh();
        }

        private void InitializeVertices()
        {
            vertices = new Vector3[5];

            float halfBaseSize = 0.5f;
            float height = 1.0f;

            // Define the vertices of a pyramid in local space, with the base rotated 90 degrees on the Z axis
            vertices[0] = new Vector3(0, -halfBaseSize, -halfBaseSize); // Bottom
            vertices[1] = new Vector3(0, halfBaseSize, -halfBaseSize);  // Top
            vertices[2] = new Vector3(0, halfBaseSize, halfBaseSize);   // Top-right
            vertices[3] = new Vector3(0, -halfBaseSize, halfBaseSize);  // Bottom-right
            vertices[4] = new Vector3(height, 0, 0);                    // Apex
        }

        public void UpdateMesh()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
                // Assign a default material to the MeshRenderer
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }

            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;

            // Define the vertices and triangles of the pyramid
            mesh.vertices = vertices;

            mesh.triangles = new int[]
            {
            // Base
            0, 1, 2,
            0, 2, 3,

            // Sides
            0, 1, 4,
            1, 2, 4,
            2, 3, 4,
            3, 0, 4
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

    }
}

namespace Viguar.Cubes
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(CustomCube))]
    public class CustomCubeEditor : Editor
    {
        private const int ApexVertexIndex = 4; // Assuming the apex vertex is the fifth in the vertices array

        private void OnSceneGUI()
        {
            CustomCube customPyramid = (CustomCube)target;

            for (int i = 0; i < customPyramid.vertices.Length; i++)
            {
                if (i == ApexVertexIndex)
                    continue; // Skip handling for the apex vertex

                EditorGUI.BeginChangeCheck();
                Vector3 worldPosition = customPyramid.transform.TransformPoint(customPyramid.vertices[i]);
                Vector3 newTargetPosition = Handles.PositionHandle(worldPosition, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(customPyramid, "Move Vertex");
                    customPyramid.vertices[i] = customPyramid.transform.InverseTransformPoint(newTargetPosition);
                    customPyramid.UpdateMesh();
                }
            }
        }
    }
#endif
}

