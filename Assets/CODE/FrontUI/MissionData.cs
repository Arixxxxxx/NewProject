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
    Button MissionOpenBtn;//�̼�â ���� ��ư

    [SerializeField] GameObject[] list_MissionWindow;// ����,�ְ�,Ư���̼�â
    [SerializeField] Image[] list_MissionTopBtnImage;
    [SerializeField] Sprite[] list_topBtnSelectSprite;
    [SerializeField] Sprite[] list_topBtnNonSelectSprite;
    int missionTypeNum = 0;


    //���Ϲ̼�
    List<Mission> list_DailyMission = new List<Mission>();

    //�ְ��̼�
    List<Mission> list_WeeklyMission = new List<Mission>();

    //����� �̼�
    List<Mission> list_SpecialMission = new List<Mission>();


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
        for (int iNum = 0; iNum < DailyCount; iNum++)
        {
            list_WeeklyMission.Add(obj_DailyContents.GetChild(iNum).GetComponent<Mission>());
        }
        int SpecialCount = obj_SpecialContents.childCount;
        for (int iNum = 0; iNum < DailyCount; iNum++)
        {
            list_SpecialMission.Add(obj_DailyContents.GetChild(iNum).GetComponent<Mission>());
        }

        UIManager.Instance.GetShopOpenBtn().onClick.AddListener(() => SetDailyMission("���� �湮", 1));
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

    public void SetSpecialMission(string Name, int count)
    {
        int listNum = -1;
        int listcount = list_SpecialMission.Count;
        for (int iNum = 0; iNum < listcount; iNum++)
        {
            if (list_SpecialMission[iNum].Name == Name)
            {
                listNum = iNum;
            }
        }
        if (listNum != -1)
        {
            list_SpecialMission[listNum].Count += count;
        }
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
