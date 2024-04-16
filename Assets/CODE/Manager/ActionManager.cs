using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ActionManager : MonoBehaviour
{
    public static ActionManager inst;

    GameStatus statusManager;
    PetContollerManager petManager;

    //ī�޶�
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin camShake;

    //���
    GameObject worldSpaceRef;

    Material mat;
    [SerializeField] float backGroundSpeed;
    [SerializeField] GameObject[] pooling_Obj;
    Transform dmgFontParent;
    Transform goldActionParent;


    Queue<GameObject>[] prefabsQue;


    //�÷��̾�
    Animator playerAnim;
    GameObject playerRef;
    int attackSpeedLv;
    float attackSpeed;
    string atkPower;
    SpriteRenderer palyerWeapenSr;
    [SerializeField] Sprite[] weaponSprite;
    [SerializeField] Sprite[] backGroudSprite;
    SpriteRenderer backGroundIMG;
    GameObject moveWindParticle;
    ParticleSystem playerDustPs;

    // ���ʹ�
    GameObject enemyObj;
    SpriteRenderer enemySr;
    Animator enemyAnim;
    Image hpBar_IMG;
    TMP_Text hpBar_Text;
    int curEnemyNum;
    [SerializeField] string enemyCurHP;
    [SerializeField] string enemyMaxHP;
    [SerializeField] Sprite[] enemySprite;

    //Ÿ�� ����Ʈ
    GameObject effectRef;

    [SerializeField]
    ParticleSystem[] playerAtkEffect;
    [SerializeField]
    ParticleSystem[] playerAtkCriEffect;
    ParticleSystem swordEffect;

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
        hpBar_IMG = enemyObj.transform.Find("HPBar_Canvas/HP_Bar/Front").GetComponent<Image>();
        hpBar_Text = enemyObj.transform.Find("HPBar_Canvas/HP_Bar/Text").GetComponent<TMP_Text>();

        enemy_StartPoint = worldSpaceRef.transform.Find("SpawnPoint").GetComponent<Transform>();
        enemy_StopPoint = worldSpaceRef.transform.Find("StopPoint").GetComponent<Transform>();

        cam = GameObject.Find("---[Cams]/Cam_0").GetComponent<CinemachineVirtualCamera>();
        camShake = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Prefabs_Awake();
    }

    void Start()
    {
        //���� init
        Enemyinit();
        UI_Init();
        
        
    }

    Vector2 matVec;
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

    int index = 1;

    float feverCountTimer = 0.8f;
    float feverNextTimer;
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

        //�׽�Ʈ��
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameStatus.inst.GetGiftDay[0] -= 1;
            GameStatus.inst.GetNewbieGiftDay[0] -= 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //BuffContoller.inst.ActiveBuff(4, 3);
            LetterManager.inst.MakeLetter(0, "�̵���", "�׽�Ʈ (���)�����Դϴ�", 5000);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LetterManager.inst.MakeLetter(1, "����GM", "�׽�Ʈ (���)�����Դϴ�", 150);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LetterManager.inst.MakeLetter(2, "�ڰ���", "�׽�Ʈ ���� (��)�Դϴ�", 200);
        }
    }

    // UI Bar �ʱ�ȭ
    private void UI_Init()
    {

        statusManager = GameStatus.inst;
        if (statusManager.FloorLv > 1)
        {
            WorldUI_Manager.inst.Set_StageUiBar(statusManager.FloorLv);
        }
        else if (statusManager.FloorLv == 1)
        {
            WorldUI_Manager.inst.Reset_StageUiBar();
        }

        
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
            playerAnim.transform.position = new Vector2(-0.706f, 5.45f);
            moveWindParticle.gameObject.SetActive(false);
        }

        if (palyerWeapenSr.enabled == false) // ���� SpriteRenderer On
        {
            palyerWeapenSr.enabled = true;
        }
    }


    private void EnemyMove()
    {
        //���ʹ� ���� �� �����ұ��� ����
        //checkPosition = Vector2.Distance(enemyObj.transform.position, enemy_StopPoint.position);
        //float spriteDistance = (enemySr.bounds.size.x / 2);

        checkPosition = enemyObj.transform.position.x - enemy_StopPoint.position.x;

        if (checkPosition > 1.35f) // �Ÿ��� üũ 2�̻� �̵�
        {
            enemyPosX += (Time.deltaTime * enemySpawnSpeed) * (1 + (GameStatus.inst.BuffAddSpeed + GameStatus.inst.NewbieMoveSpeedBuffValue));
            enemyVec.x -= enemyPosX;
            enemyObj.transform.position = enemyVec;
        }
        else if (checkPosition < 1.35f) // 2�̸� ����
        {
            attackReady = true;

            enemyVec.x = 0;
            ismove = false;
        }
    }

    // ����ĳ���� �ִϸ��̼� ���� 
    public void A_PlayerAttackToEnemy()
    {
        StopCoroutine(PlayerOnHitDMG());
        StartCoroutine(PlayerOnHitDMG());
    }


    /// <summary>
    /// ����� �ִϸ��̼� ���� �Լ�
    /// </summary>
    /// <param name="CrewTypeIndex"> 0 ��ź��/ 1 ��ɼ��� </param>
    public void A_CrewAttackToEnemy(int CrewTypeIndex)
    {
        StopCoroutine(CrewAttackToEnemyDMG(CrewTypeIndex));
        StartCoroutine(CrewAttackToEnemyDMG(CrewTypeIndex));
    }

    // ���� �Լ�!! //
    IEnumerator PlayerOnHitDMG() // < =
    {
        enemyAnim.SetTrigger("Hit");
        PlayerInit();

        // ������� ����ī��Ʈ �� ���� 
        GameStatus.inst.NewbieAttackCountUp(true);
        if (GameStatus.inst.IsNewBie)
        {
            atkPower = CalCulator.inst.StringAndIntMultiPly(atkPower, GameStatus.inst.Get_NewBieAttackBuff_MultiplyValue());
        }

        string DMG = atkPower;
        swordEffect.Play();

        // ũ��Ƽ�� ���
        float randomDice = Random.Range(0f, 100f);
        bool cri = false;

        //ũ��Ƽ�� ������ ķ��鸲
        if (randomDice < GameStatus.inst.CriticalChance)
        {
            F_PlayerOnHitCamShake();
            DMG = CalCulator.inst.PlayerCriDMGCalculator(DMG); // ġ��Ÿ ���ط� ���
            cri = true;
        }

        string checkDMG = CalCulator.inst.DigidMinus(enemyCurHP, DMG, true);

        if (checkDMG != "Dead" && attackReady == true)
        {
            // �������Ʈ
            GameObject obj = Get_Pooling_Prefabs(0);
            obj.transform.position = dmgFontParent.position;
            obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitAddFloatChanger(DMG), randomDice < GameStatus.inst.CriticalChance ? true : false, 2);
            obj.SetActive(true);

            enemyCurHP = CalCulator.inst.DigidMinus(enemyCurHP, DMG, true);
            EnemyHPBarUI_Updater();
            EnemyOnHitEffect(cri);
        }
        else if (checkDMG == "Dead")//���ʹ� ��� �� �ʱ�ȭ
        {
            DogamManager.inst.MosterDogamIndexValueUP(curEnemyNum); // ���� �������� ���
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
        PlayerInit();

        // ������� ����ī��Ʈ �� ���� 
        GameStatus.inst.NewbieAttackCountUp(true);
        if (GameStatus.inst.IsNewBie)
        {
            atkPower = CalCulator.inst.StringAndIntMultiPly(atkPower, GameStatus.inst.Get_NewBieAttackBuff_MultiplyValue());
        }

        string DMG = atkPower;
        string CrewATK = string.Empty;
        string MinusValue = string.Empty;

        if (CrewType == 0) // ��ź�� ( ��ü�¿��� ���ݷ��� ���� )
        {
            CrewATK = CalCulator.inst.StringAndIntMultiPly(DMG, GameStatus.inst.Pet0_Lv + 1);
            MinusValue = CalCulator.inst.DigidMinus(enemyCurHP, CrewATK, true);
        }
        else if (CrewType == 1) // ��ɼ���
        {
            CrewATK = CalCulator.inst.CrewNumber2AtkCalculator(enemyCurHP);

            if (CrewATK == null) // ���Ͱ� ���߿� �׾��ٸ� ����
            {
                yield break;
            }

            MinusValue = CalCulator.inst.DigidMinus(enemyCurHP, CrewATK, true);
        }
        

        if (MinusValue != "Dead" && attackReady == true)
        {
            enemyCurHP = MinusValue;
            EnemyHPBarUI_Updater();

            // �������Ʈ
            GameObject obj = Get_Pooling_Prefabs(0);
            obj.transform.position = dmgFontParent.position;

            if(CrewType == 0)
            {
                obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitAddFloatChanger(CrewATK), false, 0); // ������ ����� ��Ʈ
            }
            else if(CrewType == 1)
            {
                obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitAddFloatChanger(CrewATK), false, 1); // �þ�� �������Ʈ
            }
            obj.SetActive(true);
        }
        else if (MinusValue == "Dead")//���ʹ� ��� �� �ʱ�ȭ
        {
            
            DogamManager.inst.MosterDogamIndexValueUP(curEnemyNum); // ���� �������� ���
            StartCoroutine(GetGoldActionParticle());
            // ���� �޾ƾߵǴ� �� ���
            string getGold = Get_EnemyDeadGold();
            WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(0, getGold);
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
        ps.transform.position = enemyAnim.transform.position + (Vector3.up * 0.5f);
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


        floorCount++;

        if (floorCount < 4)
        {
            Enemyinit();
            GameStatus.inst.FloorLv++;
            WorldUI_Manager.inst.Set_StageUiBar(floorCount);
        }
        else if (floorCount == 4)
        {
            Enemyinit();
            GameStatus.inst.FloorLv++;
            WorldUI_Manager.inst.Set_StageUiBar(floorCount);
            //�������� �÷��ִ� ~
        }
        else if (floorCount == 5)
        {
            doEnemyMove = false;
            Enemyinit();
            floorCount = 0;
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

        if (floorCount < 4)
        {
            Enemyinit();
            GameStatus.inst.FloorLv++;
            WorldUI_Manager.inst.Set_StageUiBar(floorCount);

        }
        else if (floorCount == 4)
        {
            Enemyinit();
            GameStatus.inst.FloorLv++;
            WorldUI_Manager.inst.Set_StageUiBar(floorCount);
            //�������� �÷��ִ� ~
        }
        else if (floorCount == 5)
        {
            Enemyinit();
            floorCount = 0;
            GameStatus.inst.FloorLv++;
            WorldUI_Manager.inst.Reset_StageUiBar();
        }
    }

    IEnumerator NextStageAction()
    {

        playerAnim.SetTrigger("Out");

        yield return new WaitForSeconds(0.2f);

        WorldUI_Manager.inst.Set_Auto_BlackCutton(1);
        yield return new WaitForSeconds(1);

        // �ʺ���
        TestMapChanger();
        doEnemyMove = true;

        yield return new WaitForSeconds(0.5f);


        WorldUI_Manager.inst.Reset_StageUiBar();
        playerAnim.transform.localPosition = new Vector3(-5, 0, 0);
        yield return null;
        playerAnim.SetTrigger("In");

        yield return new WaitForSeconds(1f);
    }


    int effectIndexCount = 0;
    int effectCriIndexCount = 0;

    //���ʹ� �ǰ� ����Ʈ �Լ�
    private void EnemyOnHitEffect(bool cri)
    {
        if (cri == false)
        {
            if (effectIndexCount == playerAtkEffect.Length - 1)
            {
                effectIndexCount = 0;
            }

            playerAtkEffect[effectIndexCount].Play();
            effectIndexCount++;
        }
        else
        {
            if (effectCriIndexCount == playerAtkCriEffect.Length - 1)
            {
                effectCriIndexCount = 0;
            }

            playerAtkCriEffect[effectCriIndexCount].Play();
            effectCriIndexCount++;
        }


    }
    bool isUIActive;



    // ���� �ʱ�ȭ
    private void Enemyinit()
    {
        // ���߿� ü�� �ʱ�ȭ ���� �ٲ����
        swordEffect.Stop();
        enemyObj.transform.position = enemy_StartPoint.position; // ��ġ �ʱ�ȭ

        enemyMaxHP = CalCulator.inst.EnemyHpSetup();
        enemyCurHP = enemyMaxHP;

        //Hpbar �ʱ�ȭ
        hpBar_IMG.fillAmount = 1;
        hpBar_Text.text = string.Empty;
        EnemyHPBarUI_Updater(); // ü��

        //��������Ʈ �� �Ҵ�
        int spriteCount = enemySprite.Length;
        curEnemyNum = Random.Range(0, spriteCount);
        enemySr.sprite = enemySprite[curEnemyNum];
   
    }


    // �÷��̾� �ʱ�ȭ
    private void PlayerInit()
    {
        atkPower = CalCulator.inst.Get_CurPlayerATK();
        attackSpeedLv = GameStatus.inst.AtkSpeedLv;
        float attackTempSpeed = 0.6f;

        //���÷����� 0.15�ʾ� ����
        attackSpeed = attackTempSpeed - (attackSpeedLv * 0.15f);
    }

    // ���ʹ� HP �� ��������

    private void EnemyHPBarUI_Updater()
    {
        hpBar_IMG.fillAmount = CalCulator.inst.StringAndStringDivideReturnFloat(enemyCurHP, enemyMaxHP, 3);
        hpBar_Text.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(enemyCurHP)}";
    }



    ////////////////////////////// [ �÷��̾� �ӵ����� ���� ] /////////////////////////////////////////


    //�÷��̾� ���ݼӵ� ���� �Լ�
    public void PlayerAttackSpeedLvUp()
    {
        if (GameStatus.inst.AtkSpeedLv >= 10) { return; }

        if (playerAnim != null)
        {
            playerAnim.SetFloat("AttackSpeed", 1 + ((0.15f * GameStatus.inst.AtkSpeedLv)) + GameStatus.inst.NewbieAttackSpeed);
        }

    }


    // �÷��̾� ������ �ӵ�����
    public void SetPlayerMoveSpeed()
    {
        float speed = 1 + GameStatus.inst.BuffAddSpeed;
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

    // ���� ��������Ʈ ���� �Լ�
    public void Set_WeaponSprite_Changer(int index)
    {
        palyerWeapenSr.sprite = weaponSprite[index];
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


    // ���� �װ� ��� ���
    public string Get_EnemyDeadGold() => CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.GetTotalGold(), 3);

    // �÷��̾� ���̶�Ű ������Ʈ ����
    public GameObject ReturnPlayerObjInHierachy() => playerAnim.gameObject;
    public GameObject ReturnEnemyObjInHierachy() => enemyObj;

    /// <summary>
    /// ���� ��������Ʈ ���������Լ�
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Sprite Get_WeaponSprite(int index) => weaponSprite[index];

}
