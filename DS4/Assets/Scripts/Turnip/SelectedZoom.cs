using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedZoom : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    float _ZoomScale = 1.15f;
    public void OnSelect(BaseEventData eventData)
    {
        this.transform.localScale = new Vector3(_ZoomScale, _ZoomScale, 1);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        this.transform.localScale = Vector3.one;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(_ZoomScale, _ZoomScale, 1);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = Vector3.one;
    }
}

