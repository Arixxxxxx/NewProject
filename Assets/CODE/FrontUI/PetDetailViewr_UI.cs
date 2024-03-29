using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetDetailViewr_UI : MonoBehaviour
{
    public static PetDetailViewr_UI inst;

    // �ش� ��ũ��Ʈ index�� 0 = ������ / 1 ������ / 2 ����� ����

    [Header("Input Charactor BackGround Sprite")]
    [Space]
    [SerializeField][Tooltip("�������� / �������� / �������")] Sprite[] charactorBG;
    [SerializeField][Tooltip("Ȱ��ȭ / ��Ȱ��ȭ")] Sprite[] topArrayBtnIMG;
    [SerializeField][Tooltip("Ȱ��ȭ / ��Ȱ��ȭ")] Sprite[] middleArrayBtnIMG;

    GameObject frontUIObj;
    GameObject PetDetailViwerObj;
    GameObject hiearchySurchPoint;

    //��� �ϴ� ���ù�ȣ
    int curCharNum, curBotNum;



    // Title ��� ������ư
    Button xBtn;
    GameObject[] petChar = new GameObject[3];
    TMP_Text viewLeftBotText;

    // ��� ĳ���� ��ư �� ��ư �̹���

    Image ViewBG; // ���
    Button[] topArrayBtns;
    Image[] topArrayBtnsImage = new Image[3];

    // �ߴ� ��ư
    [SerializeField]
    Button[] midArrayBtns;
    [SerializeField]
    Image[] midArrayBtnsImage = new Image[3];


    // ���� ������Ʈ��
    GameObject[] petInfo = new GameObject[3];

    // ���� ������Ʈ��
    GameObject[] petGakSeong = new GameObject[3];

    // ��ȭ ������Ʈ��
    GameObject[] petUpgrade = new GameObject[3];

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

        /////////////////���̶�Ű Ref ���� /////////////////

        frontUIObj = GameObject.Find("---[FrontUICanvas]").gameObject;
        PetDetailViwerObj = frontUIObj.transform.Find("Pet_Detail_Window").gameObject;
        hiearchySurchPoint = PetDetailViwerObj.transform.Find("Window").gameObject;

        ///////////////���̶�Ű ����///////////////////

        // �ֻ�� X ��ư
        xBtn = hiearchySurchPoint.transform.Find("Title").GetComponentInChildren<Button>();

        // ���� ��׶��� X ��ư
        ViewBG = hiearchySurchPoint.transform.Find("ViewBG").GetComponent<Image>();

        // ��� �� ĳ���� ������Ʈ [���ϸ����� & �̹���]
        petChar[0] = ViewBG.transform.Find("PET_0").gameObject;
        petChar[1] = ViewBG.transform.Find("PET_1").gameObject;
        petChar[2] = ViewBG.transform.Find("PET_2").gameObject;

        // �� �ر����� ����ĭ
        viewLeftBotText = ViewBG.transform.Find("TextBar").GetComponentInChildren<TMP_Text>();

        // ��� �꼱�� ��ư
        topArrayBtns = hiearchySurchPoint.transform.Find("Top_Btn_Array").GetComponentsInChildren<Button>();
        topArrayBtnsImage[0] = topArrayBtns[0].GetComponent<Image>();
        topArrayBtnsImage[1] = topArrayBtns[1].GetComponent<Image>();
        topArrayBtnsImage[2] = topArrayBtns[2].GetComponent<Image>();

        // �ߴ� ��� ��ư
        midArrayBtns = hiearchySurchPoint.transform.Find("Middle_Btn_Array").GetComponentsInChildren<Button>();
        midArrayBtnsImage[0] = midArrayBtns[0].GetComponent<Image>();
        midArrayBtnsImage[1] = midArrayBtns[1].GetComponent<Image>();
        midArrayBtnsImage[2] = midArrayBtns[2].GetComponent<Image>();

        // �ϴ� ���� ������
        petInfo[0] = hiearchySurchPoint.transform.Find("PetInfo").GetChild(0).gameObject;
        petInfo[1] = hiearchySurchPoint.transform.Find("PetInfo").GetChild(1).gameObject;
        petInfo[2] = hiearchySurchPoint.transform.Find("PetInfo").GetChild(2).gameObject;

        // ����
        petGakSeong[0] = hiearchySurchPoint.transform.Find("GakSeong").GetChild(0).gameObject;
        petGakSeong[1] = hiearchySurchPoint.transform.Find("GakSeong").GetChild(1).gameObject;
        petGakSeong[2] = hiearchySurchPoint.transform.Find("GakSeong").GetChild(2).gameObject;

        // ��ȭ
        petUpgrade[0] = hiearchySurchPoint.transform.Find("Upgrade").GetChild(0).gameObject;
        petUpgrade[1] = hiearchySurchPoint.transform.Find("Upgrade").GetChild(1).gameObject;
        petUpgrade[2] = hiearchySurchPoint.transform.Find("Upgrade").GetChild(2).gameObject;

        BtnInIt();
    }
    void Start()
    {

    }
    void Update()
    {

    }
    private void BtnInIt()
    {
        xBtn.onClick.AddListener(() => { middleBtnImageChanger(0); curCharNum = 0; curBotNum = 0; PetDetailViwerObj.SetActive(false); });
        topArrayBtns[0].onClick.AddListener(() => TopArrayBtnActive(0));
        topArrayBtns[1].onClick.AddListener(() => TopArrayBtnActive(1));
        topArrayBtns[2].onClick.AddListener(() => TopArrayBtnActive(2));

        midArrayBtns[0].onClick.AddListener(() =>
        {
            curBotNum = 0;
            middleBtnImageChanger(0);

            BottomInformationActive(curCharNum, true);

            BottomGaksungActive(false);
            BottomUpGradeActive(false);
        });

        midArrayBtns[1].onClick.AddListener(() =>
        {
            curBotNum = 1;
            middleBtnImageChanger(1);

            BottomInformationActive(curCharNum, false);
            BottomGaksungActive(true);
            BottomUpGradeActive(false);
        });


        midArrayBtns[2].onClick.AddListener(() =>
        {
            curBotNum = 2;
            middleBtnImageChanger(2);

            BottomInformationActive(curCharNum, false);
            BottomGaksungActive(false);
            BottomUpGradeActive(true);

        });
    }

    /// <summary>
    /// ��� ĳ���͹�ư ��� ���� �Լ�
    /// </summary>
    /// <param name="indexNum"> ������ / ������ / ����� </param>
    public void TopArrayBtnActive(int indexNum)
    {
        if (PetDetailViwerObj.gameObject.activeSelf == false)
        {
            PetDetailViwerObj.gameObject.SetActive(true);
        }

        curCharNum = indexNum;

        for (int index = 0; index < 3; index++)
        {
            if (index == indexNum)
            {
                topArrayBtnsImage[index].sprite = topArrayBtnIMG[0]; // ��ư ��� �̹��� ����
                petChar[index].gameObject.SetActive(true); // ���� ĳ���� ����
            }
            else
            {
                topArrayBtnsImage[index].sprite = topArrayBtnIMG[1];
                petChar[index].gameObject.SetActive(false);
            }
        }

        ViewBG.sprite = charactorBG[indexNum]; // ��� ����
        viewLeftBotText.text = ViewrLeftBottomTextInit(indexNum); // �����ϴ� ȹ������ �ؽ�Ʈ �ʱ�ȭ

        BottomReset();

    }

    private string ViewrLeftBottomTextInit(int indexNum)
    {
        switch (indexNum)
        {
            case 0:
                return "ȹ������ : " + "Stage 10" + " ���Խ� ȹ��";

            case 1:
                return "ȹ������ : " + "Stage 20" + " ���Խ� ȹ��";

            case 2:
                return "ȹ������ : " + "Stage 30" + " ���Խ� ȹ��";
        }

        return null;
    }


    /// <summary>
    ///  ��� ĳ���� ���� ��ư Ŭ����  �ϴܺ� �ڵ� �ʱ�ȭ �Լ� [��� ��ư��]
    /// </summary>
    private void BottomReset()
    {
        switch (curBotNum)
        {
            case 0:
                BottomInformationActive(curCharNum, true); // �ϴ� ����
                BottomGaksungActive(false);
                BottomUpGradeActive(false);
                break;

            case 1:
                BottomUpGradeActive(false);
                BottomInformationActive(curCharNum, false);
                BottomGaksungActive(true);
                
                break;

            case 2:
                BottomUpGradeActive(true);
                BottomInformationActive(curCharNum, false);
                BottomGaksungActive(false);
                break;

        }






    }

    private void middleBtnImageChanger(int indexNum)
    {
        for (int index = 0; index < midArrayBtnsImage.Length; index++)
        {
            if (index == indexNum)
            {
                midArrayBtnsImage[index].sprite = middleArrayBtnIMG[0];
            }
            else
            {
                midArrayBtnsImage[index].sprite = middleArrayBtnIMG[1];
            }
        }
    }


    /// <summary>
    /// �ϴ� ����â ON/OFF
    /// </summary>
    /// <param name="charIndex"></param>
    private void BottomInformationActive(int charIndex, bool value)
    {
        if (value) // Ʈ��� �ش� �ε��� ���ְ�
        {
            for (int index = 0; index < petInfo.Length; index++)
            {
                if (charIndex == index)
                {
                    petInfo[index].gameObject.SetActive(true);
                }
                else
                {
                    petInfo[index].gameObject.SetActive(false);
                }
            }
        }
        else //�޽��� �ٲ�
        {
            for (int index = 0; index < petInfo.Length; index++)
            {
                petInfo[index].gameObject.SetActive(false);
            }

        }
    }

    private void BottomGaksungActive(bool value)
    {
        if (value)
        {
            for (int index = 0; index < petInfo.Length; index++)
            {
                if (curCharNum == index)
                {
                    petGakSeong[index].gameObject.SetActive(true);
                }
                else
                {
                    petGakSeong[index].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int index = 0; index < petInfo.Length; index++)
            {
                petGakSeong[index].gameObject.SetActive(false);
            }
        }
    }

    private void BottomUpGradeActive(bool value)
    {
        if (value)
        {
            for (int index = 0; index < petInfo.Length; index++)
            {
                if (curCharNum == index)
                {
                    petUpgrade[index].gameObject.SetActive(true);
                }
                else
                {
                    petUpgrade[index].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int index = 0; index < petInfo.Length; index++)
            {
                petUpgrade[index].gameObject.SetActive(false);
            }
        }
    }
}
