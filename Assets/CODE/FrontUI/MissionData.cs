using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class MissionData : MonoBehaviour
{
    public static MissionData Instance;

    GameObject obj_UICanvas; //ĵ����
    Transform obj_MissionWindow; //�̼�â
    Transform obj_DailyContents; //���Ϲ̼� ������
    Transform obj_WeeklyContents; //�ְ��̼� ������
    Transform obj_SpecialContents; //����ȹ̼� ������
    Transform trs_NowMissionParents; //������������ �̼� �θ�
    Button MissionOpenBtn;//�̼�â ���� ��ư
    TMP_Text topTitleText;//�̼�â ��� Ÿ��Ʋ �ؽ�Ʈ
    TMP_Text topDetailText;//�̼�â ��� �����ؽ�Ʈ
    TMP_Text worldTitleText;//����ȭ�� �̼� Ÿ��Ʋ �ؽ�Ʈ
    TMP_Text worldDetailText;//����ȭ�� �̼� ���� �ؽ�Ʈ

    [SerializeField] GameObject obj_Mission;//����,�ְ� �̼� ��
    [SerializeField] GameObject obj_SpecialMission;//����� �̼� ��
    GameObject[] list_MissionWindow = new GameObject[3];// ����,�ְ�,Ư���̼�â
    Image[] list_MissionTopBtnImage;// ��� ��ư �̹���
    [SerializeField] Sprite[] list_topBtnSelectSprite;// ��� ��ư ���� ��������Ʈ
    [SerializeField] Sprite[] list_topBtnNonSelectSprite;// ��� ��ư ���� ��������Ʈ
    int missionTypeIndex = 0;//������ ��� �̼� ��ư �ε�����ȣ
    int nowSpecialIndex { get; set; } = 0;//���� �������� ����� �̼� �ε���

    GameObject simball;
    int clearStack = 0;
    int ClearStack
    {
        get => clearStack;
        set
        {
            clearStack = value;
            if (clearStack == 0)
            {
                simball.SetActive(false);
            }
            else
            {
                simball.SetActive(true);
            }
        }
    }

    bool isCanResetDaily = true;
    bool isCanResetWeekly = true;

    //////////////////���� �̼�//////////////////
    [Header("���� �̼� ���")]
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
        public int Count
        {
            get => count;
            set
            {
                if (count < maxCount)
                {
                    count = value;
                    GameStatus.inst.SetDailyMissionCount(index,count);

                    if (isClear == false && count >= maxCount)
                    {
                        count = maxCount;
                        Instance.ClearStack++;
                        moveBtn.gameObject.SetActive(false);
                        clearBtn.gameObject.SetActive(true);

                    }
                    else if (isClear)
                    {
                        clearBtn.gameObject.SetActive(false);
                        Mask.SetActive(true);
                        ClearText.SetActive(true);
                    }

                    BarText.text = $"{count} / {maxCount}";
                    imageBar.fillAmount = (float)count / maxCount;
                }
            }
        }
        public void InitStart()
        {
            imageIcon = trs.Find("Icon_IMG").GetComponent<Image>();
            imageBar = trs.Find("Space/Playbar/PlayBar(Front)").GetComponent<Image>();
            moveBtn = trs.Find("MoveBtn").GetComponent<Button>();
            clearBtn = trs.Find("ClearBtn").GetComponent<Button>();
            NameText = trs.Find("Space/Title_Text").GetComponent<TMP_Text>();
            rewardText = trs.Find("Space/ReturnItem_Text").GetComponent<TMP_Text>();
            BarText = trs.Find("Space/Playbar/BarText").GetComponent<TMP_Text>();
            ClearText = trs.Find("ClearText").gameObject;
            Mask = trs.Find("Mask").gameObject;
            index = trs.GetSiblingIndex();

            isClear = GameStatus.inst.GetDailyMIssionClear(index);
            Debug.Log(isClear);
            Count = GameStatus.inst.GetDailyMissionCount(index);

            NameText.text = Name;
            switch (rewardTag)
            {
                case ProductTag.Gold:
                    rewardText.text = $"��� {rewardCount}��, ����Ƽ�� 10��";
                    break;
                case ProductTag.Ruby:
                    rewardText.text = $"��� {rewardCount}��, ����Ƽ�� 10��";
                    break;
                case ProductTag.Star:
                    rewardText.text = $"�� {rewardCount}��, ����Ƽ�� 10��";
                    break;
            }

            switch (MissionTag)
            {
                case DailyMissionTag.VisitShop:
                    moveBtn.onClick.AddListener(() =>
                    {
                        ShopManager.Instance.OpenShop(0);
                        Instance.obj_MissionWindow.gameObject.SetActive(false);
                    });
                    break;
                case DailyMissionTag.UseRuby:
                    moveBtn.onClick.AddListener(() =>
                    {
                        ShopManager.Instance.OpenShop(0);
                        Instance.obj_MissionWindow.gameObject.SetActive(false);
                    });
                    break;
                case DailyMissionTag.KillMonster:
                    moveBtn.onClick.AddListener(() =>
                    {
                        Instance.obj_MissionWindow.gameObject.SetActive(false);
                    });
                    break;
            }

            BarText.text = $"{count} / {maxCount}";
            imageBar.fillAmount = (float)Count / maxCount;
            imageIcon.sprite = UIManager.Instance.GetProdSprite((int)rewardTag);

            clearBtn.onClick.AddListener(() => { ClickClearBtn(); Instance.ClearStack--; });
        }

        public void ClickClearBtn()
        {
            isClear = true;
            switch (rewardTag)
            {
                case ProductTag.Gold:
                    LetterManager.inst.MakeLetter(1, "�̼�", "���Ϲ̼� Ŭ����!", int.Parse(rewardCount));
                    GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
                case ProductTag.Ruby:
                    LetterManager.inst.MakeLetter(0, "�̼�", "���Ϲ̼� Ŭ����!", int.Parse(rewardCount));
                    GameStatus.inst.Ruby += int.Parse(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
                case ProductTag.Star:
                    LetterManager.inst.MakeLetter(2, "�̼�", "���Ϲ̼� Ŭ����!", int.Parse(rewardCount));
                    GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
            }
            if (Name != "���Ϲ̼� Ŭ����")
            {
                Instance.SetDailyMission("���Ϲ̼� Ŭ����", 1);
            }
            else if (Name == "���Ϲ̼� Ŭ����")
            {
                Instance.SetWeeklyMission("���Ϲ̼� ��� Ŭ����", 1);
            }
            GameStatus.inst.RouletteTicket += 10;
            GameStatus.inst.SetDailyMIssionClear(index, true);
            clearBtn.gameObject.SetActive(false);
            trs.SetAsLastSibling();
            Mask.SetActive(true);
            ClearText.SetActive(true);
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
            Mask.SetActive(false);
            ClearText.SetActive(false);
            moveBtn.gameObject.SetActive(true);
        }
        public void SetClearState()
        {
            if (isClear)
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

    //////////////////�ְ� �̼�//////////////////
    [Header("�ְ� �̼� ���")]
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
        bool isClear;
        int count;
        public int Count
        {
            get => count;
            set
            {
                if (count < maxCount)
                {
                    count = value;
                    GameStatus.inst.SetWeeklyMissionCount(index, count);

                    if (isClear == false && count >= maxCount)
                    {
                        count = maxCount;
                        Instance.ClearStack++;
                        moveBtn.gameObject.SetActive(false);
                        clearBtn.gameObject.SetActive(true);
                    }
                    else if (isClear)
                    {
                        clearBtn.gameObject.SetActive(false);
                        Mask.SetActive(true);
                        ClearText.SetActive(true);
                    }

                    BarText.text = $"{count} / {maxCount}";
                    imageBar.fillAmount = (float)count / maxCount;
                }
            }
        }
        public void InitStart()
        {
            imageIcon = trs.Find("Icon_IMG").GetComponent<Image>();
            imageBar = trs.Find("Space/Playbar/PlayBar(Front)").GetComponent<Image>();
            moveBtn = trs.Find("MoveBtn").GetComponent<Button>();
            clearBtn = trs.Find("ClearBtn").GetComponent<Button>();
            NameText = trs.Find("Space/Title_Text").GetComponent<TMP_Text>();
            rewardText = trs.Find("Space/ReturnItem_Text").GetComponent<TMP_Text>();
            BarText = trs.Find("Space/Playbar/BarText").GetComponent<TMP_Text>();
            ClearText = trs.Find("ClearText").gameObject;
            Mask = trs.Find("Mask").gameObject;
            index = trs.GetSiblingIndex();

            isClear = GameStatus.inst.GetWeeklyMIssionClear(index);
            Count = GameStatus.inst.GetWeeklyMissionCount(index);

            NameText.text = Name;
            switch (rewardTag)
            {
                case ProductTag.Gold:
                    rewardText.text = $"��� +{rewardCount}��";
                    break;
                case ProductTag.Ruby:
                    rewardText.text = $"��� +{rewardCount}��";
                    break;
                case ProductTag.Star:
                    rewardText.text = $"�� +{rewardCount}��";
                    break;
            }

            switch (missionTag)
            {
                case WeeklyMissionTag.DailyMissionAllClear:
                    moveBtn.onClick.AddListener(() =>
                    {
                        Instance.ClickMissionType(0);
                    });
                    break;
                case WeeklyMissionTag.Reincarnation:
                    moveBtn.onClick.AddListener(() =>
                    {
                        Instance.obj_MissionWindow.gameObject.SetActive(false);
                    });
                    break;
                case WeeklyMissionTag.QuestLvUp:
                    moveBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.ClickBotBtn(0);
                        Instance.obj_MissionWindow.gameObject.SetActive(false);
                    });
                    break;
                case WeeklyMissionTag.WeaponUpgrade:
                    moveBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.ClickBotBtn(1);
                        Instance.obj_MissionWindow.gameObject.SetActive(false);
                    });
                    break;
            }
            BarText.text = $"{count} / {maxCount}";
            imageBar.fillAmount = (float)Count / maxCount;
            imageIcon.sprite = UIManager.Instance.GetProdSprite((int)rewardTag);

            clearBtn.onClick.AddListener(() => { Instance.ClearStack--; ClickClearBtn(); });
        }

        public void ClickClearBtn()
        {
            isClear = true;
            switch (rewardTag)
            {
                case ProductTag.Gold:
                    LetterManager.inst.MakeLetter(1, "�̼�", "�ְ��̼� Ŭ����!", int.Parse(rewardCount));
                    GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
                case ProductTag.Ruby:
                    LetterManager.inst.MakeLetter(0, "�̼�", "�ְ��̼� Ŭ����!", int.Parse(rewardCount));
                    GameStatus.inst.Ruby += int.Parse(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
                case ProductTag.Star:
                    LetterManager.inst.MakeLetter(2, "�̼�", "�ְ��̼� Ŭ����!", int.Parse(rewardCount));
                    GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(rewardCount));
                    break;
            }
            GameStatus.inst.SetWeeklyMIssionClear(index, true);
            clearBtn.gameObject.SetActive(false);
            trs.SetAsLastSibling();
            Mask.SetActive(true);
            ClearText.SetActive(true);
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
            Mask.SetActive(false);
            ClearText.SetActive(false);
            moveBtn.gameObject.SetActive(true);
        }
        public void SetClearState()
        {
            if (isClear)
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

    //////////////////����� �̼�//////////////////
    [Serializable]
    public class SpecialMIssion
    {
        [SerializeField] public string Name;
        [SerializeField] int maxCount;
        [SerializeField] public SpMissionTag missionType;
        [SerializeField] int typeindex;
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

        int count;
        public int Count
        {
            get => count;
            set
            {
                if (count < maxCount)
                {
                    count = value;
                    GameStatus.inst.SetSpecialMissionCount(index,count);
                    if (isActive && count >= maxCount)
                    {
                        count = maxCount;
                        GameStatus.inst.SetSpecialMissionCount(index, count);
                        moveBtn.gameObject.SetActive(false);
                        clearBtn.gameObject.SetActive(true);
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

            index = trs.GetSiblingIndex();
            NameText.text = $"{index + 1}�ܰ� �̼�";
            MissionText.text = Name;
            imageIcon.sprite = UIManager.Instance.GetProdSprite((int)rewardTag);
            Count = GameStatus.inst.GetSpecailMissionCount(index);

            switch (rewardTag)
            {
                case ProductTag.Gold:
                    rewardText.text = $"��� +{rewardCount}��";
                    break;
                case ProductTag.Ruby:
                    rewardText.text = $"��� +{rewardCount}��";
                    break;
                case ProductTag.Star:
                    rewardText.text = $"�� +{rewardCount}��";
                    break;
            }

            switch (missionType)
            {
                case SpMissionTag.Quest:
                    moveBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.ClickBotBtn(0);
                        Instance.obj_MissionWindow.gameObject.SetActive(false);
                    });
                    break;
                case SpMissionTag.Weapon:
                    moveBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.ClickBotBtn(1);
                        Instance.obj_MissionWindow.gameObject.SetActive(false);
                    });
                    break;
                case SpMissionTag.Relic:
                    moveBtn.onClick.AddListener(() =>
                    {
                        UIManager.Instance.ClickBotBtn(2);
                        Instance.obj_MissionWindow.gameObject.SetActive(false);
                    });
                    break;
            }

            clearBtn.onClick.AddListener(() =>
            {
                isActive = false;
                clearBtn.gameObject.SetActive(false);
                needClearText.gameObject.SetActive(true);
                needClearText.text = "Ŭ����!";
                mask.SetActive(true);
                trs.SetParent(Instance.GetSpecialParents());
                trs.GetComponent<RectTransform>().sizeDelta = new Vector2(278, 60);
                trs.SetAsLastSibling();
                GameStatus.inst.SetWeeklyMIssionClear(index, true);
                Instance.nowSpecialIndex = index + 1;
                GameStatus.inst.SpecialMIssionClearNum = Instance.nowSpecialIndex;
                Instance.SetSpecialMissionRectPosition();

                switch (rewardTag)
                {
                    case ProductTag.Gold:
                        LetterManager.inst.MakeLetter(1, "�̼�", "Ư���̼� Ŭ����!", int.Parse(rewardCount));
                        GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                    case ProductTag.Ruby:
                        LetterManager.inst.MakeLetter(0, "�̼�", "Ư���̼� Ŭ����!", int.Parse(rewardCount));
                        GameStatus.inst.Ruby += int.Parse(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                    case ProductTag.Star:
                        LetterManager.inst.MakeLetter(2, "�̼�", "Ư���̼� Ŭ����!", int.Parse(rewardCount));
                        GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                }
            });
        }
        public void SetClearState()
        {
            moveBtn.gameObject.SetActive(false);
            clearBtn.gameObject.SetActive(false);
            needClearText.gameObject.SetActive(true);
            needClearText.text = "Ŭ����!";
            mask.SetActive(true);
            trs.SetAsLastSibling();
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
    [Header("����� �̼� ���")]
    [SerializeField] List<SpecialMIssion> list_SpecialMIssion = new List<SpecialMIssion>();

    List<SpecialMIssion> list_SpecialQuest = new List<SpecialMIssion>();//����� �̼� ����Ʈ
    List<SpecialMIssion> list_SpecialWeapon = new List<SpecialMIssion>();//����� �̼� ����
    List<SpecialMIssion> list_SpecialRelic = new List<SpecialMIssion>();//����� �̼� ����

    /// <summary>
    /// ����� �̼� ������ ������Ű�� ������ ����
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

            topTitleText.text = $"{nowSpecialIndex + 1}�ܰ� �̼�";
            topDetailText.text = list_SpecialMIssion[nowSpecialIndex].Name;
            worldTitleText.text = $"{nowSpecialIndex + 1}�ܰ� �̼�";
            worldDetailText.text = list_SpecialMIssion[nowSpecialIndex].Name;
        }
    }

    /// <summary>
    /// ����� �̼� �� ����
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
                if (list_SpecialWeapon.Count >= Num && Num > 0)
                {
                    list_SpecialWeapon[Num - 1].Count = count;
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

    // ����� �̼� �θ� ����
    public Transform GetSpecialParents()
    {
        return obj_SpecialContents;
    }

    // ����� �̼� ��� ���� �θ� ����
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
        Transform worldUiCanvas = GameObject.Find("---[World UI Canvas]").transform;
        MissionOpenBtn = worldUiCanvas.Find("StageUI/QeustList/Button").GetComponent<Button>();
        simball = worldUiCanvas.Find("StageUI/QeustList/Button/Simball").gameObject;
        worldTitleText = worldUiCanvas.Find("StageUI/QeustList/BG/Step").GetComponent<TMP_Text>();
        worldDetailText = worldUiCanvas.Find("StageUI/QeustList/BG/Text").GetComponent<TMP_Text>();

        obj_MissionWindow = obj_UICanvas.transform.Find("ScreenArea/Mission");
        list_MissionWindow[0] = obj_MissionWindow.Find("Mission/Window/Daily(Scroll View)").gameObject;
        list_MissionWindow[1] = obj_MissionWindow.Find("Mission/Window/Weekly(Scroll View)").gameObject;
        list_MissionWindow[2] = obj_MissionWindow.Find("Mission/Window/Special(Scroll View)").gameObject;
        topTitleText = obj_MissionWindow.Find("Mission/Window/TopBar_Mission/Title_Text").GetComponent<TMP_Text>();
        topDetailText = obj_MissionWindow.Find("Mission/Window/TopBar_Mission/Detail_Text").GetComponent<TMP_Text>();

        Transform trsTopBtn = obj_MissionWindow.Find("Mission/Window/Top_Btn");
        int TopBtnCount = trsTopBtn.childCount;
        list_MissionTopBtnImage = new Image[TopBtnCount];
        for (int iNum = 0; iNum < TopBtnCount; iNum++)
        {
            list_MissionTopBtnImage[iNum] = trsTopBtn.GetChild(iNum).GetComponent<Image>();
        }

        obj_DailyContents = list_MissionWindow[0].GetComponent<ScrollRect>().content;
        obj_WeeklyContents = list_MissionWindow[1].GetComponent<ScrollRect>().content;
        obj_SpecialContents = list_MissionWindow[2].GetComponent<ScrollRect>().content;
        trs_NowMissionParents = obj_MissionWindow.Find("Mission/Window/Special(Scroll View)/NowMission");


        //���� �̼� �ʱ�ȭ
        int DailyCount = list_DailyMission.Count;
        for (int iNum = 0; iNum < DailyCount; iNum++)
        {
            list_DailyMission[iNum].Trs = Instantiate(obj_Mission, obj_DailyContents).transform;
            list_DailyMission[iNum].InitStart();
        }
        for (int iNum = 0; iNum < DailyCount; iNum++)
        {
            list_DailyMission[iNum].SetClearState();
        }

        //�ְ� �̼� �ʱ�ȭ
        int WeeklyCount = list_WeeklyMission.Count;
        for (int iNum = 0; iNum < WeeklyCount; iNum++)
        {
            list_WeeklyMission[iNum].Trs = Instantiate(obj_Mission, obj_WeeklyContents).transform;
            list_WeeklyMission[iNum].InitStart();
        }
        for (int iNum = 0; iNum < WeeklyCount; iNum++)
        {
            list_WeeklyMission[iNum].SetClearState();
        }

        //����� �̼� ����Ʈ �ʱ�ȭ
        int SpecialCount = list_SpecialMIssion.Count;
        for (int iNum = 0; iNum < SpecialCount; iNum++)
        {
            Transform trs = Instantiate(obj_SpecialMission, obj_SpecialContents).transform;

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
        SetSpecialMissionRectPosition();//���� �������� Ư���̼� ������ ����

        //��ư �ʱ�ȭ
        UIManager.Instance.GetShopOpenBtn().onClick.AddListener(() => SetDailyMission("���� �湮", 1));
        MissionOpenBtn.onClick.AddListener(() =>
        {
            obj_MissionWindow.gameObject.SetActive(true);
        });
    }

    // 1. ��� ����,�ְ�,����� �̼� ��ư Ŭ��
    /// <summary>
    /// ��� ����,�ְ�,����� �̼� ��ư Ŭ��
    /// </summary>
    /// <param name="value"></param>
    public void ClickMissionType(int value)
    {
        list_MissionWindow[missionTypeIndex].SetActive(false);
        list_MissionTopBtnImage[missionTypeIndex].sprite = list_topBtnNonSelectSprite[missionTypeIndex];
        missionTypeIndex = value;
        list_MissionWindow[missionTypeIndex].SetActive(true);
        list_MissionTopBtnImage[missionTypeIndex].sprite = list_topBtnSelectSprite[missionTypeIndex];
    }

    private void Update()
    {
        checkDay();
        checkWeek();
    }

    void checkDay()
    {
        if (DateTime.Now.ToString("HH") == "00" && isCanResetDaily == true)
        {
            isCanResetDaily = false;
            initDailyMission();
        }
        else if (DateTime.Now.ToString("HH") != "00")
        {
            isCanResetDaily = true;
        }
    }
    void checkWeek()
    {
        if (DateTime.Today.DayOfWeek == DayOfWeek.Monday && isCanResetWeekly == true)
        {
            isCanResetWeekly = false;
            initWeeklyMission();
        }
        else if (DateTime.Today.DayOfWeek != DayOfWeek.Monday)
        {
            isCanResetWeekly = true;
        }
    }

    void initDailyMission()//���� �̼� �ʱ�ȭ
    {
        int count = list_DailyMission.Count;

        for (int iNum = count - 1; iNum >= 0; iNum--)
        {
            for (int jNum = 0; jNum < count; jNum++)
            {
                if (list_DailyMission[jNum].GetIndex() == iNum)
                {
                    list_DailyMission[jNum].Trs.SetAsFirstSibling();
                    list_DailyMission[jNum].initMission();
                    break;
                }
            }
        }
    }
    void initWeeklyMission()//�ְ� �̼� �ʱ�ȭ
    {
        int count = list_WeeklyMission.Count;

        for (int iNum = count - 1; iNum >= 0; iNum--)
        {
            for (int jNum = 0; jNum < count; jNum++)
            {
                if (list_WeeklyMission[jNum].GetIndex() == iNum)
                {
                    list_WeeklyMission[jNum].Trs.SetAsFirstSibling();
                    list_WeeklyMission[jNum].initMission();
                    break;
                }
            }
        }
    }
}
