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

    //구매버튼
    Button buyBtn;
    TMP_Text buyBtnText;
    Image buyBtnImg;

    //하단 텍스트
    TMP_Text bottomText;

    // 광고제거 카운트다운
    double buffTime;
    
    bool isAdDeleteBuy;
    public bool IsAdDeleteBuy
    {
        get { return isAdDeleteBuy; }
    }

    // 기타참조
    Color greenColor = new Color(0.2f, 1, 1, 1);
    Color redColor = new Color(1, 0.2f, 0, 1);
    string btnTextTemp; // 버튼 텍스트 임시저장용

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

        // 월드버튼
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
        
        //구매 버튼
        buyBtn.onClick.AddListener(() =>
        {
            int Price = RubyPrice.inst.AdDeletePrice; // 루비가격 초기화
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
            if (isAdDeleteBuy == true) // 버프적용중 버튼 비활성화
            {
                isAdDeleteBuy = false;
                buyBtn.enabled = true;
                buyBtnText.alignment = TextAlignmentOptions.Center;
                buyBtnText.text = btnTextTemp;
                buyBtnImg.enabled = true;
                buffTime = 0;
                bottomText.color = redColor;
                bottomText.text = "광고 제거 미적용";
            }
        }
        else if (buffTime > 0) // 광고시간 진행중
        {
            if(isAdDeleteBuy == false) // 버프적용중 버튼 비활성화
            {
                isAdDeleteBuy = true;
                buyBtn.enabled = false;
                buyBtnText.alignment = TextAlignmentOptions.Left;
                buyBtnText.text = " 구매 완료";
                buyBtnImg.enabled = false;
            }
                
            buffTime -= Time.deltaTime;
            bottomText.color = greenColor;

            int days = (int)buffTime / 86400;
            int hours = ((int)buffTime % 86400) / 3600;
            int minutes = ((int)buffTime % 3600) / 60;
            int seconds = ((int)buffTime % 60);

            string timeText = "광고 제거 기능 남은 시간: ";

            if (days > 0)
            {
                timeText += $"{days}일 {hours}시간 {minutes}분";
            }
            else if (hours > 0)
            {
                timeText += $"{hours}시간 {minutes}분";
            }
            else if (minutes > 0)
            {
                timeText += $"{minutes}분";
            }
            else
            {
                timeText += $"{seconds}초";
            }

            bottomText.text = timeText;
        }
    }

    /// <summary>
    /// 30일 시간 넣어주는 함수
    /// </summary>
    private void AddAdDeleteTime()
    {
        buffTime += 2592000;
    }

}
