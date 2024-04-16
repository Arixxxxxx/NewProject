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

    //카메라
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin camShake;

    //배경
    GameObject worldSpaceRef;

    Material mat;
    [SerializeField] float backGroundSpeed;
    [SerializeField] GameObject[] pooling_Obj;
    Transform dmgFontParent;
    Transform goldActionParent;


    Queue<GameObject>[] prefabsQue;


    //플레이어
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

    // 에너미
    GameObject enemyObj;
    SpriteRenderer enemySr;
    Animator enemyAnim;
    Image hpBar_IMG;
    TMP_Text hpBar_Text;
    int curEnemyNum;
    [SerializeField] string enemyCurHP;
    [SerializeField] string enemyMaxHP;
    [SerializeField] Sprite[] enemySprite;

    //타격 이펙트
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

    // 이동애니메이션
    [SerializeField] float checkPosition;
    Vector2 enemyVec;
    float enemyPosX;
    [SerializeField] float enemySpawnSpeed;


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
        //최초 init
        Enemyinit();
        UI_Init();
        
        
    }

    Vector2 matVec;
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

    int index = 1;

    float feverCountTimer = 0.8f;
    float feverNextTimer;
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

        //테스트용
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameStatus.inst.GetGiftDay[0] -= 1;
            GameStatus.inst.GetNewbieGiftDay[0] -= 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //BuffContoller.inst.ActiveBuff(4, 3);
            LetterManager.inst.MakeLetter(0, "이동은", "테스트 (루비)편지입니다", 5000);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LetterManager.inst.MakeLetter(1, "게임GM", "테스트 (골드)편지입니다", 150);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LetterManager.inst.MakeLetter(2, "박겸희", "테스트 편지 (별)입니다", 200);
        }
    }

    // UI Bar 초기화
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
            playerAnim.transform.position = new Vector2(-0.706f, 5.45f);
            moveWindParticle.gameObject.SetActive(false);
        }

        if (palyerWeapenSr.enabled == false) // 무기 SpriteRenderer On
        {
            palyerWeapenSr.enabled = true;
        }
    }


    private void EnemyMove()
    {
        //에너미 스폰 및 대기장소까지 전진
        //checkPosition = Vector2.Distance(enemyObj.transform.position, enemy_StopPoint.position);
        //float spriteDistance = (enemySr.bounds.size.x / 2);

        checkPosition = enemyObj.transform.position.x - enemy_StopPoint.position.x;

        if (checkPosition > 1.35f) // 거리값 체크 2이상 이동
        {
            enemyPosX += (Time.deltaTime * enemySpawnSpeed) * (1 + (GameStatus.inst.BuffAddSpeed + GameStatus.inst.NewbieMoveSpeedBuffValue));
            enemyVec.x -= enemyPosX;
            enemyObj.transform.position = enemyVec;
        }
        else if (checkPosition < 1.35f) // 2미만 공격
        {
            attackReady = true;

            enemyVec.x = 0;
            ismove = false;
        }
    }

    // 메인캐릭터 애니메이션 공격 
    public void A_PlayerAttackToEnemy()
    {
        StopCoroutine(PlayerOnHitDMG());
        StartCoroutine(PlayerOnHitDMG());
    }


    /// <summary>
    /// 동료들 애니메이션 공격 함수
    /// </summary>
    /// <param name="CrewTypeIndex"> 0 폭탄마/ 1 사령술사 </param>
    public void A_CrewAttackToEnemy(int CrewTypeIndex)
    {
        StopCoroutine(CrewAttackToEnemyDMG(CrewTypeIndex));
        StartCoroutine(CrewAttackToEnemyDMG(CrewTypeIndex));
    }

    // 공격 함수!! //
    IEnumerator PlayerOnHitDMG() // < =
    {
        enemyAnim.SetTrigger("Hit");
        PlayerInit();

        // 뉴비버프 어택카운트 및 버프 
        GameStatus.inst.NewbieAttackCountUp(true);
        if (GameStatus.inst.IsNewBie)
        {
            atkPower = CalCulator.inst.StringAndIntMultiPly(atkPower, GameStatus.inst.Get_NewBieAttackBuff_MultiplyValue());
        }

        string DMG = atkPower;
        swordEffect.Play();

        // 크리티컬 계산
        float randomDice = Random.Range(0f, 100f);
        bool cri = false;

        //크리티컬 판정시 캠흔들림
        if (randomDice < GameStatus.inst.CriticalChance)
        {
            F_PlayerOnHitCamShake();
            DMG = CalCulator.inst.PlayerCriDMGCalculator(DMG); // 치명타 피해량 계산
            cri = true;
        }

        string checkDMG = CalCulator.inst.DigidMinus(enemyCurHP, DMG, true);

        if (checkDMG != "Dead" && attackReady == true)
        {
            // 대미지폰트
            GameObject obj = Get_Pooling_Prefabs(0);
            obj.transform.position = dmgFontParent.position;
            obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitAddFloatChanger(DMG), randomDice < GameStatus.inst.CriticalChance ? true : false, 2);
            obj.SetActive(true);

            enemyCurHP = CalCulator.inst.DigidMinus(enemyCurHP, DMG, true);
            EnemyHPBarUI_Updater();
            EnemyOnHitEffect(cri);
        }
        else if (checkDMG == "Dead")//에너미 사망 및 초기화
        {
            DogamManager.inst.MosterDogamIndexValueUP(curEnemyNum); // 몬스터 도감조각 얻기
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
        PlayerInit();

        // 뉴비버프 어택카운트 및 버프 
        GameStatus.inst.NewbieAttackCountUp(true);
        if (GameStatus.inst.IsNewBie)
        {
            atkPower = CalCulator.inst.StringAndIntMultiPly(atkPower, GameStatus.inst.Get_NewBieAttackBuff_MultiplyValue());
        }

        string DMG = atkPower;
        string CrewATK = string.Empty;
        string MinusValue = string.Empty;

        if (CrewType == 0) // 폭탄마 ( 총체력에서 공격력을 뺀값 )
        {
            CrewATK = CalCulator.inst.StringAndIntMultiPly(DMG, GameStatus.inst.Pet0_Lv + 1);
            MinusValue = CalCulator.inst.DigidMinus(enemyCurHP, CrewATK, true);
        }
        else if (CrewType == 1) // 사령술사
        {
            CrewATK = CalCulator.inst.CrewNumber2AtkCalculator(enemyCurHP);

            if (CrewATK == null) // 몬스터가 도중에 죽었다면 종료
            {
                yield break;
            }

            MinusValue = CalCulator.inst.DigidMinus(enemyCurHP, CrewATK, true);
        }
        

        if (MinusValue != "Dead" && attackReady == true)
        {
            enemyCurHP = MinusValue;
            EnemyHPBarUI_Updater();

            // 대미지폰트
            GameObject obj = Get_Pooling_Prefabs(0);
            obj.transform.position = dmgFontParent.position;

            if(CrewType == 0)
            {
                obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitAddFloatChanger(CrewATK), false, 0); // 빨간색 대미지 폰트
            }
            else if(CrewType == 1)
            {
                obj.GetComponent<DMG_Font>().SetText(CalCulator.inst.StringFourDigitAddFloatChanger(CrewATK), false, 1); // 시얀색 대미지폰트
            }
            obj.SetActive(true);
        }
        else if (MinusValue == "Dead")//에너미 사망 및 초기화
        {
            
            DogamManager.inst.MosterDogamIndexValueUP(curEnemyNum); // 몬스터 도감조각 얻기
            StartCoroutine(GetGoldActionParticle());
            // 현재 받아야되는 돈 계산
            string getGold = Get_EnemyDeadGold();
            WorldUI_Manager.inst.Get_Increase_GetGoldAndStar_Font(0, getGold);
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
        ps.transform.position = enemyAnim.transform.position + (Vector3.up * 0.5f);
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
            //보스피통 늘려주는 ~
        }
        else if (floorCount == 5)
        {
            doEnemyMove = false;
            Enemyinit();
            floorCount = 0;
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
            //보스피통 늘려주는 ~
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

        // 맵변경
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

    //에너미 피격 이펙트 함수
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



    // 몬스터 초기화
    private void Enemyinit()
    {
        // 나중에 체력 초기화 연산 바꿔야함
        swordEffect.Stop();
        enemyObj.transform.position = enemy_StartPoint.position; // 위치 초기화

        enemyMaxHP = CalCulator.inst.EnemyHpSetup();
        enemyCurHP = enemyMaxHP;

        //Hpbar 초기화
        hpBar_IMG.fillAmount = 1;
        hpBar_Text.text = string.Empty;
        EnemyHPBarUI_Updater(); // 체력

        //스프라이트 값 할당
        int spriteCount = enemySprite.Length;
        curEnemyNum = Random.Range(0, spriteCount);
        enemySr.sprite = enemySprite[curEnemyNum];
   
    }


    // 플레이어 초기화
    private void PlayerInit()
    {
        atkPower = CalCulator.inst.Get_CurPlayerATK();
        attackSpeedLv = GameStatus.inst.AtkSpeedLv;
        float attackTempSpeed = 0.6f;

        //어택레벨당 0.15초씩 감소
        attackSpeed = attackTempSpeed - (attackSpeedLv * 0.15f);
    }

    // 에너미 HP 바 업데이터

    private void EnemyHPBarUI_Updater()
    {
        hpBar_IMG.fillAmount = CalCulator.inst.StringAndStringDivideReturnFloat(enemyCurHP, enemyMaxHP, 3);
        hpBar_Text.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(enemyCurHP)}";
    }



    ////////////////////////////// [ 플레이어 속도증가 관련 ] /////////////////////////////////////////


    //플레이어 공격속도 증가 함수
    public void PlayerAttackSpeedLvUp()
    {
        if (GameStatus.inst.AtkSpeedLv >= 10) { return; }

        if (playerAnim != null)
        {
            playerAnim.SetFloat("AttackSpeed", 1 + ((0.15f * GameStatus.inst.AtkSpeedLv)) + GameStatus.inst.NewbieAttackSpeed);
        }

    }


    // 플레이어 움직임 속도변경
    public void SetPlayerMoveSpeed()
    {
        float speed = 1 + GameStatus.inst.BuffAddSpeed;
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

    // 카메라 쉐이크
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


    public void TestBtnWeaponChange()
    {
        index++;

        if (index >= weaponSprite.Length)
        {
            index = 0;
        }

        palyerWeapenSr.sprite = weaponSprite[index];

    }


    // 몬스터 죽고 골드 상승
    public string Get_EnemyDeadGold() => CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.GetTotalGold(), 3);

    // 플레이어 하이라키 오브젝트 리턴
    public GameObject ReturnPlayerObjInHierachy() => playerAnim.gameObject;
    public GameObject ReturnEnemyObjInHierachy() => enemyObj;

    /// <summary>
    /// 무기 스프라이트 가져가는함수
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Sprite Get_WeaponSprite(int index) => weaponSprite[index];

}
