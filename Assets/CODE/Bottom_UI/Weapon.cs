using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour, IClickLvUpAble
{
    int Number;//���� �ܰ�
    int lv;//���� ���׷��̵� ����
    int Lv
    {
        get => lv;
        set
        {
            lv = value;
            Atk = getAtk(lv);
            GameStatus.inst.SetAryWeaponLv(Number, value);

            if (lv >= 5)
            {
                SetbtnActive();
                priceText.text = "";
                upBtnImage.SetActive(false);
                upBtnMask.SetActive(true);
                UIManager.Instance.WeaponUpComplete(Number);
            }
        }
    }
    [Header("��� �����")]
    [SerializeField] float costGrowthRate;//��� �����
    [Header("�ܰ躰 ���� ��·�")]
    [SerializeField] float atkpowNumRate;//�ܰ躰 ���� ��·�
    [Header("���ݷ� ��·�")]
    [SerializeField] float atkRate;//���ݷ� ��·�
    [Space]
    [SerializeField] string Name;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI upAtkText;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] TextMeshProUGUI totalAtkText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Button objBtn;
    [SerializeField] GameObject mask;
    [SerializeField] GameObject upBtnMask;
    [SerializeField] GameObject upBtnImage;

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

    public void InitWeapon()
    {

        weaponImage = transform.Find("imageBtn/IMG").GetComponent<Image>();
        initValue();
        weaponImage.sprite = SpriteResource.inst.Weapons[Number];
        nameText.text = $"{Number + 1}. {Set_WeaponName(Number)}";
        SetbtnActive();
        GameStatus.inst.OnPercentageChanged.AddListener(() =>
        {
            setNextCost(); setText(); Atk = getAtk(Lv);
        });
        GameStatus.inst.OnGoldChanged.AddListener(SetbtnActive);
        UIManager.Instance.WeaponReset.AddListener(resetWeapon);
        //objBtn.onClick.AddListener(() => { AudioManager.inst.Play_Ui_SFX(1, 0.8f); });
    }

    void initValue() //�ʱⰪ ����
    {
        Number = transform.GetSiblingIndex();
        float powNum = atkpowNumRate * (Number * (Number + 1)) / 2;
        resultPowNum = CalCulator.inst.CalculatePow(10, powNum);
        Lv = GameStatus.inst.GetAryWeaponLv(Number);

        if (Lv != 0)
        {
            //UIManager.Instance.SetTopWeaponNum(Number);
            clickWeaponImage();
        }

        //baseCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv), 1.67f);
        baseCost = BigInteger.Pow(1000, Number);
        setNextCost();
        setText();
    }

    private void setText()
    {
        LvText.text = $"Lv. {Lv} / 5";
        upAtkText.text = $"LV ����ġ +{CalCulator.inst.StringFourDigitAddFloatChanger((getAtk(Lv + 1) - getAtk(Lv)).ToString())}";
        totalAtkText.text = $"���� ����� : {CalCulator.inst.StringFourDigitAddFloatChanger(atk.ToString())} / Ÿ��";
        if (Lv < 5)
        {
            priceText.text = CalCulator.inst.StringFourDigitAddFloatChanger(nextCost.ToString());
        }
    }

    public void ClickUp()
    {
        //if (Lv >= 5)
        //{
        //    Debug.Log("�̹� �ִ� �����Դϴ�");
        //    return;
        //}

        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);

        if (haveGold >= nextCost)
        {
            Lv++;
            MissionData.Instance.SetWeeklyMission("���� ��ȭ", 1);

            GameStatus.inst.MinusGold(nextCost.ToString());
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
        if (Lv <= 5)
        {
            float pricediscount = GameStatus.inst.GetAryPercent((int)ItemTag.WeaponDiscount);
            //nextCost = baseCost * CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv + Number * 5), 1.67f * (1 - (pricediscount / 100))) * resultPowNum;
            if (Lv == 0)
            {
                nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(baseCost, 1 - (pricediscount / 100));
            }
            else
            {

                nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(200 * Lv * baseCost, 1 - (pricediscount / 100));
            }
        }
    }

    private BigInteger getAtk(int lv)
    {
        float relicAtk = 1 + GameStatus.inst.GetAryPercent((int)ItemTag.Atk) / 100;
        //if (relicAtk == 0)
        //{
        //    return (BigInteger)(lv * Mathf.Pow(Number + 1, atkRate) + 100 * lv);
        //}
        //else
        //{
            return (BigInteger)((lv * Mathf.Pow(Number + 1, atkRate) + 100 * lv) * relicAtk);
        //}
    }

    //private BigInteger getNextAtk(int num)
    //{
    //    float relicAtk = GameStatus.inst.GetAryPercent((int)ItemTag.Atk);
    //    if (relicAtk == 0)
    //    {
    //        return (BigInteger)((Lv + 1) * Mathf.Pow(num + 1, atkRate) + 100 * (Lv + 1));
    //    }
    //    else
    //    {
    //        return (BigInteger)(((Lv + 1) * Mathf.Pow(num + 1, atkRate) + 100 * (Lv + 1)) * GameStatus.inst.GetAryPercent((int)ItemTag.Atk));
    //    }

    //}

    private void SetbtnActive()
    {
        BigInteger havegold = BigInteger.Parse(GameStatus.inst.Gold);
        if (Lv >= 5)
        {
            objBtn.interactable = false;
        }
        else if (havegold < nextCost && objBtn.interactable == true || nextCost == 0)
        {
            objBtn.interactable = false;
        }
        else if (mask.activeSelf == false && objBtn.interactable == false && havegold >= nextCost)
        {
            objBtn.interactable = true;
        }
        else if (mask.activeSelf && objBtn.interactable == true)
        {
            objBtn.interactable = false;
        }
    }

    private void resetWeapon()
    {
        Lv = 0;
        if (Number != 0)
        {
            SetMaskActive(true);
        }
        else
        {
            GameStatus.inst.EquipWeaponNum = Number;
        }
        upBtnImage.SetActive(true);
        upBtnMask.SetActive(false);
        setNextCost();
        setText();
        SetbtnActive();
    }

    public BigInteger GetNextCost()
    {
        return nextCost;
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


    /// <summary>
    /// �����̸� �ʱ�ȭ
    /// </summary>
    /// <param name="WeaponNumber"> Sibling</param>
    /// <returns></returns>
    private string Set_WeaponName(int WeaponNumber)
    {
        string[] weaponName =
            {
            "�׵�", //1
            "���", //2
            "�߽ĵ�", //3
            "�յ���",//4
            "��", //5
            "�Ϻ���",//6
            "�ڵ�", //7
            "��â", //8
            "�����", //9
            "ȫ����", //10
            "����", //11
            "ȣ�α�", //12
            "���Ǻ�", //13 
            "���Ȼ��", //14
            "û������", //15
            "Ź��ä", //16
            "ĥ��ĥ��", //17
            "��¤��", //18
            "ȭ��� û�ҵ���", //19
            "�汤��", //20
            "�츮���̵�����", //21 
            "�վ",  //22
            "��~�и�ġ", //23
            "����", //24
            "������ ���̼�", //25
            "�ż��� û��ġ", //26
            "¥��~��", //27
            "�ջ���",  //28
            "���� ���ķ�", //29
            "�˸����"  //30
        };
        return weaponName[WeaponNumber];
    }
}
