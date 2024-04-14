using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    Canvas canvas;
    [HideInInspector] public UnityEvent OnBuyCountChanged;//퀘스트 구매갯수 바뀌는 이벤트
    [Header("버튼 선택, 비선택 스프라이트")]
    [SerializeField] List<Sprite> m_BtnSprite = new List<Sprite>();//버튼 선택, 비선택 스프라이트
    List<GameObject> m_listMainUI = new List<GameObject>();//하단 Ui 리스트
    List<Image> m_list_BottomBtn = new List<Image>();//메인UI 하단 버튼
    int bottomBtnNum = 0;//선택한 하단 버튼 번호
    Button[] m_aryPetDetailInforBtns;//펫 상세보기버튼

    [Header("퀘스트")]
    List<Transform> m_list_Quest = new List<Transform>();//퀘스트 리스트
    Transform m_QuestParents;//퀘스트 컨텐츠 트래스폼
    List<Image> m_list_QuestBuyCountBtn = new List<Image>(); //퀘스트 구매갯수 조절 버튼
    TextMeshProUGUI m_totalGold;// 초당 골드 생산량 텍스트

    int questBuyCount = 1;//퀘스트 구매하려는 갯수
    int questBuyCountBtnNum = 0;//선택한 퀘스트 한번에 구매 버튼 번호
    public int QuestBuyCountBtnNum
    {
        get => questBuyCountBtnNum;
    }
    public int QuestBuyCount
    {
        get => questBuyCount;
        set
        {
            questBuyCount = value;
            OnBuyCountChanged?.Invoke();
        }
    }

    [Header("무기")]
    List<Transform> m_list_Weapon = new List<Transform>();
    Transform m_WeaponParents;
    RectTransform m_WeaponParentRect;
    TextMeshProUGUI m_totalAtk;
    Button WeaponBook;

    int haveWeaponLv;//보유중인 무기중 제일 최상위 무기 번호
    int equipWeaponNum;//장착중인 무기 이미지 번호
    public int EquipWeaponNum
    {
        get => equipWeaponNum;
        set
        {
            equipWeaponNum = value;
            ActionManager.inst.Set_WeaponSprite_Changer(value);
        }
    }

    public void WeaponUpComplete(Transform _trs)
    {
        int index = m_list_Weapon.FindIndex(x => x == _trs);
        m_list_Weapon[index + 1].GetComponent<Weapon>().SetMaskActive(false);
    }

    public TextMeshProUGUI GettotalAtkText()
    {
        return m_totalAtk;
    }

    public void SetTopWeaponNum(int _num)
    {
        haveWeaponLv = _num;
    }

    [Header("펫")]
    [SerializeField] List<Pet> list_Pet;
    [SerializeField] GameObject obj_Pet;
    [Serializable]
    public class Pet
    {
        [SerializeField] string Name;
        [SerializeField] string Explane;
        [SerializeField] int baseCost;
        [SerializeField] PetType type;
        [SerializeField] Sprite sprite;
        Transform trs;
        public Transform Trs { get => trs; set { trs = value; } }
        int nextCost;
        Button upBtn;
        Image petImage;
        TMP_Text costText;
        TMP_Text lvText;
        TMP_Text NameText;
        TMP_Text ExText;

        public void initStart()
        {
            upBtn = Trs.Find("Button").GetComponent<Button>();
            costText = Trs.Find("Button/PriceText").GetComponent<TMP_Text>();
            lvText = Trs.Find("LvText").GetComponent<TMP_Text>();
            NameText = Trs.Find("NameText").GetComponent<TMP_Text>();
            ExText = Trs.Find("ExplaneText").GetComponent<TMP_Text>();
            petImage = Trs.Find("imageBtn/Image").GetComponent<Image>();

            switch (type)
            {
                case PetType.AtkPet:
                    lvText.text = $"{GameStatus.inst.Pet0_Lv}";
                    nextCost = baseCost + GameStatus.inst.Pet0_Lv * 100;
                    break;

                case PetType.BuffPet:
                    lvText.text = $"{GameStatus.inst.Pet1_Lv}";
                    nextCost = baseCost + GameStatus.inst.Pet1_Lv * 100;
                    break;

                case PetType.GoldPet:
                    lvText.text = $"{GameStatus.inst.Pet2_Lv}";
                    nextCost = baseCost + GameStatus.inst.Pet2_Lv * 100;
                    break;
            }
            NameText.text = Name;
            ExText.text = Explane;
            costText.text = $"{nextCost}";

            petImage.sprite = sprite;
            petImage.SetNativeSize();
            RectTransform rect = petImage.GetComponent<RectTransform>();
            float ratio = rect.sizeDelta.x / rect.sizeDelta.y;
            rect.sizeDelta = new UnityEngine.Vector2(40 * ratio, 40);

            upBtn.onClick.AddListener(ClickUp);
        }

        public Button GetUpBtn()
        {
            return upBtn;
        }

        void ClickUp()
        {
            int haveruby = GameStatus.inst.Ruby;
            if (haveruby >= nextCost)
            {
                GameStatus.inst.Ruby -= nextCost;
                switch (type)
                {
                    case PetType.AtkPet:
                        GameStatus.inst.Pet0_Lv++;
                        nextCost = baseCost + GameStatus.inst.Pet0_Lv * 100;
                        lvText.text = $"{GameStatus.inst.Pet0_Lv}";
                        break;

                    case PetType.BuffPet:
                        GameStatus.inst.Pet1_Lv++;
                        nextCost = baseCost + GameStatus.inst.Pet1_Lv * 100;
                        lvText.text = $"{GameStatus.inst.Pet1_Lv}";
                        break;

                    case PetType.GoldPet:
                        GameStatus.inst.Pet2_Lv++;
                        nextCost = baseCost + GameStatus.inst.Pet2_Lv * 100;
                        lvText.text = $"{GameStatus.inst.Pet2_Lv}";
                        break;
                }
                costText.text = $"{nextCost}";
            }
        }
    }

    [Header("상품 스프라이트")]
    [SerializeField] Sprite[] list_prodSprite;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">0 = 골드, 1 = 루비, 2 = 별</param>
    /// <returns></returns>
    public Sprite GetProdSprite(int index)
    {
        return list_prodSprite[index];
    }

    Button ShopOpenBtn;
    public Button GetShopOpenBtn()
    {
        return ShopOpenBtn;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShopManager.Instance.OpenRubyShop();
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
        //기본 UI 초기화
        canvas = GameObject.Find("---[UI Canvas]").GetComponent<Canvas>();

        Transform trsBotBtn = canvas.transform.Find("BackGround/BottomBtn");
        int botBtnCount = trsBotBtn.childCount;
        for (int iNum = 0; iNum < botBtnCount; iNum++)
        {
            m_list_BottomBtn.Add(trsBotBtn.GetChild(iNum).GetComponent<Image>());
        }
        ShopOpenBtn = trsBotBtn.Find("Shop").GetComponent<Button>();

        m_listMainUI.Add(canvas.transform.Find("BackGround/Quest").gameObject);
        m_listMainUI.Add(canvas.transform.Find("BackGround/Weapon").gameObject);
        m_listMainUI.Add(canvas.transform.Find("BackGround/Pet").gameObject);
        m_listMainUI.Add(canvas.transform.Find("BackGround/Relic").gameObject);

        //퀘스트 초기화
        m_QuestParents = canvas.transform.Find("BackGround/Quest/Scroll View").GetComponent<ScrollRect>().content;
        Transform trsQuestBuyCount = canvas.transform.Find("BackGround/Quest/AllLvUpBtn");
        int QuestBuyCount = trsQuestBuyCount.childCount;
        for (int iNum = QuestBuyCount - 1; iNum >= 0; iNum--)
        {
            m_list_QuestBuyCountBtn.Add(trsQuestBuyCount.GetChild(iNum).GetComponent<Image>());
        }

        m_totalGold = canvas.transform.Find("BackGround/Quest/TotalGps").GetComponent<TextMeshProUGUI>();

        //무기 초기화
        m_WeaponParents = canvas.transform.Find("BackGround/Weapon/Scroll View").GetComponent<ScrollRect>().content;
        m_WeaponParentRect = m_WeaponParents.GetComponent<RectTransform>();
        m_totalAtk = canvas.transform.Find("BackGround/Weapon/TotalAtk").GetComponent<TextMeshProUGUI>();
        WeaponBook = canvas.transform.Find("BackGround/Weapon/AllLvUpBtn/DogamBtn").GetComponent<Button>();


        //펫 초기화
        Transform trsPetContents = canvas.transform.Find("BackGround/Pet/Scroll View/Viewport/Content");
        int petCount = list_Pet.Count;
        m_aryPetDetailInforBtns = new Button[petCount];
        for (int iNum = 0; iNum < petCount; iNum++)
        {
            Transform trs = Instantiate(obj_Pet, trsPetContents).transform;
            m_aryPetDetailInforBtns[iNum] = trs.Find("imageBtn").GetComponent<Button>();
            list_Pet[iNum].Trs = trs;
            list_Pet[iNum].initStart();
        }

        m_list_BottomBtn[1].transform.GetComponent<Button>().onClick.AddListener(SetWeaponScroll);
        m_totalGold.text = "초당 골드생산량 : " + CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.TotalProdGold.ToString());
        SetAtkText(CalCulator.inst.StringFourDigitAddFloatChanger(CalCulator.inst.Get_CurPlayerATK()));
        InvokeRepeating("getGoldPerSceond", 0, 1);

        int weaponCount = m_WeaponParents.childCount;
        for (int iNum = 0; iNum < weaponCount; iNum++)
        {
            m_list_Weapon.Add(m_WeaponParents.GetChild(iNum));
        }

        int questCount = m_QuestParents.childCount;
        for (int iNum = 0; iNum < questCount; iNum++)
        {
            m_list_Quest.Add(m_QuestParents.GetChild(iNum));
        }

        initButton();
    }

    void initButton()
    {
        m_aryPetDetailInforBtns[0].onClick.AddListener(() =>//1번 펫 상세보기
        {
            PetDetailViewr_UI.inst.TopArrayBtnActive(0);
        });

        m_aryPetDetailInforBtns[1].onClick.AddListener(() =>//2번 펫 상세보기
        {
            PetDetailViewr_UI.inst.TopArrayBtnActive(1);
        });

        m_aryPetDetailInforBtns[2].onClick.AddListener(() =>//3번 펫 상세보기
        {
            PetDetailViewr_UI.inst.TopArrayBtnActive(2);
        });

        WeaponBook.onClick.AddListener(() => DogamManager.inst.Set_DogamListAcitve(0, true));
    }

    public void SettotalGoldText(string _text)
    {
        if (m_totalGold != null)
        {
            m_totalGold.text = "초당 골드생산량 : " + _text;
        }
    }

    public Sprite GetSelectUISprite(int num)
    {
        return m_BtnSprite[num];
    }

    void getGoldPerSceond()
    {
        GameStatus.inst.GetGold(GameStatus.inst.GetTotalGold());
    }

    void SetWeaponScroll()
    {
        m_WeaponParentRect.anchoredPosition = new UnityEngine.Vector2(0, 64 * (haveWeaponLv / 5) - 64);
    }

    public void changeSortOder(int value)
    {
        canvas.sortingOrder = value;
    }

    public void ClickBotBtn(int _num)
    {
        m_list_BottomBtn[bottomBtnNum].sprite = m_BtnSprite[0];
        m_listMainUI[bottomBtnNum].SetActive(false);

        bottomBtnNum = _num;
        m_list_BottomBtn[bottomBtnNum].sprite = m_BtnSprite[1];
        m_listMainUI[_num].SetActive(true);
    }


    public void ClickOpenThisTab(GameObject _obj)
    {
        _obj.SetActive(true);
        canvas.sortingOrder = 4;
    }

    public void ClickCloseThisTab(GameObject _obj)
    {
        _obj.SetActive(false);
    }

    public void ClickBuyCountBtn(int count)
    {
        m_list_QuestBuyCountBtn[questBuyCountBtnNum].sprite = m_BtnSprite[0];
        questBuyCountBtnNum = count;
        switch (count)
        {
            case 0:
                QuestBuyCount = 1;
                break;
            case 1:
                QuestBuyCount = 10;
                break;
            case 2:
                QuestBuyCount = 100;
                break;
            case 3:
                QuestBuyCount = 0;
                break;
        }
        m_list_QuestBuyCountBtn[questBuyCountBtnNum].sprite = m_BtnSprite[1];
    }

    public void MaxBuyWeapon()
    {
        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
        int lv = haveWeaponLv;
        int Number = lv / 5;
        BigInteger nextcost = m_list_Weapon[Number].GetComponent<Weapon>().GetNextCost();
        while (haveGold >= nextcost)
        {
            Weapon ScWeapon = m_list_Weapon[Number].GetComponent<Weapon>();
            ScWeapon.ClickBuy();
            haveGold -= nextcost;

            lv = haveWeaponLv;
            nextcost = ScWeapon.GetNextCost();
            Number = lv / 5;
        }
    }

    public void SetAtkText(string _atk)
    {
        m_totalAtk.text = "초당 데미지 : " + _atk;
    }
}
