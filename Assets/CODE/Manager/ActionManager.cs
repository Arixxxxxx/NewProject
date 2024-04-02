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
    GameObject worldSpaceGroup;

    Material mat;
    [SerializeField] float backGroundSpeed;
    [SerializeField] GameObject[] pooling_Obj;
    Transform dmgFontParent;
    Transform goldActionParent;


    Queue<GameObject>[] prefabsQue;


    //�÷��̾�
    Animator playerAnim;
    int attackSpeedLv;
    float attackSpeed;
    string atkPower;
    SpriteRenderer palyerWeapenSr;
    [SerializeField] Sprite[] weaponSprite;
    [SerializeField] Sprite[] backGroudSprite;
    SpriteRenderer backGroundIMG;
    GameObject moveWindParticle;
    ParticleSystem swordEffect;


    // ���ʹ�
    GameObject enemyObj;
    SpriteRenderer enemySr;
    Animator enemyAnim;
    Image hpBar_IMG;
    TMP_Text hpBar_Text;
    [SerializeField] string enemyCurHP;
    [SerializeField] string enemyMaxHP;
    [SerializeField] Sprite[] enemySprite;

    //Ÿ�� ����Ʈ
    ParticleSystem[] enemyEffect;
    ParticleSystem[] enemyCriEffect;

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
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        worldSpaceGroup = GameObject.Find("---[World Space]").gameObject;
        petManager = GetComponent<PetContollerManager>();

        goldActionParent = worldSpaceGroup.transform.Find("GoldActionDynamic").GetComponent<Transform>();

        backGroundIMG = worldSpaceGroup.transform.Find("BackGround_IMG").GetComponent<SpriteRenderer>();
        mat = backGroundIMG.material;
        playerAnim = worldSpaceGroup.transform.Find("Player_Obj/Sprite").GetComponent<Animator>();

        moveWindParticle = playerAnim.transform.Find("MoveWind").gameObject;

        palyerWeapenSr = playerAnim.transform.Find("Weapon").GetComponent<SpriteRenderer>();
        swordEffect = playerAnim.transform.Find("0").GetComponent<ParticleSystem>();

        enemyObj = worldSpaceGroup.transform.Find("Enemy").gameObject;
        enemySr = enemyObj.transform.Find("Sprite").GetComponent<SpriteRenderer>();

        enemyAnim = enemySr.GetComponent<Animator>();
        int forcount = enemyObj.transform.Find("Effect").childCount;

        enemyEffect = new ParticleSystem[forcount];
        for (int i = 0; i < forcount; i++)
        {
            enemyEffect[i] = enemyObj.transform.Find("Effect").GetChild(i).GetComponent<ParticleSystem>();
        }

        forcount = enemyObj.transform.Find("CriEffect").childCount;
        enemyCriEffect = new ParticleSystem[forcount];
        for (int i = 0; i < forcount; i++)
        {
            enemyCriEffect[i] = enemyObj.transform.Find("CriEffect").GetChild(i).GetComponent<ParticleSystem>();
        }


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

        //���� init
        Enemyinit();
        UI_Init();
    }

    Vector2 matVec;
    bool doEnemyMove = true;
    private void FixedUpdate()
    {
        if (attackReady == false) //��� �̵�
        {
            MoveMap();

            if (doEnemyMove == true)
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

        


        //�׽�Ʈ��

        if (Input.GetKeyDown(KeyCode.Q))
        {
           
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameStatus.inst.GetGiftDay[0] -= 1;
            GameStatus.inst.GetNewbieGiftDay[0] -= 1;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LetterManager.inst.MakeLetter(0, "�̵���", "�׽�Ʈ (���)�����Դϴ�", 100);
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


    private void UI_Init()
    {
        // UI Bar �ʱ�ȭ
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


    private void MoveMap()
    {
        // �����̱��� �ʱ�ȭ
        if (ismove == false)
        {
            ismove = true;

            if (playerAnim.GetBool("Move") == false)
            {
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
        matVec.x += (Time.deltaTime * backGroundSpeed) * GameStatus.inst.BuffAddSpeed;
        matVec.x = Mathf.Repeat(matVec.x, 1);
        mat.mainTextureOffset = matVec;

    }

    
    
    private void EnemyMove()
    {
        //���ʹ� ���� �� �����ұ��� ����
        checkPosition = Vector2.Distance(enemyObj.transform.position, enemy_StopPoint.position);

        if (checkPosition > 0.85f) // �Ÿ��� üũ 2�̻� �̵�
        {
            enemyPosX += (Time.deltaTime * enemySpawnSpeed) * GameStatus.inst.BuffAddSpeed;
            enemyVec.x -= enemyPosX;
            enemyObj.transform.position = enemyVec;
        }
        else if (checkPosition < 0.85f) // 2�̸� ����
        {
            attackReady = true;

            enemyVec.x = 0;
            ismove = false;
        }
    }

    //�ִϸ��̼� ���� �Լ�
    public void A_PlayerAttackToEnemy()
    {
        StopCoroutine(enemyOnHit(0));
        StartCoroutine(enemyOnHit(0));
    }
    public void A_Pet0AttackToEnemy()
    {
        StopCoroutine(enemyOnHit(1));
        StartCoroutine(enemyOnHit(1));
    }

    // ���� �Լ� 
    private void AttackEnemy()
    {

        if (playerAnim.GetBool("Move") == true) // ���� Animation On
        {

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


    IEnumerator enemyOnHit(int index)
    {
        enemyAnim.SetTrigger("Hit");
        PlayerInit();
        //�⺻ ����� ���
        string firstDmg = CalCulator.inst.DigidPlus(atkPower, GameStatus.inst.AddPetAtkBuff);
        string AdDMG = CalCulator.inst.DigidPlus(firstDmg, GameStatus.inst.BuffAddAdATK);
        string DMG = CalCulator.inst.DigidPlus(AdDMG, GameStatus.inst.BuffAddATK);

        if (index == 0) // �÷��̾��Ͻ�
        {
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
            //Debug.Log($"���� ü�� : {enemyCurHP} / �ִ�ü�� : {enemyMaxHP} / ���� ����� {DMG}");

            if (checkDMG != "Dead" && attackReady == true)
            {
                // �������Ʈ
                GameObject obj = Get_Pooling_Prefabs(0);
                obj.transform.position = dmgFontParent.position;
                obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitChanger(DMG), randomDice < GameStatus.inst.CriticalChance ? true : false, 1);
                obj.SetActive(true);

                enemyCurHP = CalCulator.inst.DigidMinus(enemyCurHP, DMG, true);
                EnemyHPBarUI_Updater();
                EnemyOnHitEffect(cri);
            }
            else if(checkDMG == "Dead")//���ʹ� ��� �� �ʱ�ȭ
            {
                StartCoroutine(GetGoldActionParticle());
                // ���� �޾ƾߵǴ� �� ���
                string getGold = Get_EnemyDeadGold();
                GameStatus.inst.TakeGold(getGold);
                EnemyDeadFloorUp();
            }
            
            // ��Ÿ ������ ������ 0���� �ʱ�ȭ
            PetContollerManager.inst.AttackBuffDisable();

        }
        

        else if(index == 1)
        {
            // ������ ���� => �÷��̾� ����� * �극��+1

            string PetDmg = CalCulator.inst.StringAndIntMultiPly(DMG, GameStatus.inst.Pet0_Lv + 1);
            string MinusValue = CalCulator.inst.DigidMinus(enemyCurHP, PetDmg, true); // ��ü�¿��� ���ݷ��� ����

            if (MinusValue != "Dead" && attackReady == true)
            {
                enemyCurHP = MinusValue;
                EnemyHPBarUI_Updater();

                // �������Ʈ
                GameObject obj = Get_Pooling_Prefabs(0);
                obj.transform.position = dmgFontParent.position;

                obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitChanger(PetDmg), false,0);
                obj.SetActive(true);
                
            }
            else if(MinusValue == "Dead")//���ʹ� ��� �� �ʱ�ȭ
            {
                StartCoroutine(GetGoldActionParticle());
                // ���� �޾ƾߵǴ� �� ���
                string getGold = Get_EnemyDeadGold();
                WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(0, getGold);
                GameStatus.inst.TakeGold(getGold);
                EnemyDeadFloorUp();
            }
        }

        yield return null;

    }

    // ���� �׾����� ���� �� Ƣ�� ��ƼŬ ��� �Լ�
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
    /// ���� ��� Init �Լ�
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
            if (effectIndexCount == enemyEffect.Length - 1)
            {
                effectIndexCount = 0;
            }

            enemyEffect[effectIndexCount].Play();
            effectIndexCount++;
        }
        else
        {
            if (effectCriIndexCount == enemyCriEffect.Length - 1)
            {
                effectCriIndexCount = 0;
            }

            enemyCriEffect[effectCriIndexCount].Play();
            effectCriIndexCount++;
        }


    }
    bool isUIActive;


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
        int spirteRanValue = Random.Range(0, spriteCount);
        enemySr.sprite = enemySprite[spirteRanValue];
    }


    private void PlayerInit()
    {
        atkPower = CalCulator.inst.Get_ATKtoString();
        attackSpeedLv = GameStatus.inst.AtkSpeedLv;
        float attackTempSpeed = 0.6f;

        //���÷����� 0.3�ʾ� ����
        attackSpeed = attackTempSpeed - (attackSpeedLv * 0.15f);
    }

    // ���ʹ� HP �� ��������
    float fillAmountA, fillAmountB;
    private void EnemyHPBarUI_Updater()
    {
        hpBar_IMG.fillAmount = CalCulator.inst.StringAndStringDivideReturnFloat(enemyCurHP, enemyMaxHP , 3);
        hpBar_Text.text = $"{CalCulator.inst.StringFourDigitChanger(enemyCurHP)}";
    }

    // ���Ŀ� ���� �Է��ؾ���
    


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
    public string Get_EnemyDeadGold() => CalCulator.inst.StringAndIntMultiPly(UIManager.Instance.GetTotalGold(), 3);
    
    // �÷��̾� ���̶�Ű ������Ʈ ����
    public GameObject ReturnPlayerObjInHierachy () => playerAnim.gameObject;
    public GameObject ReturnEnemyObjInHierachy () => enemyObj;

    // �÷��̾� ������ �ӵ�����
    public void SetPlayerMoveSpeed() => playerAnim.SetFloat("MoveSpeed", GameStatus.inst.BuffAddSpeed);

}
