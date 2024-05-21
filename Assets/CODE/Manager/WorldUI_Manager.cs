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
    [Header("# Reward 및 금화획득 풀링 ")]
    [SerializeField] GameObject[] poolingObj;
    UI_IncreaseValueFont[] minusMaterialFont;
    Queue<GameObject>[] poolingQue;

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

    //// 몬스터도감 버튼  ( 무기도감과 합병 24.05.16 )
    //Button mosterDogamBtn;

    //환생버튼
    Button hwanSengBtn;

    // 광고제거 버튼
    Button adDeleteBtn;

    // 동료 살펴보기 버튼
    Button crewViewrBtn;

    // 무기도감버튼
    Button weaponShopBtn;

    // 이벤트샵 버튼
    Button eventShopBtn;

    // 빙고게임 버튼
    Button bingoBtn;

    // 버프샵
    Button buffShopBtn;
    Animator buffWindowAnim;

    //레드심볼 관리
    List<GameObject> redSimBall_Icons = new List<GameObject>();

    // 삼각형 버튼
    Button openMenuIcon;
    Animator menuAnim;
    Transform checkArrowScaleX;

    // 프론트UI 블랙페이드 커튼
    Action cuttonAction;
    Image frontUiCutton;

    // 상단바
    TMP_Text nickNameText;

    Image fakeScreen;

    // 로우이미지 관리자
    GameObject rawCam;
    GameObject[] rawImageRef;

    // 이펙트 커튼(하얀색)
    Image whiteCutton;

    //아이템획득 애니메이션
    VerticalLayoutGroup getItemLayOut;
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

        worldUI = GameManager.inst.WorldUiRef;
        
        frontUICanvas = GameManager.inst.FrontUiRef;

        //로우이미지
        rawCam = GameManager.inst.CamsRef.transform.Find("RenderCam").gameObject;
        int rawIMGCount = GameManager.inst.RawImgRef.transform.childCount;
        rawImageRef = new GameObject[rawIMGCount];
        for(int index=0; index<rawIMGCount; index++)
        {
            rawImageRef[index] = GameManager.inst.RawImgRef.transform.GetChild(index).gameObject; 
        }

        //로드시 페이크 스크린
        fakeScreen = frontUICanvas.transform.Find("FakeScreen").GetComponent<Image>();

        //혹시 카메라 빠지면 다시 채워줌
        worldUI.GetComponent<Canvas>().worldCamera = Camera.main;
        GameManager.inst.UiCanvasRef.GetComponent<Canvas>().worldCamera = Camera.main;
        frontUICanvas.GetComponent<Canvas>().worldCamera = Camera.main;



        //리워드관련
        rewardRef = frontUICanvas.transform.Find("ReWard").gameObject;
        rewards = rewardRef.GetComponentsInChildren<Reward_Parts>(true);
        rewardChildCount = rewardRef.transform.childCount;


        //버프창
        buffSelectUIWindow = frontUICanvas.transform.Find("Buff_Window").gameObject;
        buffWindowAnim = buffSelectUIWindow.GetComponent<Animator>();

        cuttonBlack = worldUI.transform.Find("Cutton(B)").GetComponent<Animator>();
        stageText = worldUI.transform.Find("StageUI/TopLayOut/StageInfo/Text").GetComponent<TMP_Text>();
        uiBossHead = worldUI.transform.Find("StageUI/TopLayOut/StageInfo/Boss").GetComponent<Image>();
        fontDanymic = worldUI.transform.Find("StageUI/Dyanamic").GetComponent<Transform>();
        getItemLayOut = worldUI.transform.Find("StageUI/Get").GetComponent<VerticalLayoutGroup>();

        for (int index = 0; index < stageSlot.Length; index++)
        {
            stageSlot[index] = worldUI.transform.Find("StageUI/TopLayOut/StageInfo").GetChild(index).GetComponent<Image>();
        }

        //테스트 버튼
        testBtn = worldUI.transform.Find("TestBtn").GetComponentsInChildren<Button>();


        weapbtnText = new TMP_Text[testBtn.Length];
        for (int index = 0; index < testBtn.Length; index++)
        {
            weapbtnText[index] = testBtn[index].GetComponentInChildren<TMP_Text>();
        }
        curMaterial[0] = worldUI.transform.Find("StageUI/MaterialBox/Gold/Text").GetComponent<TMP_Text>();
        curMaterial[1] = worldUI.transform.Find("StageUI/MaterialBox/Star/Text").GetComponent<TMP_Text>();
        curMaterial[3] = worldUI.transform.Find("StageUI/MaterialBox/Ruby/Text").GetComponent<TMP_Text>();
        nickNameText = worldUI.transform.Find("StageUI/TopLayOut/Profile/NickName").GetComponent<TMP_Text>();


        questListBtn = worldUI.transform.Find("StageUI/QeustList/Button").GetComponent<Button>();
        mainMenuBtn = worldUI.transform.Find("StageUI/MainMenu").GetComponent<Button>();
        mainMenuBtn.onClick.AddListener(() => MainMenuManager.inst.Set_MainMenuActive(true));

        hwanSengBtn = worldUI.transform.Find("StageUI/HwanSeng").GetComponent<Button>();

        // 메뉴버튼들

        getLetterBtn = worldUI.transform.Find("StageUI/Letter").GetComponent<Button>(); // 우편함
        dailyPlayCheckBtn = worldUI.transform.Find("StageUI/DailyCheck").GetComponent<Button>(); //출석체크
        newBieBtn = worldUI.transform.Find("StageUI/NewBie").GetComponent<Button>(); //뉴비
        //mosterDogamBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/MosterDogam").GetComponent<Button>(); //몬스터도감
        minigameAlrimBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/MiniGame").GetComponent<Button>(); //미니게임
        adDeleteBtn = worldUI.transform.Find("StageUI/AdDelete").GetComponent<Button>(); // 광고제거
        openMenuIcon = worldUI.transform.Find("StageUI/MenuBox/MeneOpen/RealBtn").GetComponent<Button>(); // 메뉴 삼각형버튼
        checkArrowScaleX = openMenuIcon.transform.parent.GetComponent<Transform>();
        buffShopBtn = worldUI.transform.Find("StageUI/BuffShop").GetComponent<Button>(); // 버프상점
        crewViewrBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/CrewViewr").GetComponent<Button>();      // 동료 살펴보기 버튼
        weaponShopBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/WeaponDogam").GetComponent<Button>();        // 무기도감버튼
        eventShopBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/EventShop").GetComponent<Button>();        // 이벤트샵 버튼
        bingoBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/Bingo").GetComponent<Button>(); // 빙고게임 버튼

        menuAnim = worldUI.transform.Find("StageUI/MenuBox").GetComponent<Animator>(); // 메뉴바 연출 애니메ㅐ이션

        // FrontUI
        frontUiCutton = frontUICanvas.transform.Find("Cutton").GetComponent<Image>();

        // 화이트커튼 이펙트용
        whiteCutton = frontUICanvas.transform.Find("Cutton(W)").GetComponent<Image>();

        Prefabs_Awake();

    }



    void Start()
    {
        BtnInIt();
        //menuAnim.SetTrigger("Open");
    }


    /// <summary>
    /// 스테이지 UI바 색칠
    /// </summary>
    /// <param name="curFloorLv"></param>
    public void Set_StageUiBar(int curFloorLv)
    {
        int setupCount = curFloorLv - 1;
        stageSlot[setupCount].sprite = stageSprite[2];
        stageText.text = $"스테이지 {GameStatus.inst.StageLv} - {GameStatus.inst.FloorLv}";
        uiBossHead.gameObject.SetActive(setupCount == 4 ? true : false);
        for (int index = 0; index < setupCount; index++)
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
        stageText.text = $"스테이지 {GameStatus.inst.StageLv} - {GameStatus.inst.FloorLv}";
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

        // 월드 버튼 초기화
        hwanSengBtn.onClick.AddListener(() => HwanSengSystem.inst.Set_HwansengUIActive(true)); //환생버튼
        getLetterBtn.onClick.AddListener(() => { LetterManager.inst.OpenPostOnOfficeAndInit(true); });
        dailyPlayCheckBtn.onClick.AddListener(() => { DailyPlayCheckUIManager.inst.MainWindow_Acitve(true); });
        newBieBtn.onClick.AddListener(() => { Newbie_Content.inst.Set_NewbieWindowActive(true); });
        //mosterDogamBtn.onClick.AddListener(() => { DogamManager.inst.Set_DogamListAcitve(1, true); });
        adDeleteBtn.onClick.AddListener(() => AdDelete.inst.ActiveAdDeleteWindow());
        buffShopBtn.onClick.AddListener(() => { BuffManager.inst.Buff_UI_Active(true); });
        crewViewrBtn.onClick.AddListener(() =>
        {
            PetDetailViewr_UI.inst.TopArrayBtnActive(0);
        });

        weaponShopBtn.onClick.AddListener(() =>
        {
            DogamManager.inst.Set_DogamListAcitve(0, true);
        });

        eventShopBtn.onClick.AddListener(() =>
        {
            EventShop_RulletManager.inst.Active_RulletEventShop(true);
        });

        // 겸희 빙고 나중에 연결
        bingoBtn.onClick.AddListener(() =>
        {

        });

        minigameAlrimBtn.onClick.AddListener(() =>
        {
            MinigameManager.inst.Active_minigameEntrance(true);
        });

        openMenuIcon.onClick.AddListener(() =>
        {
            if (checkArrowScaleX.localScale.x == 1)
            {
                menuAnim.SetTrigger("Open");
            }
            else if (checkArrowScaleX.localScale.x == -1)
            {
                menuAnim.SetTrigger("Close");
            }
        });
    }

    private void Prefabs_Awake()
    {
        int poolingCount = poolingObj.Length;
        minusMaterialFont = new UI_IncreaseValueFont[poolingCount];

        // Que 초기화        
        poolingQue = new Queue<GameObject>[poolingCount];
        for (int index = 0; index < poolingCount; index++)
        {
            poolingQue[index] = new Queue<GameObject>();
        }
        int count = 10;



        for (int forCount = 0; forCount < poolingCount; forCount++)
        {
            for (int index = 0; index < count; index++)
            {
                InstantiatePrefabs(forCount);
            }
        }

        fontPoint[0] = worldUI.transform.Find("StageUI/Dyanamic/0").GetComponent<Transform>();
        fontPoint[1] = worldUI.transform.Find("StageUI/Dyanamic/1").GetComponent<Transform>();


    }

    /// <summary>
    /// 오브젝트 생성
    /// </summary>
    /// <param name="value"></param>
    private void InstantiatePrefabs(int value)
    {
        GameObject obj = null;

        switch (value)
        {
            case 0: // Reward
                obj = Instantiate(poolingObj[value], rewardRef.transform);
                break;

            case 1: //좌하단
                obj = Instantiate(poolingObj[value], getItemLayOut.gameObject.transform);
                break;

            case 2: // 골드소모 폰트
                obj = Instantiate(poolingObj[value], fontDanymic);
                minusMaterialFont[value] = obj.GetComponent<UI_IncreaseValueFont>();
                obj.transform.position = fontDanymic.transform.position;
                break;
        }
        poolingQue[value].Enqueue(obj);
        obj.SetActive(false);
    }



    /// <summary>
    ///  WorldUI_Prefabs
    /// </summary>
    /// <param name="index"> 0Reward 상단바 / 1 좌측하단 겟아이템 </param>
    /// <returns></returns>
    public GameObject Get_WorldUIPooling_Prefabs_Object(int index)
    {
        GameObject obj;

        if (poolingQue[index].Count <= 0)
        {
            InstantiatePrefabs(index);
        }

        obj = poolingQue[index].Dequeue();

        return obj;
    }

    /// <summary>
    /// 골드 및 별 사용 UI에서 숫자 올라감
    /// </summary>
    /// <param name="type"> 0골드, 1 별</param>
    /// <param name="Matrielvalue"></param>
    public void Use_GoldOrStarMetrialFontPooling(int type, string Matrielvalue)
    {
        GameObject obj;

        if (poolingQue[2].Count <= 0)
        {
            InstantiatePrefabs(2);
        }
       
        obj = poolingQue[2].Dequeue();
        obj.transform.localPosition = fontPoint[type].localPosition;
        obj.GetComponent<UI_IncreaseValueFont>().Set_PosAndColorInit(Matrielvalue);
    }
    /// <summary>
    /// World UI 좌측하단 알림
    /// </summary>
    /// <param name="index"></param>
    /// <param name="img"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public void Get_ItemInfomation_UI_Active(Sprite img, string Value)
    {
        if(!worldUI.gameObject.activeInHierarchy) { return; }

        GameObject obj;

        if (poolingQue[1].Count <= 0)
        {
            InstantiatePrefabs(1);
        }

        obj = poolingQue[1].Dequeue();
        obj.gameObject.SetActive(true);
        obj.GetComponent<GetItemPrefabs>().Set_GetItemSpriteAndText(img, Value);

    }

    /// <summary>
    /// WorldUI Prefabs Return
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"> 0 Reward / 1 UI Material</param>
    public void Return_WorldUIObjPoolingObj(GameObject obj, int value)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }
        poolingQue[value].Enqueue(obj);
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
        if (cuttonCor != null)
        {
            StopCoroutine(cuttonCor);
        }

        cuttonCor = StartCoroutine(PlayCutton(funtion));
    }
    IEnumerator PlayCutton(Action funtion)
    {
        if (frontUiCutton.gameObject.activeSelf == false)
        {
            frontUiCutton.gameObject.SetActive(true);
        }

        while (frontUiCutton.color.a < 1)
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


    /// <summary>
    /// 월드 중앙 상단 Reward창 호출
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="text"> 아이템 설명내용</param>
    public void Set_RewardUI_Invoke(Sprite sprite, string text)
    {
        if (!worldUI.gameObject.activeInHierarchy) { return; }

        GameObject obj = Get_WorldUIPooling_Prefabs_Object(0);
        obj.GetComponent<Reward_Parts>().Set_Reward(sprite, text);
    }

    /// <summary>
    /// 월드 중앙 상단 Reward + Action 포함 호출
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="text"> 아이템 설명내용</param>
    public void Set_Reward_InclueAction(Sprite sprite, string text, Action funtion)
    {
        if (!worldUI.gameObject.activeInHierarchy) { return; }

        GameObject obj = Get_WorldUIPooling_Prefabs_Object(0);
        obj.GetComponent<Reward_Parts>().Set_RewardIncludeAction(sprite, text, funtion);
    }



    Color fadeOutColor = new Color(0, 0, 0, 0.05f);
    float FadeOutSpeedMultiFlyer = 12f;

    /// <summary>
    /// 로그인씬에서 메인씬 로드완료시 페이크 페이드아웃 실행
    /// </summary>
    public void LoadScene_FakeScreen_Active()
    {
        StartCoroutine(FakeScreenStart());
    }

    IEnumerator FakeScreenStart()
    {
        if (!fakeScreen.gameObject.activeSelf)
        {
            fakeScreen.gameObject.SetActive(true);
        }

        while (fakeScreen.color.a > 0f)
        {
            fakeScreen.color -= fadeOutColor * Time.deltaTime * FadeOutSpeedMultiFlyer;
            yield return null;
        }

        fakeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// 로우이미지 키고 켜기
    /// </summary>
    /// <param name="indexNum"></param>
    /// <param name="Active"></param>
    public void RawImagePlay(int indexNum, bool Active)
    {
       rawCam.SetActive(Active);
       rawImageRef[indexNum].SetActive(Active);
    }

    public void RawImagePlay(bool Active)
    {
        rawCam.SetActive(Active);
        for(int index=0; index < rawImageRef.Length; index++)
        {
            if (rawImageRef[index].activeSelf)
            {
                rawImageRef[index].SetActive(Active);
            }
        }
    }

    /// <summary>
    /// 화면 페이드 하얘졌다가 점점 내림
    /// </summary>
    public void Effect_WhiteCutton()
    {
        StartCoroutine(PlayWhiteEffect());
    }

    Color fadeStartColor = new Color(1, 1, 1, 0.9f);
    float duration = 2f;
    float fadeTimer = 0f;

    IEnumerator PlayWhiteEffect()
    {
        fadeTimer = 0;
        whiteCutton.color = fadeStartColor;
        whiteCutton.gameObject.SetActive(true); 
        
        while(fadeTimer < duration)
        {
            float alpha = Mathf.Lerp(fadeStartColor.a, 0f, fadeTimer / duration);
            whiteCutton.color = new Color(1, 1, 1, alpha);
            fadeTimer += Time.deltaTime;
            yield return null;
        }

        whiteCutton.color = new Color(1,1,1,0);
        whiteCutton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Load시 닉네임 초기화
    /// </summary>
    /// <param name="nickname"></param>
    public void Set_Nickname(string nickname) => nickNameText.text = nickname;


    public void NewbieBtnAcitveFalse() => newBieBtn.gameObject.SetActive(false);

    /// <summary>
    /// 아이템획득 레이아웃 껏다켯다
    /// </summary>
    /// <param name="value"></param>
    public void GetTrs_VerticalLayOutActive(bool value) => getItemLayOut.enabled = value;



}
