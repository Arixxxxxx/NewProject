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
                PriceText.text += "원";
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
                        ProductText.text = count.ToString("N0") + "개";
                        break;
                }
            });

            BuyBtn.onClick.AddListener(() =>
            {



                //가격타입에 맞는 보유자산 체크
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
                        //결제
                        break;
                }

                //상품 종류에 따른 액션 등록
                inst.ClickProduct(ProdImage.sprite, ProductText.text, () =>
                {
                    //비용 차감
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
                            //결제
                            break;
                    }

                    //상품 지급
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
                BuyBtnText.text = "재고 소진";
                //상품 종류에 따른 액션 등록
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
                    BuyBtnText.text = "광고 시청";
                }
                else if (adShopDate.Date <= DateTime.Now.Date)
                {
                    BuyBtn.interactable = false;
                    BuyBtnText.text = "시청 완료";
                }
            }
            else
            {
                BuyBtn.interactable = true;
                BuyBtnText.text = "광고 시청";
            }

        }
    }
    //////////////////// < 인스펙터 참조 > ////////////////////////

    [Header("# BotArrayBtn Image <color=yellow>(Sprite)</color>")]
    [Space]
    [SerializeField]
    Sprite[] botArr_NonClickImage;
    [SerializeField]
    Sprite[] botArr_ClickImage;

    //////////////////////////////////////////////////////////////


    GameObject shopRef;
    Transform shopListRef;

    public GameObject ShopRef => shopRef; // 동은 작업 연결용

    Button[] botArrBtn; // 상점 하단 버튼
    Image[] botArrImage; // 상점 하단 이동 버튼 이미지
    TMP_Text[] botArrText;
    TMP_Text curRubyText;

    //구매확인창
    GameObject ObjCheckBuy;
    Button BuyYesBtn;
    Image ProdImage;
    TMP_Text ProdText;

    [HideInInspector] public UnityEvent onDailyReset;

    [Tooltip("0갓챠/1골드상점/2루비상점/3광고상점")] GameObject[] shopListChildRef;

    // 현재 눌려있는 번호
    int curSelectMenu = -1;

    private void Awake()
    {
        //싱글톤
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

        // 상점 리스트 초기화
        shopListChildRef = new GameObject[shopListRef.childCount - 1]; // CurRuby는 제외
        for (int index = 0; index < shopListChildRef.Length; index++)
        {
            shopListChildRef[index] = shopListRef.GetChild(index).gameObject;
        }

        // 상점 좌측상단 소지 루비현황 텍스트
        curRubyText = shopListRef.Find("CurRuby/Box/Text (TMP)").GetComponent<TMP_Text>();

        //상점 하단 버튼초기화
        botArrBtn = shopRef.transform.Find("ShopBottomBtn").GetComponentsInChildren<Button>();
        botArrImage = new Image[botArrBtn.Length];
        botArrText = new TMP_Text[botArrBtn.Length];
        for (int index = 0; index < botArrBtn.Length; index++)
        {
            botArrImage[index] = botArrBtn[index].GetComponent<Image>();
            botArrText[index] = botArrImage[index].GetComponentInChildren<TMP_Text>();
        }

        //골드상점 초기화
        Transform GoldProdParents = shopListChildRef[1].transform.Find("ProductList");
        int GoldShopCount = GoldProdParents.childCount;
        for (int iNum = 0; iNum < GoldShopCount; iNum++)
        {
            list_GoldProduct[iNum].initProduct(GoldProdParents.GetChild(iNum));
        }

        //루비상점 초기화
        Transform RubyProdParents = shopListChildRef[2].transform.Find("ProductList");
        int RubyShopCount = RubyProdParents.childCount;
        for (int iNum = 0; iNum < RubyShopCount; iNum++)
        {
            list_RubyProduct[iNum].initProduct(RubyProdParents.GetChild(iNum));
        }

        //광고상점 초기화
        Transform AdProdParents = shopListChildRef[3].transform.Find("ProductList");
        int AdShopCount = AdProdParents.childCount;
        for (int iNum = 0; iNum < AdShopCount; iNum++)
        {
            list_AdProduct[iNum].initProduct(AdProdParents.GetChild(iNum));
        }
        //구매 확인창 초기화
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
        // 하단 상점이동 버튼부 초기화
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
    /// 상점호출
    /// </summary>
    /// <param name="ShopTypeNumber">0뽑기/1골드/2루비/3광고 </param>
    public void Active_Shop(int ShopTypeNumber, bool active)
    {
        if (active) // 해당 상점 호출
        {
            AudioManager.inst.noSound = true;
            // 최초 넘어온게 아니라면 버튼음 재생
            if (curSelectMenu != -1 && curSelectMenu != ShopTypeNumber)
            {
                AudioManager.inst.Play_Ui_SFX(4, 0.8f);
            }

            //동일 버튼 또 클릭시 리턴
            if (curSelectMenu == ShopTypeNumber) { return; }

            //현재 선택되어있는 상점 추적
            curSelectMenu = ShopTypeNumber;

            //상점마다 startInit
            switch (curSelectMenu)
            {
                //뽑기상점이라면
                case 0:
                    Shop_Gacha.inst.Init(true);
                    break;
            }

            // 소비루비 초기화
            curRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Ruby.ToString());

            // 창 열어주기
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

            // 이미지 변경
            BotArrBtn_ImageChanger(ShopTypeNumber);
        }
        else // 상점 종료
        {
            switch (curSelectMenu)
            {
                //뽑기상점이라면
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
    // 하단버튼 이미지 변경
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

