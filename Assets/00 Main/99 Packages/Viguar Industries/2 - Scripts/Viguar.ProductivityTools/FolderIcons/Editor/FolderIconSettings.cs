using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Viguar.ProductivityTools
{
    [CreateAssetMenu (fileName = "Folder Icon Manager", menuName = "Viguar/Productivity/Folder & Icon Manager")]
    public class FolderIconSettings : ScriptableObject
        {
        [Serializable]
        public class FolderIcon
            {
            public DefaultAsset folder;

            public Texture2D folderIcon;
            public Texture2D overlayIcon;
            }

        //Global Settings
        public bool showOverlay = true;
        public bool showCustomFolder = true;

        public FolderIcon[] icons;
        }
    }
