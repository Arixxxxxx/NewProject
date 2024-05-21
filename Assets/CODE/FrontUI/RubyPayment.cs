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

        buttonInit();
    }
    void Start()
    {


    }

    private void buttonInit()
    {
        //결제창 버튼 초기화
        rubyPayNo.onClick.AddListener(() => CloseUI());

        // 루비없는 창 버튼 초기화
        goToRubyShopBtnNo.onClick.AddListener(() => // 아니요 버튼
        {
            CloseUI();
        });

        goToRubyShopBtnYes.onClick.AddListener(() =>  //예 버튼
        {
            nohaveRubyRef.SetActive(false);
            AllFrontUIClose();
            ShopManager.Instance.OpenRubyShop();
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
            curRubyText.text = curRuby.ToString("N0");
            minusRubyText.text = Price.ToString("N0");


            totalRubyText.text = totalPrice.ToString("N0");

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

    private void CloseUI()
    {
        payReadyRef.SetActive(false);
        nohaveRubyRef.SetActive(false);
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
