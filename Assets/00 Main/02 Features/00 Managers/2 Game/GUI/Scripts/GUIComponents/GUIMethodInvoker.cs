using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Viguar.Inspector.PropertyFields;

public class GUIMethodInvoker : MonoBehaviour
{
    //Attached to Buttons & Other Elements that need to invoke methods in the "master" GUIController.cs script.

    private GUIController guiController;

    private void Start()
    {
        guiController = GetComponentInParent<GUIController>();
    }

    public void OpenMenuPage(string pageName)
    {
        guiController.OpenMenu(pageName);
    }
}
