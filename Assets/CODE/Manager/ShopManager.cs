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
    [SerializeField] GameObject obj_shop;// 상점 오브젝트
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

    public void OpenRubyShop()
    {
        obj_shop.SetActive(true);
        ClickShopBtn(3);
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
        list_buyWaiting = _list;//구매 물품 저장
        waitingPrice = _price;//구매 물품 가격
        waitingPriceType = _priceType;//구매 가격 재화 타입
        int count = list_productImage.Count;// 이미지 다 꺼주기
        for (int iNum = 0; iNum < count; iNum++)
        {
            list_productImage[iNum].gameObject.SetActive(false);
        }

        count = _list.Count;
        for (int iNum = 0; iNum < count; iNum++)// 이미지 갯수에 맞게 켜주고 이미지 변경
        {
            list_productImage[iNum].gameObject.SetActive(true);
            list_productImage[iNum].sprite = _list[iNum].sprite;
            list_productCountText[iNum].text = _list[iNum].count;
        }
        text_price.text = _price;
    }

    public void ClickBuyYes()
    {
        int count;
        switch (waitingPriceType)
        {
            case ProductTag.Gold:
                string convertGold = CalCulator.inst.ConvertChartoIndex(waitingPrice);//문자로 표기돼있는 숫자를 풀어서 반환
                BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
                if (haveGold >= BigInteger.Parse(convertGold))
                {
                    GameStatus.inst.MinusGold(convertGold);
                    count = list_buyWaiting.Count;
                    for (int iNum = 0; iNum < count; iNum++)// 구매
                    {
                        list_buyWaiting[iNum].buyProduct();
                    }
                    obj_BuyCheck.SetActive(false);
                }
                else
                {
                    Debug.Log("골드가 부족합니다");
                }
                break;
            case ProductTag.Ruby:
                int haveRuby = int.Parse(GameStatus.inst.Gold);
                int iPrice = int.Parse(waitingPrice);
                if (haveRuby >= iPrice)
                {
                    GameStatus.inst.Ruby -= iPrice;
                    count = list_buyWaiting.Count;
                    for (int iNum = 0; iNum < count; iNum++)//구매
                    {
                        list_buyWaiting[iNum].buyProduct();
                    }
                    obj_BuyCheck.SetActive(false);
                }
                else
                {
                    Debug.Log("루비가 부족합니다");
                }
                break;
            case ProductTag.Star:
                string convertStar = CalCulator.inst.ConvertChartoIndex(waitingPrice);//문자로 표기돼있는 숫자를 풀어서 반환
                BigInteger haveStar = BigInteger.Parse(GameStatus.inst.Gold);
                if (haveStar >= BigInteger.Parse(convertStar))
                {
                    //별 갯수 차감
                    count = list_buyWaiting.Count;// 구매
                    for (int iNum = 0; iNum < count; iNum++)
                    {
                        list_buyWaiting[iNum].buyProduct();
                    }
                    obj_BuyCheck.SetActive(false);
                }
                else
                {
                    Debug.Log("별이 부족합니다");
                }
                break;
        }
        
    }
}
