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

        Screen.SetResolution(Screen.width, Screen.width / 9 * 16, true);
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
