using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] int Number;//����Ʈ �ܰ�
    [SerializeField] int Lv;//����Ʈ ���׷��̵� ����
    [SerializeField] float atkGrowthRate;//���ݷ� �����
    [SerializeField] float atkpowNumRate;//�ʱ���ݷ�����
    [SerializeField] int WeaponNum; //���� �̹�����ȣ

    BigInteger baseCost;//�ʱ� ���
    BigInteger nextCost;//�������� ���
    BigInteger resultPowNum;//������ ����Ʈ�� ��ȯ
    BigInteger atk;//���ݷ�
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
        initValue();
    }

    void initValue()//�ʱⰪ ����
    {
        float powNum = 0;
        for (int iNum = 0; iNum <= Number; iNum++)// �ܰ躰 ���� ����
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
        }
        baseCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(atkGrowthRate, Lv), 1.67f);
        setNextCost();
        setText();
    }

    private void setText()
    {
        priceText.text = "���� : " + CalCulator.inst.StringFourDigitChanger(nextCost.ToString());
        LvText.text = $"Lv : {Lv - Number * 5} / 5";
        upAtkText.text = "+" + CalCulator.inst.StringFourDigitChanger((BigInteger.Multiply(resultPowNum, Lv + 1) - BigInteger.Multiply(resultPowNum, Lv)).ToString());
        totalAtkText.text = "���ݷ� : " + CalCulator.inst.StringFourDigitChanger(atk.ToString());
    }

    public void ClickBuy()
    {
        Lv++;
        Atk = BigInteger.Multiply(resultPowNum, Lv);
        UIManager.Instance.EquipWeaponNum = WeaponNum;
        setNextCost();
        setText();
        if (Lv - Number * 5 >= 5)
        {
            objBtn.SetActive(false);
        }
    }

    private void setNextCost()
    {
        nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(atkGrowthRate, Lv), 1.67f) * resultPowNum;
    }

    public void clickWeaponImage()
    {
        UIManager.Instance.EquipWeaponNum = WeaponNum;
        Debug.Log(UIManager.Instance.EquipWeaponNum);
    }
}
