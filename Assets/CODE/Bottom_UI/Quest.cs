using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class Quest : MonoBehaviour
{
    [Header("����Ʈ�ܰ�")]
    [SerializeField] int Number;//����Ʈ �ܰ�
    [Header("�����")]
    [SerializeField] float growthRate;//�����
    [Header("�ʱ� ���귮�� �ʱ����� ���� �������� �ʱⰡ���� �����")]
    [SerializeField] float initalProdRate;//�ʱ� ���귮�� �ʱ����� ����
    [Header("���� ���귮")]
    [SerializeField] float baseProd;//���� ���귮
    [Header("�ܰ躰 ��·� ����")]
    [SerializeField] float powNumRate;//�ܰ躰 ��·� ����
    int lv;//����Ʈ ���׷��̵� ����
    int Lv
    {
        get => lv;
        set
        {
            lv = value;
            GameStatus.inst.SetAryQuestLv(Number,value);
        }
    }
    int LvCur = 1; //��������
    float itemCur = 1; //�����ۺ���
    float powNum;//�ܰ躰 ����
    int buyCount = 1;

    BigInteger baseCost;//�ʱ� ���
    BigInteger nextCost;//�������� ���
    BigInteger initialProd;//�ʱ� ���귮
    BigInteger totalProd;//�� ���귮
    private BigInteger TotalProd
    {
        set
        {
            GameStatus.inst.TotalProdGold -= totalProd;
            totalProd = value;
            GameStatus.inst.TotalProdGold += totalProd;
        }
    }
    [Space]
    [Space]
    [SerializeField] Button UpBtn;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI upGoldText;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] TextMeshProUGUI totalGoldText;

    void Start()
    {
        initValue();
        UIManager.Instance.OnBuyCountChanged.AddListener(_OnCountChanged);
        UIManager.Instance.OnBuyCountChanged.AddListener(SetbtnActive);
        GameStatus.inst.OnGoldChanged.AddListener(SetbtnActive);
        GameStatus.inst.OnPercentageChanged.AddListener(_OnItemPercentChanged);
    }

    void initValue()//�ʱⰪ ����
    {
        powNum = 0;
        for (int iNum = 0; iNum <= Number; iNum++)// �ܰ躰 ���� ����
        {
            powNum += powNumRate * iNum;
        }
        BigInteger resultPowNum = CalCulator.inst.CalculatePow(10, powNum);

        initialProd = BigInteger.Multiply((BigInteger)baseProd, resultPowNum);

        baseCost = BigInteger.Multiply(initialProd, (BigInteger)initalProdRate);
        setNextCost();
        TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
        setText();
    }

    private void setText()
    {
        upGoldText.text = "+" + CalCulator.inst.StringFourDigitAddFloatChanger($"{initialProd * (Lv + buyCount) - initialProd * (Lv)}");
        priceText.text = CalCulator.inst.StringFourDigitAddFloatChanger(nextCost.ToString());
        
        LvText.text = "Lv : " + CalCulator.inst.StringFourDigitChanger(Lv.ToString());
        totalGoldText.text = "Gps : " + CalCulator.inst.StringFourDigitChanger($"{totalProd}");
    }

    public void ClickBuy()
    {
        BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
        if (haveGold >= nextCost)
        {
            Lv += buyCount;
            MissionData.Instance.SetWeeklyMission("����Ʈ ������", buyCount);
            if (Lv >= 25 * LvCur)
            {
                LvCur *= 2;
            }
            TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
            GameStatus.inst.MinusGold(nextCost.ToString());
            UIManager.Instance.OnBuyCountChanged?.Invoke();
        }
        else
        {
            Debug.Log("���� �����մϴ�.");
        }
    }

    private void setNextCost()
    {
        int btnnum = UIManager.Instance.QuestBuyCountBtnNum;
        if (btnnum != 3)//max�� �ƴҶ�
        {
            nextCost = baseCost * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buyCount) - 1) / (growthRate - 1)));
        }
        else//max�� ��
        {
            buyCount = 1;
            BigInteger haveGold = BigInteger.Parse(GameStatus.inst.Gold);
            setNextCost(buyCount);
            while (haveGold >= nextCost)
            {
                buyCount++;
                setNextCost(buyCount);
            }
            buyCount--;
            nextCost = baseCost * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buyCount) - 1) / (growthRate - 1)));
        }
    }

    private void setNextCost(int count)
    {
        nextCost = baseCost * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, count) - 1) / (growthRate - 1)));
    }

    private void _OnCountChanged()
    {
        buyCount = UIManager.Instance.QuestBuyCount;
        setNextCost();
        setText();
    }

    private void _OnItemPercentChanged()
    {
        itemCur = GameStatus.inst.GetAryPercent((int)NormalRelicTag.QuestGold);
        TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
        setText();
    }

    private void SetbtnActive()
    {
        BigInteger havegold = BigInteger.Parse(GameStatus.inst.Gold);
        if (havegold < nextCost || nextCost == 0)
        {
            UpBtn.interactable = false;
        }
        else
        {
            UpBtn.interactable = true;
        }
    }

    public int GetLv()
    {
        return Lv;
    }
}
