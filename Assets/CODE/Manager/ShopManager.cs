using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Numerics;
using System.Collections.Generic;


public class ShopManager : MonoBehaviour
{
    public static ShopManager inst;

    [SerializeField] List<Product> list_GoldProduct;
    [SerializeField] List<Product> list_RubyProduct;
    [SerializeField] List<AdProduct> list_AdProduct;

    [Serializable]
    public class Product
    {
        [SerializeField] ProductTag PriceType;
        [SerializeField] string Price;
        [SerializeField] ProductTag ProductType;
        [SerializeField] int count;
        BigInteger prodCount;
        Transform trs;
        TMP_Text PriceText;
        TMP_Text ProductText;
        Button BuyBtn;
        Image ProdImage;


        public void initProduct(Transform _trs)
        {
            trs = _trs;
            BuyBtn = trs.Find("Button").GetComponent<Button>();
            ProdImage = trs.Find("ProductImage").GetComponent<Image>();
            PriceText = trs.Find("Button/PriceText").GetComponent<TMP_Text>();
            ProductText = trs.Find("RewardText").GetComponent<TMP_Text>();
            PriceText.text = string.Format("{0:#,0}", Price);
            if (PriceType == ProductTag.Money)
            {
                PriceText.text += "��";
            }

            UIManager.Instance.onOpenShop.AddListener(() =>
            {
                switch (ProductType)
                {
                    case ProductTag.Gold:
                        prodCount = GameStatus.inst.TotalProdGold * count;
                        ProductText.text = CalCulator.inst.StringFourDigitAddFloatChanger(prodCount.ToString());

                        break;
                    case ProductTag.Star:

                        break;
                    case ProductTag.Ruby:
                        ProductText.text = count.ToString("N0") + "��";
                        break;
                }
            });

            BuyBtn.onClick.AddListener(() =>
            {



                //����Ÿ�Կ� �´� �����ڻ� üũ
                switch (PriceType)
                {
                    case ProductTag.Gold:
                        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
                        BigInteger price = BigInteger.Parse(CalCulator.inst.ConvertChartoIndex(Price));
                        if (haveGold < price)
                        {
                            return;
                        }
                        break;
                    case ProductTag.Star:

                        break;
                    case ProductTag.Ruby:
                        int haveRuby = GameStatus.inst.Ruby;
                        if (haveRuby < int.Parse(Price))
                        {
                            return;
                        }
                        break;

                    case ProductTag.Money:
                        //����
                        break;
                }

                //��ǰ ������ ���� �׼� ���
                inst.ClickProduct(ProdImage.sprite, ProductText.text, () =>
                {
                    //��� ����
                    switch (PriceType)
                    {
                        case ProductTag.Gold:
                            GameStatus.inst.MinusGold(CalCulator.inst.ConvertChartoIndex(Price));
                            break;
                        case ProductTag.Star:
                            GameStatus.inst.MinusStar(CalCulator.inst.ConvertChartoIndex(Price));
                            break;
                        case ProductTag.Ruby:
                            GameStatus.inst.Ruby -= int.Parse(Price);
                            break;

                        case ProductTag.Money:
                            //����
                            break;
                    }

                    //��ǰ ����
                    switch (ProductType)
                    {
                        case ProductTag.Gold:
                            GameStatus.inst.PlusGold(prodCount.ToString());
                            break;
                        case ProductTag.Star:
                            GameStatus.inst.PlusStar(prodCount.ToString());
                            break;
                        case ProductTag.Ruby:
                            GameStatus.inst.PlusRuby(count);
                            break;
                    }
                });

            });
        }
    }

    [Serializable]
    public class AdProduct
    {
        [SerializeField] ProductTag ProductType;
        [SerializeField] int count;
        BigInteger prodCount;
        Transform trs;
        TMP_Text ProductText;
        TMP_Text BuyBtnText;
        Button BuyBtn;

