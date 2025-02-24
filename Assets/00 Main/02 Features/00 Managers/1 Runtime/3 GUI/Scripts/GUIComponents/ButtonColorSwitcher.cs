using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonColorSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool KeepOtherSelectedButtonsColored;
    private GUIController guiController;
    [HideInInspector] public bool currentlySelected;
    
    private void Start()
    {
        guiController = GetComponentInParent<GUIController>();
    }

    public void OnSelected()
    {        
        if (!KeepOtherSelectedButtonsColored)
        {         
        ButtonColorSwitcher[] buttonColorSwitchers = FindObjectsByType<ButtonColorSwitcher>(FindObjectsSortMode.None);        
        foreach (ButtonColorSwitcher buttonColorSwitcher in buttonColorSwitchers) 
        {
                buttonColorSwitcher.ChangeButtonColor(guiController._MenuSetup.DefaultButtonColor);
                buttonColorSwitcher.currentlySelected = false;
        }
        }
        ChangeButtonColor(guiController._MenuSetup.CurrentlySelectedButtonColor);
        currentlySelected = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeButtonColor(guiController._MenuSetup.CurrentlySelectedButtonColor);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!currentlySelected) { ChangeButtonColor(guiController._MenuSetup.DefaultButtonColor); }        
    }




    private void ChangeButtonColor(Color color)
    {
        if(GetComponent<RawImage>() != null) { GetComponent<RawImage>().color = color; }
        else if (GetComponent<Image>() != null) { GetComponent<Image>().color = color; }
        else if (GetComponent<TMP_Text>() != null) { GetComponent<TMP_Text>().color = color; }
    }
}
