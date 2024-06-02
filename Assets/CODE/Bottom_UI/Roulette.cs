using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Numerics;
using TMPro;

public class Roulette : MonoBehaviour
{
    //////////////////////////////////저장해야되는 값////////////////////////////////////////////////
    //int RouletteTicket
    //{
    //    get => rouletteTicket;
    //    set
    //    {
    //        rouletteTicket = value;

    //    }
    //}
    int rouletteStack;
    int RouletteStack
    {
        get => rouletteStack;
        set
        {
            rouletteStack = value;
            GameStatus.inst.RouletteStack = rouletteStack;
        }
    }
    List<bool> bingoBoard = new List<bool>();
    void SetBingoBoard(int index, bool value)
    {
        bingoBoard[index] = value;
        GameStatus.inst.SetBingoBoard(bingoBoard);
    }
    void SetBingoBoard(List<bool> value)
    {
        bingoBoard = value;
        GameStatus.inst.SetBingoBoard(bingoBoard);
    }
    //bool[] boingoBoard = new bool[8];
    //bool[] BingoBoard
    //{
    //    get => boingoBoard;
    //    set
    //    {
    //        boingoBoard = value;
    //        GameStatus.inst.SetBingoBoard(boingoBoard);
    //    }
    //}
    ////////////////////////////////////////////////////////////////////////////////////////////////

    [SerializeField] float setspd;
    [SerializeField] float despd;
    [SerializeField] List<RewardList> list_reward = new List<RewardList>();
    [Serializable]
    public class RewardList
    {
        [SerializeField] ProductTag rewardType;
        [SerializeField] int count;

        public ProductTag GetRewardType()
        {
            return rewardType;
        }

        public int GetCount()
        {
            return count;
        }
    }

    Image[] list_showImage;
    Image[] list_countBtnImage = new Image[3];
    int countBtnNum = 0;
    TMP_Text[] list_GetText;
    TMP_Text[] list_CountText;
    Transform BingoParents;
    Transform ShowRewardParents;
    Transform ShowBingoParents;
    GameObject showBingo;
    Button showBingoCloseBtn;

    Image roulette;
    Image player;
    Button openBtn;
    Button closeBtn;
    Button StartBtn;
    Button StopBtn;
    RectTransform rouletteRect;
    Animator animator;
    TMP_Text[] goldText2 = new TMP_Text[3];
    TMP_Text[] goldText5 = new TMP_Text[3];
    TMP_Text[] goldText10 = new TMP_Text[3];
    TMP_Text ticketText;

    BigInteger nowTotalGold;
    float speed;
    bool isSpin;
    int useCount = 1;
    int bingoStack;
    int BingoStack
    {
        get => bingoStack;
        set
        {
            bingoStack = value;
            GameStatus.inst.BingoStack = bingoStack;
        }
    }

    List<GameObject> bingoMask = new List<GameObject>();
    List<GameObject> showBingoMask = new List<GameObject>();


