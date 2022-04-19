using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UiClickHandler : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onRightClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerId == -2) { onRightClick.Invoke(); }  // -2 is right click  (-1 left and -3 middle )
    }
}
