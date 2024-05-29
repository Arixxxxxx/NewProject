using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class Quest : MonoBehaviour
{
    [Header("퀘스트단계")]
    int Number;//퀘스트 단계
    [Header("비용 성장률")]
    [SerializeField] float growthRate;//성장률
    [Header("초기 생산량과 초기비용의 비율 낮을수록 초기가격이 비싸짐")]
    [SerializeField] float initalProdRate;//초기 생산량과 초기비용의 비율
    [Header("기초 생산량")]
    [SerializeField] float baseProd;//기초 생산량
    [Header("단계별 상승량 지수")]
    [SerializeField] float powNumRate;//단계별 상승량 지수
    [SerializeField] GameObject objMask;//단계별 상승량 지수
    int lv;//퀘스트 업그레이드 레벨
    int Lv
    {
        get => lv;
        set
        {
            lv = value;
            GameStatus.inst.SetAryQuestLv(Number, value);
            TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
            if (nextRelease == false && lv >= 1)
            {
                UIManager.Instance.QeustUpComplete(Number);
                nextRelease = true;
            }
        }
    }
    int LvCur = 1; //레벨보정
    float itemCur = 1; //아이템보정
    float powNum;//단계별 지수
    int buyCount = 1;
    bool nextRelease = false;

    BigInteger baseCost;//초기 비용
    BigInteger nextCost;//다음레벨 비용
    BigInteger initialProd;//초기 생산량
    BigInteger totalProd;//총 생산량
    private BigInteger TotalProd
    {
        set
        {
            GameStatus.inst.TotalProdGold -= totalProd;
            totalProd = value;
            GameStatus.inst.TotalProdGold += totalProd;
        }
    }
    [Space]
    [Space]
    [SerializeField] Button UpBtn;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI upGoldText;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] TextMeshProUGUI totalGoldText;

    // 자동 초기화

    void Start()
    {

    }

    public void initQuest()
    {
        Number = transform.GetSiblingIndex();
        Lv = GameStatus.inst.GetAryQuestLv(Number);
        initValue();

        //자동초기화
        transform.Find("NameText").GetComponent<TMP_Text>().text = Set_QuestName(Number);
        transform.Find("IMG_LayOut/IMG").GetComponent<Image>().sprite = SpriteResource.inst.Get_QuestIcon(Number);

        UIManager.Instance.OnBuyCountChanged.AddListener(_OnCountChanged);
        UIManager.Instance.OnBuyCountChanged.AddListener(SetbtnActive);
        UIManager.Instance.QuestReset.AddListener(resetQuest);
        GameStatus.inst.OnGoldChanged.AddListener(() =>
        {
            if (UIManager.Instance.QuestBuyCountBtnNum == 3)//max일 때
            {
                setNextCost();
                setText();
            }
            SetbtnActive();
        });
        GameStatus.inst.OnPercentageChanged.AddListener(_OnItemPercentChanged);
        GameStatus.inst.OnPercentageChanged.AddListener(setItemCur);
        UpBtn.onClick.AddListener(() => { AudioManager.inst.PlaySFX(1); });
    }

    void initValue()//초기값 설정
    {
        powNum = powNumRate * (Number * (Number + 1)) / 2;// 단계별 지수 설정
        BigInteger resultPowNum = CalCulator.inst.CalculatePow(10, powNum);

        initialProd = BigInteger.Multiply((BigInteger)baseProd, resultPowNum);

        baseCost = BigInteger.Multiply(initialProd, (BigInteger)initalProdRate);
        setNextCost();
        TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
        setText();
    }

    private void setText()
    {
        upGoldText.text = "LV별 증가치 +" + CalCulator.inst.StringFourDigitAddFloatChanger($"{initialProd * (Lv + buyCount) - initialProd * (Lv)}");
        priceText.text = CalCulator.inst.StringFourDigitAddFloatChanger(nextCost.ToString());

        LvText.text = "Lv : " + CalCulator.inst.StringFourDigitChanger(Lv.ToString());
        totalGoldText.text = $"퀘스트 골드 증가량 : {CalCulator.inst.StringFourDigitAddFloatChanger($"{totalProd}")} / 초";
    }

    public void ClickBuy()
    {
        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
        if (haveGold >= nextCost)
        {
            Lv += buyCount;
            MissionData.Instance.SetWeeklyMission("퀘스트 레벨업", buyCount);
            if (Lv >= 25 * LvCur)
            {
                LvCur *= 2;
            }
            //TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
            GameStatus.inst.MinusGold(nextCost.ToString());
            UIManager.Instance.OnBuyCountChanged?.Invoke();
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
        }
    }

    void setItemCur()
    {
        itemCur = GameStatus.inst.GetAryPercent((int)ItemTag.QuestGold);
        itemCur = itemCur / 100 + 1;
    }

    private void setNextCost()
    {
        int btnnum = UIManager.Instance.QuestBuyCountBtnNum;
        float pricediscount = GameStatus.inst.GetAryPercent((int)ItemTag.QuestDiscount);
        if (btnnum != 3)//max가 아닐때
        {
            nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(baseCost, 1 - (pricediscount / 100)) * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buyCount) - 1) / (growthRate - 1)));
        }
        else//max일 때
        {
            buyCount = 1;
            BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
            setNextCost(buyCount);
            while (haveGold >= nextCost)
            {
                buyCount++;
                setNextCost(buyCount);
                if (buyCount >= 100)
                {
                    break;
                }

            }
            buyCount--;
            nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(baseCost, 1 - (pricediscount / 100)) * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buyCount) - 1) / (growthRate - 1)));
        }
    }

    private void setNextCost(int count)
    {
        float pricediscount = GameStatus.inst.GetAryPercent((int)ItemTag.QuestDiscount);
        nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(baseCost, 1 - (pricediscount / 100)) * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, count) - 1) / (growthRate - 1)));
    }

    private void _OnCountChanged()
    {
        buyCount = UIManager.Instance.QuestBuyCount;
        setNextCost();
        setText();
    }

    private void _OnItemPercentChanged()
    {
        itemCur = GameStatus.inst.GetAryPercent((int)NormalRelicTag.QuestGold);
        TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
        setText();
    }

    private void SetbtnActive()
    {
        BigInteger havegold = BigInteger.Parse(GameStatus.inst.Gold);
        if (havegold < nextCost && UpBtn.interactable == true || nextCost == 0)
        {
            UpBtn.interactable = false;
        }
        else if (objMask.activeSelf == false && UpBtn.interactable == false && havegold >= nextCost)
        {
            UpBtn.interactable = true;
        }
        else if (objMask.activeSelf && UpBtn.interactable == true)
        {
            UpBtn.interactable = false;
        }
    }

    void resetQuest()
    {
        Lv = 0;
        nextRelease = false;

        if (Number != 0)
        {
            SetMaskActive(true);
        }
        setNextCost();
        setText();
        SetbtnActive();
    }

    public int GetLv()
    {
        return Lv;
    }

    public void SetMaskActive(bool value)
    {
        objMask.SetActive(value);
    }

    private string Set_QuestName(int QeustNumber)
    {
        string[] questName =
            {
            "등산 하기", //1
            "동전 줍기", //2
            "항아리 만들기", //3
            "허브차 마시기", //4
            "판다 응가 치우기", //5
            "전단지 나눠주기", //6
            "선물 포장하기", //7
            "편의점 알바하기", //8
            "판다동상 만들기", //9
            "그림 그리기", //11
            "뒷산 제초작업", //12
            "자전거 라이딩 ", //13
            "폭탄 제조하기", //14
            "캠핑장 관리하기", //15
            "바다낚시 하기", //16
            "부서진 수레 수리하기", //17
            "양파 다듬기", //18
            "마당 청소하기", //19
            "후임 전투화 물광내기", //20
            "PX에서 고참 빵사오기", //21
            "판다 쓰다듬기", //22
            "고백하기", //23
        };

        return questName[QeustNumber];
    }
}
