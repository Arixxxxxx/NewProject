using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyPlayCheckUIManager : MonoBehaviour
{
    public static DailyPlayCheckUIManager inst;
    [Header("# Input Ad Wide Btn <Color=yellow>( Sprite File )</Color>")]
    [Space]
    [SerializeField] Sprite[] adBtnSprite;

    GameObject worldFrontRef, dailyCheckObjRef, dailyWindowRef, ItemListRef, worldUiref;
    GameObject[] iconBG;
    int iconCount;

    Button xBtn;

    //받기 버튼 혹은 없는
    GameObject[] GetBtn = new GameObject[2];
    TMP_Text mainTaxt;

    // 아이콘 초기화부
    int itemCount;
    TMP_Text[] itemCountText;
    TMP_Text[] itemNumberText;
    GameObject[] SelectOutLine;
    GameObject[] gotItemCheck;


    Button adViewAndGetRubyBtn;

    // 알림 심볼
    GameObject simBall;

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        // 하이라키 Ref
        worldUiref = GameManager.inst.WorldUiRef;
        worldFrontRef = GameManager.inst.FrontUiRef;

        dailyCheckObjRef = worldFrontRef.transform.Find("DailyCheck").gameObject;
        dailyWindowRef = dailyCheckObjRef.transform.Find("Window").gameObject;
        ItemListRef = dailyWindowRef.transform.Find("RubyList").gameObject;

        // Init 필요 오브젝트
        itemCount = ItemListRef.transform.childCount;
        itemCountText = new TMP_Text[itemCount];
        SelectOutLine = new GameObject[itemCount];
        gotItemCheck = new GameObject[itemCount];
        itemNumberText = new TMP_Text[itemCount];

        for (int index = 0; index < itemCount; index++)
        {
            itemCountText[index] = ItemListRef.transform.GetChild(index).Find("InBox/CountText").GetComponent<TMP_Text>();
            itemNumberText[index] = ItemListRef.transform.GetChild(index).Find("InBox/NumberText").GetComponent<TMP_Text>();
            SelectOutLine[index] = ItemListRef.transform.GetChild(index).Find("OutLine").gameObject;
            gotItemCheck[index] = ItemListRef.transform.GetChild(index).Find("Get_Active").gameObject;
        }



        // 버튼
        xBtn = dailyWindowRef.transform.Find("Title/X_Btn").GetComponent<Button>();
        adViewAndGetRubyBtn = dailyWindowRef.transform.Find("ShowADBtn").GetComponent<Button>();

        // 수락 버튼부분 스위치
        GetBtn[0] = dailyWindowRef.transform.Find("TextLayOut/NoGet").gameObject;
        GetBtn[1] = dailyWindowRef.transform.Find("TextLayOut/Got").gameObject;
        mainTaxt = GetBtn[0].GetComponent<TMP_Text>();

        simBall = worldUiref.transform.Find("StageUI/DailyCheck/SimBall").gameObject;
        BtnInIt();
    }
    private void Start()
    {
        
    }
    private void Update()
    {

    }
    private void BtnInIt()
    {
        xBtn.onClick.AddListener(() => MainWindow_Acitve(false));


        adViewAndGetRubyBtn.onClick.AddListener(() => //광고 버튼
        {
            ADViewManager.inst.AdMob_ActiveAndFuntion(() =>
            {
                GameStatus.inst.PlusRuby(100);
                WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "루비 +100");
                GameStatus.inst.DailyADRuby = true;
                adViewAndGetRubyBtn_Init(false);
            });
        });
    }

    
    /// <summary>
    /// 출석체크 호출
    /// </summary>
    /// <param name="value"></param>
    public void MainWindow_Acitve(bool value)
    {
        if (value)
        {
            AudioManager.inst.Play_Ui_SFX(4, 0.8f);
        }
        else
        {
            AudioManager.inst.Play_Ui_SFX(3, 0.8f);
        }

        dailyCheckObjRef.SetActive(value);
    }

    /// <summary>
    /// 출석체크 보상 활성화 및 초기화 함수
    /// </summary>
    /// <param name="Boolian"> true / false </param>
    public void DialyContent_Init(bool Boolian)
    {
        AutoDialyCheckReset();
        DailyCheck_Material_Init();

        GetBtnAcitve(!Boolian);
        simBall.SetActive(!Boolian);

        adViewAndGetRubyBtn_Init(!GameStatus.inst.DailyADRuby); // 하단부 광고버튼 (1일에 한번 열림) => 다른곳이사

        if (Boolian == false)
        {
            //루비 계산 (적어놓은 텍스트에서 빼옴)
            int valueIndex = GameStatus.inst.GotDaily_Reward % 20;
            int value = int.Parse(ItemListRef.transform.GetChild(valueIndex).Find("InBox/CountText").GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());
            mainTaxt.text = $"  < {GameStatus.inst.GotDaily_Reward + 1}번째 > 출석체크 보상받기\r\n - 보상은 <color=green>우편함</color>으로 발송됩니다.";

            GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.RemoveAllListeners();
            GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioManager.inst.Play_Ui_SFX(4, 1);
                LetterManager.inst.MakeLetter(0, "게임GM", $"출석체크 {GameStatus.inst.GotDaily_Reward + 1}일차 보상", value); // 보상 우편 획득

                GameStatus.inst.TotayGotDaily_Reward = true;
                simBall.SetActive(false);
                GameStatus.inst.GotDaily_Reward++; // 받은 카운트 올려줌

                //버튼인잇
                DailyCheck_Material_Init();
                GetBtnAcitve(false); // 버튼 비활성화
            });
        }
    }

    /// <summary>
    /// 출석체크 보상 현재 상태 Init
    /// </summary>
    public void DailyCheck_Material_Init()
    {
        int count = GameStatus.inst.GotDaily_Reward % 20;
        if(count == 0)
        {
            AutoDialyCheckReset();

            //받아진거까지 체크표시
            for (int index = 0; index < itemCount; index++)
            {
                gotItemCheck[index].SetActive(false);
            }

            //현재 항목 표시
            for (int index = 0; index < itemCount; index++)
            {
                if (index == 0)
                {
                    SelectOutLine[index].SetActive(true);
                }
                else
                {
                    SelectOutLine[index].SetActive(false);
                }
            }
        }

        //받아진거까지 체크표시
        for (int index = 0; index < count; index++)
        {
            gotItemCheck[index].SetActive(false);

            if (index < count)
            {
                gotItemCheck[index].SetActive(true);
            }
        }

        //현재 항목 표시
        for (int index = 0; index < itemCount; index++)
        {
            if (index == count)
            {
                SelectOutLine[index].SetActive(true);
            }
            else
            {
                SelectOutLine[index].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 출석체크 하단 광고버튼 활성 / 비활성
    /// </summary>
    /// <param name="value"> Active ? </param>
    private void adViewAndGetRubyBtn_Init(bool value)
    {
        if (value)
        {
            adViewAndGetRubyBtn.GetComponent<Image>().sprite = adBtnSprite[0]; //스프라이트 교체
            adViewAndGetRubyBtn.onClick.RemoveAllListeners(); 
            adViewAndGetRubyBtn.onClick.AddListener(() => //광고 버튼
            {
                ADViewManager.inst.AdMob_ActiveAndFuntion(() =>
                {
                    GameStatus.inst.PlusRuby(100);
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "루비 +100");
                    GameStatus.inst.DailyADRuby = true;
                    adViewAndGetRubyBtn_Init(false);
                });
            });
        }
        else
        {
            adViewAndGetRubyBtn.GetComponent<Image>().sprite = adBtnSprite[1]; //스프라이트 교체
            adViewAndGetRubyBtn.onClick.RemoveAllListeners(); // 버튼기능 -> 창종료로 바꿈
            adViewAndGetRubyBtn.onClick.AddListener(() => MainWindow_Acitve(false));
        }
    }

    /// <summary>
    /// 출석보상 아이콘박스 번호 및 아이템 종료 및 갯수 초기화
    /// </summary>
    public void AutoDialyCheckReset()
    {
        if(GameStatus.inst.GotDaily_Reward % 20 == 0)
        {
            GameStatus.inst.MakeDailyRewardCount = GameStatus.inst.GotDaily_Reward;

        }

        int startCount = GameStatus.inst.MakeDailyRewardCount + 1;

        for(int index=0;  index< itemCount; index++) 
        {
            //박스의 번호 초기화
            itemNumberText[index].text = startCount.ToString();

            //박스의 루비 갯수 초기화
            itemCountText[index].text = $"루비 +{startCount * 10}";
            startCount++;
        }
    }

    /// <summary>
    /// 하단부 버튼
    /// </summary>
    /// <param name="value"></param>
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

}
