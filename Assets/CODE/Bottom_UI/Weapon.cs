using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    int Number;//���� �ܰ�
    int lv;//���� ���׷��̵� ����
    int Lv
    {
        get => lv;
        set
        {
            lv = value;
            GameStatus.inst.SetAryWeaponLv(Number, value - Number * 5);
        }
    }
    [SerializeField] float costGrowthRate;//��� �����
    [SerializeField] float atkpowNumRate;//�ʱ���ݷ�����

    Image weaponImage;

    BigInteger baseCost;//�ʱ� ���
    BigInteger nextCost;//�������� ���
    BigInteger resultPowNum;//������ ����Ʈ�� ��ȯ
    BigInteger atk;//���ݷ�
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
    [SerializeField] Button objBtn;
    [SerializeField] GameObject mask;

    private void Awake()
    {

    }

    private void Start()
    {
        weaponImage = transform.Find("imageBtn/IMG").GetComponent<Image>();
        Number = transform.GetSiblingIndex();
        weaponImage.sprite = ActionManager.inst.Get_WeaponSprite(Number);
        initValue();
    }

    void initValue() //�ʱⰪ ����
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
            clickWeaponImage();
        }
        baseCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv), 1.67f);
        setNextCost();
        setText();
    }

    private void setText()
    {
        priceText.text = CalCulator.inst.StringFourDigitAddFloatChanger(nextCost.ToString());
        LvText.text = $"Lv. {Lv - Number * 5} / 5";
        upAtkText.text = "+" + CalCulator.inst.StringFourDigitAddFloatChanger((BigInteger.Multiply(resultPowNum, Lv + 1) - BigInteger.Multiply(resultPowNum, Lv)).ToString());
        totalAtkText.text = CalCulator.inst.StringFourDigitAddFloatChanger(atk.ToString());
    }

    public void ClickBuy()
    {
        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);

        if (haveGold >= nextCost)
        {
            Lv++;
            MissionData.Instance.SetWeeklyMission("���� ��ȭ", 1);
            Atk = BigInteger.Multiply(resultPowNum, Lv);
            GameStatus.inst.MinusGold(nextCost.ToString());
            UIManager.Instance.SetTopWeaponNum(Lv);
            clickWeaponImage();
            setNextCost();
            setText();
            if (Lv % 5 == 0)
            {
                if (objBtn == null)
                {
                    objBtn = transform.Find("Button").GetComponent<Button>();
                }
                objBtn.gameObject.SetActive(false);
                UIManager.Instance.WeaponUpComplete(transform);
                DogamManager.inst.GetWeaponCheck(Number + 1);
            }
        }
        else
        {
            Debug.Log("��尡 �����մϴ�");
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
