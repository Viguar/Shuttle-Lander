using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Inspector.PropertyFields;
using UnityEngine.SceneManagement;

public class GUIController : MonoBehaviour
{
    //Main GUI Controller for the Main Menu
    public enum MenuTypes { MainMenu, PauseMenu, }
    
    public MenuTypes _MenuType;
    [SerializeField, ReadOnly] private GameObject _CurrentMenu;

    public GUIMenuData _MenuSetup;
    public GUILevelData _LevelData;

    [Space(10)]
    public GameObject _MainContentArea;
   
    private void Start() //Called once ever only.
    {
        
    }

    private void OnEnable() //Called every time it's enabled.
    {
        var onEnableMenu = _MenuSetup._MenuPages.Find(data => data.DefaultMenuPage == true);
        if (onEnableMenu != null) { _CurrentMenu = Instantiate(onEnableMenu.MenuPagePrefab, _MainContentArea.transform); }
    }
    public void OpenMenu(string RequestedMenu) //Attempt to open the requested menu under conditions.
    {    
        //First, try to find the correct menu to open.
        var reqMenu = _MenuSetup._MenuPages.Find(data => data.MenuPageName == RequestedMenu);
        if(reqMenu != null)
        {
            //Evaluate how to open the menu.
            switch (reqMenu.MenuPageType) 
            {
                //Handle it as a new menu: Destroy old, Instantiate new.
                case MenuPageInfo.MenuPageTypes.FullPage: 
                    if (_CurrentMenu != null) { Destroy(_CurrentMenu); }
                    //_CurrentMenu = null;
                    _CurrentMenu = Instantiate(reqMenu.MenuPagePrefab, _MainContentArea.transform);
                    break;

                //Handle it as a context menu/keep current.
                case MenuPageInfo.MenuPageTypes.ContextOverlayPage: 
                    Instantiate(reqMenu.MenuPagePrefab, _CurrentMenu.transform);
                    break;            
            }
        }
    }
    public void LoadLevel(string SceneName)
    {
        if(!string.IsNullOrEmpty(SceneName))
        {
            SceneManager.LoadScene(SceneName);
        }
    }

}
