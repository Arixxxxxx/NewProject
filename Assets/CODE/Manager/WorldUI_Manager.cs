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

    [SerializeField] Sprite[] stageSprite;
    [SerializeField] GameObject getGoldAndStar_Text;
    Queue<GameObject> getGoldAndStar_TextQue = new Queue<GameObject>();
    Transform fontDanymic;
    Transform[] fontPoint = new Transform[2]; // 풀링오브젝트 스타트포인트 초기화용

    GameObject worldUI;
    Image[] stageSlot = new Image[5];
    Image uiBossHead;
    TMP_Text stageText;

    Button[] testBtn;
    TMP_Text[] weapbtnText;
    Animator cuttonBlack;

    // 하단 자원현황바 텍스트
    TMP_Text[] curMaterial = new TMP_Text[4];

    //퀘스트 목록 관련
    Button questListBtn;
    TMP_Text questListSideText;
    GameObject frontUICanvas;
    GameObject buffSelectUIWindow;

    //텍스트알림
    Animator textAlrim;
    TMP_Text alrimText;

    //샘플 광고테스트
    GameObject adSample;
    Button adXbtn;

    //우편 수신함
    Button getLetterBtn;
    // 출석체크
    Button dailyPlayCheckBtn;
    // 뉴비버튼
    Button newBieBtn;

    //레드심볼 관리
    List<GameObject> redSimBall_Icons = new List<GameObject>();

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
        frontUICanvas = GameObject.Find("---[FrontUICanvas]").gameObject;

        //샘플광고
        adSample = frontUICanvas.transform.Find("SampleAD").gameObject;
        adXbtn = adSample.transform.Find("X").GetComponent<Button>();

        //텍스트 알림
        textAlrim = worldUI.transform.Find("TextAlrim").GetComponent<Animator>();
        alrimText = textAlrim.GetComponentInChildren<TMP_Text>();

        //버프창
        buffSelectUIWindow = frontUICanvas.transform.Find("Buff_Window").gameObject;

        cuttonBlack = worldUI.transform.Find("Cutton(B)").GetComponent<Animator>();
        stageText = worldUI.transform.Find("StageUI/StageInfo/Text").GetComponent<TMP_Text>();
        uiBossHead = worldUI.transform.Find("StageUI/StageInfo/Boss").GetComponent<Image>();
        fontDanymic = worldUI.transform.Find("StageUI/Dyanamic").GetComponent<Transform>();

        for (int index = 0; index < stageSlot.Length; index++)
        {
            stageSlot[index] = worldUI.transform.Find("StageUI/StageInfo").GetChild(index).GetComponent<Image>();
        }

        //테스트 버튼
        testBtn = worldUI.transform.Find("TestBtn").GetComponentsInChildren<Button>();
        weapbtnText = new TMP_Text[testBtn.Length];
        for (int index = 0; index < testBtn.Length; index++)
        {
            weapbtnText[index] = testBtn[index].GetComponentInChildren<TMP_Text>();
        }
        curMaterial[0] = worldUI.transform.Find("StageUI/Bottom/Gold/UI_Text").GetComponent<TMP_Text>();
        curMaterial[1] = worldUI.transform.Find("StageUI/Bottom/Star/UI_Text").GetComponent<TMP_Text>();
        curMaterial[2] = worldUI.transform.Find("StageUI/Bottom/Key/UI_Text").GetComponent<TMP_Text>();
        curMaterial[3] = worldUI.transform.Find("StageUI/Bottom/Ruby/UI_Text").GetComponent<TMP_Text>();



        questListBtn = worldUI.transform.Find("StageUI/Right/QeustList/Button").GetComponent<Button>();


        // 게임화면 우측상단 버튼들
        getLetterBtn = worldUI.transform.Find("StageUI/Right/0_Line/Letter").GetComponent<Button>(); // 우편함
        dailyPlayCheckBtn = worldUI.transform.Find("StageUI/Right/0_Line/DailyCheck").GetComponent<Button>(); //출석체크
        newBieBtn = worldUI.transform.Find("StageUI/Right/1_Line/NewBie").GetComponent<Button>(); //출석체크

        Prefabs_Awake();
        redSimballI_Icons_List_Init();
    }


    // 아이콘 위에 빨간색구슬 초기화
    private void redSimballI_Icons_List_Init()
    {
        GameObject simballRef = worldUI.transform.Find("StageUI/Right/Simballs").gameObject;
        int count = simballRef.transform.childCount;

        for (int index = 0; index < count; index++)
        {
            redSimBall_Icons.Add(simballRef.transform.GetChild(index).gameObject);
        }

    }
    void Start()
    {
        //테스트용 나중에 지워야함
        BtnInIt();

        // 최초 소지재화들 초기화
        curMaterial[0].text = GameStatus.inst.PulsGold;
        curMaterial[1].text = GameStatus.inst.Star;
        curMaterial[2].text = GameStatus.inst.Key;
        curMaterial[3].text = GameStatus.inst.Ruby.ToString();
    }


    /// <summary>
    /// 스테이지 UI바 색칠
    /// </summary>
    /// <param name="curFloorLv"></param>
    public void Set_StageUiBar(int curFloorLv)
    {
        stageSlot[curFloorLv].sprite = stageSprite[2];
        stageText.text = $"Stage {GameStatus.inst.StageLv} - {GameStatus.inst.FloorLv}";
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
        stageText.text = $"Stage {GameStatus.inst.StageLv} - {GameStatus.inst.FloorLv}";
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
    public void Set_Menual_WorldBlackCottun(bool Value)
    {
        cuttonBlack.SetTrigger(Value == true ? "FadeOn" : "FadeOff");
    }

    /// <summary>
    /// 게임화면 페이드 인아웃 함수 
    /// </summary>
    /// <param name="durationTime">지속되는 시간</param>
    public void Set_Auto_BlackCutton(float durationTime)
    {
        StopCoroutine(StartCutton(durationTime));
        StartCoroutine(StartCutton(durationTime));
    }
    IEnumerator StartCutton(float durationTime)
    {
        cuttonBlack.SetTrigger("FadeOn");
        yield return new WaitForSeconds(durationTime);
        cuttonBlack.SetTrigger("FadeOff");

    }

    int weaponNum;

    private void BtnInIt()
    {
        testBtn[0].onClick.AddListener(() =>
        {
            //weaponNum++;
            //weapbtnText[0].text = $"무기 교체 {weaponNum}번";
            ActionManager.inst.TestBtnWeaponChange();
        });

        testBtn[1].onClick.AddListener(() =>
        {
            GameStatus.inst.AtkSpeedLv++;
            if (GameStatus.inst.AtkSpeedLv < 10)
            {
                weapbtnText[1].text = $"공격 속도 x {GameStatus.inst.AtkSpeedLv}";
            }
            else if (GameStatus.inst.AtkSpeedLv >= 10)
            {
                weapbtnText[1].text = $"만렙";
            }
        });

        questListBtn.onClick.AddListener(() => { QuestListWindow.inst.F_QuestList_ActiveWindow(0); });
        getLetterBtn.onClick.AddListener(() => { LetterManager.inst.OpenPostOnOfficeAndInit(true); });
        dailyPlayCheckBtn.onClick.AddListener(() => { DailyPlayCheckUIManager.inst.MainWindow_Acitve(true); });
        newBieBtn.onClick.AddListener(() => { Newbie_Content.inst.Set_NewbieWindowActive(true); });
    }

    private void Prefabs_Awake()
    {
        int count = 10;

        fontPoint[0] = worldUI.transform.Find("StageUI/Dyanamic/0").GetComponent<Transform>();
        fontPoint[1] = worldUI.transform.Find("StageUI/Dyanamic/1").GetComponent<Transform>();

        for (int index = 0; index < count; index++)
        {
            GameObject obj = Instantiate(getGoldAndStar_Text, fontDanymic);
            getGoldAndStar_TextQue.Enqueue(obj);
            obj.transform.position = fontDanymic.transform.position;
            obj.SetActive(false);
        }
    }
    /// <summary>
    /// 화면 자원바 획득한 자원량 숫자올라가는 연출 ( Gold = 0 / Star = 1 )
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public void Get_Increase_GetGoldAndStar_Font(int index, string textvalue)
    {
        if (getGoldAndStar_TextQue.Count <= 1)
        {
            GameObject obj = Instantiate(getGoldAndStar_Text, fontDanymic);
            getGoldAndStar_TextQue.Enqueue(obj);
            obj.transform.position = fontDanymic.transform.position;
            obj.SetActive(false);
        }


        GameObject objs = getGoldAndStar_TextQue.Dequeue();
        objs.transform.localPosition = fontPoint[index].localPosition;
        objs.GetComponent<UI_IncreaseValueFont>().Set_PosAndColorInit(index, textvalue);
        objs.gameObject.SetActive(true);
    }

    public void Return_GoldAndStarFontPrefabs(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }

        getGoldAndStar_TextQue.Enqueue(obj);

    }


    Color orijinColor;
    /// <summary>
    /// 텍스트 알림창 호출
    /// </summary>
    /// <param name="data">알림창에 띄울 메세지</param>
    public void Set_TextAlrim(string data)
    {
        orijinColor = alrimText.color;

        alrimText.text = data;
        StopCoroutine(TextalrimStart());
        StartCoroutine(TextalrimStart());
    }

    IEnumerator TextalrimStart()
    {
        textAlrim.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        textAlrim.SetTrigger("Off");
        yield return new WaitForSecondsRealtime(1.5f);
        textAlrim.gameObject.SetActive(false);
        alrimText.color = orijinColor;
    }



    /// <summary>
    /// 광고보고 버프 활성화시켜주는 함수
    /// </summary>
    /// <param name="witch"> buff ~~</param>
    /// <param name="value">0 ~ 2 버프선택 창에있는 버프들 / 3 = 클릭하는 화면버프 </param>
    public void SampleADBuff(string witch, int value)
    {
        adXbtn.onClick.RemoveAllListeners();
        adXbtn.onClick.AddListener(() =>
        {
            if (witch == "buff" && value != 3)
            {
                BuffContoller.inst.ActiveBuff(value, BuffManager.inst.AdbuffTime(value)); //버프활성화
                BuffManager.inst.AddBuffCoolTime(value, (int)BuffManager.inst.AdbuffTime(value)); // 쿨타임 시간추가
                Set_TextAlrim(BuffManager.inst.MakeAlrimMSG(value, (int)BuffManager.inst.AdbuffTime(value))); // 알림띄우기

            }
            else if (value == 3) // 클릭하는 화면 버프
            {
                BuffContoller.inst.ActiveBuff(value, BuffManager.inst.AdbuffTime(value)); //버프활성화
                Set_TextAlrim(BuffManager.inst.MakeAlrimMSG(0, (int)BuffManager.inst.AdbuffTime(value))); // 알림띄우기
            }
            adXbtn.gameObject.SetActive(false);
            adSample.SetActive(false);
            buffSelectUIWindow.SetActive(false);
        });

        StopCoroutine(PlayAD());
        StartCoroutine(PlayAD());
    }

    /// <summary>
    /// 광고보고 루비 혹은 돈
    /// </summary>
    /// <param name="itemType"> 0 루비 / 1 골드</param>
    /// <param name="value"> 값 </param>
    public void SampleAD_Get_Currency(int itemType, int value)
    {
        adXbtn.onClick.RemoveAllListeners();
        adXbtn.onClick.AddListener(() =>
        {
            switch (itemType)
            {
                case 0:
                    GameStatus.inst.Ruby += value;
                    break;

                case 1:
                    GameStatus.inst.TakeGold($"{value}");
                    break;
            }

            adXbtn.gameObject.SetActive(false);
            adSample.SetActive(false);
            buffSelectUIWindow.SetActive(false);
        });

        StopCoroutine(PlayAD());
        StartCoroutine(PlayAD());
    }
    IEnumerator PlayAD()
    {
        adSample.SetActive(true);
        yield return new WaitForSeconds(3);
        adXbtn.gameObject.SetActive(true);
    }

    /// <summary>
    /// 빨간색 심볼 켜주고 꺼주고
    /// </summary>
    /// <param name="index"> 0= 우편수신함</param>
    /// <param name="value"> 키고 / 끄기 </param>
    public void OnEnableRedSimball(int index, bool value)
    {
        redSimBall_Icons[index].SetActive(value);
    }


    /// <summary>
    /// 월드UI 자원바 업데이트 함수 
    /// </summary>
    /// <param name="index"> 0골드 / 1별 / 2키 / 3루비</param>
    /// <param name="EA"> 현재 자원량 </param>
    public void CurMaterialUpdate(int index, string EA)
    {
        if (index != 3) // 루비가 아니면 (골드,별,키)
        {
            curMaterial[index].text = CalCulator.inst.StringFourDigitAddFloatChanger(EA);
        }
        else if (index == 3)
        {
            curMaterial[index].text = EA;
        }
    }


    /// <summary>
    /// 버프 선택창 호출
    /// </summary>
    /// <param name="value"> true / false </param>
    public void buffSelectUIWindowAcitve(bool value) => buffSelectUIWindow.SetActive(value);

    public void NewbieBtnAcitveFalse() => newBieBtn.gameObject.SetActive(false);



}