    private void Start()
    {
        Transform canvas = GameObject.Find("---[UI Canvas]").transform;
        roulette = transform.Find("BackGround/Roulette").GetComponent<Image>();
        player = transform.Find("BackGround/Player").GetComponent<Image>();

        openBtn = GameObject.Find("---[World UI Canvas]").transform.Find("StageUI/MenuBox/Btns/Bingo").GetComponent<Button>();
        StartBtn = transform.Find("BackGround/StartBtn").GetComponent<Button>();
        StopBtn = transform.Find("BackGround/StopBtn").GetComponent<Button>();
        closeBtn = transform.Find("TopScroll/CloseBtn").GetComponent<Button>();
        showBingoCloseBtn = transform.Find("BackGround/ShowBingo/Button").GetComponent<Button>();

        goldText2[0] = transform.Find("BackGround/Roulette/Gold2/Text (TMP)").GetComponent<TMP_Text>();
        goldText2[1] = transform.Find("BackGround/bingo/Coin2/Text (TMP)").GetComponent<TMP_Text>();
        goldText2[2] = transform.Find("BackGround/ShowBingo/Bingo/Coin2/Text (TMP)").GetComponent<TMP_Text>();

        goldText5[0] = transform.Find("BackGround/Roulette/Gold5/Text (TMP)").GetComponent<TMP_Text>();
        goldText5[1] = transform.Find("BackGround/bingo/Coin5/Text (TMP)").GetComponent<TMP_Text>();
        goldText5[2] = transform.Find("BackGround/ShowBingo/Bingo/Coin5/Text (TMP)").GetComponent<TMP_Text>();

        goldText10[0] = transform.Find("BackGround/Roulette/Gold10/Text (TMP)").GetComponent<TMP_Text>();
        goldText10[1] = transform.Find("BackGround/bingo/Coin10/Text (TMP)").GetComponent<TMP_Text>();
        goldText10[2] = transform.Find("BackGround/ShowBingo/Bingo/Coin10/Text (TMP)").GetComponent<TMP_Text>();

        ticketText = transform.Find("BackGround/count").GetComponent<TMP_Text>();
        showBingo = transform.Find("BackGround/ShowBingo").gameObject;
        list_countBtnImage[0] = transform.Find("BackGround/CountBtn/Button").GetComponent<Image>();
        list_countBtnImage[1] = transform.Find("BackGround/CountBtn/Button (1)").GetComponent<Image>();
        list_countBtnImage[2] = transform.Find("BackGround/CountBtn/Button (2)").GetComponent<Image>();

        RouletteStack = GameStatus.inst.RouletteStack;
        BingoStack = GameStatus.inst.BingoStack;
        SetBingoBoard(GameStatus.inst.GetBingoBoard());

        //빙고판 초기화
        BingoParents = transform.Find("BackGround/bingo");
        int BingoCount = BingoParents.childCount;
        for (int iNum = 0; iNum < BingoCount; iNum++)
        {
            bingoMask.Add(BingoParents.GetChild(iNum).Find("Mask").gameObject);
        }
        bingoMask.RemoveAt(4);

        //빙고 달성창 초기화
        ShowBingoParents = transform.Find("BackGround/ShowBingo/Bingo");
        int showBingoCount = ShowBingoParents.childCount;
        for (int iNum = 0; iNum < showBingoCount; iNum++)
        {
            showBingoMask.Add(ShowBingoParents.GetChild(iNum).Find("Mask").gameObject);
        }

        int bingoCount = bingoMask.Count;
        for (int iNum = 0; iNum < bingoCount; iNum++)
        {
            if (bingoBoard[iNum])
            {
                bingoMask[iNum].SetActive(true);
                if (iNum < 4)
                {
                    showBingoMask[iNum].SetActive(true);
                }
                else
                {
                    showBingoMask[iNum + 1].SetActive(true);
                }
            }
        }
        showBingoMask[4].SetActive(true);



        //보상 확인창 초기화
        ShowRewardParents = transform.Find("BackGround/ShowReward");
        int ShowCount = ShowRewardParents.childCount;
        list_showImage = new Image[BingoCount];
        list_GetText = new TMP_Text[BingoCount];
        list_CountText = new TMP_Text[BingoCount];
        for (int iNum = 0; iNum < ShowCount; iNum++)
        {
            list_showImage[iNum] = ShowRewardParents.GetChild(iNum).Find("Image").GetComponent<Image>();
            list_GetText[iNum] = ShowRewardParents.GetChild(iNum).Find("GetText").GetComponent<TMP_Text>();
            list_CountText[iNum] = ShowRewardParents.GetChild(iNum).Find("CountText").GetComponent<TMP_Text>();

        }
        clickOpen();
        GameStatus.inst.OnRouletteTicketChanged.AddListener(() =>
        {
            ticketText.text = GameStatus.inst.RouletteTicket.ToString();
            if (GameStatus.inst.RouletteTicket <= 0)
            {
                StartBtn.interactable = false;
            }
            else
            {
                StartBtn.interactable = true;
            }
        });
        GameStatus.inst.RouletteTicket = GameStatus.inst.RouletteTicket;
        rouletteRect = roulette.GetComponent<RectTransform>();
        animator = player.GetComponent<Animator>();
        openBtn.onClick.AddListener(clickOpen);
        StartBtn.onClick.AddListener(clickStart);
        StopBtn.onClick.AddListener(clickStop);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        rouletteRect.eulerAngles = UnityEngine.Vector3.zero;
        StartBtn.gameObject.SetActive(true);
        GameStatus.inst.RouletteTicket = GameStatus.inst.RouletteTicket;
        StopBtn.gameObject.SetActive(false);
        StopBtn.interactable = true;
    }

