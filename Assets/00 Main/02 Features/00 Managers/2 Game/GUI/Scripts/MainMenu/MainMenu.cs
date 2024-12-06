using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private GUIController GUIController;
    private GUISpringboard GUISpringboard;
    private void OnEnable()
    {
        GUIController = GetComponentInParent<GUIController>();
        GUISpringboard = GetComponentInChildren<GUISpringboard>();
        foreach(SelectableLevelInfo dat in GUIController._LevelData._Levels)
        {
            InstantiateLevelElements(dat);
        }
    }



    #region SPRINGBOARD HANDLING
    private void InstantiateLevelElements(SelectableLevelInfo dat)
    {
        if (dat._SceneName != null)
        {
            GameObject NewElement = Instantiate(GUIController._LevelData._SpringboardElementPrefab, GUISpringboard.transform);
            NewElement.name = dat._SceneDisplayName;

            //References of the element
            Button ElementButton;
            TMP_Text ElementTitle;
            RawImage ElementImage;
            RawImage ElementOverlay;
            RawImage ElementHoverOverlay;

            //Set up springboard element components

            //Button
            if (NewElement.GetComponentInChildren<GUISpringboardElement>().GetComponent<Button>() != null)
            {
                ElementButton = NewElement.GetComponentInChildren<GUISpringboardElement>().GetComponent<Button>();
                ElementButton.onClick.AddListener(() => GUIController.LoadLevel(dat._SceneName));
            }

            //Text
            if(NewElement.GetComponentInChildren<GUISpringboardElementTitle>().GetComponent<TMP_Text>() != null)
            {
                ElementTitle = NewElement.GetComponentInChildren<GUISpringboardElementTitle>().GetComponent<TMP_Text>();
                ElementTitle.text = dat._SceneDisplayName != null ? dat._SceneDisplayName : dat._SceneName;
            }

            //Main Image
            if(NewElement.GetComponentInChildren<GUISpringboardElementImage>().GetComponent<RawImage>() != null)
            {
                ElementImage = NewElement.GetComponentInChildren<GUISpringboardElementImage>().GetComponent<RawImage>();
                if (dat._SceneDisplayImage != null) { ElementImage.texture = dat._SceneDisplayImage; }
            }

            //Label Image Overlay
            if(NewElement.GetComponentInChildren<GUISpringboardElementImageOverlay>().GetComponent<RawImage>() != null)
            {
                ElementOverlay = NewElement.GetComponentInChildren<GUISpringboardElementImageOverlay>().GetComponent<RawImage>();
                if (dat._HasOverlayImage)
                {
                    if (dat._SceneOverlayImage != null) { ElementOverlay.texture = dat._SceneOverlayImage; } else { ElementOverlay.enabled = false; }
                }
                else { ElementOverlay.enabled = false; }
            }

            //Hover Image Overlay
            if (NewElement.GetComponentInChildren<GUISpringboardHoverOverlayImage>().GetComponent<RawImage>() != null)
            {
                ElementHoverOverlay = NewElement.GetComponentInChildren<GUISpringboardHoverOverlayImage>().GetComponent<RawImage>();
                ElementHoverOverlay.texture = GUIController._LevelData._OnHoverOverlayImage;
                ElementHoverOverlay.enabled = false;
            }                                   
        }
    }

    #endregion
}
