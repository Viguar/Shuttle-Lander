using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft.Management
{
    public class DebugMeshManager : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

        public void InitDebugMeshManager()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            ForceDebugMeshRendererState(false);
        }

        public void ToggleDebugMeshRenderer()
        {
            meshRenderer.enabled = !meshRenderer.enabled;
        }

        public void ForceDebugMeshRendererState(bool forcedState)
        {
            meshRenderer.enabled = forcedState;
        }
    }
}
