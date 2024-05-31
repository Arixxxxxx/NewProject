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

    // ���ε� ����

    RectTransform ScreenArea;

    public SaveData savedata = new SaveData();




    

    bool isHaveJsonFile = false;
    public bool IshaveJsonFile { get { return isHaveJsonFile; } }


    [System.Serializable]
    public class SaveData
    {
        public string LastSignDate;

        public string Name;

        // 1. ��ȭ
        public string Gold = "0";
        public string Star = "0";
        public int Ruby;

        // 2. ���� ��ȭ ���
        public int[] CrewUpgradeMaterial = new int[3];

        // 3. �̴ϰ���
        public bool adRulletPlay;
        public bool adSlotMachinePlay;
        public int miniTicket;

        // 3. ���� ���� �ð�
        public int buffAtkTime;
        public int buffGoldTime;
        public int buffMoveSpeedTime;
        public int buffBigAtkTime;

        // 4. ���� ����
        public int getNewbieRewardCount;
        public bool todayGetRaward;
        public string newbieBuffLastDay;
        
        // 5. �⼮üũ
        public int GetGiftCount;
        public bool todayGetDailyReward;
        public bool DailyADRuby;

        // 6. ĳ���� ����
        public int HwanSeangCount;

        // 7. ������Ȳ (��������)
        public int TotalFloor = 1;
        public int Stage = 1;
        public int NowFloor = 1;

        // 8. ���� ����
        public int Crew0Lv = 0;
        public int Crew1Lv = 0;
        public int Crew2Lv = 0;

        // 9. ���� �ϴ� UI ��Ȳ
        public List<int> QuestLv = new List<int>(100);
        public List<int> WeaponLv = new List<int>(100);
        public List<int> RelicLv;        
        public int NowEquipWeaponNum;

        // 10. �̼� ��Ȳ
        public List<bool> DailyMIssionClear = new List<bool>(10);
        public List<bool> WeeklyMissionClear = new List<bool>(10);
        public int SpecialMissionClearNum;
        public List<int> DailyMissionCount = new List<int>(10);
        public List<int> WeeklyMissionCount = new List<int>(10);
        public List<int> SpecialMissionCount = new List<int>(100);

        //11. ���� ��Ȳ
        public List<bool> BingoBoard = new List<bool>();
        public int RouletteTicket;
        public int BingoStack;

        //12 ����
        public List<SaveLetter> LetterBox = new List<SaveLetter>();

        // ��������
        public string adDeleteBuffTime = string.Empty;

        // ����
        public int[] monsterDogamList;

        // ����
        //�Ϸ翡 �ѹ� �̱� ���,����
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
            CheckJsonFile();//���ʽ���� json���� Ȯ��
            DontDestroyOnLoad(inst);
        }

        setScreen();

        // 1���� �ε��Ϸ�� FakeUI �۵�����
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
    // ���� ����� �ڵ�����
    void OnApplicationQuit()
    {
        if (saveAble)
        {
            Save_EndGame();
            Debug.Log($"{path} ��η� Save");
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) // ���ø����̼��� �Ͻ� ������ ��
        {
            if (saveAble)
            {
                Save_EndGame();
                Debug.Log($"{path} ��η� Save");
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


    ///////////////////////////////// ���̺� �ε� ���� ///////////////////////////////////////




    /// <summary>
    /// ���� ���Խ� ����
    /// </summary>
    public void Save_NewCreateAccount(string inputText)
    {
        savedata.Name = inputText;
        string json = JsonUtility.ToJson(savedata,true);
        File.WriteAllText(path, json);
    }
    
    /// <summary>
    /// ���� ���Խ� ����
    /// </summary>
    public void Save_EndGame()
    {
        string save = GameStatus.inst.Get_SaveData();
        
        File.WriteAllText(path, save);
    }



    /// <summary>
    /// ���� Json���� �ִ��� ���� Ȯ��
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
    /// ���� �ε��� �ѹ�
    /// </summary>
    /// <returns></returns>

    public int Get_CurSceneIndexNumber() => sceneIndexNumber;
}
