using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Newbie_Content : MonoBehaviour
{
    public static Newbie_Content inst;

    [SerializeField][Tooltip("0 = Ȱ�� / 1 = ��Ȱ�� ")] Sprite[] imgBoxSideSprite;
    [SerializeField] Color gotItemColor;

    /// Ref
    GameObject frontUI, newbieWindow, gameWindow, layoutRef;
    Button xBtn;

    int iconLayoutCount = 0;

    TMP_Text mainTaxt;
    // ������ư
    GameObject[] GetBtn = new GameObject[2];
    GameObject alrimWindow;
    Image alrimWindowItemIMG;
    Button alrimBtn;

    //�ű����� ���� �ٷΰ��� ��ư
    Button buffViewrBtn;

    // �����ܹڽ� �̹��� �� ���ư��� ��� �̹��� ����
    Image[] iconLayoutIMG;


    Image[] iconRoadLineIMG;
    GameObject[] iconBG;

    // ���� ���� ������ ����â
    GameObject buffInfoWindow;
    TMP_Text buffLeftTimeText;
    Button buffinfoBottomBtn;
    

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

        frontUI = GameObject.Find("---[FrontUICanvas]").gameObject;
        newbieWindow = frontUI.transform.Find("Newbie").gameObject;
        gameWindow = newbieWindow.transform.Find("Window").gameObject;
        layoutRef = gameWindow.transform.Find("Lyaout").gameObject;
        xBtn = gameWindow.transform.Find("Title/X_Btn").GetComponent<Button>();
        xBtn.onClick.AddListener(() => Set_NewbieWindowActive(false));
        buffViewrBtn = gameWindow.transform.Find("TextLayOut_2/NoGet/GetGiftBtn").GetComponent<Button>();
        buffViewrBtn.onClick.AddListener(() => { newbieWindow.gameObject.SetActive(false); NewBieBuffInfoWindowActive(true); });

        iconLayoutCount = layoutRef.transform.childCount;
        iconLayoutIMG = new Image[iconLayoutCount];
        iconRoadLineIMG = new Image[iconLayoutCount - 1];

        mainTaxt = gameWindow.transform.Find("TextLayOut/NoGet").GetComponent<TMP_Text>();

        for (int index = 0; index < iconLayoutCount; index++) // ������ �ڽ�
        {
            iconLayoutIMG[index] = layoutRef.transform.GetChild(index).GetComponent<Image>();
        }

        for (int index = 0; index < iconLayoutCount - 1; index++) // ��� ����
        {
            iconRoadLineIMG[index] = gameWindow.transform.Find("Line").GetChild(index).GetComponent<Image>();
        }

        GetBtn[0] = gameWindow.transform.Find("TextLayOut/NoGet").gameObject;
        GetBtn[1] = gameWindow.transform.Find("TextLayOut/Got").gameObject;

        alrimWindow = newbieWindow.transform.Find("Alrim").gameObject;
        alrimWindowItemIMG = alrimWindow.transform.Find("Window/Frame_LayOut/IMG_Frame/IMG").GetComponent<Image>();
        alrimBtn = alrimWindow.transform.Find("Window/Button").GetComponent<Button>();
        alrimBtn.onClick.AddListener(() => { alrimWindow.SetActive(false); });

        LayoutIconBGInit(); // ��� BG ��������Ʈ �Ÿݰ� 

        //���� ������Ŭ���� ����â
        buffInfoWindow = frontUI.transform.Find("NewbieBtnInfo").gameObject;
        buffLeftTimeText = buffInfoWindow.transform.Find("Window/TextLayOut/NoGet").GetComponent<TMP_Text>();
        buffinfoBottomBtn = buffLeftTimeText.transform.GetComponentInChildren<Button>();
        buffinfoBottomBtn.onClick.AddListener(() => buffInfoWindow.gameObject.SetActive(false));
    }

    /// <summary>
    /// ���� ���� ������ Ŭ���� ����â  �ʱ�ȭ
    /// </summary>
    /// <param name="value"> On / Off </param>
    public void NewBieBuffInfoWindowActive(bool value)
    {
        if (value == true)
        {
            buffLeftTimeText.text = $"- ���ʰ��� �� 7�ϰ� ����˴ϴ�.\n- ���� ������� �����ð�\n   " +
                $"  <color=green>   {(BuffContoller.inst.GetBuffTime(4)/60) / 24}�� {(BuffContoller.inst.GetBuffTime(4) / 60) % 24}�ð� {BuffContoller.inst.GetBuffTime(4) % 60}��</color>";

            buffInfoWindow.SetActive(true);
        }
        else
        {
            buffInfoWindow.SetActive(false);
        }
    }

    void Start()
    {
        IconRedSimballInit();
      
        
    }



    public void Set_NewbieWindowActive(bool value)
    {
        if (value == true && GameStatus.inst.GotNewbieGiftCount < 7)
        {
            // �ʱ�ȭ�κ� �׵θ� �����Ŀ� �ٽ� 
            for (int index = 0; index < iconLayoutCount; index++)
            {
                iconLayoutIMG[index].sprite = imgBoxSideSprite[1];
            }
            LayOutInit();
            IconBoxInit();
        }
        newbieWindow.SetActive(value);
    }


    private void LayoutIconBGInit()
    {
        int iconCount = layoutRef.transform.childCount;
        iconBG = new GameObject[iconCount];

        for (int index = 0; index < iconCount; index++)
        {
            iconBG[index] = layoutRef.transform.GetChild(index).Find("BG").gameObject;
            iconBG[index].SetActive(true);
        }
    }


    public void LayOutInit()
    {
        int[] LastGetGiftDay = GameStatus.inst.GetNewbieGiftDay; // ���������� ������� �Ͻ� ������

        if (LastGetGiftDay.Sum() == 0 && GameStatus.inst.GotNewbieGiftCount == 0)
        {
            IconInit();   // ���� �� �׳� ������
        }
        else if (LastGetGiftDay.Sum() != 0 && GameStatus.inst.GotNewbieGiftCount > 0)
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
        if (GameStatus.inst.GotNewbieGiftCount < layoutRef.transform.childCount)
        {
            IconBoxInit();
            layoutRef.transform.GetChild(GameStatus.inst.GotNewbieGiftCount).Find("BG").gameObject.SetActive(false);
        }

        //��� ��� (������� �ؽ�Ʈ���� ����)
        int value = int.Parse(layoutRef.transform.GetChild(GameStatus.inst.GotNewbieGiftCount).Find("CountText").GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());
        int checkDay = int.Parse(gameWindow.transform.Find("Box").GetChild(GameStatus.inst.GotNewbieGiftCount).GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());

        //Sprite ItemIMG = layoutRef.transform.GetChild(GameStatus.inst.GotNewbieGiftCount).Find("Image").GetComponent<Image>().sprite;
        // ���߿� �˸�â�� �̹��� ��ﲨ�� �ٽ� �츮���

        // N���� ����ޱ� ~~ �ؽ�Ʈ �ʱ�ȭ
        mainTaxt.text = $"  < {checkDay}���� > �ű����� ����ޱ�\r\n - ������ <color=green>������</color>���� �߼۵˴ϴ�.";

        //�������� ���
        int[] NowDate = new int[3];
        NowDate[0] = DateTime.Now.Year;
        NowDate[1] = DateTime.Now.Month;
        NowDate[2] = DateTime.Now.Day;

        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.RemoveAllListeners();
        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.AddListener(() => // ������ư��
        {
            LetterManager.inst.MakeLetter(0, "����GM", $"�ű����� {checkDay}���� ����", value); // ���� ���� ȹ��
            GetIconChanger(GameStatus.inst.GotNewbieGiftCount); // ������ ����ó��
            GameStatus.inst.GetNewbieGiftDay = NowDate; // ���� ������Ʈ
            GameStatus.inst.GotNewbieGiftCount++; // ���� ī��Ʈ �÷���
            WorldUI_Manager.inst.OnEnableRedSimball(2, false); // �����ɺ� ���ֱ�

            if (GameStatus.inst.GotNewbieGiftCount < layoutRef.transform.childCount)
            {
                layoutRef.transform.GetChild(GameStatus.inst.GotNewbieGiftCount).Find("BG").gameObject.SetActive(false);
                IconBoxInit();
            }


            GetBtnAcitve(false); // ��ư ��Ȱ��ȭ
            ConfirmWindowAcitve();

        });

    }

    //�����ܹڽ� �ֽ�ȭ �Լ�
    private void IconBoxInit()
    {
        // ������ ��� ����
        for (int index = 0; index < GameStatus.inst.GotNewbieGiftCount; index++) 
        {
            layoutRef.transform.GetChild(index).Find("Check").gameObject.SetActive(true);
            iconRoadLineIMG[index].color = gotItemColor; 
        }

        // ������ �ڽ� �̹���
        for (int index = 0; index < GameStatus.inst.GotNewbieGiftCount+1; index++)
        {
            if (iconLayoutIMG[index] != null)
            {
                iconLayoutIMG[index].sprite = imgBoxSideSprite[0];
            }
        }
    }

    private void ConfirmWindowAcitve()
    {
        alrimWindow.SetActive(true);
    }

    public void GetBtnAcitve(bool value)
    {
        if (value == true)
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


    public void IconRedSimballInit() // �����ɺ� Ȱ��ȭ
    {
        int[] LastGetGiftDay = GameStatus.inst.GetGiftDay;

        if (LastGetGiftDay.Sum() == 0 && GameStatus.inst.GotNewbieGiftCount == 0)
        {
            WorldUI_Manager.inst.OnEnableRedSimball(2, true);
        }
        else if (LastGetGiftDay.Sum() != 0 && GameStatus.inst.GotNewbieGiftCount > 0)
        {
            //������������
            if (LastGetGiftDay[0] < DateTime.Now.Year) //���� �ð� üũ
            {
                WorldUI_Manager.inst.OnEnableRedSimball(2, true);
            }
            else if (LastGetGiftDay[1] < DateTime.Now.Month)
            {
                WorldUI_Manager.inst.OnEnableRedSimball(2, true);
            }
            else if (LastGetGiftDay[2] < DateTime.Now.Day)
            {
                WorldUI_Manager.inst.OnEnableRedSimball(2, true);
            }
        }
    }


    private void GetIconChanger(int value)
    {
        layoutRef.transform.GetChild(value).Find("BG").gameObject.SetActive(true);
        layoutRef.transform.GetChild(value).Find("Check").gameObject.SetActive(true);
    }

    
}



