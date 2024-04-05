using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionData : MonoBehaviour
{
    public static MissionData Instance;

    GameObject obj_UICanvas; //ĵ����
    Transform obj_MissionWindow; //�̼�â
    Transform obj_DailyContents; //���Ϲ̼� ������
    Transform obj_WeeklyContents; //�ְ��̼� ������
    Transform obj_SpecialContents; //����ȹ̼� ������

    Button MissionOpenBtn;//�̼�â ���� ��ư

    [SerializeField] GameObject[] list_MissionWindow;// ����,�ְ�,Ư���̼�â
    int missionTypeNum = 0;


    //���Ϲ̼�
    List<Image> list_DailyImageBar = new List<Image>();
    List<TMP_Text> list_DailyText = new List<TMP_Text>();
    List<GameObject> list_DailyMoverBtn = new List<GameObject>();
    List<GameObject> list_DailyClearBtn = new List<GameObject>();
    int dMissionClearCount;
    public int DMissionClearCount
    {
        get => dMissionClearCount;
        set
        {
            dMissionClearCount = value;
        }
    }
    int VisitShop = 0;
    int RubyUseCount;
    int EnemyKillCount;

    //�ְ��̼�
    List<Image> list_WeeklyImageBar = new List<Image>();
    List<TMP_Text> list_WeeklyText = new List<TMP_Text>();

    //����� �̼�
    List<Image> list_SpecialimageBar = new List<Image>();


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
        obj_UICanvas = GameObject.Find("---[UI Canvas]");
        MissionOpenBtn = GameObject.Find("---[World UI Canvas]").transform.Find("StageUI/Right/QeustList/Button").GetComponent<Button>();
        obj_MissionWindow = obj_UICanvas.transform.Find("Mission");
        obj_DailyContents = obj_MissionWindow.Find("Mission/Window/Daily(Scroll View)").GetComponent<ScrollRect>().content;
        obj_WeeklyContents = obj_MissionWindow.Find("Mission/Window/Weekly(Scroll View)").GetComponent<ScrollRect>().content;
        obj_SpecialContents = obj_MissionWindow.Find("Mission/Window/Special(Scroll View)").GetComponent<ScrollRect>().content;

        int dailyCount = obj_DailyContents.transform.childCount;
        for (int iNum = 0; iNum < dailyCount; iNum++)
        {
            list_DailyImageBar.Add(obj_DailyContents.GetChild(iNum).Find("Space/Playbar/PlayBar(Front)").GetComponent<Image>());
        }

        for (int iNum = 0; iNum < dailyCount; iNum++)
        {
            list_DailyText.Add(obj_DailyContents.GetChild(iNum).Find("Space/Playbar/Text (TMP)").GetComponent<TMP_Text>());
        }

        for (int iNum = 0; iNum < dailyCount; iNum++)
        {
            list_DailyMoverBtn.Add(obj_DailyContents.GetChild(iNum).Find("MoveBtn").gameObject);
            list_DailyClearBtn.Add(obj_DailyContents.GetChild(iNum).Find("ClearBtn").gameObject);
        }


        int weeklyCount = obj_WeeklyContents.transform.childCount;
        for (int iNum = 0; iNum < weeklyCount; iNum++)
        {
            list_WeeklyImageBar.Add(obj_WeeklyContents.GetChild(iNum).Find("Space/Playbar/PlayBar(Front)").GetComponent<Image>());
        }

        UIManager.Instance.GetShopOpenBtn().onClick.AddListener(() => SetVisitShop());
        MissionOpenBtn.onClick.AddListener(() => 
        {
            obj_MissionWindow.gameObject.SetActive(true);
            UIManager.Instance.changeSortOder(4);

        });
    }

    public void SetVisitShop()
    {
        if (VisitShop == 0)
        {
            VisitShop = 1;
            list_DailyImageBar[0].fillAmount = VisitShop;
            list_DailyText[0].text = $"{VisitShop} / {1}";
            list_DailyMoverBtn[0].SetActive(false);
            list_DailyClearBtn[0].SetActive(true);
        }
    }

    public void GetRubyReword(int count)
    {
        GameStatus.inst.Ruby += count;
        DMissionClearCount++;
    }

    public void ClickMissionType(int value)
    {
        list_MissionWindow[missionTypeNum].SetActive(false);
        missionTypeNum = value;
        list_MissionWindow[missionTypeNum].SetActive(true);
    }
}
