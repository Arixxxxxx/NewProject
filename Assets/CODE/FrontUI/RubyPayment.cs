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


    // �� ��ȭ ����â
    GameObject crewPayRef, nohaveCrewMatRef;
    TMP_Text curMatText, minusMatText, totalMatText;
    Image[] crewPayWindowImg;
    Button[] crewpayWindowBtn = new Button[2];

    //������ â
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


        // ��ȭ��� ����â
        crewPayRef = parentRef.transform.GetChild(2).gameObject;
        curMatText = crewPayRef.transform.Find("Title/CurMatText").GetComponent<TMP_Text>();
        minusMatText = crewPayRef.transform.Find("Title/PriceText").GetComponent<TMP_Text>();
        totalMatText = crewPayRef.transform.Find("Title/TotalText").GetComponent<TMP_Text>();
        crewpayWindowBtn[0] = crewPayRef.transform.Find("Title/NoBtn").GetComponent<Button>();
        crewpayWindowBtn[1] = crewPayRef.transform.Find("Title/YesBtn").GetComponent<Button>();
        crewPayWindowImg = crewPayRef.transform.Find("Title/Ruby_Text").GetComponentsInChildren<Image>();

        // ������ â
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
        //����â ��ư �ʱ�ȭ
        rubyPayNo.onClick.AddListener(() => CloseUI());
        crewpayWindowBtn[0].onClick.AddListener(() => CloseUI());

        // ������ â ��ư �ʱ�ȭ
        goToRubyShopBtnNo.onClick.AddListener(() => // �ƴϿ� ��ư
        {
            CloseUI();
        });

        goToRubyShopBtnYes.onClick.AddListener(() =>  //�� ��ư
        {
            CloseUI();
            AllFrontUIClose();
            ShopManager.inst.Active_Shop(2, true);
        });

        // ��ȭ��� ����â
        nohaveCrewMatWindowBtn[0].onClick.AddListener(() => // �ƴϿ� ��ư
        {
            CloseUI();
        });

        nohaveCrewMatWindowBtn[1].onClick.AddListener(() =>  //�� ��ư
        {
            CloseUI();
            AllFrontUIClose();
            UIManager.Instance.ClickBotBtn(4);
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
            curRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(curRuby.ToString());
            minusRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(Price.ToString());
            totalRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(totalPrice.ToString());

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


    // ���ᰭȭâ ȣ�� �� �ʱ�ȭ
    /// <summary>
    /// ���ᰭȭ ����â
    /// </summary>
    /// <param name="myType"> 0��ȭ, 1����ũ, 2ȣ��(��ȭ���������)</param>
    /// <param name="Price"> ��ߵǴ°��� </param>
    /// <param name="action"> ���� ȣ�� </param>
    public void CrewMatPaymentUiActive(int myType, int Price, Action action)
    {
        if (parentRef.activeSelf == false) //����â ����
        {
            parentRef.SetActive(true);
        }

        // ���� ���� ��� üũ
        int curMat = GameStatus.inst.CrewMaterial[myType];
        int totalPrice = curMat - Price;

        if (totalPrice < 0)  // ������ => ���������� ������ ���
        {
            nohaveCrewMatRef.SetActive(true);
        }
        else if (totalPrice >= 0) // ����â �ʱ�ȭ
        {
            for (int index = 0; index < crewPayWindowImg.Length; index++)
            {
                crewPayWindowImg[index].sprite = SpriteResource.inst.CrewMaterialIMG(myType);
            }

            curMatText.text = curMat.ToString("N0");
            minusMatText.text = Price.ToString("N0");
            totalMatText.text = totalPrice.ToString("N0");

            //������ ������ �۵��� ���
            crewpayWindowBtn[1].onClick.RemoveAllListeners();
            crewpayWindowBtn[1].onClick.AddListener(() => //������ư 
            {
                paymentAction += action;
                paymentAction?.Invoke();
                paymentAction = null;

                crewPayRef.SetActive(false);
            });

            crewPayRef.SetActive(true);
        }
    }

    // ���� �ݱ�
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
