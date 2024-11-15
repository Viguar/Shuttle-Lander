using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Level Data", menuName = "Viguar/GUI/Level Data", order = 0)]
public class GUILevelData : ScriptableObject
{
    public GameObject _SpringboardElementPrefab;
    public List<SelectableLevelInfo> _Levels = new List<SelectableLevelInfo>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach(SelectableLevelInfo level in _Levels)
        {
            if(level._Scene != null) 
            {
                level._SceneName = level._Scene.name;               
                string scenePath = AssetDatabase.GetAssetPath(level._Scene);
                level._ScenePath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            }
            else
            {
                level._SceneName = null;
                level._ScenePath = null;
            }
        }
    }
#endif
}
