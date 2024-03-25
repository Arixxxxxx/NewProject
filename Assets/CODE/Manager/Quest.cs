using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;

public class Quest : MonoBehaviour
{
    [Tooltip("퀘스트단계")]
    [SerializeField] int Number;//퀘스트 단계
    [Tooltip("성장률")]
    [SerializeField] float growthRate;//성장률
    [Tooltip("초기 생산량과 초기비용의 비율 낮을수록 초기가격이 비싸짐")]
    [SerializeField] float initalProdRate;//초기 생산량과 초기비용의 비율 낮을수록 초기가격이 비싸짐
    [Tooltip("기초 생산량")]
    [SerializeField] float baseProd;//기초 생산량
    [Tooltip("초기생산량 지수")]
    [SerializeField] float powNumRate;//초기생산량지수
    int Lv;//퀘스트 업그레이드 레벨
    int LvCur = 1; //레벨보정
    int itemCur = 1; //아이템보정
    float powNum;//단계별 지수

    BigInteger baseCost;//초기 비용
    BigInteger nextCost;//다음레벨 비용
    BigInteger initialProd;//초기 생산량
    BigInteger totalProd;//총 생산량
    private BigInteger TotalProd
    {
        set
        {
            UIManager.Instance.TotalProdGold -= totalProd;
            totalProd = value;
            UIManager.Instance.TotalProdGold += totalProd;
        }
    }

    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI upGoldText;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] TextMeshProUGUI totalGoldText;

    void Start()
    {
        initValue();
        UIManager.Instance.OnBuyCountChanged.AddListener(Test_OnCountChanged);
    }

    void initValue()//초기값 설정
    {
        powNum = 0;
        for (int iNum = 0; iNum <= Number; iNum++)// 단계별 지수 설정
        {
            powNum += powNumRate * iNum;
        }
        BigInteger resultPowNum = CalCulator.inst.CalculatePow(10,powNum);

        initialProd = BigInteger.Multiply((BigInteger)baseProd, resultPowNum);

        baseCost = BigInteger.Multiply(initialProd, (BigInteger)initalProdRate);
        setNextCost();
        TotalProd = initialProd * Lv;
        setText();
    }

    private void setText()
    {
        priceText.text = "가격 : " + CalCulator.inst.StringFourDigitChanger(nextCost.ToString());
        upGoldText.text ="+" + CalCulator.inst.StringFourDigitChanger($"{initialProd * (Lv + UIManager.Instance.BuyCount) - initialProd * (Lv)}");
        LvText.text = "Lv : " + CalCulator.inst.StringFourDigitChanger(Lv.ToString());
        totalGoldText.text = "Gps : " + CalCulator.inst.StringFourDigitChanger($"{totalProd}");
    }

    public void ClickBuy()
    {
        Lv += UIManager.Instance.BuyCount;
        if (Lv >= 25 * LvCur)
        {
            LvCur *= 2;
        }
        TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
        setNextCost();
        setText();
    }

    private void setNextCost()
    {
        int buycount = UIManager.Instance.BuyCount;
        if (buycount != 0)//max가 아닐때
        {
            nextCost = baseCost * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buycount) - 1) / (growthRate - 1)));
        }
    }

    private void Test_OnCountChanged()
    {
        setNextCost();
        setText();
    }
}
