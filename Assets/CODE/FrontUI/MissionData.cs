using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class MissionData : MonoBehaviour
{
    public static MissionData Instance;

    Canvas UICanvas; //캔버스
    Transform trs_MissionWindow; //미션창
    Transform trs_DailyContents; //일일미션 컨텐츠
    Transform trs_WeeklyContents; //주간미션 컨텐츠
    Transform trs_SpecialContents; //스페셜미션 컨텐츠
    Transform trs_NowMissionParents; //현재진행중인 미션 부모
    Button MissionOpenBtn;//미션창 여는 버튼
    Button MissionCloseBtn;//미션창 닫는 버튼
    Button MissionBGBtn;//미션창 백그라운드 버튼
    Button topMoveBtn;//미션창 상단 이동 버튼
    TMP_Text topTitleText;//미션창 상단 타이틀 텍스트
    TMP_Text topDetailText;//미션창 상단 세부텍스트
    TMP_Text worldTitleText;//메인화면 미션 타이틀 텍스트
    TMP_Text worldDetailText;//메인화면 미션 세부 텍스트
    Image missionIconBG;
    Animator missionIconAnim;

    [SerializeField] GameObject obj_Mission;//일일,주간 미션 바
    [SerializeField] GameObject obj_SpecialMission;//스페셜 미션 바
    GameObject[] list_MissionWindow = new GameObject[3];// 일일,주간,특별미션창
    Image[] list_MissionTopBtnImage;// 상단 버튼 이미지
    [SerializeField] Sprite[] list_topBtnSelectSprite;// 상단 버튼 선택 스프라이트
    [SerializeField] Sprite[] list_topBtnNonSelectSprite;// 상단 버튼 비선택 스프라이트
    int missionTypeIndex = 0;//선택한 상단 미션 버튼 인덱스번호
    int nowSpecialIndex { get; set; } = 0;//현재 진행중인 스페셜 미션 인덱스

    GameObject simball;
    GameObject dialySimball;
    GameObject weeklySimball;
    GameObject specialSimball;

    int clearStack = 0;
    int ClearStack
    {
        get => clearStack;
        set
        {
            clearStack = value;
            if (clearStack <= 0)
            {
                simball.SetActive(false);
            }
            else
            {
                simball.SetActive(true);
            }
        }
    }
    int dailyClearStack;
    int DailyClearStack
    {
        get => dailyClearStack;
        set
        {
            dailyClearStack = value;
            if (dailyClearStack <= 0)
            {
                dialySimball.SetActive(false);
            }
            else
            {
                dialySimball.SetActive(true);
            }
        }
    }
    int weeklyClearStack;
    int WeeklyClearStack
    {
        get => weeklyClearStack;
        set
        {
            weeklyClearStack = value;
            if (weeklyClearStack <= 0)
            {
                weeklySimball.SetActive(false);
            }
            else
            {
                weeklySimball.SetActive(true);
            }
        }
    }

    int specialClearStack;
    int SpecialClearStack
    {
        get => specialClearStack;
        set
        {
            specialClearStack = value;
            if (specialClearStack <= 0)
            {
                specialSimball.SetActive(false);
            }
            else
            {
                specialSimball.SetActive(true);
            }
        }
    }

    //////////////////일일 미션//////////////////
    [Header("일일 미션 목록")]
    [SerializeField] List<DailyMission> list_DailyMission = new List<DailyMission>();
    [Serializable]
    public class DailyMission
    {
        [SerializeField] string Name;
        [SerializeField] int maxCount;
        [SerializeField] string rewardCount;
        [SerializeField] ProductTag rewardTag;
        [SerializeField] DailyMissionTag MissionTag;
        int index;
        bool isClear;
        bool IsClear
        {
            get => isClear;
            set
            {
                isClear = value;
                GameStatus.inst.SetDailyMIssionClear(index, value);
            }
        }
        Transform trs;
        public Transform Trs { get => trs; set { trs = value; } }
        Image imageIcon;
        Image imageBar;
        Button moveBtn;
        Button clearBtn;
        TMP_Text NameText;
        TMP_Text rewardText;
        TMP_Text BarText;
        GameObject ClearText;
        GameObject Mask;
        int count;
        bool isMaxCount = false;
        public int Count
        {
            get => count;
            set
            {
                count = value;

                if (IsClear == false && count >= maxCount)
                {
                    if (isMaxCount == false)
                    {
                        isMaxCount = true;
                        Instance.ClearStack++;
                        Instance.DailyClearStack++;
                        Instance.SetIconBGColor(true);
                    }
                    count = maxCount;
                    moveBtn.gameObject.SetActive(false);
                    clearBtn.gameObject.SetActive(true);

                }
                else if (IsClear)
                {
                    count = maxCount;
                    clearBtn.gameObject.SetActive(false);
                    moveBtn.gameObject.SetActive(false);
                    Mask.SetActive(true);
                    ClearText.SetActive(true);
                }

                GameStatus.inst.SetDailyMissionCount(index, count);
                BarText.text = $"{count} / {maxCount}";
                imageBar.fillAmount = (float)count / maxCount;

            }
        }
        public void InitStart()
        {
            imageIcon = trs.Find("IconBG/Icon_IMG").GetComponent<Image>();
            imageBar = trs.Find("Space/Playbar/PlayBar(Front)").GetComponent<Image>();
            moveBtn = trs.Find("MoveBtn").GetComponent<Button>();
            clearBtn = trs.Find("ClearBtn").GetComponent<Button>();
            NameText = trs.Find("Space/Title_Text").GetComponent<TMP_Text>();
            rewardText = trs.Find("Space/ReturnItem_Text").GetComponent<TMP_Text>();
            BarText = trs.Find("Space/Playbar/BarText").GetComponent<TMP_Text>();
            ClearText = trs.Find("ClearText").gameObject;
            Mask = trs.Find("Mask").gameObject;
            index = trs.GetSiblingIndex();

            IsClear = GameStatus.inst.GetDailyMIssionClear(index);
            Count = GameStatus.inst.GetDailyMissionCount(index);

            NameText.text = Name;
            switch (rewardTag)
            {
                case ProductTag.Gold:
                    rewardText.text = $"골드 {rewardCount}개, 빙고티켓 10개";
                    break;
                case ProductTag.Ruby:
                    rewardText.text = $"루비 {rewardCount}개, 빙고티켓 10개";
                    break;
                case ProductTag.Star:
                    rewardText.text = $"별 {rewardCount}개, 빙고티켓 10개";
                    break;
            }

            switch (MissionTag)
            {
                case DailyMissionTag.VisitShop:
                    moveBtn.onClick.AddListener(() =>
                    {
                        ShopManager.inst.Active_Shop(0,true);
                        Instance.trs_MissionWindow.gameObject.SetActive(false);
                        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    });
                    break;
                case DailyMissionTag.UseRuby:
                    moveBtn.onClick.AddListener(() =>
                    {
                        ShopManager.inst.Active_Shop(0,true);
                        Instance.trs_MissionWindow.gameObject.SetActive(false);
                        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    });
                    break;
                case DailyMissionTag.KillMonster:
                    moveBtn.onClick.AddListener(() =>
                    {
                        Instance.trs_MissionWindow.gameObject.SetActive(false);
                        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    });
                    break;
            }

            BarText.text = $"{count} / {maxCount}";
            imageBar.fillAmount = (float)Count / maxCount;
            imageIcon.sprite = UIManager.Instance.GetProdSprite((int)rewardTag);
            if (rewardTag == ProductTag.Star)
            {
                imageIcon.rectTransform.sizeDelta = new Vector2(23, 23);
            }

            clearBtn.onClick.AddListener(() => { ClickClearBtn(); });
        }

        public void ClickClearBtn()
        {
            IsClear = true;
            switch (rewardTag)
            {
                case ProductTag.Gold:
                    LetterManager.inst.MakeLetter(1, "미션", "일일미션 클리어!", int.Parse(rewardCount));
                    GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
                case ProductTag.Ruby:
                    LetterManager.inst.MakeLetter(0, "미션", "일일미션 클리어!", int.Parse(rewardCount));
                    GameStatus.inst.Ruby += int.Parse(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
                case ProductTag.Star:
                    LetterManager.inst.MakeLetter(2, "미션", "일일미션 클리어!", int.Parse(rewardCount));
                    GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
            }
            if (Name != "일일미션 클리어")
            {
                Instance.SetDailyMission("일일미션 클리어", 1);
            }
            else if (Name == "일일미션 클리어")
            {
                Instance.SetWeeklyMission("일일미션 모두 클리어", 1);
            }
            GameStatus.inst.RouletteTicket += 10;
            GameStatus.inst.SetDailyMIssionClear(index, true);
            clearBtn.gameObject.SetActive(false);
            trs.SetAsLastSibling();
            Mask.SetActive(true);
            ClearText.SetActive(true);
            Instance.ClearStack--;
            Instance.DailyClearStack--;
            Instance.SetIconBGColor(false);
            AudioManager.inst.Play_Ui_SFX(4, 0.8f);
        }

        public int GetIndex()
        {
            return index;
        }

        public string GetMissionName()
        {
            return Name;
        }
        public void initMission()
        {
            Count = 0;
            IsClear = false;
            Mask.SetActive(false);
            ClearText.SetActive(false);
            clearBtn.gameObject.SetActive(false);
            moveBtn.gameObject.SetActive(true);
        }
        public void SetClearState()
        {
            if (IsClear)
            {
                trs.SetAsLastSibling();
            }
        }
    }

    public void SetDailyMission(string Name, int count)
    {
        int listNum = -1;
        int listcount = list_DailyMission.Count;
        for (int iNum = 0; iNum < listcount; iNum++)
        {
            if (list_DailyMission[iNum].GetMissionName() == Name)
            {
                listNum = iNum;
                break;
            }
        }

        list_DailyMission[listNum].Count += count;
    }

    //////////////////주간 미션//////////////////
    [Header("주간 미션 목록")]
    [SerializeField] List<WeeklyMission> list_WeeklyMission = new List<WeeklyMission>();
    [Serializable]
    public class WeeklyMission
    {
        [SerializeField] string Name;
        int index;
        [SerializeField] int maxCount;
        [SerializeField] string rewardCount;
        [SerializeField] WeeklyMissionTag missionTag;
        [SerializeField] ProductTag rewardTag;
        Transform trs;
        public Transform Trs { get => trs; set { trs = value; } }
        Image imageIcon;
        Image imageBar;
        Button moveBtn;
        Button clearBtn;
        TMP_Text NameText;
        TMP_Text rewardText;
        TMP_Text BarText;
        GameObject ClearText;
        GameObject Mask;
        bool isMaxCount = false;
        bool isClear;
        bool IsClear
        {
            get => isClear;
            set
            {
                isClear = value;
                GameStatus.inst.SetWeeklyMIssionClear(index, value);
            }
        }
        int count;
        public int Count
        {
            get => count;
            set
            {

                count = value;

                if (IsClear == false && count >= maxCount)
                {
                    count = maxCount;
                    if (isMaxCount == false)
                    {
                        isMaxCount = true;
                        Instance.ClearStack++;
                        Instance.WeeklyClearStack++;
                        Instance.SetIconBGColor(true);
                    }
                    moveBtn.gameObject.SetActive(false);
                    clearBtn.gameObject.SetActive(true);
                }
                else if (IsClear)
                {
                    count = maxCount;
                    clearBtn.gameObject.SetActive(false);
                    Mask.SetActive(true);
                    ClearText.SetActive(true);
                }

                GameStatus.inst.SetWeeklyMissionCount(index, count);
                BarText.text = $"{count} / {maxCount}";
                imageBar.fillAmount = (float)count / maxCount;

            }
        }
        public void InitStart()
        {
            imageIcon = trs.Find("IconBG/Icon_IMG").GetComponent<Image>();
            imageBar = trs.Find("Space/Playbar/PlayBar(Front)").GetComponent<Image>();
            moveBtn = trs.Find("MoveBtn").GetComponent<Button>();
            clearBtn = trs.Find("ClearBtn").GetComponent<Button>();
            NameText = trs.Find("Space/Title_Text").GetComponent<TMP_Text>();
            rewardText = trs.Find("Space/ReturnItem_Text").GetComponent<TMP_Text>();
            BarText = trs.Find("Space/Playbar/BarText").GetComponent<TMP_Text>();
            ClearText = trs.Find("ClearText").gameObject;
            Mask = trs.Find("Mask").gameObject;
            index = trs.GetSiblingIndex();

            IsClear = GameStatus.inst.GetWeeklyMIssionClear(index);
            Count = GameStatus.inst.GetWeeklyMissionCount(index);

            NameText.text = Name;
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

            switch (missionTag)
            {
                case WeeklyMissionTag.DailyMissionAllClear:
                    moveBtn.onClick.AddListener(() =>
                    {
                        Instance.ClickMissionType(0);
                        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    });
                    break;
                case WeeklyMissionTag.Reincarnation:
                    moveBtn.onClick.AddListener(() =>
                    {
                        Instance.trs_MissionWindow.gameObject.SetActive(false);
                        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    });
                    break;
                case WeeklyMissionTag.QuestLvUp:
                    moveBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.ClickBotBtn(0);
                        Instance.trs_MissionWindow.gameObject.SetActive(false);
                        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    });
                    break;
                case WeeklyMissionTag.WeaponUpgrade:
                    moveBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.ClickBotBtn(1);
                        Instance.trs_MissionWindow.gameObject.SetActive(false);
                        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    });
                    break;
            }
            BarText.text = $"{count} / {maxCount}";
            imageBar.fillAmount = (float)Count / maxCount;
            imageIcon.sprite = UIManager.Instance.GetProdSprite((int)rewardTag);
            if (rewardTag == ProductTag.Star)
            {
                imageIcon.rectTransform.sizeDelta = new Vector2(23, 23);
            }

            clearBtn.onClick.AddListener(() => { ClickClearBtn(); });
        }

        public void ClickClearBtn()
        {
            IsClear = true;
            switch (rewardTag)
            {
                case ProductTag.Gold:
                    LetterManager.inst.MakeLetter(1, "미션", "주간미션 클리어!", int.Parse(rewardCount));
                    GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
                case ProductTag.Ruby:
                    LetterManager.inst.MakeLetter(0, "미션", "주간미션 클리어!", int.Parse(rewardCount));
                    GameStatus.inst.Ruby += int.Parse(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
                case ProductTag.Star:
                    LetterManager.inst.MakeLetter(2, "미션", "주간미션 클리어!", int.Parse(rewardCount));
                    GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
            }
            GameStatus.inst.SetWeeklyMIssionClear(index, true);
            clearBtn.gameObject.SetActive(false);
            trs.SetAsLastSibling();
            Mask.SetActive(true);
            ClearText.SetActive(true);
            Instance.ClearStack--;
            Instance.WeeklyClearStack--;
            Instance.SetIconBGColor(false);
            AudioManager.inst.Play_Ui_SFX(4, 0.8f);
        }

        public int GetIndex()
        {
            return index;
        }

        public string GetMissionName()
        {
            return Name;
        }
        public void initMission()
        {
            Count = 0;
            IsClear = false;
            Mask.SetActive(false);
            ClearText.SetActive(false);
            clearBtn.gameObject.SetActive(false);
            moveBtn.gameObject.SetActive(true);
        }
        public void SetClearState()
        {
            if (IsClear)
            {
                trs.SetAsLastSibling();
            }
        }
    }
    public void SetWeeklyMission(string Name, int count)
    {
        int listNum = -1;
        int listcount = list_WeeklyMission.Count;
        for (int iNum = 0; iNum < listcount; iNum++)
        {
            if (list_WeeklyMission[iNum].GetMissionName() == Name)
            {
                listNum = iNum;
                break;
            }
        }
        if (listNum != -1)
        {
            list_WeeklyMission[listNum].Count += count;
        }
    }

    //////////////////스페셜 미션//////////////////
    [Serializable]
    public class SpecialMIssion
    {
        [SerializeField] public string Name;
        [SerializeField] int maxCount;
        [SerializeField] public SpMissionTag missionType;
        //[SerializeField] int typeindex;
        [SerializeField] string rewardCount;
        [SerializeField] ProductTag rewardTag;
        [HideInInspector] Transform trs;
        public Transform Trs
        {
            get => trs; set { trs = value; }
        }
        Image imageIcon;
        GameObject mask;
        Button clearBtn;
        Button moveBtn;
        TMP_Text NameText;
        TMP_Text MissionText;
        TMP_Text rewardText;
        TMP_Text needClearText;

        int index;
        bool isActive = false;
        bool isMaxCount = false;

        int count;
        public int Count
        {
            get => count;
            set
            {
                count = value;
                if (count >= maxCount)
                {
                    count = maxCount;
                    if (isActive)
                    {
                        if (isMaxCount == false)
                        {
                            isMaxCount = true;
                            Instance.ClearStack++;
                            Instance.SpecialClearStack++;
                            Instance.SetIconBGColor(true);
                        }
                        moveBtn.gameObject.SetActive(false);
                        clearBtn.gameObject.SetActive(true);
                    }
                }
                GameStatus.inst.SetSpecialMissionCount(index, count);

            }
        }

        public void initSpecialMission(Transform _trs)
        {
            trs = _trs;
            imageIcon = _trs.Find("IconBG/Icon_IMG").GetComponent<Image>();
            moveBtn = _trs.Find("MoveBtn").GetComponent<Button>();
            clearBtn = _trs.Find("ClearBtn").GetComponent<Button>();
            NameText = _trs.Find("Space/Title_Text").GetComponent<TMP_Text>();
            MissionText = _trs.Find("Space/MissionText").GetComponent<TMP_Text>();
            rewardText = _trs.Find("Space/RewardText").GetComponent<TMP_Text>();
            needClearText = _trs.Find("NeedClearText").GetComponent<TMP_Text>();
            mask = _trs.Find("Mask").gameObject;

            index = trs.GetSiblingIndex();
            NameText.text = $"{index + 1}단계 미션";
            MissionText.text = Name;
            imageIcon.sprite = UIManager.Instance.GetProdSprite((int)rewardTag);

            Count = GameStatus.inst.GetSpecailMissionCount(index);


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

            switch (missionType)
            {
                case SpMissionTag.Quest:
                    moveBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.ClickBotBtn(0);
                        Instance.trs_MissionWindow.gameObject.SetActive(false);
                        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    });
                    break;
                case SpMissionTag.Weapon:
                    moveBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.ClickBotBtn(1);
                        Instance.trs_MissionWindow.gameObject.SetActive(false);
                        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    });
                    break;
                case SpMissionTag.Relic:
                    moveBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.ClickBotBtn(3);
                        Instance.trs_MissionWindow.gameObject.SetActive(false);
                        AudioManager.inst.Play_Ui_SFX(4, 0.8f);
                    });
                    break;
            }

            clearBtn.onClick.AddListener(() =>
            {
                isActive = false;
                clearBtn.gameObject.SetActive(false);
                needClearText.gameObject.SetActive(true);
                needClearText.text = "클리어!";
                mask.SetActive(true);
                trs.SetParent(Instance.GetSpecialParents());
                trs.GetComponent<RectTransform>().sizeDelta = new Vector2(235, 60);
                trs.SetAsLastSibling();
                Instance.nowSpecialIndex = index + 1;
                GameStatus.inst.SpecialMIssionClearNum = Instance.nowSpecialIndex;
                Instance.SetSpecialMissionRectPosition();
                Instance.ClearStack--;
                Instance.SpecialClearStack--;
                Instance.SetIconBGColor(false);

                switch (rewardTag)
                {
                    case ProductTag.Gold:
                        LetterManager.inst.MakeLetter(1, "미션", "특별미션 클리어!", int.Parse(rewardCount));
                        GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                    case ProductTag.Ruby:
                        LetterManager.inst.MakeLetter(0, "미션", "특별미션 클리어!", int.Parse(rewardCount));
                        GameStatus.inst.Ruby += int.Parse(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                    case ProductTag.Star:
                        LetterManager.inst.MakeLetter(2, "미션", "특별미션 클리어!", int.Parse(rewardCount));
                        GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                }

                AudioManager.inst.Play_Ui_SFX(4, 0.8f);
            });
        }
        public void SetClearState()
        {
            moveBtn.gameObject.SetActive(false);
            clearBtn.gameObject.SetActive(false);
            needClearText.gameObject.SetActive(true);
            needClearText.text = "클리어!";
            mask.SetActive(true);
            trs.SetAsLastSibling();
        }

        public SpMissionTag GetMissionType()
        {
            return missionType;
        }

        public void ActiveTrue()
        {
            isActive = true;
            if (count >= maxCount)
            {
                Count = Count;
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
    [SerializeField] List<SpecialMIssion> list_SpecialMIssion = new List<SpecialMIssion>();

    Color clearColor = new Vector4(0,0.7f,0,1);
    Color defaultColor = new Vector4(0,0,0,1);

    List <SpecialMIssion> list_SpecialQuest = new List<SpecialMIssion>();//스페셜 미션 퀘스트
    List<SpecialMIssion> list_SpecialWeapon = new List<SpecialMIssion>();//스페셜 미션 무기
    List<SpecialMIssion> list_SpecialRelic = new List<SpecialMIssion>();//스페셜 미션 유물

    /// <summary>
    /// 스페셜 미션 맨위에 고정시키고 사이즈 조절
    /// </summary>
    public void SetSpecialMissionRectPosition()
    {
        if (nowSpecialIndex <= list_SpecialMIssion.Count - 1)
        {
            list_SpecialMIssion[nowSpecialIndex].ActiveTrue();
            Transform nowtrs = list_SpecialMIssion[nowSpecialIndex].Trs;
            nowtrs.SetParent(trs_NowMissionParents);
            RectTransform nowRect = nowtrs.GetComponent<RectTransform>();
            nowRect.anchorMin = new Vector2(0, 0);
            nowRect.anchorMax = new Vector2(1, 1);
            nowRect.offsetMin = Vector2.zero;
            nowRect.offsetMax = Vector2.zero;

            topTitleText.text = $"{nowSpecialIndex + 1}단계 미션";
            topDetailText.text = list_SpecialMIssion[nowSpecialIndex].Name;
            worldTitleText.text = $"{nowSpecialIndex + 1}단계 미션";
            worldDetailText.text = list_SpecialMIssion[nowSpecialIndex].Name;
            topMoveBtn.onClick.RemoveAllListeners();
            topMoveBtn.onClick.AddListener(() =>
            {
                switch (list_SpecialMIssion[nowSpecialIndex].GetMissionType())
                {
                    case SpMissionTag.Quest:
                        UIManager.Instance.ClickBotBtn(0);

                        break;
                    case SpMissionTag.Weapon:
                        UIManager.Instance.ClickBotBtn(1);

                        break;
                    case SpMissionTag.Relic:
                        UIManager.Instance.ClickBotBtn(3);

                        break;
                }
                trs_MissionWindow.gameObject.SetActive(false);

            });
        }
    }

    public void SetIconBGColor(bool isClear)
    {
        if (isClear)
        {
            missionIconBG.color = clearColor;
            missionIconAnim.SetBool("clear", true);
        }
        else
        {
            missionIconBG.color = defaultColor;
            missionIconAnim.SetBool("clear", false);
        }
    }

    /// <summary>
    /// 스페셜 미션 값 설정
    /// </summary>
    /// <param name="Num"></param>
    /// <param name="count"></param>
    /// <param name="_type"></param>
    public void SetSpecialMission(int Num, int count, SpMissionTag _type)
    {
        switch (_type)
        {
            case SpMissionTag.Quest:
                if (list_SpecialQuest.Count - 1 >= Num)
                {
                    list_SpecialQuest[Num].Count = count;
                }
                break;
            case SpMissionTag.Weapon:
                if (list_SpecialWeapon.Count - 1 >= Num)
                {
                    list_SpecialWeapon[Num].Count = count;
                }
                break;
            case SpMissionTag.Relic:
                if (list_SpecialRelic.Count - 1 >= Num)
                {
                    list_SpecialRelic[Num].Count = count;
                }
                break;
        }
    }

    // 스페셜 미션 부모 리턴
    public Transform GetSpecialParents()
    {
        return trs_SpecialContents;
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

    TMP_Text[] topBtnText;
    Image[] topBtnIMG;

    void Start()
    {
        UICanvas = GameObject.Find("---[UI Canvas]").GetComponent<Canvas>();
        Transform worldUiCanvas = GameObject.Find("---[World UI Canvas]").transform;
        MissionOpenBtn = worldUiCanvas.Find("StageUI/QeustList/Button").GetComponent<Button>();
        missionIconBG = worldUiCanvas.Find("StageUI/QeustList/BG").GetComponent<Image>();
        missionIconAnim = worldUiCanvas.Find("StageUI/QeustList/BG").GetComponent<Animator>();
        simball = worldUiCanvas.Find("StageUI/QeustList/Button/Simball").gameObject;
        worldTitleText = worldUiCanvas.Find("StageUI/QeustList/BG/Step").GetComponent<TMP_Text>();
        worldDetailText = worldUiCanvas.Find("StageUI/QeustList/BG/Text").GetComponent<TMP_Text>();

        dialySimball = UICanvas.transform.Find("ScreenArea/Mission/Mission/Window/Top_Btn/Dilay/simball").gameObject;
        weeklySimball = UICanvas.transform.Find("ScreenArea/Mission/Mission/Window/Top_Btn/Week/simball").gameObject;
        specialSimball = UICanvas.transform.Find("ScreenArea/Mission/Mission/Window/Top_Btn/Special/simball").gameObject;
        MissionCloseBtn = UICanvas.transform.Find("ScreenArea/Mission/Mission/Window/Title/X_Btn").GetComponent<Button>();
        MissionBGBtn = UICanvas.transform.Find("ScreenArea/Mission/BG(B)").GetComponent<Button>();

        trs_MissionWindow = UICanvas.transform.Find("ScreenArea/Mission");
        list_MissionWindow[0] = trs_MissionWindow.Find("Mission/Window/Daily(Scroll View)").gameObject;
        list_MissionWindow[1] = trs_MissionWindow.Find("Mission/Window/Weekly(Scroll View)").gameObject;
        list_MissionWindow[2] = trs_MissionWindow.Find("Mission/Window/Special(Scroll View)").gameObject;
        topTitleText = trs_MissionWindow.Find("Mission/TopBar_Mission/Title_Text").GetComponent<TMP_Text>();
        topDetailText = trs_MissionWindow.Find("Mission/TopBar_Mission/Detail_Text").GetComponent<TMP_Text>();
        topMoveBtn = trs_MissionWindow.Find("Mission/TopBar_Mission/MoveBtn").GetComponent<Button>();

        Transform trsTopBtn = trs_MissionWindow.Find("Mission/Window/Top_Btn");
        topBtnText = trs_MissionWindow.Find("Mission/Window/Top_Btn").GetComponentsInChildren<TMP_Text>(true);
        topBtnIMG = new Image[topBtnText.Length];
        int childCount = trs_MissionWindow.Find("Mission/Window/Top_Btn").childCount;
        for (int index = 0; index < childCount; index++)
        {
            topBtnIMG[index] = trs_MissionWindow.Find("Mission/Window/Top_Btn").GetChild(index).GetComponent<Image>();
        }

        int TopBtnCount = trsTopBtn.childCount;
        list_MissionTopBtnImage = new Image[TopBtnCount];
        for (int iNum = 0; iNum < TopBtnCount; iNum++)
        {
            list_MissionTopBtnImage[iNum] = trsTopBtn.GetChild(iNum).GetComponent<Image>();
        }

        trs_DailyContents = list_MissionWindow[0].GetComponent<ScrollRect>().content;
        trs_WeeklyContents = list_MissionWindow[1].GetComponent<ScrollRect>().content;
        trs_SpecialContents = list_MissionWindow[2].GetComponent<ScrollRect>().content;
        trs_NowMissionParents = trs_MissionWindow.Find("Mission/Window/Special(Scroll View)/NowMission");


        //일일 미션 초기화
        int DailyCount = list_DailyMission.Count;
        for (int iNum = 0; iNum < DailyCount; iNum++)
        {
            list_DailyMission[iNum].Trs = Instantiate(obj_Mission, trs_DailyContents).transform;
            list_DailyMission[iNum].InitStart();
        }
        for (int iNum = 0; iNum < DailyCount; iNum++)
        {
            list_DailyMission[iNum].SetClearState();
        }

        //주간 미션 초기화
        int WeeklyCount = list_WeeklyMission.Count;
        for (int iNum = 0; iNum < WeeklyCount; iNum++)
        {
            list_WeeklyMission[iNum].Trs = Instantiate(obj_Mission, trs_WeeklyContents).transform;
            list_WeeklyMission[iNum].InitStart();
        }
        for (int iNum = 0; iNum < WeeklyCount; iNum++)
        {
            list_WeeklyMission[iNum].SetClearState();
        }

        //스페셜 미션 리스트 초기화
        int SpecialCount = list_SpecialMIssion.Count;
        for (int iNum = 0; iNum < SpecialCount; iNum++)
        {
            Transform trs = Instantiate(obj_SpecialMission, trs_SpecialContents).transform;

            if (list_SpecialMIssion[iNum].missionType == SpMissionTag.Quest)
            {
                list_SpecialQuest.Add(list_SpecialMIssion[iNum]);
            }

            if (list_SpecialMIssion[iNum].missionType == SpMissionTag.Weapon)
            {
                list_SpecialWeapon.Add(list_SpecialMIssion[iNum]);
            }

            if (list_SpecialMIssion[iNum].missionType == SpMissionTag.Relic)
            {
                list_SpecialRelic.Add(list_SpecialMIssion[iNum]);
            }

            list_SpecialMIssion[iNum].initSpecialMission(trs);
        }

        nowSpecialIndex = GameStatus.inst.SpecialMIssionClearNum;
        for (int iNum = 0; iNum < nowSpecialIndex; iNum++)
        {
            list_SpecialMIssion[iNum].SetClearState();
        }
        SetSpecialMissionRectPosition();//현재 진행중인 특별미션 맨위로 고정

        //버튼 초기화
        UIManager.Instance.GetShopOpenBtn().onClick.AddListener(() => SetDailyMission("상점 방문", 1));
        MissionOpenBtn.onClick.AddListener(() =>
        {
            trs_MissionWindow.gameObject.SetActive(true);
            UICanvas.sortingOrder = 15;
        });
        MissionCloseBtn.onClick.AddListener(() => { UICanvas.sortingOrder = 12; });
        MissionBGBtn.onClick.AddListener(() => { UICanvas.sortingOrder = 12; });
    }

    // 1. 상부 일일,주간,스페셜 미션 버튼 클릭
    /// <summary>
    /// 상부 일일,주간,스페셜 미션 버튼 클릭
    /// </summary>
    /// <param name="value"></param>
    public void ClickMissionType(int value)
    {
        list_MissionWindow[missionTypeIndex].SetActive(false); // 아래 뷰
        
        //list_MissionTopBtnImage[missionTypeIndex].sprite = list_topBtnNonSelectSprite[missionTypeIndex]; // 안눌렀을때
        
        missionTypeIndex = value;
        list_MissionWindow[missionTypeIndex].SetActive(true);
        Set_Nonclick_Color(value); // 이미지 변경에서 색상변경으로 변경 => 동은 '24.05.24
        AudioManager.inst.Play_Ui_SFX(4, 0.8f);

        //list_MissionTopBtnImage[missionTypeIndex].sprite = list_topBtnSelectSprite[missionTypeIndex]; //눌럿을때
    }

    Color nonClick = new Color(0.3f, 0.3f, 0.3f, 1);
    private void Set_Nonclick_Color(int indexNum)
    {
        for (int index = 0; index < topBtnIMG.Length; index++)
        {
            if(index == indexNum)
            {
                topBtnIMG[index].color = Color.white;
                topBtnText[index].color = Color.white;
            }
            else
            {
                topBtnIMG[index].color = nonClick;
                topBtnText[index].color = nonClick;

            }
        }
    }

    private void Update()
    {
        //checkDay();
        //checkWeek();

        if (Input.GetKeyDown(KeyCode.J))
        {
            initDailyMission();
            initWeeklyMission();
        }
    }

    void checkDay()
    {
        TimeSpan resetTime = new TimeSpan(5, 0, 0);
        DateTime lastResetTime;

        lastResetTime = GameStatus.inst.DailyMissionResetTime;


        DateTime now = DateTime.Now;

        DateTime lastResetDateTime = lastResetTime.Date + resetTime;// 마지막으로 초기화한 날의 5시
        DateTime todayResetDateTime = now.Date + resetTime;// 오늘 5시

        if (now >= todayResetDateTime && lastResetDateTime < todayResetDateTime)// 현재시간이 초기화시간을 넘었는지 && 마지막 초기화 시간이 오늘 초기화 시간보다 낮은지
        {
            initDailyMission();
            GameStatus.inst.DailyMissionResetTime = todayResetDateTime;
        }
    }
    void checkWeek()
    {
        TimeSpan resetTime = new TimeSpan(5, 0, 0);

        DateTime lastResetTime = GameStatus.inst.WeeklyMissionResetTime;

        DateTime now = DateTime.Now;
        DateTime lastMonday = getLastMonday(now);//이번주 월요일 리턴

        DateTime lastResetDateTime = lastResetTime.Date + resetTime;// 마지막으로 리셋한 날짜의 새벽 5시
        DateTime lastMondayDateTime = lastMonday.Date + resetTime;// 이번주 월요일 날짜의 새벽 5시 현재 월요일이라면 지난주 월요일 새벽 5시


        if (lastResetDateTime < lastMondayDateTime && now >= lastMondayDateTime)//마지막 리셋날짜가 이번주 월요일보다 전일 경우 && 현재시간이 이번주 월요일 리셋시간보다 앞일경우
        {
            initWeeklyMission();
            GameStatus.inst.WeeklyMissionResetTime = lastMonday;
        }
    }

    DateTime getLastMonday(DateTime date)
    {
        int daysSinceMonday = (int)date.DayOfWeek - (int)DayOfWeek.Monday;//오늘과 월요일의 차이 구하기
        if (daysSinceMonday < 0)//월요일이 아니면 이번주 월요일 도출 월요일이면 지난주 월요일 도출
        {
            daysSinceMonday += 7;
        }
        return date.AddDays(-daysSinceMonday).Date;//이번주 월요일 리턴
    }

    public void initDailyMission()//일일 미션 초기화
    {
        int count = list_DailyMission.Count;

        for (int iNum = count - 1; iNum >= 0; iNum--)
        {
            list_DailyMission[iNum].Trs.SetAsFirstSibling();
            list_DailyMission[iNum].initMission();
        }
    }
    public void initWeeklyMission()//주간 미션 초기화
    {
        int count = list_WeeklyMission.Count;

        for (int iNum = count - 1; iNum >= 0; iNum--)
        {

            list_WeeklyMission[iNum].Trs.SetAsFirstSibling();
            list_WeeklyMission[iNum].initMission();

        }
    }
}
