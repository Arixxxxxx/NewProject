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
    //최초 가입 일시
    // 이부분 나중에 첫 가입부분으로 옮겨야함
    // 최초에 필요한값 회원가입일자 
    // 첫 선물받기 시작한 일자

    DateTime firstdate = DateTime.Now; // 일자계산 (아직 미사용)



    /////////////////////[ 출석체크 현황 및  뉴비]//////////////////////////////

    // 1.  선물받은 년/월/일
    int[] getGiftDay = new int[3];
    public int[] GetGiftDay { get { return getGiftDay; } set { getGiftDay = value; } }

    // 2.  뉴비 혜택받은 년/월/일
    int[] getNewbieGiftDay = new int[3];
    public int[] GetNewbieGiftDay { get { return getNewbieGiftDay; } set { getNewbieGiftDay = value; } }


    // 3.  출석체크 선물 받은 횟수
    int gotDilayPlayGiftCount; // 
    public int GotDilayPlayGiftCount
    {
        get { return gotDilayPlayGiftCount; }
        set { gotDilayPlayGiftCount = value; }
    }

    // 4.  뉴비 선물 받은 횟수
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

    public float NewbieMoveSpeedBuffValue { get { return newbieMoveSpeedBuffValue; } set { newbieMoveSpeedBuffValue = value; } }

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

    // 2. 소지 별
    public UnityEvent OnStartChanged;
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

    public UnityEvent OnRubyChanged;
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
            WorldUI_Manager.inst.CurMaterialUpdate(3, ruby.ToString());
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
            MissionData.Instance.SetWeeklyMission("환생하기", 1);
        }
    }
    /////////////////////[ 스테이지 현황 ]//////////////////////////////

    // 1. 총 누적층수
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
            AccumlateFloor++;
            HwanSengSystem.inst.WorldUIHwansengIconReturnStarUpdate();
            if (floorLv == 6)
            {
                floorLv = 1;
                stageLv++;
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
        // 서버에서 데이터 받아와서 초기화해줌

        // 뉴비 불리언변수 초기화
        IsNewBie = true;
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
    /// 환생시 스테이지 레벨 및 층수 초기화
    /// </summary>
    public void HwansengPointReset()
    {
        stageLv = 1;
        floorLv = 1;
        AccumlateFloor = 1;
        HWansengCount++;
        HwanSengSystem.inst.WorldUIHwansengIconReturnStarUpdate(); // 환생아이콘 수치 리셋
    }
}
