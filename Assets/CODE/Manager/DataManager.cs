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
    Scene currentSceneIndex;
    int sceneIndexNumber;

    // 씬로딩 관련

    RectTransform ScreenArea;

    public SaveData savedata = new SaveData();
    string path = Path.Combine(Application.dataPath, "Save.json");

    bool isHaveJsonFile = false;
    public bool IshaveJsonFile { get { return isHaveJsonFile; } }


    [System.Serializable]
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
        public int GetGiftCount;
        public bool todayGetDailyReward;

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
        public int[] RelicLv = new int[11];
        public int NowEquipWeaponNum;

        // 10. 미션 현황
        public bool[] DailyMIssionClear = new bool[4];
        public bool[] WeeklyMissionClear = new bool[4];
        public int SpecialMissionClearNum;
        public int[] DailyMissionCount = new int[4];
        public int[] WeeklyMissionCount = new int[4];
        public int[] SpecialMissionCount = new int[6];
        public bool canResetDailyMission;
        public bool canResetWeeklyMission;

        //11. 빙고 현황
        public bool[] BingoBoard = new bool[8];
        public int RouletteTicket;
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
        currentSceneIndex = SceneManager.GetActiveScene();
        sceneIndexNumber = currentSceneIndex.buildIndex;

        if(sceneIndexNumber ==0)
        {
            CheckJsonFile();//최초실행시 json유무 확인
            DontDestroyOnLoad(inst);
            Debug.Log(savedata.RelicLv.Length);
        }

        setScreen();
    }




    [SerializeField] public bool saveAble;
    // 게임 종료시 자동저장
    void OnApplicationQuit()
    {
        if (saveAble)
        {
            Save_EndGame();
            Debug.Log(path);
            Debug.Log("save됨!");
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


    ///////////////////////////////// 세이브 로드 관련 ///////////////////////////////////////




    /// <summary>
    /// 최초 가입시 생성
    /// </summary>
    public void Save_NewCreateAccount(string inputText)
    {
        savedata.Name = inputText;
        string json = JsonUtility.ToJson(savedata);
        File.WriteAllText(path, json);
    }
    
    /// <summary>
    /// 최초 가입시 생성
    /// </summary>
    public void Save_EndGame()
    {
        string save = GameStatus.inst.Get_SaveData();
        
        File.WriteAllText(path, save);
    }



    /// <summary>
    /// 최초 Json파일 있는지 없는 확인
    /// </summary>
    public void CheckJsonFile()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            savedata = JsonConvert.DeserializeObject<SaveData>(json);
            isHaveJsonFile = true;
        }
        else
        {
            isHaveJsonFile = false;
        }
    }



    public SaveData Get_Savedata() => savedata;

    /// <summary>
    /// 현재 인덱스 넘버
    /// </summary>
    /// <returns></returns>

    public int Get_CurSceneIndexNumber() => sceneIndexNumber;
}
