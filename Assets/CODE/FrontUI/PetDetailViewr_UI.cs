using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetDetailViewr_UI : MonoBehaviour
{
    public static PetDetailViewr_UI inst;

    // 해당 스크립트 index는 0 = 공격펫 / 1 버프펫 / 2 골드펫 기준

    [Header("Input Charactor BackGround Sprite")]
    [Space]
    [SerializeField][Tooltip("공격펫배경 / 버프펫배경 / 골드펫배경")] Sprite[] charactorBG;
    [SerializeField][Tooltip("활성화 / 비활성화")] Sprite[] topArrayBtnIMG;
    [SerializeField][Tooltip("활성화 / 비활성화")] Sprite[] middleArrayBtnIMG;

    GameObject frontUIObj;
    GameObject PetDetailViwerObj;
    GameObject hiearchySurchPoint;

    //상단 하단 선택번호
    int curCharNum, curBotNum;



    // Title 상단 엑스버튼
    Button xBtn;
    GameObject[] petChar = new GameObject[3];
    TMP_Text viewLeftBotText;

    // 상단 캐릭터 버튼 및 버튼 이미지

    Image ViewBG; // 배경
    Button[] topArrayBtns;
    Image[] topArrayBtnsImage = new Image[3];

    // 중단 버튼
    [SerializeField]
    Button[] midArrayBtns;
    [SerializeField]
    Image[] midArrayBtnsImage = new Image[3];


    // 정보 오브젝트들
    GameObject[] petInfo = new GameObject[3];

    // 각성 오브젝트들
    GameObject[] petGakSeong = new GameObject[3];

    // 강화 오브젝트들
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

        /////////////////하이라키 Ref 참조 /////////////////

        frontUIObj = GameObject.Find("---[FrontUICanvas]").gameObject;
        PetDetailViwerObj = frontUIObj.transform.Find("Pet_Detail_Window").gameObject;
        hiearchySurchPoint = PetDetailViwerObj.transform.Find("Window").gameObject;

        ///////////////하이라키 참조///////////////////

        // 최상단 X 버튼
        xBtn = hiearchySurchPoint.transform.Find("Title").GetComponentInChildren<Button>();

        // 펫뷰어 백그라운드 X 버튼
        ViewBG = hiearchySurchPoint.transform.Find("ViewBG").GetComponent<Image>();

        // 뷰어 펫 캐릭터 오브젝트 [에니메이터 & 이미지]
        petChar[0] = ViewBG.transform.Find("PET_0").gameObject;
        petChar[1] = ViewBG.transform.Find("PET_1").gameObject;
        petChar[2] = ViewBG.transform.Find("PET_2").gameObject;

        // 펫 해금조건 설명칸
        viewLeftBotText = ViewBG.transform.Find("TextBar").GetComponentInChildren<TMP_Text>();

        // 상단 펫선택 버튼
        topArrayBtns = hiearchySurchPoint.transform.Find("Top_Btn_Array").GetComponentsInChildren<Button>();
        topArrayBtnsImage[0] = topArrayBtns[0].GetComponent<Image>();
        topArrayBtnsImage[1] = topArrayBtns[1].GetComponent<Image>();
        topArrayBtnsImage[2] = topArrayBtns[2].GetComponent<Image>();

        // 중단 어레이 버튼
        midArrayBtns = hiearchySurchPoint.transform.Find("Middle_Btn_Array").GetComponentsInChildren<Button>();
        midArrayBtnsImage[0] = midArrayBtns[0].GetComponent<Image>();
        midArrayBtnsImage[1] = midArrayBtns[1].GetComponent<Image>();
        midArrayBtnsImage[2] = midArrayBtns[2].GetComponent<Image>();

        // 하단 정보 설명탭
        petInfo[0] = hiearchySurchPoint.transform.Find("PetInfo").GetChild(0).gameObject;
        petInfo[1] = hiearchySurchPoint.transform.Find("PetInfo").GetChild(1).gameObject;
        petInfo[2] = hiearchySurchPoint.transform.Find("PetInfo").GetChild(2).gameObject;

        // 각성
        petGakSeong[0] = hiearchySurchPoint.transform.Find("GakSeong").GetChild(0).gameObject;
        petGakSeong[1] = hiearchySurchPoint.transform.Find("GakSeong").GetChild(1).gameObject;
        petGakSeong[2] = hiearchySurchPoint.transform.Find("GakSeong").GetChild(2).gameObject;

        // 강화
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
    /// 상단 캐릭터버튼 배경 변경 함수
    /// </summary>
    /// <param name="indexNum"> 공격펫 / 버프펫 / 골드펫 </param>
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
                topArrayBtnsImage[index].sprite = topArrayBtnIMG[0]; // 버튼 배경 이미지 변경
                petChar[index].gameObject.SetActive(true); // 내부 캐릭터 변경
            }
            else
            {
                topArrayBtnsImage[index].sprite = topArrayBtnIMG[1];
                petChar[index].gameObject.SetActive(false);
            }
        }

        ViewBG.sprite = charactorBG[indexNum]; // 배경 변경
        viewLeftBotText.text = ViewrLeftBottomTextInit(indexNum); // 좌측하단 획득조건 텍스트 초기화

        BottomReset();

    }

    private string ViewrLeftBottomTextInit(int indexNum)
    {
        switch (indexNum)
        {
            case 0:
                return "획득조건 : " + "Stage 10" + " 진입시 획득";

            case 1:
                return "획득조건 : " + "Stage 20" + " 진입시 획득";

            case 2:
                return "획득조건 : " + "Stage 30" + " 진입시 획득";
        }

        return null;
    }


    /// <summary>
    ///  상단 캐릭터 선택 버튼 클릭시  하단부 자동 초기화 함수 [상단 버튼용]
    /// </summary>
    private void BottomReset()
    {
        switch (curBotNum)
        {
            case 0:
                BottomInformationActive(curCharNum, true); // 하단 켜줌
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
    /// 하단 정보창 ON/OFF
    /// </summary>
    /// <param name="charIndex"></param>
    private void BottomInformationActive(int charIndex, bool value)
    {
        if (value) // 트루면 해당 인덱스 켜주고
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
        else //펄스면 다끔
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
