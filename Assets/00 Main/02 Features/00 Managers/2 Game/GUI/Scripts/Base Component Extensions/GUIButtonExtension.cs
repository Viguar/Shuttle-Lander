using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GUIButtonExtension : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool InvokeOnHover;
    public UnityEvent OnHover;

    [SerializeField] private bool InvokeOnHoverExit;
    public UnityEvent OnHoverExit;


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (InvokeOnHover)
        {
            OnHover.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (InvokeOnHoverExit)
        {
            OnHoverExit.Invoke();
        }
    }
}
