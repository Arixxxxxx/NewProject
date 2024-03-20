using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ActionManager : MonoBehaviour
{
    public static ActionManager inst;

    //ī�޶�
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin camShake;

    //���
    GameObject worldSpaceGroup;
    
    Material mat;
    [SerializeField] float backGroundSpeed;
    [SerializeField] GameObject[] pooling_Obj;
    Transform dmgFontParent;

    Queue<GameObject>[] prefabsQue;


    //�÷��̾�
    Animator playerAnim;
    int attackSpeedLv;
    float attackSpeed;
    float atkPower;
    SpriteRenderer palyerWeapen;

    // ���ʹ�
    GameObject enemyObj;
    SpriteRenderer enemySr;
    Animator enemyAnim;
    Image hpBar_IMG;
    TMP_Text hpBar_Text;
    [SerializeField] float enemyCurHP;
    [SerializeField] float enemyMaxHP;
    [SerializeField] Sprite[] enemySprite;

    //Ÿ�� ����Ʈ
    [SerializeField] ParticleSystem[] enemyEffect;

    Transform enemy_StartPoint;
    Transform enemy_StopPoint;

    bool actionStart;
    bool isatk, ismove;
    [SerializeField] bool attackReady;

    // �̵��ִϸ��̼�
    [SerializeField] float checkPosition;
    Vector2 enemyVec;
    float enemyPosX;
    [SerializeField] float enemySpawnSpeed;

    

    private void Awake()
    {
        worldSpaceGroup = GameObject.Find("---[World Space]").gameObject;

        
        mat = worldSpaceGroup.transform.Find("BackGround_IMG").GetComponent<SpriteRenderer>().material;
        playerAnim = worldSpaceGroup.transform.Find("Player_Obj").GetComponent<Animator>();
        palyerWeapen = playerAnim.transform.Find("Weapon").GetComponent<SpriteRenderer>();

        enemyObj = worldSpaceGroup.transform.Find("Enemy").gameObject;
        enemySr = enemyObj.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        enemyAnim = enemySr.GetComponent<Animator>();
        enemyEffect = enemyObj.transform.Find("Effect").GetComponentsInChildren<ParticleSystem>();
        
        dmgFontParent = enemyObj.transform.Find("HPBar_Canvas/FontPosition").GetComponent<Transform>();
        hpBar_IMG = enemyObj.transform.Find("HPBar_Canvas/HP_Bar/Front").GetComponent<Image>();
        hpBar_Text = enemyObj.transform.Find("HPBar_Canvas/HP_Bar/Text").GetComponent<TMP_Text>();

        enemy_StartPoint = worldSpaceGroup.transform.Find("SpawnPoint").GetComponent<Transform>();
        enemy_StopPoint = worldSpaceGroup.transform.Find("StopPoint").GetComponent<Transform>();

        cam = GameObject.Find("---[Cams]/Cam_0").GetComponent<CinemachineVirtualCamera>();
        camShake = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Prefabs_Awake();
    }

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

        //���� init
       
        Enemyinit();
        PlayerInit();

    }

    Vector2 matVec;

    private void FixedUpdate()
    {
        if (attackReady == false) // ���� ����
        {
            MoveEnemyAndMap();
        }
    }
    void Update()
    {
        if (attackReady == true) // ����
        {
            AttackEnemy();
        }

        EnemyHPBarUI_RealTimeUpdater();
        atkPowerUpdater();
    }





    private void MoveEnemyAndMap()
    {
        // �����̱��� �ʱ�ȭ
        if (ismove == false)
        {
            ismove = true;
            if (playerAnim.GetBool("Move") == false)
            {
                playerAnim.SetBool("Move", true);
            }

            palyerWeapen.enabled = false;
            enemyPosX = 0;
            enemyVec.x = enemy_StartPoint.transform.position.x;
            enemyVec.y = enemy_StartPoint.transform.position.y;
        }

        // ��� ������
        matVec.x += Time.deltaTime * backGroundSpeed;
        matVec.x = Mathf.Repeat(matVec.x, 1);
        mat.mainTextureOffset = matVec;


        //���ʹ� ���� �� �����ұ��� ����
        checkPosition = Vector2.Distance(enemyObj.transform.position, enemy_StopPoint.position);

        if (checkPosition > 0.5f) // �Ÿ��� üũ 2�̻� �̵�
        {
            enemyPosX += Time.deltaTime * enemySpawnSpeed;
            enemyVec.x -= enemyPosX;
            enemyObj.transform.position = enemyVec;
        }
        else if (checkPosition < 0.5f) // 2�̸� ����
        {
            enemyVec.x = 0;
            attackReady = true;
            ismove = false;
        }
    }


    float count = 0;
    private void AttackEnemy()
    {
        //�ִϸ��̼� ���ְ�
        if(playerAnim.GetBool("Move") == true)
        {
            playerAnim.SetBool("Move", false);
        }

        if(palyerWeapen.enabled == false)
        {
            palyerWeapen.enabled = true;
        }


        //����
        count += Time.deltaTime;

        if(count > attackSpeed)
        {
            count = 0;
            StopCoroutine(enemyOnHit());
            StartCoroutine(enemyOnHit());
        }
    }


    IEnumerator enemyOnHit()
    {
        yield return null;
        enemyAnim.SetTrigger("Hit");
        if (enemyCurHP - atkPower > 0)
        {
            enemyCurHP -= atkPower;
            // �������Ʈ
            GameObject obj = Get_Pooling_Prefabs(0);
            obj.transform.position = dmgFontParent.position;
            float randomDice = Random.Range(0f, 100f);

            //ũ��Ƽ�� ������ ķ��鸲
            if (randomDice < GameStatus.inst.CriticalChance == true)
            {
                F_PlayerOnHitCamShake();
            }

            obj.GetComponent<DMG_Font>().SetText(atkPower.ToString(), randomDice < GameStatus.inst.CriticalChance ? true : false);
            obj.SetActive(true);

            EnemyOnHitEffect();
        }
        else if(enemyCurHP - atkPower <= 0)
        {
            //���ʹ� ��� �� �ʱ�ȭ
            Enemyinit();
            attackReady = false;
        }
        
    }

    int effectIndexCount = 0;

    //���ʹ� �ǰ� ����Ʈ �Լ�
    private void EnemyOnHitEffect()
    {
        if (effectIndexCount == enemyEffect.Length - 1)
        {
            effectIndexCount = 0;
        }

        enemyEffect[effectIndexCount].Play();
        effectIndexCount++;
    }
    bool isUIActive;


    private void Enemyinit()
    {
        // ���߿� ü�� �ʱ�ȭ ���� �ٲ����
        enemyObj.transform.position = enemy_StartPoint.position; // ��ġ �ʱ�ȭ
        enemyMaxHP = /*GameStatus.inst.StageLv **/ 100; // ü���ʱ�ȭ
        enemyCurHP = enemyMaxHP;

        //Hpbar �ʱ�ȭ
        hpBar_IMG.fillAmount = 1;
        hpBar_Text.text = string.Empty;

        //��������Ʈ �� �Ҵ�
        int spriteCount = enemySprite.Length;
        int spirteRanValue = Random.Range(0, spriteCount);
        enemySr.sprite = enemySprite[spirteRanValue];

       
    }



    private void PlayerInit()
    {
        attackSpeedLv = GameStatus.inst.AtkSpeedLv;
        float attackTempSpeed = 0.6f;

        //���÷����� 0.3�ʾ� ����
        attackSpeed = attackTempSpeed - (attackSpeedLv * 0.15f);
    }

    // ���ʹ� HP �� ��������
    private void EnemyHPBarUI_RealTimeUpdater()
    {
        hpBar_IMG.fillAmount = enemyCurHP / enemyMaxHP;

        // ���߿� ��ȯ �ʿ���!!@#@!##!@!@#
        hpBar_Text.text = $"{enemyCurHP}";
    }

    // ���Ŀ� ���� �Է��ؾ���
    private void atkPowerUpdater()
    {
        atkPower = 10;
    }




    //Ǯ���ý���
    private void Prefabs_Awake()
    {
        prefabsQue = new Queue<GameObject>[pooling_Obj.Length];

        for(int index =0; index < prefabsQue.Length; index++)
        {
            prefabsQue[index] = new Queue<GameObject>();
        }

        int count = 10;

        for(int index=0; index < count; index++)
        {
            GameObject obj = Instantiate(pooling_Obj[0], dmgFontParent);
            prefabsQue[0].Enqueue(obj);
            obj.transform.position = dmgFontParent.transform.position;
            obj.SetActive(false);
        }
    }

    public GameObject Get_Pooling_Prefabs(int indexNum)
    {
        if(prefabsQue[indexNum].Count <= 1)
        {
            GameObject obj = Instantiate(pooling_Obj[0], dmgFontParent);
            prefabsQue[0].Enqueue(obj);
            obj.transform.position = dmgFontParent.transform.position;
            obj.SetActive(false);
        }

        return prefabsQue[indexNum].Dequeue();

    }

    public void Set_Pooling_Prefabs(GameObject obj, int indexNum)
    {
        if (obj.activeSelf) 
        {
            obj.SetActive(false); 
        }

        prefabsQue[indexNum].Enqueue(obj);

    }


    // ī�޶� ����ũ
    public void F_PlayerOnHitCamShake()
    {
        StopCoroutine(ShakeCam());
        StartCoroutine(ShakeCam());
    }
    [SerializeField] float shakeTime;
    float shakeCount;
    IEnumerator ShakeCam()
    {
        camShake.m_AmplitudeGain = 2.5f;
        camShake.m_FrequencyGain = 0.15f;

        while (shakeCount < shakeTime)
        {
            shakeCount += Time.deltaTime;

            yield return null;
        }

        shakeCount = 0;

        while (camShake.m_AmplitudeGain > 0) // �ε巴�� ����ũ ����������
        {
            camShake.m_AmplitudeGain -= Time.deltaTime * 10;
            yield return null;
        }

        camShake.m_AmplitudeGain = 0;
        camShake.m_FrequencyGain = 0;

    }
}
