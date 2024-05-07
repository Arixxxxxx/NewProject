using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;

public class GameStatus : MonoBehaviour
{
    public static GameStatus inst;


    ///////////////////////////////////////////////////
    // 신규 접속 기록
    private DateTime signUpDate;
    public DateTime signDownDate { get { return signUpDate; } set { signUpDate = value; } }


    // 마지막 접속기록
    private DateTime lastLogindate;
    public DateTime LastLoginDate
    {
        get { return lastLogindate; }
        set
        {
            DateTime LoginDate = value;

            //접속시 1회
            // 첫 접속시 (신규가입시)
            if (LoginDate.Year == 1)
            {
                signUpDate = value;
                DailyPlayCheckUIManager.inst.DialyContent_Init(TotayGotDaily_Reward);
            }
            else if (LoginDate.Year >= 2000)
            {
                //하루가 지남
                if (LoginDate.Date > lastLogindate.Date)
                {
                    //출석체크 부분
                    if (TotayGotDaily_Reward == true)
                    {
                        TotayGotDaily_Reward = false;
                        DailyADRuby = true;
                    }

                    DailyPlayCheckUIManager.inst.DialyContent_Init(TotayGotDaily_Reward);
                }

                lastLogindate = value;
            }
        }
    }


    private string nickName;
    public string NickName { get { return nickName; } set { nickName = value; } }
    /////////////////////[ 출석체크 현황 및  뉴비]//////////////////////////////

    // 1.  선물받은 년/월/일
    DateTime getGiftDay;
    public DateTime GetGiftDay { get { return getGiftDay; } set { getGiftDay = value; } }


    // 3.  뉴비 선물 받은 횟수
    int gotNewbieGiftCount;
    public int GotNewbieGiftCount
    {
        get { return gotNewbieGiftCount; }
        set
        {
            gotNewbieGiftCount = value;


            if (GotNewbieGiftCount == 7)
            {
                WorldUI_Manager.inst.NewbieBtnAcitveFalse(); // 선물 다 받으면 버튼 잠궈버리기 
            }
        }
    }


    // 오늘 받았는지 확인
    bool todayGetNewBie_Reward;
    public bool TodayGetNewbie_Reward { get { return todayGetNewBie_Reward; } set { todayGetNewBie_Reward = value; } }


    // 뉴비 기간 체크
    private DateTime newbieBuffLastDay;
    public DateTime NewbieBuffLastDay
    {
        get
        {
            return newbieBuffLastDay;
        }
        set
        {
            DateTime ReLoginDate = value;
            // 최초 가입이라면
            if (ReLoginDate.Month == 0001)
            {
                // 뉴비 기간 7일 입력
                newbieBuffLastDay = DateTime.Now.AddDays(7);

                // 보상 활성화
                Newbie_Content.inst.NewbieWindow_Init(TodayGetNewbie_Reward);

                // 버프 초기화
                TimeSpan buffTime = newbieBuffLastDay - DateTime.Now;
                BuffContoller.inst.ActiveBuff(4, buffTime.TotalMinutes);
            }


        }
    }


    // 3.  출석체크 선물 받은 횟수
    int makeDailyRewardcount;
    public int MakeDailyRewardCount
    {
        get { return makeDailyRewardcount; }
        set { makeDailyRewardcount = value; }
    }

    int gotDaily_Reward; // 
    public int GotDaily_Reward
    {
        get { return gotDaily_Reward; }
        set
        {
            gotDaily_Reward = value;
        }
    }
    bool dailyADRuby; // 오늘 받은지 안받은치 체크
    public bool DailyADRuby
    {
        get { return dailyADRuby; }
        set { dailyADRuby = value; }
    }
    bool totayGotDailyGift; // 오늘 받은지 안받은치 체크
    public bool TotayGotDaily_Reward
    {
        get { return totayGotDailyGift; }
        set { totayGotDailyGift = value; }
    }

    // 5. 뉴비 변수들
    bool isNewbie = false;
    public bool IsNewBie
    {
        get { return isNewbie; }
        set { isNewbie = value; }
    }

    // 5-1. 뉴비 (공격력)
    string newbieATKBuffValue = "0";
    public string NewbieATKBuffValue { get { return newbieATKBuffValue; } set { newbieATKBuffValue = value; } }

    // 5-2. 뉴비 (이속)
    float newbieMoveSpeedBuffValue = 0;

