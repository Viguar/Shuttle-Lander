using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft.Runways
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider))]
    public class RunwayZoneDrawer : MonoBehaviour
    {
        private MeshFilter meshFilter;
        private BoxCollider boxCollider;
        private RunwayZone runwayZone;

        void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
            boxCollider = GetComponent<BoxCollider>();
            runwayZone = GetComponentInParent<RunwayZone>();
            CreateCube();
        }

        public void OnRunwayZoneValidate()
        {
            if (meshFilter == null) { meshFilter = GetComponent<MeshFilter>(); }
            if (boxCollider == null) { boxCollider = GetComponent<BoxCollider>(); }
            if (runwayZone == null) { runwayZone = GetComponentInParent<RunwayZone>(); }
            CreateCube();
        }

        private void CreateCube()
        {
            Mesh mesh = new Mesh();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Vector3 halfSize = new Vector3(runwayZone._RunwayZoneWidth / 2f, 0, runwayZone._RunwayZoneLength / 2f);

            Vector3[] vertices = new Vector3[]
            {                       
                new Vector3(-halfSize.x, runwayZone._RunwayZoneHeight, -halfSize.z), // Top
                new Vector3(halfSize.x, runwayZone._RunwayZoneHeight, -halfSize.z),
                new Vector3(halfSize.x, runwayZone._RunwayZoneHeight, halfSize.z),
                new Vector3(-halfSize.x, runwayZone._RunwayZoneHeight, halfSize.z),
            
                new Vector3(-halfSize.x, 0, halfSize.z), // Front
                new Vector3(halfSize.x, 0, halfSize.z),
                new Vector3(halfSize.x, runwayZone._RunwayZoneHeight, halfSize.z),
                new Vector3(-halfSize.x, runwayZone._RunwayZoneHeight, halfSize.z),
            
                new Vector3(halfSize.x, 0, -halfSize.z), // Back
                new Vector3(-halfSize.x, 0, -halfSize.z),
                new Vector3(-halfSize.x, runwayZone._RunwayZoneHeight, -halfSize.z),
                new Vector3(halfSize.x, runwayZone._RunwayZoneHeight, -halfSize.z),
           
                new Vector3(-halfSize.x, 0, -halfSize.z), // Left
                new Vector3(-halfSize.x, 0, halfSize.z),
                new Vector3(-halfSize.x, runwayZone._RunwayZoneHeight, halfSize.z),
                new Vector3(-halfSize.x, runwayZone._RunwayZoneHeight, -halfSize.z),
            
                new Vector3(halfSize.x, 0, halfSize.z), // Right
                new Vector3(halfSize.x, 0, -halfSize.z),
                new Vector3(halfSize.x, runwayZone._RunwayZoneHeight, -halfSize.z),
                new Vector3(halfSize.x, runwayZone._RunwayZoneHeight, halfSize.z),
            };

            int[] triangles = new int[]
            {
                0, 2, 1, // Top
                0, 3, 2,

                4, 6, 5, // Front
                4, 7, 6,

                8, 10, 9, // Back
                8, 11, 10,

                12, 14, 13, // Left
                12, 15, 14,

                16, 18, 17, // Right
                16, 19, 18
            };

            Vector3[] normals = new Vector3[]
            {     
                Vector3.up, Vector3.up, Vector3.up, Vector3.up, // Top            
                Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward, // Front            
                Vector3.back, Vector3.back, Vector3.back, Vector3.back, // Back            
                Vector3.left, Vector3.left, Vector3.left, Vector3.left, // Left             
                Vector3.right, Vector3.right, Vector3.right, Vector3.right // Right
            };

            Vector2[] uv = new Vector2[]
            {         
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), // Top            
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), // Front           
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), // Back            
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), // Left            
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) // Right
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uv;

            meshFilter.mesh = mesh;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
            // Configure the BoxCollider
            boxCollider.size = new Vector3(runwayZone._RunwayZoneWidth, runwayZone._RunwayZoneHeight, runwayZone._RunwayZoneLength);
            boxCollider.center = new Vector3(0, runwayZone._RunwayZoneHeight / 2f, 0);
            boxCollider.isTrigger = true;
        }
    }
}
