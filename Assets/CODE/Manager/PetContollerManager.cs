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

    // ��0��
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

        //0�� ����
        petAnim[0] = playerObj.transform.Find("Pet_0").GetComponent<Animator>();
        pet0Ps[0] = petAnim[0].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet0Ps[1] = enemyObj.transform.Find("0").GetComponent<ParticleSystem>();

        //0�� ����
        petAnim[1] = playerObj.transform.Find("Pet_1").GetComponent<Animator>();
        pet1Ps[0] = petAnim[1].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet1Ps[1] = petAnim[1].transform.Find("AttackBuff").GetComponent<ParticleSystem>();
        pet1Ps[2] = petAnim[1].transform.Find("CriBuff").GetComponent<ParticleSystem>();
        pet1Ps[3] = petAnim[1].transform.Find("AllBuff").GetComponent<ParticleSystem>();

        //1�� ���

        petAnim[2] = playerObj.transform.Find("Pet_2").GetComponent<Animator>();
        pet2Ps[0] = petAnim[2].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet2Ps[1] = petAnim[2].transform.Find("Gold").GetComponent<ParticleSystem>(); //�����

        animCount = petAnim.Length;
    }


    // �� �̵� �� ���� �ִϸ��̼� ��Ʈ��
    public void PetAnimPlay(bool action)
    {
        for (int index = 0; index < animCount; index++)
        {
            petAnim[index].SetBool("Move", action);
        }
    }

    // �� 0��
    /// <summary>
    /// Pet0�� ��ƼŬ ����Ʈ
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

    // �� 1��
    /// <summary>
    /// Pet0�� ��ƼŬ ����Ʈ
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
            PetBuffAcitve(); // ��������
        }
    }


    public void PetBuffAcitve()
    {
        //�ֻ���������
        int dice = Random.Range(0, 100);

        if (dice >= 0 && dice < 40) // ���ݷ� ����
        {
            ActiveBuff(0);

        }
        else if (dice >= 40 && dice < 80) //ũ��Ƽ�� ����
        {
            ActiveBuff(1);
        }
        else if (dice > 80) // ��� ����
        {
            ActiveBuff(2);
        }
    }



    private void ActiveBuff(int buffNum)
    {
        switch (buffNum)
        {
            case 0: // ���� ���ݷ� * ����+1 �߰�
                GameStatus.inst.AddPetAtkBuff = CalCulator.inst.StringAndIntMultiPly(CalCulator.inst.Get_CurPlayerATK(), GameStatus.inst.Pet0_Lv);
                Pet1_Particle_Player(1);
                break;

            case 1: // ũ��Ƽ��Ȯ�� 10%������
                GameStatus.inst.AddPetCriChanceBuff = 10 * GameStatus.inst.Pet1_Lv;
                Pet1_Particle_Player(2);
                break;

            case 2: // ��� ����
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
    ///  1 ���� / 2 ũ�� / 3 ���
    /// </summary>
    /// <param name="MagicIndex"></param>
    public void Pet1_Particle_Player(int MagicIndex)
    {
        pet1Ps[MagicIndex].Play();
    }

    // �� 2��
    /// <summary>
    /// Pet0�� ��ƼŬ ����Ʈ
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
            int Pet2Lv = GameStatus.inst.Pet2_Lv + 1;  // ������ 2�� 3�� 4�� ��Ӵþ
            //��� ����
            string curGetGoldValue = CalCulator.inst.StringAndIntMultiPly(curGetGold, Pet2Lv);
            GameStatus.inst.PlusGold(curGetGoldValue);

        }
    }
    /// <summary>
    /// �̵��� ��ƼŬ ��ü ���� 
    /// </summary>
    public void PetAllParticle_Stop()
    {
        pet0Ps[0].Stop();
        pet1Ps[0].Stop();
        pet2Ps[0].Stop();
    }


    /// <summary>
    /// �� On/Off Ȱ��ȭ �Լ�
    /// </summary>
    /// <param name="petNum">0 = ������ / 1 = ������ / 2 =����� </param>
    public void PetActive(int petNum)
    {
        if(petAnim[petNum].gameObject.activeSelf == false)
        {
            petAnim[petNum].gameObject.SetActive(true);
        }
    }
}
