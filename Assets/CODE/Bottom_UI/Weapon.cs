using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] int Number;//무기 단계
    [SerializeField] int lv;//무기 업그레이드 레벨
    int Lv
    {
        get => lv;
        set
        {
            lv = value;
            GameStatus.inst.SetAryWeaponLv(Number, value - Number * 5);
        }
    }
    [SerializeField] float costGrowthRate;//비용 성장률
    [SerializeField] float atkpowNumRate;//초기공격력지수

    Image weaponImage;

    BigInteger baseCost;//초기 비용
    BigInteger nextCost;//다음레벨 비용
    BigInteger resultPowNum;//보정값 빅인트로 전환
    BigInteger atk;//공격력
    private BigInteger Atk
    {
        set
        {
            GameStatus.inst.TotalAtk -= atk;
            atk = value;
            GameStatus.inst.TotalAtk += atk;
        }
    }

    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI upAtkText;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] TextMeshProUGUI totalAtkText;
    [SerializeField] GameObject objBtn;
    [SerializeField] GameObject mask;

    private void Awake()
    {

    }

    private void Start()
    {
        weaponImage = transform.Find("imageBtn").GetComponent<Image>();
        weaponImage.sprite = ActionManager.inst.Get_WeaponSprite(Number);
        initValue();
    }

    void initValue()//초기값 설정
    {
        float powNum = 0;
        for (int iNum = 0; iNum <= Number; iNum++)// 단계별 지수 설정
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
        baseCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv), 1.67f);
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

    public void ClickBuy()
    {
        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
        if (haveGold >= nextCost)
        {
            Lv++;
            MissionData.Instance.SetWeeklyMission("무기 강화", 1);
            Atk = BigInteger.Multiply(resultPowNum, Lv);
            GameStatus.inst.MinusGold(nextCost.ToString());
            UIManager.Instance.SetTopWeaponNum(Lv);
            clickWeaponImage();
            setNextCost();
            setText();
            if (Lv - Number * 5 >= 5)
            {
                objBtn.SetActive(false);
                UIManager.Instance.WeaponUpComplete(transform);
                DogamManager.inst.GetWeaponCheck(Number + 1);
            }
        }
        else
        {
            Debug.Log("골드가 부족합니다");
        }
    }

    private void setNextCost()
    {
        nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv), 1.67f) * resultPowNum;
    }

    public BigInteger GetNextCost()
    {
        return nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv), 1.67f) * resultPowNum;
    }

    public void clickWeaponImage()
    {
        if (Lv % 5 != 0)
        {
            UIManager.Instance.EquipWeaponNum = Number;
        }
    }

    public void SetMaskActive(bool value)
    {
        mask.SetActive(value);
    }
}
