using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class Relic : MonoBehaviour
{
    [SerializeField] RankType rankNum;
    [SerializeField] ItemTag itemNum;
    [SerializeField] float costGrowthRate;

    BigInteger nextCost;
    float percentage;
    int buyCount = 1;

    int lv;
    public int Lv
    {
        get => lv;
        set
        {
            lv = value;
            setNextCost();
            setText();
        }
    }
    Button upBtn;
    Image relicImgae;
    TextMeshProUGUI LvText;
    TextMeshProUGUI PercentText;
    TextMeshProUGUI PriceText;

    public void initRelic()
    {
        LvText = transform.Find("Button/LvText").GetComponent<TextMeshProUGUI>();
        PercentText = transform.Find("TextBox/PercentageText").GetComponent<TextMeshProUGUI>();
        PriceText = transform.Find("Button/PriceText").GetComponent<TextMeshProUGUI>();
        upBtn = transform.Find("Button").GetComponent<Button>();
        relicImgae = transform.Find("IMG_Layout/IMG").GetComponent<Image>();
        UIManager.Instance.OnRelicBuyCountChanged.AddListener(() => _OnCountChanged());
        setNextCost();
        setText();


        upBtn.onClick.AddListener(ClickUp);
    }

    void setNextCost()
    {
        //nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv), 1.67f);
        int btnnum = UIManager.Instance.RelicBuyCountBtnNum;
        if (btnnum != 3)//max가 아닐때
        {
            nextCost = /*baseCost **/ (CalCulator.inst.CalculatePow(costGrowthRate, Lv) * (BigInteger)((Mathf.Pow(costGrowthRate, buyCount) - 1) / (costGrowthRate - 1)));
        }
        else//max일 때
        {
            buyCount = 1;
            BigInteger haveStar = BigInteger.Parse(GameStatus.inst.Star);
            setNextCost(buyCount);
            while (haveStar >= nextCost)
            {
                buyCount++;
                setNextCost(buyCount);
            }
            buyCount--;
            nextCost = /*baseCost * */(CalCulator.inst.CalculatePow(costGrowthRate, Lv) * (BigInteger)((Mathf.Pow(costGrowthRate, buyCount) - 1) / (costGrowthRate - 1)));
        }
        percentage = 100 * Mathf.Pow(1.05f, Lv);
    }

    void setNextCost(int count)
    {
        nextCost = /*baseCost **/ (CalCulator.inst.CalculatePow(costGrowthRate, Lv) * (BigInteger)((Mathf.Pow(costGrowthRate, count) - 1) / (costGrowthRate - 1)));
    }

    void setText()
    {
        LvText.text = $"Lv. {lv}";
        PercentText.text = percentage.ToString("N0") + "%";
        PriceText.text = CalCulator.inst.StringFourDigitAddFloatChanger(nextCost.ToString());
    }

    void ClickUp()
    {
        BigInteger haveStar = BigInteger.Parse(CalCulator.inst.ConvertChartoIndex(GameStatus.inst.Star));
        if (haveStar >= nextCost)
        {
            Lv += buyCount;
        }
    }

    private void _OnCountChanged()
    {
        buyCount = UIManager.Instance.RelicBuyCount;
        setNextCost();
        setText();
    }

    public Sprite GetSprite()
    {
        return relicImgae.sprite;
    }
    /// <summary>
    /// x = rankNum, y = ItemNum
    /// </summary>
    /// <returns></returns>
    public UnityEngine.Vector2 GetMyType()
    {
        return new UnityEngine.Vector2((int)rankNum, (int)itemNum);
    }
}
