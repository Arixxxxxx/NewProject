using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class Relic : MonoBehaviour, IClickLvUpAble
{
    [SerializeField] RankType rankNum;
    [SerializeField] ItemTag itemNum;
    [SerializeField] float costGrowthRate;
    [SerializeField] int limitLv;

    BigInteger nextCost = new BigInteger();

    float percentage;
    int buyCount = 1;

    int lv;
    public int Lv
    {
        get => lv;
        set
        {
            lv = value;
            GameStatus.inst.SetAryRelicLv((int)itemNum, value);
            setNextCost();
            setText();
            if (limitLv != 0 && lv >= limitLv)
            {
                PriceText.text = "";
                priceImage.SetActive(false);
                priceMask.SetActive(true);
                upBtn.interactable = false;
            }
        }
    }
    Button upBtn;
    Image relicImgae;
    TextMeshProUGUI LvText;
    TextMeshProUGUI PercentText;
    TextMeshProUGUI PriceText;
    TextMeshProUGUI maxLvInfo_Text;
    GameObject priceImage;
    GameObject priceMask;
    GameObject starImgRef;

    GameObject effectRef;
    float rotateSpeedMultiPlyer = 15f;
    private void Update()
    {
        // 배경 회전
        if (rankNum != RankType.Rare && gameObject.activeInHierarchy)
        {
            if (effectRef == null)
            {
                effectRef = transform.Find("IMG_Layout/Bg_Effect").gameObject;
            }

            effectRef.transform.Rotate(UnityEngine.Vector3.forward * Time.deltaTime * rotateSpeedMultiPlyer);
        }
    }
    public void initRelic()
    {
        maxLvInfo_Text = transform.Find("LvText").GetComponent<TextMeshProUGUI>();
        LvText = transform.Find("IMG_Layout/LvText").GetComponent<TextMeshProUGUI>();
        PercentText = transform.Find("TextBox/PercentageText").GetComponent<TextMeshProUGUI>();
        PriceText = transform.Find("Button/PriceText").GetComponent<TextMeshProUGUI>();
        starImgRef = transform.Find("Button/Image").gameObject;
        upBtn = transform.Find("Button").GetComponent<Button>();
        relicImgae = transform.Find("IMG_Layout/IMG").GetComponent<Image>();
        priceImage = transform.Find("Button/Image").gameObject;
        priceMask = transform.Find("Mask").gameObject;
        UIManager.Instance.OnRelicBuyCountChanged.AddListener(() => _OnCountChanged());
        GameStatus.inst.OnStartChanged.AddListener(checkStar);

        //프리펩 하나하나 수정하기 힘들어서 코드로해놈..
        maxLvInfo_Text.text = $"</b>Lv당 {GameStatus.inst.RelicDefaultvalue((int)itemNum)}씩 증가 <color=#FFE100>( Max.{limitLv} )</color>";
        maxLvInfo_Text.fontSize = 10;
        priceMask.GetComponent<Image>().sprite = upBtn.GetComponent<Image>().sprite;
        priceMask.GetComponent<Image>().pixelsPerUnitMultiplier = 4;

        setNextCost();
        setText();

        upBtn.onClick.AddListener(() => { ClickUp(); });

    }

    BigInteger haveStar = new BigInteger();
    void setNextCost()
    {
        //nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(CalCulator.inst.CalculatePow(costGrowthRate, Lv), 1.67f);
        int btnnum = UIManager.Instance.RelicBuyCountBtnNum;
        if (Lv + buyCount >= limitLv)
        {
            buyCount = limitLv - Lv;
         
        }
        if (btnnum != 3)//max가 아닐때
        {
            nextCost = CalCulator.inst.CalculatePow(costGrowthRate, Lv) * (BigInteger)((Mathf.Pow(costGrowthRate, buyCount) - 1) / (costGrowthRate - 1));
        }
        else//max일 때
        {
            buyCount = 1;
            haveStar = BigInteger.Parse(GameStatus.inst.Star);
            setNextCost(buyCount);
            while (haveStar >= nextCost && Lv + buyCount <= limitLv)
            {
                buyCount++;
                setNextCost(buyCount);
                if (buyCount >= 100)
                {
                    break;
                }
                if (Lv + buyCount >= limitLv)
                {
                    buyCount = limitLv - Lv + 1;
                    break;
                }
            }
            buyCount--;
            nextCost = CalCulator.inst.CalculatePow(costGrowthRate, Lv) * (BigInteger)((Mathf.Pow(costGrowthRate, buyCount) - 1) / (costGrowthRate - 1));
        }
        setPercentage();
    }

    void setPercentage()
    {
        switch (itemNum)
        {
            case ItemTag.Atk:
            case ItemTag.QuestGold:
            case ItemTag.KillGold:
            case ItemTag.CriticalDmg:
            case ItemTag.FeverTime:
            case ItemTag.GetStar:
            case ItemTag.Critical:
            case ItemTag.QuestDiscount:
            case ItemTag.WeaponDiscount:
                percentage = GameStatus.inst.RelicDefaultvalue((int)itemNum) * Lv;
                break;
            case ItemTag.AtkSpeed:
                percentage = Lv;
                ActionManager.inst.PlayerAttackSpeedLvUp();
                break;
        }
        GameStatus.inst.SetAryPercent((int)itemNum, percentage);
    }

    void setNextCost(int count)
    {
        nextCost = CalCulator.inst.CalculatePow(costGrowthRate, Lv) * (BigInteger)((Mathf.Pow(costGrowthRate, count) - 1) / (costGrowthRate - 1));
    }

    void setText()
    {
        LvText.text = $"Lv. {lv}";

        if (itemNum == ItemTag.Critical)
        {
            PercentText.text = percentage.ToString("F1") + "%";
        }
        else
        {
            PercentText.text = percentage.ToString("N0") + "%";
        }

        PriceText.text = CalCulator.inst.StringFourDigitAddFloatChanger(nextCost.ToString());
    }

    BigInteger haveStar1 = new BigInteger();
    public void ClickUp()
    {
        if (Lv >= limitLv)
        {
            return;
        }
        haveStar1 = BigInteger.Parse(CalCulator.inst.ConvertChartoIndex(GameStatus.inst.Star));
        if (haveStar1 >= nextCost)
        {
            Lv += buyCount;
            GameStatus.inst.MinusStar(nextCost.ToString());
            MissionData.Instance.SetSpecialMission((int)itemNum, Lv, SpMissionTag.Relic);
        }
    }

    BigInteger haveStar2 = new BigInteger();
    void checkStar()
    {

        if (Lv >= limitLv)
        {
            upBtn.interactable = false;
            return;
        }
        haveStar2 = BigInteger.Parse(CalCulator.inst.ConvertChartoIndex(GameStatus.inst.Star));

        if (haveStar2 < nextCost && upBtn.interactable)
        {
            upBtn.interactable = false;
        }
        else if (upBtn.interactable == false && haveStar2 >= nextCost)
        {
            upBtn.interactable = true;
        }
    }

    private void _OnCountChanged()
    {
        buyCount = UIManager.Instance.RelicBuyCount;
        if (Lv + buyCount >= limitLv)
        {
            buyCount = limitLv - Lv;
        }
        setNextCost();
        setText();
        checkStar();
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

    public int Get_MyNum()
    {
        return (int)itemNum;
    }


}
