using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RubyPayment : MonoBehaviour
{
    public static RubyPayment inst;
    public static Action paymentAction; // �۵��� ��ɱ������ִ� ��������Ʈ

    GameObject frontUIRef;
    GameObject parentRef, payReadyRef, nohaveRubyRef;

    //��ü���� ��� ���� ����
    GameObject[] allFrontUIRef;

    //��������
    Button rubyPayNo;
    Button rubyPayYes;
    TMP_Text curRubyText;
    TMP_Text minusRubyText;
    TMP_Text totalRubyText;

    //�������� �̵�����
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

        // ����â
        payReadyRef = parentRef.transform.GetChild(0).gameObject;
        curRubyText = payReadyRef.transform.Find("Title/CurRubyText").GetComponent<TMP_Text>();
        minusRubyText = payReadyRef.transform.Find("Title/PriceText").GetComponent<TMP_Text>();
        totalRubyText = payReadyRef.transform.Find("Title/TotalText").GetComponent<TMP_Text>();

        rubyPayNo = payReadyRef.transform.Find("Title/NoBtn").GetComponent<Button>();
        rubyPayYes = payReadyRef.transform.Find("Title/YesBtn").GetComponent<Button>();

        //�������� �̵��ϼ���â
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
        //����â ��ư �ʱ�ȭ
        rubyPayNo.onClick.AddListener(() => CloseUI());

        // ������ â ��ư �ʱ�ȭ
        goToRubyShopBtnNo.onClick.AddListener(() => // �ƴϿ� ��ư
        {
            CloseUI();
        });

        goToRubyShopBtnYes.onClick.AddListener(() =>  //�� ��ư
        {
            nohaveRubyRef.SetActive(false);
            AllFrontUIClose();
            ShopManager.Instance.OpenRubyShop();
        });
    }

    // ����â ȣ�� �� �ʱ�ȭ
    /// <summary>
    ///  ��� ����â ȣ�� (�������ϴٸ� ���������� ����â���ζ�)
    /// </summary>
    /// <param name="Price"> ��ǰ ���� </param>
    /// <param name="action"> ������ ����� �Լ�</param>
    public void RubyPaymentUiActive(int Price, Action action)
    {
        if (parentRef.activeSelf == false) //����â ����
        {
            parentRef.SetActive(true);
        }

        // ���� ���� ��� üũ
        int curRuby = GameStatus.inst.Ruby;
        int totalPrice = curRuby - Price;

        if (totalPrice < 0)  // ������ => ���������� ������ ���
        {
            nohaveRubyRef.SetActive(true);
        }
        else if (totalPrice >= 0) // ����â �ʱ�ȭ
        {
            curRubyText.text = curRuby.ToString("N0");
            minusRubyText.text = Price.ToString("N0");


            totalRubyText.text = totalPrice.ToString("N0");

            //������ ������ �۵��� ���
            rubyPayYes.onClick.RemoveAllListeners();
            rubyPayYes.onClick.AddListener(() => //������ư 
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

    // ����â ȣ�� �� �ʱ�ȭ
    /// <summary>
    ///  ��� ����â ȣ�� (�������ϴٸ� ���������� ����â���ζ�) / ���� ���ٸ� �����ٷεǴ°Ծƴϰ� �̺�Ʈ�� ����
    /// </summary>
    /// <param name="Price"> ��ǰ ���� </param>
    /// <param name="action">����� �Լ�</param>
    public void RubyPaymentOnlyFuntion(int Price, Action action)
    {
        if (parentRef.activeSelf == false) //����â ����
        {
            parentRef.SetActive(true);
        }

        // ���� ���� ��� üũ
        int curRuby = GameStatus.inst.Ruby;
        int totalPrice = curRuby - Price;

        if (totalPrice < 0)  // ������ => ���������� ������ ���
        {
            nohaveRubyRef.SetActive(true);
        }
        else if (totalPrice >= 0) // ����â �ʱ�ȭ
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
