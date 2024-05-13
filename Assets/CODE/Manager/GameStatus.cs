using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;

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
            DateTime LastLoginDate = value;

            //���ӽ� 1ȸ
            // ù ���ӽ� (�ű԰��Խ�)
            if (LastLoginDate.Year == 1)
            {
                signUpDate = value;
                DailyPlayCheckUIManager.inst.DialyContent_Init(TotayGotDaily_Reward);
            }
            else if (LastLoginDate.Year >= 2000)
            {
                //�Ϸ簡 ����
                if (LastLoginDate.Date < DateTime.Now.Date)
                {
                     
                    //�⼮üũ �κ�
                    if (TotayGotDaily_Reward == true)
                    {
                        TotayGotDaily_Reward = false;
                        DailyADRuby = true;
                    }
                    if(DailyADRuby == true)
                    {
                        DailyADRuby = false;
                    }
                    // ���Ըӽ� ��ư��
                    if(AdRulletActive == true)
                    {
                        AdRulletActive = false;
                    }
                    if(AdSlotMachineActive == true)
                    {
                        AdSlotMachineActive = false;
                    }
                }
                else if(LastLoginDate.Date == DateTime.Now.Date) // ���� ����
                {
                    Debug.Log("�ٽø����� �ݰ�����");
                }

                DailyPlayCheckUIManager.inst.DialyContent_Init(TotayGotDaily_Reward);
                lastLogindate = LastLoginDate;

            }
        }
    }


    private string nickName;
    public string NickName { get { return nickName; } set { nickName = value; } }
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
            if (ReLoginDate.Year == 1)
            {
                // ���� �Ⱓ 7�� �Է�
                newbieBuffLastDay = DateTime.Now.AddDays(7);

                // ���� Ȱ��ȭ
                Newbie_Content.inst.NewbieWindow_Init(TodayGetNewbie_Reward);

                // ���� �ʱ�ȭ
                TimeSpan buffTime = newbieBuffLastDay - DateTime.Now;
                BuffContoller.inst.ActiveBuff(4, buffTime.TotalMinutes);
            }
            else
            {
                newbieBuffLastDay = value;
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

  
    [HideInInspector] public UnityEvent OnRubyChanged;
    // 3. ���� ���
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

    // 4. �ʴ� ��� ���귮
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
            Debug.Log($"Floor = {value}");
        }
    }

    // 2. ���� ��������
    [HideInInspector] public UnityEvent OnStageChanged;
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
            OnStageChanged?.Invoke();
            Debug.Log($"Stage = {value}");
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

    List<int> aryRelicLv = new List<int>();
    public int GetAryRelicLv(int index)
    {
        return aryRelicLv[index];
    }
    public List<int> GetAryRelicLv()
    {
        return aryRelicLv;
    }

    public void SetAryRelicLv(int index,int Lv)
    {
        aryRelicLv[index] = Lv;
    }

    /////////////////////////////////////���� �̴ϰ���/////////////////////////////////////////

    // �귿 ���� üũ
    private bool adRulletActive;
    public bool AdRulletActive { get { return adRulletActive; } 
        set
        {
            adRulletActive = value;
            EventShop_RulletManager.inst.AdPlayButtonInit(0, adRulletActive); 
        }
    }
    
    // ���Ըӽ� ���� üũ
    private bool adSlotMachineActive;
      public bool AdSlotMachineActive { get { return adSlotMachineActive; } 
        set
        {
            adSlotMachineActive = value;
            EventShop_RulletManager.inst.AdPlayButtonInit(1, adSlotMachineActive);
        }
    }

    private int minigameTicket;
    public int MinigameTicket { get { return minigameTicket; }
        set 
        {
            minigameTicket = value; 
            EventShop_RulletManager.inst?.BotText_Updater(); 
        }
    }

    /////////////////////////////////////�̼�/////////////////////////////////////////
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

    bool isCanResetDailyMIssion;
    public bool IsCanResetDailyMIssion { get => isCanResetDailyMIssion; set { isCanResetDailyMIssion = value; } }

    bool isCanResetWeeklyMIssion;
    public bool IsCanResetWeeklyMIssion { get => isCanResetWeeklyMIssion; set { isCanResetWeeklyMIssion = value; } }

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
    //bool[] bingoBoard = new bool[8];
    //public bool[] BingoBoard { get => bingoBoard; set { bingoBoard = value; } }

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
        // �������� ������ �޾ƿͼ� �ʱ�ȭ����

        if (GameManager.inst.TestMode == false)
        {
            LoadData(); // <= ������ �ο°� Ǫ�� �Լ� #######################################
        }


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
    void Start()
    {      
       // ���ӳ�¥ Ȯ���Ͽ� ���������� �ʱ�ȭ
        Newbie_ContentInit();

        // ���� �Ҹ��𺯼� �ʱ�ȭ
        IsNewBie = true;
    }



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
        Sprite img = SpriteResource.inst.CoinIMG(2);
        string text = $"�� +{CalCulator.inst.StringFourDigitAddFloatChanger(getValue)}";
        Star = result;
    }

    /// <summary>
    /// UIâ�� ���� ������� �� ����Ʈ �߰��ؼ� �ö󰡴� �Լ�
    /// </summary>
    /// <param name="getValue"></param>
    public void PlusGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue);
        Sprite img = SpriteResource.inst.CoinIMG(1);
        string outputText = $"��� +{CalCulator.inst.StringFourDigitAddFloatChanger(getValue)}";
        WorldUI_Manager.inst.Get_ItemInfomation_UI_Active(img, outputText);

        //WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(0, CalCulator.inst.StringFourDigitAddFloatChanger(getValue));
        // ������ ��ȭ ���� ���ڸ� �ö󰡴� ����
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

    public void PlusRuby(int value)
    {
        Ruby += value;

        if (DataManager.inst.saveAble == true && value > 0)
        {
            WorldUI_Manager.inst.Get_ItemInfomation_UI_Active(SpriteResource.inst.CoinIMG(0), $"��� +{value}");
        }
    }


    /// <summary>
    /// ȯ���� �������� ���� �� ���� �ʱ�ȭ
    /// </summary>
    public void HwansengPointReset()
    {
        stageLv = 1;
        floorLv = 1;
        AccumlateFloor = 0;
        HWansengCount++;
        HwanSengSystem.inst.WorldUIHwansengIconReturnStarUpdate(); // ȯ�������� ��ġ ����
    }


    /// <summary>
    /// ���������� ���ӽ� üũ
    /// </summary>
    public void Newbie_ContentInit()
    {
        // 7�� �̳��� ���
        if (DateTime.Now.Date < NewbieBuffLastDay.Date)
        {
            // ���� ���� �������� �Ϸ簡 �������� Ȯ��
            if (DateTime.Now.Date > LastLoginDate.Date)
            {
                TodayGetNewbie_Reward = false;
                Debug.Log($"����{DateTime.Now.Date} > ������ : {LastLoginDate.Date}  ���� : {TodayGetNewbie_Reward}");
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
        WorldUI_Manager.inst.Set_Nickname(NickName);

        // 1. ��ȭ
        Gold = saveData.Gold;
        Star = saveData.Star;
        Ruby = saveData.Ruby;

        // 2.�����
        CrewGatchaContent.inst.Set_CrewMeterialData(saveData.Soul, saveData.born, saveData.book);

        // 3.�̴ϰ���
        AdRulletActive = saveData.adRulletPlay;
        AdSlotMachineActive = saveData.adSlotMachinePlay;
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

        if (saveData.newbieBuffLastDay != string.Empty)
        {
            NewbieBuffLastDay = DateTime.Parse(saveData.newbieBuffLastDay, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }
        else
        {
            NewbieBuffLastDay = DateTime.MinValue;
        }
        

        // 6. �⼮üũ
        GotDaily_Reward = saveData.GetGiftCount;
        TotayGotDaily_Reward = saveData.todayGetRaward;
        DailyADRuby = saveData.DailyADRuby;

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
        aryRelicLv = saveData.RelicLv;
        EquipWeaponNum = saveData.NowEquipWeaponNum;

        // 11. �̼� ��Ȳ
        dailyMissionisCount = saveData.DailyMissionCount;
        weeklyMissionisCount = saveData.WeeklyMissionCount;
        specialMissionisCount = saveData.SpecialMissionCount;
        dailyMIssionClear = saveData.DailyMIssionClear;
        weeklyMIssionClear = saveData.WeeklyMissionClear;
        SpecialMIssionClearNum = saveData.SpecialMissionClearNum;
        IsCanResetDailyMIssion = saveData.canResetDailyMission;
        IsCanResetWeeklyMIssion = saveData.canResetWeeklyMission;

        // 12. ���� ��Ȳ
        RouletteTicket = saveData.RouletteTicket;
        bingoBoard = saveData.BingoBoard;

        //����
        LetterManager.inst.LeftLetterMake(saveData.LetterBox);

        // 0. ������ ���ӱ��
        if (saveData.LastSignDate != string.Empty)
        {
            LastLoginDate = DateTime.Parse(saveData.LastSignDate, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
        else
        {
            LastLoginDate = DateTime.MinValue;
        }

        
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
        saveData.adRulletPlay = adRulletActive;
        saveData.adSlotMachinePlay = adSlotMachineActive;
        saveData.miniTicket = MinigameTicket;

        // 4.���������ð�
        double[] BuffTime = BuffContoller.inst.BuffTimer;
        saveData.buffAtkTime = (int)BuffTime[0] / 60;
        saveData.buffMoveSpeedTime = (int)BuffTime[1] / 60;
        saveData.buffGoldTime = (int)BuffTime[2] / 60;
        saveData.buffBigAtkTime = (int)BuffTime[3] / 60;

        // 5. ���� ����
        //���� ����Ÿ�� �߰��ؾߵ�
        saveData.todayGetRaward = TodayGetNewbie_Reward;
        saveData.getNewbieRewardCount = GotNewbieGiftCount;
        saveData.newbieBuffLastDay = NewbieBuffLastDay.ToString("o");

        // 6. �⼮üũ
        saveData.GetGiftCount = GotDaily_Reward;
        saveData.todayGetDailyReward = TotayGotDaily_Reward;
        saveData.DailyADRuby = DailyADRuby;

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
        saveData.RelicLv = aryRelicLv;
        saveData.NowEquipWeaponNum = EquipWeaponNum;

        // 11. �̼� ��Ȳ
        saveData.DailyMIssionClear = dailyMIssionClear;
        saveData.WeeklyMissionClear = weeklyMIssionClear;

        saveData.DailyMissionCount = dailyMissionisCount;
        saveData.WeeklyMissionCount = weeklyMissionisCount;
        saveData.SpecialMissionCount = specialMissionisCount;

        saveData.SpecialMissionClearNum = SpecialMIssionClearNum;

        saveData.canResetDailyMission = IsCanResetDailyMIssion;
        saveData.canResetWeeklyMission = IsCanResetWeeklyMIssion;

        // 12. ���� ��Ȳ
        saveData.RouletteTicket = RouletteTicket;
        saveData.BingoBoard = bingoBoard;

        // ���� ������
        saveData.LetterBox.AddRange(LetterManager.inst.GetLeftLetter);

        // 0. ������ ���ӱ��
        saveData.LastSignDate = DateTime.Now.ToString("o");
        
        save = JsonUtility.ToJson(saveData, true);
        Debug.Log(save);

        return save;
    }
}
