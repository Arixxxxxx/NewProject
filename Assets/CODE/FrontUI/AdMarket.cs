using System;
using UnityEngine;


public class AdMarket : MonoBehaviour
{

    public static AdMarket inst;

    GameObject frontUIRef, adMarketRef;
    GameObject layer0, layer1;

    private int goodsCount;

    // �����ۻ��� ����
    AdShopBtn[] adShopBtns;
        
    // �����ۼ��� �� Ÿ��ī����
    [Header("# Input maxValue��  <color=yellow>( Float Data ) (View ToolTip)")]
    [Space]
    [SerializeField]
    [Tooltip("0 = ��� / 1 = ��� / 2 = ��\n3 = ��ȥ / 4 = �� / 5 ��")]
    private int[] maxItemCount;
    [Header("# Input Reward Count��  <color=yellow>( Float Data )")]
    [Space]
    [SerializeField]
    private int[] ItemRewardCount;
    [Header("#  Read Only �� <color=#CC3D3D>( Check Cur Value )")]
    [Space]
    [SerializeField]
    private int[] curItemCount;
    [Space]
    [SerializeField]
    private float[] coolTimeTimer;
    [Space]
    [Header("#  Charge CoolTime (Sec) ��  <color=yellow>( Float Data ) ")]
    [Space]
    [SerializeField]
    float coolTime;

   
    public float Get_CoolTime(int type) => coolTimeTimer[type];
    public int MaxItemCount(int type) => maxItemCount[type];
    public int CurItemCount(int type) => curItemCount[type];

    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        frontUIRef = GameManager.inst.FrontUiRef;
        adMarketRef = frontUIRef.transform.Find("AdMarket").gameObject;
        layer0 = adMarketRef.transform.Find("Window/BtnArry/Layer0").gameObject;
        layer1 = adMarketRef.transform.Find("Window/BtnArry/Layer1").gameObject;

        goodsCount = layer0.transform.childCount + layer1.transform.childCount;
        adShopBtns = new AdShopBtn[goodsCount];
        for (int i = 0; i < layer0.transform.childCount; i++)
        {
            adShopBtns[i] = layer0.transform.GetChild(i).GetComponent<AdShopBtn>();
        }
        for (int i = layer0.transform.childCount; i < goodsCount; i++)
        {
            adShopBtns[i] = layer1.transform.GetChild(i - layer0.transform.childCount).GetComponent<AdShopBtn>();
        }

        // �迭 �ʱ�ȭ
        curItemCount = new int[goodsCount];
        coolTimeTimer = new float[goodsCount];
        Array.Copy(maxItemCount, curItemCount, goodsCount);
        Array.Fill(coolTimeTimer, coolTime);
    }

    private void Start()
    {
        // ���߿� �̰����� JsonData�ʱ�ȭ
        
        //coolTimeTimer = 
    }
    
    void Update()
    {
        CooltimeCheaker(0);
        CooltimeCheaker(1);
        CooltimeCheaker(2);
        CooltimeCheaker(3);
        CooltimeCheaker(4);
        CooltimeCheaker(5);
    }

    public void ActiveAdMarket(bool value)
    {
        if (value)
        {
            adMarketRef.SetActive(true);
        }
        else
        {
            adMarketRef.SetActive(false);
        }
    }

    // ���Źޱ� 
    public void ClickBtn(int type)
    {
        ADViewManager.inst.SampleAD_Active_Funtion(() => 
        {
            curItemCount[type]--;
            adShopBtns[type].TextInit();

            if (curItemCount[type] == 0) // ��� ������ ����
            {
                adShopBtns[type].ActiveBtn(false);
            }

            switch (type)
            {
                case 0:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CoinIMG(1), "��� +100", () => 
                    {
                        GameStatus.inst.PlusGold("1000"); //�ӽ� 1000
                    });
                    break;

                case 1:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CoinIMG(0), "��� +200", () =>
                    {
                        GameStatus.inst.Ruby += 200;
                    });
                    break;

                case 2:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CoinIMG(2), "�� +1,000", () =>
                    {
                        GameStatus.inst.Star += 1000;
                    });
                    break;

                case 3:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CrewMaterialIMG(0), "��ȥ +150", () =>
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(0, 150);
                    });
                    break;

                case 4:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CrewMaterialIMG(1), "�� +150", () =>
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(1, 150);
                    });
                    break;

                case 5:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CrewMaterialIMG(2), "�� +150", () =>
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(2, 150);
                    });
                    break;
            }
        });
        
    }
    
    private void CooltimeCheaker(int num)
    {
        // ���� ������ �ƽ����� ���� ���ٸ�
        if (curItemCount[num] < maxItemCount[num])
        {
            coolTimeTimer[num] -= Time.deltaTime;

            // ��Ÿ�� �� ä������
            if (coolTimeTimer[num] <= 0)
            {
                coolTimeTimer[num] = coolTime;
                curItemCount[num]++;
                
                if(curItemCount[num] > 0)
                {
                    adShopBtns[num].ActiveBtn(true);
                }
            }
        }
    }
}
