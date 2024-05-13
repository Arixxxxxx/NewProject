using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reward_Parts : MonoBehaviour
{
    GameObject parentRef;
    Animator anim;
    Image itemIMG;
    TMP_Text itemInfoText;
    Action rewardAction;
    private void Awake()
    {
        init();
    }
    void Start()
    {
        
    }

    private void init()
    {
        parentRef = transform.parent.gameObject;
        anim = GetComponent<Animator>();
        itemIMG = transform.Find("itemSoket/IMG").GetComponent<Image>();
        itemInfoText = transform.Find("itemText").GetComponent<TMP_Text>();
    }

    public void Set_Reward(Sprite sprite, string text)
    {
        if(itemIMG == null) 
        {
            init();
        }

        itemIMG.sprite = sprite;
        itemInfoText.text = text;

        parentRef.SetActive(true);
        gameObject.SetActive(true);

    }


    public void Set_RewardIncludeAction(Sprite sprite, string text, Action funtion)
    {
        if (itemIMG == null)
        {
            init();
        }

        itemIMG.sprite = sprite;
        itemInfoText.text = text;

        parentRef.SetActive(true);
        gameObject.SetActive(true);

        rewardAction = null;
        rewardAction += funtion;
        rewardAction?.Invoke();
    }


    private void A_GameObjectActiveFalse()
    {
        WorldUI_Manager.inst.Return_WorldUIObjPoolingObj(gameObject, 0);
    }
}
