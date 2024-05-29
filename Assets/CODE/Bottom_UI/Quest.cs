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
            TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
            if (nextRelease == false && lv >= 1)
            {
                UIManager.Instance.QeustUpComplete(Number);
                nextRelease = true;
            }
        }
    }
    int LvCur = 1; //��������
    float itemCur = 1; //�����ۺ���
    float powNum;//�ܰ躰 ����
    int buyCount = 1;
    bool nextRelease = false;

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
        UIManager.Instance.QuestReset.AddListener(resetQuest);
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
        UpBtn.onClick.AddListener(() => { AudioManager.inst.PlaySFX(1); });
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
            //TotalProd = CalCulator.inst.MultiplyBigIntegerAndfloat(initialProd, Lv * LvCur * itemCur);
            GameStatus.inst.MinusGold(nextCost.ToString());
            UIManager.Instance.OnBuyCountChanged?.Invoke();
        }
        else
        {
            Debug.Log("���� �����մϴ�.");
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
        float pricediscount = GameStatus.inst.GetAryPercent((int)ItemTag.QuestDiscount);
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
                if (buyCount >= 100)
                {
                    break;
                }

            }
            buyCount--;
            nextCost = CalCulator.inst.MultiplyBigIntegerAndfloat(baseCost, 1 - (pricediscount / 100)) * (CalCulator.inst.CalculatePow(growthRate, Lv) * (BigInteger)((Mathf.Pow(growthRate, buyCount) - 1) / (growthRate - 1)));
        }
    }

    private void setNextCost(int count)
    {
        float pricediscount = GameStatus.inst.GetAryPercent((int)ItemTag.QuestDiscount);
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
        if (havegold < nextCost && UpBtn.interactable == true || nextCost == 0)
        {
            UpBtn.interactable = false;
        }
        else if (objMask.activeSelf == false && UpBtn.interactable == false && havegold >= nextCost)
        {
            UpBtn.interactable = true;
        }
        else if (objMask.activeSelf && UpBtn.interactable == true)
        {
            UpBtn.interactable = false;
        }
    }

    void resetQuest()
    {
        Lv = 0;
        nextRelease = false;

        if (Number != 0)
        {
            SetMaskActive(true);
        }
        setNextCost();
        setText();
        SetbtnActive();
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
            "���� �����ϱ�", //7
            "������ �˹��ϱ�", //8
            "�Ǵٵ��� �����", //9
            "�׸� �׸���", //11
            "�޻� �����۾�", //12
            "������ ���̵� ", //13
            "��ź �����ϱ�", //14
            "ķ���� �����ϱ�", //15
            "�ٴٳ��� �ϱ�", //16
            "�μ��� ���� �����ϱ�", //17
            "���� �ٵ��", //18
            "���� û���ϱ�", //19
            "���� ����ȭ ��������", //20
            "PX���� ���� �������", //21
            "�Ǵ� ���ٵ��", //22
            "����ϱ�", //23
        };

        return questName[QeustNumber];
    }
}
