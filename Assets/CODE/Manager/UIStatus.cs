using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Numerics;
using System;


public class UIStatus : MonoBehaviour
{
    [SerializeField] int Number;
    [SerializeField] int Lv;
    [SerializeField] float baseCost;//�ʱ� ���
    [SerializeField] float nextCost;//�������� ���
    [SerializeField] float growthRate;//�����
    [SerializeField] float initialProd;//�ʱ� ���귮
    [SerializeField] float powNum;//�ܰ躰 ����
    [SerializeField] float totalProd;//�� ���귮
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
        for (int iNum = 0; iNum <= Number; iNum++)// �ܰ躰 ���� ����
        {
            powNum +=  0.5f * iNum;
        }
        initialProd = 1.67f * Mathf.Pow(10, powNum);
        baseCost = initialProd * 2.56f;
        nextCost = baseCost * Mathf.Pow(growthRate, Lv);
        totalProd = initialProd * Lv;
        setText();
    }

    private void setText()
    {
        priceText.text = nextCost.ToString();
        upGoldText.text = $"{initialProd * (Lv + 1) - initialProd * (Lv + 1)}";
        LvText.text = Lv.ToString();
        totalGoldText.text = $"GPS : {totalProd}";
    }

    private string calculatePow(float value, int pow)
    {
        string strValue = value.ToString();
        int count = strValue.Length;
        int pointNum = 0;
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (Equals(strValue[iNum],"."))
            {
                pointNum = iNum;
                break;
            }
        }
        if (pointNum != 0)
        {
            int countValue = strValue.Length - pointNum - 1;
            BigInteger b = (BigInteger)(value * Mathf.Pow(10, countValue));
            BigInteger A = BigInteger.Pow(b, pow);
            string result = A.ToString();
            int index = result.Length - countValue;
            result.Insert(index, ".");
            return result;
        }
        else
        {
            return strValue;
        }
    }

    public void ClickBuy()
    {
        Lv++;
        totalProd = initialProd * Lv;
        nextCost = baseCost * Mathf.Pow(growthRate, Lv);
        calculatePow(growthRate, Lv);
        Debug.Log(calculatePow(growthRate, Lv));
        setText();
    }
}
