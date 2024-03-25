using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] int Number;//퀘스트 단계
    [SerializeField] int Lv;//퀘스트 업그레이드 레벨
    [SerializeField] float atkGrowthRate;//공격력 성장률
    [SerializeField] float atkpowNumRate;//초기공격력지수

    BigInteger baseCost;//초기 비용
    BigInteger nextCost;//다음레벨 비용
    BigInteger resultPowNum;//보정값 빅인트로 전환
    BigInteger atk;//공격력

    private BigInteger Atk
    {
        set
        {
            UIManager.Instance.TotalAtk -= atk;
            atk = value;
            UIManager.Instance.TotalAtk += atk;
        }
    }

    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI upAtkText;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] TextMeshProUGUI totalAtkText;
    [SerializeField] GameObject objBtn;

    void Start()
    {
        initValue();
    }

    void Update()
    {

    }

    void initValue()//초기값 설정
    {
        float powNum = 0;
        for (int iNum = 0; iNum <= Number; iNum++)// 단계별 지수 설정
        {
            powNum += atkpowNumRate * iNum;
        }
        Lv = Number * 5;

        int intpowNum = (int)Mathf.Floor(powNum);
        float pracpowNum = powNum - intpowNum;
        BigInteger temp = BigInteger.Pow(10, intpowNum);
        float temp2 = Mathf.Pow(10, pracpowNum);
        resultPowNum = BigInteger.Multiply(temp, (BigInteger)temp2);

        baseCost = multiplyBigInteger(calculatePow(atkGrowthRate, Lv), 1.67f);
        setNextCost();

        setText();
    }

    private void setText()
    {
        priceText.text = "가격 : " + CalCulator.inst.StringFourDigitChanger(nextCost.ToString());
        LvText.text = $"Lv : {Lv - Number * 5} / 5";
        upAtkText.text = "+" + CalCulator.inst.StringFourDigitChanger((BigInteger.Multiply(resultPowNum, Lv + 1) - BigInteger.Multiply(resultPowNum, Lv)).ToString());
        totalAtkText.text = "공격력 : " + CalCulator.inst.StringFourDigitChanger(atk.ToString());
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
        Lv++;
        Atk = BigInteger.Multiply(resultPowNum, Lv);
        setNextCost();
        setText();
        if (Lv - Number * 5 >= 5)
        {
            objBtn.SetActive(false);
        }
    }

    private void setNextCost()
    {
        nextCost = multiplyBigInteger(calculatePow(atkGrowthRate, Lv), 1.67f) * resultPowNum;
    }

    public void clickWeaponImage(int WeaponNum)
    {
        UIManager.Instance.EquipWeaponNum = WeaponNum;
        Debug.Log(UIManager.Instance.EquipWeaponNum);
    }
}
