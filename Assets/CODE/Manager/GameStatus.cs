using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using static DataManager;

public class GameStatus : MonoBehaviour
{
    public static GameStatus inst;


    ///////////////////////////////////////////////////
    // �ű� ���� ���
    private DateTime signUpDate;
    public DateTime signDownDate { get { return signUpDate; } set { signUpDate = value; } }


    // ������ ���ӱ��
    private DateTime lastLogindate;
    public DateTime LastLoginDate
    {
        get { return lastLogindate; }
        set
        {
            DateTime LoginDate = value;

            //���ӽ� 1ȸ
            Debug.Log($"���� : {lastLogindate}, ����{LoginDate}");

            // ù ���ӽ� (�ű԰��Խ�)
            if (LoginDate.Year == 1)
            {
                signUpDate = value;
                DailyPlayCheckUIManager.inst.DialyContent_Init(TotayGotDaily_Reward);
            }
            else if (LoginDate.Year >= 2000)
            {
                //�Ϸ簡 ����
                if (LoginDate.Date > lastLogindate.Date)
                {
                    //�⼮üũ �κ�
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
    public string NickName { get { return nickName; } set { nickName = value; Debug.Log(nickName); } }
    /////////////////////[ �⼮üũ ��Ȳ ��  ����]//////////////////////////////

    // 1.  �������� ��/��/��
    DateTime getGiftDay;
    public DateTime GetGiftDay { get { return getGiftDay; } set { getGiftDay = value; } }


    // 3.  ���� ���� ���� Ƚ��
    int gotNewbieGiftCount;
    public int GotNewbieGiftCount
    {
        get { return gotNewbieGiftCount; }
        set
        {
            gotNewbieGiftCount = value;


            if (GotNewbieGiftCount == 7)
            {
                WorldUI_Manager.inst.NewbieBtnAcitveFalse(); // ���� �� ������ ��ư ��Ź����� 
            }
        }
    }


    // ���� �޾Ҵ��� Ȯ��
    bool todayGetNewBie_Reward;
    public bool TodayGetNewbie_Reward { get { return todayGetNewBie_Reward; } set { todayGetNewBie_Reward = value; } }


    // ���� �Ⱓ üũ
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
            // ���� �����̶��
            if (ReLoginDate.Month == 0001)
            {
                // ���� �Ⱓ 7�� �Է�
                newbieBuffLastDay = DateTime.Now.AddDays(7);

                // ���� Ȱ��ȭ
                Newbie_Content.inst.NewbieWindow_Init(TodayGetNewbie_Reward);

                // ���� �ʱ�ȭ
                TimeSpan buffTime = newbieBuffLastDay - DateTime.Now;
                BuffContoller.inst.ActiveBuff(4, buffTime.TotalMinutes);
            }


        }
    }


    // 3.  �⼮üũ ���� ���� Ƚ��
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
    bool dailyADRuby; // ���� ������ �ȹ���ġ üũ
    public bool DailyADRuby
    {
        get { return dailyADRuby; }
        set { dailyADRuby = value; }
    }
    bool totayGotDailyGift; // ���� ������ �ȹ���ġ üũ
    public bool TotayGotDaily_Reward
    {
        get { return totayGotDailyGift; }
        set { totayGotDailyGift = value; }
    }

    // 5. ���� ������
    bool isNewbie = false;
    public bool IsNewBie
    {
        get { return isNewbie; }
        set { isNewbie = value; }
    }

    // 5-1. ���� (���ݷ�)
    string newbieATKBuffValue = "0";
    public string NewbieATKBuffValue { get { return newbieATKBuffValue; } set { newbieATKBuffValue = value; } }

    // 5-2. ���� (�̼�)
    float newbieMoveSpeedBuffValue = 0;

    public float NewbieMoveSpeedBuffValue { get { return newbieMoveSpeedBuffValue; } set { newbieMoveSpeedBuffValue = value; ActionManager.inst.SetPlayerMoveSpeed(); } }

    // 5-3. ���� (��差)
    string newbieGoldBuffValue = "0";

    public string NewbieGoldBuffValue { get { return newbieGoldBuffValue; } set { newbieGoldBuffValue = value; } }

    // 5-4. ���� ( ���ݼӵ�)
    float newbieAttackSpeed = 0;

    public float NewbieAttackSpeed { get { return newbieAttackSpeed; } set { newbieAttackSpeed = value; ActionManager.inst.PlayerAttackSpeedLvUp(); } }

    // 5-5 ���� ���� 100ȸ�� ���ݷ� 2�� ���� (�ִ� 20�� / ���� ����� �ʱ�ȭ) ����
    private int newbieAttackCount;


    /// <summary>
    /// ������� ����ī��Ʈ
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

    /////////////////////[ �ÿ��̾� ��ȭ ����]//////////////////////////////

    // 1. ���� ���
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

    // 2. ���� ��
    [HideInInspector] public UnityEvent OnStartChanged;
    string star = "0"; // ȯ���� �ִ� ȭ��
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

    // 3. ���� Ű
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
    // 4. ���� ���
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
                MissionData.Instance.SetDailyMission("��� ���", UseRuby);
            }
            ruby = value;
            WorldUI_Manager.inst.CurMaterialUpdate(3, ruby.ToString("N0"));
            OnRubyChanged?.Invoke();
        }
    }

    // 5. �ʴ� ��� ���귮
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

    /////////////////////[ �÷��̾� ���� ���� ]//////////////////////////////

    // 1. ���ݷ�
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


    // 2. ���ݼӵ�
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

    // 2. ũ��Ƽ�� Ȯ��
    float criticalChance = 20;  // �⺻�� 20

    public float CriticalChance
    {
        get
        {
            return criticalChance + addPetCriChanceBuff;  // <= �� ����������
        }
        set { criticalChance = value; }
    }

    // 3. ũ��Ƽ�� ��������
    float criticalPower = 0;
    public float CriticalPower
    {
        get
        { return criticalPower; } // <= �� ����������
        set
        {
            criticalChance = value;
        }
    }

    // 4. ���� ���� ų��
    int totalEnemyKill;
    public int TotalEnemyKill
    {
        get { return totalEnemyKill; }
        set
        {
            totalEnemyKill = value;
            MissionData.Instance.SetDailyMission("���� óġ", 1);
        }
    }

    // 5. ȯ��Ƚ��
    int hwansengCount;
    public int HWansengCount
    {
        get { return hwansengCount; }
        set
        {
            hwansengCount = value;
            //MissionData.Instance.SetWeeklyMission("ȯ���ϱ�",0);
        }
    }
    /////////////////////[ �������� ��Ȳ ]//////////////////////////////

    // 1. �� ��������
    int accumlateFloor = 1;
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

    // 2. ���� ��������
    int stageLv = 1; // ���� 
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

    // 3. ���� ����
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
            AccumlateFloor++;
            HwanSengSystem.inst.WorldUIHwansengIconReturnStarUpdate();
            if (floorLv == 6)
            {
                floorLv = 1;
                stageLv++;
            }
        }
    }




    /////////////////////////////// ���� ���� ������ ���� //////////////////////////////////

    // 1. ���ݷ����� ����
    string buffAddATK = "0";
    public string BuffAddATK { get { return buffAddATK; } set { buffAddATK = value; } }

    // 2. ȭ�� ���� ���ݷ����� 
    string buffAddAdATK = "0";
    public string BuffAddAdATK { get { return buffAddAdATK; } set { buffAddAdATK = value; } }

    // 3. ������� ����
    string buffAddGold = "0";
    public string BuffAddGold { get { return buffAddGold; } set { buffAddGold = value; } }

    // 4. �̵��ӵ����� ����
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
            Debug.Log($"�Էµ� {buffAddSpeed} / {value}");

            ActionManager.inst.SetPlayerMoveSpeed();
        }
    }



    /////////////////////////////// �� ���� ������ ���� //////////////////////////////////

    // 1. ������
    int pet0_Lv = 1;
    public int Pet0_Lv
    {
        get { return pet0_Lv; }
        set { pet0_Lv = value; }
    }

    // 2. ������
    int pet1_Lv = 1;
    public int Pet1_Lv
    {
        get { return pet1_Lv; }
        set { pet1_Lv = value; }
    }

    // 2-1 ���� ���ݷ� ����
    string addPetAtkBuff = "0";
    public string AddPetAtkBuff
    {
        get { return addPetAtkBuff; }
        set { addPetAtkBuff = value; }
    }

    // 2-2 ���� ũ��Ƽ��Ȯ�� ����
    int addPetCriChanceBuff = 0;
    public int AddPetCriChanceBuff
    {
        get { return addPetCriChanceBuff; }
        set { addPetCriChanceBuff = value; }
    }

    // 3. ��ɼ���
    int pet2_Lv = 1;
    public int Pet2_Lv
    {
        get { return pet2_Lv; }
        set { pet2_Lv = value; }
    }

    /////////////////////////////// �ϴ� UI ������ //////////////////////////////////
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

    int equipWeaponNum;//�������� ���� �̹��� ��ȣ
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
    /////////////////////////////////////�̼�/////////////////////////////////////////

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

    int rouletteTicket;
    public int RouletteTicket { get => rouletteTicket; set { rouletteTicket = value; } }

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
        // �������� ������ �޾ƿͼ� �ʱ�ȭ����

        if (GameManager.inst.TestMode == false)
        {
            LoadData(); // <= ������ �ο°� Ǫ�� �Լ� #######################################
        }

        // ���ӳ�¥ Ȯ���Ͽ� ���������� �ʱ�ȭ
        Newbie_ContentInit();

        // ���� �Ҹ��𺯼� �ʱ�ȭ
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
    ///  UIâ�� ��������� �ȶߴ� �Լ� ( �ʴ� ��� �ڵ����� )
    /// </summary>
    /// <param name="getValue"></param>
    public void GetGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue); // �⺻ ���
        result = CalCulator.inst.DigidPlus(result, buffAddGold); // ���� ���������Ѱ� �߰�
        result = CalCulator.inst.DigidPlus(result, newbieGoldBuffValue); // ���� ���������Ѱ� �߰�
        Gold = result;
    }

    public void PlusStar(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(Star, getValue);
        WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(1, CalCulator.inst.StringFourDigitAddFloatChanger(getValue));
        Star = result;
    }

    /// <summary>
    /// UIâ�� ���� ������� �� ����Ʈ �߰��ؼ� �ö󰡴� �Լ�
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
    /// ȯ���� �������� ���� �� ���� �ʱ�ȭ
    /// </summary>
    public void HwansengPointReset()
    {
        stageLv = 1;
        floorLv = 1;
        AccumlateFloor = 1;
        HWansengCount++;
        HwanSengSystem.inst.WorldUIHwansengIconReturnStarUpdate(); // ȯ�������� ��ġ ����
    }


    /// <summary>
    /// ���������� ���ӽ� üũ
    /// </summary>
    public void Newbie_ContentInit()
    {
        // 7�� �̳��� ���
        if (DateTime.Now < NewbieBuffLastDay)
        {
            // ���� ���� �������� �Ϸ簡 �������� Ȯ��
            if (DateTime.Now.Date > LastLoginDate.Date)
            {
                TodayGetNewbie_Reward = false;
            }

            // ���� Ȱ��ȭ �κ�
            Newbie_Content.inst.NewbieWindow_Init(TodayGetNewbie_Reward);

            // ���� �ð� ����
            TimeSpan buffTime = NewbieBuffLastDay - DateTime.Now;
            BuffContoller.inst.ActiveBuff(4, buffTime.TotalMinutes);
        }

        // 7���� ���� ����
        else
        {
            // ���� �Ⱓ ����
            // ���� ���� UI ��Ȱ��ȭ ����
            WorldUI_Manager.inst.NewbieBtnAcitveFalse();
        }
    }




    private void LoadData()
    {
        DataManager.SaveData saveData = DataManager.inst.Get_Savedata();
        NickName = saveData.Name;


        // 1. ��ȭ
        Gold = saveData.Gold;
        Star = saveData.Star;
        Ruby = saveData.Ruby;

        // 2.�����
        CrewGatchaContent.inst.Set_CrewMeterialData(saveData.Soul, saveData.born, saveData.book);

        // 3.�̴ϰ���
        MinigameTicket = saveData.miniTicket;

        // 4.���������ð�
        BuffContoller.inst.ActiveBuff(0, saveData.buffAtkTime);
        BuffContoller.inst.ActiveBuff(1, saveData.buffMoveSpeedTime);
        BuffContoller.inst.ActiveBuff(2, saveData.buffGoldTime);
        BuffContoller.inst.ActiveBuff(3, saveData.buffBigAtkTime);


        // 5. ���� ����
        //���� ����Ÿ�� �߰��ؾߵ�
        TodayGetNewbie_Reward = saveData.todayGetRaward;
        GotNewbieGiftCount = saveData.getNewbieRewardCount;
        NewbieBuffLastDay = saveData.newbieBuffLastDay;


        // 6. �⼮üũ
        GotDaily_Reward = saveData.GetGiftCount;
        TotayGotDaily_Reward = saveData.todayGetRaward;

        // 7. ĳ���� ����
        AtkSpeedLv = saveData.AtkSpeedLv;
        HWansengCount = saveData.HwanSeangCount;

        // 8. ������Ȳ (��������)
        AccumlateFloor = saveData.TotalFloor;
        StageLv = saveData.Stage;
        FloorLv = saveData.NowFloor;

        // 9. ���� ����
        Pet0_Lv = saveData.Crew0Lv;
        Pet1_Lv = saveData.Crew1Lv;
        Pet2_Lv = saveData.Crew2Lv;

        // 10. ���� �ϴ� UI ��Ȳ
        aryQuestLv = saveData.QuestLv;
        aryWeaponLv = saveData.WeaponLv;
        AryNormalRelicLv = saveData.RelicLv;
        EquipWeaponNum = saveData.NowEquipWeaponNum;

        // 11. �̼� ��Ȳ
        DailyMIssionisCount = saveData.DailyMissionCount;
        WeeklyMissionisCount = saveData.WeeklyMissionCount;
        SpecialMissionisCount = saveData.SpecialMissionCount;
        DailyMIssionClear = saveData.DailyMIssionClear;
        WeeklyMIssionClear = saveData.WeeklyMissionClear;
        SpecialMIssionClearNum = saveData.SpecialMissionClearNum;
        IsCanResetDailyMIssion = saveData.canResetDailyMission;
        IsCanResetWeeklyMIssion = saveData.canResetWeeklyMission;

        // 12. ���� ��Ȳ
        RouletteTicket = saveData.RouletteTicket;
        BingoBoard = saveData.BingoBoard;

        // 0. ������ ���ӱ��
        LastLoginDate = saveData.lastSigninDate;

        //���̺갡��
        DataManager.inst.saveAble = true;
    }


    public string Get_SaveData()
    {
        string save = string.Empty;
        DataManager.SaveData saveData = new DataManager.SaveData();

        saveData.Name = NickName;
        saveData.QuestLv = aryQuestLv;

        // 1. ��ȭ
        saveData.Gold = Gold;
        saveData.Star = Star;
        saveData.Ruby = Ruby;

        // 2.�����
        int[] material = CrewGatchaContent.inst.Get_CurCrewUpgreadMaterial();
        saveData.Soul = material[0];
        saveData.born = material[1];
        saveData.book = material[2];


        // 3.�̴ϰ���
        saveData.miniTicket = MinigameTicket;

        // 4.���������ð�
        double[] BuffTime = BuffContoller.inst.BuffTimer;
        saveData.buffAtkTime = BuffTime[0];
        saveData.buffMoveSpeedTime = BuffTime[1];
        saveData.buffGoldTime = BuffTime[2];
        saveData.buffBigAtkTime = BuffTime[3];

        // 5. ���� ����
        //���� ����Ÿ�� �߰��ؾߵ�
        saveData.todayGetRaward = TodayGetNewbie_Reward;
        saveData.getNewbieRewardCount = GotNewbieGiftCount;
        saveData.newbieBuffLastDay = NewbieBuffLastDay;


        // 6. �⼮üũ
        saveData.GetGiftCount = GotDaily_Reward;
        saveData.todayGetRaward = TotayGotDaily_Reward;

        // 7. ĳ���� ����
        saveData.AtkSpeedLv = AtkSpeedLv;
        saveData.HwanSeangCount = HWansengCount;

        // 8. ������Ȳ (��������)
        saveData.TotalFloor = AccumlateFloor;
        saveData.Stage = StageLv;
        saveData.NowFloor = FloorLv;

        // 9. ���� ����
        saveData.Crew0Lv = Pet0_Lv;
        saveData.Crew1Lv = Pet1_Lv;
        saveData.Crew2Lv = Pet2_Lv;

        // 10. ���� �ϴ� UI ��Ȳ
        saveData.WeaponLv = aryWeaponLv;
        saveData.RelicLv = AryNormalRelicLv;
        saveData.NowEquipWeaponNum = EquipWeaponNum;

        // 11. �̼� ��Ȳ
        saveData.DailyMIssionClear = DailyMIssionClear;
        saveData.WeeklyMissionClear = WeeklyMIssionClear;

        saveData.DailyMissionCount = DailyMIssionisCount;
        saveData.WeeklyMissionCount = weeklyMissionisCount;
        saveData.SpecialMissionCount = specialMissionisCount;

        saveData.SpecialMissionClearNum = SpecialMIssionClearNum;

        saveData.canResetDailyMission = IsCanResetDailyMIssion;
        saveData.canResetWeeklyMission = IsCanResetWeeklyMIssion;

        // 12. ���� ��Ȳ
        saveData.RouletteTicket = RouletteTicket;
        saveData.BingoBoard = BingoBoard;

        // 0. ������ ���ӱ��
        saveData.lastSigninDate = DateTime.Now;

        save = JsonUtility.ToJson(saveData);

        Debug.Log(save);

        return save;
    }
}
