using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] List<GameObject> m_listBottomUI = new List<GameObject>();
    [SerializeField] TextMeshProUGUI m_totalGold;
    [SerializeField] TextMeshProUGUI m_totalAtk;

    [SerializeField] private int buyCount = 1;
    [HideInInspector] public UnityEvent OnBuyCountChanged;

    private int equipWeaponNum;
    public int EquipWeaponNum
    {
        get => equipWeaponNum;
        set
        {
            equipWeaponNum = value;
            ActionManager.inst.Set_WeaponSprite_Changer(value);
        }
    }

    BigInteger totalProdGold;
    public BigInteger TotalProdGold
    {
        get => totalProdGold;
        set
        {
            totalProdGold = value;
            m_totalGold.text = "초당 골드생산량 : " + CalCulator.inst.StringFourDigitChanger(totalProdGold.ToString());
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
            m_totalAtk.text = "총 공격력 : " + CalCulator.inst.StringFourDigitChanger(totalAtk.ToString());
        }
    }
    public int BuyCount
    {
        get => buyCount;
        set
        {
            buyCount = value;
            OnBuyCountChanged?.Invoke();
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
        m_totalGold.text = "초당 골드생산량 : " + CalCulator.inst.StringFourDigitChanger(totalProdGold.ToString());
        m_totalAtk.text = "총 공격력 : " + CalCulator.inst.StringFourDigitChanger(totalAtk.ToString());
    }

    void Update()
    {

    }

    public void ClickBotBtn(float _num)
    {
        int count = m_listBottomUI.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (iNum == _num)
            {
                m_listBottomUI[iNum].SetActive(true);
            }
            else
            {
                m_listBottomUI[iNum].SetActive(false);
            }
        }
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
        BuyCount = count;
    }
}
