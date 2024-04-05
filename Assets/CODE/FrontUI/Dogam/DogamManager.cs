using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class DogamManager : MonoBehaviour
{
    public static DogamManager inst;

    // Ref
    GameObject frontUI, mainMenuRef, windowRef, weaponInfoRef, monsterInfoRef;
    Button xBtn;


    ////////////////////// 무기 도감

    // Input Sprite
    [SerializeField][Tooltip("0 = 활성화 / 1 = 비활성화")] Sprite[] topArrayBtnSpirte;
    [SerializeField][Tooltip("0 = 셀렉트 / 1 = 비선택 / 2 = 컬렉션")] Sprite[] weaponIcon;

    // Spirte Color
    [SerializeField][Tooltip("0 = 활성화 / 1 = 비활성화")] Color[] weaponColor;

    // Top Array Btn
    Button[] topArrayBtn;
    Image[] topArrayImg;

    // Main Info
    Image charactorWeaponIMG; // 무기 이미지
    TMP_Text[] weaponInfoText = new TMP_Text[2];

    // ViewrSlot
    TMP_Text viewrTitleText;
    DogamWeaponSlot[] weaponSlot;

    //Bottom Imfo
    TMP_Text bonusAttackDMGText;
    TMP_Text curGotCountText;

    RectTransform[] scrollViewRectTrs = new RectTransform[2];

    // 이전 클릭 확인용
    int beforeWeaponSelectNum = -1;
    public int BeforeWeaponSelectNum { get { return beforeWeaponSelectNum; } }

    int beforeMonsterSelectNum = -1;
    public int BeforeMonsterSelectNum { get { return beforeMonsterSelectNum; } }

    // 수집 유/무 데이터확인용
    bool[] isGotThisWeapon;
    int gotCount; // => 데미지 곱연산
    public int weaponDogamGetCount
    {
        get { return gotCount; }
    }

    // 무기 컬렉션 정보
    public bool[] IsgotThisWeapon// <=== 나중에 싸서 올려야됨 ★★★★
    {
        get { return isGotThisWeapon; }
        set { IsgotThisWeapon = value; }
    }

    // 몬스터 컬렉션 정보
    int[] mosterCollectionConut;     // <=== 나중에 싸서 올려야됨★★★★
    public int[] MonsterColltionCount
    {
        get { return mosterCollectionConut; }
        set { mosterCollectionConut = value; }
    }



    // 무기정보
    string[,] weaponInfoTextLog;




    ////////////////////// 몬스터 도감

    // Main Info
    Image monsterIMG;
    TMP_Text[] mosterInfoText = new TMP_Text[2];
    DogamMonsterSlot[] monsterSlot;

    // 몬스터정보
    string[,] mosterInfoTextLog;

    // Bottom Text
    TMP_Text bonusEnemyDogamAtkText;
    TMP_Text curEnemyDogamContText;

    int monsterCollectionCount;




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

        // Ref
        frontUI = GameObject.Find("---[FrontUICanvas]").gameObject;
        mainMenuRef = frontUI.transform.Find("Dogam").gameObject;
        windowRef = mainMenuRef.transform.Find("Window").gameObject;
        weaponInfoRef = windowRef.transform.Find("Weapon").gameObject;
        monsterInfoRef = windowRef.transform.Find("Monster").gameObject;


        // Title 
        xBtn = mainMenuRef.transform.Find("Window/Title/X_Btn").GetComponent<Button>();

        // TopSelectCartegory Btn
        topArrayBtn = windowRef.transform.Find("TopBtnArray").GetComponentsInChildren<Button>();
        topArrayImg = windowRef.transform.Find("TopBtnArray").GetComponentsInChildren<Image>();

        // MainInfo
        charactorWeaponIMG = weaponInfoRef.transform.Find("WeaponMainInfo/BG/Weapon").GetComponent<Image>();
        weaponInfoText[0] = weaponInfoRef.transform.Find("WeaponMainInfo/Box/WeaponText").GetComponent<TMP_Text>();
        weaponInfoText[1] = weaponInfoRef.transform.Find("WeaponMainInfo/Box/WeaponInfoText").GetComponent<TMP_Text>();
        viewrTitleText = weaponInfoRef.transform.Find("WeaponList/DogamTitle/TitleText").GetComponent<TMP_Text>();

        monsterIMG = monsterInfoRef.transform.Find("MainInfo/BG/IMG").GetComponent<Image>();
        mosterInfoText[0] = monsterInfoRef.transform.Find("MainInfo/Box/name").GetComponent<TMP_Text>();
        mosterInfoText[1] = monsterInfoRef.transform.Find("MainInfo/Box/Info").GetComponent<TMP_Text>();

        // Slot
        weaponSlot = weaponInfoRef.transform.Find("WeaponList/Scroll View/Viewport/Content").GetComponentsInChildren<DogamWeaponSlot>();
        scrollViewRectTrs[0] = weaponInfoRef.transform.Find("WeaponList/Scroll View/Viewport/Content").GetComponent<RectTransform>();

        weaponInfoTextLog = new string[weaponSlot.Length, 2];
        isGotThisWeapon = new bool[weaponSlot.Length];

        monsterSlot = monsterInfoRef.transform.Find("EnemyList/Scroll View/Viewport/Content").GetComponentsInChildren<DogamMonsterSlot>();
        scrollViewRectTrs[1] = monsterInfoRef.transform.Find("EnemyList/Scroll View/Viewport/Content").GetComponent<RectTransform>();

        mosterInfoTextLog = new string[monsterSlot.Length, 2];
        MonsterColltionCount = new int[monsterSlot.Length];
        
        // Bottom Text
        bonusAttackDMGText = weaponInfoRef.transform.Find("BottomInfo/Top").GetComponent<TMP_Text>();
        curGotCountText = weaponInfoRef.transform.Find("BottomInfo/MiddleRight").GetComponent<TMP_Text>();

        bonusEnemyDogamAtkText = monsterInfoRef.transform.Find("BottomInfo/Top").GetComponent<TMP_Text>();
        curEnemyDogamContText = monsterInfoRef.transform.Find("BottomInfo/MiddleRight").GetComponent<TMP_Text>();
        //Test
        GetWeaponCheck(0); // 기본무기는 항상 컬렉션활성화
        


        BtnInit();
        WeaponTextInit(); // 무기 스트링데이터
        MonsterTextInit();// 몬스터 스트링데이터
      
    }

    private void BtnInit()
    {
        xBtn.onClick.AddListener(() => Set_DogamListAcitve(0, false));

        topArrayBtn[0].onClick.AddListener(() =>
        {
            Set_WeaponViewrInit();
        });
        topArrayBtn[1].onClick.AddListener(() =>
        {
            Set_MosterViewrInit();
        });
    }

    private void Start()
    {
        EnemyDogam_TextAnd_Count_Init(); //최초 도감정보 최신화해줌
    }
    /// <summary>
    ///  도감창 Active 
    /// </summary>
    /// <param name="indextype"> 0 = 무기도감 / 1 = 몬스터도감 </param>
    /// <param name="value"> true / false </param>
    public void Set_DogamListAcitve(int indextype, bool value)
    {
        if (value == true && indextype == 0)
        {
            beforeMonsterSelectNum = 0;
            Set_WeaponViewrInit();
        }
        else if (value == true && indextype == 1)
        {
            beforeMonsterSelectNum = 0;
            Set_MosterViewrInit();
        }

        if (value == false)
        {
            //스크롤바 위치 초기화 및 이전 클릭번호 초기화

            beforeWeaponSelectNum = -1;
            beforeMonsterSelectNum = -1;
            Vector3 reposition0 = new Vector3(scrollViewRectTrs[0].position.x , 0);
            Vector3 reposition1 = new Vector3(scrollViewRectTrs[1].position.x , 0);    

            scrollViewRectTrs[0].position = reposition0;
            scrollViewRectTrs[1].position = reposition1;
        }

        mainMenuRef.SetActive(value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {

        }
    }



    //////////////////////////////////////////////////////////////
    ///////////////////////  무기 도감 관련 ///////////////////////
    //////////////////////////////////////////////////////////////

    // 무기 도감 활성화 및  초기화
    private void Set_WeaponViewrInit()
    {

        topArrayImg[0].sprite = topArrayBtnSpirte[0];
        topArrayImg[1].sprite = topArrayBtnSpirte[1];

        if (monsterInfoRef.activeSelf == true)
        {
            monsterInfoRef.SetActive(false);
        }

        if (beforeWeaponSelectNum == -1) // 최초실행때만
        {
            for (int index = 0; index < weaponSlot.Length; index++)
            {
                weaponSlot[index].ResetIconBoxLayout();
            }
        }

        weaponInfoRef.SetActive(true);

    }




    /// <summary>
    /// 버튼에서 메인창 캐릭터의 무기이미지를 교체 및 자기 번호 전송
    /// </summary>
    /// <param name="img"></param>
    public void CharactorWeaponImgChangerAndNumber(Sprite img, int Num)
    {
        charactorWeaponIMG.sprite = img; // 메인캐릭터 무기바꿔줌
        MainWeaponInfoChanger(Num); // 메인창 무기정보 바꿔줌

        if (beforeWeaponSelectNum != -1) //아이콘박스 이전눌럿던것만 바꿔줌
        {
            weaponSlot[beforeWeaponSelectNum].ResetIconBoxLayout();
        }

        beforeWeaponSelectNum = Num;
    }



    /// <summary>
    /// 메인창 무기이름 , 정보 변경
    /// </summary>
    /// <param name="clickNum"></param>
    public void MainWeaponInfoChanger(int clickNum)
    {
        if (IsgotThisWeapon[clickNum] == true)
        {
            weaponInfoText[0].text = weaponInfoTextLog[clickNum, 0];
            weaponInfoText[1].text = weaponInfoTextLog[clickNum, 1];
        }
        else
        {
            weaponInfoText[0].text = " ??? ";
            weaponInfoText[1].text = "획득한적이 없어서 정보가 없다.";
        }
    }



    /// <summary>
    /// 무기 업그레이드시 최초 1회 획득정보 저장 (겸희가 해줘야댐)
    /// </summary>
    /// <param name="indexNum">무기인덱스 번호</param>
    public void GetWeaponCheck(int indexNum)
    {
        if (IsgotThisWeapon[indexNum] == true) { return; }

        IsgotThisWeapon[indexNum] = true; // 획득 정보 저장

        gotCount = IsgotThisWeapon.Count(x => x== true); // 총 획득량 확인

        bonusAttackDMGText.text = $"보너스 공격력 {gotCount}%";
        curGotCountText.text = $"{gotCount} / {IsgotThisWeapon.Length}";
    }




    //////////////////////////////////////////////////////////////
    ///////////////////////  몬스터 도감 관련 ///////////////////////
    //////////////////////////////////////////////////////////////


    // 몬스터 도감 활성화 및 초기화
    private void Set_MosterViewrInit()
    {
        topArrayImg[0].sprite = topArrayBtnSpirte[1];
        topArrayImg[1].sprite = topArrayBtnSpirte[0];

        if (weaponInfoRef.activeSelf == true)
        {
            weaponInfoRef.SetActive(false);
        }


        for (int index = 0; index < monsterSlot.Length; index++)
        {
            monsterSlot[index].ResetIconBoxLayout(); //아이콘리셋 
        }

        EnemyDogam_TextAnd_Count_Init(); // 몬스터 도감정보 초기화
        monsterInfoRef.SetActive(true);
    }

    public void MonsterIMGChanger(Sprite img, int myNum)
    {

        if (beforeMonsterSelectNum != myNum)
        {
            int temp = beforeMonsterSelectNum;
            beforeMonsterSelectNum = myNum;
            monsterSlot[temp].ResetIconBoxLayout(); // 이전 클릭했던것 아웃라인 리셋
        }

        monsterIMG.sprite = img; // 스프라이트 교체
        MainMosterInfoChanger(myNum); // 텍스트 교체

        beforeMonsterSelectNum = myNum;
    }


    /// <summary>
    /// 메인창 몬스터 이름 , 정보 변경
    /// </summary>
    /// <param name="clickNum"></param>
    public void MainMosterInfoChanger(int clickNum)
    {
        if (MonsterColltionCount[clickNum] == 50)
        {
            mosterInfoText[0].text = mosterInfoTextLog[clickNum, 0];
            mosterInfoText[1].text = mosterInfoTextLog[clickNum, 1];
        }
        else
        {
            mosterInfoText[0].text = " ??? ";
            mosterInfoText[1].text = "도감이 완료되어야 출력됩니다.";
        }
    }

    /// <summary>
    /// 몬스터 처치에 꼽아줘야함
    /// </summary>
    /// <param name="indexNum"></param>
    public void Get_EnemyElement(int indexNum)
    {
        if (MonsterColltionCount[indexNum] == 50) { return; }

        MonsterColltionCount[indexNum]++;
        monsterSlot[indexNum].Set_CollectionCount(MonsterColltionCount[indexNum]); // 도감정보도 같이 업데이트
    }

    /// <summary>
    /// 몬스터 도감 최하단 Init  나중에 캐릭터정보 초기화시 함수실행시켜서 초기화해줘야할듯
    /// </summary>
    public void EnemyDogam_TextAnd_Count_Init()
    {
        monsterCollectionCount = MonsterColltionCount.Count(x => x == 50);
        int enmeyLength = MonsterColltionCount.Length;

        // 도감창 하단부
        bonusEnemyDogamAtkText.text = $"보너스 공격력 {monsterCollectionCount}%";
        curEnemyDogamContText.text = $"{monsterCollectionCount} / {enmeyLength}";

        //도감창 각 캐릭터별 하단 텍스트 초기화
        for (int index = 0; index < enmeyLength; index++)
        {
            monsterSlot[index].Set_CollectionCount(MonsterColltionCount[index]);
        }
    }


    //////////////////////////////////////////////////////////////
    ///////////////////////  스트링 정보        ///////////////////////
    //////////////////////////////////////////////////////////////

    // 1. 무기도감 메인인포에 뜨는 텍스트정보 0 = 이름 1 = 내용
    private void WeaponTextInit()
    {
        weaponInfoTextLog[0, 0] = "초심자의 검";
        weaponInfoTextLog[0, 1] = "길 지나가다가 사은품으로 받은 아주 기초적인 검";

        weaponInfoTextLog[1, 0] = "휴지통에서 주은검";
        weaponInfoTextLog[1, 1] = "우리집 휴지통에 누군가 버린검 \n <color=yellow><b>(대체 누가버린거야?)";

        weaponInfoTextLog[2, 0] = "탕후루";
        weaponInfoTextLog[2, 1] = "왕가탕후루를 보고 따라만든 탕후루";

        weaponInfoTextLog[3, 0] = "요망한 검";
        weaponInfoTextLog[3, 1] = "요망한대장장이가 만든 요망한검";

        weaponInfoTextLog[4, 0] = "겸희검";
        weaponInfoTextLog[4, 1] = "CU편의점에서 판매하는검";

        weaponInfoTextLog[5, 0] = "동은검";
        weaponInfoTextLog[5, 1] = "SSS등급의 초레어칼";

        weaponInfoTextLog[6, 0] = "질럿광선검";
        weaponInfoTextLog[6, 1] = "질럿이 쓰던 사이오닉광선검";

    }

    // 2. 몬스터도감 메인인포에 뜨는 텍스트정보 0 = 이름 1 = 내용
    private void MonsterTextInit()
    {
        mosterInfoTextLog[0, 0] = "쥐돌이";
        mosterInfoTextLog[0, 1] = "동굴에서 흔히 볼수있는 쥐";

        mosterInfoTextLog[1, 0] = "애자";
        mosterInfoTextLog[1, 1] = "딱 봐도 애자같이 생겨서 애자임";
    }

    
}
