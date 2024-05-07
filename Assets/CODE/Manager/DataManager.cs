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

    // ���ε� ����

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

        // 1. ��ȭ
        public string Gold = "0";
        public string Star = "0";
        public int Ruby;

        // 2. ���� ��ȭ ���
        public int Soul;
        public int book;
        public int born;

        // 3. �̴ϰ���
        public int miniTicket;

        // 3. ���� ���� �ð�
        public double buffAtkTime;
        public double buffGoldTime;
        public double buffMoveSpeedTime;
        public double buffBigAtkTime;

        // 4. ���� ����
        public int getNewbieRewardCount;
        public bool todayGetRaward;
        public DateTime newbieBuffLastDay;

        // 5. �⼮üũ
        public int GetGiftCount;
        public bool todayGetDailyReward;

        // 6. ĳ���� ����
        public int AtkSpeedLv;
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
        public int[] QuestLv = new int[30];
        public int[] WeaponLv = new int[30];
        public int[] RelicLv = new int[11];
        public int NowEquipWeaponNum;

        // 10. �̼� ��Ȳ
        public bool[] DailyMIssionClear = new bool[4];
        public bool[] WeeklyMissionClear = new bool[4];
        public int SpecialMissionClearNum;
        public int[] DailyMissionCount = new int[4];
        public int[] WeeklyMissionCount = new int[4];
        public int[] SpecialMissionCount = new int[6];
        public bool canResetDailyMission;
        public bool canResetWeeklyMission;

        //11. ���� ��Ȳ
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
            CheckJsonFile();//���ʽ���� json���� Ȯ��
            DontDestroyOnLoad(inst);
            Debug.Log(savedata.RelicLv.Length);
        }

        setScreen();
    }




    [SerializeField] public bool saveAble;
    // ���� ����� �ڵ�����
    void OnApplicationQuit()
    {
        if (saveAble)
        {
            Save_EndGame();
            Debug.Log(path);
            Debug.Log("save��!");
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
        string json = JsonUtility.ToJson(savedata);
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
