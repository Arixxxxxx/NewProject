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

    //받기 버튼 혹은 없는
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

        // 하이라키 Ref
        worldFrontRef = GameObject.Find("---[FrontUICanvas]").gameObject;
        dailyCheckObjRef = worldFrontRef.transform.Find("DailyCheck").gameObject;
        dailyWindowRef = dailyCheckObjRef.transform.Find("Window").gameObject;
        layoutRef = dailyWindowRef.transform.Find("Lyaout").gameObject;

        //받기버튼 부
        GetBtn[0] = dailyWindowRef.transform.Find("TextLayOut/NoGet").gameObject;
        GetBtn[1] = dailyWindowRef.transform.Find("TextLayOut/Got").gameObject;

        mainTaxt = GetBtn[0].GetComponent<TMP_Text>();

        // 루비
        adViewAndGetRubyBtn = dailyWindowRef.transform.Find("ShowADBtn").GetComponent<Button>();

        LayoutIconBGInit(); // 아이콘 백그라운드 일단 다 끄기

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
        
        
        adViewAndGetRubyBtn.onClick.AddListener(() => //광고 버튼
        {
            WorldUI_Manager.inst.SampleAD_Get_Currency(0, 100); //재화주기
            adViewAndGetRubyBtn.GetComponent<Image>().sprite = adBtnSprite[1]; //스프라이트 교체
            adViewAndGetRubyBtn.onClick.RemoveAllListeners(); // 버튼기능 -> 창종료로 바꿈
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
    /// 켜주면서 초기화
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


    // 게임실행시 아이콘에 빨간심볼 만들어주는 함수
    public void IconRedSimballInit()
    {
        int[] LastGetGiftDay = GameStatus.inst.GetGiftDay;

        if (LastGetGiftDay.Sum() == 0 && GameStatus.inst.GotDilayPlayGiftCount == 0)
        {
            WorldUI_Manager.inst.OnEnableRedSimball(1,true);
        }
        else if (LastGetGiftDay.Sum() != 0 && GameStatus.inst.GotDilayPlayGiftCount > 0)
        {
            //받은적이잇음
            if (LastGetGiftDay[0] < DateTime.Now.Year) //현재 시간 체크
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

    // 출석체크 초기화 대상날짜인지 확인
    public void LayOutInit()
    {
        int[] LastGetGiftDay = GameStatus.inst.GetGiftDay;

        if (LastGetGiftDay.Sum() == 0 && GameStatus.inst.GotDilayPlayGiftCount == 0)
        {
            IconInit();   // 최초 임 그냥 열어줌
        }
        else if(LastGetGiftDay.Sum() != 0 && GameStatus.inst.GotDilayPlayGiftCount > 0)
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
        for (int index = 0; index < GameStatus.inst.GotDilayPlayGiftCount; index++)
        {
            layoutRef.transform.GetChild(index).Find("Check").gameObject.SetActive(true);
        }

        layoutRef.transform.GetChild(GameStatus.inst.GotDilayPlayGiftCount).Find("BG").gameObject.SetActive(false);

        //루비 계산 (적어놓은 텍스트에서 빼옴)
        int value = int.Parse(layoutRef.transform.GetChild(GameStatus.inst.GotDilayPlayGiftCount).Find("CountText").GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());
        int checkDay = int.Parse(layoutRef.transform.GetChild(GameStatus.inst.GotDilayPlayGiftCount).Find("NumberText").GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());

        // N일차 보상받기 ~~ 텍스트 초기화
        mainTaxt.text = $"  < {checkDay}일차 > 출석체크 보상받기\r\n - 보상은 <color=green>우편함</color>으로 발송됩니다.";

        //수락일자 계산
        int[] NowDate = new int[3];
        NowDate[0] = DateTime.Now.Year;
        NowDate[1] = DateTime.Now.Month;
        NowDate[2]  = DateTime.Now.Day;

        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.RemoveAllListeners();
        GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            LetterManager.inst.MakeLetter(0, "게임GM", $"출석체크 {checkDay}일차 보상", value); // 보상 우편 획득
            GetIconChanger(GameStatus.inst.GotDilayPlayGiftCount); // 아이콘 받음처리
            GameStatus.inst.GetGiftDay = NowDate; // 일자 업데이트
            GameStatus.inst.GotDilayPlayGiftCount++; // 받은 카운트 올려줌
            WorldUI_Manager.inst.OnEnableRedSimball(1, false); // 빨간심볼 꺼주기
            GetBtnAcitve(false); // 버튼 비활성화
        });


    }


    /// <summary>
    /// 아이콘을 받은 후 받은 IMG처리해주는 함수
    /// </summary>
    /// <param name="value"></param>
    private void GetIconChanger(int value)
    {
        layoutRef.transform.GetChild(value).Find("BG").gameObject.SetActive(true);
        layoutRef.transform.GetChild(value).Find("Check").gameObject.SetActive(true);
    }

    /// <summary>
    /// 하단부 버튼
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
