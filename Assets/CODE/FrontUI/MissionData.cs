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

    [SerializeField] GameObject[] list_MissionWindow;// ����,�ְ�,Ư���̼�â
    [SerializeField] Image[] list_MissionTopBtnImage;// ��� ��ư �̹���
    [SerializeField] Sprite[] list_topBtnSelectSprite;// ��� ��ư ���� ��������Ʈ
    [SerializeField] Sprite[] list_topBtnNonSelectSprite;// ��� ��ư ���� ��������Ʈ
    int missionTypeIndex = 0;//������ ��� �̼� ��ư �ε�����ȣ
    int nowSpecialIndex { get; set; } = 0;//���� �������� ����� �̼� �ε���

    //////////////////���� �̼�//////////////////
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

    //////////////////�ְ� �̼�//////////////////
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

    //////////////////����� �̼�//////////////////
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

            NameText.text = $"{Instance.GetSpecialMyIndex(this) + 1}�ܰ� �̼�";
            MissionText.text = Name;
            imageIcon.sprite = UIManager.Instance.GetProdSprite((int)rewardTag);

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

            clearBtn.onClick.AddListener(() =>
            {
                clearBtn.gameObject.SetActive(false);
                needClearText.gameObject.SetActive(true);
                needClearText.text = "Ŭ����!";
                mask.SetActive(true);
                trs.SetParent(Instance.GetSpecialParents());
                trs.GetComponent<RectTransform>().sizeDelta = new Vector2(298, 60);
                trs.SetAsLastSibling();

                Instance.nowSpecialIndex = Instance.GetSpecialMyIndex(this) + 1;
                Instance.SetSpecialMissionRectPosition();
                
                switch (rewardTag)
                {
                    case ProductTag.Gold:
                        LetterManager.inst.MakeLetter(1, "�̼�", "Ư���̼� Ŭ����", int.Parse(rewardCount));
                        GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                    case ProductTag.Ruby:
                        LetterManager.inst.MakeLetter(1, "�̼�", "Ư���̼� Ŭ����", int.Parse(rewardCount));
                        GameStatus.inst.Ruby += int.Parse(CalCulator.inst.ConvertChartoIndex(rewardCount));
                        break;
                    case ProductTag.Star:
                        LetterManager.inst.MakeLetter(1, "�̼�", "Ư���̼� Ŭ����", int.Parse(rewardCount));
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
    [Header("����� �̼� ���")]
    [SerializeField] List<Special> list_SpecialMIssion = new List<Special>();

    List<Special> list_SpecialQuest = new List<Special>();//����� �̼� ����Ʈ
    List<Special> list_SpecialWeapon = new List<Special>();//����� �̼� ����
    List<Special> list_SpecialRelic = new List<Special>();//����� �̼� ����

    /// <summary>
    /// ����� �̼� ������ ������Ű�� ������ ����
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
    /// ����� �̼� �� ����
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
    /// ����� �̼� ���� �ε�����ȣ ����
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
        MissionOpenBtn = GameObject.Find("---[World UI Canvas]").transform.Find("StageUI/Right/QeustList/Button").GetComponent<Button>();
        obj_MissionWindow = obj_UICanvas.transform.Find("Mission");
        obj_DailyContents = obj_MissionWindow.Find("Mission/Window/Daily(Scroll View)").GetComponent<ScrollRect>().content;
        obj_WeeklyContents = obj_MissionWindow.Find("Mission/Window/Weekly(Scroll View)").GetComponent<ScrollRect>().content;
        obj_SpecialContents = obj_MissionWindow.Find("Mission/Window/Special(Scroll View)").GetComponent<ScrollRect>().content;
        trs_NowMissionParents = obj_MissionWindow.Find("Mission/Window/Special(Scroll View)/NowMission");

        //���� �̼� �ʱ�ȭ
        int DailyCount = obj_DailyContents.childCount;
        for (int iNum = 0; iNum < DailyCount; iNum++)
        {
            list_DailyMission.Add(obj_DailyContents.GetChild(iNum).GetComponent<Mission>());
        }

        //�ְ� �̼� �ʱ�ȭ
        int WeeklyCount = obj_WeeklyContents.childCount;
        for (int iNum = 0; iNum < WeeklyCount; iNum++)
        {
            list_WeeklyMission.Add(obj_WeeklyContents.GetChild(iNum).GetComponent<Mission>());
        }

        //����� �̼� ����Ʈ �ʱ�ȭ
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
        SetSpecialMissionRectPosition();//���� �������� Ư���̼� ������ ����

        //��ư �ʱ�ȭ
        UIManager.Instance.GetShopOpenBtn().onClick.AddListener(() => SetDailyMission("���� �湮", 1));
        MissionOpenBtn.onClick.AddListener(() =>
        {
            obj_MissionWindow.gameObject.SetActive(true);
            UIManager.Instance.changeSortOder(4);
        });
    }


    public void ClickMissionType(int value)//��� ����,�ְ�,����� �̼� ��ư Ŭ��
    {
        list_MissionWindow[missionTypeIndex].SetActive(false);
        list_MissionTopBtnImage[missionTypeIndex].sprite = list_topBtnNonSelectSprite[missionTypeIndex];
        missionTypeIndex = value;
        list_MissionWindow[missionTypeIndex].SetActive(true);
        list_MissionTopBtnImage[missionTypeIndex].sprite = list_topBtnSelectSprite[missionTypeIndex];
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
                    list_DailyMission[jNum].transform.SetAsFirstSibling();
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
                    list_WeeklyMission[jNum].transform.SetAsFirstSibling();
                    list_WeeklyMission[jNum].initMission();
                    break;
                }
            }
        }
    }
}
