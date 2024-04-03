using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionData : MonoBehaviour
{
    public static MissionData Instance;

    int[] QuestLv;//각 퀘스트별 레벨

    GameObject frontUICanvas;

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

    }

    void Update()
    {
        
    }

    public void SetQuestLv(int index, int lv)
    {
        QuestLv[index] = lv;

    }
}
