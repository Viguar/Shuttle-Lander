using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Viguar.Inspector.PropertyFields;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SelectableLevelInfo
{
    [ReadOnly] public string _SceneName;
    [ReadOnly] public string _ScenePath;
#if UNITY_EDITOR
    [SerializeField] public SceneAsset _Scene;
#endif
    public string _SceneDisplayName;
    public Texture2D _SceneDisplayImage;
    [DrawIf("_HasOverlayImage", true)] public Texture2D _SceneOverlayImage;   
    public bool _HasOverlayImage;    
}
