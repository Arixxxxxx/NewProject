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
    GameObject worldUiRef, frontUIRef, buffSelectUIWindow, QuestionWindowRef;

    //���� �����׽�Ʈ
    GameObject adSample;
    Button adXbtn;

    //�ؽ�Ʈ�˸�
    Animator textAlrim;
    TMP_Text alrimText;

    //���� ���� �Ҳ��������� �����â -> Reward â ����
    // �̹��� ����
    Image itemicon;
    // ��ư2��
    Button backBtn;
    Button acceptBtn;
    Action questionWindowAction;
    TMP_Text itemInfoText;

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

        //�����û �� ����â
        QuestionWindowRef = frontUIRef.transform.Find("QuestionWindow").gameObject;
        itemicon = QuestionWindowRef.transform.Find("Window/IMG_Frame/IMG").GetComponent<Image>();
        itemInfoText = QuestionWindowRef.transform.Find("Window/ItemText").GetComponent<TMP_Text>();

        backBtn = QuestionWindowRef.transform.Find("Window/Btns/BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(() => QuestionWindowRef.SetActive(false));
        
        acceptBtn = QuestionWindowRef.transform.Find("Window/Btns/AcceptBtn").GetComponent<Button>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    /// <summary>
    /// ���� ���� �� ��������Ʈ ȣ��
    /// </summary>
    /// <param name="funtion"> �����ų �Լ� </param>
    public void SampleAD_Active_Funtion(Action funtion)
    {
        if (AdDelete.inst.IsAdDeleteBuy == false) // ���� ���� �̱��Խ�
        {
            adXbtn.onClick.RemoveAllListeners();
            adXbtn.onClick.AddListener(() =>
            {
                AdAfterInvokeFuntion += funtion;
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
        else if (AdDelete.inst.IsAdDeleteBuy == true) //�����Խ� �ٷιٷ� �ߵ�
        {
            AdAfterInvokeFuntion += funtion;
            AdAfterInvokeFuntion?.Invoke();
            AdAfterInvokeFuntion = null;
        }
    }

    /// <summary>
    ///  ������ ����������� ����� â �ʱ�ȭ
    /// </summary>
    /// <param name="value"> Acitve => true / false </param>
    /// <param name="type">0buff / 1 coin</param>
    /// <param name="index">SpriteResource�� �մ� index</param>
    /// <param name="action"> ���� �� ����� �Լ� </param>
    public void ActiveQuestionWindow(bool value, int type, int index, string ItemInfo, Action action)
    {
        if (value)
        {
            //�̹��� �ʱ�ȭ
            switch (type)
            {
                case 0:
                    itemicon.sprite = SpriteResource.inst.BuffIMG(index);
                    break;

                case 1:
                    itemicon.sprite = SpriteResource.inst.CoinIMG(index);
                    break;
            }
            //�ؽ�Ʈ �ʱ�ȭ
            itemInfoText.text = ItemInfo;

            //���� ��ư �ʱ�ȭ
            acceptBtn.onClick.RemoveAllListeners();
            acceptBtn.onClick.AddListener(() =>
            {
                questionWindowAction = null;
                questionWindowAction += action;
                questionWindowAction?.Invoke();
                QuestionWindowRef.SetActive(false);
            });

            QuestionWindowRef.SetActive(true);
        }
        else
        {
            QuestionWindowRef.SetActive(false);
        }
    }
}
