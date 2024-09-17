using System.Collections;
using UnityEngine;
using Viguar.Aircraft;
using TMPro;

public class ValueDisplayer : MonoBehaviour
{
    private TMP_Text textfield;
    public enum valueType { Float, Bool, Vector, String, }
    public valueType displayValue;
    public string variableName;

    private Color DefaultTextColor;
    private Color PositiveTextColor;
    private Color NegativeTextColor;

    public void SetText(string Text)
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
        textfield = GetComponent<TMP_Text>();
        DefaultTextColor = defCol;
        PositiveTextColor = posCol;
        NegativeTextColor = negCol;
    }
}
