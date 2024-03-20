using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ActionManager : MonoBehaviour
{
    public static ActionManager inst;

    //카메라
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin camShake;

    //배경
    GameObject worldSpaceGroup;
    
    Material mat;
    [SerializeField] float backGroundSpeed;
    [SerializeField] GameObject[] pooling_Obj;
    Transform dmgFontParent;

    Queue<GameObject>[] prefabsQue;


    //플레이어
    Animator playerAnim;
    int attackSpeedLv;
    float attackSpeed;
    float atkPower;
    SpriteRenderer palyerWeapen;

    // 에너미
    GameObject enemyObj;
    SpriteRenderer enemySr;
    Animator enemyAnim;
    Image hpBar_IMG;
    TMP_Text hpBar_Text;
    [SerializeField] float enemyCurHP;
    [SerializeField] float enemyMaxHP;
    [SerializeField] Sprite[] enemySprite;

    //타격 이펙트
    [SerializeField] ParticleSystem[] enemyEffect;

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

        //최초 init
       
        Enemyinit();
        PlayerInit();

    }

    Vector2 matVec;

    private void FixedUpdate()
    {
        if (attackReady == false) // 몬스터 전진
        {
            MoveEnemyAndMap();
        }
    }
    void Update()
    {
        if (attackReady == true) // 전투
        {
            AttackEnemy();
        }

        EnemyHPBarUI_RealTimeUpdater();
        atkPowerUpdater();
    }





    private void MoveEnemyAndMap()
    {
        // 움직이기전 초기화
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

        // 배경 움직임
        matVec.x += Time.deltaTime * backGroundSpeed;
        matVec.x = Mathf.Repeat(matVec.x, 1);
        mat.mainTextureOffset = matVec;


        //에너미 스폰 및 대기장소까지 전진
        checkPosition = Vector2.Distance(enemyObj.transform.position, enemy_StopPoint.position);

        if (checkPosition > 0.5f) // 거리값 체크 2이상 이동
        {
            enemyPosX += Time.deltaTime * enemySpawnSpeed;
            enemyVec.x -= enemyPosX;
            enemyObj.transform.position = enemyVec;
        }
        else if (checkPosition < 0.5f) // 2미만 공격
        {
            enemyVec.x = 0;
            attackReady = true;
            ismove = false;
        }
    }


    float count = 0;
    private void AttackEnemy()
    {
        //애니메이션 켜주고
        if(playerAnim.GetBool("Move") == true)
        {
            playerAnim.SetBool("Move", false);
        }

        if(palyerWeapen.enabled == false)
        {
            palyerWeapen.enabled = true;
        }


        //공격
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
            // 대미지폰트
            GameObject obj = Get_Pooling_Prefabs(0);
            obj.transform.position = dmgFontParent.position;
            float randomDice = Random.Range(0f, 100f);

            //크리티컬 판정시 캠흔들림
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
            //에너미 사망 및 초기화
            Enemyinit();
            attackReady = false;
        }
        
    }

    int effectIndexCount = 0;

    //에너미 피격 이펙트 함수
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
        // 나중에 체력 초기화 연산 바꿔야함
        enemyObj.transform.position = enemy_StartPoint.position; // 위치 초기화
        enemyMaxHP = /*GameStatus.inst.StageLv **/ 100; // 체력초기화
        enemyCurHP = enemyMaxHP;

        //Hpbar 초기화
        hpBar_IMG.fillAmount = 1;
        hpBar_Text.text = string.Empty;

        //스프라이트 값 할당
        int spriteCount = enemySprite.Length;
        int spirteRanValue = Random.Range(0, spriteCount);
        enemySr.sprite = enemySprite[spirteRanValue];

       
    }



    private void PlayerInit()
    {
        attackSpeedLv = GameStatus.inst.AtkSpeedLv;
        float attackTempSpeed = 0.6f;

        //어택레벨당 0.3초씩 감소
        attackSpeed = attackTempSpeed - (attackSpeedLv * 0.15f);
    }

    // 에너미 HP 바 업데이터
    private void EnemyHPBarUI_RealTimeUpdater()
    {
        hpBar_IMG.fillAmount = enemyCurHP / enemyMaxHP;

        // 나중에 변환 필요함!!@#@!##!@!@#
        hpBar_Text.text = $"{enemyCurHP}";
    }

    // 추후에 연산 입력해야함
    private void atkPowerUpdater()
    {
        atkPower = 10;
    }




    //풀링시스템
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
}
