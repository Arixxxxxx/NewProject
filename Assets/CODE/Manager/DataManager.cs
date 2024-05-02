using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager inst;

    // 씬로딩 관련

    public bool loadEnd;
    int sceneNumber;


    public SaveData savedata = new SaveData();
    //파일명 수정해야됨
    string path = Path.Combine(Application.dataPath, "TestData.json");
    RectTransform ScreenArea;


    public class SaveData
    {
        public DateTime lastSigninDate;

        public string Name;
        
        // 1. 재화
        public string Gold = "0";
        public string Star = "0";
        public int Ruby;

        // 2. 동료 강화 재료
        public int Soul;
        public int book;
        public int born;

       // 3. 미니게임
        public int miniTicket;

        // 3. 버프 남은 시간
        public double buffAtkTime;
        public double buffGoldTime;
        public double buffMoveSpeedTime;
        public double buffBigAtkTime;

        // 4. 뉴비 혜택
        public int getNewbieRewardCount;
        public bool todayGetRaward;
        public DateTime newbieBuffLastDay;

        // 5. 출석체크
        public int[] GetGiftDay = new int[3];
        public int GetGiftCount;

        // 6. 캐릭터 관련
        public int AtkSpeedLv;
        public int HwanSeangCount;
        
        // 7. 게임현황 (스테이지)
        public int TotalFloor = 1;
        public int Stage = 1;
        public int NowFloor = 1;

        // 8. 동료 레벨
        public int Crew0Lv = 0;
        public int Crew1Lv = 0;
        public int Crew2Lv = 0;

        // 9. 메인 하단 UI 현황
        public int[] QuestLv = new int[30];
        public int[] WeaponLv = new int[30];
        public int[] RelicLv;
        public int NowEquipWeaponNum;

        // 10. 미션 현황
        public bool[] DailyMIssionClear;
        public bool[] WeeklyMissionClear;
        public bool[] SpMissionClear;
        public bool canResetDailyMission;
        public bool canResetWeeklyMission;

        //11. 빙고 현황
        public bool[] ClearBingo;
        public int BingoTicket;
    }


    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(inst);
        }
        CheckJsonFile();//최초실행시 json유무 확인
        DontDestroyOnLoad(inst);
        setScreen();
        //Screen.SetResolution(Screen.width, Screen.width / 9 * 16, true);

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadingManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePath();
        }
    }

    void setScreen()
    {
        //ScreenArea = GameObject.Find("---[UI Canvas]").transform.Find("ScreenArea").GetComponent<RectTransform>();

        int setWidth = 1080;
        int setheight = 1920;
        float setRatio = (float)setWidth / setheight;

        int deviceWitdh = Screen.width;
        int deviceHeight = Screen.height;
        float deviceRatio = (float)Screen.width / Screen.height;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWitdh) * setWidth), true);
        if (setRatio < deviceRatio)
        {
            float newWidth = setRatio / deviceRatio;
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else
        {
            float newHeight = deviceRatio / setRatio;
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }

    } 


    public void SavePath()
    { 
        string json = JsonUtility.ToJson(savedata);

        Debug.Log(path);
        File.WriteAllText(path, json);
    }

    bool isHaveJsonFile = false;
    public bool IshaveJsonFile { get { return isHaveJsonFile; } }

    public void CheckJsonFile()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            savedata = JsonConvert.DeserializeObject<SaveData>(json);
            isHaveJsonFile = true;
            Debug.Log(savedata.QuestLv[0]);
        }
    }

    public void SetName(string _name)
    {
        savedata.Name = _name;
        SavePath();
    }

    public SaveData Get_Savedata()
    {
        return savedata;
    }
}
