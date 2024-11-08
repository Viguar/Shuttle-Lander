using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Inspector.PropertyFields;
using System;
using System.Linq;
using TMPro;

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

    private GameObject settingsElement; //The matching configurable gameobject for the setting. (Slider, Toggle, etc.)
    private TMP_Text settingsDisplayText;

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

