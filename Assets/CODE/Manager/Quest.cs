using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;

public class Quest : MonoBehaviour
{
    [Tooltip("Äù½ºÆ®´Ü°è")]
    [SerializeField] int Number;//Äù½ºÆ® ´Ü°è
    [Tooltip("¼ºÀå·ü")]
    [SerializeField] float growthRate;//¼ºÀå·ü
    [Tooltip("ÃÊ±â »ı»ê·®°ú ÃÊ±âºñ¿ëÀÇ ºñÀ² ³·À»¼ö·Ï ÃÊ±â°¡°İÀÌ ºñ½ÎÁü")]
    [SerializeField] float initalProdRate;//ÃÊ±â »ı»ê·®°ú ÃÊ±âºñ¿ëÀÇ ºñÀ² ³·À»¼ö·Ï ÃÊ±â°¡°İÀÌ ºñ½ÎÁü
    [Tooltip("±âÃÊ »ı»ê·®")]
    [SerializeField] float baseProd;//±âÃÊ »ı»ê·®
    [Tooltip("ÃÊ±â»ı»ê·® Áö¼ö")]
    [SerializeField] float powNumRate;//ÃÊ±â»ı»ê·®Áö¼ö
    int Lv;//Äù½ºÆ® ¾÷±×·¹ÀÌµå ·¹º§
    int LvCur = 1; //·¹º§º¸Á¤
    int itemCur = 1; //¾ÆÀÌÅÛº¸Á¤
    float powNum;//´Ü°èº° Áö¼ö

    BigInteger baseCost;//ÃÊ±â ºñ¿ë
    BigInteger nextCost;//´ÙÀ½·¹º§ ºñ¿ë
    BigInteger initialProd;//ÃÊ±â »ı»ê·®
    BigInteger totalProd;//ÃÑ »ı»ê·®
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
        UIManager.Instance.OnBuyCountChanged.AddListener(_OnCountChanged);
    }

    void initValue()//ÃÊ±â°ª ¼³Á¤
    {
        powNum = 0;
        for (int iNum = 0; iNum <= Number; iNum++)// ´Ü°èº° Áö¼ö ¼³Á¤
        {
            powNum += powNumRate * iNum;
        }
        BigInteger resultPowNum = CalCulator.inst.CalculatePow(10, powNum);

        initialProd = BigInteger.Multiply((BigInteger)baseProd, resultPowNum);

        baseCost = BigInteger.Multiply(initialProd, (BigInteger)initalProdRate);
        setNextCost();
        TotalProd = initialProd * Lv;
        setText();
    }

    private void setText()
    {
        priceText.text = "°¡°İ : " + CalCulator.inst.StringFourDigitChanger(nextCost.ToString());
        upGoldText.text = "+" + CalCulator.inst.StringFourDigitChanger($"{initialProd * (Lv + UIManager.Instance.BuyCount) - initialProd * (Lv)}");
        LvText.text = "Lv : " + CalCulator.inst.StringFourDigitChanger(Lv.ToString());
        totalGoldText.text = "Gps : " + CalCulator.inst.StringFourDigitChanger($"{totalProd}");
    }

    public void ClickBuy()
    {
        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
        if (haveGold >= nextCost)
        {
            Lv += UIManager.Instance.BuyCount;
            if (Lv >= 25 * LvCur)
            {
                LvCur *= 2;
            }
            TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
<<<<<<< HEAD
            GameStatus.inst.GetGold(nextCost.ToString());
=======
            //GameStatus.inst.MinusGold(nextCost.ToString());
>>>>>>> 70a1d48 (ui ì´ë¯¸ì§€ ì‚½ì…ì¤‘)
            setNextCost();
            setText();
        }
        else
        {
            Debug.Log("µ·ÀÌ ºÎÁ·ÇÕ´Ï´Ù.");
        }
    }

    private void setNextCost()
    {
        int buycount = UIManager.Instance.BuyCount;
        if (buycount != 0)//max°¡ ¾Æ´Ò¶§
        {
            nextCost = baseCost * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buycount) - 1) / (growthRate - 1)));
        }
    }

    private void maxUpgrade()
    {

    }

    private void _OnCountChanged()
    {
        setNextCost();
        setText();
    }
}
