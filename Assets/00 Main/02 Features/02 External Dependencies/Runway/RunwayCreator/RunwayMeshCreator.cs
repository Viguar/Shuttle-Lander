using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RunwayMeshCreator : MonoBehaviour
{
    public float width = 1f;
    public float height = 1f;
    public float depth = 1f;
    public float uvScale = 1f; // New variable to scale the UV shell

    private MeshFilter meshFilter;

    void OnValidate()
    {
        GenerateCube();
    }

    void GenerateCube()
    {
        if (meshFilter == null)
            meshFilter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        mesh.name = "CustomCube";

        // Define the vertices (each face has its own set of vertices for hard edges)
        Vector3[] vertices = new Vector3[24]
        {
            // Front face
            new Vector3(-width/2, -height/2, depth/2), // Bottom-left front
            new Vector3(width/2, -height/2, depth/2),  // Bottom-right front
            new Vector3(width/2, height/2, depth/2),   // Top-right front
            new Vector3(-width/2, height/2, depth/2),  // Top-left front

            // Back face
            new Vector3(width/2, -height/2, -depth/2),  // Bottom-right back
            new Vector3(-width/2, -height/2, -depth/2), // Bottom-left back
            new Vector3(-width/2, height/2, -depth/2),  // Top-left back
            new Vector3(width/2, height/2, -depth/2),   // Top-right back

            // Left face
            new Vector3(-width/2, -height/2, -depth/2), // Bottom-left back
            new Vector3(-width/2, -height/2, depth/2),  // Bottom-left front
            new Vector3(-width/2, height/2, depth/2),   // Top-left front
            new Vector3(-width/2, height/2, -depth/2),  // Top-left back

            // Right face
            new Vector3(width/2, -height/2, depth/2),   // Bottom-right front
            new Vector3(width/2, -height/2, -depth/2),  // Bottom-right back
            new Vector3(width/2, height/2, -depth/2),   // Top-right back
            new Vector3(width/2, height/2, depth/2),    // Top-right front

            // Top face
            new Vector3(-width/2, height/2, depth/2),   // Top-left front
            new Vector3(width/2, height/2, depth/2),    // Top-right front
            new Vector3(width/2, height/2, -depth/2),   // Top-right back
            new Vector3(-width/2, height/2, -depth/2),  // Top-left back

            // Bottom face
            new Vector3(-width/2, -height/2, -depth/2), // Bottom-left back
            new Vector3(width/2, -height/2, -depth/2),  // Bottom-right back
            new Vector3(width/2, -height/2, depth/2),   // Bottom-right front
            new Vector3(-width/2, -height/2, depth/2),  // Bottom-left front
        };

        // Define the triangles (hard edges, so each face has its own set of vertices)
        int[] triangles = new int[36]
        {
            // Front face
            0, 1, 2,
            0, 2, 3,

            // Back face
            4, 5, 6,
            4, 6, 7,

            // Left face
            8, 9, 10,
            8, 10, 11,

            // Right face
            12, 13, 14,
            12, 14, 15,

            // Top face
            16, 17, 18,
            16, 18, 19,

            // Bottom face
            20, 21, 22,
            20, 22, 23
        };

        // Define UVs to maintain proportional scaling (without stretching) and apply UV scaling
        float maxDimension = Mathf.Max(width, height, depth);

        Vector2[] uvs = new Vector2[24]
        {
            // Front face
            new Vector2(0, 0) * uvScale, // Bottom-left front
            new Vector2(width / maxDimension, 0) * uvScale, // Bottom-right front
            new Vector2(width / maxDimension, height / maxDimension) * uvScale, // Top-right front
            new Vector2(0, height / maxDimension) * uvScale, // Top-left front

            // Back face
            new Vector2(0, 0) * uvScale, // Bottom-right back
            new Vector2(width / maxDimension, 0) * uvScale, // Bottom-left back
            new Vector2(width / maxDimension, height / maxDimension) * uvScale, // Top-left back
            new Vector2(0, height / maxDimension) * uvScale, // Top-right back

            // Left face
            new Vector2(0, 0) * uvScale, // Bottom-left back
            new Vector2(depth / maxDimension, 0) * uvScale, // Bottom-left front
            new Vector2(depth / maxDimension, height / maxDimension) * uvScale, // Top-left front
            new Vector2(0, height / maxDimension) * uvScale, // Top-left back

            // Right face
            new Vector2(0, 0) * uvScale, // Bottom-right front
            new Vector2(depth / maxDimension, 0) * uvScale, // Bottom-right back
            new Vector2(depth / maxDimension, height / maxDimension) * uvScale, // Top-right back
            new Vector2(0, height / maxDimension) * uvScale, // Top-right front

            // Top face
            new Vector2(0, 0) * uvScale, // Top-left front
            new Vector2(width / maxDimension, 0) * uvScale, // Top-right front
            new Vector2(width / maxDimension, depth / maxDimension) * uvScale, // Top-right back
            new Vector2(0, depth / maxDimension) * uvScale, // Top-left back

            // Bottom face
            new Vector2(0, 0) * uvScale, // Bottom-left back
            new Vector2(width / maxDimension, 0) * uvScale, // Bottom-right back
            new Vector2(width / maxDimension, depth / maxDimension) * uvScale, // Bottom-right front
            new Vector2(0, depth / maxDimension) * uvScale, // Bottom-left front
        };

        // Assign vertices, triangles, and UVs to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Recalculate normals for correct lighting
        mesh.RecalculateNormals();

        // Set the mesh to the MeshFilter component
        meshFilter.mesh = mesh;
    }
}
