using System;
using UnityEngine;


public class AdMarket : MonoBehaviour
{

    public static AdMarket inst;

    GameObject frontUIRef, adMarketRef;
    GameObject layer0, layer1;

    private int goodsCount;

    // 아이템상점 슬롯
    AdShopBtn[] adShopBtns;
        
    // 아이템수량 및 타임카운터
    [Header("# Input maxValue★  <color=yellow>( Float Data ) (View ToolTip)")]
    [Space]
    [SerializeField]
    [Tooltip("0 = 골드 / 1 = 루비 / 2 = 별\n3 = 영혼 / 4 = 뼈 / 5 고서")]
    private int[] maxItemCount;
    [Header("# Input Reward Count★  <color=yellow>( Float Data )")]
    [Space]
    [SerializeField]
    private int[] ItemRewardCount;
    [Header("#  Read Only ★ <color=#CC3D3D>( Check Cur Value )")]
    [Space]
    [SerializeField]
    private int[] curItemCount;
    [Space]
    [SerializeField]
    private float[] coolTimeTimer;
    [Space]
    [Header("#  Charge CoolTime (Sec) ★  <color=yellow>( Float Data ) ")]
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

        // 배열 초기화
        curItemCount = new int[goodsCount];
        coolTimeTimer = new float[goodsCount];
        Array.Copy(maxItemCount, curItemCount, goodsCount);
        Array.Fill(coolTimeTimer, coolTime);
    }

    private void Start()
    {
        // 나중에 이곳에서 JsonData초기화
        
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

    // 수신받기 
    public void ClickBtn(int type)
    {
        ADViewManager.inst.SampleAD_Active_Funtion(() => 
        {
            curItemCount[type]--;
            adShopBtns[type].TextInit();

            if (curItemCount[type] == 0) // 모두 소진시 종료
            {
                adShopBtns[type].ActiveBtn(false);
            }

            switch (type)
            {
                case 0:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CoinIMG(1), "골드 +100", () => 
                    {
                        GameStatus.inst.PlusGold("1000"); //임시 1000
                    });
                    break;

                case 1:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CoinIMG(0), "루비 +200", () =>
                    {
                        GameStatus.inst.Ruby += 200;
                    });
                    break;

                case 2:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CoinIMG(2), "별 +1,000", () =>
                    {
                        GameStatus.inst.Star += 1000;
                    });
                    break;

                case 3:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CrewMaterialIMG(0), "영혼 +150", () =>
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(0, 150);
                    });
                    break;

                case 4:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CrewMaterialIMG(1), "뼈 +150", () =>
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(1, 150);
                    });
                    break;

                case 5:
                    WorldUI_Manager.inst.Set_Reward_InclueAction(SpriteResource.inst.CrewMaterialIMG(2), "고서 +150", () =>
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(2, 150);
                    });
                    break;
            }
        });
        
    }
    
    private void CooltimeCheaker(int num)
    {
        // 현재 수량이 맥스수량 보다 낮다면
        if (curItemCount[num] < maxItemCount[num])
        {
            coolTimeTimer[num] -= Time.deltaTime;

            // 쿨타임 다 채웠으면
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
