using System.Collections;
using UnityEngine;
using Viguar.Aircraft;
using TMPro;


public class ValueDisplayer : MonoBehaviour
{
    private AircraftBaseProcessor _configBaseProcessor;
    private TMP_Text textfield;
    public enum valueType { Float, Bool, Vector, String, }
    public valueType displayValue;
    public string variableName;

    private Color DefaultTextColor;
    private Color PositiveTextColor;
    private Color NegativeTextColor;

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
        SetTextColor(Text);
    }
    private void SetTextColor(string Text)
    {        
        if(Text == "True" || Text == "Stable") //Positive Text
        {            
            textfield.color = PositiveTextColor;
        }
        else if(Text == "False" || Text == "Critical" || Text == "Upset") //Negative Text
        {
            textfield.color = NegativeTextColor;
        }
        else //Set DefaultColor
        {
            textfield.color = DefaultTextColor;
        }
    }
    public void InitDisplayer(Color defCol, Color posCol, Color negCol)
    {
        _configBaseProcessor = GetComponentInParent<AircraftBaseProcessor>();
        textfield = GetComponent<TMP_Text>();
        DefaultTextColor = defCol;
        PositiveTextColor = posCol;
        NegativeTextColor = negCol;
    }
}
