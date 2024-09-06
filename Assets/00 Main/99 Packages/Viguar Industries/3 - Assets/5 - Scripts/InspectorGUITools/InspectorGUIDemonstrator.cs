using UnityEngine;

//Call in namespaces
using Viguar.Inspector.PropertyFields;


public class InspectorGUIDemonstrator : MonoBehaviour
{
    #region Constructor
    #region Constructor ConditionalProperties Demo
    //This Property Attribute draws variables only when a certain condition has been met.
    //The syntax is [DrawIf(string comparedPropertyName, object comparedValue)] => In other words [DrawIf("IfVariable", IsThisValue)].
    //In this following example, the string OnTicked is only shown in the inspector when the bool TickMe returns true.
    //This can be really useful for testing & visually configuring things.
    [Header("Conditional Property Display Demo")]
    [SerializeField] private bool TickMe = false;
    [SerializeField] [DrawIf("TickMe", true)] private string OnTicked = "This Option is ticked!";
    [Space(10)]
    #endregion

    #region Constructor InspectorButton Demo
    //This Property Attribute Draws a Button in the Inspector that executes a function also in Edit Mode.
    //The syntax is [ButtonProperty(nameof(NameOfTheFunction))]. The Property Attribute needs to be followed by a placeholder variable.
    //The function the button refers to needs to be declared public.
    [Header("Inspector Button Demo")]
    [ButtonProperty(nameof(DemonstrateInspectorButton))]
    [SerializeField] private bool InspectorButton;
    [Space(10)]
    #endregion

    #region Constructor OverrideLabels Demo
    //This Property Attribute replaces the name of the variable with a custom string through the attribute.
    //The syntax is [LabelOverride(string displayName)]
    //This can be really useful for complicated, or hard to abbreviate varaible names.
    [Header("Override Label Demo")]
    [SerializeField][LabelOverride("New Display Name")] private bool ComplicatedLongVariableName;
    [Space(10)]
    #endregion

    #region Constructor ReadOnlyLabels Demo
    //This Property Attribute makes the variable non-editable through the inspector.
    //This can be really useful for debugging and monitoring.
    //The syntax is [ReadOnly].
    [Header("ReadOnly Label Demo")]
    [SerializeField] [ReadOnly] private bool ThisBoolIsFalse = false;
    [SerializeField] [ReadOnly] private bool ThisBoolIsTrue = true;
    [Space(10)]
    #endregion

    #region Constructor GameObjectLayer Demo
    //This Property Attribute displays the selected layer and converts it to the respective integer in script.
    //The syntax is [GameObjectLayer].
    [Header("GameObjectLayer Demo")]
    [GameObjectLayer]
    public int LayerInteger;
    [Space(10)]
    #endregion

    #region Constructor GameObjectTag Demo
    //This Property Attribute displays the selected tag and converts it to the respective string in script. Works just like the Layer Field.
    //The syntax is [GameObjectTag].
    [Header("GameObjectTag Demo")]
    [GameObjectTag]
    public string TagString;
    #endregion

    #endregion



    public void DemonstrateInspectorButton()
    {
        print("You pressed this button in Edit Mode!");
    }
}
