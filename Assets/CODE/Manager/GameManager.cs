using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    //Ÿ ��ũ��Ʈ���� ��ã�Ƶ��ǰ� ã�Ƽ� ������

    // WorldUI �׷�
    GameObject worldUiRef;
    public GameObject WorldUiRef { get { return worldUiRef; } }

    // FrontUI �׷�
    GameObject frontUiRef;
    public GameObject FrontUiRef { get { return frontUiRef; } }

    // WorldSpace �׷�
    GameObject worldSpaceRef;
    public GameObject WorldSpaceRef { get { return worldSpaceRef; } }

    // UI Canvas �׷�
    GameObject uiCanvasRef;
    public GameObject UiCanvasRef { get { return uiCanvasRef; } }

    // ī�޶� �׷�
    GameObject camsRef;
    public GameObject CamsRef { get { return camsRef; } }

    CinemachineVirtualCamera worldCam;
    CinemachineVirtualCamera miniGameCam;

    // �̴ϰ���
    GameObject miniGameParentRef;
    public GameObject MiniGameParentRef { get { return miniGameParentRef; } }

    GameObject miniGameRef;
    public GameObject MiniGameRef { get { return miniGameRef; } }



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
        camsRef = GameObject.Find("---[Cams]").gameObject;
        worldCam = camsRef.transform.Find("Cam_0").GetComponent<CinemachineVirtualCamera>();
        miniGameCam = camsRef.transform.Find("Minicam").GetComponent<CinemachineVirtualCamera>();
        miniGameParentRef = GameObject.Find("---[MiniGame]").gameObject;
        miniGameRef = miniGameParentRef.transform.Find("MiniGame").gameObject;
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
            miniGameRef.SetActive(true);
            miniGameCam.gameObject.SetActive(true);
            MinigameManager.inst.minigameReset();
            MinigameManager.inst.CuttonFadeOut(); // ���� ���̵� 

            worldCam.gameObject.SetActive(false);
            frontUiRef.SetActive(false);
            uiCanvasRef.SetActive(false);
            worldUiRef.SetActive(false);
        }
        else
        {
            worldCam.gameObject.SetActive(true);
            frontUiRef.SetActive(true);
            uiCanvasRef.SetActive(true);
            worldUiRef.SetActive(true);

            miniGameRef.SetActive(false);
            miniGameCam.gameObject.SetActive(false);
        }
    }
}
