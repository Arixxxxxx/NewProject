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
    GameObject buffWindow;
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
    //정말 살껀지 물어보는창
    GameObject alrimWindow;
    TMP_Text rubyValueText;
    Button[] alrimYesOrNoBtn = new Button[2];
    // 루비 부족시 뜨는창
    GameObject noHaveRubyMainWindow;
    Button[] noHaveRubyWindowYesOrNoBtn = new Button[2];

    GameObject worldUI;

    //화면에 임시로 뜨는 광고 공격력증가 버튼
    Button adBuffBtn;
    float viewAdATKBuff;


    int useRutyTemp;
    // 뉴비버튼
    GameObject newbiebuffIcon;
    GameObject newbiebuffIconActive;

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

        viewAdATKBuff = Random.Range(10f, 15f);
        worldUI = GameObject.Find("---[World UI Canvas]").gameObject;
        adBuffBtn = worldUI.transform.Find("StageUI/ADBuff").GetComponent<Button>(); // 인게임 팝업 버프아이콘

        buffWindow = GameObject.Find("---[FrontUICanvas]").gameObject;

        //기본 버프선택창
        mainWindow = buffWindow.transform.Find("Buff_Window").gameObject;
        buffSelectUIWindow = buffWindow.transform.Find("Buff_Window/Window").gameObject;
        exitBtn = buffSelectUIWindow.transform.Find("ExitBtn").GetComponent<Button>();

        btnCount = buffSelectUIWindow.transform.Find("Buff_Layout").childCount;
        viewAdBtn = new Button[btnCount];
        viewAdCoolTimer = new float[btnCount];
        btnAdActiveIMG = new GameObject[btnCount];
        useRubyBtn = new Button[btnCount];
        adCoolTimeText = new TMP_Text[btnCount];
        rubyPrice = new TMP_Text[btnCount];
        buffIconBottomTime = new TMP_Text[btnCount];

        //물어보는창 
        alrimWindow = buffWindow.transform.Find("Buff_Window/Alrim_Window").gameObject;
        rubyValueText = alrimWindow.transform.Find("Title/RubyValue_Text").GetComponent<TMP_Text>();
        alrimYesOrNoBtn[0] = alrimWindow.transform.Find("Title/NoBtn").GetComponent<Button>();
        alrimYesOrNoBtn[1] = alrimWindow.transform.Find("Title/YesBtn").GetComponent<Button>();

        // 루비없어 창
        noHaveRubyMainWindow = mainWindow.transform.Find("NoHaveRuby").gameObject;
        noHaveRubyWindowYesOrNoBtn[0] = noHaveRubyMainWindow.transform.Find("Title/NoBtn").GetComponent<Button>();
        noHaveRubyWindowYesOrNoBtn[1] = noHaveRubyMainWindow.transform.Find("Title/YesBtn").GetComponent<Button>();

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
        newbiebuffIcon = worldUI.transform.Find("StageUI/Buff/NewBie").gameObject;
        newbiebuffIconActive = newbiebuffIcon.transform.Find("Active").gameObject;

        uiWindowTimeInfo[0].text = $"+{adBuffTime[0]}M";
        uiWindowTimeInfo[1].text = $"+{RubyBuffTime[0]}M";
        uiWindowTimeInfo[2].text = $"+{adBuffTime[1]}M";
        uiWindowTimeInfo[3].text = $"+{RubyBuffTime[1]}M";
        uiWindowTimeInfo[4].text = $"+{adBuffTime[2]}M";
        uiWindowTimeInfo[5].text = $"+{RubyBuffTime[2]}M";
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
        exitBtn.onClick.AddListener(() => { WorldUI_Manager.inst.buffSelectUIWindowAcitve(false); });

        viewAdBtn[0].onClick.AddListener(() => WorldUI_Manager.inst.SampleADBuff("buff", 0)); ;
        viewAdBtn[1].onClick.AddListener(() => WorldUI_Manager.inst.SampleADBuff("buff", 1));
        viewAdBtn[2].onClick.AddListener(() => WorldUI_Manager.inst.SampleADBuff("buff", 2));

        // 인게임 광고 보고 공격력증가 버튼
        adBuffBtn.onClick.AddListener(() => {

            adBuffBtn.gameObject.SetActive(false);
            WorldUI_Manager.inst.SampleADBuff("buff", 3);
            viewAdATKBuff += 15f; // Re PopUp CoomTime

        });

        // 루비로 구매버튼 => 정말 구매하시껍니까 창으로 연결
        useRubyBtn[0].onClick.AddListener(() => Set_ReallyBuyBuffWindow_Active(0));
        useRubyBtn[1].onClick.AddListener(() => Set_ReallyBuyBuffWindow_Active(1));
        useRubyBtn[2].onClick.AddListener(() => Set_ReallyBuyBuffWindow_Active(2));

        alrimYesOrNoBtn[0].onClick.AddListener(() => alrimWindow.SetActive(false));

        //루비로 구매하지만 부족 창버튼
        noHaveRubyWindowYesOrNoBtn[0].onClick.AddListener(() => noHaveRubyMainWindow.SetActive(false)); // No 버튼
        noHaveRubyWindowYesOrNoBtn[1].onClick.AddListener(() =>  // Yes버튼
        {
            noHaveRubyMainWindow.SetActive(false);
            mainWindow.SetActive(false);
            //상점창으로 이동~~
        });

    }


    /// <summary>
    /// 정말 구매하실껀지 물어보는창 초기화 및 yes버튼 초기화
    /// </summary>
    /// <param name="value"></param>
    /// <param name="indexNum"></param>
    private void Set_ReallyBuyBuffWindow_Active(int indexNum)
    {

        // 네 창에 재화 계산되는양 계산 초기화
        int curRuby = GameStatus.inst.Ruby; //현재 소지 루비
        useRutyTemp = RubyPrice.inst.Get_buffRubyPrice(indexNum); // 사용할 루비
        int leftPrice = curRuby - useRutyTemp; // 잔액

        if (curRuby >= useRutyTemp) //가진 루비가 적을경우
        {
            rubyValueText.text = $"{curRuby}\n-{useRutyTemp}\n\n{leftPrice}";

            //네 버튼 여기서 초기화
            alrimYesOrNoBtn[1].onClick.RemoveAllListeners();

            alrimYesOrNoBtn[1].onClick.AddListener(() =>
            {
                GameStatus.inst.Ruby -= useRutyTemp;  //루비차감
                useRutyTemp = 0;

                BuffContoller.inst.ActiveBuff(indexNum, RubyBuffTime[indexNum]); // 버프주기

                alrimWindow.SetActive(false); //창닫기
                mainWindow.SetActive(false);

                WorldUI_Manager.inst.Set_TextAlrim(MakeAlrimMSG(indexNum, (int)RubyBuffTime[indexNum])); // 알림바 넣어주기
            });

            alrimWindow.SetActive(true);
        }
        else if (curRuby < useRutyTemp) //가진 루비가 적을경우
        {
            noHaveRubyMainWindow.SetActive(true);
        }
    }



    public void viewAdCoolTime(int buffIndexNum)
    {
        viewAdCoolTimer[buffIndexNum] += 15 * 60;
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
        if(viewAdATKBuff > 0 && adBuffBtn.gameObject.activeSelf == false)
        {
            viewAdATKBuff -= Time.deltaTime;
        }
        else if(viewAdATKBuff <= 0)
        {
            viewAdATKBuff = 0;

            if(adBuffBtn.gameObject.activeSelf == false)
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

    public string MakeAlrimMSG(int indexNum, int Time)
    {

        switch (indexNum)
        {
            case 0:
                return $"공격력 버프가 {Time}분 활성화 되었습니다.";


            case 1:
                return $"골드획득 버프가 {Time}분 활성화 되었습니다.";


            case 2:
                return $"이동속도 버프가 {Time}분 활성화 되었습니다.";

        }

        return null;
    }


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

