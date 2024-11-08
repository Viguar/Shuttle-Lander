using System;
using UnityEngine;
using Viguar.Inspector.PropertyFields;


[Serializable]
public class MenuPageInfo
{
    public enum MenuPageTypes { FullPage, ContextOverlayPage, }
     
    public string MenuPageName;
    public GameObject MenuPagePrefab;
    public MenuPageTypes MenuPageType;
    public bool DefaultMenuPage;
}
