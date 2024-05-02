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
    [SerializeField][Tooltip("0 = 활성 / 1 = 비활성 ")] Sprite[] imgBoxSideSprite;
    [Header("# Input GetItemLine  <Color=green>( Cyan Color )</Color>")]
    [Space]
    [SerializeField] Color gotItemColor;

    /// Ref
    GameObject frontUI, newbieWindow, gameWindow, layoutRef;
    Button xBtn;

    int iconLayoutCount = 0;

    TMP_Text mainTaxt;
    // 수락버튼
    [Tooltip("0 받기 활성화 / 1비활성화")] GameObject[] GetBtn = new GameObject[2];

    GameObject alrimWindow;
    Image alrimWindowItemIMG;
    Button alrimBtn;

    //신규유저 버프 바로가기 버튼
    Button buffViewrBtn;

    // 아이콘박스 이미지 및 나아가는 길목 이미지 색상
    Image[] iconLayoutIMG;


    Image[] iconRoadLineIMG;
    GameObject[] iconBG;
    GameObject[] checkIcon;

    // 뉴비 버프 아이콘 설명창
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

        for (int index = 0; index < iconLayoutCount; index++) // 아이콘 박스
        {
            iconLayoutIMG[index] = layoutRef.transform.GetChild(index).GetComponent<Image>();
            iconBG[index] = layoutRef.transform.GetChild(index).Find("BG").gameObject;
            checkIcon[index] = layoutRef.transform.GetChild(index).Find("Check").gameObject;
        }

        for (int index = 0; index < gameWindow.transform.Find("Line").transform.childCount; index++) // 길목 라인
        {
            iconRoadLineIMG[index] = gameWindow.transform.Find("Line").GetChild(index).GetComponent<Image>();
        }

        GetBtn[0] = gameWindow.transform.Find("TextLayOut/NoGet").gameObject;
        GetBtn[1] = gameWindow.transform.Find("TextLayOut/Got").gameObject;

        alrimWindow = newbieWindow.transform.Find("Alrim").gameObject;
        alrimWindowItemIMG = alrimWindow.transform.Find("Window/Frame_LayOut/IMG_Frame/IMG").GetComponent<Image>();
        alrimBtn = alrimWindow.transform.Find("Window/Button").GetComponent<Button>();
        alrimBtn.onClick.AddListener(() => { alrimWindow.SetActive(false); });


        //버프 아이콘클릭시 정보창
        buffInfoWindow = frontUI.transform.Find("NewbieBtnInfo").gameObject;
        buffLeftTimeText = buffInfoWindow.transform.Find("Window/TextLayOut/NoGet").GetComponent<TMP_Text>();
        buffinfoBottomBtn = buffLeftTimeText.transform.GetComponentInChildren<Button>();
        buffinfoBottomBtn.onClick.AddListener(() => buffInfoWindow.gameObject.SetActive(false));
    }

    /// <summary>
    /// 뉴비 버프 아이콘 클릭시 정보창  초기화
    /// </summary>
    /// <param name="value"> On / Off </param>
    public void NewBieBuffInfoWindowActive(bool value)
    {
        if (value == true)
        {
            buffLeftTimeText.text = $"- 최초가입 후 7일간 적용됩니다.\n- 버프 만료까지 남은시간\n   " +
                $"  <color=green>   {(BuffContoller.inst.GetBuffTime(4) / 60) / 24}일 {(BuffContoller.inst.GetBuffTime(4) / 60) % 24}시간 {BuffContoller.inst.GetBuffTime(4) % 60}분</color>";

            buffInfoWindow.SetActive(true);
        }
        else
        {
            buffInfoWindow.SetActive(false);
        }
    }




    /// <summary>
    /// 뉴비보상창 호출
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
        // 버튼 활성화 및 비활성화 변경
        GetBtnAcitve(!TodayGetReward);

        //보석 일차수 레이아웃 최신화
        IconBoxInit();
        
        //루비 계산 및 텍스트 초기화 (적어놓은 텍스트에서 빼옴)
        int rubyCount = int.Parse(layoutRef.transform.GetChild(GameStatus.inst.GotNewbieGiftCount).Find("CountText").GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());
        mainTaxt.text = $"  < {GameStatus.inst.GotNewbieGiftCount + 1}일차 > 신규유저 보상받기\r\n - 보상은 <color=green>우편함</color>으로 발송됩니다.";

        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.RemoveAllListeners();
        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.AddListener(() => // 수락버튼부
        {
            LetterManager.inst.MakeLetter(0, "게임GM", $"신규유저 {GameStatus.inst.GotNewbieGiftCount + 1}일차 보상", rubyCount); // 보상 우편 획득
                                                                                                                        //GetIconChanger(GameStatus.inst.GotNewbieGiftCount); // 아이콘 받음처리
            GameStatus.inst.GotNewbieGiftCount++; // 받은 카운트 올려줌
            GameStatus.inst.TodayGetReward = true; // 받음

            IconBoxInit(); // 항목 재설정
            GetBtnAcitve(false); // 버튼 비활성화
            ConfirmWindowAcitve(); // 수락창 활성화
        });

    }

    //아이콘박스 최신화 함수
    private void IconBoxInit()
    {
        int forCount = GameStatus.inst.GotNewbieGiftCount;

        // 아이콘 거멓게 배경
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

        // 아이콘 길목 색상
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

        // 아이콘 박스 이미지
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



