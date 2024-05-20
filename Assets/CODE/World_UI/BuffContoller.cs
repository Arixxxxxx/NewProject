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
    Image[] buffIconBg;

    [SerializeField]
    [Tooltip("0 공격력, 1 이속, 2골드, 3강한공격력, 4뉴비")]
    double[] buffTimer;
    public double[] BuffTimer {  get { return buffTimer; } }
    

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
        buffIconBg = new Image[buffChild];


        for (int index = 0; index < buffBtns.Length; index++)
        {
            buffBtns[index] = buffParent.transform.GetChild(index).GetComponent<Button>();
            buffIconBg[index] = buffBtns[index].transform.GetChild(0).GetComponent<Image>();
            buffActive[index] = buffBtns[index].transform.GetChild(1).gameObject;
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
    /// 버프 활성화 함수 => 매개변수 ( 버프 인덱스번호(공,이속,골드,강공,뉴비) , 시간(초))
    /// </summary>
    /// <param name="Num">인덱스번호</param>
    /// <param name="Time">시간(분)</param>
    public void ActiveBuff(int Num, double Time)
    {

        buffTimer[Num] += Time * 60;
        buffIconBg[Num].gameObject.SetActive(false);
        BuffValueActiver(Num, true);

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
            buffIconBg[index].gameObject.SetActive(true);
            BuffValueActiver(index, false);

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
            if (buffIconBg[index].gameObject.activeSelf == true)
            {
                buffIconBg[index].gameObject.SetActive(false);
            }

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
        if (buffTimer[4] > 0)
        {
            buffTimer[4] -= Time.deltaTime;

            if (buffIconBg[4].gameObject.activeSelf == true && buffTimer[4] <= 0)
            {
                buffTimer[4] = 0;
                BuffValueActiver(4, false);
                buffIconBg[4].gameObject.SetActive(false);
            }
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


    /// <summary>
    /// 버프활성화된거에따라서 값을 올려주는 함수
    /// </summary>
    /// <param name="buffIndex"> 0공격력 / 1 이속 / 2골드 / 3 강한공격력 / 4 뉴비버프</param>
    /// <param name="Active"> true / false </param>
    private void BuffValueActiver(int buffIndex, bool Active)
    {
        if (Active == true)
        {
            switch (buffIndex)
            {
                case 0: // 버프창 공격력증가
                    GameStatus.inst.BuffAddATK = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.TotalAtk.ToString(), 2);
                    break;

                case 1: // 이동속도 증가
                    GameStatus.inst.BuffAddSpeed = 0.8f;
                    break;

                case 2: // 골드 획득량증가
                    GameStatus.inst.BuffAddGold = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.GetTotalGold(), 2);
                    break;

                case 3: // 강한 공격력
                    GameStatus.inst.BuffAddAdATK = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.TotalAtk.ToString(), 3);
                    break;

                case 4: //뉴비버프
                    GameStatus.inst.NewbieATKBuffValue = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.TotalAtk.ToString(), 2);
                    GameStatus.inst.NewbieAttackSpeed = 0.2f;
                    GameStatus.inst.NewbieGoldBuffValue = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.GetTotalGold(), 2);
                    GameStatus.inst.NewbieMoveSpeedBuffValue = 0.3f;
                    break;
            }
        }
        else
        {
            switch (buffIndex)
            {
                case 0:
                    GameStatus.inst.BuffAddATK = "0";
                    break;

                case 1:
                    GameStatus.inst.BuffAddSpeed = 0f;
                    break;

                case 2:
                    GameStatus.inst.BuffAddGold = "0";
                    break;

                case 3: // 강한 공격력
                    GameStatus.inst.BuffAddAdATK = "0";
                    break;

                case 4: //뉴비버프
                    GameStatus.inst.NewbieATKBuffValue = "0";
                    GameStatus.inst.NewbieAttackSpeed = 0f;
                    GameStatus.inst.NewbieGoldBuffValue = "0";
                    GameStatus.inst.NewbieMoveSpeedBuffValue = 0f;
                    break;
            }
        }


    }


}
