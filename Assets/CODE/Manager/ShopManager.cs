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
    GameObject obj_shop;// ���� ������Ʈ
    Button mainShopCloseBtn;// ���� �ݱ��ư
    GameObject[] list_ShopUi;// ���� ����Ʈ
    Image[] list_BottomBtn;//���� �ϴ� ��ư ����Ʈ
    int botBtnNum = 0;// ���� �ϴܹ�ư ��ȣ
    [SerializeField] GameObject obj_Product;
    [SerializeField] GameObject obj_EmptyImage;

    [Header("��ǰ ����")]
    GameObject obj_BuyCheck;
    Button BuyYesBtn;
    TMP_Text text_price;
    Transform productImageParents;//��ǰ �̹��� �θ�
    List<Image> list_productImage = new List<Image>();//��ǰ �̹��� ����Ʈ
    List<TMP_Text> list_productCountText = new List<TMP_Text>();//��ǰ ���� �ؽ�Ʈ

    [Header("��ǰ ���")]
    Transform GoldShopParnets;
    Transform RubyShopParnets;
    [SerializeField] List<ProductList> list_GoldProductList;
    [SerializeField] List<ProductList> list_RubyProductList;
    [Serializable]
    public class ProductList
    {
        [Header("��ǰ ���")]
        [SerializeField] List<Product> list_product = new List<Product>();
        [Header("��ǰ ����")]
        [SerializeField] string price;
        [Header("���� Ÿ��")]
        [SerializeField] ProductTag priceType;
        [Space]

        Transform trs;
        public Transform Trs { get => trs; set { trs = value; } }
        TMP_Text priceText;
        Transform imageParents;
        Image priceImage;
        List<Image> list_rewordImage = new List<Image>();
        List<TMP_Text> list_rewordText = new List<TMP_Text>();


        [Serializable]
        public class Product
        {
            [SerializeField] public ProductTag prodtag;
            [SerializeField] public string count;

            public void buyProduct()
            {
                switch (prodtag)
                {
                    case ProductTag.Gold:
                        GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(count));
                        break;
                    case ProductTag.Ruby:
                        GameStatus.inst.Ruby += int.Parse(count);
                        break;
                    case ProductTag.Star:
                        GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(count));
                        break;
                }
            }

            public Sprite GetSprite()
            {
                switch (prodtag)
                {
                    case ProductTag.Gold:
                        return UIManager.Instance.GetProdSprite(0);

                    case ProductTag.Star:
                        return UIManager.Instance.GetProdSprite(1);

                    case ProductTag.Ruby:
                        return UIManager.Instance.GetProdSprite(2);

                }
                return null;
            }
        }

        public void InitStart()
        {
            priceText = Trs.Find("PriceText").GetComponent<TMP_Text>();
            imageParents = Trs.Find("ProductList");
            priceImage = Trs.Find("priceImage").GetComponent<Image>();
            trs.GetComponent<Button>().onClick.AddListener(ClickBuy);

            priceText.text = price;
            priceImage.sprite = UIManager.Instance.GetProdSprite((int)priceType);
            int prodCount = list_product.Count;//��ǰ �̹��� ����
            for (int iNum = 0; iNum < prodCount; iNum++)
            {
                GameObject obj = Instantiate(Instance.GetEmptyImage(), imageParents);
                list_rewordImage.Add(obj.transform.Find("Image").GetComponent<Image>());
                list_rewordText.Add(obj.transform.Find("RewordText").GetComponent<TMP_Text>());
                list_rewordImage[iNum].sprite = list_product[iNum].GetSprite();
                list_rewordText[iNum].text = list_product[iNum].count;
            }
        }

        public void ClickBuy()
        {
            Instance.SetCheckBuy(list_product, price, priceType);
        }
    }



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
        obj_shop = GameObject.Find("---[UI Canvas]").transform.Find("Shop").gameObject;
        mainShopCloseBtn = obj_shop.transform.Find("MainShopClosdBtn").GetComponent<Button>();

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
        int BotBtnCount = trsShopParent.childCount;
        list_BottomBtn = new Image[BotBtnCount];
        for (int iNum = 0; iNum < BotBtnCount; iNum++)
        {
            list_BottomBtn[iNum] = trsBotBtn.GetChild(iNum).GetComponent<Image>();
        }

        GoldShopParnets = obj_shop.transform.Find("ShopList/Gold/Scroll View").GetComponent<ScrollRect>().content;
        RubyShopParnets = obj_shop.transform.Find("ShopList/Ruby/Scroll View").GetComponent<ScrollRect>().content;

        //������ ��ǰ �ʱ�ȭ
        int goldShopCount = list_GoldProductList.Count;
        for (int iNum = 0; iNum < goldShopCount; iNum++)
        {
            Transform trs = Instantiate(obj_Product, GoldShopParnets).transform;
            list_GoldProductList[iNum].Trs = trs;
            list_GoldProductList[iNum].InitStart();
        }

        //������ ��ǰ �ʱ�ȭ
        int rubyShopCount = list_RubyProductList.Count;
        for (int iNum = 0; iNum < rubyShopCount; iNum++)
        {
            Transform trs = Instantiate(obj_Product, RubyShopParnets).transform;
            list_RubyProductList[iNum].Trs = trs;
            list_RubyProductList[iNum].InitStart();
        }

        obj_BuyCheck = obj_shop.transform.Find("CheckBuy").gameObject;
        text_price = obj_BuyCheck.transform.Find("PriceText").GetComponent<TMP_Text>();
        productImageParents = obj_BuyCheck.transform.Find("ProductImage");
        BuyYesBtn = obj_BuyCheck.transform.Find("YesBtn").GetComponent<Button>();

        mainShopCloseBtn.onClick.AddListener(() => UIManager.Instance.changeSortOder(0));

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
        ClickShopBtn(3);
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
        }
        else
        {
            CrewGatchaContent.inst.CrewMaterialGatchaActive(false);
            list_BottomBtn[botBtnNum].sprite = UIManager.Instance.GetSelectUISprite(0);
            list_ShopUi[botBtnNum].SetActive(false);

            botBtnNum = _num;
            list_BottomBtn[botBtnNum].sprite = UIManager.Instance.GetSelectUISprite(1);
            list_ShopUi[botBtnNum].SetActive(true);
        }
    }


    public void SetCheckBuy(List<ProductList.Product> _list, string _price, ProductTag _priceType)
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
        text_price.text = _price;
    }
}

