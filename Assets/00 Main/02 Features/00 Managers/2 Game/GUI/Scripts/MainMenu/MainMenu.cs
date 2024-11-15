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

    private void InstantiateLevelElements(SelectableLevelInfo dat)
    {
        if (dat._SceneName != null)
        {
            GameObject NewElement = Instantiate(GUIController._LevelData._SpringboardElementPrefab, GUISpringboard.transform);
            NewElement.name = dat._SceneDisplayName;

            Button ElementButton = NewElement.GetComponentInChildren<GUISpringboardElement>().GetComponent<Button>();
            TMP_Text ElementTitle = NewElement.GetComponentInChildren<GUISpringboardElementTitle>().GetComponent<TMP_Text>();
            RawImage ElementImage = NewElement.GetComponentInChildren<GUISpringboardElementImage>().GetComponent<RawImage>();
            RawImage ElementOverlay = NewElement.GetComponentInChildren<GUISpringboardElementImageOverlay>().GetComponent<RawImage>();
            

            ElementTitle.text = dat._SceneDisplayName != null ? dat._SceneDisplayName : dat._SceneName;
            if(dat._SceneDisplayImage != null) { ElementImage.texture = dat._SceneDisplayImage; }
            if(dat._HasOverlayImage) 
            {
                if (dat._SceneOverlayImage != null) { ElementOverlay.texture = dat._SceneOverlayImage; }
                else { ElementOverlay.gameObject.SetActive(false); }

            }
            ElementButton.onClick.AddListener(() => GUIController.LoadLevel(dat._SceneName));
        }
    }
}
