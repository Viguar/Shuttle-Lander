using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Menu Data", menuName = "Viguar/GUI/Menu Data", order = 0)]
public class GUIMenuData : ScriptableObject
{
    public Color DefaultButtonColor;
    public Color CurrentlySelectedButtonColor;
    public List<MenuPageInfo> _MenuPages = new List<MenuPageInfo>();
}