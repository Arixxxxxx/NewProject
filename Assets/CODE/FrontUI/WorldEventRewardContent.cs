using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldEventRewardContent : MonoBehaviour
{
    public static WorldEventRewardContent inst;
    

    // Ref
    GameObject worldUiRef, frontUIRef, eventBoxRef;
    Vector3 startPos, endPos;





    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        worldUiRef = GameManager.inst.WorldSpaceRef;
        frontUIRef = GameManager.inst.FrontUiRef;
        eventBoxRef = worldUiRef.transform.Find("EventPresent/ClickPresent").gameObject;
        startPos = worldUiRef.transform.Find("EventPresent/Trs/Start").transform.localPosition;
        endPos = worldUiRef.transform.Find("EventPresent/Trs/End").transform.localPosition;

  
    }
    void Start()
    {

    }
    void Update()
    {
        // 시간
        // 초기화
        // 팝업
    }

    public void EventActive(int type)
    {
        switch (type)
        {
            // 즉시 버프 3분
            case 0:
                BuffManager.inst.ActiveBuff(type, 3, "공격력x5 버프 3분");
                break;

            case 1:
                BuffManager.inst.ActiveBuff(type, 3, "이속증가 버프 3분");
                break;

            case 2:
                BuffManager.inst.ActiveBuff(type, 3, "골드획득증가 버프 3분");
                break;

            //루비 (광고시청후)
            case 3:
                //물어보는창 오픈
                ADViewManager.inst.ActiveQuestionWindow(true, 1, 0, "루비 +100", () => {
                    //광고보고
                    ADViewManager.inst.SampleAD_Active_Funtion(() =>
                    {
                        GameStatus.inst.Ruby += 100; //실행
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " 루비 +100");
                    });
                });
                break;

            //루비 (광고 시청후)
            case 4:

                string curgold = ActionManager.inst.Get_EnemyDeadGold();
                
                //물어보는창 오픈
                ADViewManager.inst.ActiveQuestionWindow(true, 1, 1, $"골드 +{curgold}", () => {
                    //광고보고
                    ADViewManager.inst.SampleAD_Active_Funtion(() =>
                    {
                        GameStatus.inst.PlusGold(curgold);
                        curgold = CalCulator.inst.StringFourDigitAddFloatChanger(curgold);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(1), $"골드 +{curgold}");
                    });
                });
                break;
        }

        //eventBoxReset();

    }


    /// <summary>
    /// 박스 위치 초기화 및 엑티브False
    /// </summary>
    public void eventBoxReset()
    {
        eventBoxRef.gameObject.SetActive(false);
        eventBoxRef.transform.localPosition = startPos;
    }
}
