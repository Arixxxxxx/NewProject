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
    [SerializeField] int limitLv;

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
            GameStatus.inst.SetAryRelicLv((int)itemNum, value);
            setNextCost();
            setText();
            if (limitLv != 0 && lv >= limitLv)
            {
                PriceText.text = "";
                priceImage.SetActive(false);
                priceMask.SetActive(true);
            }
        }
    }
    Button upBtn;
    Image relicImgae;
    TextMeshProUGUI LvText;
    TextMeshProUGUI PercentText;
    TextMeshProUGUI PriceText;
    GameObject priceImage;
    GameObject priceMask;
    GameObject starImgRef;
    
    GameObject effectRef;
    float rotateSpeedMultiPlyer = 15f;
    private void Update()
    {
        if (rankNum != RankType.Rare)
        {
            if(effectRef == null)
            {
                effectRef = transform.Find("IMG_Layout/Bg_Effect").gameObject;
            }

            effectRef.transform.Rotate(UnityEngine.Vector3.forward * Time.deltaTime * rotateSpeedMultiPlyer);
        }
    }
    public void initRelic()
    {
        LvText = transform.Find("IMG_Layout/LvText").GetComponent<TextMeshProUGUI>();
        PercentText = transform.Find("TextBox/PercentageText").GetComponent<TextMeshProUGUI>();
        PriceText = transform.Find("Button/PriceText").GetComponent<TextMeshProUGUI>();
        starImgRef = transform.Find("Button/Image").gameObject;
        upBtn = transform.Find("Button").GetComponent<Button>();
        relicImgae = transform.Find("IMG_Layout/IMG").GetComponent<Image>();
        priceImage = transform.Find("Button/Image").gameObject;
        priceMask = transform.Find("Button/Mask").gameObject;
        UIManager.Instance.OnRelicBuyCountChanged.AddListener(() => _OnCountChanged());
        GameStatus.inst.OnStartChanged.AddListener(checkStar);
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
            nextCost =CalCulator.inst.CalculatePow(costGrowthRate, Lv) * (BigInteger)((Mathf.Pow(costGrowthRate, buyCount) - 1) / (costGrowthRate - 1));
        }
        else//max일 때
        {
            buyCount = 1;
            BigInteger haveStar = BigInteger.Parse(GameStatus.inst.Star);
            setNextCost(buyCount);
            while (haveStar >= nextCost && Lv + buyCount <= limitLv)
            {
                buyCount++;
                setNextCost(buyCount);
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
                percentage = 100 * Mathf.Pow(1.05f, Lv);
                break;
            case ItemTag.AtkSpeed:
                percentage = 100 + Lv;
                GameStatus.inst.AtkSpeedLv = Lv;
                break;
            case ItemTag.Critical:
                percentage = 100 + Lv;
                break;
            case ItemTag.QuestDiscount:
            case ItemTag.WeaponDiscount:
                percentage = 100 + Lv;
                break;
        }
        GameStatus.inst.SetAryPercent((int)itemNum, percentage - 100);
    }

    void setNextCost(int count)
    {
        nextCost = CalCulator.inst.CalculatePow(costGrowthRate, Lv) * (BigInteger)((Mathf.Pow(costGrowthRate, count) - 1) / (costGrowthRate - 1));
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
            GameStatus.inst.MinusStar(nextCost.ToString());
            Lv += buyCount;
            MissionData.Instance.SetSpecialMission((int)itemNum, Lv, SpMissionTag.Relic);
        }
    }

    void checkStar()
    {
        BigInteger haveStar = BigInteger.Parse(CalCulator.inst.ConvertChartoIndex(GameStatus.inst.Star));

        if (haveStar < nextCost && upBtn.interactable)
        {
            upBtn.interactable = false;
        }
        else if (upBtn.interactable == false)
        {
            upBtn.interactable = true;
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
