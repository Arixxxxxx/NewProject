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
    IClickLvUpAble clickLvUpAble;

    private void Awake()
    {
        clickLvUpAble = targetObj.GetComponent<IClickLvUpAble>();
    }


    private void Start()
    {
        Btn = GetComponent<Button>();
    }

    float startTime = 0.5f;
    float reBuyInterval = 0.1f;
    void Update()
    {
        if (isHolding && Btn.interactable)
        {
            staytime += Time.deltaTime;
            if (staytime >= startTime)
            {
                delay += Time.deltaTime;
                if (delay >= reBuyInterval)
                {
                    clickLvUpAble.ClickUp();
                    AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    delay = 0;
                }
            }
        }
    }

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

}
