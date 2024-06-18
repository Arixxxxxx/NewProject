using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGame_0 : MonoBehaviour
{
    public static MiniGame_0 inst;

    GameObject miniGameRef, game0Ref, mainScrrenRef, gameInfoRef;

    GameObject[] selectMenu = new GameObject[3];

    [Header("Pooling Prefabs => <color=yellow>( �׼� ������ )")]
    [Space]
    [SerializeField]
    GameObject prefabs;

    //������Ʈ (Ǯ��) && ���� ������

    //ĳ����
    Rigidbody2D charRigid;
    AudioSource charAudio;
    Animator charAnim;
    ParticleSystem runningDustPs;
    ParticleSystem itemGetPs;

    // World �ΰ���
    GameObject world;
    float gameTime = 30f;
    float gameTimeCounter = 30;
    TMP_Text timerText;
    TMP_Text scoreText;
    Vector2 originCharVec;

    //���� üũ
    bool gameStart;
    public bool GameStart { get { return gameStart; } set { gameStart = value; } }

    bool popupresult;
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

        miniGameRef = GameManager.inst.MiniGameRef;
        game0Ref = miniGameRef.transform.Find("MiniGames/Game_0").gameObject;
        mainScrrenRef = game0Ref.transform.Find("MainScrrenCanvas").gameObject;
        gameInfoRef = game0Ref.transform.Find("Info").gameObject;
        selectMenu[2] = mainScrrenRef.transform.Find("BG/Start/Select").gameObject;
        selectMenu[1] = mainScrrenRef.transform.Find("BG/Info/Select").gameObject;
        selectMenu[0] = mainScrrenRef.transform.Find("BG/Back/Select").gameObject;

        world = game0Ref.transform.Find("Game(World)").gameObject;
        charRigid = game0Ref.transform.Find("Game(World)/Player").GetComponent<Rigidbody2D>();
        charAudio = charRigid.gameObject.GetComponent<AudioSource>();
        charAudio.volume = 0.5f;
        originCharVec = charRigid.position;

        runningDustPs = charRigid.transform.Find("Dust").GetComponent<ParticleSystem>();
        itemGetPs = charRigid.transform.Find("GetPs").GetComponent<ParticleSystem>();

        timerText = world.transform.Find("UI/Timer").GetComponent<TMP_Text>();
        scoreText = world.transform.Find("UI/Score").GetComponent<TMP_Text>();

        PrefabsInit();
    }
    void Start()
    {

    }

    Transform leftPostion, rightPosition, upMinPos, upMaxPos, prefabsPool;

    Queue<Bamboo> PrefabsQue = new Queue<Bamboo>();
    private void PrefabsInit()
    {
        leftPostion = world.transform.Find("Stage/PopupTrs/Left").GetComponent<Transform>();
        rightPosition = world.transform.Find("Stage/PopupTrs/Right").GetComponent<Transform>();
        upMinPos = world.transform.Find("Stage/PopupTrs/UpMin").GetComponent<Transform>();
        upMaxPos = world.transform.Find("Stage/PopupTrs/UpMax").GetComponent<Transform>();
        prefabsPool = world.transform.Find("Stage/PopupTrs/BambooPool").GetComponent<Transform>();

        //Pooling Awake
        Bamboo obj;
        for (int index = 0; index < 15; index++)
        {
            obj = Instantiate(prefabs, prefabsPool).GetComponent<Bamboo>();
            obj.gameObject.SetActive(false);
            PrefabsQue.Enqueue(obj);
        }

    }



    List<Bamboo> playBamboo = new List<Bamboo>();

    // �׼� Ǯ�� ����
    float poolinginterval = 0.75f;
    float Timer = 0f;
    private void PoolingStart()
    {
        int posSelect = Random.Range(0, 2);
        Timer += Time.deltaTime;

        if (Timer > poolinginterval)
        {
            AudioManager.inst.Play_Ui_SFX(5, 1f);
            Timer = 0;
            maxBambooCount++;

            if (PrefabsQue.Count <= 0)
            {
                Bamboo addobj = Instantiate(prefabs, prefabsPool).GetComponent<Bamboo>();
                addobj.gameObject.SetActive(false);
                PrefabsQue.Enqueue(addobj);
            }

            Bamboo obj = PrefabsQue.Dequeue();
            // ȸ���� ����Ʈ
            playBamboo.Add(obj);

            float whereDrop = Random.Range(0f, 100f);

            if (whereDrop < 25f) //����
            {
                //�¿����ϱ�
                int leftRight = Random.Range(0, 2);

                //���� ���Ʒ�  ����
                float ran = Random.Range(-0.3f, 0.3f);
                obj.gameObject.transform.localPosition = Vector3.zero;
                obj.gameObject.transform.localPosition += new Vector3(0, ran);

                if (leftRight == 0)
                {
                    obj.gameObject.transform.localPosition = leftPostion.transform.localPosition;

                }
                else if (leftRight == 1)
                {
                    obj.gameObject.transform.localPosition = rightPosition.transform.localPosition;
                }
                obj.gameObject.SetActive(true);
                obj.AddForceImpers(leftRight);

            }
            else if (whereDrop > 25) // ��������
            {
                float PosX = Random.Range(upMinPos.transform.localPosition.x, upMaxPos.transform.localPosition.x);
                Vector2 poolingVec = new Vector2(PosX, upMinPos.transform.localPosition.y);
                obj.gameObject.transform.localPosition = poolingVec;
                obj.gameObject.SetActive(true);
                obj.RandomDropSpeed();
            }


        }


    }
    /// <summary>
    /// ����� ������ ȸ��
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnBambooObj(Bamboo obj)
    {
        playBamboo.Remove(obj);
        obj.gameObject.SetActive(false);
        PrefabsQue.Enqueue(obj);
    }

    //��������� ���� ����
    public void AllBabooReturn()
    {
        if (playBamboo.Count <= 0) { return; }
        for (int index = playBamboo.Count - 1; index >= 0; index--)
        {
            ReturnBambooObj(playBamboo[index]);
        }
    }

    private void FixedUpdate()
    {
        if (gameStart)
        {
            MoveChar();
        }
    }
    void Update()
    {
        // ����ȭ��

        if (mainScrrenRef.activeInHierarchy || gameInfoRef.activeInHierarchy)
        {
            gameMainScrrenController();
            SelectMenu();
        }

        // �ΰ���
        if (gameStart)
        {
            ScoreAndTimeUpdater(); //�ð���
            PoolingStart();
        }

    }



    //////////////////////////////////// ����ȭ�� UI /////////////////////////////////////

    [SerializeField]
    int mainScrrenSelectIndex = 2;
    private void gameMainScrrenController()
    {
        //���ۺ�

        if (MinigameController.inst.Up && mainScrrenSelectIndex < 2)
        {
            AudioManager.inst.Play_Ui_SFX(3, 0.8f);
            MinigameController.inst.Up = false;
            mainScrrenSelectIndex++;
            SelectOptionInit();
        }
        else if (MinigameController.inst.Down && mainScrrenSelectIndex > 0)
        {
            AudioManager.inst.Play_Ui_SFX(3, 0.8f);
            MinigameController.inst.Down = false;
            mainScrrenSelectIndex--;
            SelectOptionInit();
        }

    }

   

    // ���� ����ȭ�� �޴� ���ý�
    private void SelectMenu()
    {
        //���ۺ�

        if (MinigameController.inst.Bbtn && mainScrrenSelectIndex == 2 && MinigameController.inst.Gamestart == true)
        {
            MinigameController.inst.Bbtn = false;

            MinigameManager.inst.CuttonFadeInoutAndFuntion(() =>
            {
                AudioManager.inst.PlayBGM(4, 0.4f);
                StartCoroutine(gameStartCorutines());
            });
        }

        if (MinigameController.inst.Bbtn && mainScrrenSelectIndex == 1 && MinigameController.inst.Gamestart == true )
        {
            MinigameController.inst.Bbtn = false;

            MinigameManager.inst.CuttonFadeInoutAndFuntion(() =>
            {
                mainScrrenSelectIndex = 3;
                mainScrrenRef.SetActive(false);
                gameInfoRef.SetActive(true);
            });
        }

        if (MinigameController.inst.Bbtn && mainScrrenSelectIndex == 3  && MinigameController.inst.Gamestart == true)
        {
            MinigameController.inst.Bbtn = false;

            MinigameManager.inst.CuttonFadeInoutAndFuntion(() =>
            {
               
                mainScrrenSelectIndex = 2;
                SelectOptionInit(); 
                mainScrrenRef.SetActive(true);
                gameInfoRef.SetActive(false);
            });
        }

        if (MinigameController.inst.Bbtn && mainScrrenSelectIndex == 0 && MinigameController.inst.Gamestart == true )
        {
            MinigameController.inst.Bbtn = false;

            MinigameManager.inst.CuttonFadeInoutAndFuntion(() =>
            {

                MinigameManager.inst.ActiveMinigame(0, false);
            });
        }

    }

    IEnumerator gameStartCorutines()
    {
        mainScrrenRef.SetActive(false);
        world.SetActive(true);

        gameTimeCounter = gameTime;
        timerText.text = gameTimeCounter.ToString();

        curBambooCount = 0;
        scoreText.text = curBambooCount.ToString();

        //ĳ���� �ڸ� �ʱ�ȭ
        charRigid.transform.position = originCharVec;
        charRigid.transform.localScale = Vector2.one;

        yield return new WaitForSeconds(2);
        MinigameManager.inst.ActiveGameStartCountAnimationWithAction(0);
    }
    /// <summary>
    ///  Ÿ��Ʋȭ�� ȭ��ǥ ��Ƽ�����
    /// </summary>
    public void SelectOptionInit()
    {
        for (int i = 0; i < selectMenu.Length; i++)
        {
            if (i == mainScrrenSelectIndex)
            {
                selectMenu[i].gameObject.SetActive(true);
            }
            else
            {
                selectMenu[i].gameObject.SetActive(false);
            }
        }
    }

    //////////////////////////////////// �ΰ���  /////////////////////////////////////

    /// 1. ĳ���� ///

    Vector2 moveVec;
    float moveSpeed = 0.3f;
    float maxSpeed = 0.12f;
    private void MoveChar()
    {
        if (MinigameController.inst.Right)
        {
            if(runningDustPs.isPlaying == false)
            {
                runningDustPs.Play();
            }
            
            moveVec.x += Time.deltaTime * moveSpeed;

            if (moveVec.x >= maxSpeed)
            {
                moveVec.x = maxSpeed;
            }

            if (charRigid.transform.localScale.x != 1)
            {
                charRigid.transform.localScale = new Vector2(1, 1);
            }
        }
        else if (MinigameController.inst.Left)
        {
            if (runningDustPs.isPlaying == false)
            {
                runningDustPs.Play();
            }
            moveVec.x -= Time.deltaTime * moveSpeed;

            if (moveVec.x < maxSpeed * -1)
            {
                moveVec.x = maxSpeed * -1;
            }

            if (charRigid.transform.localScale.x != -1)
            {
                charRigid.transform.localScale = new Vector2(-1, 1);
            }
        }
        else if (MinigameController.inst.Right == false && MinigameController.inst.Left == false)
        {
            runningDustPs.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            if (moveVec.x > 0)
            {
                moveVec.x -= Time.deltaTime * (moveSpeed * 2);

                if (moveVec.x <= 0)
                {
                    moveVec.x = 0;
                }
            }
            else if (moveVec.x < 0)
            {
                moveVec.x += Time.deltaTime * (moveSpeed * 2);

                if (moveVec.x >= 0)
                {
                    moveVec.x = 0;
                }
            }
        }
        charRigid.MovePosition(charRigid.position + moveVec);
        if(moveVec.x != 0 && charAudio.isPlaying == false)
        {
            charAudio.Play();
        }
        else if(moveVec.x == 0 && charAudio.isPlaying == true)
        {
            charAudio.Stop();
        }
    }

    /////  UI 
    private void ScoreAndTimeUpdater()
    {
        if (gameStart == false) { return; }

        if (gameTimeCounter > 0)
        {
            gameTimeCounter -= Time.deltaTime;
            timerText.text = ((int)gameTimeCounter).ToString();

            if (gameTimeCounter <= 0)
            {
                AudioManager.inst.PlayBGM_Mute();
                AudioManager.inst.Play_Ui_SFX(14, 0.8f);
                charAudio.Stop();
                gameStart = false;
                runningDustPs.gameObject.SetActive(false);
                gameTimeCounter = 0f;
                timerText.text = ((int)gameTimeCounter).ToString();
                // ���� �����Լ�
                MinigameManager.inst.TimeUPAnimationInvoke();
            }
        }
    }




    float curBambooCount;
    float maxBambooCount;
    // �׼� ������ ������ �־��ٰ͵�
    public void F_bamboocountUP()
    {
        AudioManager.inst.Play_Ui_SFX(13, 0.4f);
        curBambooCount++;
        scoreText.text = curBambooCount.ToString();
    }

    /// <summary>
    /// �̴ϰ��� 0�� ���θ޴���
    /// </summary>
    public void ReturnMainMenu()
    {
        GameAllReset();
    }
    /// <summary>
    /// �̴ϰ��� 0�� ����ŸƮ
    /// </summary>
    public void ReStartGame()
    {
          StartCoroutine(RestartCorutine());
     }

    IEnumerator RestartCorutine()
    {
        ResetGameInfo();
        yield return new WaitForSeconds(2);
        MinigameManager.inst.ActiveGameStartCountAnimationWithAction(0);
    }

    /// <summary>
    /// ��������� ���� ������
    /// </summary>
    /// <returns></returns>
    public float[] Get_GameScore()
    {
        float[] curandmax = new float[2];

        curandmax[0] = curBambooCount;
        curandmax[1] = maxBambooCount;

        curBambooCount = 0;
        maxBambooCount = 0;
        return curandmax;
    }

    private void ResetGameInfo()
    {
        gameTimeCounter = gameTime;
        timerText.text = gameTimeCounter.ToString();

        curBambooCount = 0;
        maxBambooCount = 0;
        scoreText.text = curBambooCount.ToString();

        charRigid.transform.position = originCharVec;
        charRigid.transform.localScale = Vector2.one;
    }


    public void GameAllReset()
    {
        GameStart = false;
        mainScrrenRef.SetActive(true);
        gameInfoRef.SetActive(false);
        world.SetActive(false);

        gameTimeCounter = gameTime;
        timerText.text = gameTimeCounter.ToString();

        curBambooCount = 0;
        maxBambooCount = 0;
        scoreText.text = curBambooCount.ToString();

        //ĳ���� �ڸ� �ʱ�ȭ
        charRigid.transform.position = originCharVec;
        charRigid.transform.localScale = Vector2.one;

        mainScrrenSelectIndex = 2;
        SelectOptionInit();
    }

    public void bambooGetParticlePlay()
    {
        itemGetPs.Play();
    }
}
