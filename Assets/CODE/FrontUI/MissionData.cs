using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionData : MonoBehaviour
{
    public static MissionData Instance;

    int[] QuestLv;//각 퀘스트별 레벨

    GameObject obj_FrontUICanvas;
    Transform obj_MissionWindow;
    Transform obj_DailyContents;
    Transform obj_WeeklyContents;
    Transform obj_SpecialContents;

    List<Image> list_DailyImageBar = new List<Image>();
    List<Image> list_WeeklyImageBar = new List<Image>();
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
        obj_FrontUICanvas = GameObject.Find("---[FrontUICanvas]");
        obj_MissionWindow = obj_FrontUICanvas.transform.Find("Active_WindowUI").Find("Mission").Find("Window");
        obj_DailyContents = obj_MissionWindow.Find("Daily(Scroll View)").GetComponent<ScrollRect>().content;
        obj_WeeklyContents = obj_MissionWindow.Find("Weekly(Scroll View)").GetComponent<ScrollRect>().content;
        obj_SpecialContents = obj_MissionWindow.Find("Special(Scroll View)").GetComponent<ScrollRect>().content;

        int dailyCount = obj_DailyContents.transform.childCount;
        for (int iNum = 0; iNum < dailyCount; iNum++)
        {
            list_DailyImageBar.Add(obj_DailyContents.GetChild(iNum).Find("Space/Playbar/PlayBar(Front)test").GetComponent<Image>());
        }

        int weeklyCount = obj_WeeklyContents.transform.childCount;
        for (int iNum = 0; iNum < weeklyCount; iNum++)
        {
            list_WeeklyImageBar.Add(obj_WeeklyContents.GetChild(iNum).Find("Space").Find("Playbar").Find("PlayBar(Front)").GetComponent<Image>());
        } 
    }

    void Update()
    {
        
    }

    public void SetQuestLv(int index, int lv)
    {
        QuestLv[index] = lv;

    }
}
