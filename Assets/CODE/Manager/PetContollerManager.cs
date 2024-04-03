using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetContollerManager : MonoBehaviour
{
    public static PetContollerManager inst;

    GameObject playerObj;
    GameObject enemyObj;

    Animator[] petAnim = new Animator[3];
    int animCount = 0;

    // 펫0번
    ParticleSystem[] pet0Ps = new ParticleSystem[2];
    ParticleSystem[] pet1Ps = new ParticleSystem[4];
    ParticleSystem[] pet2Ps = new ParticleSystem[2];
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
    }

    void Start()
    {
        playerObj = ActionManager.inst.ReturnPlayerObjInHierachy();
        enemyObj = ActionManager.inst.ReturnEnemyObjInHierachy().transform.Find("PetEffect").gameObject;

        //0번 공격
        petAnim[0] = playerObj.transform.Find("Pet_0").GetComponent<Animator>();
        pet0Ps[0] = petAnim[0].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet0Ps[1] = enemyObj.transform.Find("0").GetComponent<ParticleSystem>();

        //0번 버퍼
        petAnim[1] = playerObj.transform.Find("Pet_1").GetComponent<Animator>();
        pet1Ps[0] = petAnim[1].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet1Ps[1] = petAnim[1].transform.Find("AttackBuff").GetComponent<ParticleSystem>();
        pet1Ps[2] = petAnim[1].transform.Find("CriBuff").GetComponent<ParticleSystem>();
        pet1Ps[3] = petAnim[1].transform.Find("AllBuff").GetComponent<ParticleSystem>();

        //1번 골드

        petAnim[2] = playerObj.transform.Find("Pet_2").GetComponent<Animator>();
        pet2Ps[0] = petAnim[2].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet2Ps[1] = petAnim[2].transform.Find("Gold").GetComponent<ParticleSystem>(); //골드펑

        animCount = petAnim.Length;
    }


    // 펫 이동 및 공격 애니메이션 컨트롤
    public void PetAnimPlay(bool action)
    {
        for (int index = 0; index < animCount; index++)
        {
            petAnim[index].SetBool("Move", action);
        }
    }

    // 펫 0번
    /// <summary>
    /// Pet0번 파티클 이펙트
    /// </summary>
    /// <param name="Value"> Charge , Attack </param>
    public void Pet_0_StartEffect(string Value)
    {
        if (Value == "Charge")
        {
            pet0Ps[0].Play();
        }
        else if (Value == "Attack")
        {
            ActionManager.inst.A_Pet0AttackToEnemy();
            pet0Ps[0].Stop();
            pet0Ps[1].Play();
        }
    }

    // 펫 1번
    /// <summary>
    /// Pet0번 파티클 이펙트
    /// </summary>
    /// <param name="Value"> Charge , Attack </param>
    public void Pet_1_StartEffect(string Value)
    {
        if (Value == "Charge")
        {
            pet1Ps[0].Play();
        }
        else if (Value == "Attack")
        {
            pet1Ps[0].Stop();
            PetBuffAcitve(); // 버프증가
        }
    }


    public void PetBuffAcitve()
    {
        //주사위굴리고
        int dice = Random.Range(0, 100);

        if (dice >= 0 && dice < 40) // 공격력 증가
        {
            ActiveBuff(0);

        }
        else if (dice >= 40 && dice < 80) //크리티컬 증가
        {
            ActiveBuff(1);
        }
        else if (dice > 80) // 모두 증가
        {
            ActiveBuff(2);
        }
    }



    private void ActiveBuff(int buffNum)
    {
        switch (buffNum)
        {
            case 0: // 현재 공격력 * 레벨+1 추가
                GameStatus.inst.AddPetAtkBuff = CalCulator.inst.StringAndIntMultiPly(CalCulator.inst.Get_CurPlayerATK(), GameStatus.inst.Pet0_Lv);
                Pet1_Particle_Player(1);
                break;

            case 1: // 크리티컬확률 10%씩증가
                GameStatus.inst.AddPetCriChanceBuff = 10 * GameStatus.inst.Pet1_Lv;
                Pet1_Particle_Player(2);
                break;

            case 2: // 모두 증가
                GameStatus.inst.AddPetAtkBuff = CalCulator.inst.StringAndIntMultiPly(CalCulator.inst.Get_CurPlayerATK(), GameStatus.inst.Pet0_Lv);
                GameStatus.inst.AddPetCriChanceBuff = 10 * GameStatus.inst.Pet1_Lv;
                Pet1_Particle_Player(3);
                break;
        }
    }

    public void AttackBuffDisable()
    {
        GameStatus.inst.AddPetAtkBuff = "0";
        GameStatus.inst.AddPetCriChanceBuff = 0;
    }
    /// <summary>
    ///  1 어택 / 2 크리 / 3 모두
    /// </summary>
    /// <param name="MagicIndex"></param>
    public void Pet1_Particle_Player(int MagicIndex)
    {
        pet1Ps[MagicIndex].Play();
    }

    // 펫 2번
    /// <summary>
    /// Pet0번 파티클 이펙트
    /// </summary>
    /// <param name="Value"> Charge , Attack </param>
    public void Pet_2_StartEffect(string Value)
    {
        if (Value == "Charge")
        {
            pet2Ps[0].Play();
        }
        else if (Value == "Attack")
        {
            pet2Ps[0].Stop();
            pet2Ps[1].Play();

            string curGetGold = UIManager.Instance.GetTotalGold();
            int Pet2Lv = GameStatus.inst.Pet2_Lv + 1;  // 레벨당 2배 3배 4배 계속늘어남
            //골드 증가
            string curGetGoldValue = CalCulator.inst.StringAndIntMultiPly(curGetGold, Pet2Lv);
            GameStatus.inst.PlusGold(curGetGoldValue);

        }
    }
    /// <summary>
    /// 이동시 파티클 전체 종료 
    /// </summary>
    public void PetAllParticle_Stop()
    {
        pet0Ps[0].Stop();
        pet1Ps[0].Stop();
        pet2Ps[0].Stop();
    }


    /// <summary>
    /// 펫 On/Off 활성화 함수
    /// </summary>
    /// <param name="petNum">0 = 공격펫 / 1 = 버프펫 / 2 =골드펫 </param>
    public void PetActive(int petNum)
    {
        if(petAnim[petNum].gameObject.activeSelf == false)
        {
            petAnim[petNum].gameObject.SetActive(true);
        }
    }
}
