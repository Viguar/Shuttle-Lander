using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Inspector.PropertyFields;
using System;
using System.Linq;
using TMPro;
using System.Reflection;
using UnityEngine.UI;

public class GUISettingsComponent : MonoBehaviour
{
    //This component configures the setting element. What type it is, texts, default states, connected variable names etc. etc.

    #region Checklist
    //  Checklist:
    //      
    //      -For Headers, only need to inspector-draw the text it should display. (Maybe later add sub header options)
    //      -For Spacers, nothing needs to be displayed.
    //      -For Text, inspector-draw a string field, and add a "text type" option: e.g. Normal Text or Warning. Add an icon option too.
    //      
    //      FOR EVERY SETTING TYPE WE NEED A "DRAWIF" FEATURE
    //      (E.g, when a bool is true, draw these headers, and options, else, don't, or if a slider value is over a certain value)
    //      This feature should to be only executed in runtime because otherwise editing will be annoying :D
    //      also dont destroy the settings types in the case, but only setactive = false, so they can turn on again lol :p
    //
    //      Checklist Settings Configs. (Note: Default State in here means in case there is not a settings config file or something)
    //
    //      -UniToggle:
    //          -Settings Display Name
    //          -Connected Value
    //          -Default State
    //      -SelectorToggle:
    //          -Settings Display Name
    //          -Connected Value       Comment: I think to unify things, this should be an enum (even if its only two options or something)
    //          -Default State
    //      -Slider:
    //          -Settings Display Name
    //          -Connected Value       Comment: We then need to evaluate the type of connected value: float or int? (can be changed in the slider component via script)
    //          -Associated Units       Comment: Like %, or dB or whatever.
    //          -Min / Max
    //          -DefaultState
    //      -Dropdown:
    //          -Settings Display Name
    //          -Connected Value        WARNING: In dropdowns, this is apparently an int and not the string of the dropdown!!
    //          -DefaultState
    //      -InputField:
    //          -Settings Display Name
    //          -Connected Value        Comment: Evaluate the type of connected value: String, Int, Float? We can then restrict input.    
    //          -Default State
    //      -Button:
    //          -Settings Display Name
    //          -Events on Button Press...
    //          -Button Type            (Text or Boxed Text, with or without icon)
    //
    //
    //
    #endregion
    public enum FieldTypes { type_bool, type_string, type_float, type_int, type_Vector2, type_Vector3, type_Vector4, type_enum}

    private GameObject settingsElement; //The matching configurable gameobject for the setting. (Slider, Toggle, etc.)
    private TMP_Text settingsDisplayText;
    private AppSettingsManager appSettingsManager;

    private FieldInfo fieldInfo;        //The variable (like Gameobject <myGameobject>)
    private Type fieldType;             //The type of the variable (like bool, float, GameObject)
    private object fieldValue;          //The value of the variable.
    private FieldTypes eType;           //The type as an enum.

    //UI Components
    private Toggle UniToggleComponent;
    private Slider SliderComponent;

    [Header("Setting Type Configuration")]
    public GUISettingElementTypes GUISettingElementType; //Compare to element types in the children configs. (Like a tag.)
    [Space(20)]    

    //Draw cases GUI Organisation

    [DrawIf("GUISettingElementType", GUISettingElementTypes.Header)]
    public HeaderElementData HeaderElementData;


    //Draw cases GUI Setting Elements

    [DrawIf("GUISettingElementType", GUISettingElementTypes.UniToggle)]
    public UniToggleElementData UniToggleConfiguration;

    //[DrawIf("GUISettingElementType", GUISettingElementTypes.SelectorToggle)]
    //public UniToggleElementData SelectorToggleConfiguration;

    [DrawIf("GUISettingElementType", GUISettingElementTypes.Slider)]
    public SliderElementData SliderConfiguration;


    private void OnValidate()
    {
        ConfigureSettingComponentEditor();
    }

    public void InitComponent(AppSettingsManager settingsManager)
    {
        appSettingsManager = settingsManager;
    }

