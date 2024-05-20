using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BuffContoller : MonoBehaviour
{
    public static BuffContoller inst;

    // ���� �ε��� ����
    // 0 ���ݷ�
    // 1 �̵��ӵ�
    // 2 ���
    // 3 ���� 5�� ���ݷ¹���
    // 4 ����

    GameObject worldUI;
    GameObject buffParent;


    Button[] buffBtns;

    GameObject[] buffActive;
    TMP_Text[] buffTime;
    Image[] buffIconBg;

    [SerializeField]
    [Tooltip("0 ���ݷ�, 1 �̼�, 2���, 3���Ѱ��ݷ�, 4����")]
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

        // ���� ���� �ð� �־��� (���߿� �����͸Ŵ��� �ű������Ͻó־������� �Űܾ���)
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
    /// ���� Ȱ��ȭ �Լ� => �Ű����� ( ���� �ε�����ȣ(��,�̼�,���,����,����) , �ð�(��))
    /// </summary>
    /// <param name="Num">�ε�����ȣ</param>
    /// <param name="Time">�ð�(��)</param>
    public void ActiveBuff(int Num, double Time)
    {

        buffTimer[Num] += Time * 60;
        buffIconBg[Num].gameObject.SetActive(false);
        BuffValueActiver(Num, true);

        switch (Num)
        {
            case 0:  //��
            case 1:  //�̼�
            case 2:  //���

                if (buffActive[Num].activeSelf == false)
                {
                    buffActive[Num].SetActive(true);
                    BuffIconParticleReset();
                }

                break;

            case 3: // ���ݷ� ���� 5��

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

        //����Ȱ��ȭ�Ǿ��ٰ� �˸� 
    }


    /// <summary>
    /// ���� �����
    /// </summary>
    private void BuffTimeCheck(int index)
    {
        if (buffTimer[index] <= 0 && buffActive[index].activeSelf)
        {
            buffIconBg[index].gameObject.SetActive(true);
            BuffValueActiver(index, false);

            switch (index)
            {
                case 0: // ���ݷ� ����
                case 1:  // �̼� ����
                case 2: // ��� ����
                    buffTimer[index] = 0;
                    buffActive[index].gameObject.SetActive(false);
                    break;

                case 3:  // �̺�Ʈ ���� ����
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


    //���� ���� Ÿ��üĿ
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
    /// �����ð��� ���� �ð� [�� (min) ] ���� ����
    /// </summary>
    /// <param name="type"> 0����/1�̼�/2���/3�������/4�������</param>
    /// <returns></returns>
    public int GetBuffTime(int type)
    {
        return (int)buffTimer[type] / 60;
    }


    /// <summary>
    /// ����Ȱ��ȭ�Ȱſ����� ���� �÷��ִ� �Լ�
    /// </summary>
    /// <param name="buffIndex"> 0���ݷ� / 1 �̼� / 2��� / 3 ���Ѱ��ݷ� / 4 �������</param>
    /// <param name="Active"> true / false </param>
    private void BuffValueActiver(int buffIndex, bool Active)
    {
        if (Active == true)
        {
            switch (buffIndex)
            {
                case 0: // ����â ���ݷ�����
                    GameStatus.inst.BuffAddATK = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.TotalAtk.ToString(), 2);
                    break;

                case 1: // �̵��ӵ� ����
                    GameStatus.inst.BuffAddSpeed = 0.8f;
                    break;

                case 2: // ��� ȹ�淮����
                    GameStatus.inst.BuffAddGold = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.GetTotalGold(), 2);
                    break;

                case 3: // ���� ���ݷ�
                    GameStatus.inst.BuffAddAdATK = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.TotalAtk.ToString(), 3);
                    break;

                case 4: //�������
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

                case 3: // ���� ���ݷ�
                    GameStatus.inst.BuffAddAdATK = "0";
                    break;

                case 4: //�������
                    GameStatus.inst.NewbieATKBuffValue = "0";
                    GameStatus.inst.NewbieAttackSpeed = 0f;
                    GameStatus.inst.NewbieGoldBuffValue = "0";
                    GameStatus.inst.NewbieMoveSpeedBuffValue = 0f;
                    break;
            }
        }


    }


}
