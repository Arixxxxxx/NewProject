using System.Collections;
using System.Collections.Generic;
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
        
        // ���� ���� �ð� �־��� (���߿� �����͸Ŵ��� �ű������Ͻó־������� �Űܾ���)
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
    /// ���� Ȱ��ȭ �Լ� => �Ű����� ( �����ε�����ȣ , �ð�(��))
    /// </summary>
    /// <param name="Num">�ε�����ȣ</param>
    /// <param name="Time">�ð�(��)</param>
    public void ActiveBuff(int Num, float Time)
    {
        buffTimer[Num] += Time * 60;

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
    /// �����ð��� ���� �ð� [�� (min) ] ���� ����
    /// </summary>
    /// <param name="type"> 0����/1�̼�/2���/3�������/4�������</param>
    /// <returns></returns>
    public int GetBuffTime(int type)
    {
        return (int)buffTimer[type] / 60;
    }

}
