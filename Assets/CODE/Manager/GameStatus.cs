using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus inst;


    [Header("# Resource")]
    [SerializeField] float gold;
    public float Gold { get { return gold; } set { gold = value; } }

    [SerializeField] float key; // ȯ���� �ִ� Ű
    [SerializeField] float ruby;
    [SerializeField] int rebirthToken; // ȯ�� ��ū

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
    float criticalPower = 0; // ũ��Ƽ�� ��������
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }

    [Space]
    [Header("# Stage Info")]
    int stageLv = 1; // ���� 
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
            if (floorLv == 5)
            {
                floorLv = 0;
                accumlateFloor++;
                stageLv++;
            }
        }
    }

    [Space]
    [Header("# Total Get Resource")]
    [SerializeField] float mosterKill;
    [SerializeField] float bossKill;
    [SerializeField] float getGold;
    [SerializeField] int rebirthCount; // ȯ�� Ƚ��

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



}
