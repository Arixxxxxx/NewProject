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
    GameObject frontUI, newbieWindow, gameWindow, layoutRef, worldUiRef;
    Button xBtn;

    int iconLayoutCount = 0;

    TMP_Text mainTaxt;
    // ������ư
    [Tooltip("0 �ޱ� Ȱ��ȭ / 1��Ȱ��ȭ")] GameObject[] GetBtn = new GameObject[2];
    Image bottomBoxIMG;

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
    TMP_Text buffLeftTimeText;


    //�˸� �ɺ�
    GameObject simBall;
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

        worldUiRef = GameManager.inst.WorldUiRef;
        frontUI = GameManager.inst.FrontUiRef;

        newbieWindow = frontUI.transform.Find("Newbie").gameObject;
        gameWindow = newbieWindow.transform.Find("Window").gameObject;
        layoutRef = gameWindow.transform.Find("RubyList").gameObject;
        xBtn = gameWindow.transform.Find("Title/X_Btn").GetComponent<Button>();
        xBtn.onClick.AddListener(() => Set_NewbieWindowActive(false));


        iconLayoutCount = layoutRef.transform.childCount;
        iconLayoutIMG = new Image[iconLayoutCount];
        iconRoadLineIMG = new Image[iconLayoutCount];
        checkIcon = new GameObject[iconLayoutCount];
        iconBG = new GameObject[iconLayoutCount];

        mainTaxt = gameWindow.transform.Find("TextLayOut/Bottom/NoGet").GetComponent<TMP_Text>();

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

        bottomBoxIMG = gameWindow.transform.Find("TextLayOut/Bottom").GetComponent<Image>();
        GetBtn[0] = gameWindow.transform.Find("TextLayOut/Bottom/NoGet").gameObject;
        GetBtn[1] = gameWindow.transform.Find("TextLayOut/Bottom/Got").gameObject;

        alrimWindow = newbieWindow.transform.Find("Alrim").gameObject;
        alrimWindowItemIMG = alrimWindow.transform.Find("Window/Frame_LayOut/IMG_Frame/IMG").GetComponent<Image>();
        alrimBtn = alrimWindow.transform.Find("Window/Button").GetComponent<Button>();
        alrimBtn.onClick.AddListener(() => { alrimWindow.SetActive(false); });


        buffLeftTimeText = gameWindow.transform.Find("TextLayOut/LeftTime/TimeText").GetComponent<TMP_Text>();


        simBall = worldUiRef.transform.Find("StageUI/NewBie/SimBall").gameObject;
    }


    /// <summary>
    /// ���񺸻�â ȣ��
    /// </summary>
    /// <param name="value"> true / false </param>
    public void Set_NewbieWindowActive(bool value)
    {
        newbieWindow.SetActive(value);
        buffLeftTimeText.text = $"�ű����� ���� �����ð� : " +
              $"<color=green>{(BuffContoller.inst.GetBuffTime(4) / 60) / 24}�� {(BuffContoller.inst.GetBuffTime(4) / 60) % 24}�ð� {BuffContoller.inst.GetBuffTime(4) % 60}��</color>";
    }



    /// <summary>
    ///  ���� ������ �ڽ� / �ʱ�ȭ
    /// </summary>
    /// <param name="value"></param>
    public void NewbieWindow_Init(bool TodayGetReward)
    {
        // ��ư Ȱ��ȭ �� ��Ȱ��ȭ ����
        GetBtnAcitve(!TodayGetReward);
        simBall.SetActive(!TodayGetReward);
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
            GameStatus.inst.TodayGetNewbie_Reward = true; // ����

            simBall.SetActive(false);
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
        for (int index = 0; index < iconBG.Length; index++)
        {
            if (index == forCount)
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
            if (forCount + 1 > iconLayoutIMG.Length) { return; }

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
            bottomBoxIMG.color = Color.white;
        }
        else
        {
            GetBtn[0].gameObject.SetActive(false);
            GetBtn[1].gameObject.SetActive(true);
            bottomBoxIMG.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }





    private void GetIconChanger(int value)
    {
        layoutRef.transform.GetChild(value).Find("BG").gameObject.SetActive(true);
        layoutRef.transform.GetChild(value).Find("Check").gameObject.SetActive(true);
    }

    /// <summary>
    /// �˸� ������ ��Ƽ��
    /// </summary>
    /// <param name="value"></param>
    public void Active_AlrimSimBall(bool value) => simBall.SetActive(value);



}