        public void initProduct(Transform _trs)
        {
            trs = _trs;
            BuyBtn = trs.Find("Button").GetComponent<Button>();
            ProductText = trs.Find("RewardText").GetComponent<TMP_Text>();
            BuyBtnText = trs.Find("Button/PriceText").GetComponent<TMP_Text>();

            inst.onDailyReset.AddListener(() => { ResetAdBtn(GameStatus.inst.AdViewrAdShopData); });


            UIManager.Instance.onOpenShop.AddListener(() =>
            {
                switch (ProductType)
                {
                    case ProductTag.Gold:
                        prodCount = GameStatus.inst.TotalProdGold * count;
                        ProductText.text = CalCulator.inst.StringFourDigitAddFloatChanger(prodCount.ToString());

                        break;
                    case ProductTag.Star:

                        break;
                    case ProductTag.Ruby:
                        ProductText.text = count.ToString();
                        break;

                }
            });

            BuyBtn.onClick.AddListener(() =>
            {
                BuyBtn.interactable = false;
                BuyBtnText.text = "��� ����";
                //��ǰ ������ ���� �׼� ���
                switch (ProductType)
                {
                    case ProductTag.Gold:
                        ADViewManager.inst.AdMob_ActiveAndFuntion(() => { GameStatus.inst.PlusGold(prodCount.ToString()); });

                        break;
                    case ProductTag.Star:

                        break;
                    case ProductTag.Ruby:
                        ADViewManager.inst.AdMob_ActiveAndFuntion(() => { GameStatus.inst.PlusRuby(count); });

                        break;
                }

                GameStatus.inst.Ad_Viewr_AdShopDataDateValue(_trs.GetSiblingIndex(), DateTime.Now);
            });
        }

        public void ResetAdBtn(string[] dateValue)
        {
            int index = trs.GetSiblingIndex();
            if (dateValue[index] != string.Empty)
            {
                DateTime adShopDate = DateTime.Parse(dateValue[index]);

                if (adShopDate.Date < DateTime.Now.Date)
                {
                    BuyBtn.interactable = true;
                    BuyBtnText.text = "���� ��û";
                }
                else if (adShopDate.Date <= DateTime.Now.Date)
                {
                    BuyBtn.interactable = false;
                    BuyBtnText.text = "��û �Ϸ�";
                }
            }
            else
            {
                BuyBtn.interactable = true;
                BuyBtnText.text = "���� ��û";
            }

        }
    }
    //////////////////// < �ν����� ���� > ////////////////////////

    [Header("# BotArrayBtn Image <color=yellow>(Sprite)</color>")]
    [Space]
    [SerializeField]
    Sprite[] botArr_NonClickImage;
    [SerializeField]
    Sprite[] botArr_ClickImage;

    //////////////////////////////////////////////////////////////


    GameObject shopRef;
    Transform shopListRef;

    public GameObject ShopRef => shopRef; // ���� �۾� �����

    Button[] botArrBtn; // ���� �ϴ� ��ư
    Image[] botArrImage; // ���� �ϴ� �̵� ��ư �̹���
    TMP_Text[] botArrText;
    TMP_Text curRubyText;

    //����Ȯ��â
    GameObject ObjCheckBuy;
    Button BuyYesBtn;
    Image ProdImage;
    TMP_Text ProdText;

    [HideInInspector] public UnityEvent onDailyReset;

    [Tooltip("0��í/1������/2������/3�������")] GameObject[] shopListChildRef;

    // ���� �����ִ� ��ȣ
    int curSelectMenu = -1;

    private void Awake()
    {
        //�̱���
        #region
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion

        shopRef = transform.parent.Find("ScreenArea/BackGround/Shop").gameObject;
        shopListRef = shopRef.transform.Find("Shop_List");

        // ���� ����Ʈ �ʱ�ȭ
        shopListChildRef = new GameObject[shopListRef.childCount - 1]; // CurRuby�� ����
        for (int index = 0; index < shopListChildRef.Length; index++)
        {
            shopListChildRef[index] = shopListRef.GetChild(index).gameObject;
        }

        // ���� ������� ���� �����Ȳ �ؽ�Ʈ
        curRubyText = shopListRef.Find("CurRuby/Box/Text (TMP)").GetComponent<TMP_Text>();

        //���� �ϴ� ��ư�ʱ�ȭ
        botArrBtn = shopRef.transform.Find("ShopBottomBtn").GetComponentsInChildren<Button>();
        botArrImage = new Image[botArrBtn.Length];
        botArrText = new TMP_Text[botArrBtn.Length];
        for (int index = 0; index < botArrBtn.Length; index++)
        {
            botArrImage[index] = botArrBtn[index].GetComponent<Image>();
            botArrText[index] = botArrImage[index].GetComponentInChildren<TMP_Text>();
        }

        //������ �ʱ�ȭ
        Transform GoldProdParents = shopListChildRef[1].transform.Find("ProductList");
        int GoldShopCount = GoldProdParents.childCount;
        for (int iNum = 0; iNum < GoldShopCount; iNum++)
        {
            list_GoldProduct[iNum].initProduct(GoldProdParents.GetChild(iNum));
        }

        //������ �ʱ�ȭ
        Transform RubyProdParents = shopListChildRef[2].transform.Find("ProductList");
        int RubyShopCount = RubyProdParents.childCount;
        for (int iNum = 0; iNum < RubyShopCount; iNum++)
        {
            list_RubyProduct[iNum].initProduct(RubyProdParents.GetChild(iNum));
        }

        //������� �ʱ�ȭ
        Transform AdProdParents = shopListChildRef[3].transform.Find("ProductList");
        int AdShopCount = AdProdParents.childCount;
        for (int iNum = 0; iNum < AdShopCount; iNum++)
        {
            list_AdProduct[iNum].initProduct(AdProdParents.GetChild(iNum));
        }
        //���� Ȯ��â �ʱ�ȭ
        ObjCheckBuy = transform.parent.Find("ScreenArea/BackGround/CheckBuyWindow").gameObject;
        BuyYesBtn = ObjCheckBuy.transform.Find("YesBtn").GetComponent<Button>();
        ProdImage = ObjCheckBuy.transform.Find("ProductImage").GetComponent<Image>();
        ProdText = ObjCheckBuy.transform.Find("ProductText").GetComponent<TMP_Text>();


        Btn_Init();
    }

