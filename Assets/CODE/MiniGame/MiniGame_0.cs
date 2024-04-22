using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame_0 : MonoBehaviour
{

    GameObject miniGameRef, game0Ref, mainScrrenRef;
    [SerializeField]
    GameObject[] selectMenu = new GameObject[3];

    private void Awake()
    {
        miniGameRef = GameManager.inst.MiniGameRef;
        game0Ref = miniGameRef.transform.Find("BG/MiniGames/Game_0").gameObject;
        mainScrrenRef = miniGameRef.transform.Find("BG/MiniGames/Game_0/MainScrrenCanvas").gameObject;
        selectMenu[2] = game0Ref.transform.Find("MainScrrenCanvas/BG/Start/Select").gameObject;
        selectMenu[1] = game0Ref.transform.Find("MainScrrenCanvas/BG/Info/Select").gameObject;
        selectMenu[0] = game0Ref.transform.Find("MainScrrenCanvas/BG/Back/Select").gameObject;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameMainScrrenController();
    }

    [SerializeField]
    int mainScrrenSelectIndex = 2;
    private void gameMainScrrenController()
    {
        //¡∂¿€∫Œ

        if(mainScrrenRef.activeSelf && MinigameController.inst.Up && mainScrrenSelectIndex < 2)  
        {
            MinigameController.inst.Up = false;
            mainScrrenSelectIndex++;
            SelectOptionInit();
        }
        else if(mainScrrenRef.activeSelf && MinigameController.inst.Down && mainScrrenSelectIndex > 0)
        {
            MinigameController.inst.Down = false;
            mainScrrenSelectIndex--;
            SelectOptionInit();
        }

    }

    /// <summary>
    ///  
    /// </summary>
    public void SelectOptionInit()
    {
        for(int i = 0; i < selectMenu.Length; i++)
        {
            if(i == mainScrrenSelectIndex)
            {
                selectMenu[i].gameObject.SetActive(true);
            }
            else
            {
                selectMenu[i].gameObject.SetActive(false);
            }

        }
    }
}
