using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ActionManager : MonoBehaviour
{
    public static ActionManager inst;

    GameStatus statusManager;
    PetContollerManager petManager;

    //ī�޶�
    GameObject camsRef;
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin camShake;

    //���
    GameObject worldSpaceRef;

    Material mat;


    [Header("1. Input BG Tilling Speed <Color=yellow>( Float Data )</Color>")]
    [Space]
    [SerializeField] float backGroundSpeed;
    [Space]
    [Header("2. Insert Object Prefabs  <Color=#6699FF>( Prefabs )")]
    [Tooltip("�������Ʈ, �����װ��� ��Ƣ�� ����Ʈ")]
    [Space]
    [SerializeField] GameObject[] pooling_Obj;
    [Space]
    Transform dmgFontParent;
    Transform goldActionParent;


    Queue<GameObject>[] prefabsQue;


    //�÷��̾�
    Animator playerAnim;
    GameObject playerRef;
    int attackSpeedLv;
    string atkPower;
    SpriteRenderer palyerWeapenSr;
    Sprite[] weaponSprite;

    SpriteRenderer backGroundIMG;
    GameObject moveWindParticle;
    ParticleSystem playerDustPs;

    // ���ʹ�

    GameObject enemyObj;
    SpriteRenderer enemySr;
    Animator enemyAnim;
    GameObject[] hpBar_OutLine = new GameObject[2];

    GameObject hpBarRef;
    Image hpBar_IMG;
    Image hpBar_BackIMG;
    TMP_Text hpBar_Text;

    [Header("5. ReadOnly �� <Color=#CC3D3D>( Check Data )")]
    [Space]
    [SerializeField] string enemyCurHP;
    [SerializeField] string enemyMaxHP;
    [SerializeField] bool attackReady;
    [Space]
    //[Header("6. Insert Enemy List => <Color=#47C832>( Sprite File )")]
    //[Space]
    //[SerializeField] Sprite[] enemySprite;
    //[Space]

    //Ÿ�� ����Ʈ
    GameObject effectRef, playerAttackEffectRef, playerCriAttackEffectRef;

    ParticleSystem[] playerAtkEffect;
    ParticleSystem[] playerAtkCriEffect;
    ParticleSystem swordEffect;

    Transform enemy_StartPoint;
    Transform enemy_StopPoint;

    bool actionStart;
    bool isatk, ismove;
    public bool IsMove { get { return ismove; } }


    // �̵��ִϸ��̼�

    float checkPosition;
    UnityEngine.Vector2 enemyVec;
    float enemyPosX;
    [Header("7. Insert Value => <Color=yellow>( Float Data )")]
    [Space]
    [SerializeField] float enemySpawnSpeed;

    // ī�޶� ����ũ
    [SerializeField] float shakeTime;
    float shakeCount;

    int floorCount;

    //�ǹ� üũ
    bool isFever;
    public bool IsFever
    {
        get { return isFever; }
        set { isFever = value; }
    }


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

        worldSpaceRef = GameManager.inst.WorldSpaceRef;

        petManager = GetComponent<PetContollerManager>();
        effectRef = worldSpaceRef.transform.Find("Effect").gameObject;

        goldActionParent = worldSpaceRef.transform.Find("GoldActionDynamic").GetComponent<Transform>();

        backGroundIMG = worldSpaceRef.transform.Find("BackGround_IMG").GetComponent<SpriteRenderer>();
        mat = backGroundIMG.material;
        playerAnim = worldSpaceRef.transform.Find("Player_Obj").GetComponent<Animator>();
        playerRef = playerAnim.transform.Find("Player").gameObject;
        playerDustPs = playerRef.transform.Find("Dust").GetComponent<ParticleSystem>();

        moveWindParticle = playerRef.transform.Find("MoveWind").gameObject;

        palyerWeapenSr = playerRef.transform.Find("Weapon").GetComponent<SpriteRenderer>();
        swordEffect = playerRef.transform.Find("SwordEffect").GetComponent<ParticleSystem>();

        enemyObj = worldSpaceRef.transform.Find("Enemy").gameObject;
        enemySr = enemyObj.transform.Find("Sprite").GetComponent<SpriteRenderer>();

        enemyAnim = enemySr.GetComponent<Animator>();

        int atkEffectCount = effectRef.transform.Find("AtkEffect").childCount;
        playerAttackEffectRef = effectRef.transform.Find("AtkEffect").gameObject;
        playerCriAttackEffectRef = effectRef.transform.Find("CriEffect").gameObject;

        playerAtkEffect = new ParticleSystem[atkEffectCount];
        for (int index = 0; index < atkEffectCount; index++)
        {
            playerAtkEffect[index] = effectRef.transform.Find("AtkEffect").GetChild(index).GetComponent<ParticleSystem>();
        }

        int atkCriEffectCount = effectRef.transform.Find("CriEffect").childCount;
        playerAtkCriEffect = new ParticleSystem[atkCriEffectCount];

        for (int index = 0; index < atkCriEffectCount; index++)
        {
            playerAtkCriEffect[index] = effectRef.transform.Find("CriEffect").GetChild(index).GetComponent<ParticleSystem>();
        }


        dmgFontParent = worldSpaceRef.transform.Find("DmgFontCanvas/FontPosition").GetComponent<Transform>();

        hpBarRef = enemyObj.transform.Find("HPBar_Canvas").gameObject;
        hpBar_IMG = enemyObj.transform.Find("HPBar_Canvas/HP_Bar/Front").GetComponent<Image>();
        hpBar_BackIMG = enemyObj.transform.Find("HPBar_Canvas/HP_Bar/Back").GetComponent<Image>();
        hpBar_Text = enemyObj.transform.Find("HPBar_Canvas/HP_Bar/Text").GetComponent<TMP_Text>();

        hpBar_OutLine[0] = enemyObj.transform.Find("HPBar_Canvas/HP_Bar/NormalOutLine").gameObject;
        hpBar_OutLine[1] = enemyObj.transform.Find("HPBar_Canvas/HP_Bar/BossOutLine").gameObject;

        enemy_StartPoint = worldSpaceRef.transform.Find("SpawnPoint").GetComponent<Transform>();
        enemy_StopPoint = worldSpaceRef.transform.Find("StopPoint").GetComponent<Transform>();

        camsRef = GameObject.Find("---[Cams]").gameObject;
        cam = camsRef.transform.Find("Cam_0").GetComponent<CinemachineVirtualCamera>();
        camShake = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        weaponSprite = SpriteResource.inst.Weapons;
        Prefabs_Awake();
    }

    void Start()
    {
        //���� init
        Enemyinit();
    }

    UnityEngine.Vector2 matVec;
    bool doEnemyMove = true;
    private void FixedUpdate()
    {
        if (attackReady == false && IsFever == false) //��� �̵�
        {
            MoveMap();

            if (doEnemyMove == true)
            {
                EnemyMove();
            }
        }
    }

   
    float feverCountTimer = 0.6f;
    float feverNextTimer = 0f;
       

    void Update()
    {
        if (attackReady == true && IsFever == false) // ����
        {
            AttackEnemy();
        }

        if (IsFever == true) // �ǹ��Ͻ� �������� �÷���
        {
            feverNextTimer += Time.deltaTime;
            
            if (feverNextTimer > feverCountTimer)
            {
                feverNextTimer = 0;
                FeverFloorUp();
            }
        }

        BackHPBarUpdater();

#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.W))
        {
            WorldUI_Manager.inst.Get_ItemInfomation_UI_Active(SpriteResource.inst.CoinIMG(0), "��� +100");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //BuffContoller.inst.ActiveBuff(4, 3);
            LetterManager.inst.MakeLetter(0, "�̵���", "�׽�Ʈ (���)�����Դϴ�", 5000);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LetterManager.inst.MakeLetter(1, "����GM", "�׽�Ʈ (���)�����Դϴ�", 15000);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LetterManager.inst.MakeLetter(2, "�ڰ���", "�׽�Ʈ ���� (��)�Դϴ�", 2000);
        }

#endif
    }

    public void FeverTime_End()
    {
        attackReady = false;
        doEnemyMove = true;
    }

    // �����̱��� �ʱ�ȭ
    private void MoveMap()
    {
        if (ismove == false)
        {
            ismove = true;

            if (playerAnim.GetBool("Move") == false)
            {
                playerDustPs.gameObject.SetActive(true);
                petManager.PetAllParticle_Stop();
                playerAnim.SetBool("Move", true);
                petManager.PetAnimPlay(true);
                moveWindParticle.gameObject.SetActive(true);
                swordEffect.Stop();
            }

            palyerWeapenSr.enabled = false;
            enemyPosX = 0;
            enemyVec.x = enemy_StartPoint.transform.position.x;
            enemyVec.y = enemy_StartPoint.transform.position.y;
        }

        // ��� ������
        matVec.x += (Time.deltaTime * backGroundSpeed) * (1 + (GameStatus.inst.BuffAddSpeed + GameStatus.inst.NewbieMoveSpeedBuffValue));
        matVec.x = Mathf.Repeat(matVec.x, 1);
        mat.mainTextureOffset = matVec;

    }

    // ���� �Լ� 
    private void AttackEnemy()
    {
        if (playerAnim.GetBool("Move") == true) // ���� Animation On
        {
            playerDustPs.gameObject.SetActive(false);
            playerAnim.SetBool("Move", false);
            petManager.PetAnimPlay(false);
            playerAnim.transform.position = new UnityEngine.Vector2(-0.706f, 5.45f);
            moveWindParticle.gameObject.SetActive(false);
        }

        if (palyerWeapenSr.enabled == false) // ���� SpriteRenderer On
        {
            palyerWeapenSr.enabled = true;
        }
    }

    public float buffAddSpeed = 0f;
    private void EnemyMove()
    {
        //���ʹ� ���� �� �����ұ��� ����
        //checkPosition = Vector2.Distance(enemyObj.transform.position, enemy_StopPoint.position);
        //float spriteDistance = (enemySr.bounds.size.x / 2);

        checkPosition = enemyObj.transform.position.x - enemy_StopPoint.position.x;

        if (checkPosition > 1.4f)
        {
            enemyPosX += (Time.deltaTime * enemySpawnSpeed) * (1 + ((GameStatus.inst.BuffAddSpeed) + GameStatus.inst.NewbieMoveSpeedBuffValue));
            enemyVec.x -= enemyPosX;
            enemyObj.transform.position = enemyVec;
        }
        else if (checkPosition < 1.4f)
        {
            attackReady = true;

            enemyVec.x = 0;
            ismove = false;
        }
    }

    // ����ĳ���� �ִϸ��̼� ���� 
    public void A_PlayerAttackToEnemy()
    {
        if (IsFever) { StopCoroutine(PlayerOnHitDMG()); return; }
        StopCoroutine(PlayerOnHitDMG());
        StartCoroutine(PlayerOnHitDMG());
    }


    /// <summary>
    /// ����� �ִϸ��̼� ���� �Լ�
    /// </summary>
    /// <param name="CrewTypeIndex"> 0 ��ź��/ 1 ��ɼ��� </param>
    public void A_CrewAttackToEnemy(int CrewTypeIndex)
    {
        if(IsFever) { StopCoroutine(CrewAttackToEnemyDMG(CrewTypeIndex)); return; }
        StopCoroutine(CrewAttackToEnemyDMG(CrewTypeIndex));
        StartCoroutine(CrewAttackToEnemyDMG(CrewTypeIndex));
    }

    // ���� �Լ�!! //
    IEnumerator PlayerOnHitDMG() // < =
    {
        enemyAnim.SetTrigger("Hit");
        

        // ������� ����ī��Ʈ �� ���� 
        GameStatus.inst.NewbieAttackCountUp(true);
        string DMG = CalCulator.inst.Get_CurPlayerATK();
        swordEffect.Play();

        // ũ��Ƽ�� ���
        float randomDice = UnityEngine.Random.Range(0f, 100f);
        bool cri = false;

        //ũ��Ƽ�� ������ ķ��鸲
        if (randomDice < GameStatus.inst.CriticalChance)
        {
            F_PlayerOnHitCamShake();
            DMG = CalCulator.inst.PlayerCriDMGCalculator(DMG); // ġ��Ÿ ���ط� ���
            cri = true;
        }
        AudioManager.inst.Play_HitOnly(0, 0.4f, cri); // �����÷���

        string checkDMG = CalCulator.inst.BigIntigerMinus(enemyCurHP, DMG);/* DigidMinus(enemyCurHP, DMG, true);*/

        if (checkDMG != "0" && attackReady == true)
        {
            AudioManager.inst.MonsterHitOnly(true,1f);
            // �������Ʈ
            GameObject obj = Get_Pooling_Prefabs(0);
            obj.transform.position = enemySr.bounds.center;
            //obj.transform.position = dmgFontParent.position;
            obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitAddFloatChanger(DMG), randomDice < GameStatus.inst.CriticalChance ? true : false, 2);
            obj.SetActive(true);

            enemyCurHP = CalCulator.inst.BigIntigerMinus(enemyCurHP, DMG);
            EnemyHPBar_FillAmount_Init();
            EnemyOnHitEffect(cri);
        }
        else if (checkDMG == "0")//���ʹ� ��� �� �ʱ�ȭ
        {
            AudioManager.inst.MonsterHitOnly(false, 1f); //

            DogamManager.inst.MosterDogamIndexValueUP(curEnemyNumber[0], curEnemyNumber[1]); // ���� �������� ���
            StartCoroutine(GetGoldActionParticle()); // ��� ȹ���ϴ� ��ƼŬ ���

            // ���� �޾ƾߵǴ� �� ���
            string getGold = Get_EnemyDeadGold();
            GameStatus.inst.PlusGold(getGold); // ��� ���
            EnemyDeadFloorUp();
            GameStatus.inst.NewbieAttackCountUp(false); // ������� ����ī��Ʈ0

            // ���� ���
            GameStatus.inst.TotalEnemyKill++;
        }

        // ��Ÿ ������ ������ 0���� �ʱ�ȭ
        PetContollerManager.inst.AttackBuffDisable();


        yield return null;
    }


    // ���� ���� �ڷ�ƾ
    /// <summary>
    /// 
    /// </summary>
    /// <param name="CrewType"> 0 ��ź�� / 1 ��ɼ���</param>
    /// <returns></returns>
    IEnumerator CrewAttackToEnemyDMG(int CrewType)
    {
        enemyAnim.SetTrigger("Hit");
        //PlayerInit();
        // ������� ����ī��Ʈ �� ���� 

        //string DMG = CalCulator.inst.Get_CurPlayerATK();
        string CrewATK = string.Empty;
        string MinusValue = string.Empty;

        if (CrewType == 0) // ��ź�� ( ��ü�¿��� ���ݷ��� ���� )
        {
            CrewATK = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.TotalAtk.ToString(), GameStatus.inst.Pet0_Lv + 2); // ���۽� 3�� ����
            MinusValue = CalCulator.inst.BigIntigerMinus(enemyCurHP, CrewATK);
        }
        else if (CrewType == 1) // ��ɼ���
        {
            CrewATK = CalCulator.inst.CrewNumber2AtkCalculator(enemyCurHP);

            if (CrewATK == null) // ���Ͱ� ���߿� �׾��ٸ� ����
            {
                yield break;
            }

            MinusValue = CalCulator.inst.BigIntigerMinus(enemyCurHP, CrewATK);
        }


        if (MinusValue != "0" && attackReady == true)
        {
            enemyCurHP = MinusValue;
            EnemyHPBar_FillAmount_Init();

            // �������Ʈ
            GameObject obj = Get_Pooling_Prefabs(0);
            obj.transform.position = enemySr.bounds.center;

            if (CrewType == 0)
            {
                obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitAddFloatChanger(CrewATK), false, 0);
            }
            else if (CrewType == 1)
            {
                obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitAddFloatChanger(CrewATK), false, 1);
            }

            obj.SetActive(true);
        }
        else if (MinusValue == "0")//���ʹ� ��� �� �ʱ�ȭ
        {

            DogamManager.inst.MosterDogamIndexValueUP(curEnemyNumber[0], curEnemyNumber[1]); // ���� �������� ���
            StartCoroutine(GetGoldActionParticle());

            // ���� �޾ƾߵǴ� �� ���
            string getGold = Get_EnemyDeadGold();
            GameStatus.inst.PlusGold(getGold);

            EnemyDeadFloorUp();
            GameStatus.inst.NewbieAttackCountUp(false); // ������� ����ī��Ʈ0

            // ���� ���
            GameStatus.inst.TotalEnemyKill++;
        }


        yield return null;

    }
    // ���� �׾����� ���� �� Ƣ�� ��ƼŬ ��� 
    IEnumerator GetGoldActionParticle()
    {
        GameObject ps = Get_Pooling_Prefabs(1);
        ps.transform.position = enemyAnim.transform.position + (UnityEngine.Vector3.up * 0.5f);
        ps.SetActive(true);
        ps.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1);
        Return_Pooling_Prefabs(ps, 1);
    }

    /// <summary>
    /// ���� ��� Init
    /// </summary>
    private void EnemyDeadFloorUp()
    {
        attackReady = false;

        if (GameStatus.inst.FloorLv < 5)
        {
            GameStatus.inst.FloorLv++;
            Enemyinit();
        }
        else if (GameStatus.inst.FloorLv == 5)
        {
            doEnemyMove = false;
            GameStatus.inst.FloorLv++;
            StartCoroutine(NextStageAction()); //  ���� ������ �̵��ϴ°�ó��
        }
    }


    /// <summary>
    /// �ǹ��� ���
    /// </summary>
    private void FeverFloorUp()
    {
        floorCount++;
        // ���� �޾ƾߵǴ� �� ���
        string getGold = Get_EnemyDeadGold();
        GameStatus.inst.PlusGold(getGold); // ��� ���

        if (GameStatus.inst.FloorLv < 5)
        {
            GameStatus.inst.FloorLv++;
            Enemyinit();
        }
        else if (GameStatus.inst.FloorLv == 5)
        {
            GameStatus.inst.FloorLv++;
            Enemyinit();
            WorldUI_Manager.inst.Reset_StageUiBar();
        }
    }

    IEnumerator NextStageAction()
    {
        //��ġ �ȱ���
        enemyObj.transform.position = enemy_StartPoint.position;

        playerAnim.SetTrigger("Out");

        yield return new WaitForSeconds(0.2f);

        WorldUI_Manager.inst.Set_Auto_BlackCutton(1);
        yield return new WaitForSeconds(1);

        // �ʺ���
        MapChanger();
        Enemyinit();
        doEnemyMove = true;

        yield return new WaitForSeconds(0.5f);


        WorldUI_Manager.inst.Reset_StageUiBar();
        playerAnim.transform.localPosition = new UnityEngine.Vector3(-5, 0, 0);
        yield return null;
        playerAnim.SetTrigger("In");

        yield return new WaitForSeconds(1f);
    }


    int effectIndexCount = 0;
    int effectCriIndexCount = 0;

    //���ʹ� �ǰ� ����Ʈ �Լ�
    UnityEngine.Vector3 particleEffectPo = UnityEngine.Vector3.zero;

    private void EnemyOnHitEffect(bool cri)
    {
        particleEffectPo = Get_CurEnemyCenterPosition();

        if (cri == false)
        {
            if (effectIndexCount == playerAtkEffect.Length - 1)
            {
                effectIndexCount = 0;
            }


            playerAttackEffectRef.transform.position = particleEffectPo;
            playerAtkEffect[effectIndexCount].Play();
            effectIndexCount++;
        }
        else
        {
            if (effectCriIndexCount == playerAtkCriEffect.Length - 1)
            {
                effectCriIndexCount = 0;
            }
            playerCriAttackEffectRef.transform.position = particleEffectPo;
            playerAtkCriEffect[effectCriIndexCount].Play();
            effectCriIndexCount++;
        }


    }
    bool isUIActive;



    // ���� �ʱ�ȭ
    UnityEngine.Vector3 hpbarPos;

    //float hpBar_downX = 0.1f;
    float hpBar_downY = 0f;
    private void Enemyinit()
    {
        // ���߿� ü�� �ʱ�ȭ ���� �ٲ����
        swordEffect.Stop();
        enemyObj.transform.position = enemy_StartPoint.position; // ��ġ �ʱ�ȭ

        //Hpbar �ʱ�ȭ
        hpBar_IMG.fillAmount = 1;
        hpBar_Text.text = string.Empty;

        Set_EnemyBossOrNormal();
    }

    [SerializeField]
    [Tooltip("Stage, Enemy Number")] int[] curEnemyNumber = new int[2];
    [SerializeField]
    Sprite[] curStageEnemy;
    UnityEngine.Vector3 bossSpriteSize = new UnityEngine.Vector3(1.5f, 1.5f, 1);
    /// <summary>
    /// ���ʹ� ���� ����
    /// </summary>
    /// <param name="isBoss"></param>
    private void Set_EnemyBossOrNormal()
    {
        int bossHPMultiPlyer = 1;
        
        //���� ���������� ���� ��������Ʈ ������
        switch (backGroundIMG.sprite.name)
        {
            case "1_Stage":
                curEnemyNumber[0] = 1;
                bossHPMultiPlyer = 2;
                break;

            case "2_Stage":
                curEnemyNumber[0] = 2;
                bossHPMultiPlyer = 2;
                break;

            case "3_Stage":
                curEnemyNumber[0] = 3;
                bossHPMultiPlyer = 3;
                break;
        }
               
        curStageEnemy = SpriteResource.inst.enemySprite(curEnemyNumber[0]);

        //���� ���� ��ȣ ����
        curEnemyNumber[1] = UnityEngine.Random.Range(0, curStageEnemy.Length);
        enemySr.sprite = curStageEnemy[curEnemyNumber[1]];

        // ü���ʱ�ȭ
        enemyMaxHP = CalCulator.inst.EnemyHpSetup();

        if (GameStatus.inst.FloorLv != 5) // �Ϲݸ�
        {
            enemySr.transform.localScale = UnityEngine.Vector3.one;

            //���� HP�� �ƿ�����
            hpBar_OutLine[0].SetActive(true);
            hpBar_OutLine[1].SetActive(false);
        }
        else if (GameStatus.inst.FloorLv == 5) // ������
        {
            //����ü�� 2��
            enemyMaxHP = CalCulator.inst.StringAndIntMultiPly(enemyMaxHP, bossHPMultiPlyer);
            enemySr.transform.localScale = bossSpriteSize;
            //���� HP�� �ƿ�����
            hpBar_OutLine[0].SetActive(false);
            hpBar_OutLine[1].SetActive(true);
        }

        // HPBar ��ġ �ʱ�ȭ
        hpbarPos = enemyObj.transform.position;
        //hpbarPos.x -= hpBar_downX;
        hpbarPos.y -= hpBar_downY;
        hpBarRef.transform.position = hpbarPos;

        enemyCurHP = enemyMaxHP;
        EnemyHPBar_FillAmount_Init(); // ü��
    }





    // ���� ü�¹� ��������
    private void EnemyHPBar_FillAmount_Init()
    {
        hpBar_IMG.fillAmount = CalCulator.inst.StringAndStringDivideReturnFloat(enemyCurHP, enemyMaxHP, 3);
        hpBar_Text.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(enemyCurHP)}";
    }

    // ü�� �Ĺ�� 
    float backHpBarFillSpeed_MultiFlyer = 0.4f;
    private void BackHPBarUpdater()
    {
        if (hpBar_BackIMG.fillAmount > hpBar_IMG.fillAmount)
        {
            hpBar_BackIMG.fillAmount -= Time.deltaTime * backHpBarFillSpeed_MultiFlyer;
        }
        else if (hpBar_BackIMG.fillAmount <= hpBar_IMG.fillAmount)
        {
            hpBar_BackIMG.fillAmount = hpBar_IMG.fillAmount;
        }
    }

    ////////////////////////////// [ �÷��̾� �ӵ����� ���� ] /////////////////////////////////////////



    //�÷��̾� ���ݼӵ� ���� �Լ�
    public void PlayerAttackSpeedLvUp()
    {

        int multypleValue = GameStatus.inst.GetAryRelicLv(7); // ���� ����
        float defaultSpeed = GameStatus.inst.RelicDefaultvalue(7); // 1%
        float newbieBuffSpeed = GameStatus.inst.NewbieAttackSpeed; // 20%

        if (playerAnim != null)
        {
            if(multypleValue > 350)
            {
                multypleValue = 350;
            }

            playerAnim.SetFloat("AttackSpeed", 1 + ((defaultSpeed * multypleValue)) + newbieBuffSpeed);
        }

    }


    // �÷��̾� ������ �ӵ�����
    public void SetPlayerMoveSpeed()
    {
        float speed = 1 + GameStatus.inst.BuffAddSpeed + GameStatus.inst.NewbieMoveSpeedBuffValue;
        //Debug.Log($"��Ż {speed} /����{GameStatus.inst.BuffAddSpeed} / ���� {GameStatus.inst.NewbieMoveSpeedBuffValue}");
        playerAnim.SetFloat("MoveSpeed", speed);
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

        for (int forCount = 0; forCount < count; forCount++)
        {
            GameObject obj = Instantiate(pooling_Obj[0], dmgFontParent);
            prefabsQue[0].Enqueue(obj);
            obj.transform.position = dmgFontParent.transform.position;
            obj.SetActive(false);
        }

        for (int forCount = 0; forCount < 3; forCount++)
        {
            GameObject obj = Instantiate(pooling_Obj[1], goldActionParent);
            prefabsQue[1].Enqueue(obj);
            obj.transform.position = dmgFontParent.transform.position;
            obj.SetActive(false);
        }
    }

    Transform trsPrent;
    public GameObject Get_Pooling_Prefabs(int indexNum)
    {

        if (prefabsQue[indexNum].Count <= 1)
        {

            switch (indexNum)
            {
                case 0:
                    trsPrent = dmgFontParent;
                    break;

                case 1:
                    trsPrent = goldActionParent;
                    break;
            }

            GameObject obj = Instantiate(pooling_Obj[indexNum], trsPrent);
            prefabsQue[indexNum].Enqueue(obj);
            obj.transform.position = dmgFontParent.transform.position;
            obj.SetActive(false);
        }

        return prefabsQue[indexNum].Dequeue();

    }

    public void Return_Pooling_Prefabs(GameObject obj, int indexNum)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }

        prefabsQue[indexNum].Enqueue(obj);

    }

    int curMapNumber = 0;
    private void MapChanger()
    {
        curMapNumber++;
        curMapNumber = (int)Mathf.Repeat(curMapNumber, 3);
        backGroundIMG.sprite = SpriteResource.inst.Map(curMapNumber);
    }


    // ī�޶� ����ũ
    public void F_PlayerOnHitCamShake()
    {
        if (GameManager.inst.MiniGameMode == true) { return; }
        StopCoroutine(ShakeCam());
        StartCoroutine(ShakeCam());
    }



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

    // ���� ��������Ʈ ���� �Լ�
    public void Set_WeaponSprite_Changer(int index)
    {
        palyerWeapenSr.sprite = weaponSprite[index];
    }



    // ���� �װ� ��� ���
    float defaultKillGoldValue = 0.5f;
    int relicMultiplyer = 0;
    BigInteger curgold = new BigInteger();
    public string Get_EnemyDeadGold()
    {
        curgold = BigInteger.Parse(GameStatus.inst.GetTotalGold());
        curgold = CalCulator.inst.MultiplyBigIntegerAndfloat(curgold, defaultKillGoldValue);
        string normalValue = curgold.ToString();

        // �� ó����� ����
        if (GameStatus.inst.GetAryRelicLv(9) != 0)
        {
            relicMultiplyer = GameStatus.inst.GetAryRelicLv(9);
            normalValue = CalCulator.inst.DigitAndIntPercentMultiply(normalValue, relicMultiplyer);
        }

        return normalValue;
    }


    // �÷��̾� ���̶�Ű ������Ʈ ����
    public GameObject ReturnPlayerObjInHierachy() => playerAnim.gameObject;
    public GameObject ReturnEnemyObjInHierachy() => enemyObj;

    /// <summary>
    /// ���� ��������Ʈ ���������Լ�
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Sprite Get_WeaponSprite(int index) => weaponSprite[index];

    /// <summary>
    /// ���� ���� ��������Ʈ���� ���� ���� ������
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public UnityEngine.Vector3 Get_CurEnemyCenterPosition() => enemySr.bounds.center;


}
