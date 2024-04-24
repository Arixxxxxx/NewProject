using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameController : MonoBehaviour
{
    public static MinigameController inst;

    GameObject miniGameRef, controllerRef, gameBoy, colorThreeBtn;
    GameObject titleScrren, selectGameScrren, mainScrrenRef;
    //���Ӻ��� ��ư
    bool up, down, left, right, aBtn, bBtn, selectBtn, startBtn;

    public bool Up
    {
        get { return up; }
        set
        { // �Է½�ȣ�� �ѹ��� �ޱ�����
            if (up == !value)
                up = value;
        }
    }
    public bool Down { get { return down; } set { if (down == !value) down = value; } }
    public bool Left { get { return left; } set { if (left == !value) left = value; } }
    public bool Right { get { return right; } set { if (right == !value) right = value; } }
    public bool Abtn { get { return aBtn; } set { if (aBtn == !value) aBtn = value; } }
    public bool Bbtn { get { return bBtn; } set { if (bBtn == !value) bBtn = value; } }
    public bool SelectBtn { get { return selectBtn; } set { if (selectBtn == !value) selectBtn = value; } }
    public bool StartBtn { get { return startBtn; } set { if (startBtn == !value) startBtn = value; } }
    bool red, green, blue;


    //SelectGame ���� ������

    RectTransform viewBox;
    int gameCount;
    TMP_Text gameNameText;
    

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
        controllerRef = miniGameRef.transform.Find("GameController").gameObject;
        gameBoy = controllerRef.transform.Find("GameBoyPad").gameObject;

        mainScrrenRef = MinigameManager.inst.MainScrrenRef;
        titleScrren = MinigameManager.inst.TitleLogoRef;
        selectGameScrren = MinigameManager.inst.SelectGameRef;

        viewBox = selectGameScrren.transform.Find("ViewGame").GetComponent<RectTransform>();
        gameCount = viewBox.childCount;
        gameNameText = selectGameScrren.transform.Find("GameName").GetComponent<TMP_Text>();

    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TitleScrrenKeyInput_Cheack();
        SelectGameScrrenKeyInput_Cheack();
    }

    //Ÿ��Ʋȭ�� -> ���Ӽ���ȭ��
    private void TitleScrrenKeyInput_Cheack()
    {
        if (mainScrrenRef.gameObject.activeSelf == false) { return; }

        if (mainScrrenRef.gameObject.activeSelf && bBtn == true)
        {
            titleScrren.GetComponent<Animator>().SetTrigger("Hide");
        }
    }

    //���Ӽ���ȭ�� -> ���ӽ���

    int gameIndex = 0;
    bool waitAction;
    private void SelectGameScrrenKeyInput_Cheack()
    {
        if (selectGameScrren.gameObject.activeSelf == false && gameStart == false) { return; }

        if (selectGameScrren.gameObject.activeSelf && left == true && waitAction == false)
        {
            waitAction = true;
            StartCoroutine(ViewNext(0));
        }
        if (selectGameScrren.gameObject.activeSelf && right == true && waitAction == false)
        {
            waitAction = true;
            StartCoroutine(ViewNext(1));
        }

        if(selectGameScrren.gameObject.activeSelf && bBtn == true && gameStart == false) 
        {
            gameStart = true;
            GameStart();
        }
    }


    IEnumerator ViewNext(int leftRight)
    {
        //����ó��
        if (leftRight == 0 && gameIndex == 0 ) { waitAction=false; yield break; }
        if (leftRight == 1 && gameIndex == gameCount - 1) { waitAction = false; yield break; }


        Vector2 vec = new Vector2(-1000 * Time.deltaTime, 0);
        float nextAmount = 455;
        float arrivePosX = 0f;
        if (leftRight == 0)
        {
           arrivePosX = viewBox.anchoredPosition.x + nextAmount;

            gameIndex--;
            while (viewBox.anchoredPosition.x < arrivePosX)
            {
                viewBox.anchoredPosition -= vec;
                yield return null;
            }
        }
        else if(leftRight == 1)
        {
            arrivePosX = viewBox.anchoredPosition.x - nextAmount;
            gameIndex++;

            while (viewBox.anchoredPosition.x > arrivePosX)
            {
                viewBox.anchoredPosition += vec;
                yield return null;
            }
        }

        viewBox.anchoredPosition = new Vector2(arrivePosX, 0);
        GameNameTextInit(); // ���� �̸� �ʱ�ȭ
        waitAction = false;
    }


    private void GameNameTextInit()
    {
        switch(gameIndex)
        {
            case 0:
                gameNameText.text = "<color=yellow>�׼� �׼�!</color>";
                break;

            case 1:
                gameNameText.text = "<color=yellow>��� �ٽ�!";
                break;

            case 2:
                gameNameText.text = "<color=red>Coming Soon";
                break;

            case 3:
                gameNameText.text = "<color=red>Coming Soon";
                break;
        }
    }

    bool gameStart; // ���߿� ������ ���ƿË�
    public bool Gamestart  {   get { return gameStart; } set { gameStart = value; }}
    private void GameStart()
    {
        MinigameManager.inst.CuttonFadeInoutAndFuntion(() => 
        {
            MinigameManager.inst.ActiveMinigame(gameIndex,true);
        });
    }


    /// <summary>
    /// ���������� �ʿ��Ѱ͵� ����
    /// </summary>
    public void ResetViewBoxPos()
    {
        gameStart =false;
        gameIndex = 0;
        waitAction = false;
        viewBox.anchoredPosition = Vector2.zero;
    }
}
