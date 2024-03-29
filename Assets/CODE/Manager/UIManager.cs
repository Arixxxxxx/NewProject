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
    [HideInInspector] public UnityEvent OnBuyCountChanged;

    [SerializeField] List<GameObject> m_listMainUI = new List<GameObject>();
    [SerializeField] List<Sprite> m_BtnSprite = new List<Sprite>();
    [SerializeField] List<Image> m_list_BottomBtn = new List<Image>();
    int bottomBtnNum = 0;//������ �ϴ� ��ư ��ȣ

    [Header("����Ʈ")]
    [SerializeField] List<Image> m_list_QuestBuyCountBtn = new List<Image>();
    [SerializeField] TextMeshProUGUI m_totalGold;
    int questBuyCountBtnNum = 0;//������ ����Ʈ �ѹ��� ���� ��ư ��ȣ
    [SerializeField] private int questBuyCount = 1;//����Ʈ �����Ϸ��� ����
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
    [SerializeField] List<Transform> m_list_Weapon = new List<Transform>();
    [SerializeField] Transform m_WeaponParents;
    [SerializeField] TextMeshProUGUI m_totalAtk;
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
    public void SetTopWeaponNum(int _num)
    {
        haveWeaponLv = _num;
    }

    BigInteger totalProdGold = 10;
    public BigInteger TotalProdGold
    {
        get => totalProdGold;
        set
        {
            totalProdGold = value;
            m_totalGold.text = "�ʴ� �����귮 : " + CalCulator.inst.StringFourDigitChanger(totalProdGold.ToString());
        }
    }
    public string GetTotalGold()
    {
        return TotalProdGold.ToString();
    }

    BigInteger totalAtk = 5;
    public BigInteger TotalAtk
    {
        get => totalAtk;
        set
        {
            totalAtk = value;
            m_totalAtk.text = "�� ���ݷ� : " + CalCulator.inst.StringFourDigitChanger(totalAtk.ToString());
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
        m_totalGold.text = "�ʴ� �����귮 : " + CalCulator.inst.StringFourDigitChanger(totalProdGold.ToString());
        m_totalAtk.text = "�� ���ݷ� : " + CalCulator.inst.StringFourDigitChanger(totalAtk.ToString());
        InvokeRepeating("getGoldPerSceond", 0, 1);

        int count = m_WeaponParents.childCount;
        for (int iNum = 0; iNum < count; iNum++)
        {
            m_list_Weapon.Add(m_WeaponParents.GetChild(iNum));
        }
    }

    void getGoldPerSceond()
    {
        GameStatus.inst.GetGold(GetTotalGold());
    }

    void Update()
    {

    }

    public void ClickBotBtn(int _num)
    {
        if (_num != 4)
        {
            m_list_BottomBtn[bottomBtnNum].sprite = m_BtnSprite[0];
            m_listMainUI[bottomBtnNum].SetActive(false);

            bottomBtnNum = _num;
            m_list_BottomBtn[bottomBtnNum].sprite = m_BtnSprite[1];
        }
        m_listMainUI[_num].SetActive(true);
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
        QuestBuyCount = count;
        m_list_QuestBuyCountBtn[questBuyCountBtnNum].sprite = m_BtnSprite[0];
        switch (count)
        {
            case 1:
                questBuyCountBtnNum = 0;
                break;
            case 10:
                questBuyCountBtnNum = 1;
                break;
            case 100:
                questBuyCountBtnNum = 2;
                break;
            case 0:
                questBuyCountBtnNum = 3;
                break;
        }
        m_list_QuestBuyCountBtn[questBuyCountBtnNum].sprite = m_BtnSprite[1];
    }

    public void MaxBuyWeapon()
    {
        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.PulsGold);
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
}
