using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // 리워드UI 관련
    GameObject rewardRef;
    Reward_Parts[] rewards;
    int rewardChildCount;
    [SerializeField]
    GameObject stageUI;

    //우편 수신함
    
    Button getLetterBtn;

    // 출석체크
    Button dailyPlayCheckBtn;
    // 뉴비버튼
    Button newBieBtn;

    // 메인메뉴 버튼
    Button mainMenuBtn;

    // 미니게임 버튼 
    Button minigameAlrimBtn;

    // 몬스터도감 버튼
    Button mosterDogamBtn;

    //환생버튼
    Button hwanSengBtn;

    // 광고제거 버튼
    Button adDeleteBtn;

    //레드심볼 관리
    List<GameObject> redSimBall_Icons = new List<GameObject>();

    // 삼각형 버튼
    Button openMenuIcon;
    Animator menuAnim;
    Transform checkArrowScaleX;

    // 프론트UI 블랙페이드 커튼
    Action cuttonAction;
    Image frontUiCutton;


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

        //리워드관련
        rewardRef = frontUICanvas.transform.Find("ReWard").gameObject;
        rewards = rewardRef.GetComponentsInChildren<Reward_Parts>(true);
        rewardChildCount = rewardRef.transform.childCount;


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



        questListBtn = worldUI.transform.Find("StageUI/QeustList/Button").GetComponent<Button>();
        mainMenuBtn = worldUI.transform.Find("StageUI/MainMenu").GetComponent<Button>();
        mainMenuBtn.onClick.AddListener(() => MainMenuManager.inst.Set_MainMenuActive(true));

        hwanSengBtn = worldUI.transform.Find("StageUI/HwanSeng").GetComponent<Button>();

        // 메뉴버튼들
        
        getLetterBtn = worldUI.transform.Find("StageUI/Letter").GetComponent<Button>(); // 우편함
        dailyPlayCheckBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/DailyCheck").GetComponent<Button>(); //출석체크
        newBieBtn = worldUI.transform.Find("StageUI/NewBie").GetComponent<Button>(); //뉴비
        mosterDogamBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/MosterDogam").GetComponent<Button>(); //몬스터도감
        minigameAlrimBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/MiniGame").GetComponent<Button>(); //미니게임
        adDeleteBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/AdDelete").GetComponent<Button>(); // 광고제거
        openMenuIcon = worldUI.transform.Find("StageUI/MenuBox/MeneOpen/RealBtn").GetComponent<Button>(); // 메뉴 삼각형버튼
        checkArrowScaleX = openMenuIcon.transform.parent.GetComponent<Transform>(); 

        menuAnim = worldUI.transform.Find("StageUI/MenuBox").GetComponent<Animator>(); // 메뉴바 연출 애니메ㅐ이션

        // FrontUI
        frontUiCutton = frontUICanvas.transform.Find("Cutton").GetComponent<Image>();

        Prefabs_Awake();
        
    }



    void Start()
    {
        //테스트용 나중에 지워야함
        BtnInIt();

        // 최초 소지재화들 초기화
        curMaterial[0].text = GameStatus.inst.Gold;
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

        //testBtn[0].onClick.AddListener(() =>
        //{
        //    GameStatus.inst.AtkSpeedLv++;
        //    if (GameStatus.inst.AtkSpeedLv < 10)
        //    {
        //        weapbtnText[0].text = $"공격 속도 x {GameStatus.inst.AtkSpeedLv}";
        //    }
        //    else if (GameStatus.inst.AtkSpeedLv >= 10)
        //    {
        //        weapbtnText[0].text = $"만렙";
        //    }
        //});


        // 월드 버튼 초기화

        hwanSengBtn.onClick.AddListener(() => HwanSengSystem.inst.Set_HwansengUIActive(true)); //환생버튼
        getLetterBtn.onClick.AddListener(() => { LetterManager.inst.OpenPostOnOfficeAndInit(true); });
        dailyPlayCheckBtn.onClick.AddListener(() => { DailyPlayCheckUIManager.inst.MainWindow_Acitve(true); });
        newBieBtn.onClick.AddListener(() => { Newbie_Content.inst.Set_NewbieWindowActive(true); });
        mosterDogamBtn.onClick.AddListener(() => { DogamManager.inst.Set_DogamListAcitve(1, true); });
        adDeleteBtn.onClick.AddListener(() => AdDelete.inst.ActiveAdDeleteWindow());
        minigameAlrimBtn.onClick.AddListener(() => 
        {
            MinigameManager.inst.Active_minigameEntrance(true);
        });

        openMenuIcon.onClick.AddListener(() => 
        {
          if(checkArrowScaleX.localScale.x == 1)
            {
                menuAnim.SetTrigger("Open");
            }
          else if(checkArrowScaleX.localScale.x == -1)
            {
                menuAnim.SetTrigger("Close");
            }
        });
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

    Coroutine cuttonCor;
    Color colorFadeValue = new Color(0, 0, 0, 0.2f);
    float fadeSpeed = 20;
    /// <summary>
    /// 프론트UI 페이드인 함수실행
    /// </summary>
    /// <param name="funtion"></param>
    public void FrontUICuttonAction(Action funtion)
    {
        if(cuttonCor != null)
        {
            StopCoroutine(cuttonCor);
        }

        cuttonCor = StartCoroutine(PlayCutton(funtion));
    }
    IEnumerator PlayCutton(Action funtion)
    {
        if(frontUiCutton.gameObject.activeSelf == false)
        {
            frontUiCutton.gameObject.SetActive(true);
        }

        while(frontUiCutton.color.a < 1)
        {
            frontUiCutton.color += colorFadeValue * Time.deltaTime * fadeSpeed;
            yield return null;  
        }
        cuttonAction = null;
        cuttonAction += funtion;
        cuttonAction?.Invoke();
        
        while (frontUiCutton.color.a > 0)
        {
            frontUiCutton.color -= colorFadeValue * Time.deltaTime * fadeSpeed;
            yield return null;
        }

        if (frontUiCutton.gameObject.activeSelf == true)
        {
            frontUiCutton.gameObject.SetActive(false);
        }
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

    int curIndex = 0;
    /// <summary>
    /// 월드 중앙 상단 Reward창 호출
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="text"> 아이템 설명내용</param>
   public void Set_RewardUI_Invoke(Sprite sprite, string text)
    {
        curIndex = (int)Mathf.Repeat(curIndex, rewardChildCount);
        rewards[curIndex].Set_Reward(sprite, text);
        curIndex++;
    }

    /// <summary>
    /// 월드 중앙 상단 Reward + Action 포함 호출
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="text"> 아이템 설명내용</param>
    public void Set_Reward_InclueAction(Sprite sprite, string text,Action funtion)
    {
        curIndex = (int)Mathf.Repeat(curIndex, rewardChildCount);
        rewards[curIndex].Set_RewardIncludeAction(sprite, text, funtion);
        curIndex++;
    }


    /// <summary>
    /// 버프 선택창 호출
    /// </summary>
    /// <param name="value"> true / false </param>
    public void buffSelectUIWindowAcitve(bool value) => buffSelectUIWindow.SetActive(value);

    public void NewbieBtnAcitveFalse() => newBieBtn.gameObject.SetActive(false);



}
