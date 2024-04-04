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
    [HideInInspector] public UnityEvent OnBuyCountChanged;//퀘스트 구매갯수 바뀌는 이벤트
    [Header("메인UI")]
    [SerializeField] List<GameObject> m_listMainUI = new List<GameObject>();//하단 Ui 리스트
    [SerializeField] List<Sprite> m_BtnSprite = new List<Sprite>();//버튼 선택, 비선택 스프라이트
    [SerializeField] List<Image> m_list_BottomBtn = new List<Image>();//메인UI 하단 버튼
    int bottomBtnNum = 0;//선택한 하단 버튼 번호
    [SerializeField] Button[] m_aryPetDetailInforBtns;//펫 상세보기버튼

    [Header("퀘스트")]
    [SerializeField] List<Transform> m_list_Quest = new List<Transform>();//퀘스트 리스트
    [SerializeField] Transform m_QuestParents;//퀘스트 컨텐츠 트래스폼
    [SerializeField] List<Image> m_list_QuestBuyCountBtn = new List<Image>(); //퀘스트 구매갯수 조절 버튼
    [SerializeField] TextMeshProUGUI m_totalGold;// 초당 골드 생산량 텍스트
    public TextMeshProUGUI GettotalGoldText()
    {
        return m_totalGold;
    }

    [SerializeField] private int questBuyCount = 1;//퀘스트 구매하려는 갯수
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
    [SerializeField] List<Transform> m_list_Weapon = new List<Transform>();
    public void WeaponUpComplete(Transform _trs)
    {
        int index = m_list_Weapon.FindIndex(x => x == _trs);
        m_list_Weapon[index + 1].GetComponent<Weapon>().SetMaskActive(false);
    }
    [SerializeField] Transform m_WeaponParents;
    [SerializeField] TextMeshProUGUI m_totalAtk;
    public TextMeshProUGUI GettotalAtkText()
    {
        return m_totalAtk;
    }
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
    public void SetTopWeaponNum(int _num)
    {
        haveWeaponLv = _num;
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
        m_totalGold.text = "초당 골드생산량 : " + CalCulator.inst.StringFourDigitChanger(GameStatus.inst.TotalProdGold.ToString());
        m_totalAtk.text = "총 공격력 : " + CalCulator.inst.StringFourDigitChanger(GameStatus.inst.TotalAtk.ToString());
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

        m_aryPetDetailInforBtns[0].onClick.AddListener(() =>
        {
            PetDetailViewr_UI.inst.TopArrayBtnActive(0);
        });

        m_aryPetDetailInforBtns[1].onClick.AddListener(() =>
        {
            PetDetailViewr_UI.inst.TopArrayBtnActive(1);
        });

        m_aryPetDetailInforBtns[2].onClick.AddListener(() =>
        {
            PetDetailViewr_UI.inst.TopArrayBtnActive(2);
        });
    }
    public Sprite GetSelectUISprite(int num)
    {
        return m_BtnSprite[num];
    }

    void getGoldPerSceond()
    {
        GameStatus.inst.GetGold(GameStatus.inst.GetTotalGold());
    }

    public void changeSortOder(int value)
    {
        canvas.sortingOrder = value;
    }

    public int GetQuestLv(int index)//원하는 퀘스트의 레벨 가져오기
    {
        return m_list_Quest[index].GetComponent<Quest>().GetLv();
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
}
