using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;

public class Quest : MonoBehaviour
{
    [Tooltip("����Ʈ�ܰ�")]
    [SerializeField] int Number;//����Ʈ �ܰ�
    [Tooltip("�����")]
    [SerializeField] float growthRate;//�����
    [Tooltip("�ʱ� ���귮�� �ʱ����� ���� �������� �ʱⰡ���� �����")]
    [SerializeField] float initalProdRate;//�ʱ� ���귮�� �ʱ����� ���� �������� �ʱⰡ���� �����
    [Tooltip("���� ���귮")]
    [SerializeField] float baseProd;//���� ���귮
    [Tooltip("�ʱ���귮 ����")]
    [SerializeField] float powNumRate;//�ʱ���귮����
    int Lv;//����Ʈ ���׷��̵� ����
    int LvCur = 1; //��������
    int itemCur = 1; //�����ۺ���
    float powNum;//�ܰ躰 ����

    BigInteger baseCost;//�ʱ� ���
    BigInteger nextCost;//�������� ���
    BigInteger initialProd;//�ʱ� ���귮
    BigInteger totalProd;//�� ���귮
    private BigInteger TotalProd
    {
        set
        {
            UIManager.Instance.TotalProdGold -= totalProd;
            totalProd = value;
            UIManager.Instance.TotalProdGold += totalProd;
        }
    }

    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI upGoldText;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] TextMeshProUGUI totalGoldText;

    void Start()
    {
        initValue();
        UIManager.Instance.OnBuyCountChanged.AddListener(Test_OnCountChanged);
    }

    void initValue()//�ʱⰪ ����
    {
        powNum = 0;
        for (int iNum = 0; iNum <= Number; iNum++)// �ܰ躰 ���� ����
        {
            powNum += powNumRate * iNum;
        }
        BigInteger resultPowNum = CalCulator.inst.CalculatePow(10,powNum);

        initialProd = BigInteger.Multiply((BigInteger)baseProd, resultPowNum);

        baseCost = BigInteger.Multiply(initialProd, (BigInteger)initalProdRate);
        setNextCost();
        TotalProd = initialProd * Lv;
        setText();
    }

    private void setText()
    {
        priceText.text = "���� : " + CalCulator.inst.StringFourDigitChanger(nextCost.ToString());
        upGoldText.text ="+" + CalCulator.inst.StringFourDigitChanger($"{initialProd * (Lv + UIManager.Instance.BuyCount) - initialProd * (Lv)}");
        LvText.text = "Lv : " + CalCulator.inst.StringFourDigitChanger(Lv.ToString());
        totalGoldText.text = "Gps : " + CalCulator.inst.StringFourDigitChanger($"{totalProd}");
    }

    public void ClickBuy()
    {
        Lv += UIManager.Instance.BuyCount;
        if (Lv >= 25 * LvCur)
        {
            LvCur *= 2;
        }
        TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
        setNextCost();
        setText();
    }

    private void setNextCost()
    {
        int buycount = UIManager.Instance.BuyCount;
        if (buycount != 0)//max�� �ƴҶ�
        {
            nextCost = baseCost * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buycount) - 1) / (growthRate - 1)));
        }
    }

    private void Test_OnCountChanged()
    {
        setNextCost();
        setText();
    }
}
