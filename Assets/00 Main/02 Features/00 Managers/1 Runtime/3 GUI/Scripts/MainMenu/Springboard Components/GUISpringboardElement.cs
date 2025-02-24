using UnityEngine;
using UnityEngine.UI;

public class GUISpringboardElement : MonoBehaviour
{
    private RawImage _HoverOverlayImage;

    private void OnEnable()
    {
        if(GetComponentInChildren<GUISpringboardElementImageOverlay>() != null)
        {
            _HoverOverlayImage = GetComponentInChildren<GUISpringboardElementImageOverlay>().GetComponent<RawImage>();
        }
    }





    //Overlay Image Handling
    public void EnableHoverOverlayImage()
    {
        if(_HoverOverlayImage != null) 
        {
            _HoverOverlayImage.enabled = true;   
        }
    }
    public void DisableHoverOverlayImage()
    {
        if (_HoverOverlayImage != null)
        {
            _HoverOverlayImage.enabled = false;
        }
    }
}
