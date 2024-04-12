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
    [SerializeField] Canvas canvas;
    [HideInInspector] public UnityEvent OnBuyCountChanged;//����Ʈ ���Ű��� �ٲ�� �̺�Ʈ
    [Header("����UI")]
    [SerializeField] List<GameObject> m_listMainUI = new List<GameObject>();//�ϴ� Ui ����Ʈ
    [SerializeField] List<Sprite> m_BtnSprite = new List<Sprite>();//��ư ����, ���� ��������Ʈ
    [SerializeField] List<Image> m_list_BottomBtn = new List<Image>();//����UI �ϴ� ��ư
    int bottomBtnNum = 0;//������ �ϴ� ��ư ��ȣ
    [SerializeField] Button[] m_aryPetDetailInforBtns;//�� �󼼺����ư

    [Header("����Ʈ")]
    [SerializeField] List<Transform> m_list_Quest = new List<Transform>();//����Ʈ ����Ʈ
    [SerializeField] Transform m_QuestParents;//����Ʈ ������ Ʈ������
    [SerializeField] List<Image> m_list_QuestBuyCountBtn = new List<Image>(); //����Ʈ ���Ű��� ���� ��ư
    [SerializeField] TextMeshProUGUI m_totalGold;// �ʴ� ��� ���귮 �ؽ�Ʈ


    public TextMeshProUGUI GettotalGoldText()
    {
        return m_totalGold;
    }

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
    [SerializeField] Transform m_WeaponParents;
    [SerializeField] RectTransform m_WeaponParentRect;
    [SerializeField] TextMeshProUGUI m_totalAtk;
    [SerializeField] Button WeaponBook;

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
    [SerializeField] Button[] list_PetUpBtn;

    [Header("����")]
    [SerializeField] Sprite[] list_prodSprite;
    public Sprite GetProdSprite(int index)
    {
        return list_prodSprite[index];
    }
    [SerializeField] Button ShopOpenBtn;
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
        m_WeaponParentRect = m_WeaponParents.GetComponent<RectTransform>();
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

    //public int GetQuestLv(int index)//���ϴ� ����Ʈ�� ���� ��������
    //{
    //    return m_list_Quest[index].GetComponent<Quest>().GetLv();
    //}

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
