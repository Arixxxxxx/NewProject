using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;


public class ActionManager : MonoBehaviour
{
    public static ActionManager inst;

    GameStatus statusManager;

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
    SpriteRenderer palyerWeapenSr;
    [SerializeField] Sprite[] weaponSprite;
    [SerializeField] Sprite[] backGroudSprite;
    SpriteRenderer backGroundIMG;
    GameObject moveWindParticle;


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
    int floorCount;


    private void Awake()
    {
        worldSpaceGroup = GameObject.Find("---[World Space]").gameObject;

        backGroundIMG = worldSpaceGroup.transform.Find("BackGround_IMG").GetComponent<SpriteRenderer>();
        mat = backGroundIMG.material;
        playerAnim = worldSpaceGroup.transform.Find("Player_Obj/Sprite").GetComponent<Animator>();
       
        moveWindParticle = playerAnim.transform.Find("MoveWind").gameObject;
        palyerWeapenSr = playerAnim.transform.Find("Weapon").GetComponent<SpriteRenderer>();

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
        UI_Init();
    }

    Vector2 matVec;
    bool doEnemyMove = true;
    private void FixedUpdate()
    {
        if (attackReady == false) //��� �̵�
        {
            MoveMap();

            if(doEnemyMove == true)
            {
                EnemyMove();
            }
        }
    }

    int index = 1;
    float attackS = 0.15f;
    void Update()
    {
        if (attackReady == true) // ����
        {
            AttackEnemy();
        }

        EnemyHPBarUI_RealTimeUpdater();
        atkPowerUpdater();


        if(Input.GetKeyDown(KeyCode.Q))
        {
            playerAnim.SetTrigger("Out");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetTrigger("In");
        }
    }


    private void UI_Init()
    {
        // UI Bar �ʱ�ȭ
        statusManager = GameStatus.inst;
        if (statusManager.FloorLv != 0)
        {
            WorldUI_Manager.inst.Set_StageUiBar(statusManager.FloorLv);
        }
        else if (statusManager.FloorLv == 0)
        {
            WorldUI_Manager.inst.Reset_StageUiBar();
        }

    }


    private void MoveMap()
    {
        // �����̱��� �ʱ�ȭ
        if (ismove == false)
        {
            ismove = true;
            if (playerAnim.GetBool("Move") == false)
            {
                playerAnim.SetBool("Move", true);
                moveWindParticle.gameObject.SetActive(true);
            }

            palyerWeapenSr.enabled = false;
            enemyPosX = 0;
            enemyVec.x = enemy_StartPoint.transform.position.x;
            enemyVec.y = enemy_StartPoint.transform.position.y;
        }

        // ��� ������
        matVec.x += Time.deltaTime * backGroundSpeed;
        matVec.x = Mathf.Repeat(matVec.x, 1);
        mat.mainTextureOffset = matVec;
    
    }

    private void EnemyMove()
    {
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
            attackReady = true;

            enemyVec.x = 0;
            ismove = false;
        }
    }

    //�ִϸ��̼� ���� �Լ�
    public void A_PlayerAttackToEnemy()
    {
        StopCoroutine(enemyOnHit());
        StartCoroutine(enemyOnHit());
    }

    float count = 0;
    private void AttackEnemy()
    {
     
        if (playerAnim.GetBool("Move") == true) // ���� Animation On
        {
            
            playerAnim.SetBool("Move", false);
            playerAnim.transform.position = new Vector2(-0.706f, 5.45f);
            moveWindParticle.gameObject.SetActive(false);
        }

        if (palyerWeapenSr.enabled == false) // ���� SpriteRenderer On
        {
            palyerWeapenSr.enabled = true;
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
        else if (enemyCurHP - atkPower <= 0) //���ʹ� ��� �� �ʱ�ȭ
        {
            EnemyDeadFloorUp();
        }

    }

    /// <summary>
    /// ���� ��� Init �Լ�
    /// </summary>
    private void EnemyDeadFloorUp()
    {
        attackReady = false;


        floorCount++;

        if (floorCount < 4)
        {
            Enemyinit();
            GameStatus.inst.FloorLv = floorCount;
            WorldUI_Manager.inst.Set_StageUiBar(floorCount);
        }
        else if (floorCount == 4)
        {
             Enemyinit();
            GameStatus.inst.FloorLv = floorCount;
            WorldUI_Manager.inst.Set_StageUiBar(floorCount);
            //�������� �÷��ִ� ~
        }
        else if (floorCount == 5)
        {
            doEnemyMove = false;
            Enemyinit();
            floorCount = 0;
            StartCoroutine(NextStageAction()); //  ���� ������ �̵��ϴ°�ó��


            
        }
    }

   
    IEnumerator NextStageAction()
    {
        GameStatus.inst.StageLv++;
        GameStatus.inst.FloorLv = floorCount;

        playerAnim.SetTrigger("Out");
       
        yield return new WaitForSeconds(0.2f);

        WorldUI_Manager.inst.Set_Auto_BlackCutton(1);
        yield return new WaitForSeconds(1);

        // �ʺ���
        TestMapChanger();
        doEnemyMove = true;

        yield return new WaitForSeconds(0.5f);

        
        WorldUI_Manager.inst.Reset_StageUiBar();
        playerAnim.transform.localPosition = new Vector3(-5,0,0);
        yield return null;
        playerAnim.SetTrigger("In");

        yield return new WaitForSeconds(1f);
       
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
        enemyMaxHP = /*GameStatus.inst.StageLv **/ 50; // ü���ʱ�ȭ
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


    public void PlayerAttackSpeedLvUp(int Lv)
    {
        if (GameStatus.inst.AtkSpeedLv >= 10) { return; }

        playerAnim.SetFloat("AttackSpeed", 1 + (attackS * Lv));
        Debug.Log($"Total : {1 + (attackS * Lv)} / AddSpeed {attackS} + {Lv}");
    }


    //Ǯ���ý���
    private void Prefabs_Awake()
    {
        prefabsQue = new Queue<GameObject>[pooling_Obj.Length];

        for (int index = 0; index < prefabsQue.Length; index++)
        {
            prefabsQue[index] = new Queue<GameObject>();
        }

        int count = 10;

        for (int index = 0; index < count; index++)
        {
            GameObject obj = Instantiate(pooling_Obj[0], dmgFontParent);
            prefabsQue[0].Enqueue(obj);
            obj.transform.position = dmgFontParent.transform.position;
            obj.SetActive(false);
        }
    }

    public GameObject Get_Pooling_Prefabs(int indexNum)
    {
        if (prefabsQue[indexNum].Count <= 1)
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

    int mapint = 0;
    private void TestMapChanger()
    {
        mapint++;
        mapint = mapint == 2 ? 0 : 1;
        backGroundIMG.sprite = backGroudSprite[mapint];
    }
    private void Set_MapSpriteChanger(int indexNum)
    {
        backGroundIMG.sprite = backGroudSprite[indexNum];
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


    public void TestBtnWeaponChange()
    {
        index++;

        if (index >= weaponSprite.Length)
        {
            index = 0;
        }
        
        palyerWeapenSr.sprite = weaponSprite[index];

    }

  
}