using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListWindow : MonoBehaviour
{
    public static QuestListWindow inst;

    [SerializeField] Sprite[] topButtonIMG;

    GameObject worldUI;
    GameObject mainWindow;
    GameObject window;
    GameObject topQuestInfo;
    [SerializeField] GameObject[] scrollViewr = new GameObject[3];


   
    Button mainWindowCloseBtn;

    // 데일리/위클리/퀘스트
    [SerializeField] GameObject topBtnTrsRef;
    [SerializeField] Image[] topBtnIMG;
    [SerializeField] Button[] topBtn;

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

        //오브젝트 참조
        worldUI = GameObject.Find("---[FrontUICanvas]").gameObject;
        //mainWindow = worldUI.transform.Find("Active_WindowUI").gameObject;//겸희가 가져감
        //window = mainWindow.transform.Find("Mission/Window").gameObject;
        topQuestInfo = window.transform.Find("TopBar_Mission").gameObject;
        scrollViewr[0] = window.transform.Find("Daily(Scroll View)").gameObject;
        scrollViewr[1] = window.transform.Find("Weekly(Scroll View)").gameObject;
        scrollViewr[2] = window.transform.Find("Special(Scroll View)").gameObject;

        // 데일리 위클리 퀘스트 버튼 초기화
        topBtnTrsRef = window.transform.Find("Top_Btn").gameObject;

        topBtnIMG = topBtnTrsRef.GetComponentsInChildren<Image>();
        topBtn = topBtnTrsRef.GetComponentsInChildren<Button>();

        // 창 X버튼 
        mainWindowCloseBtn = window.transform.Find("Title/X_Btn").GetComponent<Button>();


        Btn_Init();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //겸희가 가져감
    //public void F_QuestList_ActiveWindow(int indexNum)
    //{
    //    if(mainWindow.activeSelf) { return; }
    //    ViewScroolBarSetActive(indexNum);
    //    mainWindow.gameObject.SetActive(true);
    //}
   

    private void Btn_Init()
    {
        // 1.상단 데일리 위클리 스페셜 버튼
        topBtn[0].onClick.AddListener(() => { ViewScroolBarSetActive(0); });
        topBtn[1].onClick.AddListener(() => { ViewScroolBarSetActive(1); });
        topBtn[2].onClick.AddListener(() => { ViewScroolBarSetActive(2); });

        // 2. 종료 버튼 (X)
        //mainWindowCloseBtn.onClick.AddListener(() => { if (mainWindow.activeSelf) { mainWindow.SetActive(false); } }); 
    }


    // 스크롤바 OnOff 함수
    private void ViewScroolBarSetActive(int value)
    {
        if (scrollViewr[value].activeSelf) { return; }

        TopBtnImageChanger(value);

        for (int count = 0; count < scrollViewr.Length; count++)
        {
            if (value == count)
            {
                scrollViewr[count].SetActive(true);
            }
            else
            {
                scrollViewr[count].SetActive(false);
            }
        }
    }

    // 버튼 이미지 변경 함수
    private void TopBtnImageChanger(int value)
    {
        int spriteIndexNum = value == 0 ? 1 : value == 1 ? 3 : value == 2 ? 5 : 0; // 스프라이트 활성화 저장해놓은순서

        int spriteNum = 0;
        for(int index = 0; index < topBtnIMG.Length-1; index++) // 일단 전부 비활성화 아이콘으로 변경
        {
            topBtnIMG[index].sprite = topButtonIMG[spriteNum];
            spriteNum+=2;
        }

        topBtnIMG[value].sprite = topButtonIMG[spriteIndexNum]; // 활성화된 아이콘만 넣어줌

    }
}
