using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Numerics;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("상점")]
    [SerializeField] Button mainShopCloseBtn;// 상점 닫기버튼
    [SerializeField] GameObject[] list_ShopUi;// 상점 하단버튼 리스트
    [SerializeField] Image[] list_BottomBtn;//메인UI 하단 버튼
    int botBtnNum = 0;// 상점 하단버튼 번호

    [Header("상품구매")]
    [SerializeField] GameObject obj_BuyCheck;
    [SerializeField] TMP_Text text_price;
    [SerializeField] Transform productImageParents;//상품 이미지 부모
    List<Image> list_productImage = new List<Image>();//상품 이미지 리스트
    List<TMP_Text> list_productCountText = new List<TMP_Text>();//상품 갯수 텍스트
    List<GoldShop.Product> list_buyWaiting = new List<GoldShop.Product>();//구매 대기 상품
    string waitingPrice;
    ProductTag waitingPriceType;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        mainShopCloseBtn.onClick.AddListener(() => UIManager.Instance.changeSortOder(0));

        int productImageCount = productImageParents.childCount;
        for (int iNum = 0; iNum < productImageCount; iNum++)
        {
            list_productImage.Add(productImageParents.GetChild(iNum).GetComponent<Image>());
        }

        int productImageTextCount = list_productImage.Count;
        for (int iNum = 0; iNum < productImageTextCount; iNum++)
        {
            list_productCountText.Add(list_productImage[iNum].transform.GetChild(0).GetComponent<TMP_Text>());
        }
    }

    void Update()
    {

    }

    public void ClickShopBtn(int _num)
    {
        list_BottomBtn[botBtnNum].sprite = UIManager.Instance.GetSelectUISprite(0);
        list_ShopUi[botBtnNum].SetActive(false);

        botBtnNum = _num;
        list_BottomBtn[botBtnNum].sprite = UIManager.Instance.GetSelectUISprite(1);
        list_ShopUi[botBtnNum].SetActive(true);
    }

    public void SetCheckBuyActive(bool value)
    {
        obj_BuyCheck.SetActive(value);
    }

    public void SetCheckBuy(List<GoldShop.Product> _list, string _price, ProductTag _priceType)
    {
        list_buyWaiting = _list;
        waitingPrice = _price;
        waitingPriceType = _priceType;
        int count = list_productImage.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            list_productImage[iNum].gameObject.SetActive(false);
        }

        count = _list.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            list_productImage[iNum].gameObject.SetActive(true);
            list_productImage[iNum].sprite = _list[iNum].sprite;
            list_productCountText[iNum].text = _list[iNum].count;
        }
        text_price.text = _price.ToString();
    }

    public void ClickBuyYes()
    {
        int count;
        switch (waitingPriceType)
        {
            case ProductTag.Gold:
                BigInteger haveGold = BigInteger.Parse(GameStatus.inst.PulsGold);
                if (haveGold >= BigInteger.Parse(waitingPrice))
                {
                    GameStatus.inst.MinusGold(waitingPrice);
                    count = list_buyWaiting.Count;
                    for (int iNum = 0; iNum < count; iNum++)
                    {
                        list_buyWaiting[iNum].buyProduct();
                    }
                }
                break;
            case ProductTag.Ruby:
                int haveRuby = int.Parse(GameStatus.inst.PulsGold);
                int iPrice = int.Parse(waitingPrice);
                if (haveRuby >= iPrice)
                {
                    GameStatus.inst.Ruby -= iPrice;
                    count = list_buyWaiting.Count;
                    for (int iNum = 0; iNum < count; iNum++)
                    {
                        list_buyWaiting[iNum].buyProduct();
                    }
                }
                break;
            case ProductTag.Star:
                BigInteger haveStar = BigInteger.Parse(GameStatus.inst.PulsGold);
                if (haveStar >= BigInteger.Parse(waitingPrice))
                {
                    //별 갯수 차감
                    count = list_buyWaiting.Count;
                    for (int iNum = 0; iNum < count; iNum++)
                    {
                        list_buyWaiting[iNum].buyProduct();
                    }
                }
                break;
        }
        obj_BuyCheck.SetActive(false);
    }
}
