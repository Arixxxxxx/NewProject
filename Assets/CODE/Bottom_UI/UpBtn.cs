using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject targetObj;
    float staytime;
    bool isHolding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        staytime = 0;
    }

    void Update()
    {
        if (isHolding)
        {
            staytime += Time.deltaTime;
            if (staytime >= 1)
            {
                targetObj.GetComponent<IClickLvUpAble>().ClickUp();
            }
        }
    }
}
