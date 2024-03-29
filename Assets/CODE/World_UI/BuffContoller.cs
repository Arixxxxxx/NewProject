using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffContoller : MonoBehaviour
{
    public static BuffContoller inst;

    GameObject worldUI;
    GameObject buffParent;


    Button[] buffBtns;
    GameObject[] buffActive;
    TMP_Text[] buffTime;

    float[] buffTimer;

    ParticleSystem[] buffIconPs;

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

        worldUI = GameObject.Find("---[World UI Canvas]").gameObject;
        buffParent = worldUI.transform.Find("StageUI/Buff").gameObject;

        int buffChild = buffParent.transform.childCount;
        buffBtns = new Button[buffChild];
        buffActive = new GameObject[buffChild];
        buffTime = new TMP_Text[buffChild];
        buffTimer = new float[buffChild];
        buffIconPs = new ParticleSystem[buffChild];



        for (int index = 0; index < buffBtns.Length; index++)
        {
            buffBtns[index] = buffParent.transform.GetChild(index).GetComponent<Button>();
            buffActive[index] = buffBtns[index].transform.GetChild(0).gameObject;
            buffTime[index] = buffActive[index].GetComponentInChildren<TMP_Text>();
            buffIconPs[index] = buffActive[index].GetComponentInChildren<ParticleSystem>();
        }


        //버튼 초기화

        // 활성화
    }
    private void Start()
    {

    }


    private void Update()
    {
        BuffTimeCheck(0);
        BuffTimeCheck(1);
        BuffTimeCheck(2);
        BuffTimeCheck(3);
    }

    /// <summary>
    /// 버프 활성화 함수 => 매개변수 ( 버프인덱스번호 , 시간(초))
    /// </summary>
    /// <param name="Num">인덱스번호</param>
    /// <param name="Time">시간(분)</param>
    public void ActiveBuff(int Num, float Time)
    {
        buffTimer[Num] += Time * 60;

        if (Num != 3 && buffActive[Num].activeSelf == false)
        {
            buffActive[Num].SetActive(true);
            BuffIconParticleReset();
        }
        else if(Num == 3 && buffBtns[Num].gameObject.activeSelf == false)
        {
            buffBtns[Num].gameObject.SetActive(true);
            BuffIconParticleReset();
        }


        //버프활성화되엇다고 알림 
    }


    /// <summary>
    /// 버프 실행기
    /// </summary>
    private void BuffTimeCheck(int index)
    {
        // 버프 1번
        if (buffTimer[index] <= 0 && buffActive[index].activeSelf)
        {
            if (index != 3)
            {
                buffTimer[index] = 0;
                buffActive[index].gameObject.SetActive(false);
            }
            else if (index == 3)
            {
                buffBtns[index].gameObject.SetActive(false);
            }
        }
        else if (buffTimer[index] > 0 && buffActive[index].activeSelf)
        {
            buffTimer[index] -= Time.deltaTime;

            int timeValue = 0;
            string stringValue = string.Empty;

            if (buffTimer[index] > 3600)
            {
                timeValue = (int)buffTimer[index] / 3600;
                stringValue = "H";
            }
            else if (buffTimer[index] > 60 && buffTimer[index] < 3600)
            {
                timeValue = (int)buffTimer[index] / 60;
                stringValue = "M";
            }
            else if (buffTimer[index] > 0 && buffTimer[index] < 60)
            {
                timeValue = (int)buffTimer[index];
                stringValue = "S";
            }
            buffTime[index].text = timeValue.ToString() + stringValue;
        }
    }

    public void BuffIconParticleReset()
    {
        for (int index = 0; index < buffIconPs.Length; index++)
        {
            buffIconPs[index].Stop();
            buffIconPs[index].Play();
        }
    }

}
