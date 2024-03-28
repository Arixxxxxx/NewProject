using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public static BuffManager inst;

    GameObject mainWindow;
    GameObject buffWindow;
    GameObject buffSelectUIWindow;

    Button exitBtn;
    int btnCount;
    Button[] viewAdBtn;
    // AD ��Ÿ�Ӱ���
    float[] viewAdCoolTimer;
    GameObject[] btnAdActiveIMG;
    TMP_Text[] adCoolTimeText;

    // ��� ����â����
    Button[] useRubyBtn;
    TMP_Text[] rubyPrice;
    //���� �첫�� �����â
    GameObject alrimWindow;
    TMP_Text rubyValueText;
    Button[] alrimYesOrNoBtn = new Button[2];

    int useRutyTemp;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(inst);
        }

        buffWindow = GameObject.Find("---[FrontUICanvas]").gameObject;

        //�⺻ ��������â
        mainWindow = buffWindow.transform.Find("Buff_Window").gameObject;
        buffSelectUIWindow = buffWindow.transform.Find("Buff_Window/Window").gameObject;
        exitBtn = buffSelectUIWindow.transform.Find("ExitBtn").GetComponent<Button>();
        
        btnCount = buffSelectUIWindow.transform.Find("Buff_Layout").childCount;
        viewAdBtn = new Button[btnCount];
        viewAdCoolTimer = new float[btnCount];
        btnAdActiveIMG = new GameObject[btnCount];
        useRubyBtn = new Button[btnCount];
        adCoolTimeText = new TMP_Text[btnCount];
        rubyPrice = new TMP_Text[btnCount];

        //�����â 
        alrimWindow = buffWindow.transform.Find("Buff_Window/Alrim_Window").gameObject;
        rubyValueText = alrimWindow.transform.Find("Title/RubyValue_Text").GetComponent<TMP_Text>();
        alrimYesOrNoBtn[0] = alrimWindow.transform.Find("Title/NoBtn").GetComponent<Button>();
        alrimYesOrNoBtn[1] = alrimWindow.transform.Find("Title/YesBtn").GetComponent<Button>();

        //ATK �ʱ�ȭ
        viewAdBtn[0] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/ChoiseBtn_AD").GetComponent<Button>();
        btnAdActiveIMG[0] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/ChoiseBtn_AD/AD").gameObject;
        adCoolTimeText[0] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/ChoiseBtn_AD/Left_Time").GetComponent<TMP_Text>();
        
        viewAdBtn[1] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/ChoiseBtn_AD").GetComponent<Button>();
        btnAdActiveIMG[1] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/ChoiseBtn_AD/AD").gameObject;
        adCoolTimeText[1] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/ChoiseBtn_AD/Left_Time").GetComponent<TMP_Text>();

        viewAdBtn[2] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/ChoiseBtn_AD").GetComponent<Button>();
        btnAdActiveIMG[2] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/ChoiseBtn_AD/AD").gameObject;
        adCoolTimeText[2] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/ChoiseBtn_AD/Left_Time").GetComponent<TMP_Text>();

        useRubyBtn[0] = buffSelectUIWindow.transform.Find("Buff_Layout/ATK_Up/ChoiseBtn_Ruby").GetComponent<Button>();
        useRubyBtn[1] = buffSelectUIWindow.transform.Find("Buff_Layout/Gold_Up/ChoiseBtn_Ruby").GetComponent<Button>();
        useRubyBtn[2] = buffSelectUIWindow.transform.Find("Buff_Layout/Speed_Up/ChoiseBtn_Ruby").GetComponent<Button>();
    }

    private void Start()
    {
        // UII ��� ����Text �ʱ�ȭ
        useRubyBtn[0].transform.Find("AD/Text").GetComponent<TMP_Text>().text = RubyPrice.inst.Get_buffRubyPrice(0).ToString();
        useRubyBtn[1].transform.Find("AD/Text").GetComponent<TMP_Text>().text = RubyPrice.inst.Get_buffRubyPrice(1).ToString();
        useRubyBtn[2].transform.Find("AD/Text").GetComponent<TMP_Text>().text = RubyPrice.inst.Get_buffRubyPrice(2).ToString();
        
        BtnInIt();
    }
    private void Update()
    {
        CheakCoomTime();
    }
    private void BtnInIt()
    {
        exitBtn.onClick.AddListener(() => { WorldUI_Manager.inst.buffSelectUIWindowAcitve(false); });

        viewAdBtn[0].onClick.AddListener(() => WorldUI_Manager.inst.SampleAD("buff",0));;
        viewAdBtn[1].onClick.AddListener(() => WorldUI_Manager.inst.SampleAD("buff", 1));
        viewAdBtn[2].onClick.AddListener(() => WorldUI_Manager.inst.SampleAD("buff", 2));


        // ���� ���Ź�ư => ���� �����Ͻò��ϱ� â���� ����
        useRubyBtn[0].onClick.AddListener(() => Set_ReallyBuyBuffWindow_Active(true,0) ); 
        useRubyBtn[1].onClick.AddListener(() => Set_ReallyBuyBuffWindow_Active(true,1) ); 
        useRubyBtn[2].onClick.AddListener(() => Set_ReallyBuyBuffWindow_Active(true,2) );

        alrimYesOrNoBtn[0].onClick.AddListener(() => Set_ReallyBuyBuffWindow_Active(false,0));
    }

    
    /// <summary>
    /// ���� �����Ͻǲ��� �����â �ʱ�ȭ �� yes��ư �ʱ�ȭ
    /// </summary>
    /// <param name="value"></param>
    /// <param name="indexNum"></param>
    private void Set_ReallyBuyBuffWindow_Active(bool value, int indexNum)
    {
        if (value)
        {
            // �� â�� ��ȭ ���Ǵ¾� ��� �ʱ�ȭ
            int curRuby = GameStatus.inst.Ruby; //���� ���� ���
            useRutyTemp = RubyPrice.inst.Get_buffRubyPrice(indexNum); // ����� ���
            int leftPrice = curRuby - useRutyTemp; // �ܾ�

            rubyValueText.text = $"{curRuby}\n-{useRutyTemp}\n\n{leftPrice}";

            //�� ��ư ���⼭ �ʱ�ȭ
            alrimYesOrNoBtn[1].onClick.RemoveAllListeners();
            alrimYesOrNoBtn[1].onClick.AddListener(() =>
            {
                GameStatus.inst.Ruby -= useRutyTemp;  //�������
                useRutyTemp = 0;
                
                BuffContoller.inst.ActiveBuff(indexNum,30); // �����ֱ�
                
                alrimWindow.SetActive(false); //â�ݱ�
                mainWindow.SetActive(false);

                WorldUI_Manager.inst.Set_TextAlrim(MakeAlrimMSG(indexNum, 30)); // �˸��� �־��ֱ�
            });
        }

        alrimWindow.SetActive(value);

    }

    public void viewAdCoolTime(int buffIndexNum)
    {
        viewAdCoolTimer[buffIndexNum] += 15 * 60;
    }

    public void CheakCoomTime()
    {
        // ���� 1�� ����
        if (viewAdCoolTimer[0] > 0)
        {
            if (btnAdActiveIMG[0].gameObject.activeSelf == true && adCoolTimeText[0].gameObject.activeSelf == false)
            {
                viewAdBtn[0].interactable = false;
                btnAdActiveIMG[0].gameObject.SetActive(false);
                adCoolTimeText[0].gameObject.SetActive(true);
            }

            viewAdCoolTimer[0] -= Time.deltaTime;
            int hour = (int)viewAdCoolTimer[0] / 60;
            int min = (int)viewAdCoolTimer[0] % 60;
            adCoolTimeText[0].text = $"{hour} : {min}";
        }

        else if(viewAdCoolTimer[0] <= 0)
        {
            if(viewAdCoolTimer[0] != 0)
            {
                viewAdCoolTimer[0] = 0;
            }
            if (btnAdActiveIMG[0].gameObject.activeSelf == false && adCoolTimeText[0].gameObject.activeSelf == true)
            {
                viewAdBtn[0].interactable = true;
                btnAdActiveIMG[0].gameObject.SetActive(true);
                adCoolTimeText[0].gameObject.SetActive(false);
            }
        }


        //���� 2�� ����
        if (viewAdCoolTimer[1] > 0)
        {
            if (btnAdActiveIMG[1].gameObject.activeSelf == true && adCoolTimeText[1].gameObject.activeSelf == false)
            {
                viewAdBtn[1].interactable = false;
                btnAdActiveIMG[1].gameObject.SetActive(false);
                adCoolTimeText[1].gameObject.SetActive(true);
            }

            viewAdCoolTimer[1] -= Time.deltaTime;
            int hour = (int)viewAdCoolTimer[1] / 60;
            int min = (int)viewAdCoolTimer[1] % 60;
            adCoolTimeText[1].text = $"{hour} : {min}";
        }

        else if (viewAdCoolTimer[1] <= 0)
        {
            if (viewAdCoolTimer[1] != 0)
            {
                viewAdCoolTimer[1] = 0;
            }
            if (btnAdActiveIMG[1].gameObject.activeSelf == false && adCoolTimeText[1].gameObject.activeSelf == true)
            {
                viewAdBtn[1].interactable = true;
                btnAdActiveIMG[1].gameObject.SetActive(true);
                adCoolTimeText[1].gameObject.SetActive(false);
            }
        }

        // ���� 3�� ����
        if (viewAdCoolTimer[2] > 0)
        {
            if (btnAdActiveIMG[2].gameObject.activeSelf == true && adCoolTimeText[2].gameObject.activeSelf == false)
            {
                viewAdBtn[2].interactable = false;
                btnAdActiveIMG[2].gameObject.SetActive(false);
                adCoolTimeText[2].gameObject.SetActive(true);
            }

            viewAdCoolTimer[2] -= Time.deltaTime;
            int hour = (int)viewAdCoolTimer[2] / 60;
            int min = (int)viewAdCoolTimer[2] % 60;
            adCoolTimeText[2].text = $"{hour} : {min}";
        }

        else if (viewAdCoolTimer[2] <= 0)
        {
            if (viewAdCoolTimer[2] != 0)
            {
                viewAdCoolTimer[2] = 0;
            }
            if (btnAdActiveIMG[2].gameObject.activeSelf == false && adCoolTimeText[2].gameObject.activeSelf == true)
            {
                viewAdBtn[2].interactable = true;
                btnAdActiveIMG[2].gameObject.SetActive(true);
                adCoolTimeText[2].gameObject.SetActive(false);
            }
        }

    }  

    /// <summary>
    /// ���� ��Ÿ�ӿ� �ð��ֱ�
    /// </summary>
    /// <param name="index">���� �ε��� ��ȣ</param>
    /// <param name="Time">�ð�(��)</param>
    public void AddBuffCoolTime(int index, int Time) => viewAdCoolTimer[index] = Time * 60;
    
    public string MakeAlrimMSG(int indexNum , int Time)
    {
        
        switch (indexNum)
        {
            case 0:
               return $"���ݷ� ������ {Time}�� Ȱ��ȭ �Ǿ����ϴ�.";
               

            case 1:
                return $"���ȹ�� ������ {Time}�� Ȱ��ȭ �Ǿ����ϴ�.";
                

            case 2:
                return $"�̵��ӵ� ������ {Time}�� Ȱ��ȭ �Ǿ����ϴ�.";
     
        }

        return null;
    }
}

