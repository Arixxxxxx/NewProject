using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Numerics;
using System;
using static UnityEngine.Rendering.DebugUI;

public class UIStatus : MonoBehaviour
{
    [SerializeField] int Number;
    [SerializeField] int Lv;
    int LvCur = 1;
    int itemCur = 1;
    [SerializeField] BigInteger baseCost;//초기 비용
    [SerializeField] BigInteger nextCost;//다음레벨 비용
    [SerializeField] float growthRate;//성장률
    [SerializeField] BigInteger initialProd;//초기 생산량
    [SerializeField] float powNum;//단계별 지수
    [SerializeField] BigInteger totalProd;//총 생산량
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI upGoldText;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] TextMeshProUGUI totalGoldText;

    // Start is called before the first frame update
    void Start()
    {
        initValue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void initValue()
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

        baseCost = BigInteger.Multiply(initialProd, (BigInteger)2.56f);
        nextCost = BigInteger.Multiply(baseCost, BigInteger.Pow((BigInteger)growthRate, Lv));
        totalProd = initialProd * Lv;
        setText();
    }

    private void setText()
    {
        priceText.text = CalCulator.inst.StringFourDigitChanger(nextCost.ToString());
        upGoldText.text = CalCulator.inst.StringFourDigitChanger($"{initialProd * (Lv + 1) - initialProd * (Lv)}");
        LvText.text = CalCulator.inst.StringFourDigitChanger(Lv.ToString());
        totalGoldText.text ="Gps : " + CalCulator.inst.StringFourDigitChanger($"{totalProd}");
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

    private BigInteger multiplyBigInteger(BigInteger Ivalue, float fvalue)
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

    public void ClickBuy()
    {
        Lv++;
        if (Lv >= 25 * LvCur)
        {
            LvCur *= 2;
        }
        totalProd = multiplyBigInteger(initialProd, Lv * LvCur * itemCur);
        Debug.Log(totalProd);
        nextCost = baseCost * calculatePow(growthRate, Lv);
        setText();
    }
}
