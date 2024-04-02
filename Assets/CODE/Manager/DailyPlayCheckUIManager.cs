using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyPlayCheckUIManager : MonoBehaviour
{
    public static DailyPlayCheckUIManager inst;

    [SerializeField] Sprite[] adBtnSprite;
    
    GameObject worldFrontRef, dailyCheckObjRef, dailyWindowRef, layoutRef;
    GameObject[] iconBG;
    int iconCount;

    Button xBtn;

    //�ޱ� ��ư Ȥ�� ����
    [SerializeField]
    GameObject[] GetBtn = new GameObject[2];
    TMP_Text mainTaxt;

    //
    Button adViewAndGetRubyBtn;

    

    void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        // ���̶�Ű Ref
        worldFrontRef = GameObject.Find("---[FrontUICanvas]").gameObject;
        dailyCheckObjRef = worldFrontRef.transform.Find("DailyCheck").gameObject;
        dailyWindowRef = dailyCheckObjRef.transform.Find("Window").gameObject;
        layoutRef = dailyWindowRef.transform.Find("Lyaout").gameObject;

        //�ޱ��ư ��
        GetBtn[0] = dailyWindowRef.transform.Find("TextLayOut/NoGet").gameObject;
        GetBtn[1] = dailyWindowRef.transform.Find("TextLayOut/Got").gameObject;

        mainTaxt = GetBtn[0].GetComponent<TMP_Text>();

        // ���
        adViewAndGetRubyBtn = dailyWindowRef.transform.Find("ShowADBtn").GetComponent<Button>();

        LayoutIconBGInit(); // ������ ��׶��� �ϴ� �� ����

        xBtn = dailyWindowRef.transform.Find("Title/X_Btn").GetComponent<Button>();

        BtnInIt();
    }
    void Start()
    {
        IconRedSimballInit();
    }

    private void Update()
    {
        
    }
    private void BtnInIt()
    {
        xBtn.onClick.AddListener(() => dailyCheckObjRef.SetActive(false));
        
        
        adViewAndGetRubyBtn.onClick.AddListener(() => //���� ��ư
        {
            WorldUI_Manager.inst.SampleAD_Get_Currency(0, 100); //��ȭ�ֱ�
            adViewAndGetRubyBtn.GetComponent<Image>().sprite = adBtnSprite[1]; //��������Ʈ ��ü
            adViewAndGetRubyBtn.onClick.RemoveAllListeners(); // ��ư��� -> â����� �ٲ�
            adViewAndGetRubyBtn.onClick.AddListener(() => MainWindow_Acitve(false));
        });
    }

    private void LayoutIconBGInit()
    {
        iconCount = layoutRef.transform.childCount;
        iconBG = new GameObject[iconCount];

        for (int index=0; index <iconCount; index++)
        {
            iconBG[index] = layoutRef.transform.GetChild(index).Find("BG").gameObject;
            iconBG[index].SetActive(true);
        }
    }
      

    /// <summary>
    /// ���ָ鼭 �ʱ�ȭ
    /// </summary>
    /// <param name="value"></param>
    public void MainWindow_Acitve(bool value)
    {
        if (value == true && GameStatus.inst.GotDilayPlayGiftCount < 20)
        {
            LayOutInit();
        }

        dailyCheckObjRef.SetActive(value);
    }


    // ���ӽ���� �����ܿ� �����ɺ� ������ִ� �Լ�
    public void IconRedSimballInit()
    {
        int[] LastGetGiftDay = GameStatus.inst.GetGiftDay;

        if (LastGetGiftDay.Sum() == 0 && GameStatus.inst.GotDilayPlayGiftCount == 0)
        {
            WorldUI_Manager.inst.OnEnableRedSimball(1,true);
        }
        else if (LastGetGiftDay.Sum() != 0 && GameStatus.inst.GotDilayPlayGiftCount > 0)
        {
            //������������
            if (LastGetGiftDay[0] < DateTime.Now.Year) //���� �ð� üũ
            {
                WorldUI_Manager.inst.OnEnableRedSimball(1, true);
            }
            else if (LastGetGiftDay[1] < DateTime.Now.Month)
            {
                WorldUI_Manager.inst.OnEnableRedSimball(1, true);
            }
            else if (LastGetGiftDay[2] < DateTime.Now.Day)
            {
                WorldUI_Manager.inst.OnEnableRedSimball(1, true);
            }
        }
    }

    // �⼮üũ �ʱ�ȭ ���¥���� Ȯ��
    public void LayOutInit()
    {
        int[] LastGetGiftDay = GameStatus.inst.GetGiftDay;

        if (LastGetGiftDay.Sum() == 0 && GameStatus.inst.GotDilayPlayGiftCount == 0)
        {
            IconInit();   // ���� �� �׳� ������
        }
        else if(LastGetGiftDay.Sum() != 0 && GameStatus.inst.GotDilayPlayGiftCount > 0)
        {
            //������������
            if (LastGetGiftDay[0] < DateTime.Now.Year) //���� �ð� üũ
            {
                IconInit();
                GetBtnAcitve(true);
            }
            else if (LastGetGiftDay[1] < DateTime.Now.Month)
            {
                IconInit();
                GetBtnAcitve(true);
            }
            else if (LastGetGiftDay[2] < DateTime.Now.Day)
            {
                IconInit();
                GetBtnAcitve(true);
            }
        }
    }



    private void IconInit()
    {
      
        //��ư �ʱ�ȭ �� (������ ���� ���ְ�, ������ �մٸ� ����)
        for (int index = 0; index < GameStatus.inst.GotDilayPlayGiftCount; index++)
        {
            layoutRef.transform.GetChild(index).Find("Check").gameObject.SetActive(true);
        }

        layoutRef.transform.GetChild(GameStatus.inst.GotDilayPlayGiftCount).Find("BG").gameObject.SetActive(false);

        //��� ��� (������� �ؽ�Ʈ���� ����)
        int value = int.Parse(layoutRef.transform.GetChild(GameStatus.inst.GotDilayPlayGiftCount).Find("CountText").GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());
        int checkDay = int.Parse(layoutRef.transform.GetChild(GameStatus.inst.GotDilayPlayGiftCount).Find("NumberText").GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());

        // N���� ����ޱ� ~~ �ؽ�Ʈ �ʱ�ȭ
        mainTaxt.text = $"  < {checkDay}���� > �⼮üũ ����ޱ�\r\n - ������ <color=green>������</color>���� �߼۵˴ϴ�.";

        //�������� ���
        int[] NowDate = new int[3];
        NowDate[0] = DateTime.Now.Year;
        NowDate[1] = DateTime.Now.Month;
        NowDate[2]  = DateTime.Now.Day;

        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.RemoveAllListeners();
        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            LetterManager.inst.MakeLetter(0, "����GM", $"�⼮üũ {checkDay}���� ����", value); // ���� ���� ȹ��
            GetIconChanger(GameStatus.inst.GotDilayPlayGiftCount); // ������ ����ó��
            GameStatus.inst.GetGiftDay = NowDate; // ���� ������Ʈ
            GameStatus.inst.GotDilayPlayGiftCount++; // ���� ī��Ʈ �÷���
            WorldUI_Manager.inst.OnEnableRedSimball(1, false); // �����ɺ� ���ֱ�
            GetBtnAcitve(false); // ��ư ��Ȱ��ȭ
        });


    }


    /// <summary>
    /// �������� ���� �� ���� IMGó�����ִ� �Լ�
    /// </summary>
    /// <param name="value"></param>
    private void GetIconChanger(int value)
    {
        layoutRef.transform.GetChild(value).Find("BG").gameObject.SetActive(true);
        layoutRef.transform.GetChild(value).Find("Check").gameObject.SetActive(true);
    }

    /// <summary>
    /// �ϴܺ� ��ư
    /// </summary>
    /// <param name="value"></param>
    public void GetBtnAcitve(bool value)
    {
        if(value == true)
        {
            GetBtn[0].gameObject.SetActive(true);
            GetBtn[1].gameObject.SetActive(false);
        }
        else
        {
            GetBtn[0].gameObject.SetActive(false);
            GetBtn[1].gameObject.SetActive(true);
        }
    }

}
