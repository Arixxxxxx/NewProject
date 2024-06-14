using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager inst;

    GameObject frontUiRef;

    // 현재 실행중인 게임번호
    int curPlayGameNum = -1;
    public int CurPlayGameNum { get { return curPlayGameNum; } set { curPlayGameNum = value; } }

    //Ref
    GameObject mainScrrenRef;
    public GameObject MainScrrenRef
    {
        get { return mainScrrenRef; }
    }

    GameObject miniGameRef, miniGamesRef;
    GameObject titleLogo;

    //MiniGameref
    int miniGameCount;
    GameObject[] miniGame;

    //미니게임 입장창 Ref
    GameObject enterWindowRef;
    Button goRulletBtn, startMinigameBtn, enterWindowXBtn;

    //RulletShop
    GameObject rulletShop;

    //종료할껀지 묻는창
    GameObject endGameQuestionBox;
    Button noBtn, YesBtn;


    Button gameEndBtn;
    public GameObject TitleLogoRef
    {
        get { return titleLogo; }
    }

    GameObject selectGameRef;
    
    public GameObject SelectGameRef { get { return selectGameRef; } }
    Image cutton;
    Action startGameFuntion;


    // 게임시작 애니메이션관련
    Action gameCountAnimationEvent;
    Animator startCountAnim;

    //타임업 애니메이션
    Animator timeUpAnim;

    //Result 결과창 애니메이션 팝업
    bool popupresult;
    public bool PopupResult { get { return popupresult; } set { popupresult = value; } }

    GameObject[] stars = new GameObject[3];
    Animator starAnim; // 별 애니메이션
    TMP_Text countText; // 미니게임화폐 갯수 출력
    Image fillbar;
    TMP_Text fillText;
    TMP_Text result_MenuText, result_ReStartText;
    GameObject resultRef, startCanvas;
    //결과창 선택 화살표 액티브
    [SerializeField][Tooltip("0논클릭/1클릭")] TMP_ColorGradient[] ResultMenuGradiuntPreset;
    GameObject[] resultMenuSelect = new GameObject[2];
    Canvas gameEndCanvas;

    [SerializeField]
    Canvas[] minigameCanvas;
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

        //모든 캔버스 다 찾아서 
        minigameCanvas = miniGameRef.GetComponentsInChildren<Canvas>(true);
        for(int index =0; index < minigameCanvas.Length; index++)
        {
            minigameCanvas[index].worldCamera = Camera.main;
        }

        frontUiRef = GameManager.inst.FrontUiRef;

        //미니게임 입장창
        enterWindowRef = frontUiRef.transform.Find("MiniGameEntrance").gameObject;
        goRulletBtn = enterWindowRef.transform.Find("Window/Btns/Rullet").GetComponent<Button>();
        startMinigameBtn = enterWindowRef.transform.Find("Window/Btns/GameStart").GetComponent<Button>();
        enterWindowXBtn = enterWindowRef.transform.Find("Window/Title/X_Btn").GetComponent<Button>();

        mainScrrenRef = miniGameRef.transform.Find("MainScreen").gameObject;
        miniGamesRef = miniGameRef.transform.Find("MiniGames").gameObject;
        miniGameCount = miniGamesRef.transform.childCount;
        miniGame = new GameObject[miniGameCount];

        for (int index = 0; index < miniGameCount; index++)
        {
            miniGame[index] = miniGamesRef.transform.GetChild(index).gameObject;
        }

        cutton = miniGameRef.transform.Find("GameController/GameBoyPad/Cutton").GetComponent<Image>();
        titleLogo = mainScrrenRef.transform.Find("UI/Title").gameObject;
        selectGameRef = mainScrrenRef.transform.Find("UI/SelectGame").gameObject;
        gameEndBtn = miniGameRef.transform.Find("GameController/GameBoyPad/OnOFFBtn").GetComponent<Button>();

        endGameQuestionBox = miniGameRef.transform.Find("GameEnd").gameObject;
        gameEndCanvas = endGameQuestionBox.transform.Find("Window").GetComponent<Canvas>();
        gameEndCanvas.worldCamera = Camera.main;

        noBtn = endGameQuestionBox.transform.Find("Window/Cutton/Middle/No").GetComponent<Button>();
        YesBtn = endGameQuestionBox.transform.Find("Window/Cutton/Middle/Yes").GetComponent<Button>();

        startCanvas = miniGamesRef.transform.Find("StartCanvas").gameObject;
        resultRef = miniGamesRef.transform.Find("StartCanvas/Result").gameObject;
        startCountAnim = miniGamesRef.transform.Find("StartCanvas/GameStart_Animation").GetComponent<Animator>();
        timeUpAnim = miniGamesRef.transform.Find("StartCanvas/Time_Up").GetComponent<Animator>();
        //resultAnim = miniGamesRef.transform.Find("StartCanvas/Result/Result").GetComponent<Animator>();
        starAnim = miniGamesRef.transform.Find("StartCanvas/Result/Star").GetComponent<Animator>();
        fillbar = miniGamesRef.transform.Find("StartCanvas/Result/GageBar/FillBar").GetComponent<Image>();
        fillText = miniGamesRef.transform.Find("StartCanvas/Result/GageBar/GetCount").GetComponent<TMP_Text>();

        resultMenuSelect[0] = miniGamesRef.transform.Find("StartCanvas/Result/MenuBtn/Active").gameObject;
        resultMenuSelect[1] = miniGamesRef.transform.Find("StartCanvas/Result/ReStartBtn/Active").gameObject;

        countText = miniGamesRef.transform.Find("StartCanvas/Result/RewardText/CountText").GetComponent<TMP_Text>();
        stars[0] = miniGamesRef.transform.Find("StartCanvas/Result/Star/0/star").gameObject;
        stars[1] = miniGamesRef.transform.Find("StartCanvas/Result/Star/1/star").gameObject;
        stars[2] = miniGamesRef.transform.Find("StartCanvas/Result/Star/2/star").gameObject;

        result_MenuText = miniGamesRef.transform.Find("StartCanvas/Result/MenuBtn").GetComponent<TMP_Text>();
        result_ReStartText = miniGamesRef.transform.Find("StartCanvas/Result/ReStartBtn").GetComponent<TMP_Text>();
        BtnInit();
     
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ResultMenuContoller();
    }

    // 버튼 초기화
    private void BtnInit()
    {
        gameEndBtn.onClick.AddListener(() => endGameQuestionBox.SetActive(true));
        noBtn.onClick.AddListener(() => endGameQuestionBox.SetActive(false));
        YesBtn.onClick.AddListener(() =>
        {
            GameManager.inst.ActiveMiniGameMode(false);
            minigameReset();
        });

        goRulletBtn.onClick.AddListener(() =>
        {
            Active_minigameEntrance(false);
            EventShop_RulletManager.inst.Active_RulletEventShop(true);
        });

        startMinigameBtn.onClick.AddListener(() => 
        {
            Active_minigameEntrance(false);
            GameManager.inst.ActiveMiniGameMode(true);
        });
        enterWindowXBtn.onClick.AddListener(() =>
        {
            Active_minigameEntrance(false);
        });
    }

    /// <summary>
    /// 미니게임 입장전 설명 및 어디로 갈지 선택하는 창
    /// </summary>
    /// <param name="value"></param>
    public void Active_minigameEntrance(bool value)
    {
        enterWindowRef.SetActive(value);
    }

    //껏다 켯을때 최초상태로 해주는 함수
    public void minigameReset()
    {
        for (int index = 0; index < startCanvas.transform.childCount; index++)
        {
            startCanvas.transform.GetChild(index).gameObject.SetActive(false);
        }

        mainScrrenRef.SetActive(true);
        titleLogo.SetActive(true);
        selectGameRef.SetActive(false);
        endGameQuestionBox.SetActive(false);
        curPlayGameNum = -1;
        resultRef.SetActive(false);
        // 게임들 초기화
        MinigameController.inst.ResetViewBoxPos();
        MiniGame_0.inst.GameAllReset();
    }
    /// <summary>
    /// 타이틀화면 종료 -> 게임셀렉트로이동 
    /// </summary>
    public void endTitle()
    {
        StartCoroutine(endTitleCor());
    }

    // 타이틀화면에서 게임고르는창으로 넘어가는 시간
    WaitForSeconds nextMenuTime = new WaitForSeconds(0.2f);
    IEnumerator endTitleCor()
    {
        titleLogo.SetActive(false);
        yield return nextMenuTime;
        selectGameRef.SetActive(true);
    }

    float startFadeDuration = 1f;
    float ingameFadeDuration = 0.5f;

    /// <summary>
    /// 게임시작시 페이드아웃
    /// </summary>
    public void CuttonFadeOut()
    {
        StartCoroutine(CuttonFadeOutCorutine());
    }

    Color outColor = new Color(0, 0, 0, 0.003f);
    Color blackColor = new Color(0, 0, 0, 1f);
    IEnumerator CuttonFadeOutCorutine()
    {
        cutton.color = blackColor;

        float elapsedTime = 0f;

        // 페이드 아웃
        while (elapsedTime < startFadeDuration)
        {
            cutton.color = new Color(0, 0, 0, 1 - Mathf.Clamp01(elapsedTime / startFadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    [SerializeField]
    bool selectGame;


    /// <summary>
    /// 게임 고르고 시작할때 호출하는 페이드인아웃
    /// </summary>
    /// <param name="funtion"></param>
    public void CuttonFadeInoutAndFuntion(Action funtion)
    {
        if (selectGame == true) { return; }
        AudioManager.inst.Play_Ui_SFX(0, 0.8f);
        selectGame = true;
        cutton.color = new Color(0, 0, 0, 0);

        startGameFuntion = null;
        startGameFuntion += funtion;

        StartCoroutine(CuttonFadeInOut());
    }

  
    IEnumerator CuttonFadeInOut()
    {

        float elapsedTime = 0f;
        
        // 페이드 인
        while (elapsedTime < ingameFadeDuration)
        {
            cutton.color = new Color(0, 0, 0, Mathf.Clamp01(elapsedTime / ingameFadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cutton.color = new Color(0, 0, 0, 1);
        startGameFuntion?.Invoke();

        yield return nextMenuTime;
        yield return nextMenuTime;



        elapsedTime = 0f;

        // 페이드 아웃
        while (elapsedTime < ingameFadeDuration)
        {
            cutton.color = new Color(0, 0, 0, 1 - Mathf.Clamp01(elapsedTime / ingameFadeDuration));
            elapsedTime += Time.deltaTime * 0.5f;
            yield return null;
        }

        cutton.color = new Color(0, 0, 0, 0);
        selectGame = false;

        selectGame = false;
    }

    /// <summary>
    /// 미니게임 활성화 혹은 종료 해서 나오기
    /// </summary>
    /// <param name="index"></param>
    public void ActiveMinigame(int index, bool value)
    {
        //BGM변경
        switch (index)
        {
            case 0:
                AudioManager.inst.PlayBGM(2, 0.6f);
                break;
        }

        if (value)
        {
            miniGame[index].SetActive(true);
            mainScrrenRef.SetActive(false);
        }
        else
        {
            for (int indexs = 0; indexs < miniGame.Length; indexs++)
            {
                if (miniGame[indexs].activeSelf == true)
                {
                    miniGame[indexs].SetActive(false);
                }
            }
            AudioManager.inst.PlayBGM(1, 0.8f);
            minigameReset();
           
        }
    }


    /// <summary>
    /// 게임번호 받아서 
    /// </summary>
    /// <param name="gameNum"></param>
    public void MiniGameStart(int gameNum)
    {
        switch (gameNum)
        {
            case 0:
                MiniGame_0.inst.GameStart = true;
                break;

            case 1:
                //miniGame_0.GameStart = true; 1번 게임으로 바꿔야함
                break;
        }
    }


    /// <summary>
    ///<br> 미니게임 시작후 3,2,1 카운트 호출하는 함수 </br>
    ///<br> 매개변수로 미니게임 번호를 넣으면된다 </br>
    /// </summary>
    /// <param name="gameNum"></param>
    public void ActiveGameStartCountAnimationWithAction(int gameNum)
    {
        CurPlayGameNum = gameNum; // 시작하면서 게임번호 저장함
        gameCountAnimationEvent = null;
        gameCountAnimationEvent += () => MiniGameStart(gameNum);
        startCountAnim.gameObject.SetActive(true);
        startCountAnim.SetTrigger("Start");
    }

    /// <summary>
    /// 애니메이션 클립에서 호출함
    /// </summary>
    public void InvokeStartCountAction()
    {
        gameCountAnimationEvent?.Invoke();
    }

    //게임종료
    public void TimeUPAnimationInvoke()
    {
        timeUpAnim.gameObject.SetActive(true);
    }

    /// <summary>
    /// 미니게임 종료후 결과창 호출
    /// </summary>
    public void Set_ReSultValueAndActive()
    {
        PopupResult = true;

        fillbar.fillAmount = 0;
        fillText.text = "/";
        for(int index=0; index < stars.Length; index++)
        {
            stars[index].SetActive(false);
        }

        resultRef.SetActive(true);
        AudioManager.inst.Play_Ui_SFX(17, 0.5f);
        float[] getCountAndMaxCunt = new float[2];

        // 스코어 및 최대점수량 가져오기
        switch (CurPlayGameNum)
        {
            case 0:
                getCountAndMaxCunt = MiniGame_0.inst.Get_GameScore();
                break;

            case 1:

                break;
        }

        // 별 갯수 계산
        float checkValue = getCountAndMaxCunt[0] / getCountAndMaxCunt[1];

        int star = 0;
        if (checkValue >= 0 && checkValue < 0.4f) 
        {
            star = 1;
        }
        else if(checkValue >= 0.4f && checkValue < 0.7f)
        {
            star = 2;
        }
        else if(checkValue >= 0.7f && checkValue <= 1)
        {
            star = 3;
        }
        countText.text = star.ToString();

        // 미니게임토큰 지급 
        GameStatus.inst.MinigameTicket += star;

        // 바 계산
        StartCoroutine(fillAndTextAction(getCountAndMaxCunt[0], getCountAndMaxCunt[1], star));
    }

    WaitForSeconds fillWaitTime = new WaitForSeconds(0.08f);
    IEnumerator fillAndTextAction(float cur, float max, int star)
    {
        yield return null;

        string animString = star.ToString();
        starAnim.SetTrigger(animString);
        float actionValue = 0;

        float duration = 0.01f; // 보간에 걸리는 시간, 필요에 따라 조절
        float interpolationProgress = 0f; // 보간 진행률

        while (actionValue < cur)
        {
            float targetFillAmount = actionValue / max;

            // 초기 보간 진행률을 리셋
            interpolationProgress = 0f;

            // 현재 fillAmount부터 목표 fillAmount까지 보간 실행
            while (interpolationProgress < 1.0f)
            {
                interpolationProgress += Time.deltaTime / duration;
                fillbar.fillAmount = Mathf.Lerp(fillbar.fillAmount, targetFillAmount, interpolationProgress);
                yield return null; // 다음 프레임까지 기다림
            }

            fillText.text = $"{actionValue} / {max}";
            actionValue++;
            yield return fillWaitTime;
        }

    }

    int resultMenuSelectIndex = 0;
    public int ResultMenuSelectIndex { get { return resultMenuSelectIndex; } set { resultMenuSelectIndex = value; resultMenuSelectActiveInit(); } }

    float nonClickFontSize = 30f;
    float ClickFontSize= 40f;
    // Result 메뉴 팝업시 하단 화살표 이동 및 B버튼 이용 실행
    public void ResultMenuContoller()
    {
        if (PopupResult == false) { return; }

        if (MinigameController.inst.Right == true && popupresult == true)
        {
            MinigameController.inst.Right = false;

            if (ResultMenuSelectIndex < 1)
            {
                AudioManager.inst.Play_Ui_SFX(2, 0.8f);
                ResultMenuSelectIndex++;
                result_ReStartText.fontSize = ClickFontSize;
                result_ReStartText.colorGradientPreset = ResultMenuGradiuntPreset[1];
                result_MenuText.fontSize = nonClickFontSize;
                result_MenuText.colorGradientPreset = ResultMenuGradiuntPreset[0];

            }
        }
        else if (MinigameController.inst.Left == true && popupresult == true)
        {
            MinigameController.inst.Left = false;

            if (ResultMenuSelectIndex > 0)
            {
                AudioManager.inst.Play_Ui_SFX(2, 0.8f);
                ResultMenuSelectIndex--;
                result_ReStartText.fontSize = nonClickFontSize;
                result_ReStartText.colorGradientPreset = ResultMenuGradiuntPreset[0];
                result_MenuText.fontSize = ClickFontSize;
                result_MenuText.colorGradientPreset = ResultMenuGradiuntPreset[1];
            }
        }

        //메인메뉴로 (B버튼 클릭)
        if (MinigameController.inst.Bbtn == true && popupresult == true && ResultMenuSelectIndex == 0)
        {
            //AudioManager.inst.SleepMode_SFX(9, 0.8f);
            AudioManager.inst.PlayBGM(2, 0.8f);
            MinigameController.inst.Bbtn = false;
            popupresult = false;


            //팝업창끄기
            switch (curPlayGameNum)
            {
                case 0:
                    CuttonFadeInoutAndFuntion(() =>
                    {
                        MiniGame_0.inst.ReturnMainMenu();
                        resultRef.SetActive(false);
                        ResultMenuSelectIndex = 0;
                    });

                    break;
            }
        }

        //리스타트
        if (MinigameController.inst.Bbtn == true && popupresult == true && resultMenuSelectIndex == 1)
        {
            MinigameController.inst.Bbtn = false;
            popupresult = false;


            switch (curPlayGameNum)
            {
                case 0:
                    CuttonFadeInoutAndFuntion(() =>
                    {
                        AudioManager.inst.PlayBGM(4, 0.4f);
                        MiniGame_0.inst.ReStartGame();
                        resultRef.SetActive(false);
                        ResultMenuSelectIndex = 0;
                    });
                    break;
            }
        }

    }

    private void resultMenuSelectActiveInit()
    {
        for (int i = 0; i < resultMenuSelect.Length; i++)
        {
            if (i == resultMenuSelectIndex)
            {
                resultMenuSelect[i].SetActive(true);
            }
            else
            {
                resultMenuSelect[i].SetActive(false);
            }
        }
    }




}
