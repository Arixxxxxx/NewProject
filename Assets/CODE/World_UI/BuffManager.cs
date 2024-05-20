using System.Linq;
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
    Animator mainWindowAnim;


    GameObject frontUIRef;
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
 

    GameObject worldUIRef, buffLayOutRef;

    //ȭ�鿡 �ӽ÷� �ߴ� ���� ���ݷ����� ��ư
    Button adBuffBtn;
    float viewAdATKBuff;


    int useRutyTemp;
    // �����ư
    GameObject newbiebuffIcon;
    GameObject newbiebuffIconActive;

    
    // ȭ����� Reward ����
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

        adBuffBtn = worldUIRef.transform.Find("ADBuff").GetComponent<Button>(); // �ΰ��� �˾� ����������


        //�⺻ ��������â
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
        newbiebuffIcon = worldUIRef.transform.Find("StageUI/Buff/NewBie").gameObject;
        newbiebuffIconActive = newbiebuffIcon.transform.Find("Active").gameObject;

        uiWindowTimeInfo[0].text = $"+{adBuffTime[0]}M";
        uiWindowTimeInfo[1].text = $"+{RubyBuffTime[0]}M";
        uiWindowTimeInfo[2].text = $"+{adBuffTime[1]}M";
        uiWindowTimeInfo[3].text = $"+{RubyBuffTime[1]}M";
        uiWindowTimeInfo[4].text = $"+{adBuffTime[2]}M";
        uiWindowTimeInfo[5].text = $"+{RubyBuffTime[2]}M";

        buffstringText[0] = $"���ݷ� 2�� ���� {AdbuffTime(0)} ��";
        buffstringText[1] = $"�̵��ӵ� 2�� ���� {AdbuffTime(1)} ��";
        buffstringText[2] = $"���ȹ�淮 2�� ���� {AdbuffTime(2)} ��";
        buffstringText[3] = $" ���ݷ� 3�� ���� {AdbuffTime(3)} ��";
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
        exitBtn.onClick.AddListener(() => { Buff_UI_Active(false); });

        // 1. ���� ���� 
        viewAdBtn[0].onClick.AddListener(() => AdBuffActive(0));
        viewAdBtn[1].onClick.AddListener(() => AdBuffActive(1));
        viewAdBtn[2].onClick.AddListener(() => AdBuffActive(2));

        // 2. ��� ����

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

    
        // �ΰ��� ���� ���� ���ݷ����� ��ư
        adBuffBtn.onClick.AddListener(() => ADViewManager.inst.SampleAD_Active_Funtion(() =>
        {
            adBuffBtn.gameObject.SetActive(false);
            BuffContoller.inst.ActiveBuff(3, AdbuffTime(3)); //����Ȱ��ȭ
            viewAdATKBuff += Random.Range(2 * 60f, 3 * 60); // Re PopUp CoomTime
            WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.BuffIMG(3), buffstringText[3]);
        }));

    }

    /// <summary>
    /// ������ On/Off
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

    // �ִϸ��̼� ����
    public void MainWindow_Active_False() => mainWindow.SetActive(false);

    private void RubyPayBtnInit(int value)
    {
        BuffContoller.inst.ActiveBuff(value, RubyBuffTime[value]);
        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.BuffIMG(value), RewardStringMaker(value));
        mainWindow.SetActive(false);
    }

    //������ �ִ� ���� string �迭 �����ͼ� ��Ȱ��
    private string RewardStringMaker(int value)
    {
        int findindex = buffstringText[value].IndexOf('��');
        return new string(buffstringText[value].Substring(0,findindex + 1).ToArray()) + $" {RubyBuffTime[value]} ��";
    }


    private void AdBuffActive(int value)
    {
        ADViewManager.inst.SampleAD_Active_Funtion(() =>
        {
            BuffContoller.inst.ActiveBuff(value, AdbuffTime(value)); //����Ȱ��ȭ
            AddBuffCoolTime(value, (int)AdbuffTime(value)); // ��Ÿ�� �ð��߰�
            mainWindow.gameObject.SetActive(false);
            //Set_TextAlrim(MakeAlrimMSG(0, (int)AdbuffTime(0))); // �˸�����
            WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.BuffIMG(value), buffstringText[value]);
        });
    }

    /// <summary>
    /// ���� Ȱ��ȭ
    /// </summary>
    /// <param name="type">0��/1�̼�/2���/3���Ѱ��ݷ�</param>
    /// <param name="Time">�� ����</param>
    /// <param name="text">ȹ��â</param>
    public void ActiveBuff(int type, int Time, string text)
    {
        if (Time == 0) { return; }

        BuffContoller.inst.ActiveBuff(type, Time); //����Ȱ��ȭ
        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.BuffIMG(type), text);
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
    /// ���� ��Ÿ�ӿ� �ð��ֱ�
    /// </summary>
    /// <param name="index">���� �ε��� ��ȣ</param>
    /// <param name="Time">�ð�(��)</param>
    public void AddBuffCoolTime(int index, int Time) => viewAdCoolTimer[index] = Time * 60;


    /// <summary>
    /// ���� ���� �����ð� Return
    /// </summary>
    /// <param name="buffIndexNum"></param>
    public void viewAdCoolTime(int buffIndexNum) => viewAdCoolTimer[buffIndexNum] += 15 * 60;


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

