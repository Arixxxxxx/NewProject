using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] float key; // ȯ���� �ִ� Ű
    [SerializeField] float ruby;
    [SerializeField] int rebirthToken; // ȯ�� ��ū
    [Header("# Player Info")]
    [Space]
    [SerializeField] float atkPower; 
    [SerializeField] float atkSpeed; // ���ݼӵ� ����
    [SerializeField] float criticalChance;  //ũ��Ƽ�� Ȯ��
    [SerializeField] float criticalPower; // ũ��Ƽ�� ��������
    [Space]
    [Header("# Stage Info")]
    [SerializeField] float stageLv; // ���� 
    [SerializeField] int floorLv; // �ش� ���� ���� �ܰ� 
    [Space]
    [Header("# Total Get Resource")]
    [SerializeField] float mosterKill; 
    [SerializeField] float bossKill;
    [SerializeField] float getGold;
    [SerializeField] int rebirthCount; // ȯ�� Ƚ��

    void Start()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
