using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
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
    [SerializeField] Image[] list_MissionTopBtnImage;
    [SerializeField] Sprite[] list_topBtnSelectSprite;
    [SerializeField] Sprite[] list_topBtnNonSelectSprite;
    int missionTypeNum = 0;


    //일일미션
    List<Mission> list_DailyMission = new List<Mission>();

    //주간미션
    List<Mission> list_WeeklyMission = new List<Mission>();

    //스페셜 미션
    [SerializeField] List<Special> list_SpecialMission = new List<Special>();
    [Serializable]
    public class Special
    {
        [SerializeField] public string Name;
        [SerializeField] int maxCount;
        [SerializeField] int CountGap;
        [SerializeField] public int QuestNum;
        [SerializeField] string rewardCount;
        [SerializeField] ProductTag rewardTag;
        Image imageIcon;
        Button moveBtn;
        Button clearBtn;
        TMP_Text NameText;
        TMP_Text rewardText;
        int count;
        public int Count
        {
            get => count;
            set
            {
                if (count < maxCount)
                {
                    count = value;
                    if (count >= maxCount)
                    {
                        clearBtn.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void initSpecialMission(Transform _trs)
        {
            imageIcon = _trs.Find("Icon_IMG").GetComponent<Image>();
            moveBtn = _trs.Find("MoveBtn").GetComponent<Button>();
            clearBtn = _trs.Find("ClearBtn").GetComponent<Button>();
            NameText = _trs.Find("Space/MissionText").GetComponent<TMP_Text>();
            rewardText = _trs.Find("Space/RewardText").GetComponent<TMP_Text>();

            NameText.text = Name + $"{maxCount}달성";
            imageIcon.sprite = UIManager.Instance.GetProdSprite((int)rewardTag);

            switch (rewardTag)
            {
                case ProductTag.Gold:
                    rewardText.text = $"골드 +{rewardCount}개";
                    break;
                case ProductTag.Ruby:
                    rewardText.text = $"루비 +{rewardCount}개";
                    break;
                case ProductTag.Star:
                    rewardText.text = $"별 +{rewardCount}개";
                    break;
            }
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
        obj_UICanvas = GameObject.Find("---[UI Canvas]");
        MissionOpenBtn = GameObject.Find("---[World UI Canvas]").transform.Find("StageUI/Right/QeustList/Button").GetComponent<Button>();
        obj_MissionWindow = obj_UICanvas.transform.Find("Mission");
        obj_DailyContents = obj_MissionWindow.Find("Mission/Window/Daily(Scroll View)").GetComponent<ScrollRect>().content;
        obj_WeeklyContents = obj_MissionWindow.Find("Mission/Window/Weekly(Scroll View)").GetComponent<ScrollRect>().content;
        obj_SpecialContents = obj_MissionWindow.Find("Mission/Window/Special(Scroll View)").GetComponent<ScrollRect>().content;

        int DailyCount = obj_DailyContents.childCount;
        for (int iNum = 0; iNum < DailyCount; iNum++)
        {
            list_DailyMission.Add(obj_DailyContents.GetChild(iNum).GetComponent<Mission>());
        }

        int WeeklyCount = obj_WeeklyContents.childCount;
        for (int iNum = 0; iNum < WeeklyCount; iNum++)
        {
            list_WeeklyMission.Add(obj_WeeklyContents.GetChild(iNum).GetComponent<Mission>());
        }

        int SpecialCount = obj_SpecialContents.childCount;
        for (int iNum = 0; iNum < SpecialCount; iNum++)
        {
            list_SpecialMission[iNum].initSpecialMission(obj_SpecialContents.GetChild(iNum));
        }

        UIManager.Instance.GetShopOpenBtn().onClick.AddListener(() => SetDailyMission("상점 방문", 1));
        MissionOpenBtn.onClick.AddListener(() =>
        {
            obj_MissionWindow.gameObject.SetActive(true);
            UIManager.Instance.changeSortOder(4);
        });
    }

    public void SetDailyMission(string Name, int count)
    {
        int listNum = -1;
        int listcount = list_DailyMission.Count;
        for (int iNum = 0; iNum < listcount; iNum++)
        {
            if (list_DailyMission[iNum].Name == Name)
            {
                listNum = iNum;
            }
        }

        list_DailyMission[listNum].Count += count;
    }

    public void SetWeeklyMission(string Name, int count)
    {
        int listNum = -1;
        int listcount = list_WeeklyMission.Count;
        for (int iNum = 0; iNum < listcount; iNum++)
        {
            if (list_WeeklyMission[iNum].Name == Name)
            {
                listNum = iNum;
            }
        }
        if (listNum != -1)
        {
            list_WeeklyMission[listNum].Count += count;
        }
    }

    public void SetSpecialMission(int Num, int count)
    {
        list_SpecialMission[Num].Count += count;
    }

    public void ClickMissionType(int value)
    {
        list_MissionWindow[missionTypeNum].SetActive(false);
        list_MissionTopBtnImage[missionTypeNum].sprite = list_topBtnNonSelectSprite[missionTypeNum];
        missionTypeNum = value;
        list_MissionWindow[missionTypeNum].SetActive(true);
        list_MissionTopBtnImage[missionTypeNum].sprite = list_topBtnSelectSprite[missionTypeNum];
    }
}
