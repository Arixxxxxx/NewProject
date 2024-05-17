using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class Quest : MonoBehaviour
{
    [Header("����Ʈ�ܰ�")]
    int Number;//����Ʈ �ܰ�
    [Header("��� �����")]
    [SerializeField] float growthRate;//�����
    [Header("�ʱ� ���귮�� �ʱ����� ���� �������� �ʱⰡ���� �����")]
    [SerializeField] float initalProdRate;//�ʱ� ���귮�� �ʱ����� ����
    [Header("���� ���귮")]
    [SerializeField] float baseProd;//���� ���귮
    [Header("�ܰ躰 ��·� ����")]
    [SerializeField] float powNumRate;//�ܰ躰 ��·� ����
    [SerializeField] GameObject objMask;//�ܰ躰 ��·� ����
    int lv;//����Ʈ ���׷��̵� ����
    int Lv
    {
        get => lv;
        set
        {
            lv = value;
            GameStatus.inst.SetAryQuestLv(Number, value);
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

    // �ڵ� �ʱ�ȭ

    void Start()
    {

    }

    public void initQuest()
    {
        Number = transform.GetSiblingIndex();
        Lv = GameStatus.inst.GetAryQuestLv(Number);
        initValue();

        //�ڵ��ʱ�ȭ
        transform.Find("NameText").GetComponent<TMP_Text>().text = Set_QuestName(Number);
        transform.Find("IMG_LayOut/IMG").GetComponent<Image>().sprite = SpriteResource.inst.Get_QuestIcon(Number);

        UIManager.Instance.OnBuyCountChanged.AddListener(_OnCountChanged);
        UIManager.Instance.OnBuyCountChanged.AddListener(SetbtnActive);
        GameStatus.inst.OnGoldChanged.AddListener(() =>
        {
            if (UIManager.Instance.QuestBuyCountBtnNum == 3)//max�� ��
            {
                setNextCost();
                setText();
            }
            SetbtnActive();
        });
        GameStatus.inst.OnPercentageChanged.AddListener(_OnItemPercentChanged);
        GameStatus.inst.OnPercentageChanged.AddListener(setItemCur);
    }

    void initValue()//�ʱⰪ ����
    {
        powNum = powNumRate * (Number * (Number + 1)) / 2;// �ܰ躰 ���� ����
        BigInteger resultPowNum = CalCulator.inst.CalculatePow(10, powNum);

        initialProd = BigInteger.Multiply((BigInteger)baseProd, resultPowNum);

        baseCost = BigInteger.Multiply(initialProd, (BigInteger)initalProdRate);
        setNextCost();
        TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
        setText();
    }

    private void setText()
    {
        upGoldText.text = "LV�� ����ġ +" + CalCulator.inst.StringFourDigitAddFloatChanger($"{initialProd * (Lv + buyCount) - initialProd * (Lv)}");
        priceText.text = CalCulator.inst.StringFourDigitAddFloatChanger(nextCost.ToString());

        LvText.text = "Lv : " + CalCulator.inst.StringFourDigitChanger(Lv.ToString());
        totalGoldText.text = $"����Ʈ ��� ������ : {CalCulator.inst.StringFourDigitAddFloatChanger($"{totalProd}")} / ��";
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

        if (Lv >= 1)
        {
            UIManager.Instance.QeustUpComplete(Number);
        }
    }

    void setItemCur()
    {
        itemCur = GameStatus.inst.GetAryPercent((int)ItemTag.QuestGold);
        itemCur = itemCur / 100 + 1;
    }

    private void setNextCost()
    {
        int btnnum = UIManager.Instance.QuestBuyCountBtnNum;
        float pricediscount = GameStatus.inst.GetAryPercent((int)ItemTag.QuestWeaponPrice);
        if (btnnum != 3)//max�� �ƴҶ�
        {
            nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(baseCost, 1 - (pricediscount / 100)) * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buyCount) - 1) / (growthRate - 1)));
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
            nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(baseCost, 1 - (pricediscount / 100)) * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buyCount) - 1) / (growthRate - 1)));
        }
    }

    private void setNextCost(int count)
    {
        float pricediscount = GameStatus.inst.GetAryPercent((int)ItemTag.QuestWeaponPrice);
        nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(baseCost, 1 - (pricediscount / 100)) * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, count) - 1) / (growthRate - 1)));
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

    public void SetMaskActive(bool value)
    {
        objMask.SetActive(value);
    }

    private string Set_QuestName(int QeustNumber)
    {
        string[] questName =
            {
            "��� �ϱ�", //1
            "���� �ݱ�", //2
            "�׾Ƹ� �����", //3
            "����� ���ñ�", //4
            "�Ǵ� ���� ġ���", //5
            "������ �����ֱ�", //6
            "������ �˹��ϱ�", //7
            "�Ǵٵ��� �����", //8
            "�׸� �׸���", //9
            "���� �����ϱ�", //11
            "�޻� �����۾�", //12
            "��ź �����ϱ� ", //13
            "ķ���� �����ϱ�", //14
            "�ٴٳ��� �ϱ�", //15
            "������ ���̵�", //16
            "���� �ٵ��", //17
            "���� û���ϱ�", //18
            "���� ����ȭ ��������", //19
            "�μ��� ���� �����ϱ�", //20
            "PX���� ���� �������", //21
            "�Ǵ� ���ٵ��", //22
            "����ϱ�", //23
        };

        return questName[QeustNumber];
    }
}
