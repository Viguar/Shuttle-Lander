using System.Collections;
using UnityEngine;
using Viguar.Aircraft;
using TMPro;


public class ValueDisplayer : MonoBehaviour
{
    private AircraftBaseProcessor _configBaseProcessor;
    private DebugCommander _debugCommander;
    private TMP_Text textfield;
    public enum valueType { Float, Bool, Vector, String, }
    public valueType displayValue;
    public string variableName;

    void Start()
    {
        _configBaseProcessor = GetComponentInParent<AircraftBaseProcessor>();
        _debugCommander = GetComponentInParent<DebugCommander>();
        textfield = GetComponent<TMP_Text>();
    }

    public void DisplayDebugValue()
    {        
        switch (displayValue)
        {
            case valueType.Float:
                _configBaseProcessor.DefineDebugDictionaryFloat();
                _configBaseProcessor.DebugFloatDict.TryGetValue(variableName, out float VariableFloat);
                SetText(VariableFloat.ToString());
                break;
            case valueType.Bool:
                _configBaseProcessor.DefineDebugDictionaryBool();
                _configBaseProcessor.DebugBoolDict.TryGetValue(variableName, out bool VariableBool);
                SetText(VariableBool.ToString());
                break;
            case valueType.Vector:
                _configBaseProcessor.DefineDebugDictionaryVector();
                _configBaseProcessor.DebugVectorDict.TryGetValue(variableName, out Vector3 VariableVector);
                SetText(VariableVector.ToString());
                break;
            case valueType.String:
                _configBaseProcessor.DefineDebugDictionaryString();
                _configBaseProcessor.DebugStringDict.TryGetValue(variableName, out string VariableString);
                SetText(VariableString);
                break;
        }         
    }  
    private void SetText(string Text)
    {
        textfield.text = Text;
        if(Text == "True") { textfield.color = _debugCommander.BoolTrueColor; }
        else if(Text == "False") { textfield.color = _debugCommander.BoolFalseColor; }
        else if(Text == "Stable") { textfield.color = _debugCommander.StateStableColor; }
        else if (Text == "Critical") { textfield.color = _debugCommander.StateCriticalColor; }
        else if (Text == "Upset") { textfield.color = _debugCommander.StateUpsetColor; }
        else { textfield.color = _debugCommander.DefaultTextColor; }
    }

}
