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

    // ���ϸ�/��Ŭ��/����Ʈ
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

        //������Ʈ ����
        worldUI = GameObject.Find("---[FrontUICanvas]").gameObject;
        //mainWindow = worldUI.transform.Find("Active_WindowUI").gameObject;//���� ������
        //window = mainWindow.transform.Find("Mission/Window").gameObject;
        topQuestInfo = window.transform.Find("TopBar_Mission").gameObject;
        scrollViewr[0] = window.transform.Find("Daily(Scroll View)").gameObject;
        scrollViewr[1] = window.transform.Find("Weekly(Scroll View)").gameObject;
        scrollViewr[2] = window.transform.Find("Special(Scroll View)").gameObject;

        // ���ϸ� ��Ŭ�� ����Ʈ ��ư �ʱ�ȭ
        topBtnTrsRef = window.transform.Find("Top_Btn").gameObject;

        topBtnIMG = topBtnTrsRef.GetComponentsInChildren<Image>();
        topBtn = topBtnTrsRef.GetComponentsInChildren<Button>();

        // â X��ư 
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

    //���� ������
    //public void F_QuestList_ActiveWindow(int indexNum)
    //{
    //    if(mainWindow.activeSelf) { return; }
    //    ViewScroolBarSetActive(indexNum);
    //    mainWindow.gameObject.SetActive(true);
    //}
   

    private void Btn_Init()
    {
        // 1.��� ���ϸ� ��Ŭ�� ����� ��ư
        topBtn[0].onClick.AddListener(() => { ViewScroolBarSetActive(0); });
        topBtn[1].onClick.AddListener(() => { ViewScroolBarSetActive(1); });
        topBtn[2].onClick.AddListener(() => { ViewScroolBarSetActive(2); });

        // 2. ���� ��ư (X)
        //mainWindowCloseBtn.onClick.AddListener(() => { if (mainWindow.activeSelf) { mainWindow.SetActive(false); } }); 
    }


    // ��ũ�ѹ� OnOff �Լ�
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

    // ��ư �̹��� ���� �Լ�
    private void TopBtnImageChanger(int value)
    {
        int spriteIndexNum = value == 0 ? 1 : value == 1 ? 3 : value == 2 ? 5 : 0; // ��������Ʈ Ȱ��ȭ �����س�������

        int spriteNum = 0;
        for(int index = 0; index < topBtnIMG.Length-1; index++) // �ϴ� ���� ��Ȱ��ȭ ���������� ����
        {
            topBtnIMG[index].sprite = topButtonIMG[spriteNum];
            spriteNum+=2;
        }

        topBtnIMG[value].sprite = topButtonIMG[spriteIndexNum]; // Ȱ��ȭ�� �����ܸ� �־���

    }
}
