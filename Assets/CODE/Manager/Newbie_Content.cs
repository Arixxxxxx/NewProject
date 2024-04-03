using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Newbie_Content : MonoBehaviour
{
    public static Newbie_Content inst;

    [SerializeField][Tooltip("0 = 활성 / 1 = 비활성 ")] Sprite[] imgBoxSideSprite;
    [SerializeField] Color gotItemColor;

    /// Ref
    GameObject frontUI, newbieWindow, gameWindow, layoutRef;
    Button xBtn;

    int iconLayoutCount = 0;

    TMP_Text mainTaxt;
    // 수락버튼
    GameObject[] GetBtn = new GameObject[2];
    GameObject alrimWindow;
    Image alrimWindowItemIMG;
    Button alrimBtn;

    //신규유저 버프 바로가기 버튼
    Button buffViewrBtn;

    // 아이콘박스 이미지 및 나아가는 길목 이미지 색상
    Image[] iconLayoutIMG;


    Image[] iconRoadLineIMG;
    GameObject[] iconBG;

    // 뉴비 버프 아이콘 설명창
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

        for (int index = 0; index < iconLayoutCount; index++) // 아이콘 박스
        {
            iconLayoutIMG[index] = layoutRef.transform.GetChild(index).GetComponent<Image>();
        }

        for (int index = 0; index < iconLayoutCount - 1; index++) // 길목 라인
        {
            iconRoadLineIMG[index] = gameWindow.transform.Find("Line").GetChild(index).GetComponent<Image>();
        }

        GetBtn[0] = gameWindow.transform.Find("TextLayOut/NoGet").gameObject;
        GetBtn[1] = gameWindow.transform.Find("TextLayOut/Got").gameObject;

        alrimWindow = newbieWindow.transform.Find("Alrim").gameObject;
        alrimWindowItemIMG = alrimWindow.transform.Find("Window/Frame_LayOut/IMG_Frame/IMG").GetComponent<Image>();
        alrimBtn = alrimWindow.transform.Find("Window/Button").GetComponent<Button>();
        alrimBtn.onClick.AddListener(() => { alrimWindow.SetActive(false); });

        LayoutIconBGInit(); // 모든 BG 스프라이트 거멓게 

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
                $"  <color=green>   {(BuffContoller.inst.GetBuffTime(4)/60) / 24}일 {(BuffContoller.inst.GetBuffTime(4) / 60) % 24}시간 {BuffContoller.inst.GetBuffTime(4) % 60}분</color>";

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
            // 초기화부분 테두리 리셋후에 다시 
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
        int[] LastGetGiftDay = GameStatus.inst.GetNewbieGiftDay; // 마지막으로 선울받은 일시 가져옴

        if (LastGetGiftDay.Sum() == 0 && GameStatus.inst.GotNewbieGiftCount == 0)
        {
            IconInit();   // 최초 임 그냥 열어줌
        }
        else if (LastGetGiftDay.Sum() != 0 && GameStatus.inst.GotNewbieGiftCount > 0)
        {
            //받은적이잇음
            if (LastGetGiftDay[0] < DateTime.Now.Year) //현재 시간 체크
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
        //버튼 초기화 부 (받은게 있음 꺼주고, 받을게 잇다면 켜줌)
        if (GameStatus.inst.GotNewbieGiftCount < layoutRef.transform.childCount)
        {
            IconBoxInit();
            layoutRef.transform.GetChild(GameStatus.inst.GotNewbieGiftCount).Find("BG").gameObject.SetActive(false);
        }

        //루비 계산 (적어놓은 텍스트에서 빼옴)
        int value = int.Parse(layoutRef.transform.GetChild(GameStatus.inst.GotNewbieGiftCount).Find("CountText").GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());
        int checkDay = int.Parse(gameWindow.transform.Find("Box").GetChild(GameStatus.inst.GotNewbieGiftCount).GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());

        //Sprite ItemIMG = layoutRef.transform.GetChild(GameStatus.inst.GotNewbieGiftCount).Find("Image").GetComponent<Image>().sprite;
        // 나중에 알림창에 이미지 띄울꺼면 다시 살리면됨

        // N일차 보상받기 ~~ 텍스트 초기화
        mainTaxt.text = $"  < {checkDay}일차 > 신규유저 보상받기\r\n - 보상은 <color=green>우편함</color>으로 발송됩니다.";

        //수락일자 계산
        int[] NowDate = new int[3];
        NowDate[0] = DateTime.Now.Year;
        NowDate[1] = DateTime.Now.Month;
        NowDate[2] = DateTime.Now.Day;

        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.RemoveAllListeners();
        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.AddListener(() => // 수락버튼부
        {
            LetterManager.inst.MakeLetter(0, "게임GM", $"신규유저 {checkDay}일차 보상", value); // 보상 우편 획득
            GetIconChanger(GameStatus.inst.GotNewbieGiftCount); // 아이콘 받음처리
            GameStatus.inst.GetNewbieGiftDay = NowDate; // 일자 업데이트
            GameStatus.inst.GotNewbieGiftCount++; // 받은 카운트 올려줌
            WorldUI_Manager.inst.OnEnableRedSimball(2, false); // 빨간심볼 꺼주기

            if (GameStatus.inst.GotNewbieGiftCount < layoutRef.transform.childCount)
            {
                layoutRef.transform.GetChild(GameStatus.inst.GotNewbieGiftCount).Find("BG").gameObject.SetActive(false);
                IconBoxInit();
            }


            GetBtnAcitve(false); // 버튼 비활성화
            ConfirmWindowAcitve();

        });

    }

    //아이콘박스 최신화 함수
    private void IconBoxInit()
    {
        // 아이콘 길목 색상
        for (int index = 0; index < GameStatus.inst.GotNewbieGiftCount; index++) 
        {
            layoutRef.transform.GetChild(index).Find("Check").gameObject.SetActive(true);
            iconRoadLineIMG[index].color = gotItemColor; 
        }

        // 아이콘 박스 이미지
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


    public void IconRedSimballInit() // 빨간심볼 활성화
    {
        int[] LastGetGiftDay = GameStatus.inst.GetGiftDay;

        if (LastGetGiftDay.Sum() == 0 && GameStatus.inst.GotNewbieGiftCount == 0)
        {
            WorldUI_Manager.inst.OnEnableRedSimball(2, true);
        }
        else if (LastGetGiftDay.Sum() != 0 && GameStatus.inst.GotNewbieGiftCount > 0)
        {
            //받은적이잇음
            if (LastGetGiftDay[0] < DateTime.Now.Year) //현재 시간 체크
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



