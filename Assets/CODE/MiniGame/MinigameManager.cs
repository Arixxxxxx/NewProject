using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager inst;

    GameObject frontUiRef;

    // ���� �������� ���ӹ�ȣ
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

    //�̴ϰ��� ����â Ref
    GameObject enterWindowRef;
    Button goRulletBtn, startMinigameBtn, enterWindowXBtn;

    //RulletShop
    GameObject rulletShop;

    //�����Ҳ��� ����â
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


    // ���ӽ��� �ִϸ��̼ǰ���
    Action gameCountAnimationEvent;
    Animator startCountAnim;

    //Ÿ�Ӿ� �ִϸ��̼�
    Animator timeUpAnim;

    //Result ���â �ִϸ��̼� �˾�
    bool popupresult;
    public bool PopupResult { get { return popupresult; } set { popupresult = value; } }

    Animator starAnim; // �� �ִϸ��̼�
    TMP_Text countText; // �̴ϰ���ȭ�� ���� ���
    Image fillbar;
    TMP_Text fillText;
    GameObject resultRef;
    //���â ���� ȭ��ǥ ��Ƽ��
    GameObject[] resultMenuSelect = new GameObject[2];

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
        frontUiRef = GameManager.inst.FrontUiRef;

        //�̴ϰ��� ����â
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
        noBtn = endGameQuestionBox.transform.Find("Window/Cutton/Middle/No").GetComponent<Button>();
        YesBtn = endGameQuestionBox.transform.Find("Window/Cutton/Middle/Yes").GetComponent<Button>();

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

    // ��ư �ʱ�ȭ
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
    /// �̴ϰ��� ������ ���� �� ���� ���� �����ϴ� â
    /// </summary>
    /// <param name="value"></param>
    public void Active_minigameEntrance(bool value)
    {
        enterWindowRef.SetActive(value);
    }

    //���� ������ ���ʻ��·� ���ִ� �Լ�
    public void minigameReset()
    {
        mainScrrenRef.SetActive(true);
        titleLogo.SetActive(true);
        selectGameRef.SetActive(false);
        endGameQuestionBox.SetActive(false);
        curPlayGameNum = -1;
        resultRef.SetActive(false);
        // ���ӵ� �ʱ�ȭ
        MinigameController.inst.ResetViewBoxPos();
        MiniGame_0.inst.GameAllReset();
    }
    /// <summary>
    /// Ÿ��Ʋȭ�� ���� -> ���Ӽ���Ʈ���̵� 
    /// </summary>
    public void endTitle()
    {
        StartCoroutine(endTitleCor());
    }

    // Ÿ��Ʋȭ�鿡�� ���Ӱ���â���� �Ѿ�� �ð�
    WaitForSeconds nextMenuTime = new WaitForSeconds(0.2f);
    IEnumerator endTitleCor()
    {
        titleLogo.SetActive(false);
        yield return nextMenuTime;
        selectGameRef.SetActive(true);
    }

    /// <summary>
    /// ���ӽ��۽� ���̵�ƿ�
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

        while (cutton.color.a > 0)
        {
            cutton.color -= outColor;
            yield return null;
        }
    }

    [SerializeField]
    bool selectGame;
    /// <summary>
    /// ���� ���� �����Ҷ� ȣ���ϴ� ���̵��ξƿ�
    /// </summary>
    /// <param name="funtion"></param>
    public void CuttonFadeInoutAndFuntion(Action funtion)
    {
        if (selectGame == true) { return; }

        selectGame = true;
        cutton.color = new Color(0, 0, 0, 0);

        startGameFuntion = null;
        startGameFuntion += funtion;

        StartCoroutine(CuttonFadeInOut());
    }
    Color inOutColor = new Color(0, 0, 0, 0.005f);
    IEnumerator CuttonFadeInOut()
    {
        while (cutton.color.a < 1)
        {
            cutton.color += inOutColor;
            yield return null;
        }

        startGameFuntion?.Invoke();

        yield return nextMenuTime;
        yield return nextMenuTime;
        yield return nextMenuTime;
        yield return nextMenuTime;
        yield return nextMenuTime;

        while (cutton.color.a > 0)
        {
            cutton.color -= inOutColor;
            yield return null;
        }

        selectGame = false;
    }

    /// <summary>
    /// �̴ϰ��� Ȱ��ȭ
    /// </summary>
    /// <param name="index"></param>
    public void ActiveMinigame(int index, bool value)
    {
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
        }
    }


    /// <summary>
    /// ���ӹ�ȣ �޾Ƽ� 
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
                //miniGame_0.GameStart = true; 1�� �������� �ٲ����
                break;
        }
    }


    /// <summary>
    ///<br> �̴ϰ��� ������ 3,2,1 ī��Ʈ ȣ���ϴ� �Լ� </br>
    ///<br> �Ű������� �̴ϰ��� ��ȣ�� ������ȴ� </br>
    /// </summary>
    /// <param name="gameNum"></param>
    public void ActiveGameStartCountAnimationWithAction(int gameNum)
    {
        CurPlayGameNum = gameNum; // �����ϸ鼭 ���ӹ�ȣ ������
        gameCountAnimationEvent = null;
        gameCountAnimationEvent += () => MiniGameStart(gameNum);
        startCountAnim.gameObject.SetActive(true);
        startCountAnim.SetTrigger("Start");
    }

    /// <summary>
    /// �ִϸ��̼� Ŭ������ ȣ����
    /// </summary>
    public void InvokeStartCountAction()
    {
        gameCountAnimationEvent?.Invoke();
    }

    //��������
    public void TimeUPAnimationInvoke()
    {
        timeUpAnim.gameObject.SetActive(true);
    }

    /// <summary>
    /// �̴ϰ��� ������ ���â ȣ��
    /// </summary>
    public void Set_ReSultValueAndActive()
    {
        PopupResult = true;

        fillbar.fillAmount = 0;
        fillText.text = "/";

        resultRef.SetActive(true);

        float[] getCountAndMaxCunt = new float[2];

        // ���ھ� �� �ִ������� ��������
        switch (CurPlayGameNum)
        {
            case 0:
                getCountAndMaxCunt = MiniGame_0.inst.Get_GameScore();
                break;

            case 1:

                break;
        }

        // �� ���� ���
        int star = getCountAndMaxCunt[0] / getCountAndMaxCunt[1] < 0.3f ? 1 : getCountAndMaxCunt[0] / getCountAndMaxCunt[1] < 0.6f ? 2 : 3;
        countText.text = star.ToString();

        // �̴ϰ�����ū ���� 
        GameStatus.inst.MinigameTicket += star;

        // �� ���
        StartCoroutine(fillAndTextAction(getCountAndMaxCunt[0], getCountAndMaxCunt[1], star));
    }

    WaitForSeconds fillWaitTime = new WaitForSeconds(0.08f);
    IEnumerator fillAndTextAction(float cur, float max, int star)
    {
        yield return null;

        string animString = star.ToString();
        starAnim.SetTrigger(animString);
        float actionValue = 0;

        float duration = 0.01f; // ������ �ɸ��� �ð�, �ʿ信 ���� ����
        float interpolationProgress = 0f; // ���� �����

        while (actionValue < cur)
        {
            float targetFillAmount = actionValue / max;

            // �ʱ� ���� ������� ����
            interpolationProgress = 0f;

            // ���� fillAmount���� ��ǥ fillAmount���� ���� ����
            while (interpolationProgress < 1.0f)
            {
                interpolationProgress += Time.deltaTime / duration;
                fillbar.fillAmount = Mathf.Lerp(fillbar.fillAmount, targetFillAmount, interpolationProgress);
                yield return null; // ���� �����ӱ��� ��ٸ�
            }

            fillText.text = $"{actionValue} / {max}";
            actionValue++;
            yield return fillWaitTime;
        }

    }

    int resultMenuSelectIndex = 0;
    public int ResultMenuSelectIndex { get { return resultMenuSelectIndex; } set { resultMenuSelectIndex = value; resultMenuSelectActiveInit(); } }

    // Result �޴� �˾��� �ϴ� ȭ��ǥ �̵� �� B��ư �̿� ����
    public void ResultMenuContoller()
    {
        if (PopupResult == false) { return; }

        if (MinigameController.inst.Right == true && popupresult == true)
        {
            MinigameController.inst.Right = false;

            if (ResultMenuSelectIndex < 1)
            {
                ResultMenuSelectIndex++;
            }
        }
        else if (MinigameController.inst.Left == true && popupresult == true)
        {
            MinigameController.inst.Left = false;

            if (ResultMenuSelectIndex > 0)
            {
                ResultMenuSelectIndex--;
            }
        }

        //���θ޴��� (B��ư Ŭ��)
        if (MinigameController.inst.Bbtn == true && popupresult == true && ResultMenuSelectIndex == 0)
        {
            MinigameController.inst.Bbtn = false;
            popupresult = false;


            //�˾�â����
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

        //����ŸƮ
        if (MinigameController.inst.Bbtn == true && popupresult == true && resultMenuSelectIndex == 1)
        {
            MinigameController.inst.Bbtn = false;
            popupresult = false;


            switch (curPlayGameNum)
            {
                case 0:
                    CuttonFadeInoutAndFuntion(() =>
                    {
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
