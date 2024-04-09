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

    // 각 버프이미지 하단 Text
    TMP_Text keyPassiveUpText;
    TMP_Text stageLvUpPassiveText;
    TMP_Text totalStarValueUpPassiveText;

    // UI하단 버튼3종류
    Button maxHwansengStart;
    Button middelHwansengStart;
    Button normallHwansengStart;

    // 최종 환생하기 버튼창
    GameObject lastHwansengRef;
    Button hwanSengxBtn;
    Button hwanSengYesBtn;
    Button hwanSengAdYesBtn;
    TMP_Text hwansengWindowTitleText;

    // 메인 화면 좌측하단 환생버튼 Return Star값 Text
    TMP_Text hwansengIconReturnValueText;

    // 루비 부족 알림창
    GameObject failBuyRubyRef;
    Button failRubyBackBtn;
    Button goRubyStroeBtn;

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

        // 기본창 하단 초강화환생, 강화환생, 일반환생
        maxHwansengStart = mainWindowRef.transform.Find("Btn/LeftBtn").GetComponent<Button>();
        middelHwansengStart = mainWindowRef.transform.Find("Btn/MiddleBtn").GetComponent<Button>();
        normallHwansengStart = mainWindowRef.transform.Find("Btn/RightBtn").GetComponent<Button>(); ;

        // 별부스터 확인창
        exitStartBusterBtn = starBusterRef.transform.Find("Box/ExitBtn").GetComponent<Button>();
        buyRubyStartBuster = starBusterRef.transform.Find("Box/BuyRuby").GetComponent<Button>();

        // 최종 환생하기 창
        lastHwansengRef = hwansengRef.transform.Find("LastCheck").gameObject;
        hwanSengxBtn = lastHwansengRef.transform.Find("Box/Title/X_Btn").GetComponent<Button>();
        hwansengWindowTitleText = hwanSengxBtn.transform.parent.Find("TitleText").GetComponent<TMP_Text>();

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
        keyPassiveUpText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_L/Text").GetComponent<TMP_Text>();
        stageLvUpPassiveText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_C/Text").GetComponent<TMP_Text>();
        totalStarValueUpPassiveText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_R/Text").GetComponent<TMP_Text>();

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
        xBtn.onClick.AddListener(() =>
        {
            Set_HwansengUIActive(false);
        });

        starBusterBtn.onClick.AddListener(() =>  // 별모양 버튼
        {
            StartBusterWindowActive(true);
        });

        exitStartBusterBtn.onClick.AddListener(() => // 별 부스터 뒤로가기
        {
            StartBusterWindowActive(false);
        });

        buyRubyStartBuster.onClick.AddListener(() =>
        {
            // 환생 추가열쇠 300개 추가 => 루비 300 구매 버튼
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
            }
            else if (curRuby < needRuby)// false면 상점으로 이동 필요
            {
                NoHaveRubyAlrimWindowActive(true);
                Debug.Log($"루비 {needRuby - curRuby} 부족");
            }


        });

        normallHwansengStart.onClick.AddListener(() =>
        {
            LastCheckWindowActive(true, 2);
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
            // 추후에 상점이동 함수 넣어야함
        });
    }

    public void Set_HwansengUIActive(bool active)
    {
        hwansengRef.SetActive(active);
    }

    public void StartBusterWindowActive(bool active)
    {
        starBusterRef.SetActive(active);
    }

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
        lastHwansengRef.SetActive(active);
    }

    // 환생아이콘 값 최신화 업데이터
    public void WorldUIHwansengIconReturnStarUpdate()
    {
        string curReturnStarCount = CalCulator.inst.CurHwansengPoint(0);
        curReturnStarCount = CalCulator.inst.StringFourDigitAddFloatChanger(curReturnStarCount);
        hwansengIconReturnValueText.text = curReturnStarCount;
    }
}
