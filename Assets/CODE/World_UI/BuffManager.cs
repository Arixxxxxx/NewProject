using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public static BuffManager inst;

    [Header("# Input Buff Time (Min) / View ToolTip")]

    [Space]
    [SerializeField][Tooltip("0�� : UIâ ���� ATK \n1�� : UI â ���� �̵��ӵ�\n2��: UIâ ���� ���ȹ�淮\n3��: �ΰ��� �˾����� ���ݷ�// 4�� �������")] float[] adBuffTime; 
    public float AdbuffTime(int index) => adBuffTime[index];

    [SerializeField][Tooltip("0�� : UIâ ��� ATK \n1�� : UI â ��� �̵��ӵ�\n2��: UIâ ��� ���ȹ�淮")] float[] RubyBuffTime;

    GameObject mainWindow;
    GameObject buffWindow;
    GameObject buffSelectUIWindow;

    Button exitBtn;
    int btnCount;
    Button[] viewAdBtn;
    // AD ��Ÿ�Ӱ���
    float[] viewAdCoolTimer;
    GameObject[] btnAdActiveIMG;
    TMP_Text[] adCoolTimeText;
    TMP_Text[] buffIconBottomTime;

    TMP_Text[] uiWindowTimeInfo = new TMP_Text[6]; // 0,1����, 2,3 �̼�,, 4,5���   AD : Ruby

    // ��� ����â����
    Button[] useRubyBtn;
    TMP_Text[] rubyPrice;
    //���� �첫�� �����â
    GameObject alrimWindow;
    TMP_Text rubyValueText;
    Button[] alrimYesOrNoBtn = new Button[2];
    // ��� ������ �ߴ�â
    GameObject noHaveRubyMainWindow;
    Button[] noHaveRubyWindowYesOrNoBtn = new Button[2];

    GameObject worldUI;

    //ȭ�鿡 �ӽ÷� �ߴ� ���� ���ݷ����� ��ư
    Button adBuffBtn;
    float viewAdATKBuff;


    int useRutyTemp;
    // �����ư
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
        adBuffBtn = worldUI.transform.Find("StageUI/ADBuff").GetComponent<Button>(); // �ΰ��� �˾� ����������

        buffWindow = GameObject.Find("---[FrontUICanvas]").gameObject;

        //�⺻ ��������â
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

        //�����â 
        alrimWindow = buffWindow.transform.Find("Buff_Window/Alrim_Window").gameObject;
        rubyValueText = alrimWindow.transform.Find("Title/RubyValue_Text").GetComponent<TMP_Text>();
        alrimYesOrNoBtn[0] = alrimWindow.transform.Find("Title/NoBtn").GetComponent<Button>();
        alrimYesOrNoBtn[1] = alrimWindow.transform.Find("Title/YesBtn").GetComponent<Button>();

        // ������ â
        noHaveRubyMainWindow = mainWindow.transform.Find("NoHaveRuby").gameObject;
        noHaveRubyWindowYesOrNoBtn[0] = noHaveRubyMainWindow.transform.Find("Title/NoBtn").GetComponent<Button>();
        noHaveRubyWindowYesOrNoBtn[1] = noHaveRubyMainWindow.transform.Find("Title/YesBtn").GetComponent<Button>();

        //ATK �ʱ�ȭ
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

        //���� ����
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
        // UII ��� ����Text �ʱ�ȭ
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

        // �ΰ��� ���� ���� ���ݷ����� ��ư
        adBuffBtn.onClick.AddListener(() => {

            adBuffBtn.gameObject.SetActive(false);
            WorldUI_Manager.inst.SampleADBuff("buff", 3);
            viewAdATKBuff += 15f; // Re PopUp CoomTime

        });

        // ���� ���Ź�ư => ���� �����Ͻò��ϱ� â���� ����
        useRubyBtn[0].onClick.AddListener(() => Set_ReallyBuyBuffWindow_Active(0));
        useRubyBtn[1].onClick.AddListener(() => Set_ReallyBuyBuffWindow_Active(1));
        useRubyBtn[2].onClick.AddListener(() => Set_ReallyBuyBuffWindow_Active(2));

        alrimYesOrNoBtn[0].onClick.AddListener(() => alrimWindow.SetActive(false));

        //���� ���������� ���� â��ư
        noHaveRubyWindowYesOrNoBtn[0].onClick.AddListener(() => noHaveRubyMainWindow.SetActive(false)); // No ��ư
        noHaveRubyWindowYesOrNoBtn[1].onClick.AddListener(() =>  // Yes��ư
        {
            noHaveRubyMainWindow.SetActive(false);
            mainWindow.SetActive(false);
            //����â���� �̵�~~
        });

    }


    /// <summary>
    /// ���� �����Ͻǲ��� �����â �ʱ�ȭ �� yes��ư �ʱ�ȭ
    /// </summary>
    /// <param name="value"></param>
    /// <param name="indexNum"></param>
    private void Set_ReallyBuyBuffWindow_Active(int indexNum)
    {

        // �� â�� ��ȭ ���Ǵ¾� ��� �ʱ�ȭ
        int curRuby = GameStatus.inst.Ruby; //���� ���� ���
        useRutyTemp = RubyPrice.inst.Get_buffRubyPrice(indexNum); // ����� ���
        int leftPrice = curRuby - useRutyTemp; // �ܾ�

        if (curRuby >= useRutyTemp) //���� ��� �������
        {
            rubyValueText.text = $"{curRuby}\n-{useRutyTemp}\n\n{leftPrice}";

            //�� ��ư ���⼭ �ʱ�ȭ
            alrimYesOrNoBtn[1].onClick.RemoveAllListeners();

            alrimYesOrNoBtn[1].onClick.AddListener(() =>
            {
                GameStatus.inst.Ruby -= useRutyTemp;  //�������
                useRutyTemp = 0;

                BuffContoller.inst.ActiveBuff(indexNum, RubyBuffTime[indexNum]); // �����ֱ�

                alrimWindow.SetActive(false); //â�ݱ�
                mainWindow.SetActive(false);

                WorldUI_Manager.inst.Set_TextAlrim(MakeAlrimMSG(indexNum, (int)RubyBuffTime[indexNum])); // �˸��� �־��ֱ�
            });

            alrimWindow.SetActive(true);
        }
        else if (curRuby < useRutyTemp) //���� ��� �������
        {
            noHaveRubyMainWindow.SetActive(true);
        }
    }



    public void viewAdCoolTime(int buffIndexNum)
    {
        viewAdCoolTimer[buffIndexNum] += 15 * 60;
    }


    // UI â ���� �ð� Text ������Ʈ���ִ� �Լ�
    public void CheakCoomTime(int index)
    {
        // ���� �����ð� ����
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
            buffIconBottomTime[index].text = $"���� �ð�: {min}�� {sec}��";
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
    /// �ѹ� ������
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
    /// ���� ��Ÿ�ӿ� �ð��ֱ�
    /// </summary>
    /// <param name="index">���� �ε��� ��ȣ</param>
    /// <param name="Time">�ð�(��)</param>
    public void AddBuffCoolTime(int index, int Time) => viewAdCoolTimer[index] = Time * 60;

    public string MakeAlrimMSG(int indexNum, int Time)
    {

        switch (indexNum)
        {
            case 0:
                return $"���ݷ� ������ {Time}�� Ȱ��ȭ �Ǿ����ϴ�.";


            case 1:
                return $"���ȹ�� ������ {Time}�� Ȱ��ȭ �Ǿ����ϴ�.";


            case 2:
                return $"�̵��ӵ� ������ {Time}�� Ȱ��ȭ �Ǿ����ϴ�.";

        }

        return null;
    }


    /// <summary>
    /// �ΰ��� ����â ��������� ���� üũ
    /// </summary>
    /// <param name="value"></param>
    public void NewBieBuffActive(bool value)
    {
        newbiebuffIcon.SetActive(value);
        newbiebuffIconActive.SetActive(value);
    }
}

