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

    string star = "0"; // È¯»ı½Ã ÁÖ´Â Å°
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


    string rebirthToken; // È¯»ı ÅäÅ«

    int atkSpeedLv; // °ø°İ¼Óµµ Áõ°¡

    public int AtkSpeedLv
    {
        get { return atkSpeedLv; }
        set
        {
            atkSpeedLv = value;
            ActionManager.inst.PlayerAttackSpeedLvUp(atkSpeedLv);
        }
    }

    float criticalChance = 20;  //Å©¸®Æ¼ÄÃ È®·ü

    
    public float CriticalChance { get { return criticalChance + addPetCriChanceBuff; } set { criticalChance = value;  } }

    float criticalPower = 0; // Å©¸®Æ¼ÄÃ ÇÇÇØÁõ°¡
    public float CriticalPower { get { return criticalPower + addPetCriDmgBuff; } set { criticalChance = value; } }
    [Space]
    [Header("# Stage Info")]
    int stageLv = 1; // Ãş¼ö 
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

    int floorLv = 1; // ÇØ´ç ÃşÀÇ ¸ó½ºÅÍ ´Ü°è 
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



    int lvUpPower = 10; // ¸ŞÀÎÄ³¸¯ÅÍ °ø°İ·Â * % ¼öÄ¡
    public int LvUpPower
    {
        get
        {
            return lvUpPower;
        }
    }

    int upGradeLv = 1; // ¾÷±×·¹ÀÌµå ·¹º§ => ·¹º§´ç 10¾¿ Áõ°¡
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
    [SerializeField] int rebirthCount; // È¯»ı È½¼ö


    /////////////////////////////// Æê ¹öÇÁ Áõ°¡·® °ü·Ã //////////////////////////////////

    int pet0_Lv = 1;
    public int Pet0_Lv // °ø°İÆê
    {
        get { return pet0_Lv; }
        set { pet0_Lv = value; }
    }

    int pet1_Lv = 1;
    public int Pet1_Lv // ¹öÇÁÆê
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


    int pet2_Lv = 1;  //°ñµåÆê
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
        // ¼­¹ö¿¡¼­ µ¥ÀÌÅÍ ¹Ş¾Æ¿Í¼­ ÃÊ±âÈ­ÇØÁÜ
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
    /// ½ÇÁúÀûÀ¸·Î °ñµåÁõ°¡ ½ÃÅ°´Â ÇÔ¼ö
    /// </summary>
    /// <param name="getValue"></param>
    public void GetGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue);
        Gold = result;
    }

    public void TakeGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue);
        WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(0, CalCulator.inst.StringFourDigitChanger(getValue));
        Gold = result;
    }

<<<<<<< HEAD



=======
    //public void MinusGold(string getValue)
    //{
    //    string result = CalCulator.inst.DigidMinus(gold, getValue);
    //    Gold = result;
    //}
>>>>>>> 70a1d48 (ui ì´ë¯¸ì§€ ì‚½ì…ì¤‘)
}
