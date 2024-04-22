using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager inst;

    //Ref
    GameObject mainScrrenRef;
    GameObject miniGameRef, miniGamesRef;
    GameObject titleLogo;

    //MiniGameref
    int miniGameCount;
    [SerializeField]
    GameObject[] miniGame;

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
        mainScrrenRef = miniGameRef.transform.Find("MainScreen").gameObject;
        miniGamesRef = miniGameRef.transform.Find("BG/MiniGames").gameObject;
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

        BtnInit();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
    }

    //���� ������ ���ʻ��·� ���ִ� �Լ�
    public void minigameReset()
    {
        mainScrrenRef.SetActive(true);
        titleLogo.SetActive(true);
        selectGameRef.SetActive(false);
        endGameQuestionBox.SetActive(false);
        MinigameController.inst.ResetViewBoxPos();
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
        cutton.color = new Color(0,0,0,0);

        startGameFuntion = null;
        startGameFuntion += funtion;

        StartCoroutine(CuttonFadeInOut());
    }
    Color inOutColor = new Color(0, 0, 0, 0.0025f);
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


}
