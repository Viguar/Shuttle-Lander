using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft.Management
{
    public class GlobalDebugManager : MonoBehaviour
    {
        private DebugMeshManager[] debugMeshes;

        private void Start()
        {
            InitDebugMeshRendering();
        }

        //Inits
        private void InitDebugMeshRendering()
        {
            debugMeshes = FindObjectsByType<DebugMeshManager>(FindObjectsSortMode.None);
            foreach (DebugMeshManager debugMesh in debugMeshes)
            {
                debugMesh.InitDebugMeshManager();
            }
        }

        //Functions
        public void ToggleDebugMeshRenderingState()
        {
            foreach (DebugMeshManager debugMesh in debugMeshes)
            {
                debugMesh.ToggleDebugMeshRenderer();
            }
        }

        public void ForceDebugMeshRenderingState(bool forcedState)
        {
            foreach (DebugMeshManager debugMesh in debugMeshes)
            {
                debugMesh.ForceDebugMeshRendererState(forcedState);
            }
        }
    }
}
