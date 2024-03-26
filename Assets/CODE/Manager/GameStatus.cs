using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus inst;


    [Header("# Resource")]
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
        }
    }

    string star = "0"; // 환생시 주는 키
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

    string ruby = "0";
    public string Ruby
    {
        get
        {
            return ruby;
        }

        set
        {
            ruby = value;
            WorldUI_Manager.inst.CurMaterialUpdate(3, ruby);
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

    
    public float CriticalChance { get { return criticalChance + addPetCriChanceBuff; } set { criticalChance = value; } }

    float criticalPower = 0; // 크리티컬 피해증가
    public float CriticalPower { get { return criticalPower + addPetCriDmgBuff; } set { criticalChance = value; } }
    [Space]
    [Header("# Stage Info")]
    int stageLv = 1; // 층수 
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
            if (floorLv == 5)
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


    /////////////////////////////// 펫 버프 증가량 관련 //////////////////////////////////

    int pet1_Lv = 1;
    public int Pet1_Lv
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
    public int AddPetCriBuff
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

    public void GetGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue);
        Debug.Log(result);
        Gold = result;
    }

    public void PetBuffAcitve()
    {
        //주사위굴리고
        int dice = Random.Range(0, 100);

        if (dice >= 0 && dice < 40) // 공격력 증가
        {
            StopCoroutine(ActiveBuff(0));
            StartCoroutine(ActiveBuff(0));

        }
        else if (dice >= 40 && dice < 80) //크리티컬 증가
        {
            StopCoroutine(ActiveBuff(1));
            StartCoroutine(ActiveBuff(1));
        }
        else if (dice > 80) // 크리티컬 피해증가
        {
            StopCoroutine(ActiveBuff(2));
            StartCoroutine(ActiveBuff(2));
        }
    }


    WaitForSeconds waitBuffTime;
    float buffTime = 2f;
    IEnumerator ActiveBuff(int buffNum)
    {
        buffTime = (buffTime * pet1_Lv);
        waitBuffTime = new WaitForSeconds(buffTime);
        Debug.Log($"버프 타임 : {buffTime}");
        switch (buffNum)
        {
            case 0:
                addPetAtkBuff = CalCulator.inst.Get_PetBuffValue(0);
                Debug.Log($"공격버프 시작 => 현재 공격력 {CalCulator.inst.DigidPlus(CalCulator.inst.Get_ATKtoString() , addPetAtkBuff)} ");
                yield return waitBuffTime;
                addPetAtkBuff = "0";
                Debug.Log($"공격버프 종료 => 현재 공격력 {CalCulator.inst.DigidPlus(CalCulator.inst.Get_ATKtoString(), addPetAtkBuff)} ");
                break;

            case 1:
                addPetCriChanceBuff = 25;
                Debug.Log($"크리버프 시작 => 현재 크리확률 {criticalChance} %");

                yield return waitBuffTime;
                addPetCriChanceBuff = 0;
                Debug.Log($"크리버프 종료 => 현재 크리확률 {criticalChance} %");
                break;

            case 2:
                addPetCriDmgBuff = 150;
                Debug.Log($"크리 피해량 버프 시작 => 현재 치명타피해증가량 {CriticalChance} %");

                yield return waitBuffTime;
                addPetCriChanceBuff = 0;
                Debug.Log($"크리 피버프 종료 => 현재 치명타피해증가량 {criticalChance} %");
                break;
        }
    }

}
