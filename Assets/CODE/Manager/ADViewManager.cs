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

    //샘플 광고테스트
    GameObject adSample;
    Button adXbtn;

    //텍스트알림
    Animator textAlrim;
    TMP_Text alrimText;

    //광고 보고 할껀지말껀지 물어보는창 -> Reward 창 연결
    // 이미지 상자
    Image itemicon;
    // 버튼2개
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

        //샘플광고
        adSample = frontUIRef.transform.Find("SampleAD").gameObject;
        adXbtn = adSample.transform.Find("X").GetComponent<Button>();

        //텍스트 알림
        textAlrim = worldUiRef.transform.Find("TextAlrim").GetComponent<Animator>();
        alrimText = textAlrim.GetComponentInChildren<TMP_Text>();

        //광고시청 전 질문창
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
    /// 광고보고 루비 혹은 돈
    /// </summary>
    /// <param name="itemType"> 0 루비 / 1 골드</param>
    /// <param name="value"> 값 </param>
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
    /// 샘플 광고보고 피버타임 
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
    /// 텍스트 알림창 호출
    /// </summary>
    /// <param name="data">알림창에 띄울 메세지</param>
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
    /// 광고 실행 후 델리게이트 호출
    /// </summary>
    /// <param name="funtion"> 실행시킬 함수 </param>
    public void SampleAD_Active_Funtion(Action funtion)
    {
        if (AdDelete.inst.IsAdDeleteBuy == false) // 광고 삭제 미구입시
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
        else if (AdDelete.inst.IsAdDeleteBuy == true) //광고구입시 바로바로 발동
        {
            AdAfterInvokeFuntion += funtion;
            AdAfterInvokeFuntion?.Invoke();
            AdAfterInvokeFuntion = null;
        }
    }

    /// <summary>
    ///  광고보고 보상받을껀지 물어보는 창 초기화
    /// </summary>
    /// <param name="value"> Acitve => true / false </param>
    /// <param name="type">0buff / 1 coin</param>
    /// <param name="index">SpriteResource에 잇는 index</param>
    /// <param name="action"> 수락 후 실행될 함수 </param>
    public void ActiveQuestionWindow(bool value, int type, int index, string ItemInfo, Action action)
    {
        if (value)
        {
            //이미지 초기화
            switch (type)
            {
                case 0:
                    itemicon.sprite = SpriteResource.inst.BuffIMG(index);
                    break;

                case 1:
                    itemicon.sprite = SpriteResource.inst.CoinIMG(index);
                    break;
            }
            //텍스트 초기화
            itemInfoText.text = ItemInfo;

            //수락 버튼 초기화
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
