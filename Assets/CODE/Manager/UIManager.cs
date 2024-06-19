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

    [Header("��ư ����, ���� ��������Ʈ")]
    [SerializeField] List<Sprite> TopBtnSprite = new List<Sprite>();//��� ��ư ����, ���� ��������Ʈ
    [SerializeField] List<Sprite> BotBtnSprite = new List<Sprite>();//�ϴ� ��ư ����, ���� ��������Ʈ
    [SerializeField] List<Sprite> GreenYellowBtnSprite = new List<Sprite>();//���, �ʷ� ��ư ��������Ʈ
    /// <summary>
    /// 0 = ���� �̹���, 1 = ���� �̹���
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Sprite GetBtnSprite(int index)
    {
        return TopBtnSprite[index];
    }

    public Sprite GetGYBtnSprite(int index)
    {
        return GreenYellowBtnSprite[index];
    }
    Canvas canvas;
    List<GameObject> m_listMainUI = new List<GameObject>();//�ϴ� Ui ����Ʈ
    List<Image> m_list_BottomBtn = new List<Image>();//����UI �ϴ� ��ư
    List<Animator> list_BotBtnAnim = new List<Animator>();//����UI �ϴ� ��ư �ִϸ�����
    int bottomBtnNum = 0;//������ �ϴ� ��ư ��ȣ
    RectTransform ScreenArea;


    ////////////////////////////////////////////����Ʈ///////////////////////////////////////

    [HideInInspector] public UnityEvent OnBuyCountChanged;//����Ʈ ���Ű��� �ٲ�� �̺�Ʈ
    [HideInInspector] public UnityEvent QuestReset;
    List<Transform> m_list_Quest = new List<Transform>();//����Ʈ ����Ʈ
    Transform m_QuestParents;//����Ʈ ������ Ʈ������
    RectTransform m_QuestParentRect;
    List<Image> m_list_QuestBuyCountBtn = new List<Image>(); //����Ʈ ���Ű��� ���� ��ư
    TextMeshProUGUI m_totalGold;// �ʴ� ��� ���귮 �ؽ�Ʈ

    int questBuyCount = 1;//����Ʈ �����Ϸ��� ����
    int questBuyCountBtnNum = 0;//������ ����Ʈ �ѹ��� ���� ��ư ��ȣ
    int TopQuestNum = 0;//�ر��� ����Ʈ�� ���� ���� �� ��ȣ
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

    public void QeustUpComplete(int index)
    {
        if (m_list_Quest.Count > index + 1)
        {
            m_list_Quest[index + 1].GetComponent<Quest>().SetMaskActive(false);
        }
        TopQuestNum = index + 1;
    }

    ////////////////////////////////////////////����///////////////////////////////////////

    [HideInInspector] public UnityEvent WeaponReset;
    List<Transform> m_list_Weapon = new List<Transform>();
    Transform m_WeaponParents;
    RectTransform m_WeaponParentRect;
    TextMeshProUGUI m_totalAtk;
    Button WeaponBook;

    int TopHaveWeaponNum;//�������� ������ ���� �ֻ��� ���� ��ȣ


    public void WeaponUpComplete(int index)
    {

        if (m_list_Weapon.Count > index + 1)
        {
            m_list_Weapon[index + 1].GetComponent<Weapon>().SetMaskActive(false);
        }
        TopHaveWeaponNum = index + 1;
    }

    public TextMeshProUGUI GettotalAtkText()
    {
        return m_totalAtk;
    }

    public void SetTopWeaponNum(int _num)
    {
        TopHaveWeaponNum = _num;
    }

    public void MaxBuyWeapon()
    {
        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
        if (m_list_Weapon.Count > TopHaveWeaponNum)
        {
            BigInteger nextcost = m_list_Weapon[TopHaveWeaponNum].GetComponent<Weapon>().GetNextCost();
            while (haveGold >= nextcost)
            {
                if (m_list_Weapon.Count > TopHaveWeaponNum)
                {
                    Weapon ScWeapon = m_list_Weapon[TopHaveWeaponNum].GetComponent<Weapon>();
                    ScWeapon.ClickUp();
                    haveGold -= nextcost;
                    nextcost = ScWeapon.GetNextCost();
                }
                else
                {
                    break;
                }
            }
        }
    }

    ////////////////////////////////////////////��///////////////////////////////////////

    GameObject petRef;
    Transform petParents;
    TMP_Text soulText;
    TMP_Text bornText;
    TMP_Text bookText;

    void setCrewMatText()
    {
        int[] petmat = GameStatus.inst.CrewMaterial;

        soulText.text = string.Format("{0:#,0}", petmat[0]);
        bornText.text = string.Format("{0:#,0}", petmat[1]);
        bookText.text = string.Format("{0:#,0}", petmat[2]);
    }

    ////////////////////////////////////////////����///////////////////////////////////////

    [HideInInspector] public UnityEvent OnRelicBuyCountChanged;
    [SerializeField] GameObject[] list_ObjRelic;
    Transform relicParents;
    Button GotoRelicShopBtn;
    GameObject infoRef;

    List<Image> m_list_RelicBuyCountBtn = new List<Image>();
    List<GameObject> list_haveRelic = new List<GameObject>();

    Button relicDogamInfoBtn;
    public List<GameObject> GetHaveRelic()
    {
        return list_haveRelic;
    }

    public void SetHaveRelic(List<GameObject> list)
    {
        list_haveRelic = list;

        // ����â ����
        if (list_haveRelic.Count > 0 && infoRef.activeInHierarchy)
        {
            infoRef.SetActive(false);
        }
    }

    int relicBuyCountBtnNum = 0;
    public int RelicBuyCountBtnNum
    {
        get => relicBuyCountBtnNum;
    }
    int relicBuyCount = 1;
    public int RelicBuyCount
    {
        get => relicBuyCount;
        set
        {
            relicBuyCount = value;
            OnRelicBuyCountChanged?.Invoke();
        }
    }

    ////////////////////////////////////////////����///////////////////////////////////////

    [Header("��ǰ ��������Ʈ")]
    [SerializeField] Sprite[] list_prodSprite;

    [HideInInspector] public UnityEvent onOpenShop;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">0 = ���, 1 = ��, 2 = ���</param>
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
    Button ShopCloseBtn;

    /////////////////////////////////////////////�귿///////////////////////////////////////////
    GameObject ObjRoulette;
    Button RouletteOpenBtn;

    ////////////////////////////////////////////////////////////////////////////////////////////


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameStatus.inst.PlusGold("99999999999999999999999999999999");
            GameStatus.inst.PlusRuby(1000);
            GameStatus.inst.PlusStar("10000000000000000000000000");
            GameStatus.inst.Set_crewMaterial(0, 1000);
            GameStatus.inst.Set_crewMaterial(1, 1000);
            GameStatus.inst.Set_crewMaterial(2, 1000);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            QuestReset?.Invoke();
            WeaponReset?.Invoke();
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

        //�⺻ UI �ʱ�ȭ
        canvas = GameObject.Find("---[UI Canvas]").GetComponent<Canvas>();
        GameObject worldCanvas = GameObject.Find("---[World UI Canvas]");

        Transform trsBotBtn = canvas.transform.Find("ScreenArea/BackGround/BottomBtn");
        int botBtnCount = trsBotBtn.childCount;
        for (int iNum = 0; iNum < botBtnCount; iNum++)
        {
            m_list_BottomBtn.Add(trsBotBtn.GetChild(iNum).GetComponent<Image>());
            list_BotBtnAnim.Add(trsBotBtn.GetChild(iNum).GetComponent<Animator>());
        }
        ShopOpenBtn = trsBotBtn.Find("Shop").GetComponent<Button>();
        ShopOpenBtn.onClick.AddListener(() => 
        {
            ShopManager.inst.Active_Shop(0, true);
            onOpenShop?.Invoke();
        });




        m_listMainUI.Add(canvas.transform.Find("ScreenArea/BackGround/Quest").gameObject);
        m_listMainUI.Add(canvas.transform.Find("ScreenArea/BackGround/Weapon").gameObject);
        m_listMainUI.Add(canvas.transform.Find("ScreenArea/BackGround/Pet").gameObject);
        m_listMainUI.Add(canvas.transform.Find("ScreenArea/BackGround/Relic").gameObject);
        m_listMainUI.Add(canvas.transform.Find("ScreenArea/BackGround/Shop").gameObject);
        m_list_BottomBtn[1].transform.GetComponent<Button>().onClick.AddListener(SetWeaponScroll);
        m_list_BottomBtn[0].transform.GetComponent<Button>().onClick.AddListener(SetQuestScroll);
        ScreenArea = canvas.transform.Find("ScreenArea").GetComponent<RectTransform>();

        //����Ʈ �ʱ�ȭ
        m_QuestParents = canvas.transform.Find("ScreenArea/BackGround/Quest/Scroll View").GetComponent<ScrollRect>().content;
        m_QuestParentRect = m_QuestParents.GetComponent<RectTransform>();
        Transform trsQuestBuyCount = canvas.transform.Find("ScreenArea/BackGround/Quest/AllLvUpBtn");
        int QuestBuyCount = trsQuestBuyCount.childCount;
        for (int iNum = QuestBuyCount - 1; iNum >= 0; iNum--)
        {
            m_list_QuestBuyCountBtn.Add(trsQuestBuyCount.GetChild(iNum).GetComponent<Image>());
        }
        int questCount = m_QuestParents.childCount;
        for (int iNum = 0; iNum < questCount; iNum++)
        {
            m_list_Quest.Add(m_QuestParents.GetChild(iNum));
        }

        m_totalGold = canvas.transform.Find("ScreenArea/BackGround/Quest/TotalGpsBG/TotalGps").GetComponent<TextMeshProUGUI>();
        


        //���� �ʱ�ȭ
        m_WeaponParents = canvas.transform.Find("ScreenArea/BackGround/Weapon/Scroll View").GetComponent<ScrollRect>().content;
        m_WeaponParentRect = m_WeaponParents.GetComponent<RectTransform>();
        m_totalAtk = canvas.transform.Find("ScreenArea/BackGround/Weapon/TotalAtkBG/TotalAtk").GetComponent<TextMeshProUGUI>();
        WeaponBook = canvas.transform.Find("ScreenArea/BackGround/Weapon/TopBg/DogamBtn").GetComponent<Button>();
        int weaponCount = m_WeaponParents.childCount;
        for (int iNum = 0; iNum < weaponCount; iNum++)
        {
            m_list_Weapon.Add(m_WeaponParents.GetChild(iNum));

        }

        //���� �ʱ�ȭ

        relicParents = canvas.transform.Find("ScreenArea/BackGround/Relic/NormalScroll View/Viewport/Content");
        relicDogamInfoBtn = canvas.transform.Find("ScreenArea/BackGround/Relic/TopArray/Button").GetComponent<Button>();
        relicDogamInfoBtn.onClick.AddListener(() => RelicInfoManager.inst.Set_RelicDogamActive(true));
        GotoRelicShopBtn = canvas.transform.Find("ScreenArea/BackGround/Relic/GotoRelicShopBtn").GetComponent<Button>();
        GotoRelicShopBtn.onClick.AddListener(() =>
        {
            ClickBotBtn(4);
            ShopManager.inst.Active_Shop(0, true);
        });
        Transform RelicAllUpBtnParents = canvas.transform.Find("ScreenArea/BackGround/Relic/AllLvUpBtn");
        int RelicAllCount = RelicAllUpBtnParents.childCount;
        for (int iNum = RelicAllCount - 1; iNum >= 0; iNum--)
        {
            m_list_RelicBuyCountBtn.Add(RelicAllUpBtnParents.GetChild(iNum).GetComponent<Image>());
        }

        infoRef = canvas.transform.Find("ScreenArea/BackGround/Relic/Info").gameObject;
        //�� �ʱ�ȭ
        petRef = canvas.transform.Find("ScreenArea/BackGround/Pet").gameObject;
        soulText = canvas.transform.Find("ScreenArea/BackGround/Pet/TopButton/Soul/Text (TMP)").GetComponent<TMP_Text>();
        bornText = canvas.transform.Find("ScreenArea/BackGround/Pet/TopButton/Born/Text (TMP)").GetComponent<TMP_Text>();
        bookText = canvas.transform.Find("ScreenArea/BackGround/Pet/TopButton/Book/Text (TMP)").GetComponent<TMP_Text>();
        petParents = canvas.transform.Find("ScreenArea/BackGround/Pet/Scroll View/Viewport/Content");

        //�귿 �ʱ�ȭ
        ObjRoulette = canvas.transform.Find("ScreenArea/Roulette/").gameObject;
        RouletteOpenBtn = worldCanvas.transform.Find("StageUI/MenuBox/Btns/Bingo").GetComponent<Button>();
        RouletteOpenBtn.onClick.AddListener(() => { ObjRoulette.SetActive(true); });
    }

    void Start()
    {
        //����Ʈ �ʱ�ȭ
        int questCount = m_QuestParents.childCount;
        for (int iNum = 0; iNum < questCount; iNum++)
        {
            m_QuestParents.GetChild(iNum).GetComponent<Quest>().initQuest();
        }
        list_BotBtnAnim[0].SetTrigger("select");
        list_BotBtnAnim[0].SetBool("isSelect", true);

        QuestReset.AddListener(() => { TopQuestNum = 0; });
        SetQuestScroll();

        //���� �ʱ�ȭ
        int weaponCount = m_WeaponParents.childCount;
        for (int iNum = 0; iNum < weaponCount; iNum++)
        {
            m_WeaponParents.GetChild(iNum).GetComponent<Weapon>().InitWeapon();
        }
        WeaponReset.AddListener(() => { TopHaveWeaponNum = 0; });

        //�� �ʱ�ȭ
        int petCount = petParents.childCount;
        for (int iNum = 0; iNum < petCount; iNum++)
        {
            petParents.GetChild(iNum).GetComponent<Pet>().initPet();
        }
        GameStatus.inst.onCrewMatChanged.AddListener(setCrewMatText);
        GameStatus.inst.onCrewMatChanged?.Invoke();

        //���� �ʱ�ȭ

        List<int> relicLv = GameStatus.inst.GetAryRelicLv();
        int relicCount = relicLv.Count;
        for (int iNum = 0; iNum < relicCount; iNum++)
        {
            if (relicLv[iNum] != 0)
            {
                GameObject obj = Instantiate(list_ObjRelic[iNum], relicParents);
                Relic sc = obj.GetComponent<Relic>();
                sc.initRelic();
                SetGotoGachaBtn(false);
                list_haveRelic.Add(obj);
            }
        }

        InvokeRepeating("getGoldPerSceond", 0, 1.5f);//�ʴ� ��� ȹ��
        m_totalGold.text = "��� �ڵ� ȹ�� (��) : " + CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.TotalProdGold.ToString());
        //SetAtkText(CalCulator.inst.StringFourDigitAddFloatChanger(CalCulator.inst.Get_CurPlayerATK()));
        SetAtkText(GameStatus.inst.TotalAtk.ToString());
        initButton();
    }

    void initButton()
    {
        WeaponBook.onClick.AddListener(() => DogamManager.inst.Active_DogamUI(true));
    }

    public void SettotalGoldText(string _text)
    {
        if (m_totalGold != null)
        {
            m_totalGold.text = $"��� �ڵ� ȹ�� (��)  : " + _text;
        }
    }

    public Sprite GetSelectUISprite(int num)
    {
        return TopBtnSprite[num];
    }

    void getGoldPerSceond()
    {
        GameStatus.inst.GetGold(GameStatus.inst.GetTotalGold());
    }

    void SetQuestScroll()
    {
        m_QuestParentRect.anchoredPosition = new UnityEngine.Vector2(0, 64 * TopQuestNum - 64);
    }

    void SetWeaponScroll()
    {
        m_WeaponParentRect.anchoredPosition = new UnityEngine.Vector2(0, 64 * TopHaveWeaponNum - 64);
    }


    public void changeSortOder(int value)
    {
        canvas.sortingOrder = value;
    }

    public void ClickBotBtn(int _num)
    {
        if (_num != 4)
        {
            ShopManager.inst.Active_Shop(0,false);
        }

        m_listMainUI[bottomBtnNum].SetActive(false);
        list_BotBtnAnim[bottomBtnNum].SetTrigger("nonSelect");
        list_BotBtnAnim[bottomBtnNum].SetBool("isSelect", false);
        if (bottomBtnNum == 0)
        {
            m_list_BottomBtn[bottomBtnNum].sprite = BotBtnSprite[0];
        }
        else
        {
            m_list_BottomBtn[bottomBtnNum].sprite = BotBtnSprite[1];
        }

        bottomBtnNum = _num;
        list_BotBtnAnim[bottomBtnNum].SetTrigger("select");
        list_BotBtnAnim[bottomBtnNum].SetBool("isSelect", true);
        m_listMainUI[_num].SetActive(true);
        if (bottomBtnNum == 0)
        {
            m_list_BottomBtn[bottomBtnNum].sprite = BotBtnSprite[3];
        }
        else
        {
            m_list_BottomBtn[bottomBtnNum].sprite = BotBtnSprite[4];
        }
        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
    }

    public void ClickOpenThisTab(GameObject _obj)
    {
        _obj.SetActive(true);
    }

    public void ClickCloseThisTab(GameObject _obj)
    {
        _obj.SetActive(false);
    }

    public void ClickBuyCountBtn(int count)
    {
        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
        m_list_QuestBuyCountBtn[questBuyCountBtnNum].sprite = TopBtnSprite[0];
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
        m_list_QuestBuyCountBtn[questBuyCountBtnNum].sprite = TopBtnSprite[1];
    }

    public void ClickRelicBuyCountBtn(int count)
    {
        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
        m_list_RelicBuyCountBtn[relicBuyCountBtnNum].sprite = TopBtnSprite[0];
        relicBuyCountBtnNum = count;
        switch (count)
        {
            case 0:
                RelicBuyCount = 1;
                break;
            case 1:
                RelicBuyCount = 10;
                break;
            case 2:
                RelicBuyCount = 100;
                break;
            case 3:
                RelicBuyCount = 0;
                break;
        }
        m_list_RelicBuyCountBtn[relicBuyCountBtnNum].sprite = TopBtnSprite[1];
    }

    public void SetGotoGachaBtn(bool value)
    {
        GotoRelicShopBtn.gameObject.SetActive(value);
        infoRef.SetActive(value);
    }



    public void SetAtkText(string _atk)
    {
        m_totalAtk.text = $"���� Ÿ�� ����� : {CalCulator.inst.StringFourDigitAddFloatChanger(_atk)} / Ÿ";
    }


    // ȯ���� ����Ʈ �� ���⸮��
    public void Reset_QuestAndWeapon()
    {
        QuestReset?.Invoke();
        WeaponReset?.Invoke();
    }
}
