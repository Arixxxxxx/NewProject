using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            DateTime LastLoginDate = value;

            if (!checkStart) // 첫 접속시
            {
                // 첫 접속시 (신규가입시)
                if (LastLoginDate.Year == 1)
                {
                    signUpDate = DateTime.Now;
                    Debug.Log("신규유저");
                    DailyPlayCheckUIManager.inst.DialyContent_Init(TotayGotDaily_Reward);
                }
                else if (LastLoginDate.Year >= 2000)
                {
                    //하루가 지남
                    if (LastLoginDate.Date < DateTime.Now.Date)
                    {
                        Daily_init();

                        //  주간 리셋 (월요일이 한번이라도 지났는지 확인)
                        if (HasMondayPassed(LastLoginDate))
                        {
                            MissionData.Instance.initWeeklyMission();
                        }

                    }
                    else if (LastLoginDate.Date == DateTime.Now.Date) // 오늘 재접
                    {
                        DailyPlayCheckUIManager.inst.DialyContent_Init(TotayGotDaily_Reward);
                    }


                }
                lastLogindate = DateTime.Now;
                checkStart = true;
            }
            else // 런타임중
            {
                lastLogindate = value;
            }
        }
    }

    private bool HasMondayPassed(DateTime lastLogin)
    {
        DateTime startDate = lastLogin.Date;
        DateTime now = DateTime.Now.Date;

        // 마지막 접속날짜부터 월요일이있었는지 다 확인
        while (startDate <= now)
        {
            if (startDate.DayOfWeek == DayOfWeek.Monday)
            {
                return true;
            }
            startDate = startDate.AddDays(1);
        }

        return false;
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
            if (ReLoginDate.Year == 1)
            {
                // 뉴비 기간 7일 입력
                newbieBuffLastDay = DateTime.Now.AddDays(7);

                // 보상 활성화
                Newbie_Content.inst.NewbieWindow_Init(TodayGetNewbie_Reward);

                // 버프 초기화
                TimeSpan buffTime = newbieBuffLastDay - DateTime.Now;
                BuffContoller.inst.ActiveBuff(4, buffTime.TotalMinutes);
            }
            else
            {
                newbieBuffLastDay = value;
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


    [HideInInspector] public UnityEvent OnRubyChanged;
    // 3. 소지 루비
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

    // 4. 초당 골드 생산량
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


    // 동료 강화재료
    int[] crewMaterial = new int[3];
    public int[] CrewMaterial 
    {
        get { return (int[])crewMaterial.Clone(); } // 배열의 복사본 반환
        set
        {
            if (value.Length == crewMaterial.Length)
            {
                crewMaterial = (int[])value.Clone(); // 배열의 복사본 할당
            }
            else
            {
                throw new ArgumentException("Array size mismatch");
            }
        }
    }

    /// <summary>
    ///  증가 => 순서 : 부적 / 화약 / 굴소스 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void Set_crewMaterial(int index, int count) 
    {
        crewMaterial[index] += count;
    }

    /// <summary>
    ///  사용 => 순서 : 부적 / 화약 / 굴소스 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void Use_crewMaterial(int index, int count)
    {
        crewMaterial[index] -= count;
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
            UIManager.Instance.SetAtkText(totalAtk.ToString());
        }
    }

    // 2. 크리티컬 확률
    float criticalChance = 10;  // 기본값 10

    public float CriticalChance
    {
        get
        {
            if (GetAryRelicLv(1) != 0)
            {
                float addCritical = GetAryRelicLv(1) * relicDefaultValue[1];
                return criticalChance + addPetCriChanceBuff + addCritical;
            }

            return criticalChance + addPetCriChanceBuff;  // <= 펫 시전버프량
        }
        set { criticalChance = value; }
    }


    // 3. 몬스터 누적 킬수
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

            // 상단 '설아의 출동이야기 텍스트 초기화'
            HwanSengSystem.inst.Set_WorldHwansengCount_Text_Init(hwansengCount);
        }
    }
    /////////////////////[ 스테이지 현황 ]//////////////////////////////

    // 1. 총 누적층수
    [SerializeField]
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
            //월드 환생아이콘 지급별수 초기화
            HwanSengSystem.inst.MainWindow_TextBox_Updater();
        }
    }

    // 2. 현재 스테이지
    [HideInInspector] public UnityEvent OnStageChanged;
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
            OnStageChanged?.Invoke();
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
                StageLv++;
            }
            else if (floorLv < 6)
            {
                WorldUI_Manager.inst.Set_StageUiBar(floorLv);
            }

            if (DataManager.inst.saveAble)
            {
                AccumlateFloor++;
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
    int pet0_Lv = 0;
    public int Pet0_Lv
    {
        get { return pet0_Lv; }
        set
        {
            pet0_Lv = value;
            
            // 첫 로드시에만 작동
            if (DataManager.inst.saveAble == false)
            {
                PetContollerManager.inst.PetActive(0, pet0_Lv);
            }
        }
    }

    // 2. 버프펫
    int pet1_Lv = 0;
    public int Pet1_Lv
    {
        get { return pet1_Lv; }
        set {
            
            pet1_Lv = value;
            // 첫 로드시에만 작동
            if (DataManager.inst.saveAble == false)
            {
                PetContollerManager.inst.PetActive(1, pet1_Lv);
            }
        }
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
    int pet2_Lv = 0;
    public int Pet2_Lv
    {
        get { return pet2_Lv; }
        set 
        {
            pet2_Lv = value;

            if (DataManager.inst.saveAble == false)
            {
                PetContollerManager.inst.PetActive(2, pet2_Lv);
            }
        }
    }

    /////////////////////////////// 하단 UI 데이터 //////////////////////////////////
    [HideInInspector] public UnityEvent OnQuestLvChanged;
    //private int[] aryQuestLv = new int[30];
    List<int> aryQuestLv = new List<int>();

    public int GetAryQuestLv(int Num)
    {
        return aryQuestLv[Num];
    }

    public void SetAryQuestLv(int Num, int Value)
    {
        aryQuestLv[Num] = Value;
        MissionData.Instance.SetSpecialMission(Num, Value, SpMissionTag.Quest);
    }

    List<int> aryWeaponLv = new List<int>();

    public int GetAryWeaponLv(int Num)
    {
        return aryWeaponLv[Num];
    }

    public List<int> GetAryWeaponLv()
    {
        return aryWeaponLv;
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

        if (aryWeaponLv[Num] == 5)
        {
            DogamManager.inst.MasterWeaponCheker();
        }

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

    // 작업해야함
    [Tooltip("0 : 일반공격력\n1 : 크리티컬 증가 %\n2 : 퀘스트골드증가 %\n3 : 크리티컬 데미지 증가 %\n4 : 피버타임 시간 증가 %\n5 : 퀘스트 가격 감소 %\n6 : 무기 가격 감소 %\n7 : 어택스피드 %\n8 : 환생시 별 획득량 증가 (초)\n9 : 적 처치 획득 골드량 증가 %")]
    [SerializeField]
    List<int> aryRelicLv = new List<int>();

    [Tooltip("0 : 일반공격력\n1 : 크리티컬 증가 %\n2 : 퀘스트골드증가 %\n3 : 크리티컬 데미지 증가 %\n4 : 피버타임 시간 증가 %\n5 : 퀘스트 가격 감소 %\n6 : 무기 가격 감소 %\n7 : 어택스피드 %\n8 : 환생시 별 획득량 증가 (초)\n9 : 적 처치 획득 골드량 증가 %")]
    public int GetAryRelicLv(int index)
    {
        if (aryRelicLv.Count < 1)
        {
            for (int indexs = 0; indexs < relicDefaultValue.Length; indexs++)
            {
                aryRelicLv.Add(0);
            }
        }

        return aryRelicLv[index];
    }

    [SerializeField]
    [Tooltip("0 : 일반공격력\n1 : 크리티컬 증가 %\n2 : 퀘스트골드증가 %\n3 : 크리티컬 데미지 증가 %\n4 : 피버타임 시간 증가 %\n5 : 퀘스트 가격 감소 %\n6 : 무기 가격 감소 %\n7 : 어택스피드 %\n8 : 환생시 별 획득량 증가 (초)\n9 : 적 처치 획득 골드량 증가 %")]
    float[] relicDefaultValue;

    /// <summary>
    /// 유물 LV당 디폴트값 <br/>0 : 일반공격력\n1 : 크리티컬 증가 %\n2 : 퀘스트골드증가 %\n3 : 크리티컬 데미지 증가 %\n4 : 피버타임 시간 증가 %\n5 : 퀘스트 가격 감소 %\n6 : 무기 가격 감소 %\n7 : 어택스피드 %\n8 : 환생시 별 획득량 증가 (초)\n9 : 적 처치 획득 골드량 증가 %% 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float RelicDefaultvalue(int value) => relicDefaultValue[value];

    /// <summary>
    /// 유물 LV당 디폴트값 <br/>0 : 일반공격력\n1 : 크리티컬 증가 %\n2 : 퀘스트골드증가 %\n3 : 크리티컬 데미지 증가 %\n4 : 피버타임 시간 증가 %\n5 : 퀘스트 가격 감소 %\n6 : 무기 가격 감소 %\n7 : 어택스피드 %\n8 : 환생시 별 획득량 증가 (초)\n9 : 적 처치 획득 골드량 증가 % 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public List<int> GetAryRelicLv()
    {
        return aryRelicLv;
    }

    public void SetAryRelicLv(int index, int Lv)
    {
        aryRelicLv[index] = Lv;
    }

    /////////////////////////////////////동은 미니게임/////////////////////////////////////////

    // 룰렛 광고 체크
    private bool adRulletActive;
    public bool AdRulletActive
    {
        get { return adRulletActive; }
        set
        {
            adRulletActive = value;
            EventShop_RulletManager.inst.AdPlayButtonInit(0, adRulletActive);
        }
    }

    // 슬롯머신 광고 체크
    private bool adSlotMachineActive;
    public bool AdSlotMachineActive
    {
        get { return adSlotMachineActive; }
        set
        {
            adSlotMachineActive = value;
            EventShop_RulletManager.inst.AdPlayButtonInit(1, adSlotMachineActive);
        }
    }

    private int minigameTicket;
    public int MinigameTicket
    {
        get { return minigameTicket; }
        set
        {
            minigameTicket = value;
            EventShop_RulletManager.inst?.BotText_Updater();
        }
    }

    /////////////////////////////////////미션/////////////////////////////////////////
    List<int> dailyMissionisCount = new List<int>();
    public int GetDailyMissionCount(int index)
    {
        return dailyMissionisCount[index];
    }
    public void SetDailyMissionCount(int index, int value)
    {
        dailyMissionisCount[index] = value;
    }

    List<int> weeklyMissionisCount = new List<int>();
    public int GetWeeklyMissionCount(int index)
    {
        return weeklyMissionisCount[index];
    }
    public void SetWeeklyMissionCount(int index, int value)
    {
        weeklyMissionisCount[index] = value;
    }
    //int[] weeklyMissionisCount = new int[4];
    //public int[] WeeklyMissionisCount { get => weeklyMissionisCount; set { weeklyMissionisCount = value; } }

    List<int> specialMissionisCount = new List<int>();
    public int GetSpecailMissionCount(int index)
    {
        return specialMissionisCount[index];
    }
    public void SetSpecialMissionCount(int index, int value)
    {
        specialMissionisCount[index] = value;
    }
    //int[] specialMissionisCount = new int[6];
    //public int[] SpecialMissionisCount { get => specialMissionisCount; set { specialMissionisCount = value; } }

    List<bool> dailyMIssionClear = new List<bool>();
    public bool GetDailyMIssionClear(int index)
    {
        return dailyMIssionClear[index];
    }
    public void SetDailyMIssionClear(int index, bool value)
    {
        dailyMIssionClear[index] = value;
    }
    //bool[] dailyMIssionClear = new bool[4];
    //public bool[] DailyMIssionClear { get => dailyMIssionClear; set { dailyMIssionClear = value; } }

    List<bool> weeklyMIssionClear = new List<bool>();
    public bool GetWeeklyMIssionClear(int index)
    {
        return weeklyMIssionClear[index];
    }
    public void SetWeeklyMIssionClear(int index, bool value)
    {
        weeklyMIssionClear[index] = value;
    }

    //bool[] weeklyMIssionClear = new bool[4];
    //public bool[] WeeklyMIssionClear { get => weeklyMIssionClear; set { weeklyMIssionClear = value; } }

    int specialMIssionClearNum;
    public int SpecialMIssionClearNum { get => specialMIssionClearNum; set { specialMIssionClearNum = value; } }

    DateTime dailyMissionResetTime;
    public DateTime DailyMissionResetTime { get => dailyMissionResetTime; set { dailyMissionResetTime = value; } }

    DateTime weeklyMissionResetTime;
    public DateTime WeeklyMissionResetTime { get => weeklyMissionResetTime; set { weeklyMissionResetTime = value; } }

    ///////////////////////빙고///////////////////
    List<bool> bingoBoard = new List<bool>();
    public bool GetBingoBoard(int index)
    {
        return bingoBoard[index];
    }
    public List<bool> GetBingoBoard()
    {
        return bingoBoard;
    }
    public void SetBingoBoard(int index, bool value)
    {
        bingoBoard[index] = value;
    }
    public void SetBingoBoard(List<bool> value)
    {
        bingoBoard = value;
    }
    int bingoStack;
    public int BingoStack
    {
        get => bingoStack;
        set
        {
            bingoStack = value;
        }
    }

    [HideInInspector] public UnityEvent OnRouletteTicketChanged;
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


    }

    int testInt;

    void Start()
    {
        // 접속날짜 확인하여 뉴비컨텐츠 초기화
        Newbie_ContentInit();

        // 뉴비 불리언변수 초기화
        IsNewBie = true;

        nextCheckTime = Time.time + timeCheckInterval;
    }

    private void Update()
    {
        //시간체크
        if (Time.time > nextCheckTime && checkStart)
        {
            RealTime_Check();
            nextCheckTime = Time.time + timeCheckInterval;
        }
    }


    float timeCheckInterval = 30f; // 30초에 한번씩 시간체크
    float nextCheckTime = 0.0f;
    bool checkStart = false;

    // 실시간 초기화 시간체크
    private void RealTime_Check()
    {
        if (LastLoginDate == DateTime.MinValue || LastLoginDate.Date == DateTime.Now.Date) { return; }

        // 자정이 지남 일일퀘스트 리셋 항목들
        if (LastLoginDate.Date <= DateTime.Now.Date)
        {
            //1. 출석체크
            Daily_init();

            //2. 뉴비

            //3. 미션 일일퀘스트
            MissionData.Instance.initDailyMission();


            //  주간 리셋 (매주 월요일)
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && LastLoginDate.DayOfWeek != DayOfWeek.Monday)
            {
                MissionData.Instance.initWeeklyMission();
            }

            LastLoginDate = DateTime.Now;
        }

    }


    // 일일이지나 초기화 해야되는 변수들
    public void Daily_init()
    {
        //출석체크 부분
        if (TotayGotDaily_Reward == true)
        {
            TotayGotDaily_Reward = false;
            DailyADRuby = true;
        }
        if (DailyADRuby == true)
        {
            DailyADRuby = false;
        }

        // 슬롯머신 버튼들
        if (AdRulletActive == true)
        {
            AdRulletActive = false;
        }
        if (AdSlotMachineActive == true)
        {
            AdSlotMachineActive = false;
        }

        DailyPlayCheckUIManager.inst.DialyContent_Init(TotayGotDaily_Reward);
    }





    public string CurrentGold() => TotalProdGold.ToString();

    /// <summary>
    ///  UI창에 골드증가량 안뜨는 함수 ( 초당 골드 자동증가 )
    /// </summary>
    /// <param name="getValue"></param>
    public void GetGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue); // 기본 골드

        if (BuffAddGold != "0")
        {
            result = CalCulator.inst.DigidPlus(result, BuffAddGold); // 상점 버프로인한값 추가
        }

        if (NewbieGoldBuffValue != "0")
        {
            result = CalCulator.inst.DigidPlus(result, NewbieGoldBuffValue); // 뉴비 버프로인한값 추가
        }

        Gold = result;
    }

    public void PlusStar(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(Star, getValue);
        Sprite img = SpriteResource.inst.CoinIMG(2);
        string text = $"별 +{CalCulator.inst.StringFourDigitAddFloatChanger(getValue)}";
        Star = result;
    }

    /// <summary>
    /// UI창에 현재 골드생산된 량 이펙트 추가해서 올라가는 함수
    /// </summary>
    /// <param name="getValue"></param>
    public void PlusGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue);
        Sprite img = SpriteResource.inst.CoinIMG(1);
        string outputText = $"골드 +{CalCulator.inst.StringFourDigitAddFloatChanger(getValue)}";
        WorldUI_Manager.inst.Get_ItemInfomation_UI_Active(img, outputText);

        //WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(0, CalCulator.inst.StringFourDigitAddFloatChanger(getValue));
        // 구버전 재화 위에 숫자만 올라가는 연출
        Gold = result;
    }

    public void MinusStar(string getValue)
    {
        string result = CalCulator.inst.BigIntigerMinus(star, getValue);
        WorldUI_Manager.inst.Use_GoldOrStarMetrialFontPooling(1, getValue);
        Star = result;
    }

    public void MinusGold(string getValue)
    {
        string result = CalCulator.inst.BigIntigerMinus(gold, getValue);
        WorldUI_Manager.inst.Use_GoldOrStarMetrialFontPooling(0, getValue);
        Gold = result;
    }

    public void PlusRuby(int value)
    {
        Ruby += value;

        if (DataManager.inst.saveAble == true && value > 0)
        {
            WorldUI_Manager.inst.Get_ItemInfomation_UI_Active(SpriteResource.inst.CoinIMG(0), $"루비 +{value}");
        }
    }


    /// <summary>
    /// 환생시 스테이지 레벨 및 층수 초기화
    /// </summary>
    public void HwansengPointReset()
    {
        stageLv = 1;
        floorLv = 1;
        AccumlateFloor = 0;
        WorldUI_Manager.inst.Reset_StageUiBar();
    }


    /// <summary>
    /// 뉴비컨텐츠 접속시 체크
    /// </summary>
    public void Newbie_ContentInit()
    {
        // 7일 이내인 경우
        if (DateTime.Now.Date < NewbieBuffLastDay.Date)
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
        WorldUI_Manager.inst.Set_Nickname(NickName);

        // 1. 재화
        Gold = saveData.Gold;
        Star = saveData.Star;
        Ruby = saveData.Ruby;

        // 2.펫재료
        CrewMaterial = saveData.CrewUpgradeMaterial;

        // 3.미니게임
        AdRulletActive = saveData.adRulletPlay;
        AdSlotMachineActive = saveData.adSlotMachinePlay;
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

        if (saveData.newbieBuffLastDay != string.Empty)
        {
            NewbieBuffLastDay = DateTime.Parse(saveData.newbieBuffLastDay, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }
        else
        {
            NewbieBuffLastDay = DateTime.MinValue;
        }


        // 6. 출석체크
        GotDaily_Reward = saveData.GetGiftCount;
        TotayGotDaily_Reward = saveData.todayGetRaward;
        DailyADRuby = saveData.DailyADRuby;

        // 7. 캐릭터 관련
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
        aryQuestLv = saveData.QuestLv.ToList();
        aryWeaponLv = saveData.WeaponLv.ToList();
        aryRelicLv = saveData.RelicLv.ToList();
        EquipWeaponNum = saveData.NowEquipWeaponNum;

        // 11. 미션 현황
        dailyMissionisCount = saveData.DailyMissionCount.ToList();
        weeklyMissionisCount = saveData.WeeklyMissionCount.ToList();
        specialMissionisCount = saveData.SpecialMissionCount.ToList();
        dailyMIssionClear = saveData.DailyMIssionClear.ToList();
        weeklyMIssionClear = saveData.WeeklyMissionClear.ToList();
        SpecialMIssionClearNum = saveData.SpecialMissionClearNum;

        // 12. 빙고 현황
        RouletteTicket = saveData.RouletteTicket;
        bingoBoard = saveData.BingoBoard.ToList();
        BingoStack = saveData.BingoStack;

        //우편
        LetterManager.inst.LeftLetterMake(saveData.LetterBox);

        // 광고제거
        AdDelete.inst.Set_AdDeleteBuffTime(saveData.adDeleteBuffTime);

        // 도감
        DogamManager.inst.GameLoad_MousterList_Init(saveData.monsterDogamList);

        // 0. 마지막 접속기록
        if (saveData.LastSignDate != string.Empty)
        {
            LastLoginDate = DateTime.Parse(saveData.LastSignDate, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
        else
        {
            LastLoginDate = DateTime.MinValue;
        }


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
        saveData.CrewUpgradeMaterial = CrewMaterial;
        


        // 3.미니게임
        saveData.adRulletPlay = adRulletActive;
        saveData.adSlotMachinePlay = adSlotMachineActive;
        saveData.miniTicket = MinigameTicket;

        // 4.버프남은시간
        double[] BuffTime = BuffContoller.inst.BuffTimer;
        saveData.buffAtkTime = (int)BuffTime[0] / 60;
        saveData.buffMoveSpeedTime = (int)BuffTime[1] / 60;
        saveData.buffGoldTime = (int)BuffTime[2] / 60;
        saveData.buffBigAtkTime = (int)BuffTime[3] / 60;

        // 5. 뉴비 혜택
        //뉴비 버프타임 추가해야됨
        saveData.todayGetRaward = TodayGetNewbie_Reward;
        saveData.getNewbieRewardCount = GotNewbieGiftCount;
        saveData.newbieBuffLastDay = NewbieBuffLastDay.ToString("o");

        // 6. 출석체크
        saveData.GetGiftCount = GotDaily_Reward;
        saveData.todayGetDailyReward = TotayGotDaily_Reward;
        saveData.DailyADRuby = DailyADRuby;

        // 7. 캐릭터 관련
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
        saveData.RelicLv = aryRelicLv.ToList();
        saveData.NowEquipWeaponNum = EquipWeaponNum;

        // 11. 미션 현황
        saveData.DailyMIssionClear = dailyMIssionClear;
        saveData.WeeklyMissionClear = weeklyMIssionClear;

        saveData.DailyMissionCount = dailyMissionisCount;
        saveData.WeeklyMissionCount = weeklyMissionisCount;
        saveData.SpecialMissionCount = specialMissionisCount;

        saveData.SpecialMissionClearNum = SpecialMIssionClearNum;


        // 12. 빙고 현황
        saveData.RouletteTicket = RouletteTicket;
        saveData.BingoBoard = bingoBoard;
        saveData.BingoStack = BingoStack;

        // 우편 남은것
        saveData.LetterBox.AddRange(LetterManager.inst.GetLeftLetter);

        // 광고제거
        saveData.adDeleteBuffTime = AdDelete.inst.Get_AdDeleteBuffTime();

        // 도감
        int[] dogamArr = DogamManager.inst.Get_monster_Soul();
        saveData.monsterDogamList = new int[dogamArr.Length];
        Array.Copy(dogamArr, saveData.monsterDogamList, dogamArr.Length);

        // 0. 마지막 접속기록
        saveData.LastSignDate = DateTime.Now.ToString("o");

        save = JsonUtility.ToJson(saveData, true);

        return save;
    }
}
