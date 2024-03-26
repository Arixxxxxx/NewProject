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
    ParticleSystem[] pet1Ps = new ParticleSystem[2];
    ParticleSystem[] pet2Ps = new ParticleSystem[2];
    private void Awake()
    {
        if(inst == null)
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
        petAnim[0]  = playerObj.transform.Find("Pet_0").GetComponent<Animator>();
        pet0Ps[0] = petAnim[0].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet0Ps[1] = enemyObj.transform.Find("0").GetComponent<ParticleSystem>();

        //0번 버퍼
        petAnim[1] = playerObj.transform.Find("Pet_1").GetComponent<Animator>();
        pet1Ps[0] = petAnim[1].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet1Ps[1] = petAnim[1].transform.Find("Buff").GetComponent<ParticleSystem>();

        //1번 골드

        petAnim[2] = playerObj.transform.Find("Pet_2").GetComponent<Animator>();
        pet2Ps[0] = petAnim[2].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet2Ps[1] = petAnim[2].transform.Find("Gold").GetComponent<ParticleSystem>(); //골드펑

        animCount = petAnim.Length;
    }


    // 펫 이동 및 공격 애니메이션 컨트롤
    public void PetAnimPlay(bool action)
    {
        for(int index = 0; index < animCount; index++)
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
        if(Value == "Charge")
        {
            pet0Ps[0].Play();
        }
        else if(Value == "Attack")
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
            pet1Ps[1].Play();

            //버프량 증가
            GameStatus.inst.PetBuffAcitve();
        }
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

            //골드 증가
            
        }
    }
    /// <summary>
    /// 이동시 파티클 전체 종료 
    /// </summary>
    public void PetAllParticle_Stop()
    {
        pet0Ps[0].Stop();
        pet1Ps[0].Stop();
    }
}
