using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class DataManager : MonoBehaviour
{
    public static DataManager inst;
    Scene currentSceneIndex;
    int sceneIndexNumber;

    // 씬로딩 관련

    RectTransform ScreenArea;

    public SaveData savedata = new SaveData();




    

    bool isHaveJsonFile = false;
    public bool IshaveJsonFile { get { return isHaveJsonFile; } }


    [System.Serializable]
    public class SaveData
    {
        public string LastSignDate;

        public string Name;

        // 1. 재화
        public string Gold = "0";
        public string Star = "0";
        public int Ruby;

        // 2. 동료 강화 재료
        public int[] CrewUpgradeMaterial = new int[3];

        // 3. 미니게임
        public bool adRulletPlay;
        public bool adSlotMachinePlay;
        public int miniTicket;

        // 3. 버프 남은 시간
        public int buffAtkTime;
        public int buffGoldTime;
        public int buffMoveSpeedTime;
        public int buffBigAtkTime;

        // 4. 뉴비 혜택
        public int getNewbieRewardCount;
        public bool todayGetRaward;
        public string newbieBuffLastDay;
        
        // 5. 출석체크
        public int GetGiftCount;
        public bool todayGetDailyReward;
        public bool DailyADRuby;

        // 6. 캐릭터 관련
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
        public List<int> QuestLv = new List<int>(100);
        public List<int> WeaponLv = new List<int>(100);
        public List<int> RelicLv;        
        public int NowEquipWeaponNum;

        // 10. 미션 현황
        public List<bool> DailyMIssionClear = new List<bool>(10);
        public List<bool> WeeklyMissionClear = new List<bool>(10);
        public int SpecialMissionClearNum;
        public List<int> DailyMissionCount = new List<int>(10);
        public List<int> WeeklyMissionCount = new List<int>(10);
        public List<int> SpecialMissionCount = new List<int>(100);

        //11. 빙고 현황
        public List<bool> BingoBoard = new List<bool>();
        public int RouletteTicket;
        public int BingoStack;

        //12 우편
        public List<SaveLetter> LetterBox = new List<SaveLetter>();

        // 광고제거
        public string adDeleteBuffTime = string.Empty;

        // 도감
        public int[] monsterDogamList;

        // 상점
        //하루에 한번 뽑기 재료,유물
        public string[] adViewrGachaDate = new string[2];

    }

    string path = string.Empty;
    private void Awake()
    {

#if UNITY_EDITOR
         path = Path.Combine(Application.dataPath, "Save.json");
#else
          path = Path.Combine(Application.persistentDataPath, "Save.json");
#endif

        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(inst);
        }

        Application.targetFrameRate = 60;
        currentSceneIndex = SceneManager.GetActiveScene();
        sceneIndexNumber = currentSceneIndex.buildIndex;

        if(sceneIndexNumber == 0)
        {
            CheckJsonFile();//최초실행시 json유무 확인
            DontDestroyOnLoad(inst);
        }

        setScreen();

        // 1번씬 로딩완료시 FakeUI 작동예정
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        setScreen();
        if (scene.buildIndex == 1)
        {
            WorldUI_Manager.inst.LoadScene_FakeScreen_Active();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Update()
    {
        AutoSave();
    }

    [SerializeField] public bool saveAble;
    // 게임 종료시 자동저장
    void OnApplicationQuit()
    {
        if (saveAble)
        {
            Save_EndGame();
            Debug.Log($"{path} 경로로 Save");
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) // 어플리케이션이 일시 정지될 때
        {
            if (saveAble)
            {
                Save_EndGame();
                Debug.Log($"{path} 경로로 Save");
            }
        }
    }


    float saveTime = 30f;
    float saveTimer = 0;
    private void AutoSave()
    {
        if(saveAble) 
        {
            saveTimer = Time.deltaTime;
            if(saveTimer > saveTime)
            {
                saveTimer = 0;
                Save_EndGame();
            }
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
        string json = JsonUtility.ToJson(savedata,true);
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
