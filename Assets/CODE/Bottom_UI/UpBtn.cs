using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject targetObj;
    Button Btn;
    float staytime;
    float delay;
    bool isHolding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        staytime = 0;
        delay = 0;
    }

    private void Start()
    {
        Btn = GetComponent<Button>();
    }

    void Update()
    {
        if (isHolding && Btn.interactable)
        {
            staytime += Time.deltaTime;
            if (staytime >= 0.5f)
            {
                delay += Time.deltaTime;
                if (delay >= 0.1f)
                {
                    targetObj.GetComponent<IClickLvUpAble>().ClickUp();
                    AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    delay = 0;
                }
            }
        }
    }
}
