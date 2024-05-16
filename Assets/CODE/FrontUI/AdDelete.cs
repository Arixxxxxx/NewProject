using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdDelete : MonoBehaviour
{
    public static AdDelete inst;

    //Ref
    GameObject worldUIRef, fontUIRef, AdDeleteUiRef, windowRef;
    Button xBtn;
    DateTime endBuffDate;
    //���Ź�ư
    Button buyBtn;

    // ������, �� ���� ������
    GameObject[] buyBtnBoxText = new GameObject[2];
    TMP_Text[] bottomBoxText = new TMP_Text[2];

    // �������� ī��Ʈ�ٿ�
    double buffTime;
    
    bool isAdDeleteBuy;
    public bool IsAdDeleteBuy
    {
        get { return isAdDeleteBuy; }
        set 
        {
            isAdDeleteBuy = value;
            Set_AdDeleteBought(value);
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

        // �����ư
        worldUIRef = GameManager.inst.WorldUiRef;
        fontUIRef = GameManager.inst.FrontUiRef;

        AdDeleteUiRef = fontUIRef.transform.Find("Ad_Delete").gameObject;
        windowRef = AdDeleteUiRef.transform.Find("Window").gameObject;

        xBtn = windowRef.transform.Find("Title/X_Btn").GetComponent<Button>();

        buyBtn = windowRef.transform.Find("Main/TextBox/BuyBtn").GetComponent<Button>();

        buyBtnBoxText[0] = buyBtn.transform.GetChild(0).gameObject;
        buyBtnBoxText[1] = buyBtn.transform.GetChild(0).gameObject;

        bottomBoxText[0] = windowRef.transform.Find("Main/Bottom_Box/BottomText_True").GetComponent<TMP_Text>();
        bottomBoxText[1] = windowRef.transform.Find("Main/Bottom_Box/BottomText_False").GetComponent<TMP_Text>();
       
        BtnInIt();
    }

    void Start()
    {

    }

    void Update()
    {
        BuffUpdater();
    }

    private void BtnInIt()
    {
        xBtn.onClick.AddListener(() => AdDeleteUiRef.SetActive(false));
        
        //���� ��ư
        buyBtn.onClick.AddListener(() =>
        {
            int Price = RubyPrice.inst.AdDeletePrice; // ��񰡰� �ʱ�ȭ
            RubyPayment.inst.RubyPaymentUiActive(Price, () => Set_AdDeleteBuffTime(DateTime.Now.AddDays(30).ToString("O")));
        });

    }

    public void ActiveAdDeleteWindow()
    { 
        AdDeleteUiRef.SetActive(true);
    }


    /// <summary>
    /// 30�� �ð� �־��ִ� �Լ�
    /// </summary>
    public void Set_AdDeleteBuffTime(string buffEndDateValue)
    {
        if(buffEndDateValue == string.Empty) { return; }

        endBuffDate = DateTime.Parse(buffEndDateValue);
        TimeSpan timeSpan =endBuffDate - DateTime.Now;
        buffTime += timeSpan.TotalMilliseconds;
    }

    //
    private void BuffUpdater()
    {
        if(buffTime ==0) { return; }

        if (buffTime <= 0)
        {
            if (IsAdDeleteBuy == true) // ���������� ��ư ��Ȱ��ȭ
            {
                IsAdDeleteBuy = false;
                buffTime = 0;
            }
        }
        else if (buffTime > 0) // ����ð� ������
        {
            if(IsAdDeleteBuy == false) // ���������� ��ư ��Ȱ��ȭ
            {
                IsAdDeleteBuy = true;
            }
                
            buffTime -= Time.deltaTime;

            int days = (int)buffTime / 86400;
            int hours = ((int)buffTime % 86400) / 3600;
            int minutes = ((int)buffTime % 3600) / 60;
            int seconds = ((int)buffTime % 60);

            string timeText = "���� ���� ���� �ð� : ";

            if (days > 0)
            {
                timeText += $"{days}�� {hours}�ð� {minutes}��";
            }
            else if (hours > 0)
            {
                timeText += $"{hours}�ð� {minutes}��";
            }
            else if (minutes > 0)
            {
                timeText += $"{minutes}��";
            }
            else
            {
                timeText += $"{seconds}��";
            }

            bottomBoxText[0].text = timeText;
        }
    }



    /// <summary>
    /// ���� �ϰ� ���� ������ UI ��Ʈ�� Setter�� ��������
    /// </summary>
    /// <param name="value"></param>
    private void Set_AdDeleteBought(bool value)
    {
        int select = value ? 0 : 1;

            for(int index=0; index < bottomBoxText.Length; index++)
            {
                buyBtnBoxText[index].SetActive(false);
                bottomBoxText[index].gameObject.SetActive(false);

                if(select == index)
                {
                    buyBtnBoxText[index].SetActive(true);
                    bottomBoxText[index].gameObject.SetActive(true);
                }
            }
    }
    
    /// <summary>
    /// Save ��
    /// </summary>
    /// <returns></returns>
    public string Get_AdDeleteBuffTime() => endBuffDate.ToString("O");

}
