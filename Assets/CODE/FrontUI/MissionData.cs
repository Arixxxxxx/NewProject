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
    Transform trs_NowMissionParents; //현재진행중인 미션 부모
    Button MissionOpenBtn;//미션창 여는 버튼

    [SerializeField] GameObject[] list_MissionWindow;// 일일,주간,특별미션창
    [SerializeField] Image[] list_MissionTopBtnImage;// 상단 버튼 이미지
    [SerializeField] Sprite[] list_topBtnSelectSprite;// 상단 버튼 선택 스프라이트
    [SerializeField] Sprite[] list_topBtnNonSelectSprite;// 상단 버튼 비선택 스프라이트
    int missionTypeIndex = 0;//선택한 상단 미션 버튼 인덱스번호
    int nowSpecialIndex { get; set; } = 0;//현재 진행중인 스페셜 미션 인덱스

    //////////////////일일 미션//////////////////
    List<Mission> list_DailyMission = new List<Mission>();
    public void SetDailyMission(string Name, int count)
    {
        int listNum = -1;
        int listcount = list_DailyMission.Count;
        for (int iNum = 0; iNum < listcount; iNum++)
        {
            if (list_DailyMission[iNum].GetMissionName() == Name)
            {
                listNum = iNum;
            }
        }

        list_DailyMission[listNum].Count += count;
    }

    //////////////////주간 미션//////////////////
    List<Mission> list_WeeklyMission = new List<Mission>();
    public void SetWeeklyMission(string Name, int count)
    {
        int listNum = -1;
        int listcount = list_WeeklyMission.Count;
        for (int iNum = 0; iNum < listcount; iNum++)
        {
            if (list_WeeklyMission[iNum].GetMissionName() == Name)
            {
                listNum = iNum;
            }
        }
        if (listNum != -1)
        {
            list_WeeklyMission[listNum].Count += count;
        }
    }

    //////////////////스페셜 미션//////////////////
    [Serializable]
    public class Special
    {
        [SerializeField] public string Name;
        [SerializeField] int maxCount;
        [SerializeField] public MissionType missionType;
        [SerializeField] int typeindex;
        [SerializeField] string rewardCount;
        [SerializeField] ProductTag rewardTag;
        [HideInInspector] public Transform trs;
        Image imageIcon;
        GameObject mask;
        Button clearBtn;
        Button moveBtn;
        TMP_Text NameText;
        TMP_Text MissionText;
        TMP_Text rewardText;
        TMP_Text needClearText;

        bool isActive = false;

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
                        if (isActive)
                        {
                            moveBtn.gameObject.SetActive(false);
                            clearBtn.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        public void initSpecialMission(Transform _trs)
        {
            trs = _trs;
            imageIcon = _trs.Find("Icon_IMG").GetComponent<Image>();
            moveBtn = _trs.Find("MoveBtn").GetComponent<Button>();
            clearBtn = _trs.Find("ClearBtn").GetComponent<Button>();
            NameText = _trs.Find("Space/Title_Text").GetComponent<TMP_Text>();
            MissionText = _trs.Find("Space/MissionText").GetComponent<TMP_Text>();
            rewardText = _trs.Find("Space/RewardText").GetComponent<TMP_Text>();
            needClearText = _trs.Find("NeedClearText").GetComponent<TMP_Text>();
            mask = _trs.Find("Mask").gameObject;

            NameText.text = $"{Instance.GetSpecialMyIndex(this) + 1}단계 미션";
            MissionText.text = Name;
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

            clearBtn.onClick.AddListener(() =>
            {
                clearBtn.gameObject.SetActive(false);
                needClearText.gameObject.SetActive(true);
                needClearText.text = "클리어!";
                mask.SetActive(true);
                trs.SetParent(Instance.GetSpecialParents());
                trs.GetComponent<RectTransform>().sizeDelta = new Vector2(298, 60);
                trs.SetAsLastSibling();

                Instance.nowSpecialIndex = Instance.GetSpecialMyIndex(this) + 1;
                Instance.SetSpecialMissionRectPosition();
                
                switch (rewardTag)
                {
                    case ProductTag.Gold:
                        LetterManager.inst.MakeLetter(1, "미션", "특별미션 클리어", int.Parse(rewardCount));
                        GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                    case ProductTag.Ruby:
                        LetterManager.inst.MakeLetter(1, "미션", "특별미션 클리어", int.Parse(rewardCount));
                        GameStatus.inst.Ruby += int.Parse(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                    case ProductTag.Star:
                        LetterManager.inst.MakeLetter(1, "미션", "특별미션 클리어", int.Parse(rewardCount));
                        GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                }
            });
        }

        public void ActiveTrue()
        {
            isActive = true;
            if (count >= maxCount)
            {
                clearBtn.gameObject.SetActive(true);
            }
            else
            {
                moveBtn.gameObject.SetActive(true);
            }
            needClearText.gameObject.SetActive(false);
            mask.SetActive(false);
        }
    }
    [Header("스페셜 미션 목록")]
    [SerializeField] List<Special> list_SpecialMIssion = new List<Special>();

    List<Special> list_SpecialQuest = new List<Special>();//스페셜 미션 퀘스트
    List<Special> list_SpecialWeapon = new List<Special>();//스페셜 미션 무기
    List<Special> list_SpecialRelic = new List<Special>();//스페셜 미션 유물

    /// <summary>
    /// 스페셜 미션 맨위에 고정시키고 사이즈 조절
    /// </summary>
    public void SetSpecialMissionRectPosition()
    {
        if (nowSpecialIndex <= list_SpecialMIssion.Count - 1)
        {
            list_SpecialMIssion[nowSpecialIndex].ActiveTrue();
            Transform nowtrs = list_SpecialMIssion[nowSpecialIndex].trs;
            nowtrs.SetParent(trs_NowMissionParents);
            RectTransform nowRect = nowtrs.GetComponent<RectTransform>();
            nowRect.anchorMin = new Vector2(0, 0);
            nowRect.anchorMax = new Vector2(1, 1);
            nowRect.offsetMin = Vector2.zero;
            nowRect.offsetMax = Vector2.zero;
        }
    }

    /// <summary>
    /// 스페셜 미션 값 설정
    /// </summary>
    /// <param name="Num"></param>
    /// <param name="count"></param>
    /// <param name="_type"></param>
    public void SetSpecialMission(int Num, int count, MissionType _type)
    {
        switch (_type)
        {
            case MissionType.Quest:
                if (list_SpecialQuest.Count - 1 >= Num)
                {
                    list_SpecialQuest[Num].Count = count;
                }
                break;
            case MissionType.Weapon:
                if (list_SpecialWeapon.Count >= Num && Num > 0)
                {
                    list_SpecialWeapon[Num - 1].Count = count;
                }
                break;
            case MissionType.Relic:
                if (list_SpecialRelic.Count - 1 >= Num)
                {
                    list_SpecialRelic[Num].Count = count;
                }
                break;
        }
    }
    /// <summary>
    /// 스페셜 미션 본인 인덱스번호 리턴
    /// </summary>
    /// <param name="_sp"></param>
    /// <returns></returns>
    public int GetSpecialMyIndex(Special _sp)
    {
        int count = list_SpecialMIssion.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (list_SpecialMIssion[iNum] == _sp)
            {
                return iNum;
            }
        }
        return -1;
    }

    // 스페셜 미션 부모 리턴
    public Transform GetSpecialParents()
    {
        return obj_SpecialContents;
    }

    // 스페셜 미션 상단 고정 부모 리턴
    public Transform GetNowSpecialParents()
    {
        return trs_NowMissionParents;
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
        trs_NowMissionParents = obj_MissionWindow.Find("Mission/Window/Special(Scroll View)/NowMission");

        //일일 미션 초기화
        int DailyCount = obj_DailyContents.childCount;
        for (int iNum = 0; iNum < DailyCount; iNum++)
        {
            list_DailyMission.Add(obj_DailyContents.GetChild(iNum).GetComponent<Mission>());
        }

        //주간 미션 초기화
        int WeeklyCount = obj_WeeklyContents.childCount;
        for (int iNum = 0; iNum < WeeklyCount; iNum++)
        {
            list_WeeklyMission.Add(obj_WeeklyContents.GetChild(iNum).GetComponent<Mission>());
        }

        //스페셜 미션 리스트 초기화
        int SpecialCount = obj_SpecialContents.childCount;
        for (int iNum = 0; iNum < SpecialCount; iNum++)
        {
            if (list_SpecialMIssion[iNum].missionType == MissionType.Quest)
            {
                list_SpecialQuest.Add(list_SpecialMIssion[iNum]);
            }

            if (list_SpecialMIssion[iNum].missionType == MissionType.Weapon)
            {
                list_SpecialWeapon.Add(list_SpecialMIssion[iNum]);
            }

            if (list_SpecialMIssion[iNum].missionType == MissionType.Relic)
            {
                list_SpecialRelic.Add(list_SpecialMIssion[iNum]);
            }

            list_SpecialMIssion[iNum].initSpecialMission(obj_SpecialContents.GetChild(iNum));
        }
        SetSpecialMissionRectPosition();//현재 진행중인 특별미션 맨위로 고정

        //버튼 초기화
        UIManager.Instance.GetShopOpenBtn().onClick.AddListener(() => SetDailyMission("상점 방문", 1));
        MissionOpenBtn.onClick.AddListener(() =>
        {
            obj_MissionWindow.gameObject.SetActive(true);
            UIManager.Instance.changeSortOder(4);
        });
    }


    public void ClickMissionType(int value)//상부 일일,주간,스페셜 미션 버튼 클릭
    {
        list_MissionWindow[missionTypeIndex].SetActive(false);
        list_MissionTopBtnImage[missionTypeIndex].sprite = list_topBtnNonSelectSprite[missionTypeIndex];
        missionTypeIndex = value;
        list_MissionWindow[missionTypeIndex].SetActive(true);
        list_MissionTopBtnImage[missionTypeIndex].sprite = list_topBtnSelectSprite[missionTypeIndex];
    }

    void initDailyMission()//일일 미션 초기화
    {
        int count = list_DailyMission.Count;

        for (int iNum = count - 1; iNum >= 0; iNum--)
        {
            for (int jNum = 0; jNum < count; jNum++)
            {
                if (list_DailyMission[jNum].GetIndex() == iNum)
                {
                    list_DailyMission[jNum].transform.SetAsFirstSibling();
                    list_DailyMission[jNum].initMission();
                    break;
                }
            }
        }
    }
    void initWeeklyMission()//주간 미션 초기화
    {
        int count = list_WeeklyMission.Count;

        for (int iNum = count - 1; iNum >= 0; iNum--)
        {
            for (int jNum = 0; jNum < count; jNum++)
            {
                if (list_WeeklyMission[jNum].GetIndex() == iNum)
                {
                    list_WeeklyMission[jNum].transform.SetAsFirstSibling();
                    list_WeeklyMission[jNum].initMission();
                    break;
                }
            }
        }
    }
}
