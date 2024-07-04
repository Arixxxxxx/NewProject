using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds;
using GoogleMobileAds.Api;


public class ADViewManager : MonoBehaviour
{
    public static ADViewManager inst;
    public static Action AdAfterInvokeFuntion;

    //Ref
    GameObject worldUiRef, frontUIRef, buffSelectUIWindow, QuestionWindowRef;

    //샘플 광고테스트
    GameObject adSample;
    Button adXbtn;

    //광고 보고 할껀지말껀지 물어보는창 -> Reward 창 연결
    // 이미지 상자
    Image itemicon;
    // 버튼2개
    Button backBtn;
    Button acceptBtn;
    Action questionWindowAction;
    TMP_Text itemInfoText;


    // Admob 리워드형 ID 
    //TestID : ca-app-pub-3940256099942544/5224354917
    //빌드용ID : ca-app-pub-2830745914392195/6310443548

    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";  //TestID



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

//#if UNITY_EDITOR
//        _adUnitId = "ca-app-pub-3940256099942544/5224354917";  /*Test ID*/
//#endif

        worldUiRef = GameManager.inst.WorldUiRef;
        frontUIRef = GameManager.inst.FrontUiRef;

        buffSelectUIWindow = frontUIRef.transform.Find("Buff_Window").gameObject;

        //샘플광고
        adSample = frontUIRef.transform.Find("SampleAD").gameObject;
        adXbtn = adSample.transform.Find("X").GetComponent<Button>();


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
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) => { });
        LoadRewardedAd();
    }

    // 광고 재생후 이벤트 호출
    public void AdMob_ActiveAndFuntion(Action funtion)
    {
        if (AdDelete.inst.IsAdDeleteBuy == false) // 광고 삭제 미구입시
        {
            if(_rewardedAd == null)
            {
                LoadRewardedAd();
            }

            AdAfterInvokeFuntion = null;
            AdAfterInvokeFuntion += funtion;
            ShowRewardedAd();

        }
        else if (AdDelete.inst.IsAdDeleteBuy == true) //광고구입시 바로바로 발동
        {
            AdAfterInvokeFuntion = null;
            AdAfterInvokeFuntion += funtion;
            AdAfterInvokeFuntion?.Invoke();
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






    private RewardedAd _rewardedAd;

    // 광고 로드
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

                    
                    // 빌드할때만 주석 풀것! 광고실패시 일단 테스트ID로 재생시킴
                    StartCoroutine(RetryLoadAd());
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;

                //핸들러 등록
                RegisterEventHandlers(_rewardedAd);

                // 광고형 아이디면 다시 빌드광고단위로변경 // 빌드할때만 주석 풀것!

                //if(_adUnitId == "ca-app-pub-3940256099942544/5224354917")
                //{
                //    _adUnitId = "ca-app-pub-2830745914392195/6310443548";
                //}
            });
    }

    // 광고 로드 재시도 코루틴 
    
    WaitForSeconds reloadWaitTime = new WaitForSeconds(0.5f);
    int reloadCount = 0;
    private IEnumerator RetryLoadAd()     //광고 인벤토리 부족시 3회 요청후 그래도 no fill 이 return되면 테스트 id로 일단 출력해줌
    {
        reloadCount++;
        yield return reloadWaitTime;

        if(reloadCount == 3)
        {
            _adUnitId = "ca-app-pub-3940256099942544/5224354917";
            reloadCount = 0;
        }
        
        LoadRewardedAd();
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
    //이벤트
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // 광고로 수익이 발생한 것으로 추정되는 경우
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // 광고에 대한 노출이 기록되면 발생
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        //광고 클릭이 기록되면 발생
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        //  광고가 전체 화면 콘텐츠를 열 때 발생.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("광고 시작");
        };
        // 닫혔을때
        ad.OnAdFullScreenContentClosed += () =>
        {
            StartCoroutine(PlayFuntion());
        };
        // 실패
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            //LoadRewardedAd();
        };
    }







    // 개발당시 샘플버전

    /// <summary>
    /// 광고 실행 후 델리게이트 호출
    /// </summary>
    /// <param name="funtion"> 실행시킬 함수 </param>
    //public void SampleAD_Active_Funtion(Action funtion)
    //{
    //    if (AdDelete.inst.IsAdDeleteBuy == false) // 광고 삭제 미구입시
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
    //    else if (AdDelete.inst.IsAdDeleteBuy == true) //광고구입시 바로바로 발동
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
