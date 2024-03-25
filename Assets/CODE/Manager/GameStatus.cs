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
    float criticalPower = 0; // 크리티컬 피해증가
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }

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

    [Space]
    [Header("# Total Get Resource")]
    [SerializeField] float mosterKill;
    [SerializeField] float bossKill;
    [SerializeField] float getGold;
    [SerializeField] int rebirthCount; // 환생 횟수

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


}
