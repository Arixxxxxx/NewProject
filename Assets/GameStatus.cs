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
    [SerializeField] float key; // 환생시 주는 키
    [SerializeField] float ruby;
    [SerializeField] int rebirthToken; // 환생 토큰
    [Header("# Player Info")]
    [Space]
    [SerializeField] float atkPower; 
    [SerializeField] float atkSpeed; // 공격속도 증가
    [SerializeField] float criticalChance;  //크리티컬 확률
    [SerializeField] float criticalPower; // 크리티컬 피해증가
    [Space]
    [Header("# Stage Info")]
    [SerializeField] float stageLv; // 층수 
    [SerializeField] int floorLv; // 해당 층의 몬스터 단계 
    [Space]
    [Header("# Total Get Resource")]
    [SerializeField] float mosterKill; 
    [SerializeField] float bossKill;
    [SerializeField] float getGold;
    [SerializeField] int rebirthCount; // 환생 횟수

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
