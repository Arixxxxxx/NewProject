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

    //샘플 광고테스트
    GameObject adSample;
    Button adXbtn;

    //텍스트알림
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

        //샘플광고
        adSample = frontUIRef.transform.Find("SampleAD").gameObject;
        adXbtn = adSample.transform.Find("X").GetComponent<Button>();

        //텍스트 알림
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
    /// 광고보고 버프 활성화시켜주는 함수
    /// </summary>
    /// <param name="witch"> buff ~~</param>
    /// <param name="value">0 ~ 2 버프선택 창에있는 버프들 / 3 = 클릭하는 화면버프 </param>
    public void SampleADBuff(string witch, int value)
    {

        adXbtn.onClick.RemoveAllListeners();
        adXbtn.onClick.AddListener(() =>
        {
            if (witch == "buff" && value != 3)
            {
                BuffContoller.inst.ActiveBuff(value, BuffManager.inst.AdbuffTime(value)); //버프활성화
                BuffManager.inst.AddBuffCoolTime(value, (int)BuffManager.inst.AdbuffTime(value)); // 쿨타임 시간추가
                Set_TextAlrim(BuffManager.inst.MakeAlrimMSG(value, (int)BuffManager.inst.AdbuffTime(value))); // 알림띄우기

            }
            else if (value == 3) // 클릭하는 화면 버프
            {
                BuffContoller.inst.ActiveBuff(value, BuffManager.inst.AdbuffTime(value)); //버프활성화
                Set_TextAlrim(BuffManager.inst.MakeAlrimMSG(0, (int)BuffManager.inst.AdbuffTime(value))); // 알림띄우기
            }
            adXbtn.gameObject.SetActive(false);
            adSample.SetActive(false);
            buffSelectUIWindow.SetActive(false);
        });

        StopCoroutine(PlayAD());
        StartCoroutine(PlayAD());

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