    private void Update()
    {
        spin();
    }

    void spin()
    {
        if (isSpin)
        {
            rouletteRect.eulerAngles = new UnityEngine.Vector3(0, 0, rouletteRect.eulerAngles.z + speed * Time.deltaTime);
        }
    }

    void clickOpen()
    {
        gameObject.SetActive(true);
        nowTotalGold = GameStatus.inst.TotalProdGold;
        string gold2 = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * 2).ToString());
        string gold5 = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * 5).ToString());
        string gold10 = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * 10).ToString());

        goldText2[0].text = gold2;
        goldText2[1].text = gold2;
        goldText2[2].text = gold2;

        goldText5[0].text = gold5;
        goldText5[1].text = gold5;
        goldText5[2].text = gold5;

        goldText10[0].text = gold10;
        goldText10[1].text = gold10;
        goldText10[2].text = gold10;
    }

    void clickStart()
    {
        isSpin = true;
        animator.SetBool("spin", isSpin);
        speed = setspd;
        StartBtn.gameObject.SetActive(false);
        StopBtn.gameObject.SetActive(true);
        closeBtn.interactable = false;
    }

    void clickStop()
    {
        StartCoroutine(stopRoulett(useCount));
        StopBtn.interactable = false;
    }

    IEnumerator stopRoulett(int count)
    {
        //보상 오브젝트 모두 꺼주기
        int ShowCount = ShowRewardParents.childCount;
        for (int iNum = 0; iNum < ShowCount; iNum++)
        {
            ShowRewardParents.GetChild(iNum).gameObject.SetActive(false);
        }


        List<UnityEngine.Vector2> ListRewardNum = new List<UnityEngine.Vector2>();
        //천장 도달시
        if (RouletteStack >= 24)
        {
            //획득못한 보상 모두 획득
            int bingoCount = bingoBoard.Count;
            for (int iNum = 0; iNum < bingoCount; iNum++)
            {
                if (bingoBoard[iNum] == false)
                {
                    ListRewardNum.Add(new UnityEngine.Vector2(iNum, 0));
                    bingoBoard[iNum] = true;
                    GetReward(iNum);
                }
            }
            //코인 감소
            GameStatus.inst.RouletteTicket--;
        }
        else
        {
            //갯수만큼 룰렛 돌린 후 저장
            for (int iNum = 0; iNum < count; iNum++)
            {
                //코인 감소
                GameStatus.inst.RouletteTicket--;
                if (GameStatus.inst.RouletteTicket < 0)
                {
                    GameStatus.inst.RouletteTicket = 0;
                    break;
                }

                RouletteStack++;
                if (RouletteStack >= 24)
                {
                    RouletteStack = 24;
                    break;
                }
                int rewardNumCount = ListRewardNum.Count;
                int value = UnityEngine.Random.Range(0, 8);
                int index = -1;
                //이미 획득했는지 확인
                for (int jNum = 0; jNum < rewardNumCount; jNum++)
                {
                    if (ListRewardNum[jNum].x == value)
                    {
                        index = jNum;
                        break;
                    }
                }
                //획득하지 않았다면 새로 추가
                if (index == -1)
                {
                    ListRewardNum.Add(new UnityEngine.Vector2(value, 0));
                    bingoBoard[value] = true;
                }
                //획득했다면 y값을 1올려줌
                else
                {
                    ListRewardNum[index] = new UnityEngine.Vector2(ListRewardNum[index].x, ListRewardNum[index].y + 1);
                }
                //보상 지급
                GetReward(value);

                //빙고 체크
                bool isBingo = false;
                int bingoCount = bingoBoard.Count;
                for (int jNum = 0; jNum < bingoCount; jNum++)
                {
                    if (bingoBoard[jNum] == false)
                    {
                        isBingo = false;
                        break;
                    }
                    isBingo = true;
                }
                if (isBingo)
                {
                    break;
                }
            }
        }

        //목표 회전값 계산
        float targetRot = 360 - 22.5f - 45 * ListRewardNum[0].x;
        rouletteRect.eulerAngles = new UnityEngine.Vector3(0, 0, targetRot);

        //점차 속도 감소
        while (speed > 0)
        {

            speed -= despd * Time.deltaTime;
            yield return null;
        }

        //룰렛 멈춘 후 해야될 일들

        //빙고판 획득처리
        int rewardCount = ListRewardNum.Count;
        for (int iNum = 0; iNum < rewardCount; iNum++)
        {
            //bingoBoard[(int)ListRewardNum[iNum].x] = true;
            bingoMask[(int)ListRewardNum[iNum].x].SetActive(true);
            if ((int)ListRewardNum[iNum].x < 4)
            {
                showBingoMask[(int)ListRewardNum[iNum].x].SetActive(true);
            }
            else
            {
                showBingoMask[(int)ListRewardNum[iNum].x + 1].SetActive(true);
            }
        }

        speed = 0;
        isSpin = false;
        animator.SetBool("spin", isSpin);
        ShowRewardParents.gameObject.SetActive(true);

        for (int iNum = 0; iNum < rewardCount; iNum++)
        {
            int index = (int)ListRewardNum[iNum].x;

            //스프라이트 교체
            Sprite sprite = UIManager.Instance.GetProdSprite((int)list_reward[index].GetRewardType());
            list_showImage[iNum].sprite = sprite;
            float ratio = sprite.bounds.size.x / sprite.bounds.size.y;
            list_showImage[iNum].rectTransform.sizeDelta = new UnityEngine.Vector2(ratio * 40, 40);

            //획득 갯수 텍스트 수정
            switch (list_reward[index].GetRewardType())
            {
                case ProductTag.Gold:
                    list_CountText[iNum].text = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * list_reward[index].GetCount()).ToString());
                    break;
                case ProductTag.Star:
                    list_CountText[iNum].text = CalCulator.inst.StringFourDigitAddFloatChanger(list_reward[index].GetCount().ToString());
                    break;
                case ProductTag.Ruby:
                    list_CountText[iNum].text = list_reward[index].GetCount().ToString();
                    break;
            }

            //중복 획득 갯수 텍스트 수정
            if (ListRewardNum[iNum].y == 0)
            {
                list_GetText[iNum].text = $"Get!";
            }
            else
            {
                list_GetText[iNum].text = $"x{ListRewardNum[iNum].y + 1} Get!";
            }

            ShowRewardParents.GetChild(iNum).gameObject.SetActive(true);
        }



        //빙고창 끌때까지 대기
        while (ShowRewardParents.gameObject.activeSelf)
        {
            yield return null;
        }
        yield return StartCoroutine(checkBingo());
        closeBtn.interactable = true;
        StopBtn.interactable = true;
        StopBtn.gameObject.SetActive(false);
        StartBtn.gameObject.SetActive(true);
    }

    void GetReward(int value)
    {
        switch (value)
        {
            case 0:
                GameStatus.inst.PlusRuby(100);
                break;
            case 1:
                GameStatus.inst.PlusStar("10");
                break;
            case 2:
                GameStatus.inst.PlusGold($"{nowTotalGold * 2}");
                break;
            case 3:
                GameStatus.inst.PlusRuby(200);
                break;
            case 4:
                GameStatus.inst.PlusStar("50");
                break;
            case 5:
                GameStatus.inst.PlusGold($"{nowTotalGold * 5}");
                break;
            case 6:
                GameStatus.inst.PlusStar("100");
                break;
            case 7:
                GameStatus.inst.PlusGold($"{nowTotalGold * 10}");
                break;
        }
    }

    bool[] horizontalBingo = new bool[3];
    bool[] verticalBingo = new bool[3];
    bool[] crossBingo = new bool[2];

    IEnumerator checkBingo()
    {
        List<int> bingoList = new List<int>();
        int preStack = BingoStack;
        //가로빙고
        if (horizontalBingo[0] == false && bingoBoard[0] && bingoBoard[1] && bingoBoard[2])
        {
            horizontalBingo[0] = true;
            bingoList.Add(0);
            bingoList.Add(1);
            bingoList.Add(2);
            BingoStack++;
        }

        if (horizontalBingo[1] == false && bingoBoard[3] && bingoBoard[4])
        {
            horizontalBingo[1] = true;
            bingoList.Add(3);
            bingoList.Add(4);
            bingoList.Add(5);
            BingoStack++;
        }

        if (horizontalBingo[2] == false && bingoBoard[5] && bingoBoard[6] && bingoBoard[7])
        {
            horizontalBingo[2] = true;
            bingoList.Add(6);
            bingoList.Add(7);
            bingoList.Add(8);
            BingoStack++;
        }

        //세로빙고
        if (verticalBingo[0] == false && bingoBoard[0] && bingoBoard[3] && bingoBoard[5])
        {
            verticalBingo[0] = true;
            bingoList.Add(0);
            bingoList.Add(3);
            bingoList.Add(6);
            BingoStack++;
        }

        if (verticalBingo[1] == false && bingoBoard[1] && bingoBoard[6])
        {
            verticalBingo[1] = true;
            bingoList.Add(1);
            bingoList.Add(4);
            bingoList.Add(7);
            BingoStack++;
        }

        if (verticalBingo[2] == false && bingoBoard[2] && bingoBoard[4] && bingoBoard[7])
        {
            verticalBingo[2] = true;
            bingoList.Add(2);
            bingoList.Add(5);
            bingoList.Add(8);
            BingoStack++;
        }

        //대각빙고
        if (crossBingo[0] == false && bingoBoard[0] && bingoBoard[7])
        {
            crossBingo[0] = true;
            bingoList.Add(0);
            bingoList.Add(4);
            bingoList.Add(8);
            BingoStack++;
        }

        if (crossBingo[1] == false && bingoBoard[2] && bingoBoard[5])
        {
            crossBingo[1] = true;
            bingoList.Add(2);
            bingoList.Add(4);
            bingoList.Add(6);
            BingoStack++;
        }

        //빙고 채운게 있는지 확인
        if (bingoList.Count == 0)
        {
            yield break;
        }

        //빙고 달성 이펙트
        showBingo.SetActive(true);
        int clearBingoListCount = bingoList.Count;
        for (int iNum = 0; iNum < clearBingoListCount; iNum++)
        {
            showBingoMask[bingoList[iNum]].GetComponent<Animator>().Play("BingoGetEffect", -1, 0);
            yield return new WaitForSeconds(0.2f);
        }
        showBingoCloseBtn.interactable = true;

        ////빙고 달성창 꺼질 때까지 대기(꺼지는 함수는 애니메이터에 연결돼있음)
        //while (showBingo.activeSelf == true)
        //{
        //    yield return null;
        //}

        //보상창 자식 모두 꺼주기
        int rewardCount = ShowRewardParents.childCount;
        for (int iNum = 0; iNum < rewardCount; iNum++)
        {
            ShowRewardParents.GetChild(iNum).gameObject.SetActive(false);
        }

        int jNum = 0;
        //빙고달성 보상 지급
        for (int iNum = preStack; iNum < BingoStack; iNum++)
        {

            Sprite sprite = null;
            switch (iNum)
            {
                case 0:
                    GameStatus.inst.PlusStar("20");
                    sprite = UIManager.Instance.GetProdSprite(1);
                    list_CountText[jNum].text = "20";

                    break;
                case 1:
                    GameStatus.inst.PlusGold($"{nowTotalGold * 5}");
                    sprite = UIManager.Instance.GetProdSprite(0);
                    list_CountText[jNum].text = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * 5).ToString());
                    break;
                case 2:
                    GameStatus.inst.PlusStar("100");
                    sprite = UIManager.Instance.GetProdSprite(1);
                    list_CountText[jNum].text = "100";
                    break;
                case 3:
                    GameStatus.inst.PlusGold($"{nowTotalGold * 10}");
                    sprite = UIManager.Instance.GetProdSprite(0);
                    list_CountText[jNum].text = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * 10).ToString());
                    break;
                case 4:
                    GameStatus.inst.PlusStar("200");
                    sprite = UIManager.Instance.GetProdSprite(1);
                    list_CountText[jNum].text = "200";
                    break;
                case 5:
                    GameStatus.inst.PlusGold($"{nowTotalGold * 15}");
                    sprite = UIManager.Instance.GetProdSprite(0);
                    list_CountText[jNum].text = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * 15).ToString());
                    break;
                case 6:
                    GameStatus.inst.PlusRuby(200);
                    sprite = UIManager.Instance.GetProdSprite(2);
                    list_CountText[jNum].text = "200";
                    break;
                case 7:
                    GameStatus.inst.PlusRuby(300);
                    sprite = UIManager.Instance.GetProdSprite(2);
                    list_CountText[jNum].text = "300";
                    break;
            }
            //스프라이트 교체
            list_showImage[jNum].sprite = sprite;
            float ratio = sprite.bounds.size.x / sprite.bounds.size.y;
            list_showImage[jNum].rectTransform.sizeDelta = new UnityEngine.Vector2(ratio * 40, 40);

            //텍스트 교체
            list_GetText[jNum].text = $"Get!";

            //오브젝트 활성화
            ShowRewardParents.GetChild(jNum).gameObject.SetActive(true);

            jNum++;
        }

        ShowRewardParents.gameObject.SetActive(true);

        //올빙고 달성했는지 확인
        int bingoCount = bingoBoard.Count;
        for (int iNum = 0; iNum < bingoCount; iNum++)
        {
            if (bingoBoard[iNum] == false)
            {
                yield break;
            }
        }

        //다 채워져 있으면 빙고판 리셋
        for (int iNum = 0; iNum < bingoCount; iNum++)
        {
            bingoBoard[iNum] = false;
            bingoMask[iNum].SetActive(false);
            showBingoMask[iNum].SetActive(false);
        }
        showBingoMask[4].SetActive(true);
        RouletteStack = 0;
        BingoStack = 0;
        horizontalBingo[0] = false;
        horizontalBingo[1] = false;
        horizontalBingo[2] = false;
        verticalBingo[0] = false;
        verticalBingo[1] = false;
        verticalBingo[2] = false;
        crossBingo[0] = false;
        crossBingo[1] = false;
    }


    public void ClickUseCount(int index)
    {
        list_countBtnImage[countBtnNum].sprite = UIManager.Instance.GetGYBtnSprite(0);
        countBtnNum = index;
        list_countBtnImage[countBtnNum].sprite = UIManager.Instance.GetGYBtnSprite(1);

        switch (index)
        {
            case 0:
                useCount = 1;
                break;
            case 1:
                useCount = 10;
                break;
            case 2:
                useCount = 25;
                break;
        }
    }
}
