using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEvent : MonoBehaviour, IPointerClickHandler
{

    void Start()
    {

    }

    bool once;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (once == false)
        {
            once = true;
            LoginManager.inst.NextStep();
        }
    }
}
