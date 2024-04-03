using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffContoller : MonoBehaviour
{
    public static BuffContoller inst;

    // 버프 인데스 설명
    // 0 공격력
    // 1 이동속도
    // 2 골드
    // 3 광고 5분 공격력버프
    // 4 뉴비

    GameObject worldUI;
    GameObject buffParent;


    Button[] buffBtns;

    GameObject[] buffActive;
    TMP_Text[] buffTime;
    [SerializeField]
    double[] buffTimer;

    ParticleSystem[] buffIconPs;
    
    GameObject newBieObj;
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
        buffTimer = new double[buffChild];
        buffIconPs = new ParticleSystem[buffChild];



        for (int index = 0; index < buffBtns.Length; index++)
        {
            buffBtns[index] = buffParent.transform.GetChild(index).GetComponent<Button>();
            buffActive[index] = buffBtns[index].transform.GetChild(0).gameObject;
            buffTime[index] = buffActive[index].GetComponentInChildren<TMP_Text>();
            buffIconPs[index] = buffActive[index].GetComponentInChildren<ParticleSystem>();
        }
        newBieObj = buffActive[4].transform.parent.gameObject;
        //buffActive[4] = buffActive[4].transform.parent.gameObject;
    }
    private void Start()
    {
        
        // 뉴비 버프 시간 넣어줌 (나중에 데이터매니저 신규유저일시넣어줌으로 옮겨야함)
        if (GameStatus.inst.IsNewBie && buffTimer[4] == 0)
        {
            ActiveBuff(4, 10080);
        }
    }


    private void Update()
    {
        BuffTimeCheck(0);
        BuffTimeCheck(1);
        BuffTimeCheck(2);
        BuffTimeCheck(3);

        NewBieBuffTimeCheck();
    }

    /// <summary>
    /// 버프 활성화 함수 => 매개변수 ( 버프인덱스번호 , 시간(초))
    /// </summary>
    /// <param name="Num">인덱스번호</param>
    /// <param name="Time">시간(분)</param>
    public void ActiveBuff(int Num, float Time)
    {
        buffTimer[Num] += Time * 60;

        switch (Num)
        {
            case 0:  //공
            case 1:  //이속
            case 2:  //골드

                if (buffActive[Num].activeSelf == false)
                {
                    buffActive[Num].SetActive(true);
                    BuffIconParticleReset();
                }

                break;

            case 3: // 공격력 광고 5분

                if (buffBtns[Num].gameObject.activeSelf == false)
                {
                    buffBtns[Num].gameObject.SetActive(true);
                    BuffIconParticleReset();
                }

                break;

            case 4:
                newBieObj.SetActive(true);
                break;
        }






        //버프활성화되엇다고 알림 
    }


    /// <summary>
    /// 버프 실행기
    /// </summary>
    private void BuffTimeCheck(int index)
    {
        if (buffTimer[index] <= 0 && buffActive[index].activeSelf)
        {
            switch (index)
            {
                case 0: // 공격력 버프
                case 1:  // 이속 버프
                case 2: // 골드 버프
                    buffTimer[index] = 0;
                    buffActive[index].gameObject.SetActive(false);
                    break;

                case 3:  // 이벤트 광고 버프
                    buffBtns[index].gameObject.SetActive(false);
                    
                    break;
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


    //뉴비 전용 타임체커
    private void NewBieBuffTimeCheck()
    {
        if (buffTimer[4] <= 0 && newBieObj.activeSelf == true)
        {
            newBieObj.gameObject.SetActive(false);

        }
        else if (buffTimer[4] > 0 && newBieObj.activeSelf)
        {
            buffTimer[4] -= Time.deltaTime;
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

    /// <summary>
    /// 버프시간의 남은 시간 [분 (min) ] 으로 리턴
    /// </summary>
    /// <param name="type"> 0공격/1이속/2골드/3광고버프/4뉴비버프</param>
    /// <returns></returns>
    public int GetBuffTime(int type)
    {
        return (int)buffTimer[type] / 60;
    }

}
