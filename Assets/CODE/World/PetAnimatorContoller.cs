using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAnimatorContoller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // �� < 0�� > �ִϸ����� ���� �Լ�
    public void A_Pet0_Charge()
    {
        PetContollerManager.inst.Pet_0_StartEffect("Charge");
    }
    public void A_Pet0_Attack()
    {
        PetContollerManager.inst.Pet_0_StartEffect("Attack");
    }



    // �� < 1�� > �ִϸ����� ���� �Լ�
    public void A_Pet1_Charge()
    {
        PetContollerManager.inst.Pet_1_StartEffect("Charge");
    }
    public void A_Pet1_Attack()
    {
        PetContollerManager.inst.Pet_1_StartEffect("Attack");
    }

    // �� < 2�� > �ִϸ����� ���� �Լ�
    public void A_Pet2_Charge()
    {
        PetContollerManager.inst.Pet_2_StartEffect("Charge");
    }
    public void A_Pet2_Attack()
    {
        PetContollerManager.inst.Pet_2_StartEffect("Attack");
    }

    // ��������Ʈ ����� �ִ� �Լ�
    public void A_Pet2_EffectFuntion()
    {
        ActionManager.inst.A_CrewAttackToEnemy(1);
    }
}