    public float NewbieMoveSpeedBuffValue { get { return newbieMoveSpeedBuffValue; } set { newbieMoveSpeedBuffValue = value; ActionManager.inst.SetPlayerMoveSpeed(); } }

    // 5-3. 뉴비 (골드량)
    string newbieGoldBuffValue = "0";

    public string NewbieGoldBuffValue { get { return newbieGoldBuffValue; } set { newbieGoldBuffValue = value; } }

    // 5-4. 뉴비 ( 공격속드)
    float newbieAttackSpeed = 0;

    public float NewbieAttackSpeed { get { return newbieAttackSpeed; } set { newbieAttackSpeed = value; ActionManager.inst.PlayerAttackSpeedLvUp(); } }

    // 5-5 뉴비 공격 100회당 공격력 2배 증가 (최대 20중 / 몬스터 사망시 초기화) 버프
    private int newbieAttackCount;


    /// <summary>
    /// 뉴비버프 어택카운트
    /// </summary>
    /// <param name="value"> ture = up / false = 0</param>
    public void NewbieAttackCountUp(bool value)
    {
        if (IsNewBie == false) { return; }
        if (value == true)
        {
            newbieAttackCount++;
        }
        else
        {
            newbieAttackCount = 0;
        }
    }
    public int Get_NewBieAttackBuff_MultiplyValue()
    {
        if (IsNewBie == true)
        {
            if (newbieAttackCount >= 2000)
            {
                newbieAttackCount = 2000;
            }

            return (newbieAttackCount / 100) * 2 > 0 ? (newbieAttackCount / 100) * 2 : 1;
        }

        return 1;
    }

    /////////////////////[ 플에이어 재화 변수]//////////////////////////////

