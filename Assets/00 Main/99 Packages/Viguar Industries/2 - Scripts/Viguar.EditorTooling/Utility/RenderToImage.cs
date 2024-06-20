using UnityEditor;
using UnityEngine;
using System.IO;
using Viguar.EditorTooling.InspectorUITools.InspectorButton;

namespace Viguar.EditorTooling.Utility.CameraRendering
{
    public class RenderToImage : MonoBehaviour
    {
        [SerializeField] Vector2Int resolution = new Vector2Int(5120, 2160);
        [SerializeField] string filepath = "image.png";
        [Space(10)]
        [ButtonProperty(nameof(SaveToFile))]
        public bool saveButton;

        void OnValidate()
        {
            string path = filepath.Split('.')[0];
            if (string.IsNullOrEmpty(path))
                path = "image";
            filepath = path + ".png";
        }

        [ContextMenu("SaveToFile")]
        public void SaveToFile()
        {
            print("Attempting to render current camera to " + filepath);
            var targetCamera = GetCamera();
            if (!targetCamera)
                return;

            var renderTexture = RenderToTexture(targetCamera);
            var texture = ToTexture2D(renderTexture);
            WriteToFile(texture);

            texture.EncodeToPNG();           
        }

        private Camera GetCamera()
        {
            var cam = GetComponent<Camera>();
            if (cam)
                return cam;

            return Camera.main;
        }

        private RenderTexture RenderToTexture(Camera camera)
        {
            var renderTexture = new RenderTexture(resolution.x, resolution.y, 24);

            var previousTarget = camera.targetTexture;
            camera.targetTexture = renderTexture;
            camera.Render();
            camera.targetTexture = previousTarget;
            return renderTexture;
        }

        private Texture2D ToTexture2D(RenderTexture renderTexture)
        {
            var previouslyActive = RenderTexture.active;
            RenderTexture.active = renderTexture;

            var texture = new Texture2D(resolution.x, resolution.y, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, resolution.x, resolution.y), 0, 0, false);

            RenderTexture.active = previouslyActive;

            return texture;
        }

        private void WriteToFile(Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(filepath, bytes);
        }
    }
}