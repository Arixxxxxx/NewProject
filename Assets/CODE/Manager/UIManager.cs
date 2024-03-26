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

    [SerializeField] private int buyCount = 1;

    [SerializeField] List<Image> m_list_QuestBuyCountBtn = new List<Image>();
    [SerializeField] List<Image> m_list_BottomBtn = new List<Image>();

    [SerializeField] TextMeshProUGUI m_totalAtk;
    [SerializeField] TextMeshProUGUI m_totalGold;

    int questBuyCountBtnNum = 0;
    int bottomBtnNum = 0;
    int equipWeaponNum;

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
        InvokeRepeating("getGoldperSceond", 0, 1);
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
        BuyCount = count;
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
}
