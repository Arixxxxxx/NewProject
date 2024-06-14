using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class ADViewManager : MonoBehaviour
{
    public static ADViewManager inst;
    public static Action AdAfterInvokeFuntion;

    //Ref
    GameObject worldUiRef, frontUIRef, buffSelectUIWindow, QuestionWindowRef;

    //���� �����׽�Ʈ
    GameObject adSample;
    Button adXbtn;

    //���� ���� �Ҳ��������� �����â -> Reward â ����
    // �̹��� ����
    Image itemicon;
    // ��ư2��
    Button backBtn;
    Button acceptBtn;
    Action questionWindowAction;
    TMP_Text itemInfoText;


    // Admob �������� ID 
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";  /*Test��*/



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
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) => { });
        LoadRewardedAd();
    }

    public void AdMob_ActiveAndFuntion(Action funtion)
    {
        if (AdDelete.inst.IsAdDeleteBuy == false) // ���� ���� �̱��Խ�
        {
            AdAfterInvokeFuntion = null;
            AdAfterInvokeFuntion += funtion;
            ShowRewardedAd();

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






    private RewardedAd _rewardedAd;

    // ���� �ε�
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();
       
        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;

                //�ڵ鷯 ���
                RegisterEventHandlers(_rewardedAd);
            });
    }

    // Show
    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                
            });
        }
    }

    WaitForSeconds adFistDealyTiem = new WaitForSeconds(0.25f);
    WaitForSeconds adDealyTiem = new WaitForSeconds(0.5f);
    IEnumerator PlayFuntion()
    {
        yield return adFistDealyTiem;
        
        AdAfterInvokeFuntion?.Invoke();
        yield return adDealyTiem;
        yield return adDealyTiem;
        LoadRewardedAd();
    }
    //�̺�Ʈ
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // ����� ������ �߻��� ������ �����Ǵ� ���
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // ���� ���� ������ ��ϵǸ� �߻�
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        //���� Ŭ���� ��ϵǸ� �߻�
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        //  ���� ��ü ȭ�� �������� �� �� �߻�.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // ��������
        ad.OnAdFullScreenContentClosed += () =>
        {
            StartCoroutine(PlayFuntion());
        };
        // ����
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            //LoadRewardedAd();
        };
    }







    // ���ߴ�� ���ù���

    /// <summary>
    /// ���� ���� �� ��������Ʈ ȣ��
    /// </summary>
    /// <param name="funtion"> �����ų �Լ� </param>
    //public void SampleAD_Active_Funtion(Action funtion)
    //{
    //    if (AdDelete.inst.IsAdDeleteBuy == false) // ���� ���� �̱��Խ�
    //    {
    //        adXbtn.onClick.RemoveAllListeners();
    //        adXbtn.onClick.AddListener(() =>
    //        {
    //            AdAfterInvokeFuntion += funtion;
    //            AdAfterInvokeFuntion?.Invoke();
    //            AdAfterInvokeFuntion = null;
    //            adXbtn.gameObject.SetActive(false);
    //            adSample.SetActive(false);
    //        });

    //        if (AdDelete.inst.IsAdDeleteBuy == false)
    //        {
    //            StopCoroutine(PlayAD());
    //            StartCoroutine(PlayAD());
    //        }
    //    }
    //    else if (AdDelete.inst.IsAdDeleteBuy == true) //�����Խ� �ٷιٷ� �ߵ�
    //    {
    //        AdAfterInvokeFuntion += funtion;
    //        AdAfterInvokeFuntion?.Invoke();
    //        AdAfterInvokeFuntion = null;
    //    }
    //}

    //IEnumerator PlayAD()
    //{
    //    adSample.SetActive(true);
    //    yield return new WaitForSeconds(3);
    //    adXbtn.gameObject.SetActive(true);
    //}
}
