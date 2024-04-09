using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HwanSengSystem : MonoBehaviour
{
    public static HwanSengSystem inst;

    // Ref
    GameObject frontUI, worldUI, hwansengRef, mainWindowRef;

    // Title
    Button xBtn;

    // 캐릭터뒷면 마법진
    Image charBg;
    float rotZ;

    // 별부스터창
    Button starBusterBtn;
    GameObject starBusterRef;
    Button exitStartBusterBtn;
    Button buyRubyStartBuster;
    bool activeStarbuster = false; // 구매시 쓰는 변수
    GameObject alreadyBuyAlrimWindow;
    Button alreadyBuyAlrimWindowBackBtn;
    TMP_Text alreadyBuyWindowMainText;

    // 각 버프이미지 하단 Text
    Button goldKeyBtn;
    Button floorKeyBtn;

    TMP_Text goldkeyLvText;
    TMP_Text stageLvUpkeyText;
    TMP_Text starBusterValueText;

    GameObject btnInfoWindow;
    TMP_Text btnInfoText;
    Image passiveIconBg_L; // 왼쪽 덮개 이미지
    Image passiveIconBg_M;// 가운데 아이콘 덮개 이미지

    // UI하단 환생 버튼3종류
    Button maxHwansengStart;
    Button middelHwansengStart;
    Button normallHwansengStart;

    // 최종 환생하기 버튼창
    GameObject lastHwansengRef;
    Image[] gagebar;
    Button freeUpgradeHwansengBtn;
    int adViewrCount;

    Button hwanSengxBtn;

    //환생 시작버튼 , 광고시작버튼
    Button hwanSengYesBtn;
    Button hwanSengAdYesBtn;
    TMP_Text hwansengWindowTitleText;

    // 중간 지급예상 별텍스트
    TMP_Text getStarCountViewrText;

    // 메인 화면 좌측하단 환생버튼 Return Star값 Text
    TMP_Text hwansengIconReturnValueText;

    // 루비 부족 알림창
    GameObject failBuyRubyRef;
    Button failRubyBackBtn;
    Button goRubyStroeBtn;

    // 피버타임
    Animator feverAnim;
    Image feverFrontImg;
    float feverCountTimer;

    int goldkeyLv;
    public int GoldKeyLv_Hwanseng
    {
        get { return goldkeyLv; }
        set { goldkeyLv = value; }
    }

    int stageLvUpkeyLv;
    public int StageLvUpkeyLv_Hwanseng
    {
        get { return stageLvUpkeyLv; }
        set { stageLvUpkeyLv = value; }
    }

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

        //Ref
        frontUI = GameObject.Find("---[FrontUICanvas]").gameObject;
        worldUI = GameObject.Find("---[World UI Canvas]").gameObject;

        hwansengRef = frontUI.transform.Find("Hwnaseng").gameObject;
        mainWindowRef = hwansengRef.transform.Find("Window").gameObject;
        starBusterRef = hwansengRef.transform.Find("StarBuster").gameObject;
        starBusterBtn = mainWindowRef.transform.Find("BuffViewr/iConLayOut_R").GetComponent<Button>();


        //Main Art
        charBg = mainWindowRef.transform.Find("MainArt/Bg").GetComponent<Image>();

        //Btn
        xBtn = mainWindowRef.transform.Find("Title/X_Btn").GetComponent<Button>();

        // 추가획득량창 상세정보창
        btnInfoWindow = mainWindowRef.transform.Find("BtnInfoWindow").gameObject;
        btnInfoText = btnInfoWindow.transform.Find("Text").GetComponent<TMP_Text>();
        goldKeyBtn = mainWindowRef.transform.Find("BuffViewr/iConLayOut_L").GetComponent<Button>();
        passiveIconBg_L = goldKeyBtn.transform.Find("Bg").GetComponent<Image>();

        floorKeyBtn = mainWindowRef.transform.Find("BuffViewr/iConLayOut_C").GetComponent<Button>();
        passiveIconBg_M = floorKeyBtn.transform.Find("Bg").GetComponent<Image>();

        getStarCountViewrText = mainWindowRef.transform.Find("Bot_Box/Text").GetComponent<TMP_Text>();

        // 기본창 하단 초강화환생, 강화환생, 일반환생
        maxHwansengStart = mainWindowRef.transform.Find("Btn/LeftBtn").GetComponent<Button>();
        middelHwansengStart = mainWindowRef.transform.Find("Btn/MiddleBtn").GetComponent<Button>();
        normallHwansengStart = mainWindowRef.transform.Find("Btn/RightBtn").GetComponent<Button>(); ;

        // 별부스터 확인창
        exitStartBusterBtn = starBusterRef.transform.Find("Box/ExitBtn").GetComponent<Button>();
        buyRubyStartBuster = starBusterRef.transform.Find("Box/BuyRuby").GetComponent<Button>();
        alreadyBuyAlrimWindow = hwansengRef.transform.Find("AlreadyBuyBuster").gameObject;
        alreadyBuyAlrimWindowBackBtn = alreadyBuyAlrimWindow.transform.Find("Box/Btns/BackBtn").GetComponent<Button>();
        alreadyBuyWindowMainText = alreadyBuyAlrimWindow.transform.Find("Box/Main/Option0/Text (TMP)").GetComponent<TMP_Text>();

        // 최종 환생하기 창
        lastHwansengRef = hwansengRef.transform.Find("LastCheck").gameObject;
        hwanSengxBtn = lastHwansengRef.transform.Find("Box/Title/X_Btn").GetComponent<Button>();
        hwansengWindowTitleText = hwanSengxBtn.transform.parent.Find("TitleText").GetComponent<TMP_Text>();

        gagebar = lastHwansengRef.transform.Find("Box/Main/Option1/Bar").GetComponentsInChildren<Image>(); // 광고횟수
        freeUpgradeHwansengBtn = lastHwansengRef.transform.Find("Box/Main/Option1/FreeHwansengBtn").GetComponent<Button>(); // 무료강화 버튼

        //환생 시작버튼 , 광고시작버튼
        hwanSengYesBtn = lastHwansengRef.transform.Find("Box/Btns/Yes").GetComponent<Button>();
        hwanSengAdYesBtn = lastHwansengRef.transform.Find("Box/Btns/Ad_Yes").GetComponent<Button>();

        // WorldUI 환생아이콘 텍스트
        hwansengIconReturnValueText = worldUI.transform.Find("StageUI/HwanSeng/Box/CurStarText").GetComponent<TMP_Text>();
        WorldUIHwansengIconReturnStarUpdate();

        // failBuyRubyRef
        failBuyRubyRef = hwansengRef.transform.Find("FailBuyRuby").gameObject;
        failRubyBackBtn = failBuyRubyRef.transform.Find("Box/Btns/BackBtn").GetComponent<Button>();
        goRubyStroeBtn = failBuyRubyRef.transform.Find("Box/Btns/GoRubyStoreBtn").GetComponent<Button>();

        // 중간 아이콘 하단에 수치 텍스트
        goldkeyLvText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_L/Text").GetComponent<TMP_Text>();
        stageLvUpkeyText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_C/Text").GetComponent<TMP_Text>();
        starBusterValueText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_R/Text").GetComponent<TMP_Text>();




        // 피버타임
        feverAnim = worldUI.transform.Find("Fever").GetComponent<Animator>();
        feverFrontImg = feverAnim.transform.Find("TimeBG/FRONT").GetComponent<Image>();


        BtnInIt();
    }


    private void Update()
    {
        if (hwansengRef.activeSelf) // 배경 마법진 회전
        {
            rotZ += Time.deltaTime * 10;
            rotZ = Mathf.Repeat(rotZ, 360);
            charBg.transform.localRotation = Quaternion.Euler(0, 0, rotZ);
        }
    }

    private void BtnInIt()
    {
        alreadyBuyAlrimWindowBackBtn.onClick.AddListener(() => alreadyBuyAlrimWindow.SetActive(false)); // 이미구입햇어요 뒤로가기창

        xBtn.onClick.AddListener(() =>
        {
            Set_HwansengUIActive(false);
        });

        starBusterBtn.onClick.AddListener(() =>  // 별부스터 구매버튼
        {
            if (activeStarbuster == false)
            {
                StartBusterWindowActive(true);
            }
            else
            {
                AlreadyBuyActive(1, true); // 이미 구매완료
            }

        });

        exitStartBusterBtn.onClick.AddListener(() => // 별 부스터 뒤로가기
        {
            StartBusterWindowActive(false);
        });

        buyRubyStartBuster.onClick.AddListener(() => // 별부스터 구매 버튼
        {
            int curRuby = GameStatus.inst.Ruby;
            int busterValue = 300;
            if (curRuby >= busterValue) // 구매완료
            {
                GameStatus.inst.Ruby -= busterValue;
                activeStarbuster = true;
                StartBusterWindowActive(false);
                UIValueBuffUpdate();
                AlreadyBuyActive(0, true);
            }
            else
            {
                StartBusterWindowActive(false);
                NoHaveRubyAlrimWindowActive(true);
            }


        });

        // 메인창 하단 환생하기 버튼 3종
        maxHwansengStart.onClick.AddListener(() =>
        {
            // 초강화 환생
            // 현재 루비 - 차감 루비 체크
            int curRuby = GameStatus.inst.Ruby;
            int needRuby = 1000;

            if (curRuby >= needRuby)// true면 넘어가고
            {
                LastCheckWindowActive(true, 0);
                LastCheckBtnInit(30, 0); // 최종 환생하기 , 광고보고 환생하기 버튼 초기화 (별 지급량 계산방식 다름)
            }
            else if (curRuby < needRuby)// false면 상점으로 이동 필요
            {
                NoHaveRubyAlrimWindowActive(true);
                Debug.Log($"루비 {needRuby - curRuby} 부족");
            }


        });


        middelHwansengStart.onClick.AddListener(() =>
        {
            // 강화 환생
            // 현재 루비 - 차감 루비 체크
            int curRuby = GameStatus.inst.Ruby;
            int needRuby = 500;

            if (curRuby >= needRuby)// true면 넘어가고
            {
                LastCheckWindowActive(true, 1);
                LastCheckBtnInit(30, 1); // 최종 환생하기 , 광고보고 환생하기 버튼 초기화 (별 지급량 계산방식 다름)
            }
            else if (curRuby < needRuby)// false면 상점으로 이동 필요
            {
                NoHaveRubyAlrimWindowActive(true);
            }


        });

        normallHwansengStart.onClick.AddListener(() =>
        {
            LastCheckWindowActive(true, 2);
            LastCheckBtnInit(30, 2); // 최종 환생하기 , 광고보고 환생하기 버튼 초기화 (별 지급량 계산방식 다름)
        });

        // 최종환생하기 관련 버튼

        hwanSengxBtn.onClick.AddListener(() => LastCheckWindowActive(false, 0));

        // 루비 부족알림창 뒤로가기버튼
        failRubyBackBtn.onClick.AddListener(() =>
        {
            NoHaveRubyAlrimWindowActive(false);
        });

        // 루비 부족알림창 상점으로 이동 버튼
        goRubyStroeBtn.onClick.AddListener(() =>
        {
            NoHaveRubyAlrimWindowActive(false);
            Set_HwansengUIActive(false);
            ShopManager.Instance.OpenRubyShop();
        });

        // 무료 강화 환생버튼 
        freeUpgradeHwansengBtn.onClick.AddListener(() =>
        {
            LastCheckWindowActive(false, 1); // 최종확인창 끄기
            Set_HwansengUIActive(false); // 메인창끄기
            LastCheckWindowAdViewrCountInit(false); // 최종창에 무료광고관련 항목들 리셋
            FeverTimeActive(30, 1, false); // 피버타임
        });

        // 환생포인트 증가량 버튼
        goldKeyBtn.onClick.AddListener(() => BtnInfoWindowActive(0, true, goldKeyBtn.transform.localPosition));

        floorKeyBtn.onClick.AddListener(() => BtnInfoWindowActive(1, true, floorKeyBtn.transform.localPosition));

    }



    /// <summary>
    /// 환생UI 호출 
    /// </summary>
    /// <param name="active"></param>
    public void Set_HwansengUIActive(bool active)
    {
        UIValueBuffUpdate(); // 메인창 중간 패시브량 및 텍스트들 초기화
        hwansengRef.SetActive(active);
    }




    /// <summary>
    /// 추가별 지급 패시브량 UI Text 업데이터
    /// </summary>
    /// <param name="goldkeyValue"></param>
    /// <param name="stageLvUpValue"></param>
    public void UIValueBuffUpdate()
    {
        // 골드키값 초기화
        int goldKeyValue = GoldKeyLv_Hwanseng * 10; // 유물레벨 * 10%
        passiveIconBg_L.gameObject.SetActive(goldKeyValue == 0 ? true : false);

        // 추가 층수 초기화
        int stageLvUpValue = StageLvUpkeyLv_Hwanseng * 5; // 유물레벨 * 5층
        passiveIconBg_M.gameObject.SetActive(stageLvUpValue == 0 ? true : false);

        // 받을수있는 루비양 체크
        string curStar = CalCulator.inst.CurHwansengPoint(stageLvUpValue); // 현재 지급받을수있는 화폐 체크
        goldkeyLvText.text = $"{goldKeyValue}% 증가";
        stageLvUpkeyText.text = $"{stageLvUpValue}층 증가";

        if (activeStarbuster) // 별부스터시 30% 만큼 증량
        {
            starBusterValueText.text = "30%";
            curStar = CalCulator.inst.DigitPercentMultiply(curStar, 30);
        }
        else
        {
            starBusterValueText.text = "0%";
        }

        getStarCountViewrText.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(curStar)} ( Stage{GameStatus.inst.StageLv} + 유물 추가 {stageLvUpValue}층 )";
    }



    /// <summary>
    /// 별부스터 버튼 창 호출
    /// </summary>
    /// <param name="active"></param>
    public void StartBusterWindowActive(bool active)
    {
        starBusterRef.SetActive(active);
    }




    /// <summary>
    /// 루비부족창 호출
    /// </summary>
    /// <param name="active"></param>
    private void NoHaveRubyAlrimWindowActive(bool active)
    {
        failBuyRubyRef.SetActive(active);
    }




    /// <summary>
    /// 최종 환생 확인창
    /// </summary>
    /// <param name="active"> true / false </param>
    /// <param name="invokeType"> 0 : 초강화 / 1 : 강화 / 2 : 일반 </param>
    public void LastCheckWindowActive(bool active, int invokeType)
    {
        string titleTextData = string.Empty;
        int curRuby = GameStatus.inst.Ruby;

        switch (invokeType)
        {
            case 0:
                titleTextData = "초강화 환생하기";
                break;

            case 1:
                titleTextData = "강화 환생하기";
                break;

            case 2:
                titleTextData = "환생하기";
                break;
        }

        hwansengWindowTitleText.text = titleTextData;
        LastCheckWindowAdViewrCountInit(true); // 광고횟수 및 버튼 초기화
        lastHwansengRef.SetActive(active);
    }





    Coroutine btnInfoCo;

    /// <summary>
    /// 환생포인트 증가량 클릭시 설명창 호출
    /// </summary>
    /// <param name="type"> 0 골든키 / 1 전자키</param>
    /// <param name="active">true / false</param>
    /// <param name="InvokePos"> 호출 트랜스폼 위치</param>
    private void BtnInfoWindowActive(int type, bool active, Vector3 InvokePos)
    {
        if (btnInfoWindow.activeSelf == true)
        {
            btnInfoWindow.SetActive(false);
        }

        string textInfo = string.Empty;

        switch (type)
        {
            case 0:
                textInfo = "골든키\r\n환생시 열쇠획득량 증가\r\n유물 메뉴에서 구매가능";
                break;

            case 1:
                textInfo = "전자키\r\n환생시 클리어 층수 증가\r\n유물 메뉴에서 구매가능";
                break;
        }

        btnInfoText.text = textInfo;
        btnInfoWindow.transform.localPosition = InvokePos;
        btnInfoWindow.SetActive(active);


        if (btnInfoCo != null)
        {
            StopCoroutine(btnInfoCo);
        }
        btnInfoCo = StartCoroutine(BtnInfoActiveFalse());

    }
    IEnumerator BtnInfoActiveFalse()
    {
        yield return new WaitForSeconds(3);
        btnInfoWindow.SetActive(false);
    }




    // 환생아이콘 값 최신화 업데이터
    public void WorldUIHwansengIconReturnStarUpdate()
    {
        string curReturnStarCount = CalCulator.inst.CurHwansengPoint(0);
        curReturnStarCount = CalCulator.inst.StringFourDigitAddFloatChanger(curReturnStarCount);
        hwansengIconReturnValueText.text = curReturnStarCount;
    }




    /// <summary>
    /// FeverTime Setting
    /// </summary>
    /// <param name="InputTime"> 초 </param>
    /// <param name="hwansengType"> 초강화, 강화, 일반</param>
    /// <param name="isAd"></param>
    public void FeverTimeActive(float InputTime, int hwansengType, bool isAd)
    {
        //초기화
        feverCountTimer = InputTime;
        feverFrontImg.fillAmount = 1;

        // 별 지급
        string addStarValue = CalCulator.inst.DigidPlus(GameStatus.inst.Star, CalCulator.inst.CurHwansengPoint(0));

        // 스탯 초기화
        GameStatus.inst.HwansengPointReset();

        switch (hwansengType)
        {
            case 0: // 초강화
                addStarValue = CalCulator.inst.StringAndIntMultiPly(addStarValue, 10);
                break;

            case 1: // 강화
                addStarValue = CalCulator.inst.StringAndIntMultiPly(addStarValue, 5);
                break;
        }

        if (isAd) // 광고시 10% 추가지급
        {
            addStarValue = CalCulator.inst.DigitPercentMultiply(addStarValue, 10);
        }

        GameStatus.inst.Star = addStarValue;
        // 재생
        StartCoroutine(FeverPlay(InputTime));
    }

    IEnumerator FeverPlay(float InputTime)
    {
        //켜줌
        if (feverAnim.gameObject.activeSelf == false)
        {
            feverAnim.gameObject.SetActive(true);
            ActionManager.inst.IsFever = true;
        }

        // 환생시간 피버바
        while (feverCountTimer > 0)
        {
            feverCountTimer -= Time.deltaTime;
            feverFrontImg.fillAmount = feverCountTimer / InputTime;
            yield return null;
        }

        feverCountTimer = 0;
        feverAnim.SetTrigger("Hide");
        ActionManager.inst.IsFever = false;
        yield return new WaitForSeconds(1f);
        feverAnim.gameObject.SetActive(false);

        feverAnim.SetTrigger("Exit");
    }

    private void LastCheckBtnInit(int Time, int Type)
    {
        // 최종 환생 시작버튼 초기화
        hwanSengYesBtn.onClick.RemoveAllListeners();
        hwanSengYesBtn.onClick.AddListener(() =>
        {
            LastCheckWindowActive(false, 0);
            Set_HwansengUIActive(false);

            FeverTimeActive(Time, Type, false);
        });

        // 최종 환생 광고 후 시작버튼 
        hwanSengAdYesBtn.onClick.RemoveAllListeners();
        hwanSengAdYesBtn.onClick.AddListener(() =>
        {
            LastCheckWindowActive(false, 0);
            Set_HwansengUIActive(false);

            adViewrCount++; //광고횟수
            if(adViewrCount >= 5)
            {
                adViewrCount = 5;
            }

            WorldUI_Manager.inst.SampleAD_Ad_FeverTIme(Time, Type, true); // 샘플광고
        });
    }

    /// <summary>
    /// 최종수락창 광고횟수 및 버튼 초기화 함수 초기화 및 리셋
    /// </summary>
    /// <param name="value"> true 진행 // false 리셋 </param>
    private void LastCheckWindowAdViewrCountInit(bool value)
    {
        if(value == true)
        {
            for (int index = 0; index < adViewrCount; index++)
            {
                gagebar[index].color = Color.green;
            }

            if(adViewrCount == 5)
            {
                freeUpgradeHwansengBtn.interactable = true;
            }
            else if(adViewrCount < 5)
            {
                freeUpgradeHwansengBtn.interactable = false;
            }

        }
        else if(value == false)  // 리셋기능
        {
            for (int index = 0; index < 5; index++)
            {
                gagebar[index].color = new Color(0.3f,0.3f,0.3f,0.5f);
            }
            adViewrCount = 0;
            freeUpgradeHwansengBtn.interactable = false;
        }
    }

    /// <summary>
    ///  부스터 적용 메세지 출력
    /// </summary>
    /// <param name="type"> 0 구매완료 / 1 이미 구매했음</param>
    private void AlreadyBuyActive(int type, bool value)
    {
        alreadyBuyWindowMainText.text = type == 0 ? "부스터가 적용되었습니다." : "이미 부스터가 적용되어 있습니다.";
        alreadyBuyAlrimWindow.SetActive(value);
    }

}
