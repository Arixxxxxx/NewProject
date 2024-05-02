using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewGatchaContent : MonoBehaviour
{
    public static CrewGatchaContent inst;

    int[] materialCount = new int[3]; // 영혼 , 뼈, 책

    //Ref 아이템이미지
    [Header("# Input Material <Color=yellow>( Sprite File )</Color>")]
    [Space]
    [SerializeField] Sprite[] crewMaterialItemIMG;

    //Ref
    GameObject frontUi, crewGatchaRef, window;
    GameObject boxLayout; // 상자 박스함

    // 자원현황창
    TMP_Text[] materialCountText = new TMP_Text[3];

    //Btn
    Button xBtn;
    Button allOpenBtn;
    Button closeBtn;

    // 구매버튼
    GameObject gatchaBoxBg;

    // 갓챠 박스들
    
    Button[] gatchaBox;
    BoxPrefabs[] gatchaBoxSc;


    //1회뽑기, 5회 9회 광고
    int openCount; //상자 연 횟수
    int setCount;
    public int OpenCount
    {
        get { return openCount; }
        set
        {
            openCount = value;

            if (setCount == openCount && setCount > 0) // 상자를 연횟수가 모든 횟수와 같다면 확인 버튼 팝업
            {
                StartCoroutine(PopupCloseWindow());
                
            }
        }
    }

    int createBoxCount;
    public int CreateBoxCount
    {
        get { return createBoxCount; }
        set
        {
            createBoxCount = value;
            if (createBoxCount == setCount)
            {
                allOpenBtn.gameObject.SetActive(true);
            }
        }
    }

    // 구매버튼
    Button[] buyBtn = new Button[4];
    GameObject noCoolBtnRef;
    GameObject coolBtnRef;
    TMP_Text adCoolTImeText;

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

        frontUi = GameManager.inst.FrontUiRef;
        crewGatchaRef = frontUi.transform.Find("CrewMaterialGatcha").gameObject;
        window = crewGatchaRef.transform.Find("Window").gameObject;
        boxLayout = window.transform.Find("Main/Box_LayOut").gameObject;

        gatchaBoxBg = window.transform.Find("Main/Don'tClick").gameObject;
        gatchaBox = boxLayout.transform.GetComponentsInChildren<Button>(true);
        gatchaBoxSc = boxLayout.transform.GetComponentsInChildren<BoxPrefabs>(true);

        //자원형황
        for (int index = 0; index < materialCountText.Length; index++)
        {
            materialCountText[index] = window.transform.Find("Material").GetChild(index).GetChild(1).GetComponent<TMP_Text>();
        }

        // 버튼들
        xBtn = window.transform.Find("Title/X_Btn").GetComponent<Button>();
        allOpenBtn = window.transform.Find("Main/AllOpenBtn").GetComponent<Button>();
        closeBtn = window.transform.Find("Main/CloseBtn").GetComponent<Button>();

        //구매버튼
        for (int index = 0; index < buyBtn.Length; index++)
        {
            buyBtn[index] = window.transform.Find("Bottom_Btn").GetChild(index).GetComponent<Button>();
        }

        noCoolBtnRef = buyBtn[3].transform.Find("noCoolTime").gameObject;
        coolBtnRef = buyBtn[3].transform.Find("CoolTime").gameObject;
        adCoolTImeText = coolBtnRef.transform.GetChild(1).GetComponent<TMP_Text>();

        BtnInit();
        MaterialTextBarUpdate(); // 최초 받아온값 초기화
    }

    void Start()
    {

    }

    private void Update()
    {
        AdviewTimeChaker();
    }

    WaitForSeconds waitPopup = new WaitForSeconds(1.25f);
    IEnumerator PopupCloseWindow()
    {
        allOpenBtn.gameObject.SetActive(false);
        yield return waitPopup;
        closeBtn.gameObject.SetActive(true);
    }

    private void BtnInit()
    {
        xBtn.onClick.AddListener(() =>
        {
            ShopManager.Instance.SetShopActive(false);
            CrewMaterialGatchaActive(false);
        });

        // 모두 열기
        allOpenBtn.onClick.AddListener(() =>
        {
            AllBoxOpen(setCount);
            StartCoroutine(PopupCloseWindow());
        });


        //  확인 (여기서 초기화 후 Disable 해도될듯)
        closeBtn.onClick.AddListener(() =>
        {
            setCount = 0;
            OpenCount = 0;
            createBoxCount = 0;
            AllBoxDisable(); // 프리펩 모두 Disable 처리

            closeBtn.gameObject.SetActive(false);
        });

        // 1회 뽑기 / 100원
        buyBtn[0].onClick.AddListener(() =>
        {
            RubyPayment.inst.RubyPaymentUiActive(100, () => ActiveGatchaBox(1));
        });

        // 5회 뽑기 / 400원
        buyBtn[1].onClick.AddListener(() =>
        {
            RubyPayment.inst.RubyPaymentUiActive(400, () => ActiveGatchaBox(5));
        });

        // 9회 뽑기 / 700원
        buyBtn[2].onClick.AddListener(() =>
        {
            RubyPayment.inst.RubyPaymentUiActive(700, () => ActiveGatchaBox(9));
        });

        // 광고 보고 3회 뽑기 / 100원 // 15분 쿨타임 추가
        buyBtn[3].onClick.AddListener(() =>
        {
            ADViewManager.inst.SampleAD_Active_Funtion(() => { ActiveGatchaBox(3); AdViewCoolTime(15); });
        });

    }


    /// <summary>
    /// 동료 강화재료 뽑기 UI Active 함수
    /// </summary>
    /// <param name="value"> true / false </param>
    public void CrewMaterialGatchaActive(bool value)
    {
        if (value)
        {
            MaterialTextBarUpdate(); // 아이템 정보 최신화
            crewGatchaRef.SetActive(true);
        }
        else
        {
            crewGatchaRef.SetActive(false);
        }
    }


    /// <summary>
    /// 자원추가 함수 (자동업데이트함수 포함)
    /// </summary>
    /// <param name="index"> 0영혼 / 1뼈 / 2 책</param>
    /// <param name="Value"> 아이템 갯수 </param>
    public void MaterialCountEditor(int index, int Value)
    {
        materialCount[index] += Value;
        MaterialTextBarUpdate();
    }

    /// <summary>
    /// 동료 자원 소모 (for 겸희)
    /// </summary>
    /// <param name="index"> 0영혼 / 1 뼈 / 2 책</param>
    /// <param name="Value"> 소모값 </param>
    public void Use_Crew_Material(int index, int Value)
    {
        if (materialCount[index] - Value < 0)
        {
            return;
        }
        materialCount[index] -= Value;
    }

    //자원창 업데이트
    public void MaterialTextBarUpdate()
    {
        materialCountText[0].text = materialCount[0].ToString();
        materialCountText[1].text = materialCount[1].ToString();
        materialCountText[2].text = materialCount[2].ToString();
    }

    /// <summary>
    /// Load시 SaveData Setting 용 매개변수 3개
    /// </summary>
    /// <param name="soul"></param>
    /// <param name="bone"></param>
    /// <param name="book"></param>
    public void Set_CrewMeterialData(int soul, int bone, int book)
    {
        materialCount[0] = soul;
        materialCount[1] = bone;
        materialCount[2] = book;
        MaterialTextBarUpdate();
    }

    /// <summary>
    /// 현재 동료강화재료 받아가는 함수
    /// </summary>
    /// <returns></returns>
    public int[] Get_CurCrewUpgreadMaterial() => materialCount;


    // 구매시 상자 활성화

    private void ActiveGatchaBox(int count)
    {
        setCount = count;
        gatchaBoxBg.gameObject.SetActive(true); // 백그라운드 켜줌
        StartCoroutine(BoxAction(count));
    }

    // 상자 여는 쿨타임
    WaitForSeconds boxincount = new WaitForSeconds(0.2f);
    IEnumerator BoxAction(int count)
    {
        for (int index = 0; index < count; index++)
        {
            //랜덤값 생성
            int ranType = Random.Range(0, 3);

            int criticalDice = Random.Range(0, 100);
            
            int ranitemCount = 0;

            if (criticalDice <= 5) // 5% 확률로 재료 뻥튀기
            {
                ranitemCount = Random.Range(150, 300);
            }
            else if(criticalDice > 5)
            {
                ranitemCount = Random.Range(20, 99);
            }


            //갑 초기화
            gatchaBoxSc[index].Set_MaterialCount(crewMaterialItemIMG[ranType], ranType, ranitemCount);
            gatchaBox[index].gameObject.SetActive(true);
            CreateBoxCount++;
            yield return boxincount;
        }

        yield return boxincount;

        allOpenBtn.gameObject.SetActive(true);
    }


    // 모두 열기
    private void AllBoxOpen(int count)
    {
        for (int index = 0; index < count; index++)
        {
            gatchaBoxSc[index].OpenBox();
        }
    }

    private void AllBoxDisable()
    {
        if (gatchaBoxBg.gameObject.activeSelf)
        {
            gatchaBoxBg.gameObject.SetActive(false); // 배경꺼줌
        }

        for (int index = 0; index < gatchaBoxSc.Length; index++)
        {
            gatchaBoxSc[index].gameObject.SetActive(false);
        }
    }
    // 모두 확인시 확인버튼 활성화 & 모두수락버튼 없애기

    float coolTime;
    /// <summary>
    /// 광고 뽑기 쿨타임 시간 체워주는 함수
    /// </summary>
    /// <param name="Min"> Time = Min </param>
    private void AdViewCoolTime(float Min)
    {
        coolTime = Min * 60;
    }

    /// <summary>
    /// 광고 쿨타임 체커
    /// </summary>
    private void AdviewTimeChaker()
    {
        if (coolTime <= 0) // 기본
        {
            if (noCoolBtnRef.activeSelf == false)
            {
                coolTime = 0;
                coolBtnRef.SetActive(false);
                noCoolBtnRef.SetActive(true);
                buyBtn[3].interactable = true;
            }
        }
        else if (coolTime > 0)  // 쿨돌때
        {
            if (coolBtnRef.activeSelf == false)
            {
                coolBtnRef.SetActive(true);
                noCoolBtnRef.SetActive(false);
                buyBtn[3].interactable = false;
            }

            coolTime -= Time.deltaTime;
            int min = (int)coolTime / 60;
            int sec = (int)coolTime % 60;
            adCoolTImeText.text = $"{min} : {sec}";
        }
    }
}
