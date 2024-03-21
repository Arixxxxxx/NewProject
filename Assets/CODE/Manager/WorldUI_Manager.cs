using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class WorldUI_Manager : MonoBehaviour
{
    public static WorldUI_Manager inst;


    GameObject worldUI;
    Image[] stageSlot = new Image[5];
    Image uiBossHead;
    TMP_Text stageText;
    [SerializeField] Sprite[] stageSprite;
    Button[] testBtn;
    TMP_Text[] weapbtnText;
    Animator cuttonBlack;
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }

        worldUI = GameObject.Find("---[World UI Canvas]").gameObject;


        cuttonBlack = worldUI.transform.Find("Cutton(B)").GetComponent<Animator>();
        stageText = worldUI.transform.Find("StageUI/BG/Text").GetComponent<TMP_Text>();
        uiBossHead = worldUI.transform.Find("StageUI/BG/Boss").GetComponent<Image>();
        for (int index = 0; index < stageSlot.Length; index++)
        {
            stageSlot[index] = worldUI.transform.Find("StageUI/BG").GetChild(index).GetComponent<Image>();
        }

        //테스트 버튼
        testBtn = worldUI.transform.Find("TestBtn").GetComponentsInChildren<Button>();
        weapbtnText = new TMP_Text[testBtn.Length];
        for(int index=0; index < testBtn.Length; index++)
        {
            weapbtnText[index] = testBtn[index].GetComponentInChildren<TMP_Text>();
        }

    }
    void Start()
    {
        //테스트용 나중에 지워야함
        testBtnInit();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    /// <summary>
    /// 스테이지 UI바 색칠
    /// </summary>
    /// <param name="curFloorLv"></param>
    public void Set_StageUiBar(int curFloorLv)
    {
        stageSlot[curFloorLv].sprite = stageSprite[2];
        stageText.text = $"{GameStatus.inst.StageLv} - {curFloorLv+1} 층";
        uiBossHead.gameObject.SetActive(curFloorLv == 4 ? true : false);
        for (int index = 0; index < curFloorLv; index++)
        {
            stageSlot[index].sprite = stageSprite[1];
        }

    }


    /// <summary>
    /// 스테이지바 초기화 
    /// </summary>
    public void Reset_StageUiBar()
    {
        stageSlot[0].sprite = stageSprite[2];
        stageText.text = $"Stage {GameStatus.inst.StageLv} - {GameStatus.inst.FloorLv+1}";
        uiBossHead.gameObject.SetActive(false);

        for (int index = 1; index < stageSlot.Length; index++)
        {
            stageSlot[index].sprite = stageSprite[0];
        }
    }

    /// <summary>
    /// World공간 배경 UI 페이드 커튼
    /// </summary>
    /// <param name="Value"></param>
    public void Set_WorldBlackCottun(bool Value)
    {
       cuttonBlack.SetTrigger(Value == true ? "FadeOn" : "FadeOff");
    }

    int weaponNum;
   
    private void testBtnInit()
    {
        testBtn[0].onClick.AddListener(() => 
        {
            //weaponNum++;
            //weapbtnText[0].text = $"무기 교체 {weaponNum}번";
             ActionManager.inst.TestBtnWeaponChange(); 
        } );

        testBtn[1].onClick.AddListener(() => 
        {
            GameStatus.inst.AtkSpeedLv++;
            if(GameStatus.inst.AtkSpeedLv < 10)
            {
                weapbtnText[1].text = $"공격 속도 x {GameStatus.inst.AtkSpeedLv}";
            }
            else if(GameStatus.inst.AtkSpeedLv >= 10)
            {
                weapbtnText[1].text = $"만렙";
            }
        }
        );
        
        
    }

}
