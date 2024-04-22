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
    [SerializeField] GameObject obj_Product;
    [SerializeField] GameObject obj_EmptyImage;
    GameObject[] list_ShopUi;// 상점 리스트
    GameObject obj_shop;// 상점 오브젝트
    Button mainShopCloseBtn;// 상점 닫기버튼
    Image[] list_BottomBtn;//상점 하단 버튼 리스트
    TMP_Text GoldText;
    TMP_Text StarText;
    TMP_Text RubyText;
    int botBtnNum = 0;// 상점 하단버튼 번호

    [Header("상품 구매")]
    GameObject obj_BuyCheck;
    Button BuyYesBtn;
    Transform productImageParents;//상품 이미지 부모
    List<Image> list_productImage = new List<Image>();//상품 이미지 리스트
    List<TMP_Text> list_productCountText = new List<TMP_Text>();//상품 갯수 텍스트

    [Header("상품 목록")]
    Transform GoldShopParnets;
    Transform RubyShopParnets;




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
        obj_shop = GameObject.Find("---[UI Canvas]").transform.Find("ScreenArea/Shop").gameObject;
        mainShopCloseBtn = obj_shop.transform.Find("MainShopClosdBtn").GetComponent<Button>();
        GoldText = obj_shop.transform.Find("TopUi/MoneyText/Gold/Text").GetComponent<TMP_Text>();
        StarText = obj_shop.transform.Find("TopUi/MoneyText/Star/Text").GetComponent<TMP_Text>();
        RubyText = obj_shop.transform.Find("TopUi/MoneyText/Ruby/Text").GetComponent<TMP_Text>();

        //상점 리스트 초기화
        Transform trsShopParent = obj_shop.transform.Find("ShopList");
        int shopCount = trsShopParent.childCount;
        list_ShopUi = new GameObject[shopCount];
        for (int iNum = 0; iNum < shopCount; iNum++)
        {
            list_ShopUi[iNum] = trsShopParent.GetChild(iNum).gameObject;
        }

        //상점 하단버튼 초기화
        Transform trsBotBtn = obj_shop.transform.Find("BotBtn");
        int BotBtnCount = trsBotBtn.childCount;
        list_BottomBtn = new Image[BotBtnCount];
        for (int iNum = 0; iNum < BotBtnCount; iNum++)
        {
            list_BottomBtn[iNum] = trsBotBtn.GetChild(iNum).GetComponent<Image>();
        }

        GoldShopParnets = obj_shop.transform.Find("ShopList/Gold/Scroll View").GetComponent<ScrollRect>().content;
        RubyShopParnets = obj_shop.transform.Find("ShopList/Ruby/Scroll View").GetComponent<ScrollRect>().content;

        //구매확인창 초기화
        obj_BuyCheck = obj_shop.transform.Find("CheckBuy").gameObject;
        productImageParents = obj_BuyCheck.transform.Find("BackGround/ProductImage");
        BuyYesBtn = obj_BuyCheck.transform.Find("BackGround/YesBtn").GetComponent<Button>();

        mainShopCloseBtn.onClick.AddListener(() => 
        { 
            UIManager.Instance.changeSortOder(17);
            AdMarket.inst.ActiveAdMarket(false);
        });
        GoldText.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Gold)}";
        GameStatus.inst.OnGoldChanged.AddListener(() => { GoldText.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Gold)}"; });//상점 골드 텍스트 갱신
        StarText.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Star)}";
        GameStatus.inst.OnStartChanged.AddListener(() => { StarText.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Star)}"; });//상점 별 텍스트 갱신
        RubyText.text = $"{GameStatus.inst.Ruby}";
        GameStatus.inst.OnRubyChanged.AddListener(() => { RubyText.text = $"{GameStatus.inst.Ruby}"; });//상점 골드텍스트 갱신

        int productImageCount = productImageParents.childCount;
        for (int iNum = 0; iNum < productImageCount; iNum++)
        {
            list_productImage.Add(productImageParents.GetChild(iNum).Find("Image").GetComponent<Image>());
        }

        int productImageTextCount = list_productImage.Count;
        for (int iNum = 0; iNum < productImageTextCount; iNum++)
        {
            list_productCountText.Add(productImageParents.GetChild(iNum).Find("RewordText").GetComponent<TMP_Text>());
        }

        //상점 오픈버튼 초기화
        UIManager.Instance.GetShopOpenBtn().onClick.AddListener(() => 
        {
            if (botBtnNum == 2)
            {
                CrewGatchaContent.inst.CrewMaterialGatchaActive(true);
            }
            else if (botBtnNum == 3)
            {
                AdMarket.inst.ActiveAdMarket(true);
            }
        });
    }

    public GameObject GetEmptyImage()
    {
        return obj_EmptyImage;
    }

    public GameObject GetProduct()
    {
        return obj_Product;
    }
    public void OpenRubyShop()
    {
        obj_shop.SetActive(true);
        ClickShopBtn(4);
    }
    /// <summary>
    /// 0 = 골드상점, 1 = 유물상점, 2 = 펫재료 상점, 3 = 광고 상점, 4 = 루비상점
    /// </summary>
    /// <param name="num"></param>
    public void OpenShop(int num)
    {
        obj_shop.SetActive(true);
        ClickShopBtn(num);
    }

    public void SetShopActive(bool value)
    {
        obj_shop.SetActive(value);
    }

    public void ClickShopBtn(int _num)
    {
        if (_num == 2)
        {
            CrewGatchaContent.inst.CrewMaterialGatchaActive(true);
            AdMarket.inst.ActiveAdMarket(false);
            list_BottomBtn[botBtnNum].sprite = UIManager.Instance.GetSelectUISprite(0);
            list_ShopUi[botBtnNum].SetActive(false);
            botBtnNum = _num;
            list_BottomBtn[botBtnNum].sprite = UIManager.Instance.GetSelectUISprite(1);
        }
        else if (_num == 3)
        {
            AdMarket.inst.ActiveAdMarket(true);
            CrewGatchaContent.inst.CrewMaterialGatchaActive(false);
            list_BottomBtn[botBtnNum].sprite = UIManager.Instance.GetSelectUISprite(0);
            list_ShopUi[botBtnNum].SetActive(false);
            botBtnNum = _num;
            list_BottomBtn[botBtnNum].sprite = UIManager.Instance.GetSelectUISprite(1);
        }
        else
        {
            CrewGatchaContent.inst.CrewMaterialGatchaActive(false);
            AdMarket.inst.ActiveAdMarket(false);
            list_BottomBtn[botBtnNum].sprite = UIManager.Instance.GetSelectUISprite(0);
            list_ShopUi[botBtnNum].SetActive(false);

            botBtnNum = _num;
            list_BottomBtn[botBtnNum].sprite = UIManager.Instance.GetSelectUISprite(1);
            list_ShopUi[botBtnNum].SetActive(true);
        }
    }


    public void SetCheckBuy(List<Product.ProductList> _list, string _price, ProductTag _priceType)
    {
        int count;
        switch (_priceType)
        {
            case ProductTag.Gold:
                string convertGold = CalCulator.inst.ConvertChartoIndex(_price);//문자로 표기돼있는 숫자를 풀어서 반환
                BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
                if (haveGold >= BigInteger.Parse(convertGold))
                {
                    BuyYesBtn.onClick.AddListener(() =>
                    {
                        GameStatus.inst.MinusGold(convertGold);
                        count = _list.Count;
                        for (int iNum = 0; iNum < count; iNum++)// 구매
                        {
                            _list[iNum].buyProduct();
                        }
                        obj_BuyCheck.SetActive(false);
                        BuyYesBtn.onClick.RemoveAllListeners();
                    });

                }
                else
                {
                    Debug.Log("골드가 부족합니다");
                    return;
                }
                break;
            case ProductTag.Ruby:
                int haveRuby = GameStatus.inst.Ruby;
                int iPrice = int.Parse(_price);
                if (haveRuby >= iPrice)
                {
                    BuyYesBtn.onClick.AddListener(() =>
                    {
                        GameStatus.inst.Ruby -= iPrice;
                        count = _list.Count;
                        for (int iNum = 0; iNum < count; iNum++)//구매
                        {
                            _list[iNum].buyProduct();
                        }
                        obj_BuyCheck.SetActive(false);
                        BuyYesBtn.onClick.RemoveAllListeners();
                    });
                }
                else
                {
                    Debug.Log("루비가 부족합니다");
                    return;
                }
                break;
            case ProductTag.Star:
                string convertStar = CalCulator.inst.ConvertChartoIndex(_price);//문자로 표기돼있는 숫자를 풀어서 반환
                BigInteger haveStar = BigInteger.Parse(GameStatus.inst.Star);
                if (haveStar >= BigInteger.Parse(convertStar))
                {
                    BuyYesBtn.onClick.AddListener(() =>
                    {
                        //GameStatus.inst.star(convertStar);
                        count = _list.Count;// 구매
                        for (int iNum = 0; iNum < count; iNum++)
                        {
                            _list[iNum].buyProduct();
                        }
                        obj_BuyCheck.SetActive(false);
                        BuyYesBtn.onClick.RemoveAllListeners();
                    });
                }
                else
                {
                    Debug.Log("별이 부족합니다");
                    return;
                }
                break;
        }

        obj_BuyCheck.SetActive(true);

        count = list_productImage.Count;// 이미지 다 꺼주기
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (list_productImage[iNum].gameObject.activeSelf)
            {
                list_productImage[iNum].transform.parent.gameObject.SetActive(false);
            }
        }

        count = _list.Count;
        for (int iNum = 0; iNum < count; iNum++)// 이미지 갯수에 맞게 켜주고 이미지 변경
        {
            list_productImage[iNum].transform.parent.gameObject.SetActive(true);
            list_productImage[iNum].sprite = _list[iNum].GetSprite();
            list_productCountText[iNum].text = _list[iNum].count;
        }
    }
}

