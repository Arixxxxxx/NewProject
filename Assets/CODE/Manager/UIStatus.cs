using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class UIStatus : MonoBehaviour
{
    [SerializeField] int Number;
    [SerializeField] int Lv;
    [SerializeField] float baseCost;//초기 비용
    [SerializeField] float nextCost;//다음레벨 비용
    [SerializeField] float growthRate;//성장률
    [SerializeField] float initialProd;//초기 생산량
    [SerializeField] float powNum;//단계별 지수
    [SerializeField] float totalProd;//총 생산량
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

    public void ClickBuy()
    {
        Lv++;
        totalProd = initialProd * Lv;
        nextCost = baseCost * Mathf.Pow(growthRate, Lv);
        setText();
    }
}
