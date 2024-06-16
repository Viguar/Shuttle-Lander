using UnityEditor;
using UnityEngine;
using System;
//Usage:
//  Enumerators:    [DrawIf("EnumTest", ShowValueEnum.ShowValue1)]
//  Bools:          [DrawIf("someBool", true, ComparisonType.Equals, DisablingType.ReadOnly)]
//  Float:          [DrawIf("someFloat", 1f, ComparisonType.GreaterOrEqual)] //Todo
namespace Viguar.EditorTooling.GUITools.ConditionalPropertyDisplay
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class DrawIfAttribute : PropertyAttribute
    {
        #region Fields

        public string comparedPropertyName { get; private set; }
        public object comparedValue { get; private set; }
        public DisablingType disablingType { get; private set; }

        /// <summary>
        /// Types of comperisons.
        /// </summary>
        public enum DisablingType
        {
            ReadOnly = 2,
            DontDraw = 3
        }

        #endregion

        /// <summary>
        /// Only draws the field only if a condition is met. Supports enum and bools.
        /// </summary>
        /// <param name="comparedPropertyName">The name of the property that is being compared (case sensitive).</param>
        /// <param name="comparedValue">The value the property is being compared to.</param>
        /// <param name="disablingType">The type of disabling that should happen if the condition is NOT met. Defaulted to DisablingType.DontDraw.</param>
        public DrawIfAttribute(string comparedPropertyName, object comparedValue, DisablingType disablingType = DisablingType.DontDraw)
        {
            this.comparedPropertyName = comparedPropertyName;
            this.comparedValue = comparedValue;
            this.disablingType = disablingType;
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(DrawIfAttribute))]
        public class DrawIfPropertyDrawer : PropertyDrawer
        {
            #region Fields

            // Reference to the attribute on the property.
            DrawIfAttribute drawIf;

            // Field that is being compared.
            SerializedProperty comparedField;

            #endregion

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                if (!ShowMe(property) && drawIf.disablingType == DrawIfAttribute.DisablingType.DontDraw)
                    return 0f;

                // The height of the property should be defaulted to the default height.
                return base.GetPropertyHeight(property, label);
            }
            private bool ShowMe(SerializedProperty property)
            {
                drawIf = attribute as DrawIfAttribute;
                // Replace propertyname to the value from the parameter
                string path = property.propertyPath.Contains(".") ? System.IO.Path.ChangeExtension(property.propertyPath, drawIf.comparedPropertyName) : drawIf.comparedPropertyName;

                comparedField = property.serializedObject.FindProperty(path);

                if (comparedField == null)
                {
                    Debug.LogError("Cannot find property with name: " + path);
                    return true;
                }
                // get the value & compare based on types
                switch (comparedField.type)
                { // Possible extend cases to support your own type
                    case "bool":
                        return comparedField.boolValue.Equals(drawIf.comparedValue);
                    case "Enum":
                        return comparedField.enumValueIndex.Equals((int)drawIf.comparedValue);
                    default:
                        Debug.LogError("Error: " + comparedField.type + " is not supported of " + path);
                        return true;
                }
            }
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                // If the condition is met, simply draw the field.
                if (ShowMe(property))
                {
                    EditorGUI.PropertyField(position, property);
                } //...check if the disabling type is read only. If it is, draw it disabled
                else if (drawIf.disablingType == DrawIfAttribute.DisablingType.ReadOnly)
                {
                    GUI.enabled = false;
                    EditorGUI.PropertyField(position, property);
                    GUI.enabled = true;
                }
            }
        }
#endif
    }
}

