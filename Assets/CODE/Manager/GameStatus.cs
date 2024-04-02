using System;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus inst;

    ///////////////////////////////////////////////////
    //���� ���� �Ͻ�
    // �̺κ� ���߿� ù ���Ժκ����� �Űܾ���
    // ���ʿ� �ʿ��Ѱ� ȸ���������� 
    // ù �����ޱ� ������ ����

    DateTime firstdate = DateTime.Now;
    
    int[] getGiftDay = new int[3]; // �������� ��/��/��
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

    int[] getNewbieGiftDay = new int[3]; // �������� ��/��/��
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
   
    int gotDilayPlayGiftCount; // ���� ���� Ƚ��
    public int GotDilayPlayGiftCount
    {
        get { return gotDilayPlayGiftCount; }
        set { gotDilayPlayGiftCount = value; }
    }

    int gotNewbieGiftCount; // ���� ���� Ƚ��
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
            Debug.Log($"������ {ruby}");
            WorldUI_Manager.inst.CurMaterialUpdate(3, ruby.ToString());
        }
    }


    string rebirthToken; // ȯ�� ��ū

    int atkSpeedLv; // ���ݼӵ� ����

    public int AtkSpeedLv
    {
        get { return atkSpeedLv; }
        set
        {
            atkSpeedLv = value;
            ActionManager.inst.PlayerAttackSpeedLvUp(atkSpeedLv);
        }
    }

    float criticalChance = 20;  //ũ��Ƽ�� Ȯ��

    
    public float CriticalChance { get { return criticalChance + addPetCriChanceBuff; } set { criticalChance = value;  } }

    float criticalPower = 0; // ũ��Ƽ�� ��������
    public float CriticalPower { get { return criticalPower + addPetCriDmgBuff; } set { criticalChance = value; } }
    [Space]
    [Header("# Stage Info")]
    int stageLv = 1; // ���� 

    int accumlateFloor = 1; // ��������
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
    int floorLv = 1; // �ش� ���� ���� �ܰ� 
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



    int lvUpPower = 10; // ����ĳ���� ���ݷ� * % ��ġ
    public int LvUpPower
    {
        get
        {
            return lvUpPower;
        }
    }

    int upGradeLv = 1; // ���׷��̵� ���� => ������ 10�� ����
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
    [SerializeField] int rebirthCount; // ȯ�� Ƚ��

    /////////////////////////////// ���� ���� ������ ���� //////////////////////////////////
    
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

    /////////////////////////////// �� ���� ������ ���� //////////////////////////////////

    int pet0_Lv = 1;
    public int Pet0_Lv // ������
    {
        get { return pet0_Lv; }
        set { pet0_Lv = value; }
    }

    int pet1_Lv = 1;
    public int Pet1_Lv // ������
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


    int pet2_Lv = 1;  //�����
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
        // �������� ������ �޾ƿͼ� �ʱ�ȭ����
    
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
    /// ���������� ������� ��Ű�� �Լ�
    /// </summary>
    /// <param name="getValue"></param>
    public void GetGold(string getValue)
    {
        string result = CalCulator.inst.DigidPlus(gold, getValue); // �⺻ ���
        result = CalCulator.inst.DigidPlus(result, buffAddGold); // ���� ���������Ѱ� �߰�
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
        Debug.Log($"��ȯ�� {result} / ���簪 {gold} , {getValue}");
        inputMinusGold = result;
    }
}
