using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HwanSengSystem : MonoBehaviour
{
    public static HwanSengSystem inst;

    int selectType = 0;
    int adStarIncrease = 50;
    int rubyStarMultiPle = 3;
    string[] alrimTypeText = { "환생 타입 : 일반 환생", "환생 타입 : 광고 환생", "환생 타입 : 루비 환생" };
    // Ref
    GameObject frontUI, worldUI, hwansengRef, mainWindowRef, rawImage;

    // Title
    Button xBtn;

    // 본체 필요한 변수들
    GameObject[] activeHwanseng = new GameObject[2];

    //값 텍스트
    TMP_Text textBox_FloorInfoText, textBox_StarValueText;

    //메인 화면 버튼 3개
    Button[] mainBtn;
    GameObject btnMask;

    //알림
    GameObject alrimWindowRef;
    Button[] alrimBtn;
    TMP_Text feverTimeTextSec;
    TMP_Text alrim_HwansengTypeText;
    TMP_Text worldUICenterTopText;

    //알림안에 피버 초
    float defaultFeverTime = 30f;
    float relicAddTime = 0;
    float totalFeverTime = 0;

    // 메인 화면 좌측하단 환생버튼 Return Star값 Text
    TMP_Text hwansengIconReturnValueText;

    // 피버타임
    Animator feverAnim;
    Image feverFrontImg;
    float feverCountTimer;
    Material feverBG;
    Material feverFrontBGMat;


   
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
        frontUI = GameManager.inst.FrontUiRef;
        worldUI = GameManager.inst.WorldUiRef;
        rawImage = GameManager.inst.RawImgRef;
        hwansengRef = frontUI.transform.Find("Hwnaseng").gameObject;
        mainWindowRef = hwansengRef.transform.Find("Window").gameObject;

        worldUICenterTopText = worldUI.transform.Find("HwansengCount").GetComponent<TMP_Text>();

        //Btn
        xBtn = mainWindowRef.transform.Find("Title/X_Btn").GetComponent<Button>();
        mainBtn = mainWindowRef.transform.Find("Btn").GetComponentsInChildren<Button>();
        btnMask = mainWindowRef.transform.Find("Mask").gameObject;

        // 중간 텍스트
        activeHwanseng[0] = mainWindowRef.transform.Find("Bot_Box/Active_True").gameObject;
        activeHwanseng[1] = mainWindowRef.transform.Find("Bot_Box/Active_False").gameObject;

        textBox_FloorInfoText = mainWindowRef.transform.Find("Bot_Box/Active_True/ToTalFloorText").GetComponent<TMP_Text>();
        textBox_StarValueText = mainWindowRef.transform.Find("Bot_Box/Active_True/GetStarText").GetComponent<TMP_Text>();

        // 알림창
        alrimWindowRef = hwansengRef.transform.Find("Alrim").gameObject;
        alrimBtn = alrimWindowRef.transform.Find("Box/Btns").GetComponentsInChildren<Button>();
        feverTimeTextSec = alrimWindowRef.transform.Find("Box/FeverTime/FeverTimeText").GetComponent<TMP_Text>();
        alrim_HwansengTypeText = alrimWindowRef.transform.Find("Box/HwansengTypeText").GetComponent<TMP_Text>();
        // WorldUI 환생아이콘 텍스트
        hwansengIconReturnValueText = worldUI.transform.Find("StageUI/HwanSeng/Box/CurStarText").GetComponent<TMP_Text>();

        // 피버타임
        feverAnim = worldUI.transform.Find("Fever").GetComponent<Animator>();
        feverFrontImg = feverAnim.transform.Find("TimeBG/FRONT").GetComponent<Image>();
        feverBG = feverAnim.transform.Find("BG").GetComponent<Image>().material;
        feverFrontBGMat = feverAnim.transform.Find("FrontBG").GetComponent<Image>().material;
        BtnInIt();

    }

    private void Start()
    {

    }

    Vector3 tillingVec;
    Vector3 tillingFrontVec;
    float tillingSpeedMultipleyr = 1.8f;
    float tillingFrontSpeedMultipleyr = 0.25f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) { FeverTimeActive(30, 0); }

        // 환생 애니메이션 실행시 백그라운드 
        FeverTime_AnimationUpdate();
    }

    // 환생 애니메이션 실행시 백그라운드 
    private void FeverTime_AnimationUpdate()
    {

        if (feverAnim.gameObject.activeInHierarchy)
        {
            tillingVec.x += Time.deltaTime * tillingSpeedMultipleyr;
            tillingVec.x = Mathf.Repeat(tillingVec.x, 1);
            feverBG.mainTextureOffset = tillingVec;

            tillingFrontVec.x += Time.deltaTime * tillingFrontSpeedMultipleyr;
            tillingFrontVec.x = Mathf.Repeat(tillingVec.x, 1);
            feverFrontBGMat.mainTextureOffset = tillingVec;
        }
    }
    private void BtnInIt()
    {

        xBtn.onClick.AddListener(() =>
        {
            Set_HwansengUIActive(false);
        });

        mainBtn[0].onClick.AddListener(() => // 일반 환생
        {
            selectType = 0;
            Set_AlrimWindowActive(true);
        });

        mainBtn[1].onClick.AddListener(() => // 광고 환생
        {
            selectType = 1;
            Set_AlrimWindowActive(true);
        });

        mainBtn[2].onClick.AddListener(() => // 루비 환생
        {
            selectType = 2;
            RubyPayment.inst.RubyPaymentOnlyFuntion(RubyPrice.inst.HwansengPrice, () => Set_AlrimWindowActive(true));
        });


        // 알림창 버튼들
        alrimBtn[0].onClick.AddListener(() => // 돌아가기
        {
            selectType = 0;
            Set_AlrimWindowActive(false);
        });

        alrimBtn[1].onClick.AddListener(() => // 최종 실행 버튼
        {
            switch (selectType)
            {
                case 0: // 걍 실행
                    FeverTimeActive(totalFeverTime, selectType);
                    break;

                case 1: // 광고 보여주고 실행
                    ADViewManager.inst.SampleAD_Active_Funtion(() => FeverTimeActive(totalFeverTime, selectType));
                    break;

                case 2: // 구매창 물어본 후 실행
                    RubyPayment.inst.RubyPaymentUiActive(RubyPrice.inst.HwansengPrice, () => FeverTimeActive(totalFeverTime, selectType));
                    break;
            }

        });

        //; // 피버타임   피버실행
    }


    ///////////////////////////////////////////// UI 조작부 //////////////////////////////////////////////////////////


    int openFloor = 150;
    
    /// <summary>
    /// 환생UI 호출 
    /// </summary>
    /// <param name="active"></param>
    public void Set_HwansengUIActive(bool active)
    {
        if (active)
        {
            if(GameStatus.inst.AccumlateFloor <= openFloor)
            {
                activeHwanseng[0].SetActive(false);
                activeHwanseng[1].SetActive(true);
                btnMask.SetActive(true);
            }
            else if(GameStatus.inst.AccumlateFloor > openFloor)
            {
                activeHwanseng[0].SetActive(true);
                activeHwanseng[1].SetActive(false);
                btnMask.SetActive(false);
            }
        }
        hwansengRef.SetActive(active);
    }


    /// <summary>
    /// 알림창 호출
    /// </summary>
    /// <param name="active"></param>
    private void Set_AlrimWindowActive(bool active)
    {
        if (active)
        {
            relicAddTime = GameStatus.inst.GetAryRelicLv(4);
            totalFeverTime = defaultFeverTime + relicAddTime;
            feverTimeTextSec.text = $"{totalFeverTime}초";
            alrim_HwansengTypeText.text = alrimTypeText[selectType];
        }
        alrimWindowRef.SetActive(active);
    }




    /// <summary>
    /// FeverTime Setting
    /// </summary>
    /// <param name="InputTime"> 초 </param>
    /// <param name="hwansengType"> 일반 / 광고 / 루비</param>
    /// <param name="isAd"></param>
    public void FeverTimeActive(float InputTime, int hwansengType)
    {
        //초기화
        Set_HwansengUIActive(false);
        Set_AlrimWindowActive(false);
        GameStatus.inst.HWansengCount++;

        feverCountTimer = InputTime; // 추가시간 더하기 유물
        feverFrontImg.fillAmount = 1;

        // 별 지급
        string originalStarCount = CalCulator.inst.CurHwansengPoint();

        switch (hwansengType)
        {
            case 1: // 광고 증가율 50%
                originalStarCount = CalCulator.inst.DigitAndIntPercentMultiply(originalStarCount, adStarIncrease);
                break;

            case 2: // 루비 구매율
                originalStarCount = CalCulator.inst.StringAndIntMultiPly(originalStarCount, rubyStarMultiPle);
                break;
        }

        // 스탯 초기화
        GameStatus.inst.HwansengPointReset();
        GameStatus.inst.PlusStar(originalStarCount);
        selectType = 0;

        // 재생
        StartCoroutine(FeverPlay(InputTime));
    }


    IEnumerator FeverPlay(float InputTime)
    {
        //켜줌
        if (feverAnim.gameObject.activeSelf == false)
        {
            WorldUI_Manager.inst.Effect_WhiteCutton(); // 화면 하얗게 이펙트
            feverAnim.gameObject.SetActive(true);
            WorldUI_Manager.inst.RawImagePlayAcitve(0, true);
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

        WorldUI_Manager.inst.RawImagePlayAcitve(false);
        feverAnim.gameObject.SetActive(false);

        feverAnim.SetTrigger("Exit");
    }







    StringBuilder floorInfo = new StringBuilder();
    StringBuilder starValueInfo = new StringBuilder();

    /// <summary>
    /// 환생 지급별 관련 텍스트 업데이터
    /// </summary>
    public void MainWindow_TextBox_Updater()
    {
        floorInfo.Clear();
        floorInfo.Append($"총 클리어 층수 : {GameStatus.inst.AccumlateFloor} 층");
        textBox_FloorInfoText.text = floorInfo.ToString();

        starValueInfo.Clear();
        starValueInfo.Append($"{CalCulator.inst.StringFourDigitAddFloatChanger(CalCulator.inst.CurHwansengPoint())}");
        textBox_StarValueText.text = starValueInfo.ToString();
        // 월드아이콘 Value값
        hwansengIconReturnValueText.text = starValueInfo.ToString();
    }

    public void Set_WorldHwansengCount_Text_Init(int value)
    {
        worldUICenterTopText.text = $"{value}번째 설아의 출동 이야기";
    }

}
