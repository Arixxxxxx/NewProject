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

    //카메라
    GameObject camsRef;
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin camShake;

    //배경
    GameObject worldSpaceRef;

    Material mat;


    [Header("1. Input BG Tilling Speed <Color=yellow>( Float Data )</Color>")]
    [Space]
    [SerializeField] float backGroundSpeed;
    [Space]
    [Header("2. Insert Object Prefabs  <Color=#6699FF>( Prefabs )")]
    [Tooltip("대미지폰트, 몬스터죽고나서 돈튀는 이펙트")]
    [Space]
    [SerializeField] GameObject[] pooling_Obj;
    [Space]
    Transform dmgFontParent;
    Transform goldActionParent;


    Queue<GameObject>[] prefabsQue;


    //플레이어
    Animator playerAnim;
    GameObject playerRef;
    int attackSpeedLv;
    string atkPower;
    SpriteRenderer palyerWeapenSr;
    Sprite[] weaponSprite;

    SpriteRenderer backGroundIMG;
    GameObject moveWindParticle;
    ParticleSystem playerDustPs;

    // 에너미

    GameObject enemyObj;
    SpriteRenderer enemySr;
    Animator enemyAnim;
    GameObject[] hpBar_OutLine = new GameObject[2];

    GameObject hpBarRef;
    Image hpBar_IMG;
    Image hpBar_BackIMG;
    TMP_Text hpBar_Text;

    [Header("5. ReadOnly ★ <Color=#CC3D3D>( Check Data )")]
    [Space]
    [SerializeField] string enemyCurHP;
    [SerializeField] string enemyMaxHP;
    [SerializeField] bool attackReady;
    [Space]
    //[Header("6. Insert Enemy List => <Color=#47C832>( Sprite File )")]
    //[Space]
    //[SerializeField] Sprite[] enemySprite;
    //[Space]

    //타격 이펙트
    GameObject effectRef, playerAttackEffectRef, playerCriAttackEffectRef;

    ParticleSystem[] playerAtkEffect;
    ParticleSystem[] playerAtkCriEffect;
    ParticleSystem swordEffect;

    Transform enemy_StartPoint;
    Transform enemy_StopPoint;

    bool actionStart;
    bool isatk, ismove;
    public bool IsMove { get { return ismove; } }


    // 이동애니메이션

    float checkPosition;
    UnityEngine.Vector2 enemyVec;
    float enemyPosX;
    [Header("7. Insert Value => <Color=yellow>( Float Data )")]
    [Space]
    [SerializeField] float enemySpawnSpeed;

    // 카메라 쉐이크
    [SerializeField] float shakeTime;
    float shakeCount;

    int floorCount;

    //피버 체크
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
        //최초 init
        Enemyinit();
    }

    UnityEngine.Vector2 matVec;
    bool doEnemyMove = true;
    private void FixedUpdate()
    {
        if (attackReady == false && IsFever == false) //배경 이동
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
        if (attackReady == true && IsFever == false) // 전투
        {
            AttackEnemy();
        }

        if (IsFever == true) // 피버일시 스테이지 올려줌
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
            WorldUI_Manager.inst.Get_ItemInfomation_UI_Active(SpriteResource.inst.CoinIMG(0), "루비 +100");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //BuffContoller.inst.ActiveBuff(4, 3);
            LetterManager.inst.MakeLetter(0, "이동은", "테스트 (루비)편지입니다", 5000);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LetterManager.inst.MakeLetter(1, "게임GM", "테스트 (골드)편지입니다", 15000);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LetterManager.inst.MakeLetter(2, "박겸희", "테스트 편지 (별)입니다", 2000);
        }

