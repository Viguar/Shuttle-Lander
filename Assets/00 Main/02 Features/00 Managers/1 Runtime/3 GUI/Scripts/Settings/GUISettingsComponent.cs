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

    [SerializeField,ReadOnly] private GameObject settingsElement; //The matching configurable gameobject for the setting. (Slider, Toggle, etc.)
    private TMP_Text settingsDisplayText;
    [SerializeField] private bool hideSettingsDisplayText;
    [SerializeField, ReadOnly] private AppSettingsManager appSettingsManager;

    [SerializeField, ReadOnly] private FieldInfo fieldInfo;         //The variable (like Gameobject <myGameobject>)
    [SerializeField, ReadOnly] private Type fieldType;              //The type of the variable (like bool, float, GameObject)
    [SerializeField, ReadOnly] private object fieldValue;           //The value of the variable.
    private FieldTypes eType;                                       //The type as an enum.

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
    private void OnEnable()
    {
        //ConfigureSettingComponentRuntime();
    }
    private void OnDisable()
    {
        appSettingsManager.SaveApplicationSettings();
    }


    public void InitComponent(AppSettingsManager settingsManager)
    {
        appSettingsManager = settingsManager;
        ConfigureSettingComponentRuntime();
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
                settingsDisplayText.gameObject.SetActive(false);
                break;
            case GUISettingElementTypes.Text:
                settingsDisplayText.gameObject.SetActive(false);
                break;


            case GUISettingElementTypes.UniToggle:
                ConfigUniToggleEditor();
                break;
            case GUISettingElementTypes.SelectorToggle: 
                break;
            case GUISettingElementTypes.Slider:                
                ConfigSliderEditor();
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
        ConfigureSettingDisplayComponents();

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
        eType = FieldTypes.type_bool;
        switch (eType)
        {
            case FieldTypes.type_bool:
                //Init the component
                ResolveConnectedSettingField(UniToggleConfiguration.ConnectedVariable);
                UniToggleComponent = settingsElement.GetComponentInChildren<Toggle>(true);              
                UniToggleComponent.onValueChanged.AddListener(value => OnSettingChanged(value));

                //Configure the component and set the gui to the read out value. 
                UniToggleComponent.isOn = (bool)fieldValue;
                settingsDisplayText.text = UniToggleConfiguration.SettingDisplayName;
                break;

            default:
                break;
        }
    }
    private void ConfigUniToggleEditor()
    {
        eType = FieldTypes.type_bool;
        settingsDisplayText.text = UniToggleConfiguration.SettingDisplayName;
    }



    private void ConfigSlider()
    {
        eType = SliderConfiguration.SliderType == SliderElementData.SliderTypes.FloatSlider ? FieldTypes.type_float : FieldTypes.type_int;
        switch (eType)
        {
            case FieldTypes.type_float:
                //Init the component
                ResolveConnectedSettingField(SliderConfiguration.ConnectedVariable);
                SliderComponent = settingsElement.GetComponentInChildren<Slider>(true);                
                
                //Set the value 
                SliderConfiguration.SliderType = SliderElementData.SliderTypes.FloatSlider;
                SliderComponent.wholeNumbers = false;
                SliderComponent.minValue = SliderConfiguration.FloatSliderCapMin;
                SliderComponent.maxValue = SliderConfiguration.FloatSliderCapMax;
                SliderComponent.value = (float)fieldValue;
                SliderComponent.gameObject.GetComponentInChildren<TMP_Text>().text = SliderComponent.value.ToString();

                settingsDisplayText.text = SliderConfiguration.SettingDisplayName;

                SliderComponent.onValueChanged.AddListener(value => OnSettingChanged(value));

                break;

            case FieldTypes.type_int:
                //Init the component
                ResolveConnectedSettingField(SliderConfiguration.ConnectedVariable);
                SliderComponent = settingsElement.GetComponentInChildren<Slider>(true);               
                

                //Configure the component and set the gui to the read out value. 
                SliderConfiguration.SliderType = SliderElementData.SliderTypes.IntSlider;
                SliderComponent.wholeNumbers = true;
                SliderComponent.minValue = SliderConfiguration.IntSliderCapMin;
                SliderComponent.maxValue = SliderConfiguration.IntSliderCapMax;
                SliderComponent.value = (int)fieldValue;
                SliderComponent.gameObject.GetComponentInChildren<TMP_Text>().text = SliderComponent.value.ToString();

                SliderComponent.onValueChanged.AddListener(value => OnSettingChanged((int)value));
                break;

            default:
                break;
        }
    }
    private void ConfigSliderEditor()
    {
        eType = SliderConfiguration.SliderType == SliderElementData.SliderTypes.FloatSlider ? FieldTypes.type_float : FieldTypes.type_int;
        settingsDisplayText.text = SliderConfiguration.SettingDisplayName;
    }


    private void HandleConditionalAndExtraElements<T>(T value)
    {
        switch (GUISettingElementType)
        {
            case GUISettingElementTypes.UniToggle:
                //Conditional Elements
                foreach (DrawOnUniToggleData DrawSetting in UniToggleConfiguration.ConditionalElements)
                {
                    switch (DrawSetting.DrawOnCase)
                    {
                        case DrawOnUniToggleData.DrawCases.IsOn:
                            if (UniToggleComponent.isOn) { DrawSetting.Element.SetActive(true); }
                            else { DrawSetting.Element.SetActive(false); }
                            break;
                        case DrawOnUniToggleData.DrawCases.IsOff:
                            if (UniToggleComponent.isOn) { DrawSetting.Element.SetActive(false); }
                            else { DrawSetting.Element.SetActive(true); }
                            break;
                    }
                }
                break;
            case GUISettingElementTypes.SelectorToggle:
                break;
            case GUISettingElementTypes.Slider:
                SliderComponent.gameObject.GetComponentInChildren<TMP_Text>().text = value.ToString();
                break;
            case GUISettingElementTypes.Dropdown:
                break;
            case GUISettingElementTypes.InputField:
                break;
            case GUISettingElementTypes.Button:
                break;
        }
    }

    private void OnSettingChanged<T>(T value) 
    { 
        Debug.Log($"{fieldInfo} set to {value}");
        HandleConditionalAndExtraElements(value);
        fieldInfo.SetValue(appSettingsManager._AppSettings, value); //Set the value in the settings class (Making it essentially ready to be saved to .json);
        appSettingsManager.RefreshSettingsFeatures(); //Directly update all the changes that should happen live. (E.g. changing the audio volume should affect the audio mixer instantly)
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
        settingsDisplayText = GetComponentInChildren<GUISettingsDisplayText>()?.gameObject?.GetComponentInChildren<TMP_Text>(true);
        
        if (settingsElement != null) 
        { 
            settingsElement.SetActive(true);
        }
        else
        {
            list.ForEach(obj => obj.gameObject.SetActive(false));
        }
    }
    private void ConfigureSettingDisplayComponents()
    {
        settingsDisplayText = GetComponentInChildren<GUISettingsDisplayText>().gameObject.GetComponent<TMP_Text>();

        if(hideSettingsDisplayText) { settingsDisplayText.gameObject.SetActive(false); }

    }
}

