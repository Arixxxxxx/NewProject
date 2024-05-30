using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Shop_Gacha : MonoBehaviour
{

    public static Shop_Gacha inst;

    [Header("# ��í ������")] // Ǯ��
    [Space]
    [SerializeField] GameObject[] gachaPrefbas;
    Queue<CrewMaterial_GachaPrefabs> crewMaterialQue = new Queue<CrewMaterial_GachaPrefabs>();
    CrewMaterial_GachaPrefabs[] CrewPrebfbasSc = new CrewMaterial_GachaPrefabs[20];
    Transform[] prefabsTrs = new Transform[2];
    int maxMakeCount = 20;
    Button[] xBtn = new Button[2];
    
    GameObject shopRef, gachaShopRef;
    Gacha relicGachaSc;
    GachaBox_Animator boxSc;

    //���� ���� ����
    enum SelectType { MaterialGacha, RelicGacha }
    SelectType curMode;

    // ��庯�� ��ư
    Animator boxAnim;
    Button[] modeSwapBtn;
    [HideInInspector] public bool isChange;
    GameObject[] maskIMG = new GameObject[2];
    GameObject[] buyBtnArr = new GameObject[2];

    Button[] crewMaterialBuyBtn;
    Button[] relicBuyBtn;

    // �̱����
    [SerializeField]
    GameObject gachaPlayGroundRef;
    GameObject[] gachaResultRef = new GameObject[2]; // ���Ȯ��â



    private void Awake()
    {
        //�̱���
        #region
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
        shopRef = ShopManager.inst.ShopRef;
        gachaShopRef = shopRef.transform.Find("Shop_List/Gacha_Shop").gameObject;

        relicGachaSc = UIManager.Instance.GetComponent<Gacha>();
        

        boxAnim = gachaShopRef.transform.Find("MainViewr").GetComponent<Animator>();
        boxSc = boxAnim.GetComponent<GachaBox_Animator>();
        modeSwapBtn = gachaShopRef.transform.Find("Cacha_Type_Btn").GetComponentsInChildren<Button>();
        maskIMG[0] = modeSwapBtn[0].transform.Find("Mask").gameObject;
        maskIMG[1] = modeSwapBtn[1].transform.Find("Mask").gameObject;
        buyBtnArr[0] = gachaShopRef.transform.Find("buyBtnArr").GetChild(0).gameObject;
        crewMaterialBuyBtn = buyBtnArr[0].transform.GetComponentsInChildren<Button>();

        buyBtnArr[1] = gachaShopRef.transform.Find("buyBtnArr").GetChild(1).gameObject;
        relicBuyBtn = buyBtnArr[1].transform.GetComponentsInChildren<Button>();

        //�������̱� �ʱ�ȭ
        gachaPlayGroundRef = shopRef.transform.parent.Find("GachaPlayGround").gameObject;
        gachaResultRef[0] = gachaPlayGroundRef.transform.Find("CrewMaterial").gameObject;
        prefabsTrs[0] = gachaResultRef[0].transform.Find("PrefabsFiled");
        xBtn[0] = gachaResultRef[0].transform.Find("XBtn").GetComponent<Button>();

        // ������ �ʱ�ȭ
        for (int index =0; index < maxMakeCount; index++)
        {
            CrewResultPrefbasInit(index);
        }
    }

    private void CrewResultPrefbasInit(int index)
    {
        CrewMaterial_GachaPrefabs obj = Instantiate(gachaPrefbas[0], prefabsTrs[0]).GetComponent<CrewMaterial_GachaPrefabs>();
        obj.gameObject.SetActive(false);
        CrewPrebfbasSc[index] = obj;
        crewMaterialQue.Enqueue(obj);
    }

    void Start()
    {
        BtnInit();
    }

    bool[] isTodayAdViewrGachaPlay= new bool[2];

    private void BtnInit()
    {
        modeSwapBtn[0].onClick.AddListener(() =>
        {
            if (isChange == true || curMode == SelectType.MaterialGacha) { return; }
            isChange = true;

            curMode = SelectType.MaterialGacha;
            BtnMaskChanger((int)curMode);
            boxAnim.SetTrigger(curMode.ToString());
        });

        modeSwapBtn[1].onClick.AddListener(() =>
        {
            if (isChange == true || curMode == SelectType.RelicGacha) { return; }
            isChange = true;

            curMode = SelectType.RelicGacha;
            BtnMaskChanger((int)curMode);
            boxAnim.SetTrigger(curMode.ToString());
        });


        //  ����̱� ��í ��ư
        // 3ȸ
        crewMaterialBuyBtn[0].onClick.AddListener(() =>
        {
            AudioManager.inst.PlaySFX(4);
            int price = RubyPrice.inst.CrewMaterialGachaPrice(0);
            RubyPayment.inst.RubyPaymentUiActive(price, () => Play_CrewMaterialGacha(10));
        });

        // 9ȸ
        crewMaterialBuyBtn[1].onClick.AddListener(() =>
        {
            AudioManager.inst.PlaySFX(4);
            int price = RubyPrice.inst.CrewMaterialGachaPrice(1);
            RubyPayment.inst.RubyPaymentUiActive(price, () => Play_CrewMaterialGacha(20));
        });
        // ����
        crewMaterialBuyBtn[2].onClick.AddListener(() =>
        {
            if (isTodayAdViewrGachaPlay[0] == true) { return; }     //���� ������
            AudioManager.inst.PlaySFX(4);
            Play_CrewMaterialGacha(5);
        });

        // ���� �̱���â ���� ��ư
        xBtn[0].onClick.AddListener(() =>
        {
            xBtn[0].gameObject.SetActive(false);
            gachaResultRef[0].SetActive(false);
            gachaPlayGroundRef.SetActive(false);

            for (int index = 0; index < CrewPrebfbasSc.Length; index++)
            {
                if (CrewPrebfbasSc[index].gameObject.activeSelf)
                {
                    CrewPrebfbasSc[index].ReturnPrefabs();
                }
            }
            
        });


        // �����̱� ��í ��ư

        relicBuyBtn[0].onClick.AddListener(() =>
        {
            int price = RubyPrice.inst.RelicGachaPrice(0);

        });

        // 9ȸ
        relicBuyBtn[1].onClick.AddListener(() =>
        {
            int price = RubyPrice.inst.RelicGachaPrice(1);
        });
        // ����
        relicBuyBtn[2].onClick.AddListener(() =>
        {
            if (isTodayAdViewrGachaPlay[1] == true) { return; }
        });
    }

    // ��� ��庯�� ��ư ����ũ OnOFF
    private void BtnMaskChanger(int value)
    {
        for (int index = 0; index < maskIMG.Length; index++)
        {
            if (index == value)
            {
                maskIMG[index].gameObject.SetActive(false);
                buyBtnArr[index].gameObject.SetActive(true);
            }
            else
            {
                maskIMG[index].gameObject.SetActive(true);
                buyBtnArr[index].gameObject.SetActive(false);
            }
        }
    }

    // ShopManager���� ����
    public void Init(bool active)
    {
        if (isChange == true)
        {
            isChange = false;
        }

        boxSc.AllParticleActiveFalse();

        if (active)
        {
            BtnMaskChanger(0);
            curMode = SelectType.MaterialGacha;
        }
    }


    // ���̱� ����!
    private void Play_CrewMaterialGacha(int gachaCount)
    {
        gachaPlayGroundRef.SetActive(true);
        gachaResultRef[0].SetActive(true);

        StartCoroutine(PlayMaterial(gachaCount));
    }

    
    WaitForSeconds startWaitTime = new WaitForSeconds(0.1f);
    WaitForSeconds endWaitTime = new WaitForSeconds(0.5f);
    WaitForSeconds intervalTime = new WaitForSeconds(0.14f);

    int type = 0;
    int count = 0;
    int criType = 0;
    IEnumerator PlayMaterial(int Count)
    {
        yield return startWaitTime;

        for (int index = 0; index < Count; index++) 
        {
            type = 0;
            count = 0;
            criType = 0;

            CrewMaterial_GachaPrefabs obj = crewMaterialQue.Dequeue();
            //���� ����

            //1. Ÿ�� (����, ȭ��, ���ҽ�)
            type = Random.Range(0, 3);
            
            //2. ����
            count = Random.Range(5, 20);

            //3. ũ��Ÿ�� ���� 80% = �Ϲ� / 15% ���� / 5% ����
            float Cri = Random.Range(0f, 100f);
            if(Cri > 0 && Cri < 80f)
            {
                criType = 0;
            }
            else if(Cri >= 80 && Cri < 95)
            {
                criType = 1;
                count *= 2; // 2��
            }
            else
            {
                criType = 2;
                count *= 3;// 3��
            }

            GameStatus.inst.Set_crewMaterial(type, count); // ���� ��� �־���
            obj.Set_CrewMaterialGacha(type, count, criType); // �������

            yield return intervalTime;
        }

        yield return endWaitTime;

        xBtn[0].gameObject.SetActive(true);
    }






    // Ǯ������

    public void CrewMaterial_ReturnObj_ToPool(CrewMaterial_GachaPrefabs obj)
    {
        obj.gameObject.SetActive(false);
        crewMaterialQue.Enqueue(obj);
    }
}
