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

    [Header("����")]
    [SerializeField] GameObject obj_Product;
    [SerializeField] GameObject obj_EmptyImage;
    GameObject[] list_ShopUi;// ���� ����Ʈ
    GameObject obj_shop;// ���� ������Ʈ
    Button mainShopCloseBtn;// ���� �ݱ��ư
    Image[] list_BottomBtn;//���� �ϴ� ��ư ����Ʈ
    TMP_Text GoldText;
    TMP_Text StarText;
    TMP_Text RubyText;
    int botBtnNum = 0;// ���� �ϴܹ�ư ��ȣ

    [Header("��ǰ ����")]
    GameObject obj_BuyCheck;
    Button BuyYesBtn;
    Transform productImageParents;//��ǰ �̹��� �θ�
    List<Image> list_productImage = new List<Image>();//��ǰ �̹��� ����Ʈ
    List<TMP_Text> list_productCountText = new List<TMP_Text>();//��ǰ ���� �ؽ�Ʈ

    [Header("��ǰ ���")]
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

        //���� ����Ʈ �ʱ�ȭ
        Transform trsShopParent = obj_shop.transform.Find("ShopList");
        int shopCount = trsShopParent.childCount;
        list_ShopUi = new GameObject[shopCount];
        for (int iNum = 0; iNum < shopCount; iNum++)
        {
            list_ShopUi[iNum] = trsShopParent.GetChild(iNum).gameObject;
        }

        //���� �ϴܹ�ư �ʱ�ȭ
        Transform trsBotBtn = obj_shop.transform.Find("BotBtn");
        int BotBtnCount = trsBotBtn.childCount;
        list_BottomBtn = new Image[BotBtnCount];
        for (int iNum = 0; iNum < BotBtnCount; iNum++)
        {
            list_BottomBtn[iNum] = trsBotBtn.GetChild(iNum).GetComponent<Image>();
        }

        GoldShopParnets = obj_shop.transform.Find("ShopList/Gold/Scroll View").GetComponent<ScrollRect>().content;
        RubyShopParnets = obj_shop.transform.Find("ShopList/Ruby/Scroll View").GetComponent<ScrollRect>().content;

        //����Ȯ��â �ʱ�ȭ
        obj_BuyCheck = obj_shop.transform.Find("CheckBuy").gameObject;
        productImageParents = obj_BuyCheck.transform.Find("BackGround/ProductImage");
        BuyYesBtn = obj_BuyCheck.transform.Find("BackGround/YesBtn").GetComponent<Button>();

        mainShopCloseBtn.onClick.AddListener(() => 
        { 
            UIManager.Instance.changeSortOder(17);
            AdMarket.inst.ActiveAdMarket(false);
        });
        GoldText.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Gold)}";
        GameStatus.inst.OnGoldChanged.AddListener(() => { GoldText.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Gold)}"; });//���� ��� �ؽ�Ʈ ����
        StarText.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Star)}";
        GameStatus.inst.OnStartChanged.AddListener(() => { StarText.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Star)}"; });//���� �� �ؽ�Ʈ ����
        RubyText.text = $"{GameStatus.inst.Ruby}";
        GameStatus.inst.OnRubyChanged.AddListener(() => { RubyText.text = $"{GameStatus.inst.Ruby}"; });//���� ����ؽ�Ʈ ����

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

        //���� ���¹�ư �ʱ�ȭ
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
    /// 0 = ������, 1 = ��������, 2 = ����� ����, 3 = ���� ����, 4 = ������
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
                string convertGold = CalCulator.inst.ConvertChartoIndex(_price);//���ڷ� ǥ����ִ� ���ڸ� Ǯ� ��ȯ
                BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
                if (haveGold >= BigInteger.Parse(convertGold))
                {
                    BuyYesBtn.onClick.AddListener(() =>
                    {
                        GameStatus.inst.MinusGold(convertGold);
                        count = _list.Count;
                        for (int iNum = 0; iNum < count; iNum++)// ����
                        {
                            _list[iNum].buyProduct();
                        }
                        obj_BuyCheck.SetActive(false);
                        BuyYesBtn.onClick.RemoveAllListeners();
                    });

                }
                else
                {
                    Debug.Log("��尡 �����մϴ�");
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
                        for (int iNum = 0; iNum < count; iNum++)//����
                        {
                            _list[iNum].buyProduct();
                        }
                        obj_BuyCheck.SetActive(false);
                        BuyYesBtn.onClick.RemoveAllListeners();
                    });
                }
                else
                {
                    Debug.Log("��� �����մϴ�");
                    return;
                }
                break;
            case ProductTag.Star:
                string convertStar = CalCulator.inst.ConvertChartoIndex(_price);//���ڷ� ǥ����ִ� ���ڸ� Ǯ� ��ȯ
                BigInteger haveStar = BigInteger.Parse(GameStatus.inst.Star);
                if (haveStar >= BigInteger.Parse(convertStar))
                {
                    BuyYesBtn.onClick.AddListener(() =>
                    {
                        //GameStatus.inst.star(convertStar);
                        count = _list.Count;// ����
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
                    Debug.Log("���� �����մϴ�");
                    return;
                }
                break;
        }

        obj_BuyCheck.SetActive(true);

        count = list_productImage.Count;// �̹��� �� ���ֱ�
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (list_productImage[iNum].gameObject.activeSelf)
            {
                list_productImage[iNum].transform.parent.gameObject.SetActive(false);
            }
        }

        count = _list.Count;
        for (int iNum = 0; iNum < count; iNum++)// �̹��� ������ �°� ���ְ� �̹��� ����
        {
            list_productImage[iNum].transform.parent.gameObject.SetActive(true);
            list_productImage[iNum].sprite = _list[iNum].GetSprite();
            list_productCountText[iNum].text = _list[iNum].count;
        }
    }
}

