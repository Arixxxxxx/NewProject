using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetContollerManager : MonoBehaviour
{
    public static PetContollerManager inst;

    GameObject playerObj;
    GameObject effectRef, frontUiRef;

    Animator[] petAnim = new Animator[3];
    int animCount = 0;

    // ��0��
    ParticleSystem[] pet0Ps = new ParticleSystem[2];
    ParticleSystem pet0Dust;


    ParticleSystem[] pet1Ps = new ParticleSystem[4];
    ParticleSystem pet1Dust;

    ParticleSystem[] pet2Ps = new ParticleSystem[2];

    // ��2�� ���� ����Ʈ
    Animator pet2AtkEffectAnim;

    // �� �׸��ڵ�
    GameObject[] petShadow = new GameObject[3];
    GameObject[] petWind = new GameObject[3];


    // �� ���� ����
    GameObject petUnlockRef;
    GameObject particleRef;
    Animator charSelectAnim;
    Image whiteBg, charBg;
    Button unlockWindowXbtn;
    TMP_Text charNameText;
    TMP_Text charInfoText;
    Transform[] lvupTextTrs = new Transform[3];
    RectTransform lvupTextRectTrs;
    
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

        playerObj = ActionManager.inst.ReturnPlayerObjInHierachy();
        effectRef = GameManager.inst.WorldSpaceRef.transform.Find("Effect").gameObject;
        frontUiRef = GameManager.inst.FrontUiRef;

        // ������ ���� (�������)
        lvupTextRectTrs = GameManager.inst.WorldSpaceRef.transform.Find("Player_Obj/LvUPCanvas").GetComponent<RectTransform>();
        lvupTextTrs[0] = GameManager.inst.WorldSpaceRef.transform.Find("Player_Obj/Pet_0");
        lvupTextTrs[1] = GameManager.inst.WorldSpaceRef.transform.Find("Player_Obj/Pet_1");
        lvupTextTrs[2] = GameManager.inst.WorldSpaceRef.transform.Find("Player_Obj/Pet_2");
        //0�� ����
        petAnim[0] = playerObj.transform.Find("Pet_0").GetComponent<Animator>();
        pet0Dust = petAnim[0].transform.Find("Dust").GetComponent<ParticleSystem>();
        pet0Ps[0] = petAnim[0].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet0Ps[1] = effectRef.transform.Find("PetEffect/Pet_0_AtkEffect").GetComponent<ParticleSystem>();

        //1�� ����
        petAnim[1] = playerObj.transform.Find("Pet_1").GetComponent<Animator>();
        pet1Ps[0] = petAnim[1].transform.Find("Charge").GetComponent<ParticleSystem>();
        pet1Ps[1] = petAnim[1].transform.Find("AttackBuff").GetComponent<ParticleSystem>();
        pet1Ps[2] = petAnim[1].transform.Find("CriBuff").GetComponent<ParticleSystem>();
        pet1Ps[3] = petAnim[1].transform.Find("AllBuff").GetComponent<ParticleSystem>();
        pet1Dust = petAnim[1].transform.Find("Dust").GetComponent<ParticleSystem>();

        //2�� ��ɼ���

        petAnim[2] = playerObj.transform.Find("Pet_2").GetComponent<Animator>();
        pet2AtkEffectAnim = petAnim[2].transform.Find("AttackEffect/Enemy").GetComponent<Animator>();

        //pet2Ps[0] = petAnim[2].transform.Find("Charge").GetComponent<ParticleSystem>();
        //pet2Ps[1] = petAnim[2].transform.Find("Gold").GetComponent<ParticleSystem>(); //�����

        //�� �׸���
        petShadow[0] = playerObj.transform.Find("ShadowGroup/Pet0_Shadow").gameObject;
        petShadow[1] = playerObj.transform.Find("ShadowGroup/Pet1_Shadow").gameObject;
        petShadow[2] = playerObj.transform.Find("ShadowGroup/Pet2_Shadow").gameObject;

        petWind[0] = playerObj.transform.Find("Player/MoveWind/Pet0_Wind").gameObject;
        petWind[1] = playerObj.transform.Find("Player/MoveWind/Pet1_Wind").gameObject;
        petWind[2] = playerObj.transform.Find("Player/MoveWind/Pet2_Wind").gameObject;
        animCount = petAnim.Length;

        // ���� ��� ����
        petUnlockRef = frontUiRef.transform.Find("PetUnlockAction").gameObject;
        particleRef = petUnlockRef.transform.Find("StartParticle").gameObject;
        charSelectAnim = petUnlockRef.transform.Find("Middle/Char").GetComponent<Animator>();
        charBg = petUnlockRef.transform.Find("Middle/BG").GetComponent<Image>();
        whiteBg = petUnlockRef.transform.Find("White").GetComponent<Image>();
        unlockWindowXbtn = petUnlockRef.transform.Find("Button").GetComponent<Button>();
        unlockWindowXbtn.onClick.AddListener(() => { CrewUnlock_Action(0, false); });
        charNameText = petUnlockRef.transform.Find("Detail/CharName").GetComponent<TMP_Text>();
        charInfoText = petUnlockRef.transform.Find("Detail/InfoText").GetComponent<TMP_Text>();
    }

   


    /// <summary>
    /// ���� ���Ž� ù ���ſ��� 
    /// </summary>
    /// <param name="crewNumber"> 0��ź�� / 1�丮�� / 2��ɼ��� </param>
    public void CrewUnlock_Action(int crewNumber, bool bValue)
    {
       
        if (bValue)
        {
            AudioManager.inst.Crew_Play_SFX(3, 1);
            whiteBg.color = Color.white;

            // ����
            switch (crewNumber) 
            {
                case 0:
                    AudioManager.inst.Crew_Play_SFX(5, 0.7f);
                    charBg.color = new Color(1, 0.46f, 0, 0.85f);
                    charNameText.text = $"����ũ (��ź��)";
                    charInfoText.text = "������ ������ ��ź�� ���� �����ϴ� ����\r\n����ĳ������ ���ݷ� x 3��\r\n<color=yellow>(���� ��ȭ�� 1�辿 ����)";
                    break;

                case 1:
                    AudioManager.inst.Crew_Play_SFX(6, 0.7f);
                    charBg.color = new Color(0, 1, 0, 0.85f);
                    charNameText.text = $"ȣ�� (�丮��)";
                    charInfoText.text = "�Ʊ����� ���ִ� �丮�� ������ �ִ� ����\r\n25%Ȯ�� ���ݷ����� �Ǵ� ġ��ŸȮ�� ����\r\n<color=yellow>(���� ��ȭ�� Ȯ�� ����)";
                    break;

                case 2:
                    AudioManager.inst.Crew_Play_SFX(7, 0.7f);
                    charBg.color = new Color(0.65f, 0, 1, 0.85f);
                    charNameText.text = $"��ȭ (��ɼ���)";
                    charInfoText.text = "���� ����� ������� �����ϴ� ����\r\n�� ���ݽ�  ������� 1% ��ŭ ����\r\n<color=yellow>(���� ��ȭ�� 1%�� ����)";
                    break;
            }
            whiteBg.gameObject.SetActive(true);
            petUnlockRef.SetActive(true);
            StartCoroutine(PlayUnlock(crewNumber));
        }
        else
        {
            AudioManager.inst.Play_Ui_SFX(3, 1f);
            particleRef.SetActive(false);
            petUnlockRef.SetActive(false);
        }
    }

    Color fadeColor = new Color(0, 0, 0, 0.1f);
    float fadeSpeedMultipley = 9f;
    IEnumerator PlayUnlock(int value)
    {
        yield return null;

        charSelectAnim.SetTrigger(value.ToString());

        while (whiteBg.color.a > 0)
        {
            whiteBg.color -= fadeColor * Time.deltaTime * fadeSpeedMultipley;
            
            if (whiteBg.color.a < 0.6f && particleRef.activeSelf == false)
            {
                particleRef.SetActive(true);
            }
                yield return null;
        }

        whiteBg.gameObject.SetActive(false);
    }


    // �� �̵� �� ���� �ִϸ��̼� ��Ʈ��
    public void PetAnimPlay(bool action)
    {
        for (int index = 0; index < animCount; index++)
        {
            if (petAnim[index].gameObject.activeSelf == false) { continue; }
            petAnim[index].SetBool("Move", action);
        }

        pet0Dust.gameObject.SetActive(action);
        pet1Dust.gameObject.SetActive(action);
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
            ActionManager.inst.A_CrewAttackToEnemy(0);
            pet0Ps[0].Stop();
            pet0Ps[1].transform.position = ActionManager.inst.Get_CurEnemyCenterPosition();
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

    /// <summary>
    /// �丮�� ����
    /// </summary>
    /// <param name="buffNum"></param>
    public void PetBuffAcitve()
    {
        //�ֻ���������
        int dice = UnityEngine.Random.Range(0, 100);

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



    /// <summary>
    /// �丮�� ����
    /// </summary>
    /// <param name="buffNum"></param>
    private void ActiveBuff(int buffNum)
    {
        switch (buffNum)
        {
            case 0: // ���� ���ݷ� * ����+1 �߰�
                GameStatus.inst.AddPetAtkBuff = "1";
                Pet1_Particle_Player(1);
                break;

            case 1: // ũ��Ƽ��Ȯ�� 10%������
                GameStatus.inst.AddPetCriChanceBuff = 10 + GameStatus.inst.Pet1_Lv;
                Pet1_Particle_Player(2);
                break;

            case 2: // ��� ����
                GameStatus.inst.AddPetAtkBuff = CalCulator.inst.StringAndIntMultiPly(CalCulator.inst.Get_CurPlayerATK(), GameStatus.inst.Pet0_Lv);
                GameStatus.inst.AddPetCriChanceBuff = 10 + GameStatus.inst.Pet1_Lv;
                Pet1_Particle_Player(3);
                break;
        }
    }

    // ��ᰭȭ�� ������ ����
    WaitForSeconds lvupTextDurationTime = new WaitForSeconds(5f);
    Coroutine lvuptext;
    public void PetLvUp_WorldText_Active(int petType)
    {
        lvupTextRectTrs.gameObject.transform.localPosition = lvupTextTrs[petType].localPosition;

        if(lvuptext != null)
        {
            lvupTextRectTrs.gameObject.SetActive(false);
            StopCoroutine(lvuptext);
            lvuptext = null;
        }

        lvuptext = StartCoroutine(lvupTextPlay());

    }

    IEnumerator lvupTextPlay()
    {
        lvupTextRectTrs.gameObject.SetActive(true);
        yield return lvupTextDurationTime;
        lvupTextRectTrs.gameObject.SetActive(false);
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


    float upperPos = 1.2f;
    // �� 2��
    /// <summary>
    /// Pet0�� ��ƼŬ ����Ʈ
    /// </summary>
    /// <param name="Value"> Charge , Attack </param>
    public void Pet_2_StartEffect(string Value)
    {
        if (Value == "Charge") // ������
        {

        }
        else if (Value == "Attack") //����
        {
             Vector3 Pos = ActionManager.inst.Get_CurEnemyCenterPosition();
            Pos.y += upperPos;
            pet2AtkEffectAnim.transform.position = Pos;
            pet2AtkEffectAnim.SetTrigger("Hit");
        }
    }
    /// <summary>
    /// �̵��� ��ƼŬ ��ü ���� 
    /// </summary>
    public void PetAllParticle_Stop()
    {
        pet0Ps[0].Stop();
        pet1Ps[0].Stop();
        //pet2Ps[0].Stop();
    }


    /// <summary>
    /// �� On/Off Ȱ��ȭ �Լ�
    /// </summary>
    /// <param name="petNum">0 = ������ / 1 = ������ / 2 =����� </param>
    public void PetActive(int petNum)
    {
        if (petAnim[petNum].gameObject.activeSelf == false)
        {
            petWind[petNum].SetActive(true);
            petAnim[petNum].gameObject.SetActive(true);
            petShadow[petNum].SetActive(true);
            StartCoroutine(PlayAnim(petNum));
        }
    }

    /// <summary>
    /// �� On/Off Ȱ��ȭ �Լ�
    /// </summary>
    /// <param name="petNum">0 = ������ / 1 = ������ / 2 =����� </param>
    public void PetActive(int petNum, int PetLv)
    {
        if(PetLv <= 0) { return; }
        
        if (petAnim[petNum].gameObject.activeSelf == false)
        {
            petWind[petNum].SetActive(true);
            petAnim[petNum].gameObject.SetActive(true);
            petShadow[petNum].SetActive(true);
            StartCoroutine(PlayAnim(petNum));
        }
    }


    //���۽� �ִϸ��̼� �۵�
    IEnumerator PlayAnim(int petNum)
    {
        yield return null;
        petAnim[petNum].SetBool("Move", ActionManager.inst.IsMove);
    }

}
