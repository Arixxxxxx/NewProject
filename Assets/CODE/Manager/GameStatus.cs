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

            if (!checkStart) // ù ���ӽ�
            {
                // ù ���ӽ� (�ű԰��Խ�)
                if (LastLoginDate.Year == 1)
                {
                    signUpDate = DateTime.Now;
                    Debug.Log("�ű�����");
                    DailyPlayCheckUIManager.inst.DialyContent_Init(TotayGotDaily_Reward);
                }
                else if (LastLoginDate.Year >= 2000)
                {
                    //�Ϸ簡 ����
                    if (LastLoginDate.Date < DateTime.Now.Date)
                    {
                        Daily_init();

                        //  �ְ� ���� (�������� �ѹ��̶� �������� Ȯ��)
                        if (HasMondayPassed(LastLoginDate))
                        {
                            MissionData.Instance.initWeeklyMission();
                        }

                    }
                    else if (LastLoginDate.Date == DateTime.Now.Date) // ���� ����
                    {
                        DailyPlayCheckUIManager.inst.DialyContent_Init(TotayGotDaily_Reward);
                    }


                }
                lastLogindate = DateTime.Now;
                checkStart = true;
            }
            else // ��Ÿ����
            {
                lastLogindate = value;
            }
        }
    }

    private bool HasMondayPassed(DateTime lastLogin)
    {
        DateTime startDate = lastLogin.Date;
        DateTime now = DateTime.Now.Date;

        // ������ ���ӳ�¥���� ���������־����� �� Ȯ��
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


    // ���� ��ȭ���
    int[] crewMaterial = new int[3];
    public int[] CrewMaterial 
    {
        get { return (int[])crewMaterial.Clone(); } // �迭�� ���纻 ��ȯ
        set
        {
            if (value.Length == crewMaterial.Length)
            {
                crewMaterial = (int[])value.Clone(); // �迭�� ���纻 �Ҵ�
            }
            else
            {
                throw new ArgumentException("Array size mismatch");
            }
        }
    }

    /// <summary>
    ///  ���� => ���� : ���� / ȭ�� / ���ҽ� 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void Set_crewMaterial(int index, int count) 
    {
        crewMaterial[index] += count;
    }

    /// <summary>
    ///  ��� => ���� : ���� / ȭ�� / ���ҽ� 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void Use_crewMaterial(int index, int count)
    {
        crewMaterial[index] -= count;
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
            UIManager.Instance.SetAtkText(totalAtk.ToString());
        }
    }

    // 2. ũ��Ƽ�� Ȯ��
    float criticalChance = 10;  // �⺻�� 10

    public float CriticalChance
    {
        get
        {
            if (GetAryRelicLv(1) != 0)
            {
                float addCritical = GetAryRelicLv(1) * relicDefaultValue[1];
                return criticalChance + addPetCriChanceBuff + addCritical;
            }

            return criticalChance + addPetCriChanceBuff;  // <= �� ����������
        }
        set { criticalChance = value; }
    }


    // 3. ���� ���� ų��
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

            // ��� '������ �⵿�̾߱� �ؽ�Ʈ �ʱ�ȭ'
            HwanSengSystem.inst.Set_WorldHwansengCount_Text_Init(hwansengCount);
        }
    }
    /////////////////////[ �������� ��Ȳ ]//////////////////////////////

    // 1. �� ��������
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
            //���� ȯ�������� ���޺��� �ʱ�ȭ
            HwanSengSystem.inst.MainWindow_TextBox_Updater();
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
    int pet0_Lv = 0;
    public int Pet0_Lv
    {
        get { return pet0_Lv; }
        set
        {
            pet0_Lv = value;
            
            // ù �ε�ÿ��� �۵�
            if (DataManager.inst.saveAble == false)
            {
                PetContollerManager.inst.PetActive(0, pet0_Lv);
            }
        }
    }

    // 2. ������
    int pet1_Lv = 0;
    public int Pet1_Lv
    {
        get { return pet1_Lv; }
        set {
            
            pet1_Lv = value;
            // ù �ε�ÿ��� �۵�
            if (DataManager.inst.saveAble == false)
            {
                PetContollerManager.inst.PetActive(1, pet1_Lv);
            }
        }
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

    public List<int> GetAryWeaponLv()
    {
        return aryWeaponLv;
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

    // �۾��ؾ���
    [Tooltip("0 : �Ϲݰ��ݷ�\n1 : ũ��Ƽ�� ���� %\n2 : ����Ʈ������� %\n3 : ũ��Ƽ�� ������ ���� %\n4 : �ǹ�Ÿ�� �ð� ���� %\n5 : ����Ʈ ���� ���� %\n6 : ���� ���� ���� %\n7 : ���ý��ǵ� %\n8 : ȯ���� �� ȹ�淮 ���� (��)\n9 : �� óġ ȹ�� ��差 ���� %")]
    [SerializeField]
    List<int> aryRelicLv = new List<int>();

    [Tooltip("0 : �Ϲݰ��ݷ�\n1 : ũ��Ƽ�� ���� %\n2 : ����Ʈ������� %\n3 : ũ��Ƽ�� ������ ���� %\n4 : �ǹ�Ÿ�� �ð� ���� %\n5 : ����Ʈ ���� ���� %\n6 : ���� ���� ���� %\n7 : ���ý��ǵ� %\n8 : ȯ���� �� ȹ�淮 ���� (��)\n9 : �� óġ ȹ�� ��差 ���� %")]
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
    [Tooltip("0 : �Ϲݰ��ݷ�\n1 : ũ��Ƽ�� ���� %\n2 : ����Ʈ������� %\n3 : ũ��Ƽ�� ������ ���� %\n4 : �ǹ�Ÿ�� �ð� ���� %\n5 : ����Ʈ ���� ���� %\n6 : ���� ���� ���� %\n7 : ���ý��ǵ� %\n8 : ȯ���� �� ȹ�淮 ���� (��)\n9 : �� óġ ȹ�� ��差 ���� %")]
    float[] relicDefaultValue;

    /// <summary>
    /// ���� LV�� ����Ʈ�� <br/>0 : �Ϲݰ��ݷ�\n1 : ũ��Ƽ�� ���� %\n2 : ����Ʈ������� %\n3 : ũ��Ƽ�� ������ ���� %\n4 : �ǹ�Ÿ�� �ð� ���� %\n5 : ����Ʈ ���� ���� %\n6 : ���� ���� ���� %\n7 : ���ý��ǵ� %\n8 : ȯ���� �� ȹ�淮 ���� (��)\n9 : �� óġ ȹ�� ��差 ���� %% 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float RelicDefaultvalue(int value) => relicDefaultValue[value];

    /// <summary>
    /// ���� LV�� ����Ʈ�� <br/>0 : �Ϲݰ��ݷ�\n1 : ũ��Ƽ�� ���� %\n2 : ����Ʈ������� %\n3 : ũ��Ƽ�� ������ ���� %\n4 : �ǹ�Ÿ�� �ð� ���� %\n5 : ����Ʈ ���� ���� %\n6 : ���� ���� ���� %\n7 : ���ý��ǵ� %\n8 : ȯ���� �� ȹ�淮 ���� (��)\n9 : �� óġ ȹ�� ��差 ���� % 
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

    /////////////////////////////////////���� �̴ϰ���/////////////////////////////////////////

    // �귿 ���� üũ
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

    // ���Ըӽ� ���� üũ
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

    DateTime dailyMissionResetTime;
    public DateTime DailyMissionResetTime { get => dailyMissionResetTime; set { dailyMissionResetTime = value; } }

    DateTime weeklyMissionResetTime;
    public DateTime WeeklyMissionResetTime { get => weeklyMissionResetTime; set { weeklyMissionResetTime = value; } }

    ///////////////////////����///////////////////
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
        // �������� ������ �޾ƿͼ� �ʱ�ȭ����

        if (GameManager.inst.TestMode == false)
        {
            LoadData(); // <= ������ �ο°� Ǫ�� �Լ� #######################################
        }


    }

    int testInt;

    void Start()
    {
        // ���ӳ�¥ Ȯ���Ͽ� ���������� �ʱ�ȭ
        Newbie_ContentInit();

        // ���� �Ҹ��𺯼� �ʱ�ȭ
        IsNewBie = true;

        nextCheckTime = Time.time + timeCheckInterval;
    }

    private void Update()
    {
        //�ð�üũ
        if (Time.time > nextCheckTime && checkStart)
        {
            RealTime_Check();
            nextCheckTime = Time.time + timeCheckInterval;
        }
    }


    float timeCheckInterval = 30f; // 30�ʿ� �ѹ��� �ð�üũ
    float nextCheckTime = 0.0f;
    bool checkStart = false;

    // �ǽð� �ʱ�ȭ �ð�üũ
    private void RealTime_Check()
    {
        if (LastLoginDate == DateTime.MinValue || LastLoginDate.Date == DateTime.Now.Date) { return; }

        // ������ ���� ��������Ʈ ���� �׸��
        if (LastLoginDate.Date <= DateTime.Now.Date)
        {
            //1. �⼮üũ
            Daily_init();

            //2. ����

            //3. �̼� ��������Ʈ
            MissionData.Instance.initDailyMission();


            //  �ְ� ���� (���� ������)
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && LastLoginDate.DayOfWeek != DayOfWeek.Monday)
            {
                MissionData.Instance.initWeeklyMission();
            }

            LastLoginDate = DateTime.Now;
        }

    }


    // ���������� �ʱ�ȭ �ؾߵǴ� ������
    public void Daily_init()
    {
        //�⼮üũ �κ�
        if (TotayGotDaily_Reward == true)
        {
            TotayGotDaily_Reward = false;
            DailyADRuby = true;
        }
        if (DailyADRuby == true)
        {
            DailyADRuby = false;
        }

        // ���Ըӽ� ��ư��
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
    ///  UIâ�� ��������� �ȶߴ� �Լ� ( �ʴ� ��� �ڵ����� )
    /// </summary>
    /// <param name="getValue"></param>
    public void GetGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue); // �⺻ ���

        if (BuffAddGold != "0")
        {
            result = CalCulator.inst.DigidPlus(result, BuffAddGold); // ���� ���������Ѱ� �߰�
        }

        if (NewbieGoldBuffValue != "0")
        {
            result = CalCulator.inst.DigidPlus(result, NewbieGoldBuffValue); // ���� ���������Ѱ� �߰�
        }

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
        WorldUI_Manager.inst.Reset_StageUiBar();
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
        CrewMaterial = saveData.CrewUpgradeMaterial;

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
        aryQuestLv = saveData.QuestLv.ToList();
        aryWeaponLv = saveData.WeaponLv.ToList();
        aryRelicLv = saveData.RelicLv.ToList();
        EquipWeaponNum = saveData.NowEquipWeaponNum;

        // 11. �̼� ��Ȳ
        dailyMissionisCount = saveData.DailyMissionCount.ToList();
        weeklyMissionisCount = saveData.WeeklyMissionCount.ToList();
        specialMissionisCount = saveData.SpecialMissionCount.ToList();
        dailyMIssionClear = saveData.DailyMIssionClear.ToList();
        weeklyMIssionClear = saveData.WeeklyMissionClear.ToList();
        SpecialMIssionClearNum = saveData.SpecialMissionClearNum;

        // 12. ���� ��Ȳ
        RouletteTicket = saveData.RouletteTicket;
        bingoBoard = saveData.BingoBoard.ToList();
        BingoStack = saveData.BingoStack;

        //����
        LetterManager.inst.LeftLetterMake(saveData.LetterBox);

        // ��������
        AdDelete.inst.Set_AdDeleteBuffTime(saveData.adDeleteBuffTime);

        // ����
        DogamManager.inst.GameLoad_MousterList_Init(saveData.monsterDogamList);

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
        saveData.CrewUpgradeMaterial = CrewMaterial;
        


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
        saveData.RelicLv = aryRelicLv.ToList();
        saveData.NowEquipWeaponNum = EquipWeaponNum;

        // 11. �̼� ��Ȳ
        saveData.DailyMIssionClear = dailyMIssionClear;
        saveData.WeeklyMissionClear = weeklyMIssionClear;

        saveData.DailyMissionCount = dailyMissionisCount;
        saveData.WeeklyMissionCount = weeklyMissionisCount;
        saveData.SpecialMissionCount = specialMissionisCount;

        saveData.SpecialMissionClearNum = SpecialMIssionClearNum;


        // 12. ���� ��Ȳ
        saveData.RouletteTicket = RouletteTicket;
        saveData.BingoBoard = bingoBoard;
        saveData.BingoStack = BingoStack;

        // ���� ������
        saveData.LetterBox.AddRange(LetterManager.inst.GetLeftLetter);

        // ��������
        saveData.adDeleteBuffTime = AdDelete.inst.Get_AdDeleteBuffTime();

        // ����
        int[] dogamArr = DogamManager.inst.Get_monster_Soul();
        saveData.monsterDogamList = new int[dogamArr.Length];
        Array.Copy(dogamArr, saveData.monsterDogamList, dogamArr.Length);

        // 0. ������ ���ӱ��
        saveData.LastSignDate = DateTime.Now.ToString("o");

        save = JsonUtility.ToJson(saveData, true);

        return save;
    }
}
