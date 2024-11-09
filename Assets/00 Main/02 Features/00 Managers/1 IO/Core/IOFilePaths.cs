using NUnit.Framework;
using UnityEngine;
using Viguar.Inspector.PropertyFields;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "IO File Paths", menuName = "Viguar/DataContainers/IO File Paths", order = 0)]
    public class IOFilePaths : ScriptableObject
    {
        [SerializeField, ReadOnly, LabelOverride("Path Info")]
        private string InformationOnFilePaths = "Filepaths are relative to Application.persistentDataPath."; 
        [SerializeField, ReadOnly, LabelOverride("Syntax")]
        private string InformationOnSyntax = "Syntax: /FolderName/OtherFolderName/ .";
        [LabelOverride("Use Custom File Extension")] public bool _CustomExtension;
        [LabelOverride("Custom File Extension"), DrawIf("_CustomExtension", true)] public string _AppFilesExtension; 
        [Space(10)]
        public List<CustomFilePaths> _AppCustomFilePaths = new List<CustomFilePaths>();
}

