using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RubyPayment : MonoBehaviour
{
    public static RubyPayment inst;
    public static Action paymentAction; // 작동시 기능구현해주는 델리게이트

    GameObject frontUIRef;
    GameObject parentRef, payReadyRef, nohaveRubyRef;

    //전체끄기 기능 구현 변수
    GameObject[] allFrontUIRef;

    //결제관련
    Button rubyPayNo;
    Button rubyPayYes;
    TMP_Text curRubyText;
    TMP_Text minusRubyText;
    TMP_Text totalRubyText;

    //상점으로 이동관련
    Button goToRubyShopBtnNo;
    Button goToRubyShopBtnYes;


    // 펫 강화 결제창
    GameObject crewPayRef, nohaveCrewMatRef;
    TMP_Text curMatText, minusMatText, totalMatText;
    Image[] crewPayWindowImg;
    Button[] crewpayWindowBtn = new Button[2];

    //재료없어 창
    Button[] nohaveCrewMatWindowBtn = new Button[2];

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

        frontUIRef = GameManager.inst.FrontUiRef;
        int count = frontUIRef.transform.childCount;
        allFrontUIRef = new GameObject[count];
        for (int index = 0; index < count; index++)
        {
            allFrontUIRef[index] = frontUIRef.transform.GetChild(index).gameObject;
        }

        parentRef = frontUIRef.transform.Find("RubyPay").gameObject;

        // 결제창
        payReadyRef = parentRef.transform.GetChild(0).gameObject;
        curRubyText = payReadyRef.transform.Find("Title/CurRubyText").GetComponent<TMP_Text>();
        minusRubyText = payReadyRef.transform.Find("Title/PriceText").GetComponent<TMP_Text>();
        totalRubyText = payReadyRef.transform.Find("Title/TotalText").GetComponent<TMP_Text>();

        rubyPayNo = payReadyRef.transform.Find("Title/NoBtn").GetComponent<Button>();
        rubyPayYes = payReadyRef.transform.Find("Title/YesBtn").GetComponent<Button>();

        //상점으로 이동하세요창
        nohaveRubyRef = parentRef.transform.GetChild(1).gameObject;
        goToRubyShopBtnNo = nohaveRubyRef.transform.Find("Title/NoBtn").GetComponent<Button>();
        goToRubyShopBtnYes = nohaveRubyRef.transform.Find("Title/YesBtn").GetComponent<Button>();


        // 강화재료 결제창
        crewPayRef = parentRef.transform.GetChild(2).gameObject;
        curMatText = crewPayRef.transform.Find("Title/CurMatText").GetComponent<TMP_Text>();
        minusMatText = crewPayRef.transform.Find("Title/PriceText").GetComponent<TMP_Text>();
        totalMatText = crewPayRef.transform.Find("Title/TotalText").GetComponent<TMP_Text>();
        crewpayWindowBtn[0] = crewPayRef.transform.Find("Title/NoBtn").GetComponent<Button>();
        crewpayWindowBtn[1] = crewPayRef.transform.Find("Title/YesBtn").GetComponent<Button>();
        crewPayWindowImg = crewPayRef.transform.Find("Title/Ruby_Text").GetComponentsInChildren<Image>();

        // 재료없어 창
        nohaveCrewMatRef = parentRef.transform.GetChild(3).gameObject;
        nohaveCrewMatWindowBtn[0] = nohaveCrewMatRef.transform.Find("Title/NoBtn").GetComponent<Button>();
        nohaveCrewMatWindowBtn[1] = nohaveCrewMatRef.transform.Find("Title/YesBtn").GetComponent<Button>();

        buttonInit();
    }
    void Start()
    {


    }

    private void buttonInit()
    {
        //결제창 버튼 초기화
        rubyPayNo.onClick.AddListener(() => CloseUI());
        crewpayWindowBtn[0].onClick.AddListener(() => CloseUI());

        // 루비없는 창 버튼 초기화
        goToRubyShopBtnNo.onClick.AddListener(() => // 아니요 버튼
        {
            CloseUI();
        });

        goToRubyShopBtnYes.onClick.AddListener(() =>  //예 버튼
        {
            CloseUI();
            AllFrontUIClose();
            ShopManager.inst.Active_Shop(2, true);
        });

        // 강화재료 없는창
        nohaveCrewMatWindowBtn[0].onClick.AddListener(() => // 아니요 버튼
        {
            CloseUI();
        });

        nohaveCrewMatWindowBtn[1].onClick.AddListener(() =>  //예 버튼
        {
            CloseUI();
            AllFrontUIClose();
            UIManager.Instance.ClickBotBtn(4);
        });
    }

    // 결제창 호출 및 초기화
    /// <summary>
    ///  루비 결제창 호출 (돈부족하다면 루비상점으로 연결창으로뜸)
    /// </summary>
    /// <param name="Price"> 상품 가격 </param>
    /// <param name="action"> 구매후 실행될 함수</param>
    public void RubyPaymentUiActive(int Price, Action action)
    {
        if (parentRef.activeSelf == false) //메인창 켜짐
        {
            parentRef.SetActive(true);
        }

        // 현재 소지 루비량 체크
        int curRuby = GameStatus.inst.Ruby;
        int totalPrice = curRuby - Price;

        if (totalPrice < 0)  // 돈부족 => 루비상점으로 갈껀지 물어봄
        {
            nohaveRubyRef.SetActive(true);
        }
        else if (totalPrice >= 0) // 결제창 초기화
        {
            curRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(curRuby.ToString());
            minusRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(Price.ToString());
            totalRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(totalPrice.ToString());

            //금전을 지불후 작동될 기능
            rubyPayYes.onClick.RemoveAllListeners();
            rubyPayYes.onClick.AddListener(() => //결제버튼 
            {
                GameStatus.inst.Ruby -= Price;
                paymentAction += action;
                paymentAction?.Invoke();
                paymentAction = null;

                payReadyRef.SetActive(false);
            });

            payReadyRef.SetActive(true);
        }
    }

    // 결제창 호출 및 초기화
    /// <summary>
    ///  루비 결제창 호출 (돈부족하다면 루비상점으로 연결창으로뜸) / 돈이 많다면 결제바로되는게아니고 이벤트만 실행
    /// </summary>
    /// <param name="Price"> 상품 가격 </param>
    /// <param name="action">실행될 함수</param>
    public void RubyPaymentOnlyFuntion(int Price, Action action)
    {
        if (parentRef.activeSelf == false) //메인창 켜짐
        {
            parentRef.SetActive(true);
        }

        // 현재 소지 루비량 체크
        int curRuby = GameStatus.inst.Ruby;
        int totalPrice = curRuby - Price;

        if (totalPrice < 0)  // 돈부족 => 루비상점으로 갈껀지 물어봄
        {
            nohaveRubyRef.SetActive(true);
        }
        else if (totalPrice >= 0) // 결제창 초기화
        {
            paymentAction += action;
            paymentAction?.Invoke();
            paymentAction = null;
            parentRef.SetActive(false);
        }
    }


    // 동료강화창 호출 및 초기화
    /// <summary>
    /// 동료강화 결제창
    /// </summary>
    /// <param name="myType"> 0설화, 1스파크, 2호두(강화재료순서대로)</param>
    /// <param name="Price"> 사야되는가격 </param>
    /// <param name="action"> 이후 호출 </param>
    public void CrewMatPaymentUiActive(int myType, int Price, Action action)
    {
        if (parentRef.activeSelf == false) //메인창 켜짐
        {
            parentRef.SetActive(true);
        }

        // 현재 소지 루비량 체크
        int curMat = GameStatus.inst.CrewMaterial[myType];
        int totalPrice = curMat - Price;

        if (totalPrice < 0)  // 돈부족 => 루비상점으로 갈껀지 물어봄
        {
            nohaveCrewMatRef.SetActive(true);
        }
        else if (totalPrice >= 0) // 결제창 초기화
        {
            for (int index = 0; index < crewPayWindowImg.Length; index++)
            {
                crewPayWindowImg[index].sprite = SpriteResource.inst.CrewMaterialIMG(myType);
            }

            curMatText.text = curMat.ToString("N0");
            minusMatText.text = Price.ToString("N0");
            totalMatText.text = totalPrice.ToString("N0");

            //금전을 지불후 작동될 기능
            crewpayWindowBtn[1].onClick.RemoveAllListeners();
            crewpayWindowBtn[1].onClick.AddListener(() => //결제버튼 
            {
                paymentAction += action;
                paymentAction?.Invoke();
                paymentAction = null;

                crewPayRef.SetActive(false);
            });

            crewPayRef.SetActive(true);
        }
    }

    // 전부 닫기
    private void CloseUI()
    {
        payReadyRef.SetActive(false);
        nohaveRubyRef.SetActive(false);

        crewPayRef.SetActive(false);
        nohaveCrewMatRef.SetActive(false);

        parentRef.SetActive(false);
    }

    private void AllFrontUIClose()
    {
        for (int index = 0; index < allFrontUIRef.Length; index++)
        {
            allFrontUIRef[index].SetActive(false);
        }
    }
}
