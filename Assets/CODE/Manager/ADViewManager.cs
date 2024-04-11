using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ADViewManager : MonoBehaviour
{
    public static ADViewManager inst;
    public static Action AdAfterInvokeFuntion;

    //Ref
    GameObject worldUiRef, frontUIRef, buffSelectUIWindow;

    //���� �����׽�Ʈ
    GameObject adSample;
    Button adXbtn;

    //�ؽ�Ʈ�˸�
    Animator textAlrim;
    TMP_Text alrimText;



    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        worldUiRef = GameManager.inst.WorldUiRef;
        frontUIRef = GameManager.inst.FrontUiRef;

        buffSelectUIWindow = frontUIRef.transform.Find("Buff_Window").gameObject;

        //���ñ���
        adSample = frontUIRef.transform.Find("SampleAD").gameObject;
        adXbtn = adSample.transform.Find("X").GetComponent<Button>();

        //�ؽ�Ʈ �˸�
        textAlrim = worldUiRef.transform.Find("TextAlrim").GetComponent<Animator>();
        alrimText = textAlrim.GetComponentInChildren<TMP_Text>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ������ ���� Ȱ��ȭ�����ִ� �Լ�
    /// </summary>
    /// <param name="witch"> buff ~~</param>
    /// <param name="value">0 ~ 2 �������� â���ִ� ������ / 3 = Ŭ���ϴ� ȭ����� </param>
    public void SampleADBuff(string witch, int value)
    {

        adXbtn.onClick.RemoveAllListeners();
        adXbtn.onClick.AddListener(() =>
        {
            if (witch == "buff" && value != 3)
            {
                BuffContoller.inst.ActiveBuff(value, BuffManager.inst.AdbuffTime(value)); //����Ȱ��ȭ
                BuffManager.inst.AddBuffCoolTime(value, (int)BuffManager.inst.AdbuffTime(value)); // ��Ÿ�� �ð��߰�
                Set_TextAlrim(BuffManager.inst.MakeAlrimMSG(value, (int)BuffManager.inst.AdbuffTime(value))); // �˸�����

            }
            else if (value == 3) // Ŭ���ϴ� ȭ�� ����
            {
                BuffContoller.inst.ActiveBuff(value, BuffManager.inst.AdbuffTime(value)); //����Ȱ��ȭ
                Set_TextAlrim(BuffManager.inst.MakeAlrimMSG(0, (int)BuffManager.inst.AdbuffTime(value))); // �˸�����
            }
            adXbtn.gameObject.SetActive(false);
            adSample.SetActive(false);
            buffSelectUIWindow.SetActive(false);
        });

        StopCoroutine(PlayAD());
        StartCoroutine(PlayAD());

    }

    /// <summary>
    /// ������ ��� Ȥ�� ��
    /// </summary>
    /// <param name="itemType"> 0 ��� / 1 ���</param>
    /// <param name="value"> �� </param>
    public void SampleAD_Get_Currency(int itemType, int value)
    {
        adXbtn.onClick.RemoveAllListeners();
        adXbtn.onClick.AddListener(() =>
        {
            switch (itemType)
            {
                case 0:
                    GameStatus.inst.Ruby += value;
                    break;

                case 1:
                    GameStatus.inst.PlusGold($"{value}");
                    break;
            }

            adXbtn.gameObject.SetActive(false);
            adSample.SetActive(false);
            buffSelectUIWindow.SetActive(false);
        });

        if (AdDelete.inst.IsAdDeleteBuy == false)
        {
            StopCoroutine(PlayAD());
            StartCoroutine(PlayAD());
        }
    }

    /// <summary>
    /// ���� ������ �ǹ�Ÿ�� 
    /// </summary>
    public void SampleAD_Ad_FeverTIme(int Time, int Type, bool isAd)
    {
        adXbtn.onClick.RemoveAllListeners();
        adXbtn.onClick.AddListener(() =>
        {
            HwanSengSystem.inst.FeverTimeActive(Time, Type, isAd);

            adXbtn.gameObject.SetActive(false);
            adSample.SetActive(false);
            buffSelectUIWindow.SetActive(false);
        });

        if (AdDelete.inst.IsAdDeleteBuy == false)
        {
            StopCoroutine(PlayAD());
            StartCoroutine(PlayAD());
        }

    }
    IEnumerator PlayAD()
    {
        adSample.SetActive(true);
        yield return new WaitForSeconds(3);
        adXbtn.gameObject.SetActive(true);
    }

    Color orijinColor;
    /// <summary>
    /// �ؽ�Ʈ �˸�â ȣ��
    /// </summary>
    /// <param name="data">�˸�â�� ��� �޼���</param>
    public void Set_TextAlrim(string data)
    {
        orijinColor = alrimText.color;

        alrimText.text = data;
        StopCoroutine(TextalrimStart());
        StartCoroutine(TextalrimStart());
    }

    IEnumerator TextalrimStart()
    {
        textAlrim.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        textAlrim.SetTrigger("Off");
        yield return new WaitForSecondsRealtime(1.5f);
        textAlrim.gameObject.SetActive(false);
        alrimText.color = orijinColor;
    }

    public void SampleAD_Active_Funtion()
    {
        adXbtn.onClick.RemoveAllListeners();
        adXbtn.onClick.AddListener(() =>
        {
            AdAfterInvokeFuntion?.Invoke();
            AdAfterInvokeFuntion = null;
            adXbtn.gameObject.SetActive(false);
            adSample.SetActive(false);
        });

        if (AdDelete.inst.IsAdDeleteBuy == false)
        {
            StopCoroutine(PlayAD());
            StartCoroutine(PlayAD());
        }
    }
}
