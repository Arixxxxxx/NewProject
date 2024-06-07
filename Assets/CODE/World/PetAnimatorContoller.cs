using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAnimatorContoller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 펫 < 0번 > 애니메이터 실행 함수
    public void A_Pet0_Charge()
    {
        PetContollerManager.inst.Pet_0_StartEffect("Charge");
    }
    public void A_Pet0_Attack()
    {
        AudioManager.inst.Crew_Play_SFX(0,0.6f);
        PetContollerManager.inst.Pet_0_StartEffect("Attack");
    }



    // 호두 애니메이터 실행 함수
    public void A_Pet1_Charge()
    {
        PetContollerManager.inst.Pet_1_StartEffect("Charge");
    }
    public void A_Pet1_Attack()
    {
        AudioManager.inst.Crew_Play_SFX(1, 0.2f);
        PetContollerManager.inst.Pet_1_StartEffect("Attack");
    }

    // 펫 < 2번 > 애니메이터 실행 함수
    public void A_Pet2_Charge()
    {
        PetContollerManager.inst.Pet_2_StartEffect("Charge");
    }
    public void A_Pet2_Attack()
    {
        PetContollerManager.inst.Pet_2_StartEffect("Attack");
    }

    // 공격이펙트 대미지 주는 함수
    public void A_Pet2_EffectFuntion()
    {
        ActionManager.inst.A_CrewAttackToEnemy(1);
    }

    public void A_Pet2_AttackSound()
    {
        AudioManager.inst.Crew_Play_SFX(2, 0.5f);
    }
}
