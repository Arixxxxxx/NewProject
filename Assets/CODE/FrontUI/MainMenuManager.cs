using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager inst;

    // Ref
    GameObject frontUI, mainMenuRef;
    Button xBtn;

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

        frontUI = GameObject.Find("---[FrontUICanvas]").gameObject;
        mainMenuRef = frontUI.transform.Find("MainMenu").gameObject;
        xBtn = mainMenuRef.transform.Find("Window/Title/X_Btn").GetComponent<Button>();
        xBtn.onClick.AddListener(() => Set_MainMenuActive(false));
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 메인메뉴 호출
    /// </summary>
    /// <param name="value"> true / false </param>
    public void Set_MainMenuActive(bool value)
    {
        mainMenuRef.SetActive(value);
    }
}
