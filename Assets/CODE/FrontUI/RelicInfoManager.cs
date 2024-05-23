using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicInfoManager : MonoBehaviour
{
    public static RelicInfoManager inst;

    int totalRelicCount = 0;
    string[] relicNameAndtext =
    {
        "������ǰ�=(�Ϲ�)��� : �Ϲ� ���ݷ� ����",
        "���� ����ǥ=(�Ϲ�)��� : ġ��Ÿ Ȯ�� ����",
        "Ȳ�� �䰭=(�Ϲ�)��� : �ʴ� ��� ȹ�淮 ����",
        "��ȭ�� ���� ����=(����)��� : ġ��Ÿ ����� ����",
        "ȣ��ǥ ����=(����)��� : �ǹ�Ÿ�� �ð� ����",
        "������ ����������=(����)��� : ����Ʈ ���� ����",
        "����ũ�� ����=(����)��� : ���� ���� ����",
        "�Ǵ��� �ձ�=(����)��� : ���� �ӵ� ����",
        "���� �ູ=(����)��� : ȯ���� ���޵Ǵ� �� ����",
        "Ȳ�� �׵�=(����)��� : �� óġ�� ���ȹ�淮 ����",
    };

    [SerializeField]
    int[] haveCount = new int[3];

    [SerializeField] Sprite[] relicicon_Outline;
    public Sprite Get_RelicIcon_OutLine_Sprite(int index) => relicicon_Outline[index];

    [SerializeField] TMP_ColorGradient[] typeGradient;

    GameObject frontUIRef, relicInfo_MainRef;
    GameObject epicEffectRef, legendEffectRef;
    Button xBtn;

    Image itemCase, mainItemIMG;
    TMP_Text itemName, itemInfo, haveText;
    Animator imgAnim;
    // ���� �ϴܺ� ��ư��
    GameObject relicBtnViewrRef, normalContentRef, epicContentRef, lengedContentRef;

    //�߹� ��ư (3��)
    GameObject[] ViewrRef;
    Button[] middleBtn = new Button[3];
    Image[] middleBtnImg = new Image[3];

    RelicInfo_Prefbas[] bottomRelicNormalBtn;
    RelicInfo_Prefbas[] bottomRelicEpicBtn;
    RelicInfo_Prefbas[] bottomRelicLegendBtn;

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

        frontUIRef = GameManager.inst.FrontUiRef;
        relicInfo_MainRef = frontUIRef.transform.Find("Relic_Info").gameObject;
        xBtn = relicInfo_MainRef.transform.Find("Window/Title/X_Btn").GetComponent<Button>();
        xBtn.onClick.AddListener(() => Set_RelicDogamActive(false));

        //��� ���
        itemCase = relicInfo_MainRef.transform.Find("Window/ViewBG").GetComponent<Image>(); // �ƿ�����
        mainItemIMG = itemCase.transform.Find("ItemIMG").GetComponent<Image>();
        imgAnim = mainItemIMG.GetComponent<Animator>();
        epicEffectRef = itemCase.transform.Find("Epic_Effect").gameObject; // ���� ����Ʈ
        legendEffectRef = itemCase.transform.Find("Legend_Effect").gameObject; // �������� ����Ʈ
        itemName = itemCase.transform.Find("TitleBar/ItemName").GetComponent<TMP_Text>(); // ������ ���� (���)
        itemInfo = itemCase.transform.Find("ItemInfoText").GetComponent<TMP_Text>(); // �����ۼ��� (�ϴ�)


        //�ϴܺ�
        relicBtnViewrRef = relicInfo_MainRef.transform.Find("Window/RelicViewr").gameObject;
        normalContentRef = relicBtnViewrRef.transform.Find("Normal/Viewport/Content").gameObject;
        epicContentRef = relicBtnViewrRef.transform.Find("Epic/Viewport/Content").gameObject;
        lengedContentRef = relicBtnViewrRef.transform.Find("Legend/Viewport/Content").gameObject;
        haveText = relicInfo_MainRef.transform.Find("Window/Have/Text").GetComponent<TMP_Text>(); // �ϴ� �����

        // �߹� ��ư
        int forCount = relicBtnViewrRef.transform.childCount;
        ViewrRef = new GameObject[forCount];

        for (int index = 0; index < forCount; index++)
        {
            ViewrRef[index] = relicBtnViewrRef.transform.GetChild(index).gameObject;
        }

        middleBtn = relicInfo_MainRef.transform.Find("Window/Middle_Btn").GetComponentsInChildren<Button>(true);
        middleBtn[0].onClick.AddListener(() => { SwitchBottomViewr(0); });
        middleBtn[1].onClick.AddListener(() => { SwitchBottomViewr(1); });
        middleBtn[2].onClick.AddListener(() => { SwitchBottomViewr(2); });

        for (int index = 0; index < middleBtn.Length; index++)
        {
            middleBtnImg[index] = middleBtn[index].GetComponent<Image>();
        }

    }
    void Start()
    {
        RelicBtnInit();
    }

    /// <summary>
    ///  ������ ������
    /// </summary>
    public void Set_RelicDogamActive(bool Active)
    {
        if (Active)
        {
            Init_Relic();// ���� ��ư�� �ʱ�ȭ
            Init_HaveText(); // ���� ȹ�� ���� �ʱ�ȭ
            relicInfo_MainRef.SetActive(true);
            SwitchBottomViewr(0);
        }
        else
        {
            relicInfo_MainRef.SetActive(false);
            Array.Fill(haveCount, 0);
        }
    }

    Color noHaveColor = new Color(0.2f, 0.2f, 0.2f, 1);
    public void InitViewr(int typeNumber, int indexNumber, int totalNumber)
    {
        bool ishave = false;

        if (totalNumber == 3)
        {
            imgAnim.SetTrigger("UpDown");
        }
        else
        {
            imgAnim.SetTrigger("Size");
        }

        switch (typeNumber)
        {
            case 0: // �Ϲ� ���� �ʱ�ȭ
                epicEffectRef.SetActive(false);
                legendEffectRef.SetActive(false);
                ishave = bottomRelicNormalBtn[0].MyLv() > 0 ? true : false ;
                break;

            case 1:  // ���� ���� �ʱ�ȭ
                epicEffectRef.SetActive(true);
                legendEffectRef.SetActive(false);
                ishave = bottomRelicEpicBtn[0].MyLv() > 0 ? true : false;
                break;

            case 2:  // ���� ���� �ʱ�ȭ
                epicEffectRef.SetActive(false);
                legendEffectRef.SetActive(true);
                ishave = bottomRelicLegendBtn[0].MyLv() > 0 ? true : false;
                break;
        }

            itemCase.sprite = Get_RelicIcon_OutLine_Sprite(typeNumber);
            mainItemIMG.sprite = SpriteResource.inst.Relic_Sprite_TypeAndNumber(typeNumber, indexNumber);
            itemInfo.colorGradientPreset = typeGradient[typeNumber];

        // �� ������ ù��° ��Ұ� ��ȹ����¶��
        if (ishave)
        {
            itemName.text = relicNameAndtext[totalNumber].Split('=')[0];
            itemInfo.text = relicNameAndtext[totalNumber].Split('=')[1];
            mainItemIMG.color = Color.white;
        }
        else
        {
            itemName.text = "???";
            itemInfo.text = "ȹ���ϸ� ������ �϶� �� �� �ֽ��ϴ�.";
            mainItemIMG.color = noHaveColor;
        }
    }



    // ��� ��� ����
    public void Set_MainViewr(int typeNumber, int indexNumber, int totalNumber, bool ishave)
    {
        itemCase.sprite = Get_RelicIcon_OutLine_Sprite(typeNumber);
        mainItemIMG.sprite = SpriteResource.inst.Relic_Sprite_TypeAndNumber(typeNumber, indexNumber);
        
        if (ishave)
        {
            itemName.text = relicNameAndtext[totalNumber].Split('=')[0];
            itemInfo.text = relicNameAndtext[totalNumber].Split('=')[1];
            mainItemIMG.color = Color.white;
        }
        else
        {
            itemName.text = "???";
            itemInfo.text = "ȹ���ϸ� �ɷ�ġ ����";
            mainItemIMG.color = noHaveColor;
        }

        if (totalNumber == 3)
        {
            imgAnim.SetTrigger("UpDown");
        }
        else
        {
            imgAnim.SetTrigger("Size");
        }

    }


    Color hideColor = new Color(0.4f, 0.4f, 0.4f, 1f);
    // �̵� ��ư�� (�ϴܺ� ��� ����Ī)
    private void SwitchBottomViewr(int Number)
    {
        for (int index = 0; index < ViewrRef.Length; index++)
        {
            if (Number == index)
            {
                ViewrRef[index].gameObject.SetActive(true);
                middleBtnImg[index].color = Color.white;
            }
            else
            {
                ViewrRef[index].gameObject.SetActive(false);
                middleBtnImg[index].color = hideColor;
            }
        }

        
        if (Number == 0)
        {
            InitViewr(0, 0, 0);
        }
        else if(Number == 1)
        {
            InitViewr(1, 0, relicTrsCount[0]);
        }
        else if(Number == 2)
        {
            InitViewr(2, 0, relicTrsCount[0]+relicTrsCount[1]);
        }

        haveText.text = $"ȹ�� ���� ({haveCount[Number]} / {relicTrsCount[Number]})";
    }


    // â�������� ���� ��޺��� ȹ�淮 üũ (�ֽ�ȭ)
    private void Init_HaveText()
    {
        List<int> arr = GameStatus.inst.GetAryRelicLv();
        
        for (int index = 0; index < totalRelicCount; index++) 
        {
             if(index < relicTrsCount[0] && arr[index] > 0)
            {
                haveCount[0]++;
            }
             else if(index >= relicTrsCount[0] && index < relicTrsCount[0] + relicTrsCount[1] && arr[index] > 0)
            {
                haveCount[1]++;
            }
            else if (index >= relicTrsCount[0] + relicTrsCount[1] && index < relicTrsCount.Sum() && arr[index] > 0)
            {
                haveCount[2]++;
            }
        }
    }

    private void Init_Relic()
    {
        // �븻 ���� ������
        for (int index = 0; index < relicTrsCount[0]; index++)
        {
            bottomRelicNormalBtn[index].Update_Current_Lv();
        }

        // ���� ���� ������
        for (int index = 0; index < relicTrsCount[1]; index++)
        {
            bottomRelicEpicBtn[index].Update_Current_Lv();
        }

        // �������� ���� ������
        for (int index = 0; index < relicTrsCount[2]; index++)
        {
            bottomRelicLegendBtn[index].Update_Current_Lv();
        }
    }

    [SerializeField]
    int[] relicTrsCount = new int[3];

    // �ϴܺ� �ʱ�ȭ
    private void RelicBtnInit()
    {

        relicTrsCount[0] = normalContentRef.transform.childCount;
        relicTrsCount[1] = epicContentRef.transform.childCount;
        relicTrsCount[2] = lengedContentRef.transform.childCount;

        totalRelicCount = relicTrsCount.Sum();

        bottomRelicNormalBtn = new RelicInfo_Prefbas[relicTrsCount[0]];
        bottomRelicEpicBtn = new RelicInfo_Prefbas[relicTrsCount[1]];
        bottomRelicLegendBtn = new RelicInfo_Prefbas[relicTrsCount[2]];

        // �븻 ���� ������
        for (int index = 0; index < relicTrsCount[0]; index++)
        {
            bottomRelicNormalBtn[index] = normalContentRef.transform.GetChild(index).GetComponent<RelicInfo_Prefbas>();
        }

        // ���� ���� ������
        for (int index = 0; index < relicTrsCount[1]; index++)
        {
            bottomRelicEpicBtn[index] = epicContentRef.transform.GetChild(index).GetComponent<RelicInfo_Prefbas>();
        }

        // �������� ���� ������
        for (int index = 0; index < relicTrsCount[2]; index++)
        {
            bottomRelicLegendBtn[index] = lengedContentRef.transform.GetChild(index).GetComponent<RelicInfo_Prefbas>();
        }
    }


}