    // 1. 소지 골드
    [HideInInspector] public UnityEvent OnGoldChanged;
    string gold = "0";
    public string Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
            WorldUI_Manager.inst.CurMaterialUpdate(0, gold);
            OnGoldChanged?.Invoke();
        }
    }

    // 2. 소지 별
    [HideInInspector] public UnityEvent OnStartChanged;
    string star = "0"; // 환생시 주는 화폐
    public string Star
    {
        get
        {
            return star;
        }
        set
        {
            star = value;
            WorldUI_Manager.inst.CurMaterialUpdate(1, star);
            OnStartChanged?.Invoke();
        }
    }

    // 3. 소지 키
    string key = "0";
    public string Key
    {
        get
        {
            return key;
        }
        set
        {
            key = value;
            WorldUI_Manager.inst.CurMaterialUpdate(2, key);
        }
    }

    [HideInInspector] public UnityEvent OnRubyChanged;
    // 4. 소지 루비
    int ruby = 0;
    public int Ruby
    {
        get
        {
            return ruby;
        }

        set
        {
            int UseRuby = ruby - value;
            if (UseRuby > 0)
            {
                MissionData.Instance.SetDailyMission("루비 사용", UseRuby);
            }
            ruby = value;
            WorldUI_Manager.inst.CurMaterialUpdate(3, ruby.ToString("N0"));
            OnRubyChanged?.Invoke();
        }
    }

    // 5. 초당 골드 생산량
    BigInteger totalProdGold = 10;
    public BigInteger TotalProdGold
    {
        get => totalProdGold;
        set
        {
            totalProdGold = value;
            UIManager.Instance.SettotalGoldText(CalCulator.inst.StringFourDigitAddFloatChanger(totalProdGold.ToString()));
            //UIManager.Instance.SettotalGoldText();
        }
    }
    public string GetTotalGold()
    {
        return TotalProdGold.ToString();
    }

    /////////////////////[ 플레이어 관련 변수 ]//////////////////////////////

    // 1. 공격력
    BigInteger totalAtk = 5;
    public BigInteger TotalAtk
    {
        get => totalAtk;
        set
        {
            totalAtk = value;
            UIManager.Instance.SetAtkText(CalCulator.inst.StringFourDigitAddFloatChanger(CalCulator.inst.Get_CurPlayerATK()));
        }
    }


    // 2. 공격속도
    int atkSpeedLv;

    public int AtkSpeedLv
    {
        get { return atkSpeedLv; }
        set
        {
            atkSpeedLv = value;
            ActionManager.inst.PlayerAttackSpeedLvUp();
        }
    }

    // 2. 크리티컬 확률
    float criticalChance = 20;  // 기본값 20

    public float CriticalChance
    {
        get
        {
            return criticalChance + addPetCriChanceBuff;  // <= 펫 시전버프량
        }
        set { criticalChance = value; }
    }

    // 3. 크리티컬 피해증가
    float criticalPower = 0;
    public float CriticalPower
    {
        get
        { return criticalPower; } // <= 펫 시전버프량
        set
        {
            criticalChance = value;
        }
    }

    // 4. 몬스터 누적 킬수
    int totalEnemyKill;
    public int TotalEnemyKill
    {
        get { return totalEnemyKill; }
        set
        {
            totalEnemyKill = value;
            MissionData.Instance.SetDailyMission("몬스터 처치", 1);
        }
    }

    // 5. 환생횟수
    int hwansengCount;
    public int HWansengCount
    {
        get { return hwansengCount; }
        set
        {
            hwansengCount = value;
            //MissionData.Instance.SetWeeklyMission("환생하기",0);
        }
    }
    /////////////////////[ 스테이지 현황 ]//////////////////////////////

    // 1. 총 누적층수
    int accumlateFloor = 0;
    public int AccumlateFloor
    {
        get
        {
            return accumlateFloor;
        }

        set
        {
            accumlateFloor = value;
        }
    }

    // 2. 현재 스테이지
    int stageLv = 1; // 층수 
    public int StageLv
    {
        get
        {
            return stageLv;
        }

        set
        {
            stageLv = value;

        }
    }

    // 3. 현재 층수
    int floorLv = 1;
    public int FloorLv
    {
        get
        {
            return floorLv;
        }
        set
        {
            floorLv = value;

            if (floorLv == 6)
            {
                floorLv = 1;
                stageLv++;
            }
            else if(floorLv < 6)
            {
                WorldUI_Manager.inst.Set_StageUiBar(floorLv);
            }
            

            if (DataManager.inst.saveAble)
            {
                AccumlateFloor++;
                HwanSengSystem.inst.WorldUIHwansengIconReturnStarUpdate();

              
            }
        }
    }




    /////////////////////////////// 상점 버프 증가량 관련 //////////////////////////////////

    // 1. 공격력증가 버프
    string buffAddATK = "0";
    public string BuffAddATK { get { return buffAddATK; } set { buffAddATK = value; } }

    // 2. 화면 광고 공격력증가 
    string buffAddAdATK = "0";
    public string BuffAddAdATK { get { return buffAddAdATK; } set { buffAddAdATK = value; } }

    // 3. 골드증가 버프
    string buffAddGold = "0";
    public string BuffAddGold { get { return buffAddGold; } set { buffAddGold = value; } }

    // 4. 이동속도증가 버프
    float buffAddSpeed = 0;
    public float BuffAddSpeed
    {
        get
        {
            return buffAddSpeed;
        }
        set
        {
            buffAddSpeed = value;
            ActionManager.inst.SetPlayerMoveSpeed();
        }
    }



    /////////////////////////////// 펫 버프 증가량 관련 //////////////////////////////////

    // 1. 공격펫
    int pet0_Lv = 1;
    public int Pet0_Lv
    {
        get { return pet0_Lv; }
        set { pet0_Lv = value; }
    }

    // 2. 버프펫
    int pet1_Lv = 1;
    public int Pet1_Lv
    {
        get { return pet1_Lv; }
        set { pet1_Lv = value; }
    }

    // 2-1 펫의 공격력 버프
    string addPetAtkBuff = "0";
    public string AddPetAtkBuff
    {
        get { return addPetAtkBuff; }
        set { addPetAtkBuff = value; }
    }

    // 2-2 펫의 크리티컬확률 버프
    int addPetCriChanceBuff = 0;
    public int AddPetCriChanceBuff
    {
        get { return addPetCriChanceBuff; }
        set { addPetCriChanceBuff = value; }
    }

    // 3. 사령술사
    int pet2_Lv = 1;
    public int Pet2_Lv
    {
        get { return pet2_Lv; }
        set { pet2_Lv = value; }
    }

    /////////////////////////////// 하단 UI 데이터 //////////////////////////////////
    public UnityEvent OnQuestLvChanged;
    private int[] aryQuestLv = new int[30];

    public int GetAryQuestLv(int Num)
    {
        return aryQuestLv[Num];
    }

    public void SetAryQuestLv(int Num, int Value)
    {
        aryQuestLv[Num] = Value;
        MissionData.Instance.SetSpecialMission(Num, Value, SpMissionTag.Quest);
    }

    int[] aryWeaponLv = new int[30];

    public int GetAryWeaponLv(int Num)
    {
        return aryWeaponLv[Num];
    }

    int equipWeaponNum;//장착중인 무기 이미지 번호
    public int EquipWeaponNum
    {
        get => equipWeaponNum;
        set
        {
            equipWeaponNum = value;
            ActionManager.inst.Set_WeaponSprite_Changer(value);
        }
    }

    public void SetAryWeaponLv(int Num, int Value)
    {
        aryWeaponLv[Num] = Value;
        MissionData.Instance.SetSpecialMission(Num, Value, SpMissionTag.Weapon);
    }

    [HideInInspector] public UnityEvent OnPercentageChanged;
    float[] aryPercentage = new float[11];
    public float GetAryPercent(int index)
    {
        return aryPercentage[index];
    }
    public void SetAryPercent(int index, float value)
    {
        aryPercentage[index] = value;
        OnPercentageChanged?.Invoke();
    }

    int[] aryNormalRelicLv = new int[11];

    public int[] AryNormalRelicLv
    {
        get => aryNormalRelicLv;
        set { aryNormalRelicLv = value; }
    }

    private int minigameTicket;
    public int MinigameTicket { get { return minigameTicket; } set { minigameTicket = value; } }
    /////////////////////////////////////미션/////////////////////////////////////////

    int[] dailyMissionisCount = new int[4];
    public int[] DailyMIssionisCount { get => dailyMissionisCount; set { dailyMissionisCount = value; } }

    int[] weeklyMissionisCount = new int[4];
    public int[] WeeklyMissionisCount { get => weeklyMissionisCount; set { weeklyMissionisCount = value; } }

    int[] specialMissionisCount = new int[6];
    public int[] SpecialMissionisCount { get => specialMissionisCount; set { specialMissionisCount = value; } }

    bool[] dailyMIssionClear = new bool[4];
    public bool[] DailyMIssionClear { get => dailyMIssionClear; set { dailyMIssionClear = value; } }

    bool[] weeklyMIssionClear = new bool[4];
    public bool[] WeeklyMIssionClear { get => weeklyMIssionClear; set { weeklyMIssionClear = value; } }

    int specialMIssionClearNum;
    public int SpecialMIssionClearNum { get => specialMIssionClearNum; set { specialMIssionClearNum = value; } }

    bool isCanResetDailyMIssion;
    public bool IsCanResetDailyMIssion { get => isCanResetDailyMIssion; set { isCanResetDailyMIssion = value; } }

    bool isCanResetWeeklyMIssion;
    public bool IsCanResetWeeklyMIssion { get => isCanResetWeeklyMIssion; set { isCanResetWeeklyMIssion = value; } }

    bool[] bingoBoard = new bool[8];
    public bool[] BingoBoard { get => bingoBoard; set { bingoBoard = value; } }

    public UnityEvent OnRouletteTicketChanged;
    int rouletteTicket;
    public int RouletteTicket 
    {
        get => rouletteTicket;
        set 
        { 
            rouletteTicket = value;
            OnRouletteTicketChanged?.Invoke();
        } 
    }

    int rouletteStack;
    public int RouletteStack { get => rouletteStack; set { rouletteStack = value; } }

    /////////////////////////////////////////////////////////////////////////////////

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
        // 서버에서 데이터 받아와서 초기화해줌

        if (GameManager.inst.TestMode == false)
        {
            LoadData(); // <= 데이터 싸온거 푸는 함수 #######################################
        }

        // 접속날짜 확인하여 뉴비컨텐츠 초기화
        Newbie_ContentInit();

        // 뉴비 불리언변수 초기화
        IsNewBie = true;
    }

    int testInt;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            testInt++;
            DateTime now = DateTime.Now.AddDays(testInt);
            LastLoginDate = now;
        }
    }
    void Start() { }



    /// <summary>
    ///  UI창에 골드증가량 안뜨는 함수 ( 초당 골드 자동증가 )
    /// </summary>
    /// <param name="getValue"></param>
    public void GetGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue); // 기본 골드
        result = CalCulator.inst.DigidPlus(result, buffAddGold); // 상점 버프로인한값 추가
        result = CalCulator.inst.DigidPlus(result, newbieGoldBuffValue); // 뉴비 버프로인한값 추가
        Gold = result;
    }

    public void PlusStar(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(Star, getValue);
        WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(1, CalCulator.inst.StringFourDigitAddFloatChanger(getValue));
        Star = result;
    }

    /// <summary>
    /// UI창에 현재 골드생산된 량 이펙트 추가해서 올라가는 함수
    /// </summary>
    /// <param name="getValue"></param>
    public void PlusGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue);
        WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(0, CalCulator.inst.StringFourDigitAddFloatChanger(getValue));
        Gold = result;
    }

    public void MinusStar(string getValue)
    {
        string result = CalCulator.inst.BigIntigerMinus(star, getValue);
        Star = result;
    }

    public void MinusGold(string getValue)
    {
        string result = CalCulator.inst.BigIntigerMinus(gold, getValue);
        Gold = result;
    }

    /// <summary>
    /// 환생시 스테이지 레벨 및 층수 초기화
    /// </summary>
    public void HwansengPointReset()
    {
        stageLv = 1;
        floorLv = 1;
        AccumlateFloor = 0;
        HWansengCount++;
        HwanSengSystem.inst.WorldUIHwansengIconReturnStarUpdate(); // 환생아이콘 수치 리셋
    }


    /// <summary>
    /// 뉴비컨텐츠 접속시 체크
    /// </summary>
    public void Newbie_ContentInit()
    {
        // 7일 이내인 경우
        if (DateTime.Now < NewbieBuffLastDay)
        {
            // 매일 자정 기준으로 하루가 지났는지 확인
            if (DateTime.Now.Date > LastLoginDate.Date)
            {
                TodayGetNewbie_Reward = false;
            }

            // 보상 활성화 부분
            Newbie_Content.inst.NewbieWindow_Init(TodayGetNewbie_Reward);

            // 버프 시간 재계산
            TimeSpan buffTime = NewbieBuffLastDay - DateTime.Now;
            BuffContoller.inst.ActiveBuff(4, buffTime.TotalMinutes);
        }

        // 7일이 지난 시점
        else
        {
            // 뉴비 기간 종료
            // 뉴비 관련 UI 비활성화 로직
            WorldUI_Manager.inst.NewbieBtnAcitveFalse();
        }
    }




    private void LoadData()
    {
        DataManager.SaveData saveData = DataManager.inst.Get_Savedata();
        NickName = saveData.Name;


        // 1. 재화
        Gold = saveData.Gold;
        Star = saveData.Star;
        Ruby = saveData.Ruby;

        // 2.펫재료
        CrewGatchaContent.inst.Set_CrewMeterialData(saveData.Soul, saveData.born, saveData.book);

        // 3.미니게임
        MinigameTicket = saveData.miniTicket;

        // 4.버프남은시간
        BuffContoller.inst.ActiveBuff(0, saveData.buffAtkTime);
        BuffContoller.inst.ActiveBuff(1, saveData.buffMoveSpeedTime);
        BuffContoller.inst.ActiveBuff(2, saveData.buffGoldTime);
        BuffContoller.inst.ActiveBuff(3, saveData.buffBigAtkTime);


        // 5. 뉴비 혜택
        //뉴비 버프타임 추가해야됨
        TodayGetNewbie_Reward = saveData.todayGetRaward;
        GotNewbieGiftCount = saveData.getNewbieRewardCount;
        NewbieBuffLastDay = saveData.newbieBuffLastDay;


        // 6. 출석체크
        GotDaily_Reward = saveData.GetGiftCount;
        TotayGotDaily_Reward = saveData.todayGetRaward;

        // 7. 캐릭터 관련
        AtkSpeedLv = saveData.AtkSpeedLv;
        HWansengCount = saveData.HwanSeangCount;

        // 8. 게임현황 (스테이지)
        AccumlateFloor = saveData.TotalFloor;
        StageLv = saveData.Stage;
        FloorLv = saveData.NowFloor;

        // 9. 동료 레벨
        Pet0_Lv = saveData.Crew0Lv;
        Pet1_Lv = saveData.Crew1Lv;
        Pet2_Lv = saveData.Crew2Lv;

        // 10. 메인 하단 UI 현황
        aryQuestLv = saveData.QuestLv;
        aryWeaponLv = saveData.WeaponLv;
        AryNormalRelicLv = saveData.RelicLv;
        EquipWeaponNum = saveData.NowEquipWeaponNum;

        // 11. 미션 현황
        DailyMIssionisCount = saveData.DailyMissionCount;
        WeeklyMissionisCount = saveData.WeeklyMissionCount;
        SpecialMissionisCount = saveData.SpecialMissionCount;
        DailyMIssionClear = saveData.DailyMIssionClear;
        WeeklyMIssionClear = saveData.WeeklyMissionClear;
        SpecialMIssionClearNum = saveData.SpecialMissionClearNum;
        IsCanResetDailyMIssion = saveData.canResetDailyMission;
        IsCanResetWeeklyMIssion = saveData.canResetWeeklyMission;

        // 12. 빙고 현황
        RouletteTicket = saveData.RouletteTicket;
        BingoBoard = saveData.BingoBoard;

        // 0. 마지막 접속기록
        LastLoginDate = saveData.lastSigninDate;

        //세이브가능
        DataManager.inst.saveAble = true;
    }


    public string Get_SaveData()
    {
        string save = string.Empty;
        DataManager.SaveData saveData = new DataManager.SaveData();

        saveData.Name = NickName;
        saveData.QuestLv = aryQuestLv;

        // 1. 재화
        saveData.Gold = Gold;
        saveData.Star = Star;
        saveData.Ruby = Ruby;

        // 2.펫재료
        int[] material = CrewGatchaContent.inst.Get_CurCrewUpgreadMaterial();
        saveData.Soul = material[0];
        saveData.born = material[1];
        saveData.book = material[2];


        // 3.미니게임
        saveData.miniTicket = MinigameTicket;

        // 4.버프남은시간
        double[] BuffTime = BuffContoller.inst.BuffTimer;
        saveData.buffAtkTime = BuffTime[0];
        saveData.buffMoveSpeedTime = BuffTime[1];
        saveData.buffGoldTime = BuffTime[2];
        saveData.buffBigAtkTime = BuffTime[3];

        // 5. 뉴비 혜택
        //뉴비 버프타임 추가해야됨
        saveData.todayGetRaward = TodayGetNewbie_Reward;
        saveData.getNewbieRewardCount = GotNewbieGiftCount;
        saveData.newbieBuffLastDay = NewbieBuffLastDay;


        // 6. 출석체크
        saveData.GetGiftCount = GotDaily_Reward;
        saveData.todayGetRaward = TotayGotDaily_Reward;

        // 7. 캐릭터 관련
        saveData.AtkSpeedLv = AtkSpeedLv;
        saveData.HwanSeangCount = HWansengCount;

        // 8. 게임현황 (스테이지)
        saveData.TotalFloor = AccumlateFloor;
        saveData.Stage = StageLv;
        saveData.NowFloor = FloorLv;

        // 9. 동료 레벨
        saveData.Crew0Lv = Pet0_Lv;
        saveData.Crew1Lv = Pet1_Lv;
        saveData.Crew2Lv = Pet2_Lv;

        // 10. 메인 하단 UI 현황
        saveData.WeaponLv = aryWeaponLv;
        saveData.RelicLv = AryNormalRelicLv;
        saveData.NowEquipWeaponNum = EquipWeaponNum;

        // 11. 미션 현황
        saveData.DailyMIssionClear = DailyMIssionClear;
        saveData.WeeklyMissionClear = WeeklyMIssionClear;

        saveData.DailyMissionCount = DailyMIssionisCount;
        saveData.WeeklyMissionCount = weeklyMissionisCount;
        saveData.SpecialMissionCount = specialMissionisCount;

        saveData.SpecialMissionClearNum = SpecialMIssionClearNum;

        saveData.canResetDailyMission = IsCanResetDailyMIssion;
        saveData.canResetWeeklyMission = IsCanResetWeeklyMIssion;

        // 12. 빙고 현황
        saveData.RouletteTicket = RouletteTicket;
        saveData.BingoBoard = BingoBoard;

        // 0. 마지막 접속기록
        saveData.lastSigninDate = DateTime.Now;

        save = JsonUtility.ToJson(saveData);

        Debug.Log(save);

        return save;
    }
}
