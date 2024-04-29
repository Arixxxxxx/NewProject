using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Numerics;
using TMPro;

public class Roulette : MonoBehaviour
{
    [SerializeField] float setspd;
    [SerializeField] float despd;
    [SerializeField] int rouletteTicket;
    int RouletteTicket
    {
        get => rouletteTicket;
        set
        {
            rouletteTicket = value;
            ticketText.text = rouletteTicket.ToString();
            if (rouletteTicket <= 0)
            {
                StartBtn.interactable = false;
            }
        }
    }
    [SerializeField] int RouletteStack;
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
    GameObject showBingo;
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
    int useCount = 1;
    bool isSpin;

    bool[] bingoBoard = new bool[8];
    List<GameObject> bingoMask = new List<GameObject>();


    private void Start()
    {
        Transform canvas = GameObject.Find("---[UI Canvas]").transform;
        roulette = transform.Find("BackGround/Roulette").GetComponent<Image>();
        player = transform.Find("BackGround/Player").GetComponent<Image>();
        openBtn = canvas.Find("RouletteBtn").GetComponent<Button>();
        StartBtn = transform.Find("BackGround/StartBtn").GetComponent<Button>();
        StopBtn = transform.Find("BackGround/StopBtn").GetComponent<Button>();
        closeBtn = transform.Find("BackGround/CloseBtn").GetComponent<Button>();
        goldText2[0] = transform.Find("BackGround/Roulette/Gold2/Text (TMP)").GetComponent<TMP_Text>();
        goldText2[1] = transform.Find("BackGround/bingo/Coin2/Text (TMP)").GetComponent<TMP_Text>();
        goldText2[2] = transform.Find("BackGround/ShowBingo/Bingo/Coin2/Text (TMP)").GetComponent<TMP_Text>();

        goldText5[0] = transform.Find("BackGround/Roulette/Gold5/Text (TMP)").GetComponent<TMP_Text>();
        goldText5[1] = transform.Find("BackGround/bingo/Coin5/Text (TMP)").GetComponent<TMP_Text>();
        goldText5[2] = transform.Find("BackGround/ShowBingo/Bingo/Coin5/Text (TMP)").GetComponent<TMP_Text>();

        goldText10[0] = transform.Find("BackGround/Roulette/Gold10/Text (TMP)").GetComponent<TMP_Text>();
        goldText10[1] = transform.Find("BackGround/bingo/Coin10/Text (TMP)").GetComponent<TMP_Text>();
        goldText10[2] = transform.Find("BackGround/ShowBingo/Bingo/Coin10/Text (TMP)").GetComponent<TMP_Text>();

        ticketText = transform.Find("BackGround/RouletteTicket/count").GetComponent<TMP_Text>();
        showBingo = transform.Find("BackGround/ShowBingo").gameObject;
        list_countBtnImage[0] = transform.Find("BackGround/CountBtn/Button").GetComponent<Image>();
        list_countBtnImage[1] = transform.Find("BackGround/CountBtn/Button (1)").GetComponent<Image>();
        list_countBtnImage[2] = transform.Find("BackGround/CountBtn/Button (2)").GetComponent<Image>();

        //빙고판 초기화
        BingoParents = transform.Find("BackGround/bingo");
        int BingoCount = BingoParents.childCount;
        for (int iNum = 0; iNum < BingoCount; iNum++)
        {
            bingoMask.Add(BingoParents.GetChild(iNum).Find("Mask").gameObject);
        }
        bingoMask.RemoveAt(4);

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
        RouletteTicket = RouletteTicket;
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
        StartBtn.interactable = true;
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
            int bingoCount = bingoBoard.Length;
            for (int iNum = 0; iNum < bingoCount; iNum++)
            {
                if (bingoBoard[iNum] == false)
                {
                    ListRewardNum.Add(new UnityEngine.Vector2(iNum, 0));

                    GetReward(iNum);
                }
            }
            //코인 감소
            RouletteTicket--;
        }
        else
        {
            //갯수만큼 룰렛 돌린 후 저장
            for (int iNum = 0; iNum < count; iNum++)
            {
                //코인 감소
                RouletteTicket--;
                if (RouletteTicket < 0)
                {
                    RouletteTicket = 0;
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
                int bingoCount = bingoBoard.Length;
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
                GameStatus.inst.Ruby += 100;
                break;
            case 1:
                GameStatus.inst.PlusStar("100");
                break;
            case 2:
                GameStatus.inst.PlusGold($"{nowTotalGold * 2}");
                break;
            case 3:
                GameStatus.inst.Ruby += 200;
                break;
            case 4:
                GameStatus.inst.PlusStar("1000");
                break;
            case 5:
                GameStatus.inst.PlusGold($"{nowTotalGold * 5}");
                break;
            case 6:
                GameStatus.inst.PlusStar("10000");
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
        //가로빙고
        if (horizontalBingo[0] && bingoBoard[0] && bingoBoard[1] && bingoBoard[2])
        {
            horizontalBingo[0] = true;
        }

        if (horizontalBingo[1] && bingoBoard[3] && bingoBoard[4])
        {
            horizontalBingo[1] = true;
        }

        if (horizontalBingo[2] && bingoBoard[5] && bingoBoard[6] && bingoBoard[7])
        {
            horizontalBingo[2] = true;
        }

        //세로빙고
        if (verticalBingo[0] && bingoBoard[0] && bingoBoard[3] && bingoBoard[5])
        {
            verticalBingo[0] = true;
        }

        if (verticalBingo[1] && bingoBoard[1] && bingoBoard[6])
        {
            verticalBingo[1] = true;
        }

        if (verticalBingo[2] && bingoBoard[2] && bingoBoard[4] && bingoBoard[7])
        {
            verticalBingo[2] = true;
        }

        //대각빙고
        if (crossBingo[0] && bingoBoard[0] && bingoBoard[7])
        {
            crossBingo[0] = true;
        }

        if (crossBingo[1] && bingoBoard[2] && bingoBoard[5])
        {
            crossBingo[1] = true;
        }

        //빙고 다 채운지 확인
        int count = bingoBoard.Length;
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (bingoBoard[iNum] == false)
            {
                yield break;
            }
        }
        //빙고 달성창 꺼질 때까지 대기(꺼지는 함수는 애니메이터에 연결돼있음)
        showBingo.SetActive(true);
        while (showBingo.activeSelf == true)
        {
            yield return null;
        }
        //다 채워져 있으면 빙고판 리셋
        for (int iNum = 0; iNum < count; iNum++)
        {
            bingoBoard[iNum] = false;
            bingoMask[iNum].SetActive(false);
        }
        RouletteStack = 0;
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
        list_countBtnImage[countBtnNum].sprite = UIManager.Instance.GetBtnSprite(0);
        countBtnNum = index;
        list_countBtnImage[countBtnNum].sprite = UIManager.Instance.GetBtnSprite(1);

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
