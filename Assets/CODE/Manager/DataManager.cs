using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    public static DataManager inst;

    public SaveData savedata = new SaveData();
    string path = Path.Combine(Application.dataPath, "TestData.json");
    RectTransform ScreenArea;

    public class SaveData
    {
        public int test1;
        public int[] testAry1;
        public Vector2 testVector2;
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

        DontDestroyOnLoad(inst);
        setScreen();
        //Screen.SetResolution(Screen.width, Screen.width / 9 * 16, true);
    }

    void setScreen()
    {
        ScreenArea = GameObject.Find("---[UI Canvas]").transform.Find("ScreenArea").GetComponent<RectTransform>();

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



        //Vector2 minAnchor = Screen.safeArea.min;
        //Vector2 maxAnchor = Screen.safeArea.max;



        //minAnchor.x /= setWidth;
        //minAnchor.y /= setheight;

        //maxAnchor.x /= setWidth;
        //maxAnchor.y /= setheight;

        //ScreenArea.anchorMin = minAnchor;
        //ScreenArea.anchorMax = maxAnchor;

    }

    public void SavePath()
    {
        savedata.test1 = 1;
        savedata.testAry1 = new int[3] { 1, 2, 3 };
        savedata.testVector2 = new Vector2(4, 4);

        string json = JsonUtility.ToJson(savedata);

        Debug.Log(path);
        File.WriteAllText(path, json);
    }

    public void LoadPath()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData savedata = JsonConvert.DeserializeObject<SaveData>(json);
            Debug.Log(savedata.testVector2);
        }
    }
}
