using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventRewardContent : MonoBehaviour
{
    public static WorldEventRewardContent inst;


    // Ref
    GameObject worldUiRef, frontUIRef, eventBoxRef;
    Vector3 startPos, endPos;
    FlyEventPrefabs boxSc;
    [Header("# Check Box Spawn Time ★ ( Read Only )")]
    [Space]
    [SerializeField] float reSpawnTimer;
    [Header("# Setup Respawn Range <Color=yellow>(ToolTip include)</color>")]
    [Tooltip("BoxPopupTimeRange for GameStart\nGameStart X = RangeMin Second Sec \nY = RangeMin Second")]
    [Space]
    [SerializeField] Vector2 startTimeRange;
    [Tooltip("BoxPopupTimeRange for Runtime\nGameStart X = RangeMin Second Sec \nY = RangeMin Second")]
    [Space]
    [SerializeField] Vector2 clickTimeRange;

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
        boxSc = eventBoxRef.GetComponent<FlyEventPrefabs>();
        reSpawnTimer = UnityEngine.Random.Range(startTimeRange.x, startTimeRange.y);

    }
    void Start()
    {

    }
    void Update()
    {
        // 맵벗어날시
        BoxPostionCheker();

        // 리스폰
        reSpawnCheker();
    }

    /// <summary>
    /// 0공격력증가, 1이속증가, 2골드획득증가, 3루비100개, 4 골드10배
    /// </summary>
    /// <param name="type"></param>
    public void EventActive(int type)
    {
        switch (type)
        {
            // 즉시 버프 1분
            case 0:
                BuffManager.inst.ActiveBuff(0, 1, "공격력증가 버프 1분");
                break;

            case 1:
                BuffManager.inst.ActiveBuff(1, 1, "이속증가 버프 1분");
                break;

            case 2:
                BuffManager.inst.ActiveBuff(2, 1, "골드획득증가 버프 1분");
                break;

            //루비 (광고시청후)
            case 3:
                //물어보는창 오픈
                ADViewManager.inst.ActiveQuestionWindow(true, 1, 0, "루비 +100", () => {
                    //광고보고
                    ADViewManager.inst.AdMob_ActiveAndFuntion(() =>
                    {
                        GameStatus.inst.PlusRuby(100); //실행
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " 루비 +100");
                    });
                });
                break;

            //루비 (광고 시청후)
            case 4:

                string curgold = GameStatus.inst.CurrentGold();
                curgold = CalCulator.inst.StringAndIntMultiPly(curgold, 10); // 10배

                //물어보는창 오픈
                ADViewManager.inst.ActiveQuestionWindow(true, 1, 1, $"골드 +{CalCulator.inst.StringFourDigitAddFloatChanger(curgold)}", () => {

                    //광고보고
                    ADViewManager.inst.AdMob_ActiveAndFuntion(() =>
                    {
                        GameStatus.inst.PlusGold(curgold);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(1), $"골드 +{CalCulator.inst.StringFourDigitAddFloatChanger(curgold)}");
                    });
                });
                break;
        }

        eventBoxReset();

    }

    private void BoxPostionCheker()
    {
        if (eventBoxRef.transform.position.x > endPos.x && eventBoxRef.activeSelf)
        {
            eventBoxReset();
        }
    }

    private void reSpawnCheker()
    {
        if(reSpawnTimer > 0 && eventBoxRef.activeSelf == false)
        {
            reSpawnTimer -= Time.deltaTime;
        }
        else if(reSpawnTimer <= 0 && eventBoxRef.activeSelf == false)
        {
            reSpawnTimer = 0;
            eventBoxRef.SetActive(true);
        }
    }

    /// <summary>
    /// 박스 위치 초기화 및 엑티브False
    /// </summary>
    public void eventBoxReset()
    {
        reSpawnTimer += UnityEngine.Random.Range(clickTimeRange.x, clickTimeRange.y);
        eventBoxRef.gameObject.SetActive(false);
        boxSc.ResetVelocity(); // 속도값 초기화
        eventBoxRef.transform.localPosition = startPos; // 위치초기화
        
        //시간넣어줌 시간다되면 켜줌
    }

   
}
