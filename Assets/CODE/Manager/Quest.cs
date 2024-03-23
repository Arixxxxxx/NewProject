using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;

public class Quest : MonoBehaviour
{
    [SerializeField] int Number;//퀘스트 단계
    [SerializeField] int Lv;//퀘스트 업그레이드 레벨
    [SerializeField] float growthRate;//성장률
    [SerializeField] float initalProdRate;//초기 생산량과 초기비용의 비율 낮을수록 초기가격이 비싸짐
    [SerializeField] float baseProd;//기초 생산량
    [SerializeField] float powNumRate;//초기생산량지수
    int LvCur = 1; //레벨보정
    int itemCur = 1; //아이템보정
    BigInteger baseCost;//초기 비용
    BigInteger nextCost;//다음레벨 비용
    BigInteger initialProd;//초기 생산량
    BigInteger totalProd;//총 생산량
    float powNum;//단계별 지수

    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI upGoldText;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] TextMeshProUGUI totalGoldText;

    void Start()
    {
        initValue();
        UIManager.Instance.OnBuyCountChanged.AddListener(Test_OnCountChanged);
    }

    void Update()
    {

    }

    void initValue()//초기값 설정
    {
        powNum = 0;
        for (int iNum = 0; iNum <= Number; iNum++)// 단계별 지수 설정
        {
            powNum += 0.3f * iNum;
        }
        int intpowNum = (int)Mathf.Floor(powNum);
        float pracpowNum = powNum - intpowNum;
        BigInteger temp = BigInteger.Pow(10, intpowNum);
        float temp2 = Mathf.Pow(10, pracpowNum);
        BigInteger resultPowNum = BigInteger.Multiply(temp, (BigInteger)temp2);
        initialProd = BigInteger.Multiply((BigInteger)1.67f, resultPowNum);

        baseCost = BigInteger.Multiply(initialProd, (BigInteger)initalProdRate);
        setNextCost();
        totalProd = initialProd * Lv;
        setText();
    }

    private void setText()
    {
        priceText.text = "가격 : " + CalCulator.inst.StringFourDigitChanger(nextCost.ToString());
        upGoldText.text ="+" + CalCulator.inst.StringFourDigitChanger($"{initialProd * (Lv + UIManager.Instance.BuyCount) - initialProd * (Lv)}");
        LvText.text = "Lv : " + CalCulator.inst.StringFourDigitChanger(Lv.ToString());
        totalGoldText.text = "Gps : " + CalCulator.inst.StringFourDigitChanger($"{totalProd}");
    }

    private BigInteger calculatePow(float value, int pow)// biginteger로 float 제곱 계산기
    {
        string strValue = value.ToString();
        int count = strValue.Length;
        int pointNum = 0;
        char point = '.';
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (Equals(strValue[iNum], point))
            {
                pointNum = iNum;
                break;
            }
        }
        if (pointNum != 0)
        {
            int pracCount = strValue.Length - pointNum - 1;
            float powpracCount = Mathf.Pow(10, pracCount);
            BigInteger intvalue = (BigInteger)(value * powpracCount);
            BigInteger powResult = BigInteger.Pow(intvalue, pow);
            BigInteger Result = BigInteger.Divide(powResult, BigInteger.Pow((BigInteger)powpracCount, pow));
            return Result;
        }
        else
        {
            return BigInteger.Parse(strValue);
        }
    }

    private BigInteger multiplyBigInteger(BigInteger Ivalue, float fvalue)//BigInteger * float 계산기
    {
        string strValue = fvalue.ToString();
        int count = strValue.Length;
        int pointNum = 0;
        char point = '.';
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (Equals(strValue[iNum], point))
            {
                pointNum = iNum;
                break;
            }
        }
        if (pointNum != 0)
        {
            int pracCount = strValue.Length - pointNum - 1;
            float powpracCount = Mathf.Pow(10, pracCount);
            BigInteger intvalue = (BigInteger)(fvalue * powpracCount);
            BigInteger result = BigInteger.Multiply(Ivalue, intvalue);
            result = BigInteger.Divide(result, (BigInteger)powpracCount);
            return result;
        }
        else
        {
            BigInteger result = BigInteger.Multiply(Ivalue, (BigInteger)fvalue);
            return result;
        }
    }

    private BigInteger devideBigInteger(BigInteger ivalue, float fvalue)//BigInteger / float 계산기
    {
        string strValue = fvalue.ToString();
        int count = strValue.Length;
        int pointNum = 0;
        char point = '.';
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (Equals(strValue[iNum], point))
            {
                pointNum = iNum;
                break;
            }
        }
        if (pointNum != 0)
        {
            int pracCount = strValue.Length - pointNum - 1;
            float powpracCount = Mathf.Pow(10, pracCount);
            BigInteger intvalue = (BigInteger)(fvalue * powpracCount);
            BigInteger result = BigInteger.Divide(ivalue, intvalue);
            result = BigInteger.Multiply(result, (BigInteger)powpracCount);
            return result;
        }
        else
        {
            BigInteger result = BigInteger.Divide(ivalue, (BigInteger)fvalue);
            return result;
        }
    }

    public void ClickBuy()
    {
        Lv += UIManager.Instance.BuyCount;
        if (Lv >= 25 * LvCur)
        {
            LvCur *= 2;
        }
        totalProd = multiplyBigInteger(initialProd, Lv * LvCur * itemCur);
        setNextCost();
        setText();
    }

    private void setNextCost()
    {
        int buycount = UIManager.Instance.BuyCount;
        if (buycount != 0)//max가 아닐때
        {
            nextCost = baseCost * (calculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buycount) - 1) / (growthRate - 1)));
        }
    }

    private void Test_OnCountChanged()
    {
        setNextCost();
        setText();
    }
}
