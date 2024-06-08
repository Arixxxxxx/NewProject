using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    [SerializeField] public bool TestMode;
    //타 스크립트에서 안찾아도되게 찾아서 나눠줌

    // WorldUI 그룹
    GameObject worldUiRef;
    public GameObject WorldUiRef { get { return worldUiRef; } }

    // FrontUI 그룹
    GameObject frontUiRef;
    public GameObject FrontUiRef { get { return frontUiRef; } }

    // WorldSpace 그룹
    GameObject worldSpaceRef;
    public GameObject WorldSpaceRef { get { return worldSpaceRef; } }

    // UI Canvas 그룹
    GameObject uiCanvasRef;
    public GameObject UiCanvasRef { get { return uiCanvasRef; } }

    // 카메라 그룹
    GameObject camsRef;
    public GameObject CamsRef { get { return camsRef; } }

    CinemachineVirtualCamera worldCam;
    CinemachineVirtualCamera miniGameCam;

    // 미니게임
    GameObject miniGameParentRef;
    public GameObject MiniGameParentRef { get { return miniGameParentRef; } }

    GameObject miniGameRef;
    public GameObject MiniGameRef { get { return miniGameRef; } }

    private bool miniGameMode;
    public bool MiniGameMode => miniGameMode;

    // RawImage 그룹
    GameObject rawImgRef;
    public GameObject RawImgRef { get { return rawImgRef; } }

    public GameObject touchScreenParticle;
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        worldUiRef = GameObject.Find("---[World UI Canvas]").gameObject;
        frontUiRef = GameObject.Find("---[FrontUICanvas]").gameObject;
        uiCanvasRef = GameObject.Find("---[UI Canvas]").gameObject;
        worldSpaceRef = GameObject.Find("---[World Space]").gameObject;
        rawImgRef = GameObject.Find("---[RowImageArea]").gameObject;
        camsRef = GameObject.Find("---[Cams]").gameObject;
        worldCam = camsRef.transform.Find("Cam_0").GetComponent<CinemachineVirtualCamera>();
        miniGameCam = camsRef.transform.Find("Minicam").GetComponent<CinemachineVirtualCamera>();
        miniGameParentRef = GameObject.Find("---[MiniGame]").gameObject;
        miniGameRef = miniGameParentRef.transform.Find("MiniGame").gameObject;
        touchScreenParticle = GameObject.Find("TouchScreenParticle").gameObject;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveMiniGameMode(bool value)
    {
        if (value)
        {
            TouchScrrenParticle_Actve(false);
            AudioManager.inst.PlayBGM(1,0.6f);
            miniGameMode = true;
            AudioManager.inst.noSound = true;
            miniGameRef.SetActive(true);
            miniGameCam.gameObject.SetActive(true);
            MinigameManager.inst.minigameReset();
            MinigameManager.inst.CuttonFadeOut(); // 시작 페이드 

            worldCam.gameObject.SetActive(false);
            frontUiRef.SetActive(false);
            uiCanvasRef.SetActive(false);
            worldUiRef.SetActive(false);
        }
        else
        {
            TouchScrrenParticle_Actve(true);
            AudioManager.inst.PlayBGM(0,0.8f);
            miniGameMode = false;
            AudioManager.inst.noSound = false;
            worldCam.gameObject.SetActive(true);
            frontUiRef.SetActive(true);
            uiCanvasRef.SetActive(true);
            worldUiRef.SetActive(true);

            miniGameRef.SetActive(false);
            miniGameCam.gameObject.SetActive(false);
        }
    }

    public void TouchScrrenParticle_Actve(bool value)
    {
        touchScreenParticle.SetActive(value);
    }

}
