using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] int Number;//Äù½ºÆ® ´Ü°è
    [SerializeField] int Lv;//Äù½ºÆ® ¾÷±×·¹ÀÌµå ·¹º§
    [SerializeField] float atkGrowthRate;//°ø°Ý·Â ¼ºÀå·ü
    [SerializeField] float atkpowNumRate;//ÃÊ±â°ø°Ý·ÂÁö¼ö
    [SerializeField] int WeaponNum; //¹«±â ÀÌ¹ÌÁö¹øÈ£

    BigInteger baseCost;//ÃÊ±â ºñ¿ë
    BigInteger nextCost;//´ÙÀ½·¹º§ ºñ¿ë
    BigInteger resultPowNum;//º¸Á¤°ª ºòÀÎÆ®·Î ÀüÈ¯
    BigInteger atk;//°ø°Ý·Â
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

    private void Awake()
    {

    }

    private void Start()
    {
        initValue();
    }

    void initValue()//ÃÊ±â°ª ¼³Á¤
    {
        float powNum = 0;
        for (int iNum = 0; iNum <= Number; iNum++)// ´Ü°èº° Áö¼ö ¼³Á¤
        {
            powNum += atkpowNumRate * iNum;
        }
        resultPowNum = CalCulator.inst.CalculatePow(10, powNum);

        if (Number != 0)
        {
            Lv = Number * 5;
        }

        if (Lv - (Number * 5) != 0)
        {
            Atk = BigInteger.Multiply(resultPowNum, Lv);
            clickWeaponImage();
        }
        baseCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(atkGrowthRate, Lv), 1.67f);
        setNextCost();
        setText();
    }

    private void setText()
    {
        priceText.text = "°¡°Ý : " + CalCulator.inst.StringFourDigitChanger(nextCost.ToString());
        LvText.text = $"Lv : {Lv - Number * 5} / 5";
        upAtkText.text = "+" + CalCulator.inst.StringFourDigitChanger((BigInteger.Multiply(resultPowNum, Lv + 1) - BigInteger.Multiply(resultPowNum, Lv)).ToString());
        totalAtkText.text = "°ø°Ý·Â : " + CalCulator.inst.StringFourDigitChanger(atk.ToString());
    }

    public void ClickBuy()
    {
        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
        if (haveGold >= nextCost)
        {
            Lv++;
            Atk = BigInteger.Multiply(resultPowNum, Lv);
<<<<<<< HEAD
=======
            //GameStatus.inst.MinusGold(nextCost.ToString());
>>>>>>> 70a1d48 (ui ì´ë¯¸ì§€ ì‚½ìž…ì¤‘)
            clickWeaponImage();
            setNextCost();
            setText();
            if (Lv - Number * 5 >= 5)
            {
                objBtn.SetActive(false);
            }
        }
        else
        {
            Debug.Log("°ñµå°¡ ºÎÁ·ÇÕ´Ï´Ù");
        }
    }

    private void setNextCost()
    {
        nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(atkGrowthRate, Lv), 1.67f) * resultPowNum;
    }

    public void clickWeaponImage()
    {
        UIManager.Instance.EquipWeaponNum = WeaponNum;
    }
}
