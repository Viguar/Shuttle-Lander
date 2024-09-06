using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Viguar.Inspector.PropertyFields
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class GameObjectLayerAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR
namespace Viguar.Inspector.PropertyFields
{
    [CustomPropertyDrawer(typeof(GameObjectLayerAttribute))]
    public class GameObjectLayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                position = EditorGUI.PrefixLabel(position, label);
                EditorGUI.LabelField(position, String.Format("Error: {0} attribute can be applied only to {1} type", typeof(GameObjectLayerAttribute), SerializedPropertyType.Integer));
                return;
            }

            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}
#endif