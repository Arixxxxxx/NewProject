using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    Canvas canvas;
    [HideInInspector] public UnityEvent OnBuyCountChanged;//����Ʈ ���Ű��� �ٲ�� �̺�Ʈ
    [Header("��ư ����, ���� ��������Ʈ")]
    [SerializeField] List<Sprite> m_BtnSprite = new List<Sprite>();//��ư ����, ���� ��������Ʈ
    List<GameObject> m_listMainUI = new List<GameObject>();//�ϴ� Ui ����Ʈ
    List<Image> m_list_BottomBtn = new List<Image>();//����UI �ϴ� ��ư
    int bottomBtnNum = 0;//������ �ϴ� ��ư ��ȣ
    Button[] m_aryPetDetailInforBtns;//�� �󼼺����ư

    [Header("����Ʈ")]
    List<Transform> m_list_Quest = new List<Transform>();//����Ʈ ����Ʈ
    Transform m_QuestParents;//����Ʈ ������ Ʈ������
    List<Image> m_list_QuestBuyCountBtn = new List<Image>(); //����Ʈ ���Ű��� ���� ��ư
    TextMeshProUGUI m_totalGold;// �ʴ� ��� ���귮 �ؽ�Ʈ

    int questBuyCount = 1;//����Ʈ �����Ϸ��� ����
    int questBuyCountBtnNum = 0;//������ ����Ʈ �ѹ��� ���� ��ư ��ȣ
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

    [Header("����")]
    List<Transform> m_list_Weapon = new List<Transform>();
    Transform m_WeaponParents;
    RectTransform m_WeaponParentRect;
    TextMeshProUGUI m_totalAtk;
    Button WeaponBook;

    int haveWeaponLv;//�������� ������ ���� �ֻ��� ���� ��ȣ
    int equipWeaponNum;//�������� ���� �̹��� ��ȣ
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

    [Header("��")]
    Button[] list_PetUpBtn;

    [Header("��ǰ ��������Ʈ")]
    [SerializeField] Sprite[] list_prodSprite;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">0 = ���, 1 = ���, 2 = ��</param>
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
        //�⺻ UI �ʱ�ȭ
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

        //����Ʈ �ʱ�ȭ
        m_QuestParents = canvas.transform.Find("BackGround/Quest/Scroll View").GetComponent<ScrollRect>().content;
        Transform trsQuestBuyCount = canvas.transform.Find("BackGround/Quest/AllLvUpBtn");
        int QuestBuyCount = trsQuestBuyCount.childCount;
        for (int iNum = QuestBuyCount - 1; iNum >= 0; iNum--)
        {
            m_list_QuestBuyCountBtn.Add(trsQuestBuyCount.GetChild(iNum).GetComponent<Image>());
        }

        m_totalGold = canvas.transform.Find("BackGround/Quest/TotalGps").GetComponent<TextMeshProUGUI>();

        //���� �ʱ�ȭ
        m_WeaponParents = canvas.transform.Find("BackGround/Weapon/Scroll View").GetComponent<ScrollRect>().content;
        m_WeaponParentRect = m_WeaponParents.GetComponent<RectTransform>();
        m_totalAtk = canvas.transform.Find("BackGround/Weapon/TotalAtk").GetComponent<TextMeshProUGUI>();
        WeaponBook = canvas.transform.Find("BackGround/Weapon/AllLvUpBtn/DogamBtn").GetComponent<Button>();


        //�� �ʱ�ȭ
        Transform trsPetContents = canvas.transform.Find("BackGround/Pet/Scroll View/Viewport/Content");
        int petDetailBtnCount = trsPetContents.childCount;
        m_aryPetDetailInforBtns = new Button[petDetailBtnCount];
        list_PetUpBtn = new Button[petDetailBtnCount];
        for (int iNum = 0; iNum < petDetailBtnCount; iNum++)
        {
            m_aryPetDetailInforBtns[iNum] = trsPetContents.GetChild(iNum).Find("imageBtn").GetComponent<Button>();
            list_PetUpBtn[iNum] = trsPetContents.GetChild(iNum).Find("Button").GetComponent<Button>();
        }



        m_list_BottomBtn[1].transform.GetComponent<Button>().onClick.AddListener(SetWeaponScroll);
        m_totalGold.text = "�ʴ� �����귮 : " + CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.TotalProdGold.ToString());
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
        m_aryPetDetailInforBtns[0].onClick.AddListener(() =>//1�� �� �󼼺���
        {
            PetDetailViewr_UI.inst.TopArrayBtnActive(0);
        });

        m_aryPetDetailInforBtns[1].onClick.AddListener(() =>//2�� �� �󼼺���
        {
            PetDetailViewr_UI.inst.TopArrayBtnActive(1);
        });

        m_aryPetDetailInforBtns[2].onClick.AddListener(() =>//3�� �� �󼼺���
        {
            PetDetailViewr_UI.inst.TopArrayBtnActive(2);
        });

        WeaponBook.onClick.AddListener(() => DogamManager.inst.Set_DogamListAcitve(0, true));
    }

    public void SettotalGoldText(string _text)
    {
        if (m_totalGold != null)
        {
            m_totalGold.text = "�ʴ� �����귮 : " + _text;
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
        m_totalAtk.text = "�ʴ� ������ : " + _atk;
    }
}