#endif
    }

    public void FeverTime_End()
    {
        attackReady = false;
        doEnemyMove = true;
    }

    // 움직이기전 초기화
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

        // 배경 움직임
        matVec.x += (Time.deltaTime * backGroundSpeed) * (1 + (GameStatus.inst.BuffAddSpeed + GameStatus.inst.NewbieMoveSpeedBuffValue));
        matVec.x = Mathf.Repeat(matVec.x, 1);
        mat.mainTextureOffset = matVec;

    }

    // 공격 함수 
    private void AttackEnemy()
    {
        if (playerAnim.GetBool("Move") == true) // 공격 Animation On
        {
            playerDustPs.gameObject.SetActive(false);
            playerAnim.SetBool("Move", false);
            petManager.PetAnimPlay(false);
            playerAnim.transform.position = new UnityEngine.Vector2(-0.706f, 5.45f);
            moveWindParticle.gameObject.SetActive(false);
        }

        if (palyerWeapenSr.enabled == false) // 무기 SpriteRenderer On
        {
            palyerWeapenSr.enabled = true;
        }
    }

    public float buffAddSpeed = 0f;
    private void EnemyMove()
    {
        //에너미 스폰 및 대기장소까지 전진
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

    // 메인캐릭터 애니메이션 공격 
    public void A_PlayerAttackToEnemy()
    {
        if (IsFever) { StopCoroutine(PlayerOnHitDMG()); return; }
        StopCoroutine(PlayerOnHitDMG());
        StartCoroutine(PlayerOnHitDMG());
    }


    /// <summary>
    /// 동료들 애니메이션 공격 함수
    /// </summary>
    /// <param name="CrewTypeIndex"> 0 폭탄마/ 1 사령술사 </param>
    public void A_CrewAttackToEnemy(int CrewTypeIndex)
    {
        if(IsFever) { StopCoroutine(CrewAttackToEnemyDMG(CrewTypeIndex)); return; }
        StopCoroutine(CrewAttackToEnemyDMG(CrewTypeIndex));
        StartCoroutine(CrewAttackToEnemyDMG(CrewTypeIndex));
    }

    // 공격 함수!! //
    IEnumerator PlayerOnHitDMG() // < =
    {
        enemyAnim.SetTrigger("Hit");
        

        // 뉴비버프 어택카운트 및 버프 
        GameStatus.inst.NewbieAttackCountUp(true);
        string DMG = CalCulator.inst.Get_CurPlayerATK();
        swordEffect.Play();

        // 크리티컬 계산
        float randomDice = UnityEngine.Random.Range(0f, 100f);
        bool cri = false;

        //크리티컬 판정시 캠흔들림
        if (randomDice < GameStatus.inst.CriticalChance)
        {
            F_PlayerOnHitCamShake();
            DMG = CalCulator.inst.PlayerCriDMGCalculator(DMG); // 치명타 피해량 계산
            cri = true;
        }
        AudioManager.inst.Play_HitOnly(0, 0.4f, cri); // 사운드플레이

        string checkDMG = CalCulator.inst.BigIntigerMinus(enemyCurHP, DMG);/* DigidMinus(enemyCurHP, DMG, true);*/

        if (checkDMG != "0" && attackReady == true)
        {
            AudioManager.inst.MonsterHitOnly(true,1f);
            // 대미지폰트
            GameObject obj = Get_Pooling_Prefabs(0);
            obj.transform.position = enemySr.bounds.center;
            //obj.transform.position = dmgFontParent.position;
            obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitAddFloatChanger(DMG), randomDice < GameStatus.inst.CriticalChance ? true : false, 2);
            obj.SetActive(true);

            enemyCurHP = CalCulator.inst.BigIntigerMinus(enemyCurHP, DMG);
            EnemyHPBar_FillAmount_Init();
            EnemyOnHitEffect(cri);
        }
        else if (checkDMG == "0")//에너미 사망 및 초기화
        {
            AudioManager.inst.MonsterHitOnly(false, 1f); //

            DogamManager.inst.MosterDogamIndexValueUP(curEnemyNumber[0], curEnemyNumber[1]); // 몬스터 도감조각 얻기
            StartCoroutine(GetGoldActionParticle()); // 골드 획득하는 파티클 재생

            // 현재 받아야되는 돈 계산
            string getGold = Get_EnemyDeadGold();
            GameStatus.inst.PlusGold(getGold); // 골드 얻기
            EnemyDeadFloorUp();
            GameStatus.inst.NewbieAttackCountUp(false); // 뉴비버프 어택카운트0

            // 누적 기록
            GameStatus.inst.TotalEnemyKill++;
        }

        // 한타 떄려서 버프값 0으로 초기화
        PetContollerManager.inst.AttackBuffDisable();


        yield return null;
    }


    // 동료 공격 코루틴
    /// <summary>
    /// 
    /// </summary>
    /// <param name="CrewType"> 0 폭탄마 / 1 사령술사</param>
    /// <returns></returns>
    IEnumerator CrewAttackToEnemyDMG(int CrewType)
    {
        enemyAnim.SetTrigger("Hit");
        //PlayerInit();
        // 뉴비버프 어택카운트 및 버프 

        //string DMG = CalCulator.inst.Get_CurPlayerATK();
        string CrewATK = string.Empty;
        string MinusValue = string.Empty;

        if (CrewType == 0) // 폭탄마 ( 총체력에서 공격력을 뺀값 )
        {
            CrewATK = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.TotalAtk.ToString(), GameStatus.inst.Pet0_Lv + 2); // 시작시 3배 시작
            MinusValue = CalCulator.inst.BigIntigerMinus(enemyCurHP, CrewATK);
        }
        else if (CrewType == 1) // 사령술사
        {
            CrewATK = CalCulator.inst.CrewNumber2AtkCalculator(enemyCurHP);

            if (CrewATK == null) // 몬스터가 도중에 죽었다면 종료
            {
                yield break;
            }

            MinusValue = CalCulator.inst.BigIntigerMinus(enemyCurHP, CrewATK);
        }


        if (MinusValue != "0" && attackReady == true)
        {
            enemyCurHP = MinusValue;
            EnemyHPBar_FillAmount_Init();

            // 대미지폰트
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
        else if (MinusValue == "0")//에너미 사망 및 초기화
        {

            DogamManager.inst.MosterDogamIndexValueUP(curEnemyNumber[0], curEnemyNumber[1]); // 몬스터 도감조각 얻기
            StartCoroutine(GetGoldActionParticle());

            // 현재 받아야되는 돈 계산
            string getGold = Get_EnemyDeadGold();
            GameStatus.inst.PlusGold(getGold);

            EnemyDeadFloorUp();
            GameStatus.inst.NewbieAttackCountUp(false); // 뉴비버프 어택카운트0

            // 누적 기록
            GameStatus.inst.TotalEnemyKill++;
        }


        yield return null;

    }
    // 몬스터 죽엇을때 돈이 팡 튀는 파티클 재생 
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
    /// 몬스터 사망 Init
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
            StartCoroutine(NextStageAction()); //  다음 층으로 이동하는거처럼
        }
    }


    /// <summary>
    /// 피버시 출력
    /// </summary>
    private void FeverFloorUp()
    {
        floorCount++;
        // 현재 받아야되는 돈 계산
        string getGold = Get_EnemyDeadGold();
        GameStatus.inst.PlusGold(getGold); // 골드 얻기

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
        //위치 옴긴후
        enemyObj.transform.position = enemy_StartPoint.position;

        playerAnim.SetTrigger("Out");

        yield return new WaitForSeconds(0.2f);

        WorldUI_Manager.inst.Set_Auto_BlackCutton(1);
        yield return new WaitForSeconds(1);

        // 맵변경
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

    //에너미 피격 이펙트 함수
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



    // 몬스터 초기화
    UnityEngine.Vector3 hpbarPos;

    //float hpBar_downX = 0.1f;
    float hpBar_downY = 0f;
    private void Enemyinit()
    {
        // 나중에 체력 초기화 연산 바꿔야함
        swordEffect.Stop();
        enemyObj.transform.position = enemy_StartPoint.position; // 위치 초기화

        //Hpbar 초기화
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
    /// 에너미 보스 설정
    /// </summary>
    /// <param name="isBoss"></param>
    private void Set_EnemyBossOrNormal()
    {
        int bossHPMultiPlyer = 1;
        
        //현재 스테이지의 몬스터 스프라이트 가져옴
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

        //현재 몬스터 번호 생성
        curEnemyNumber[1] = UnityEngine.Random.Range(0, curStageEnemy.Length);
        enemySr.sprite = curStageEnemy[curEnemyNumber[1]];

        // 체력초기화
        enemyMaxHP = CalCulator.inst.EnemyHpSetup();

        if (GameStatus.inst.FloorLv != 5) // 일반몹
        {
            enemySr.transform.localScale = UnityEngine.Vector3.one;

            //몬스터 HP바 아웃라인
            hpBar_OutLine[0].SetActive(true);
            hpBar_OutLine[1].SetActive(false);
        }
        else if (GameStatus.inst.FloorLv == 5) // 보스몹
        {
            //보스체력 2배
            enemyMaxHP = CalCulator.inst.StringAndIntMultiPly(enemyMaxHP, bossHPMultiPlyer);
            enemySr.transform.localScale = bossSpriteSize;
            //몬스터 HP바 아웃라인
            hpBar_OutLine[0].SetActive(false);
            hpBar_OutLine[1].SetActive(true);
        }

        // HPBar 위치 초기화
        hpbarPos = enemyObj.transform.position;
        //hpbarPos.x -= hpBar_downX;
        hpbarPos.y -= hpBar_downY;
        hpBarRef.transform.position = hpbarPos;

        enemyCurHP = enemyMaxHP;
        EnemyHPBar_FillAmount_Init(); // 체력
    }





    // 몬스터 체력바 업데이터
    private void EnemyHPBar_FillAmount_Init()
    {
        hpBar_IMG.fillAmount = CalCulator.inst.StringAndStringDivideReturnFloat(enemyCurHP, enemyMaxHP, 3);
        hpBar_Text.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(enemyCurHP)}";
    }

    // 체력 후방바 
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

    ////////////////////////////// [ 플레이어 속도증가 관련 ] /////////////////////////////////////////



    //플레이어 공격속도 증가 함수
    public void PlayerAttackSpeedLvUp()
    {

        int multypleValue = GameStatus.inst.GetAryRelicLv(7); // 유물 레벨
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


    // 플레이어 움직임 속도변경
    public void SetPlayerMoveSpeed()
    {
        float speed = 1 + GameStatus.inst.BuffAddSpeed + GameStatus.inst.NewbieMoveSpeedBuffValue;
        //Debug.Log($"토탈 {speed} /버프{GameStatus.inst.BuffAddSpeed} / 뉴비 {GameStatus.inst.NewbieMoveSpeedBuffValue}");
        playerAnim.SetFloat("MoveSpeed", speed);
    }




    //풀링시스템
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


    // 카메라 쉐이크
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

        while (camShake.m_AmplitudeGain > 0) // 부드럽게 쉐이크 꺼지기위함
        {
            camShake.m_AmplitudeGain -= Time.deltaTime * 10;
            yield return null;
        }

        camShake.m_AmplitudeGain = 0;
        camShake.m_FrequencyGain = 0;
    }

    // 무기 스프라이트 변경 함수
    public void Set_WeaponSprite_Changer(int index)
    {
        palyerWeapenSr.sprite = weaponSprite[index];
    }



    // 몬스터 죽고 골드 상승
    float defaultKillGoldValue = 0.5f;
    int relicMultiplyer = 0;
    BigInteger curgold = new BigInteger();
    public string Get_EnemyDeadGold()
    {
        curgold = BigInteger.Parse(GameStatus.inst.GetTotalGold());
        curgold = CalCulator.inst.MultiplyBigIntegerAndfloat(curgold, defaultKillGoldValue);
        string normalValue = curgold.ToString();

        // 적 처지골드 유물
        if (GameStatus.inst.GetAryRelicLv(9) != 0)
        {
            relicMultiplyer = GameStatus.inst.GetAryRelicLv(9);
            normalValue = CalCulator.inst.DigitAndIntPercentMultiply(normalValue, relicMultiplyer);
        }

        return normalValue;
    }


    // 플레이어 하이라키 오브젝트 리턴
    public GameObject ReturnPlayerObjInHierachy() => playerAnim.gameObject;
    public GameObject ReturnEnemyObjInHierachy() => enemyObj;

    /// <summary>
    /// 무기 스프라이트 가져가는함수
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Sprite Get_WeaponSprite(int index) => weaponSprite[index];

    /// <summary>
    /// 현재 몬스터 스프라이트기준 센터 월드 포지션
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public UnityEngine.Vector3 Get_CurEnemyCenterPosition() => enemySr.bounds.center;


}
