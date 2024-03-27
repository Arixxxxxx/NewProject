using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public static BuffManager inst;

    GameObject worldUI;
    GameObject buffParent;
    
    Button[] buffBtns;
    GameObject[] buffActive;
    TMP_Text[] buffTime;
    [SerializeField]
    float[] buffTimer;
    

    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(inst);
        }

        worldUI = GameObject.Find("---[World UI Canvas]").gameObject;
        buffParent = worldUI.transform.Find("StageUI/Buff").gameObject;
        
        int buffChild = buffParent.transform.childCount;
        buffBtns = new Button[buffChild];
        buffActive = new GameObject[buffChild];
        buffTime = new TMP_Text[buffChild];
        buffTimer = new float[buffChild];

        for (int index=0; index<buffBtns.Length; index++)
        {
            buffBtns[index] = buffParent.transform.GetChild(index).GetComponent<Button>();
            buffActive[index] = buffBtns[index].transform.GetChild(0).gameObject;
            buffTime[index] = buffActive[index].GetComponentInChildren<TMP_Text>();
        }

        //��ư �ʱ�ȭ

        // Ȱ��ȭ
    }
    private void Start()
    {
        
    }


    private void Update()
    {
        BuffTimeCheck();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActiveBuff(0, 60);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ActiveBuff(1, 60);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ActiveBuff(2, 60);
        }
    }

    /// <summary>
    /// ���� Ȱ��ȭ �Լ� => �Ű����� ( �����ε�����ȣ , �ð�(��))
    /// </summary>
    /// <param name="Num">�ε�����ȣ</param>
    /// <param name="Time">�ð�(��)</param>
    public void ActiveBuff(int Num, float Time)
    {
        buffTimer[Num] += Time;

        if(buffActive[Num].activeSelf == false)
        {
            buffActive[Num].SetActive(true);
        }
        
    }


    /// <summary>
    /// ���� �����
    /// </summary>
    private void BuffTimeCheck()
    {
        // ���� 1��
        if(buffTimer[0] <= 0 && buffActive[0].activeSelf)
        {
            buffTimer[0] = 0;
            buffActive[0].gameObject.SetActive(false);
        }
        else if (buffTimer[0] > 0 && buffActive[0].activeSelf)
        {
            buffTimer[0] -= Time.deltaTime;
            
            int timeValue = 0;
            string  stringValue = string.Empty;

            if (buffTimer[0] > 3600)
            {
                timeValue = (int)buffTimer[0] / 3600;
                stringValue = "H";
            }
            else if (buffTimer[0] > 60 && buffTimer[0] < 3600) 
            {
                timeValue = (int)buffTimer[0] / 60;
                stringValue = "M";
            }
            else if(buffTimer[0] > 0 && buffTimer[0] < 60)
            {
                timeValue = (int)buffTimer[0];
                stringValue = "S";
            }
            buffTime[0].text = timeValue.ToString() + stringValue;
        }

        // ���� 2��
        if (buffTimer[1] <= 0 && buffActive[1].activeSelf)
        {
            buffTimer[1] = 0;
            buffActive[1].gameObject.SetActive(false);
        }
        else if (buffTimer[1] > 0 && buffActive[1].activeSelf)
        {
            buffTimer[1] -= Time.deltaTime;

            int timeValue = 0;
            string stringValue = string.Empty;

            if (buffTimer[1] > 3600)
            {
                timeValue = (int)buffTimer[1] / 3600;
                stringValue = "H";
            }
            else if (buffTimer[1] > 60 && buffTimer[1] < 3600)
            {
                timeValue = (int)buffTimer[1] / 60;
                stringValue = "M";
            }
            else if (buffTimer[1] > 0 && buffTimer[1] < 60)
            {
                timeValue = (int)buffTimer[1];
                stringValue = "S";
            }
            buffTime[1].text = timeValue.ToString() + stringValue;
        }

        // ���� 3��
        if (buffTimer[2] <= 0 && buffActive[2].activeSelf)
        {
            buffTimer[2] = 0;
            buffActive[2].gameObject.SetActive(false);
        }
        else if (buffTimer[2] > 0 && buffActive[2].activeSelf)
        {
            buffTimer[2] -= Time.deltaTime;

            int timeValue = 0;
            string stringValue = string.Empty;

            if (buffTimer[2] > 3600)
            {
                timeValue = (int)buffTimer[2] / 3600;
                stringValue = "H";
            }
            else if (buffTimer[2] > 60 && buffTimer[2] < 3600)
            {
                timeValue = (int)buffTimer[2] / 60;
                stringValue = "M";
            }
            else if (buffTimer[2] > 0 && buffTimer[2] < 60)
            {
                timeValue = (int)buffTimer[2];
                stringValue = "S";
            }
            buffTime[2].text = timeValue.ToString() + stringValue;
        }

    }

 
     
}
