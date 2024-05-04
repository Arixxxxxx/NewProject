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
            GameStatus.inst.SetAryWeaponLv(Number, value);

            if (lv >= 5)
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

    [SerializeField] string Name;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI upAtkText;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] TextMeshProUGUI totalAtkText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Button objBtn;
    [SerializeField] GameObject mask;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    public void InitWeapon()
    {
        weaponImage = transform.Find("imageBtn/IMG").GetComponent<Image>();
        initValue();
        weaponImage.sprite = SpriteResource.inst.Weapons[Number];
        nameText.text = $"{Number + 1}. " + Name;
        SetbtnActive();
        GameStatus.inst.OnPercentageChanged.AddListener(() => { setNextCost(); setText(); });
        GameStatus.inst.OnGoldChanged.AddListener(SetbtnActive);
    }

    void initValue() //�ʱⰪ ����
    {
        Number = transform.GetSiblingIndex();
        float powNum = atkpowNumRate * (Number * (Number + 1)) / 2;
        resultPowNum = CalCulator.inst.CalculatePow(10, powNum);
        Lv = GameStatus.inst.GetAryWeaponLv(Number);

        if (Lv != 0)
        {
            Atk = BigInteger.Multiply(resultPowNum, Lv + Number * 5);
            UIManager.Instance.SetTopWeaponNum(Number);
            clickWeaponImage();
        }

        baseCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv), 1.67f);
        setNextCost();
        setText();
    }

    private void setText()
    {
        priceText.text = CalCulator.inst.StringFourDigitAddFloatChanger(nextCost.ToString());
        LvText.text = $"Lv. {Lv} / 5";
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
            if (Lv >= 5)
            {
                UIManager.Instance.SetTopWeaponNum(Number + 1);
            }
            else
            {
                UIManager.Instance.SetTopWeaponNum(Number);
            }
            clickWeaponImage();
            setNextCost();
            setText();
            SetbtnActive();
        }
        else
        {
            Debug.Log("��尡 �����մϴ�");
        }
    }

    private void setNextCost()
    {
        float pricediscount = GameStatus.inst.GetAryPercent((int)ItemTag.QuestWeaponPrice);
        nextCost = baseCost * CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv + Number * 5), 1.67f * (1 - (pricediscount / 100))) * resultPowNum;
    }

    private void SetbtnActive()
    {
        BigInteger havegold = BigInteger.Parse(GameStatus.inst.Gold);
        if (havegold < nextCost)
        {
            objBtn.interactable = false;
        }
        else if(mask.activeSelf == false)
        {
            objBtn.interactable = true;
        }
    }

    public BigInteger GetNextCost()
    {
        float pricediscount = GameStatus.inst.GetAryPercent((int)ItemTag.QuestWeaponPrice);
        return nextCost = baseCost * CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv + Number * 5), 1.67f * (1 - (pricediscount / 100))) * resultPowNum;
    }

    public void clickWeaponImage()
    {
        if (Lv != 0)
        {
            GameStatus.inst.EquipWeaponNum = Number;
        }
    }

    public void SetMaskActive(bool value)
    {
        mask.SetActive(value);
    }
}
