using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager inst;

    // Ref
    GameObject frontUI, mainMenuRef, boxRef;
    Button xBtn;


    Button[] bgmMuteBtn;
    GameObject[] bgmMuteCheckIMG = new GameObject[2];

    [SerializeField]
    Button[] sfxMuteBtn;
    GameObject[] sfxMuteCheckIMG = new GameObject[2];

    TMP_InputField couponInput;
    /// Mute 변수 세터


    // 게임 종료 버튼
    Button gameExitBtn;

    GameObject gameExitAlrimRef;// 최종창
    Button exitReturnBtn, endGameBtn; // 최종창 버튼

    // 쿠폰관련
    GameObject couponCompleteRef;
    Button couponReturnBtn;
    TMP_Text completeBoxText;


    //슬립모드
    Button sleepModeBtn;

    [SerializeField]
    bool bgmMute, sfxMute;
    public bool SfxMute
    {
        get { return sfxMute; }

        set
        {
            sfxMute = value;
            AudioManager.inst.Set_VoulemMute("SFX", sfxMute);
        }
    }

    public bool BgmMute
    {
        get { return bgmMute; }

        set
        {
            bgmMute = value;
            AudioManager.inst.Set_VoulemMute("BGM", bgmMute);
        }
    }

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

        frontUI = GameManager.inst.FrontUiRef;
        mainMenuRef = frontUI.transform.Find("MainMenu").gameObject;
        boxRef = mainMenuRef.transform.Find("Window/Box").gameObject;


        xBtn = mainMenuRef.transform.Find("Window/Title/X_Btn").GetComponent<Button>();


        bgmMuteBtn = boxRef.transform.Find("Voluem/BGMBtn").GetComponentsInChildren<Button>();
        bgmMuteCheckIMG[0] = bgmMuteBtn[0].transform.Find("Select_IMG").gameObject;
        bgmMuteCheckIMG[1] = bgmMuteBtn[1].transform.Find("Select_IMG").gameObject;


        sfxMuteBtn = boxRef.transform.Find("Voluem/SFXBtn").GetComponentsInChildren<Button>();
        sfxMuteCheckIMG[0] = sfxMuteBtn[0].transform.Find("Select_IMG").gameObject;
        sfxMuteCheckIMG[1] = sfxMuteBtn[1].transform.Find("Select_IMG").gameObject;



        //인풋필드
        couponInput = boxRef.transform.Find("Coupon/InputField (TMP)").GetComponent<TMP_InputField>();


        //게임종료
        gameExitBtn = boxRef.transform.Find("GameExit").GetComponent<Button>();
        gameExitAlrimRef = mainMenuRef.transform.Find("ExitAlrim").gameObject;
        exitReturnBtn = gameExitAlrimRef.transform.Find("Window/NoBtn").GetComponent<Button>();
        endGameBtn = gameExitAlrimRef.transform.Find("Window/YesBtn").GetComponent<Button>();

        //쿠폰
        couponCompleteRef = mainMenuRef.transform.Find("CouponComplete").gameObject;
        couponReturnBtn = couponCompleteRef.transform.Find("Window/YesBtn").GetComponent<Button>();
        completeBoxText = couponCompleteRef.transform.Find("Window/InfoText").GetComponent<TMP_Text>();

        //슬립모드
        sleepModeBtn = boxRef.transform.Find("SleepMode/Bar/SleepModeBtn").GetComponent<Button>();



    }

    void Start()
    {
        btn_init();
    }

    private void btn_init()
    {
        couponReturnBtn.onClick.AddListener(() => couponCompleteRef.SetActive(false));

        xBtn.onClick.AddListener(() => Active_MainMenu(false));

        bgmMuteBtn[0].onClick.AddListener(() =>
        {
            if (!BgmMute) { return; }
            MuteBtn_Init("BGM", false);
        });

        bgmMuteBtn[1].onClick.AddListener(() =>
        {
            if (BgmMute) { return; }
            MuteBtn_Init("BGM", true);
        });

        sfxMuteBtn[0].onClick.AddListener(() =>
        {
            if (!SfxMute) { return; }
            MuteBtn_Init("SFX", false);
        });

        sfxMuteBtn[1].onClick.AddListener(() =>
        {
            if (SfxMute) { return; }
            MuteBtn_Init("SFX", true);
        });


        //인풋필드

        couponInput.onSubmit.AddListener((text) =>
        {
            couponInput.text = string.Empty;
            completeBoxText.text = GameStatus.inst.CheckMyCoupon(text);
            couponCompleteRef.SetActive(true);
        });

        //게임 종료 버튼관련

        gameExitBtn.onClick.AddListener(() => gameExitAlrimRef.SetActive(true));
        exitReturnBtn.onClick.AddListener(() => gameExitAlrimRef.SetActive(false));

        endGameBtn.onClick.AddListener(() =>  //정말 종료
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit(); 
#endif
        } );


        // 슬립모드 ON 버튼
        sleepModeBtn.onClick.AddListener(() =>
        {
            SleepMode.inst.Active_SleepMode(true);
        });
    }


    /// <summary>
    /// 환경설정 켜고 닫기
    /// </summary>
    /// <param name="value"></param>
    public void Active_MainMenu(bool value)
    {
        mainMenuRef.SetActive(value);

        MuteBtn_Init(); // 오디오 뮤트버튼 

    }



    //////////////////////////////////////// 음소거 관련 /////////////////////////////////////////////////


    // 창 열고 닫을때 초기화
    private void MuteBtn_Init()
    {
        if (!BgmMute)
        {
            bgmMuteCheckIMG[0].SetActive(true);
            bgmMuteCheckIMG[1].SetActive(false);

        }
        else
        {
            bgmMuteCheckIMG[0].SetActive(false);
            bgmMuteCheckIMG[1].SetActive(true);
        }

        if (!SfxMute)
        {
            sfxMuteCheckIMG[0].SetActive(true);
            sfxMuteCheckIMG[1].SetActive(false);

        }
        else
        {
            sfxMuteCheckIMG[0].SetActive(false);
            sfxMuteCheckIMG[1].SetActive(true);
        }
    }


    // 버튼용
    private void MuteBtn_Init(string type, bool value)
    {
        switch (type)
        {
            case "BGM":

                BgmMute = value;

                if (!BgmMute)
                {
                    bgmMuteCheckIMG[0].SetActive(true);
                    bgmMuteCheckIMG[1].SetActive(false);

                }
                else
                {
                    bgmMuteCheckIMG[0].SetActive(false);
                    bgmMuteCheckIMG[1].SetActive(true);
                }

                break;

            case "SFX":

                SfxMute = value;

                if (!SfxMute)
                {
                    sfxMuteCheckIMG[0].SetActive(true);
                    sfxMuteCheckIMG[1].SetActive(false);

                }
                else
                {
                    sfxMuteCheckIMG[0].SetActive(false);
                    sfxMuteCheckIMG[1].SetActive(true);
                }

                break;

        }

    }


}
