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
        "곤륜산의검=(일반)등급 : 일반 공격력 증가",
        "몬스터 혈점표=(일반)등급 : 치명타 확률 증가",
        "황금 요강=(일반)등급 : 초당 골드 획득량 증가",
        "령화의 저주 인형=(에픽)등급 : 치명타 대미지 증가",
        "호두표 월병=(에픽)등급 : 피버타임 시간 증가",
        "설아의 돼지저금통=(에픽)등급 : 퀘스트 가격 인하",
        "스파크식 협상=(에픽)등급 : 무기 가격 인하",
        "판다의 손길=(전설)등급 : 공격 속도 증가",
        "용의 축복=(전설)등급 : 환생시 지급되는 별 증가",
        "황금 죽도=(전설)등급 : 적 처치시 골드획득량 증가",
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
    // 도감 하단부 버튼들
    GameObject relicBtnViewrRef, normalContentRef, epicContentRef, lengedContentRef;

    //중반 버튼 (3종)
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

        //상단 뷰어
        itemCase = relicInfo_MainRef.transform.Find("Window/ViewBG").GetComponent<Image>(); // 아웃라인
        mainItemIMG = itemCase.transform.Find("ItemIMG").GetComponent<Image>();
        imgAnim = mainItemIMG.GetComponent<Animator>();
        epicEffectRef = itemCase.transform.Find("Epic_Effect").gameObject; // 에픽 이펙트
        legendEffectRef = itemCase.transform.Find("Legend_Effect").gameObject; // 레전더리 이펙트
        itemName = itemCase.transform.Find("TitleBar/ItemName").GetComponent<TMP_Text>(); // 아이템 제목 (상단)
        itemInfo = itemCase.transform.Find("ItemInfoText").GetComponent<TMP_Text>(); // 아이템설명 (하단)


        //하단부
        relicBtnViewrRef = relicInfo_MainRef.transform.Find("Window/RelicViewr").gameObject;
        normalContentRef = relicBtnViewrRef.transform.Find("Normal/Viewport/Content").gameObject;
        epicContentRef = relicBtnViewrRef.transform.Find("Epic/Viewport/Content").gameObject;
        lengedContentRef = relicBtnViewrRef.transform.Find("Legend/Viewport/Content").gameObject;
        haveText = relicInfo_MainRef.transform.Find("Window/Have/Text").GetComponent<TMP_Text>(); // 하단 제목바

        // 중반 버튼
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
    ///  켜지고 꺼질때
    /// </summary>
    public void Set_RelicDogamActive(bool Active)
    {
        if (Active)
        {
            Init_Relic();// 내부 버튼들 초기화
            Init_HaveText(); // 유물 획득 정보 초기화
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
            case 0: // 일반 유물 초기화
                epicEffectRef.SetActive(false);
                legendEffectRef.SetActive(false);
                ishave = bottomRelicNormalBtn[0].MyLv() > 0 ? true : false ;
                break;

            case 1:  // 에픽 유물 초기화
                epicEffectRef.SetActive(true);
                legendEffectRef.SetActive(false);
                ishave = bottomRelicEpicBtn[0].MyLv() > 0 ? true : false;
                break;

            case 2:  // 전설 유물 초기화
                epicEffectRef.SetActive(false);
                legendEffectRef.SetActive(true);
                ishave = bottomRelicLegendBtn[0].MyLv() > 0 ? true : false;
                break;
        }

            itemCase.sprite = Get_RelicIcon_OutLine_Sprite(typeNumber);
            mainItemIMG.sprite = SpriteResource.inst.Relic_Sprite_TypeAndNumber(typeNumber, indexNumber);
            itemInfo.colorGradientPreset = typeGradient[typeNumber];

        // 각 유물별 첫번째 요소가 미획득상태라면
        if (ishave)
        {
            itemName.text = relicNameAndtext[totalNumber].Split('=')[0];
            itemInfo.text = relicNameAndtext[totalNumber].Split('=')[1];
            mainItemIMG.color = Color.white;
        }
        else
        {
            itemName.text = "???";
            itemInfo.text = "획득하면 정보를 일람 할 수 있습니다.";
            mainItemIMG.color = noHaveColor;
        }
    }



    // 상단 뷰어 설정
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
            itemInfo.text = "획득하면 능력치 공개";
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
    // 미들 버튼용 (하단부 뷰어 스위칭)
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

        haveText.text = $"획득 정보 ({haveCount[Number]} / {relicTrsCount[Number]})";
    }


    // 창열었을때 현재 등급별로 획득량 체크 (최신화)
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
        // 노말 유물 프리펩
        for (int index = 0; index < relicTrsCount[0]; index++)
        {
            bottomRelicNormalBtn[index].Update_Current_Lv();
        }

        // 에픽 유물 프리펩
        for (int index = 0; index < relicTrsCount[1]; index++)
        {
            bottomRelicEpicBtn[index].Update_Current_Lv();
        }

        // 레전더리 유물 프리펩
        for (int index = 0; index < relicTrsCount[2]; index++)
        {
            bottomRelicLegendBtn[index].Update_Current_Lv();
        }
    }

    [SerializeField]
    int[] relicTrsCount = new int[3];

    // 하단부 초기화
    private void RelicBtnInit()
    {

        relicTrsCount[0] = normalContentRef.transform.childCount;
        relicTrsCount[1] = epicContentRef.transform.childCount;
        relicTrsCount[2] = lengedContentRef.transform.childCount;

        totalRelicCount = relicTrsCount.Sum();

        bottomRelicNormalBtn = new RelicInfo_Prefbas[relicTrsCount[0]];
        bottomRelicEpicBtn = new RelicInfo_Prefbas[relicTrsCount[1]];
        bottomRelicLegendBtn = new RelicInfo_Prefbas[relicTrsCount[2]];

        // 노말 유물 프리펩
        for (int index = 0; index < relicTrsCount[0]; index++)
        {
            bottomRelicNormalBtn[index] = normalContentRef.transform.GetChild(index).GetComponent<RelicInfo_Prefbas>();
        }

        // 에픽 유물 프리펩
        for (int index = 0; index < relicTrsCount[1]; index++)
        {
            bottomRelicEpicBtn[index] = epicContentRef.transform.GetChild(index).GetComponent<RelicInfo_Prefbas>();
        }

        // 레전더리 유물 프리펩
        for (int index = 0; index < relicTrsCount[2]; index++)
        {
            bottomRelicLegendBtn[index] = lengedContentRef.transform.GetChild(index).GetComponent<RelicInfo_Prefbas>();
        }
    }


}