    public void ConfigureSettingComponentEditor()
    {
        ResetSettingConfigEditor();

        switch (GUISettingElementType)
        {
            case GUISettingElementTypes.None:
                break;


            case GUISettingElementTypes.Header:
                break;
            case GUISettingElementTypes.Spacer:
                break;
            case GUISettingElementTypes.Text: 
                break;


            case GUISettingElementTypes.UniToggle: 
                break;
            case GUISettingElementTypes.SelectorToggle: 
                break;
            case GUISettingElementTypes.Slider:
                break;
            case GUISettingElementTypes.Dropdown:
                break;
            case GUISettingElementTypes.InputField:
                break;
            case GUISettingElementTypes.Button: 
                break;
        }
    }
    public void ConfigureSettingComponentRuntime()
    {
        switch (GUISettingElementType)
        {
            case GUISettingElementTypes.None:
                break;


            case GUISettingElementTypes.Header:
                break;
            case GUISettingElementTypes.Spacer:
                break;
            case GUISettingElementTypes.Text:
                break;


            case GUISettingElementTypes.UniToggle:
                ConfigUniToggle();
                break;
            case GUISettingElementTypes.SelectorToggle:
                break;
            case GUISettingElementTypes.Slider:
                ConfigSlider();
                break;
            case GUISettingElementTypes.Dropdown:
                break;
            case GUISettingElementTypes.InputField:
                break;
            case GUISettingElementTypes.Button:
                break;

        }
    }


    private void ConfigUniToggle()
    {
        switch(eType)
        {
            case FieldTypes.type_bool:
                UniToggleComponent = settingsElement.GetComponentInChildren<Toggle>();
                UniToggleComponent.onValueChanged.AddListener(value => OnSettingChanged(value));
                break;

            default:
                break;
        }
    }
    private void ConfigSlider()
    {
        switch(eType)
        {
            case FieldTypes.type_float:
                SliderComponent = settingsElement.GetComponentInChildren<Slider>();
                SliderComponent.onValueChanged.AddListener(value => OnSettingChanged(value));

                SliderConfiguration.SliderType = SliderElementData.SliderTypes.FloatSlider;
                SliderComponent.minValue = SliderConfiguration.FloatSliderCapMin;
                SliderComponent.maxValue = SliderConfiguration.FloatSliderCapMax;
                SliderComponent.value = (float)fieldValue;               
                break;

            case FieldTypes.type_int:
                SliderComponent = settingsElement.GetComponentInChildren<Slider>();
                SliderComponent.onValueChanged.AddListener(value => OnSettingChanged(value));

                SliderConfiguration.SliderType = SliderElementData.SliderTypes.FloatSlider;
                SliderComponent.minValue = SliderConfiguration.IntSliderCapMin;
                SliderComponent.maxValue = SliderConfiguration.IntSliderCapMax;
                SliderComponent.value = (int)fieldValue;
                break;

            default:
                break;
        }
    }


    private void OnSettingChanged<T>(T value) 
    { 
        fieldInfo.SetValue(appSettingsManager._AppSettings, value); //Set the value in the settings class (Making it essentially ready to be saved to .json);
    }
    private void ResolveConnectedSettingField(string connectedSetting)
    {
        fieldInfo = typeof(ApplicationSettings).GetField(connectedSetting, BindingFlags.Public | BindingFlags.Instance);
        fieldType = fieldInfo.FieldType;
        fieldValue = fieldInfo.GetValue(appSettingsManager._AppSettings);
        ResolveTypeEnum(fieldType);
    }
    private void ResolveTypeEnum(Type type)
    {
        if(type == null) { }
        else if(type == typeof(bool)) { eType = FieldTypes.type_bool; }
        else if(type == typeof(string)) { eType = FieldTypes.type_string; }
        else if(type == typeof(float)) { eType = FieldTypes.type_float; }
        else if(type == typeof(int)) { eType = FieldTypes.type_int; }
        else if(type == typeof(Vector2)) { eType = FieldTypes.type_Vector2; }
        else if(type == typeof(Vector3)) { eType = FieldTypes.type_Vector3; }
        else if(type == typeof(Vector4)) { eType = FieldTypes.type_Vector4; }
        else if(type.IsEnum) { eType = FieldTypes.type_enum; }
    }
    private void ResetSettingConfigEditor()
    {
        settingsElement = null;
        settingsDisplayText = null;
        List<GUISettingsElement> list = GetComponentsInChildren<GUISettingsElement>(true).ToList();
        list.ForEach(obj => obj.gameObject.SetActive(false));
        settingsElement = list.Find(dat => dat.ElementType == GUISettingElementType)?.gameObject;
        settingsDisplayText = GetComponentInChildren<GUISettingsDisplayText>()?.gameObject?.GetComponentInChildren<TMP_Text>();

        
        if (settingsElement != null) 
        { 
            settingsElement.SetActive(true); 
        }
        else
        {
            list.ForEach(obj => obj.gameObject.SetActive(false));
        }
    }
}

