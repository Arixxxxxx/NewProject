using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BuffManager : MonoBehaviour
{
    public static BuffManager inst;

    [Header("# Input Buff Time (Min) / View ToolTip")]

    [Space]
    [SerializeField][Tooltip("0번 : UI창 광고 ATK \n1번 : UI 창 광고 이동속도\n2번: UI창 광고 골드획득량\n3번: 인게임 팝업광고 공격력// 4번 뉴비버프")] float[] adBuffTime;
    public float AdbuffTime(int index) => adBuffTime[index];

    [SerializeField][Tooltip("0번 : UI창 루비 ATK \n1번 : UI 창 루비 이동속도\n2번: UI창 루비 골드획득량")] float[] RubyBuffTime;

    GameObject mainWindow;
    Animator mainWindowAnim;


    GameObject frontUIRef;
    GameObject buffSelectUIWindow;


    Button exitBtn;
    int btnCount;
    Button[] viewAdBtn;
    // AD 쿨타임관련
    float[] viewAdCoolTimer;
    GameObject[] btnAdActiveIMG;
    TMP_Text[] adCoolTimeText;
    TMP_Text[] buffIconBottomTime;

    TMP_Text[] uiWindowTimeInfo = new TMP_Text[6]; // 0,1공격, 2,3 이속,, 4,5골드   AD : Ruby

    // 루비 선택창관련
    Button[] useRubyBtn;
    TMP_Text[] rubyPrice;
 

    GameObject worldUIRef, buffLayOutRef;

    //화면에 임시로 뜨는 광고 공격력증가 버튼
    Button adBuffBtn;
    float viewAdATKBuff;


    int useRutyTemp;
    // 뉴비버튼
    GameObject newbiebuffIcon;
    GameObject newbiebuffIconActive;

    
    // 화면메인 Reward 문구
    string[] buffstringText = new string[4];
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(inst);
        }

        viewAdATKBuff = Random.Range(60f, 120f);
        worldUIRef = GameManager.inst.WorldUiRef;
        frontUIRef = GameManager.inst.FrontUiRef;

        adBuffBtn = worldUIRef.transform.Find("ADBuff").GetComponent<Button>(); // 인게임 팝업 버프아이콘


        //기본 버프선택창
        mainWindow = frontUIRef.transform.Find("Buff_Window").gameObject;
        mainWindowAnim = mainWindow.GetComponent<Animator>();

        buffSelectUIWindow = frontUIRef.transform.Find("Buff_Window/Window").gameObject;
        exitBtn = buffSelectUIWindow.transform.Find("ExitBtn").GetComponent<Button>();

        btnCount = buffSelectUIWindow.transform.Find("Buff_Layout").childCount;
        buffLayOutRef = buffSelectUIWindow.transform.Find("Buff_Layout").gameObject;

        viewAdBtn = new Button[btnCount];
        viewAdCoolTimer = new float[btnCount];
        btnAdActiveIMG = new GameObject[btnCount];
        useRubyBtn = new Button[btnCount];
        adCoolTimeText = new TMP_Text[btnCount];
        rubyPrice = new TMP_Text[btnCount];
        buffIconBottomTime = new TMP_Text[btnCount];


        //ATK 초기화
        viewAdBtn[0] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/ChoiseBtn_AD").GetComponent<Button>();
        btnAdActiveIMG[0] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/ChoiseBtn_AD/AD").gameObject;
        adCoolTimeText[0] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/ChoiseBtn_AD/Left_Time").GetComponent<TMP_Text>();
        buffIconBottomTime[0] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/Space/ReturnItem_Text").GetComponent<TMP_Text>();
        uiWindowTimeInfo[0] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/ChoiseBtn_AD/Cock_IMG/Add_Time_Info").GetComponent<TMP_Text>();
        uiWindowTimeInfo[1] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/ChoiseBtn_Ruby/Cock_IMG/Add_Time_Info").GetComponent<TMP_Text>();


        viewAdBtn[1] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/ChoiseBtn_AD").GetComponent<Button>();
        btnAdActiveIMG[1] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/ChoiseBtn_AD/AD").gameObject;
        adCoolTimeText[1] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/ChoiseBtn_AD/Left_Time").GetComponent<TMP_Text>();
        buffIconBottomTime[1] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/Space/ReturnItem_Text").GetComponent<TMP_Text>();
        uiWindowTimeInfo[2] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/ChoiseBtn_AD/Cock_IMG/Add_Time_Info").GetComponent<TMP_Text>();
        uiWindowTimeInfo[3] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/ChoiseBtn_Ruby/Cock_IMG/Add_Time_Info").GetComponent<TMP_Text>();


        viewAdBtn[2] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/ChoiseBtn_AD").GetComponent<Button>();
        btnAdActiveIMG[2] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/ChoiseBtn_AD/AD").gameObject;
        adCoolTimeText[2] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/ChoiseBtn_AD/Left_Time").GetComponent<TMP_Text>();
        buffIconBottomTime[2] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/Space/ReturnItem_Text").GetComponent<TMP_Text>();
        uiWindowTimeInfo[4] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/ChoiseBtn_AD/Cock_IMG/Add_Time_Info").GetComponent<TMP_Text>();
        uiWindowTimeInfo[5] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/ChoiseBtn_Ruby/Cock_IMG/Add_Time_Info").GetComponent<TMP_Text>();


        useRubyBtn[0] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/ChoiseBtn_Ruby").GetComponent<Button>();
        useRubyBtn[1] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/ChoiseBtn_Ruby").GetComponent<Button>();
        useRubyBtn[2] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/ChoiseBtn_Ruby").GetComponent<Button>();

        //뉴비 관련
        newbiebuffIcon = worldUIRef.transform.Find("StageUI/Buff/NewBie").gameObject;
        newbiebuffIconActive = newbiebuffIcon.transform.Find("Active").gameObject;

        uiWindowTimeInfo[0].text = $"+{adBuffTime[0]}M";
        uiWindowTimeInfo[1].text = $"+{RubyBuffTime[0]}M";
        uiWindowTimeInfo[2].text = $"+{adBuffTime[1]}M";
        uiWindowTimeInfo[3].text = $"+{RubyBuffTime[1]}M";
        uiWindowTimeInfo[4].text = $"+{adBuffTime[2]}M";
        uiWindowTimeInfo[5].text = $"+{RubyBuffTime[2]}M";

        buffstringText[0] = $"공격력 2배 버프 {AdbuffTime(0)} 분";
        buffstringText[1] = $"이동속도 2배 버프 {AdbuffTime(1)} 분";
        buffstringText[2] = $"골드획득량 2배 버프 {AdbuffTime(2)} 분";
        buffstringText[3] = $" 공격력 3배 버프 {AdbuffTime(3)} 분";
    }

    private void Start()
    {
        // UII 루비 가격Text 초기화
        useRubyBtn[0].transform.Find("AD/Text").GetComponent<TMP_Text>().text = RubyPrice.inst.Get_buffRubyPrice(0).ToString();
        useRubyBtn[1].transform.Find("AD/Text").GetComponent<TMP_Text>().text = RubyPrice.inst.Get_buffRubyPrice(1).ToString();
        useRubyBtn[2].transform.Find("AD/Text").GetComponent<TMP_Text>().text = RubyPrice.inst.Get_buffRubyPrice(2).ToString();

        BtnInIt();
    }
    private void Update()
    {
        CheakCoomTime(0);
        CheakCoomTime(1);
        CheakCoomTime(2);
        ViewAdAtkBuff_CoolTimer();
    }

    private void BtnInIt()
    {
        exitBtn.onClick.AddListener(() => { Buff_UI_Active(false); });

        // 1. 광고 버프 
        viewAdBtn[0].onClick.AddListener(() => AdBuffActive(0));
        viewAdBtn[1].onClick.AddListener(() => AdBuffActive(1));
        viewAdBtn[2].onClick.AddListener(() => AdBuffActive(2));

        // 2. 루비 구매

        useRubyBtn[0].onClick.AddListener(() =>
        {
            RubyPayment.inst.RubyPaymentUiActive(100, () =>
            {
                RubyPayBtnInit(0);
            });
        });

        useRubyBtn[1].onClick.AddListener(() => {

            RubyPayment.inst.RubyPaymentUiActive(100, () =>
            {
                RubyPayBtnInit(1);
            });
        });

        useRubyBtn[2].onClick.AddListener(() => {
            RubyPayment.inst.RubyPaymentUiActive(100, () =>
            {
                RubyPayBtnInit(2);
            });
        });

    
        // 인게임 광고 보고 공격력증가 버튼
        adBuffBtn.onClick.AddListener(() => ADViewManager.inst.SampleAD_Active_Funtion(() =>
        {
            adBuffBtn.gameObject.SetActive(false);
            BuffContoller.inst.ActiveBuff(3, AdbuffTime(3)); //버프활성화
            viewAdATKBuff += Random.Range(2 * 60f, 3 * 60); // Re PopUp CoomTime
            WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.BuffIMG(3), buffstringText[3]);
        }));

    }

    /// <summary>
    /// 버프샵 On/Off
    /// </summary>
    /// <param name="value"></param>
    public void Buff_UI_Active(bool value)
    {
        if (value)
        {
            mainWindow.SetActive(true);

        }
        else 
        {
            buffLayOutRef.SetActive(false);
            exitBtn.gameObject.SetActive(false);
            mainWindowAnim.SetTrigger("Exit");
        }
    }

    // 애니메이션 적용
    public void MainWindow_Active_False() => mainWindow.SetActive(false);

    private void RubyPayBtnInit(int value)
    {
        BuffContoller.inst.ActiveBuff(value, RubyBuffTime[value]);
        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.BuffIMG(value), RewardStringMaker(value));
        mainWindow.SetActive(false);
    }

    //기존에 있던 버프 string 배열 가져와서 재활용
    private string RewardStringMaker(int value)
    {
        int findindex = buffstringText[value].IndexOf('프');
        return new string(buffstringText[value].Substring(0,findindex + 1).ToArray()) + $" {RubyBuffTime[value]} 분";
    }


    private void AdBuffActive(int value)
    {
        ADViewManager.inst.SampleAD_Active_Funtion(() =>
        {
            BuffContoller.inst.ActiveBuff(value, AdbuffTime(value)); //버프활성화
            AddBuffCoolTime(value, (int)AdbuffTime(value)); // 쿨타임 시간추가
            mainWindow.gameObject.SetActive(false);
            //Set_TextAlrim(MakeAlrimMSG(0, (int)AdbuffTime(0))); // 알림띄우기
            WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.BuffIMG(value), buffstringText[value]);
        });
    }

    /// <summary>
    /// 버프 활성화
    /// </summary>
    /// <param name="type">0공/1이속/2골드/3강한공격력</param>
    /// <param name="Time">분 단위</param>
    /// <param name="text">획득창</param>
    public void ActiveBuff(int type, int Time, string text)
    {
        if (Time == 0) { return; }

        BuffContoller.inst.ActiveBuff(type, Time); //버프활성화
        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.BuffIMG(type), text);
    }
 


    // UI 창 버프 시간 Text 업데이트해주는 함수
    public void CheakCoomTime(int index)
    {
        // 버프 남은시간 추적
        if (viewAdCoolTimer[index] > 0)
        {
            if (btnAdActiveIMG[index].gameObject.activeSelf == true && adCoolTimeText[index].gameObject.activeSelf == false)
            {
                viewAdBtn[index].interactable = false;
                btnAdActiveIMG[index].gameObject.SetActive(false);
                adCoolTimeText[index].gameObject.SetActive(true);
                buffIconBottomTime[index].gameObject.SetActive(true);
            }

            viewAdCoolTimer[index] -= Time.deltaTime;
            int min = (int)viewAdCoolTimer[index] / 60;
            int sec = (int)viewAdCoolTimer[index] % 60;
            adCoolTimeText[index].text = $"{min} : {sec}";
            buffIconBottomTime[index].text = $"남은 시간: {min}분 {sec}초";
        }

        else if (viewAdCoolTimer[index] <= 0)
        {
            if (viewAdCoolTimer[index] != 0)
            {
                viewAdCoolTimer[index] = 0;
            }
            if (btnAdActiveIMG[index].gameObject.activeSelf == false && adCoolTimeText[0].gameObject.activeSelf == true)
            {
                viewAdBtn[index].interactable = true;
                btnAdActiveIMG[index].gameObject.SetActive(true);
                adCoolTimeText[index].gameObject.SetActive(false);
                buffIconBottomTime[index].gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// 한번 광고보면
    /// </summary>
    private void ViewAdAtkBuff_CoolTimer()
    {
        if (viewAdATKBuff > 0 && adBuffBtn.gameObject.activeSelf == false)
        {
            viewAdATKBuff -= Time.deltaTime;
        }
        else if (viewAdATKBuff <= 0)
        {
            viewAdATKBuff = 0;

            if (adBuffBtn.gameObject.activeSelf == false)
            {
                adBuffBtn.gameObject.SetActive(true);
            }

        }
    }



    /// <summary>
    /// 광고 쿨타임에 시간넣기
    /// </summary>
    /// <param name="index">버프 인덱스 번호</param>
    /// <param name="Time">시간(분)</param>
    public void AddBuffCoolTime(int index, int Time) => viewAdCoolTimer[index] = Time * 60;


    /// <summary>
    /// 광고 버프 남은시간 Return
    /// </summary>
    /// <param name="buffIndexNum"></param>
    public void viewAdCoolTime(int buffIndexNum) => viewAdCoolTimer[buffIndexNum] += 15 * 60;


    /// <summary>
    /// 인게임 버프창 뉴비아이콘 상태 체크
    /// </summary>
    /// <param name="value"></param>
    public void NewBieBuffActive(bool value)
    {
        newbiebuffIcon.SetActive(value);
        newbiebuffIconActive.SetActive(value);
    }
}

