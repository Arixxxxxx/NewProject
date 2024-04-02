using System;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus inst;

    ///////////////////////////////////////////////////
    //최초 가입 일시
    // 이부분 나중에 첫 가입부분으로 옮겨야함
    // 최초에 필요한값 회원가입일자 
    // 첫 선물받기 시작한 일자

    DateTime firstdate = DateTime.Now;
    
    int[] getGiftDay = new int[3]; // 선물받은 년/월/일
    public int[] GetGiftDay
    {
        get
        {
            return getGiftDay;
        }
        set
        {
            getGiftDay = value;
        }
    }

    int[] getNewbieGiftDay = new int[3]; // 선물받은 년/월/일
    public int[] GetNewbieGiftDay
    {
        get
        {
            return getNewbieGiftDay;
        }
        set
        {
            getNewbieGiftDay = value;
        }
    }

    int firstjoinday = 0;
     
    public int FirstJoinDay
    {
        get { return firstjoinday; }
        set { firstjoinday += value; }
    }
   
    int gotDilayPlayGiftCount; // 선물 받은 횟수
    public int GotDilayPlayGiftCount
    {
        get { return gotDilayPlayGiftCount; }
        set { gotDilayPlayGiftCount = value; }
    }

    int gotNewbieGiftCount; // 선물 받은 횟수
    public int GotNewbieGiftCount
    {
        get { return gotNewbieGiftCount; }
        set { gotNewbieGiftCount = value; }
    }
    ///////////////////////////////////////////////////

    [Header("# Resource")]
    string gold = "0";
    public string PulsGold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
            WorldUI_Manager.inst.CurMaterialUpdate(0, gold);
        }
    }

    public string inputMinusGold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
            WorldUI_Manager.inst.CurMaterialUpdate(0, gold);
        }
    }

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
        }
    }

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

    int ruby = 0;
    public int Ruby
    {
        get
        {
            return ruby;
        }

        set
        {
            ruby = value;
            Debug.Log($"현재루비 {ruby}");
            WorldUI_Manager.inst.CurMaterialUpdate(3, ruby.ToString());
        }
    }


    string rebirthToken; // 환생 토큰

    int atkSpeedLv; // 공격속도 증가

    public int AtkSpeedLv
    {
        get { return atkSpeedLv; }
        set
        {
            atkSpeedLv = value;
            ActionManager.inst.PlayerAttackSpeedLvUp(atkSpeedLv);
        }
    }

    float criticalChance = 20;  //크리티컬 확률

    
    public float CriticalChance { get { return criticalChance + addPetCriChanceBuff; } set { criticalChance = value;  } }

    float criticalPower = 0; // 크리티컬 피해증가
    public float CriticalPower { get { return criticalPower + addPetCriDmgBuff; } set { criticalChance = value; } }
    [Space]
    [Header("# Stage Info")]
    int stageLv = 1; // 층수 

    int accumlateFloor = 1; // 누적층수
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

    [SerializeField]
    int floorLv = 1; // 해당 층의 몬스터 단계 
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

            if (floorLv == 6)
            {
                floorLv = 1;
                stageLv++;
            }
        }
    }



    int lvUpPower = 10; // 메인캐릭터 공격력 * % 수치
    public int LvUpPower
    {
        get
        {
            return lvUpPower;
        }
    }

    int upGradeLv = 1; // 업그레이드 레벨 => 레벨당 10씩 증가
    public int UpGradeLv
    {
        get
        {
            return upGradeLv;
        }
        set
        {
            upGradeLv += value;
            lvUpPower += 10;
        }
    }

    [Space]
    [Header("# Total Get Resource")]
    [SerializeField] float mosterKill;
    [SerializeField] float bossKill;
    [SerializeField] float getGold;
    [SerializeField] int rebirthCount; // 환생 횟수

    /////////////////////////////// 상점 버프 증가량 관련 //////////////////////////////////
    
    string buffAddATK = "0";
    public string BuffAddATK
    {
        get
        {
            return buffAddATK;
        }
        set
        {
            buffAddATK = value;
        }
    }

    string buffAddAdATK = "0";
    public string BuffAddAdATK
    {
        get
        {
            return buffAddAdATK;
        }
        set
        {
            buffAddAdATK = value;
        }
    }
    string buffAddGold = "0";
    public string BuffAddGold
    {
        get
        {
            return buffAddGold;
        }
        set
        {
            buffAddGold = value;
        }
    }


    [SerializeField] float buffAddSpeed = 1;
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

    int pet0_Lv = 1;
    public int Pet0_Lv // 공격펫
    {
        get { return pet0_Lv; }
        set { pet0_Lv = value; }
    }

    int pet1_Lv = 1;
    public int Pet1_Lv // 버프펫
    {
        get { return pet1_Lv; }
        set { pet1_Lv = value; }
    }


    string addPetAtkBuff = "0";
    public string AddPetAtkBuff
    {
        get { return addPetAtkBuff; }
        set { addPetAtkBuff = value; }
    }

    int addPetCriChanceBuff = 0;
    public int AddPetCriChanceBuff
    {
        get { return addPetCriChanceBuff; }
        set { addPetCriChanceBuff = value; }
    }

    float addPetCriDmgBuff = 0f;
    public float AddPetDmgBuff
    {
        get { return addPetCriDmgBuff; }
        set { addPetCriDmgBuff = value; }
    }


    int pet2_Lv = 1;  //골드펫
    public int Pet2_Lv
    {
        get { return pet2_Lv; }
        set { pet2_Lv = value; }
    }




    ///////////////////////////////////////////////////////////////////

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
    
    }

    void Start()
    {
        //criticalChance = 40;




    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
    /// <summary>
    /// 실질적으로 골드증가 시키는 함수
    /// </summary>
    /// <param name="getValue"></param>
    public void GetGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue); // 기본 골드
        result = CalCulator.inst.DigidPlus(result, buffAddGold); // 상점 버프로인한값 추가
        PulsGold = result;
    }



    public void TakeGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue);
        WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(0, CalCulator.inst.StringFourDigitChanger(getValue));
        PulsGold = result;
    }

    public void TakeStar(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(Star, getValue);
        WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(1, CalCulator.inst.StringFourDigitChanger(getValue));
        Star = result;
    }

    public void MinusGold(string getValue)
    {
        string result = CalCulator.inst.DigidMinus(gold, getValue, false);
        Debug.Log($"반환값 {result} / 현재값 {gold} , {getValue}");
        inputMinusGold = result;
    }
}
