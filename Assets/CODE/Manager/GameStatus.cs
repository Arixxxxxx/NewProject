using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus inst;

    [Header("# Check Info")]
    [Space]
    [Header("# Player Info")]
    [Space]
    [Header("# Resource")]
    [SerializeField] float gold;
    public float Gold { get { return gold; } set { gold = value; } }

    [SerializeField] float key; // ȯ���� �ִ� Ű
    [SerializeField] float ruby;
    [SerializeField] int rebirthToken; // ȯ�� ��ū
    [Header("# Player Info")]
    [Space]
    [SerializeField] float atkPower; 
    [SerializeField] int atkSpeedLv; // ���ݼӵ� ����
    public int AtkSpeedLv { get { return atkSpeedLv; } set { Debug.Log("12"); atkSpeedLv = value; ActionManager.inst.PlayerAttackSpeedLvUp(atkSpeedLv); } }

    [SerializeField] float criticalChance;  //ũ��Ƽ�� Ȯ��
    [SerializeField] float criticalPower; // ũ��Ƽ�� ��������
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }

    [Space]
    [Header("# Stage Info")]
    [SerializeField] int stageLv = 1; // ���� 
    public int StageLv { get { return stageLv; } set { stageLv = value; } }

    [SerializeField] int floorLv; // �ش� ���� ���� �ܰ� 
    public int FloorLv   { get { return floorLv; } set { floorLv = value; }}

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
        criticalChance = 40;




    }

    // Update is called once per frame
    void Update()
    {
        
    }
    


}
