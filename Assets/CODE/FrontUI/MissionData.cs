using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionData : MonoBehaviour
{
    public static MissionData Instance;

    GameObject obj_UICanvas; //캔버스
    Transform obj_MissionWindow; //미션창
    Transform obj_DailyContents; //일일미션 컨텐츠
    Transform obj_WeeklyContents; //주간미션 컨텐츠
    Transform obj_SpecialContents; //스페셜미션 컨텐츠

    Button MissionOpenBtn;//미션창 여는 버튼

    [SerializeField] GameObject[] list_MissionWindow;// 일일,주간,특별미션창
    int missionTypeNum = 0;


    //일일미션
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

    //주간미션
    List<Image> list_WeeklyImageBar = new List<Image>();
    List<TMP_Text> list_WeeklyText = new List<TMP_Text>();

    //스페셜 미션
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
