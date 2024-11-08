using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
namespace Viguar.Inspector.PropertyFields
{
    [CustomPropertyDrawer(typeof(Enum), true)]
    public class GenericEnumDividerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Ensure we're working with an enum
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                EditorGUI.LabelField(position, label.text, "Use [Enum] with EnumDividerDrawer.");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Get enum names and prepare displayed options with dividers
            string[] enumNames = property.enumNames;
            GUIContent[] displayedOptions = new GUIContent[enumNames.Length];
            int currentIndex = property.enumValueIndex;

            for (int i = 0; i < enumNames.Length; i++)
            {
                // If the enum name includes "Divider" or starts with "_", treat it as a divider
                if (enumNames[i].Contains("Divider") || enumNames[i].StartsWith("_"))
                {
                    displayedOptions[i] = new GUIContent("");
                }
                else
                {
                    displayedOptions[i] = new GUIContent(enumNames[i]);
                }
            }

            // Draw the popup
            int newIndex = EditorGUI.Popup(position, currentIndex, displayedOptions);
            if (newIndex != currentIndex)
            {
                property.enumValueIndex = newIndex;
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif