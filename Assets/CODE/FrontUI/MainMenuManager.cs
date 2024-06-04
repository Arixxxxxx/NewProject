using System.Collections;
using System.Collections.Generic;
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

    /// Mute 변수 세터

    [SerializeField]
    bool bgmMute, sfxMute;
    public bool SfxMute
    {
        get  { return sfxMute; }

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

        BgmMute = true;
        SfxMute = true;

        xBtn.onClick.AddListener(() => Active_MainMenu(false));
    }

    void Start()
    {
        btn_init();
    }

    private void btn_init()
    {
        bgmMuteBtn[0].onClick.AddListener(() =>
        {
            if (BgmMute) { return; }
            MuteBtn_Init("BGM", true);
        });

        bgmMuteBtn[1].onClick.AddListener(() =>
        {
            if (!BgmMute) { return; }
            MuteBtn_Init("BGM", false);
        });

        sfxMuteBtn[0].onClick.AddListener(() =>
        {
            if (SfxMute) { return; }
            MuteBtn_Init("Sfx", true);
        });

        sfxMuteBtn[1].onClick.AddListener(() =>
        {
            if (!SfxMute) { return; }
            MuteBtn_Init("Sfx", false);
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
        if (BgmMute)
        {
            bgmMuteCheckIMG[0].SetActive(true);
            bgmMuteCheckIMG[1].SetActive(false);

        }
        else
        {
            bgmMuteCheckIMG[0].SetActive(false);
            bgmMuteCheckIMG[1].SetActive(true);
        }

        if (SfxMute)
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

                if (BgmMute)
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

                if (SfxMute)
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
