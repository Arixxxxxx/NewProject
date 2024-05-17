using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    int Number;//무기 단계
    int lv;//무기 업그레이드 레벨
    int Lv
    {
        get => lv;
        set
        {
            lv = value;
            GameStatus.inst.SetAryWeaponLv(Number, value);

            if (lv >= 5)
            {
                //if (objBtn == null)
                //{
                //    objBtn = transform.Find("Button").GetComponent<Button>();
                //}
                //objBtn.gameObject.SetActive(false);
                SetbtnActive();
                priceText.text = "";
                upBtnImage.SetActive(false);
                upBtnMask.SetActive(true);
                UIManager.Instance.WeaponUpComplete(transform);
                DogamManager.inst.GetWeaponCheck(Number + 1);
            }
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
        nameText.text = $"{Number + 1}. {Set_WeaponName(Number)}";
        //nameText.text = $"{Number + 1}. " + Name;
        SetbtnActive();
        GameStatus.inst.OnPercentageChanged.AddListener(() => { setNextCost(); setText(); });
        GameStatus.inst.OnGoldChanged.AddListener(SetbtnActive);
    }

    void initValue() //초기값 설정
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
        LvText.text = $"Lv. {Lv} / 5";
        upAtkText.text = $"LV 증가치 +{CalCulator.inst.StringFourDigitAddFloatChanger((BigInteger.Multiply(resultPowNum, Lv + 1) - BigInteger.Multiply(resultPowNum, Lv)).ToString())}";
        totalAtkText.text = $"종합 대미지 : {CalCulator.inst.StringFourDigitAddFloatChanger(atk.ToString())} / 타격";
        if (Lv < 5)
        {
            priceText.text = CalCulator.inst.StringFourDigitAddFloatChanger(nextCost.ToString());
        }
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
            Debug.Log("골드가 부족합니다");
        }
    }

    private void setNextCost()
    {
        if (Lv <= 5)
        {
            float pricediscount = GameStatus.inst.GetAryPercent((int)ItemTag.QuestWeaponPrice);
            nextCost = baseCost * CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv + Number * 5), 1.67f * (1 - (pricediscount / 100))) * resultPowNum;
        }
    }

    private void SetbtnActive()
    {
        BigInteger havegold = BigInteger.Parse(GameStatus.inst.Gold);
        if (havegold < nextCost || Lv >= 5)
        {
            objBtn.interactable = false;
        }
        else if (mask.activeSelf == false)
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
    

    /// <summary>
    /// 무기이름 초기화
    /// </summary>
    /// <param name="WeaponNumber"> Sibling</param>
    /// <returns></returns>
    private string Set_WeaponName(int WeaponNumber)
    {
        string[] weaponName = 
            {
            "죽도", //1
            "목검", //2
            "중식도", //3
            "손도끼",//4
            "낫", //5
            "일본도",//6
            "박도", //7
            "장창", //8
            "언월도", //9
            "홍접선", //10
            "보검", //11
            "호두구", //12
            "여의봉", //13 
            "장팔사모", //14
            "청룡언월도", //15
            "탁구채", //16
            "칠교칠선", //17
            "뒤짚개", //18
            "화장실 청소도구", //19
            "경광봉", //20
            "우리아이딸랑이", //21 
            "뚫어뻥",  //22
            "왕~뿅망치", //23
            "빠루", //24
            "고오급 샤미센", //25
            "신선한 청새치", //26
            "짜앙~돌", //27
            "왕사탕",  //28
            "엄마 탕후루", //29
            "똥막대기"  //30
        };
        return weaponName[WeaponNumber];
    }
}
