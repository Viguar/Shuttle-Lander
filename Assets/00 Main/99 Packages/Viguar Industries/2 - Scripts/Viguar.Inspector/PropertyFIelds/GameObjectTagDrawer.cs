using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Viguar.Inspector.PropertyFields
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class GameObjectTagAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR
namespace Viguar.Inspector.PropertyFields
{ 
    [CustomPropertyDrawer(typeof(GameObjectTagAttribute))]
    public class GameObjectTagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                position = EditorGUI.PrefixLabel(position, label);
                EditorGUI.LabelField(position, String.Format("Error: {0} attribute can be applied only to {1} type", typeof(GameObjectTagAttribute), SerializedPropertyType.String));
                return;
            }

            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }
}
#endif