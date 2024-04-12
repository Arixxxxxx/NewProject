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

    //���Ź�ư
    Button buyBtn;
    TMP_Text buyBtnText;
    Image buyBtnImg;

    //�ϴ� �ؽ�Ʈ
    TMP_Text bottomText;

    // �������� ī��Ʈ�ٿ�
    double buffTime;
    
    bool isAdDeleteBuy;
    public bool IsAdDeleteBuy
    {
        get { return isAdDeleteBuy; }
    }

    // ��Ÿ����
    Color greenColor = new Color(0.2f, 1, 1, 1);
    Color redColor = new Color(1, 0.2f, 0, 1);
    string btnTextTemp; // ��ư �ؽ�Ʈ �ӽ������

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
        buyBtnText = buyBtn.transform.GetComponentInChildren<TMP_Text>();
        buyBtnImg = buyBtn.transform.Find("Image").GetComponent<Image>();

        bottomText = windowRef.transform.Find("Main/BottomText").GetComponent<TMP_Text>();
        btnTextTemp = buyBtnText.text;
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
            RubyPayment.inst.RubyPaymentUiActive(Price, AddAdDeleteTime);
        });

    }

    public void ActiveAdDeleteWindow()
    { 
        AdDeleteUiRef.SetActive(true);
    }

    //
    private void BuffUpdater()
    {
        if (buffTime <= 0)
        {
            if (isAdDeleteBuy == true) // ���������� ��ư ��Ȱ��ȭ
            {
                isAdDeleteBuy = false;
                buyBtn.enabled = true;
                buyBtnText.alignment = TextAlignmentOptions.Center;
                buyBtnText.text = btnTextTemp;
                buyBtnImg.enabled = true;
                buffTime = 0;
                bottomText.color = redColor;
                bottomText.text = "���� ���� ������";
            }
        }
        else if (buffTime > 0) // ����ð� ������
        {
            if(isAdDeleteBuy == false) // ���������� ��ư ��Ȱ��ȭ
            {
                isAdDeleteBuy = true;
                buyBtn.enabled = false;
                buyBtnText.alignment = TextAlignmentOptions.Left;
                buyBtnText.text = " ���� �Ϸ�";
                buyBtnImg.enabled = false;
            }
                
            buffTime -= Time.deltaTime;
            bottomText.color = greenColor;

            int days = (int)buffTime / 86400;
            int hours = ((int)buffTime % 86400) / 3600;
            int minutes = ((int)buffTime % 3600) / 60;
            int seconds = ((int)buffTime % 60);

            string timeText = "���� ���� ��� ���� �ð�: ";

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

            bottomText.text = timeText;
        }
    }

    /// <summary>
    /// 30�� �ð� �־��ִ� �Լ�
    /// </summary>
    private void AddAdDeleteTime()
    {
        buffTime += 2592000;
    }

}
