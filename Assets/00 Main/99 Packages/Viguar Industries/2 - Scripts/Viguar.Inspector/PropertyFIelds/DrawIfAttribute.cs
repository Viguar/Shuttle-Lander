using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Usage:
//  Enumerators:    [DrawIf("EnumTest", ShowValueEnum.ShowValue1)]
//  Bools:          [DrawIf("someBool", true, ComparisonType.Equals, DisablingType.ReadOnly)]
//  Float:          [DrawIf("someFloat", 1f, ComparisonType.GreaterOrEqual)] //Todo

// Attribute
namespace Viguar.Inspector.PropertyFields
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class DrawIfAttribute : PropertyAttribute
    {
        public string comparedPropertyName { get; private set; }
        public object comparedValue { get; private set; }
        public DisablingType disablingType { get; private set; }

        public enum DisablingType
        {
            ReadOnly = 2,
            DontDraw = 3
        }

        public DrawIfAttribute(string comparedPropertyName, object comparedValue, DisablingType disablingType = DisablingType.DontDraw)
        {
            this.comparedPropertyName = comparedPropertyName;
            this.comparedValue = comparedValue;
            this.disablingType = disablingType;
        }
    }
}

// PropertyDrawer
#if UNITY_EDITOR
namespace Viguar.Inspector.PropertyFields
{
    [CustomPropertyDrawer(typeof(DrawIfAttribute))]
    public class DrawIfPropertyDrawer : PropertyDrawer
    {
        private DrawIfAttribute drawIf;
        private SerializedProperty comparedField;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!ShowMe(property) && drawIf.disablingType == DrawIfAttribute.DisablingType.DontDraw)
                return 0f;

            return CalculatePropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShowMe(property))
            {
                DrawProperty(position, property, label);
            }
            else if (drawIf.disablingType == DrawIfAttribute.DisablingType.ReadOnly)
            {
                GUI.enabled = false;
                DrawProperty(position, property, label);
                GUI.enabled = true;
            }
        }

        private bool ShowMe(SerializedProperty property)
        {
            drawIf = attribute as DrawIfAttribute;
            string path = property.propertyPath.Contains(".") 
                ? System.IO.Path.ChangeExtension(property.propertyPath, drawIf.comparedPropertyName) 
                : drawIf.comparedPropertyName;

            comparedField = property.serializedObject.FindProperty(path);

            if (comparedField == null)
            {
                Debug.LogError("Cannot find property with name: " + path);
                return true;
            }

            switch (comparedField.type)
            {
                case "bool":
                    return comparedField.boolValue.Equals(drawIf.comparedValue);
                case "Enum":
                    return comparedField.enumValueIndex.Equals((int)drawIf.comparedValue);
                default:
                    Debug.LogError("Unsupported property type: " + comparedField.type + " for " + path);
                    return true;
            }
        }

        private void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private float CalculatePropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 0f;

            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                totalHeight += EditorGUI.GetPropertyHeight(property, label, true);
            }
            else
            {
                totalHeight += EditorGUI.GetPropertyHeight(property, label, true);
            }

            return totalHeight;
        }
    }
}
#endif