    private void Start()
    {
        onDailyReset?.Invoke();
    }

    private void Btn_Init()
    {
        // �ϴ� �����̵� ��ư�� �ʱ�ȭ
        for (int index = 0; index < botArrBtn.Length; index++)
        {
            int curIndex = index;
            botArrBtn[curIndex].onClick.AddListener(() => Active_Shop(curIndex, true));
        }

        // 
    }
    private void Update()
    {

    }


    /// <summary>
    /// ����ȣ��
    /// </summary>
    /// <param name="ShopTypeNumber">0�̱�/1���/2���/3���� </param>
    public void Active_Shop(int ShopTypeNumber, bool active)
    {
        if (active) // �ش� ���� ȣ��
        {
            AudioManager.inst.noSound = true;
            // ���� �Ѿ�°� �ƴ϶�� ��ư�� ���
            if (curSelectMenu != -1 && curSelectMenu != ShopTypeNumber)
            {
                AudioManager.inst.Play_Ui_SFX(4, 0.8f);
            }

            //���� ��ư �� Ŭ���� ����
            if (curSelectMenu == ShopTypeNumber) { return; }

            //���� ���õǾ��ִ� ���� ����
            curSelectMenu = ShopTypeNumber;

            //�������� startInit
            switch (curSelectMenu)
            {
                //�̱�����̶��
                case 0:
                    Shop_Gacha.inst.Init(true);
                    break;
            }

            // �Һ��� �ʱ�ȭ
            curRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Ruby.ToString());

            // â �����ֱ�
            for (int index = 0; index < shopListChildRef.Length; index++)
            {
                if (index == ShopTypeNumber)
                {
                    shopListChildRef[index].SetActive(true);
                }
                else
                {
                    shopListChildRef[index].SetActive(false);
                }
            }

            // �̹��� ����
            BotArrBtn_ImageChanger(ShopTypeNumber);
        }
        else // ���� ����
        {
            switch (curSelectMenu)
            {
                //�̱�����̶��
                case 0:
                    Shop_Gacha.inst.Init(false);
                    break;
            }

            curSelectMenu = -1;
            AudioManager.inst.noSound = false;
            shopRef.SetActive(false);
        }
    }




    float clickFontsize = 11.5f;
    float nonclickFontsize = 10f;
    // �ϴܹ�ư �̹��� ����
    private void BotArrBtn_ImageChanger(int selectBtn)
    {
        for (int index = 0; index < botArrImage.Length; index++)
        {
            if (index == selectBtn)
            {
                botArrImage[index].sprite = botArr_ClickImage[index];
                botArrText[index].fontSize = clickFontsize;
            }
            else
            {
                botArrImage[index].sprite = botArr_NonClickImage[index];
                botArrText[index].fontSize = nonclickFontsize;
            }
        }
    }

    public void ShopRubyTextInit()
    {
        curRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Ruby.ToString());
    }

    public void ClickProduct(Sprite prodSprite, string prodText, UnityAction action)
    {
        ObjCheckBuy.SetActive(true);
        ProdText.text = prodText;
        BuyYesBtn.onClick.RemoveAllListeners();
        BuyYesBtn.onClick.AddListener(() => { ObjCheckBuy.SetActive(false); });
        BuyYesBtn.onClick.AddListener(action);
        ProdImage.sprite = prodSprite;
        ProdImage.SetNativeSize();
        float ratio = ProdImage.rectTransform.sizeDelta.x / ProdImage.rectTransform.sizeDelta.y;
        ProdImage.rectTransform.sizeDelta = new UnityEngine.Vector2(50f * ratio, 50f);
    }
}

