using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorSwitcher : MonoBehaviour
{
    [SerializeField] private bool KeepOtherSelectedButtonsColored;

    public void OnSelected()
    {
        GUIController guiController = GetComponentInParent<GUIController>();
        if (!KeepOtherSelectedButtonsColored)
        {         
        ButtonColorSwitcher[] buttonColorSwitchers = FindObjectsByType<ButtonColorSwitcher>(FindObjectsSortMode.None);        
        foreach (ButtonColorSwitcher buttonColorSwitcher in buttonColorSwitchers) 
        {
                buttonColorSwitcher.ChangeButtonColor(guiController._MenuSetup.DefaultButtonColor);    
        }
        }
        ChangeButtonColor(guiController._MenuSetup.CurrentlySelectedButtonColor);
        
    }

    private void ChangeButtonColor(Color color)
    {
        if(GetComponent<RawImage>() != null) { GetComponent<RawImage>().color = color; }
        else if (GetComponent<Image>() != null) { GetComponent<Image>().color = color; }
    }

}
