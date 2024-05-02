using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Newbie_Content : MonoBehaviour
{
    public static Newbie_Content inst;

    [Header("# Input IMGBox Layout <Color=yellow>( Sprite File )</Color>")]
    [Space]
    [SerializeField][Tooltip("0 = Ȱ�� / 1 = ��Ȱ�� ")] Sprite[] imgBoxSideSprite;
    [Header("# Input GetItemLine  <Color=green>( Cyan Color )</Color>")]
    [Space]
    [SerializeField] Color gotItemColor;

    /// Ref
    GameObject frontUI, newbieWindow, gameWindow, layoutRef;
    Button xBtn;

    int iconLayoutCount = 0;

    TMP_Text mainTaxt;
    // ������ư
    [Tooltip("0 �ޱ� Ȱ��ȭ / 1��Ȱ��ȭ")] GameObject[] GetBtn = new GameObject[2];

    GameObject alrimWindow;
    Image alrimWindowItemIMG;
    Button alrimBtn;

    //�ű����� ���� �ٷΰ��� ��ư
    Button buffViewrBtn;

    // �����ܹڽ� �̹��� �� ���ư��� ��� �̹��� ����
    Image[] iconLayoutIMG;


    Image[] iconRoadLineIMG;
    GameObject[] iconBG;
    GameObject[] checkIcon;

    // ���� ���� ������ ����â
    GameObject buffInfoWindow;
    TMP_Text buffLeftTimeText;
    Button buffinfoBottomBtn;
    private void Start()
    {

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

        frontUI = GameObject.Find("---[FrontUICanvas]").gameObject;
        newbieWindow = frontUI.transform.Find("Newbie").gameObject;
        gameWindow = newbieWindow.transform.Find("Window").gameObject;
        layoutRef = gameWindow.transform.Find("RubyList").gameObject;
        xBtn = gameWindow.transform.Find("Title/X_Btn").GetComponent<Button>();
        xBtn.onClick.AddListener(() => Set_NewbieWindowActive(false));
        buffViewrBtn = gameWindow.transform.Find("TextLayOut_2/NoGet/GetGiftBtn").GetComponent<Button>();
        buffViewrBtn.onClick.AddListener(() => { newbieWindow.gameObject.SetActive(false); NewBieBuffInfoWindowActive(true); });

        iconLayoutCount = layoutRef.transform.childCount;
        iconLayoutIMG = new Image[iconLayoutCount];
        iconRoadLineIMG = new Image[iconLayoutCount];
        checkIcon = new GameObject[iconLayoutCount];
        iconBG = new GameObject[iconLayoutCount];

        mainTaxt = gameWindow.transform.Find("TextLayOut/NoGet").GetComponent<TMP_Text>();

        for (int index = 0; index < iconLayoutCount; index++) // ������ �ڽ�
        {
            iconLayoutIMG[index] = layoutRef.transform.GetChild(index).GetComponent<Image>();
            iconBG[index] = layoutRef.transform.GetChild(index).Find("BG").gameObject;
            checkIcon[index] = layoutRef.transform.GetChild(index).Find("Check").gameObject;
        }

        for (int index = 0; index < gameWindow.transform.Find("Line").transform.childCount; index++) // ��� ����
        {
            iconRoadLineIMG[index] = gameWindow.transform.Find("Line").GetChild(index).GetComponent<Image>();
        }

        GetBtn[0] = gameWindow.transform.Find("TextLayOut/NoGet").gameObject;
        GetBtn[1] = gameWindow.transform.Find("TextLayOut/Got").gameObject;

        alrimWindow = newbieWindow.transform.Find("Alrim").gameObject;
        alrimWindowItemIMG = alrimWindow.transform.Find("Window/Frame_LayOut/IMG_Frame/IMG").GetComponent<Image>();
        alrimBtn = alrimWindow.transform.Find("Window/Button").GetComponent<Button>();
        alrimBtn.onClick.AddListener(() => { alrimWindow.SetActive(false); });


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
                $"  <color=green>   {(BuffContoller.inst.GetBuffTime(4) / 60) / 24}�� {(BuffContoller.inst.GetBuffTime(4) / 60) % 24}�ð� {BuffContoller.inst.GetBuffTime(4) % 60}��</color>";

            buffInfoWindow.SetActive(true);
        }
        else
        {
            buffInfoWindow.SetActive(false);
        }
    }




    /// <summary>
    /// ���񺸻�â ȣ��
    /// </summary>
    /// <param name="value"> true / false </param>
    public void Set_NewbieWindowActive(bool value)
    {
        newbieWindow.SetActive(value);
    }



    /// <summary>
    ///  
    /// </summary>
    /// <param name="value"></param>
    public void NewbieWindow_Init(bool TodayGetReward)
    {
        // ��ư Ȱ��ȭ �� ��Ȱ��ȭ ����
        GetBtnAcitve(!TodayGetReward);

        //���� ������ ���̾ƿ� �ֽ�ȭ
        IconBoxInit();
        
        //��� ��� �� �ؽ�Ʈ �ʱ�ȭ (������� �ؽ�Ʈ���� ����)
        int rubyCount = int.Parse(layoutRef.transform.GetChild(GameStatus.inst.GotNewbieGiftCount).Find("CountText").GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());
        mainTaxt.text = $"  < {GameStatus.inst.GotNewbieGiftCount + 1}���� > �ű����� ����ޱ�\r\n - ������ <color=green>������</color>���� �߼۵˴ϴ�.";

        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.RemoveAllListeners();
        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.AddListener(() => // ������ư��
        {
            LetterManager.inst.MakeLetter(0, "����GM", $"�ű����� {GameStatus.inst.GotNewbieGiftCount + 1}���� ����", rubyCount); // ���� ���� ȹ��
                                                                                                                        //GetIconChanger(GameStatus.inst.GotNewbieGiftCount); // ������ ����ó��
            GameStatus.inst.GotNewbieGiftCount++; // ���� ī��Ʈ �÷���
            GameStatus.inst.TodayGetReward = true; // ����

            IconBoxInit(); // �׸� �缳��
            GetBtnAcitve(false); // ��ư ��Ȱ��ȭ
            ConfirmWindowAcitve(); // ����â Ȱ��ȭ
        });

    }

    //�����ܹڽ� �ֽ�ȭ �Լ�
    private void IconBoxInit()
    {
        int forCount = GameStatus.inst.GotNewbieGiftCount;

        // ������ �Ÿݰ� ���
        for(int index = 0; index < iconBG.Length; index++)
        {
            if(index == forCount)
            {
                iconBG[index]?.SetActive(false);
            }
            else
            {
                iconBG[index]?.SetActive(true);
            }
        }

        // ������ ��� ����
        for (int index = 0; index < forCount; index++)
        {
            if (iconRoadLineIMG[index] != null)
            {
                iconRoadLineIMG[index].color = gotItemColor;
            }
            if (iconRoadLineIMG[index] != null)
            {
                checkIcon[index]?.gameObject.SetActive(true);
            }
        }

        // ������ �ڽ� �̹���
        for (int index = 0; index < forCount + 1; index++)
        {
            if(forCount+1 > iconLayoutIMG.Length) { return; }

            iconLayoutIMG[index].sprite = imgBoxSideSprite[0];
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





    private void GetIconChanger(int value)
    {
        layoutRef.transform.GetChild(value).Find("BG").gameObject.SetActive(true);
        layoutRef.transform.GetChild(value).Find("Check").gameObject.SetActive(true);
    }


}



