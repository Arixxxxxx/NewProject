using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class UpdateData
{
    public string Playeratk;
    public int weaponLv = 0;

}




public class GameStatus : MonoBehaviour
{
    public static GameStatus inst;


    ///////////////////////////////////////////////////
    //���� ���� �Ͻ�
    // �̺κ� ���߿� ù ���Ժκ����� �Űܾ���
    // ���ʿ� �ʿ��Ѱ� ȸ���������� 
    // ù �����ޱ� ������ ����

    DateTime firstdate = DateTime.Now; // ���ڰ�� (���� �̻��)



    /////////////////////[ �⼮üũ ��Ȳ ��  ����]//////////////////////////////

    // 1.  �������� ��/��/��
    int[] getGiftDay = new int[3];
    public int[] GetGiftDay { get { return getGiftDay; } set { getGiftDay = value; } }

    // 2.  ���� ���ù��� ��/��/��
    int[] getNewbieGiftDay = new int[3];
    public int[] GetNewbieGiftDay { get { return getNewbieGiftDay; } set { getNewbieGiftDay = value; } }


    // 3.  �⼮üũ ���� ���� Ƚ��
    int gotDilayPlayGiftCount; // 
    public int GotDilayPlayGiftCount
    {
        get { return gotDilayPlayGiftCount; }
        set { gotDilayPlayGiftCount = value; }
    }

    // 4.  ���� ���� ���� Ƚ��
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

    public float NewbieMoveSpeedBuffValue { get { return newbieMoveSpeedBuffValue; } set { newbieMoveSpeedBuffValue = value; } }

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
    public UnityEvent OnGoldChanged;
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
    public UnityEvent OnStartChanged;
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

    public UnityEvent OnRubyChanged;
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
            WorldUI_Manager.inst.CurMaterialUpdate(3, ruby.ToString());
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
            MissionData.Instance.SetWeeklyMission("ȯ���ϱ�", 1);
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
        MissionData.Instance.SetSpecialMission(Num, Value, MissionType.Quest);
    }

    int[] aryWeaponLv = new int[30];

    public int GetAryWeaponLv(int Num)
    {
        return aryWeaponLv[Num];
    }

    public void SetAryWeaponLv(int Num, int Value)
    {
        aryWeaponLv[Num] = Value;
        MissionData.Instance.SetSpecialMission(Num, Value, MissionType.Weapon);
    }

    [HideInInspector] public UnityEvent OnPercentageChanged;
    float[] aryPercentage = new float[5];
    public float GetAryPercent(int index)
    {
        return aryPercentage[index];
    }
    public void SetAryPercent(int index, float value)
    {
        aryPercentage[index] = value;
        OnPercentageChanged?.Invoke();
    }

    int[] aryNormalRelicLv = new int[5];

    public int[] AryNormalRelicLv
    {
        get => aryNormalRelicLv;
        set { aryNormalRelicLv = value; }
    }

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

        // ���� �Ҹ��𺯼� �ʱ�ȭ
        IsNewBie = true;
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

    public void PlusGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue);
        WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(0, CalCulator.inst.StringFourDigitAddFloatChanger(getValue));
        Gold = result;
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
}
