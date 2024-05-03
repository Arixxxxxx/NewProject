using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEvent : MonoBehaviour, IPointerClickHandler
{

    void Start()
    {
        Invoke("Touch", 1.5f);
    }

    bool startClickDelay;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(startClickDelay == false) { return; }
        
        LoginManager.inst.NextStep();
        gameObject.SetActive(false);
    }

    private void Touch()
    {
        startClickDelay = true;
    }
}
