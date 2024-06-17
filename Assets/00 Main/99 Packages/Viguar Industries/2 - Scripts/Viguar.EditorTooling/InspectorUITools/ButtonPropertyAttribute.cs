using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Viguar.EditorTooling.InspectorUITools.InspectorButton
{ 
    public class ButtonPropertyAttribute : PropertyAttribute
    {
        public string buttonText;
        public string buttonMethod;
        public ButtonPropertyAttribute(string buttonMethodName) { buttonMethod = buttonMethodName; }

        #if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(ButtonPropertyAttribute))]
        public class ButtonPropertyAttributeDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                string methodName = (attribute as ButtonPropertyAttribute).buttonMethod;
                string displayText = (attribute as ButtonPropertyAttribute).buttonText;
                Object target = property.serializedObject.targetObject;
                System.Type type = target.GetType();
                System.Reflection.MethodInfo method = type.GetMethod(methodName);

                if(method == null) { GUI.Label(position, "No method found or assigned. Is it public"); return; }
                if(method.GetParameters().Length > 0) { GUI.Label(position, "Method cannot contain parameters."); return; }
                if(GUI.Button(position, method.Name)) { method.Invoke(target, null); }
            }
        }
        #endif
    }
    //Example Usage in Script:
    //[ButtonProperty(nameof(methodName))]
    //public bool methodButtonFieldBoolName;
}


