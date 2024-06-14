using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SelectType 
{
    MaterialGacha, 
    RelicGacha 
}

public class Shop_Gacha : MonoBehaviour
{

    public static Shop_Gacha inst;

    [Header("# ��í ������")] // Ǯ��
    [Space]
    [SerializeField] GameObject[] gachaPrefbas;

    //���̱� Ǯ������
    Queue<CrewMaterial_GachaPrefabs> crewMaterialQue = new Queue<CrewMaterial_GachaPrefabs>();
    CrewMaterial_GachaPrefabs[] CrewPrebfbasSc = new CrewMaterial_GachaPrefabs[20];



    //�����̱� Ǯ������
    Queue<Raw_Prefabs> rawPrefabsQue = new Queue<Raw_Prefabs>();
    Queue<Relic_Result_Prefabs> relicResultQue = new Queue<Relic_Result_Prefabs>();

    Raw_Prefabs[] rawPrefabs = new Raw_Prefabs[20];
    Relic_Result_Prefabs[] relicResultPrefabs = new Relic_Result_Prefabs[20];

    Transform rawPrefbas_SpawnTrs;

    Transform[] prefabsTrs = new Transform[2];
    int maxMakeCount = 20;
    Button[] xBtn = new Button[2];
    Button[] reStartBtn = new Button[2];
    TMP_Text[] priceTextInRestarBtn = new TMP_Text[2];

    GameObject shopRef, gachaShopRef, completePsRef;
    Gacha relicGachaSc;
    GachaBox_Animator boxSc;

    //���� ���� ����
    
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
    int curGachaCount;
    GameObject gachaPlayGroundRef;
    GameObject[] gachaResultRef = new GameObject[2]; // ���Ȯ��â


    // �����̱�
    GameObject rellicRowIMG;


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
        reStartBtn[0] = gachaResultRef[0].transform.Find("ReStart").GetComponent<Button>();
        priceTextInRestarBtn[0] = reStartBtn[0].transform.Find("Price").GetComponent<TMP_Text>();

        rellicRowIMG = gachaPlayGroundRef.transform.Find("RelicRaw").gameObject;
        gachaResultRef[1] = rellicRowIMG.transform.Find("Relic").gameObject;
        prefabsTrs[1] = gachaResultRef[1].transform.Find("PrefabsFiled");
        xBtn[1] = gachaResultRef[1].transform.Find("XBtn").GetComponent<Button>();
        reStartBtn[1] = gachaResultRef[1].transform.Find("ReStart").GetComponent<Button>();
        priceTextInRestarBtn[1] = reStartBtn[1].transform.Find("Price").GetComponent<TMP_Text>();

        //�����̱����
        rawPrefbas_SpawnTrs = GameManager.inst.RawImgRef.transform.Find("Gacha/startPos");
        completePsRef = GameManager.inst.RawImgRef.transform.Find("Gacha/Ps").gameObject;

        // ������ �ʱ�ȭ
        for (int index = 0; index < maxMakeCount; index++)
        {
            CrewResultPrefbasInit(index);
            Relic_RawPrefabsInit(index);
            Relic_ResultPrefabsInit(index);
        }


    }

    // �������̱� Result ������
    private void CrewResultPrefbasInit(int index)
    {
        CrewMaterial_GachaPrefabs obj = Instantiate(gachaPrefbas[0], prefabsTrs[0]).GetComponent<CrewMaterial_GachaPrefabs>();
        obj.gameObject.SetActive(false);
        CrewPrebfbasSc[index] = obj;
        crewMaterialQue.Enqueue(obj);
    }

    // �����̱� �ο��̹��� ������
    private void Relic_RawPrefabsInit(int index)
    {
        Raw_Prefabs obj = Instantiate(gachaPrefbas[1], rawPrefbas_SpawnTrs).GetComponent<Raw_Prefabs>();
        obj.gameObject.SetActive(false);
        rawPrefabs[index] = obj;
        rawPrefabsQue.Enqueue(obj);
    }

    // �����̱� Result ������
    private void Relic_ResultPrefabsInit(int index)
    {
        Relic_Result_Prefabs obj = Instantiate(gachaPrefbas[2], prefabsTrs[1]).GetComponent<Relic_Result_Prefabs>();
        obj.gameObject.SetActive(false);
        relicResultPrefabs[index] = obj;
        relicResultQue.Enqueue(obj);
    }

    void Start()
    {
        BtnInit();
    }

    bool[] isTodayAdViewrGachaPlay = new bool[2];

    private void BtnInit()
    {
        modeSwapBtn[0].onClick.AddListener(() =>
        {
            GachaMode_Changer(SelectType.MaterialGacha);
        });

        modeSwapBtn[1].onClick.AddListener(() =>
        {
            GachaMode_Changer(SelectType.RelicGacha);
        });


        //  ����̱� ��í ��ư
        // 3ȸ
        crewMaterialBuyBtn[0].onClick.AddListener(() =>
        {
            AudioManager.inst.Play_Ui_SFX(4,0.8f);
            int price = RubyPrice.inst.CrewMaterialGachaPrice(0);
            RubyPayment.inst.RubyPaymentUiActive(price, () => Play_CrewMaterialGacha(10, false));
        });

        // 9ȸ
        crewMaterialBuyBtn[1].onClick.AddListener(() =>
        {
            AudioManager.inst.Play_Ui_SFX(4, 0.8f);
            int price = RubyPrice.inst.CrewMaterialGachaPrice(1);
            RubyPayment.inst.RubyPaymentUiActive(price, () => Play_CrewMaterialGacha(20, false));
        });
        // ����
        crewMaterialBuyBtn[2].onClick.AddListener(() =>
        {
            AudioManager.inst.Play_Ui_SFX(4, 0.8f);
            crewMaterialBuyBtn[2].interactable = false;
            GameStatus.inst.Shop_adView_GachaDateValue(0, DateTime.Now);     // ���ڱ��
            
            ADViewManager.inst.AdMob_ActiveAndFuntion(() => Play_CrewMaterialGacha(5, true));
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

        // ����ŸƮ
        reStartBtn[0].onClick.AddListener(() =>
        {
            // ���� ȸ��
            for (int index = 0; index < CrewPrebfbasSc.Length; index++)
            {
                if (CrewPrebfbasSc[index].gameObject.activeSelf)
                {
                    CrewPrebfbasSc[index].ReturnPrefabs();
                }
            }

            GameStatus.inst.Ruby -= curGachaCount == 10 ? RubyPrice.inst.CrewMaterialGachaPrice(0) : RubyPrice.inst.CrewMaterialGachaPrice(1);
            Play_CrewMaterialGacha(curGachaCount, false);
        });


        // �����̱� ��í ��ư

        // 10ȸ
        relicBuyBtn[0].onClick.AddListener(() =>
        {
            AudioManager.inst.Play_Ui_SFX(4, 0.8f);
            int price = RubyPrice.inst.RelicGachaPrice(0);
            RubyPayment.inst.RubyPaymentUiActive(price, () => Play_RelicGacha(10, false));
        });

        // 20ȸ
        relicBuyBtn[1].onClick.AddListener(() =>
        {
            AudioManager.inst.Play_Ui_SFX(4, 0.8f);
            int price = RubyPrice.inst.RelicGachaPrice(1);
            RubyPayment.inst.RubyPaymentUiActive(price, () => Play_RelicGacha(20, false));
        });

        // ����
        relicBuyBtn[2].onClick.AddListener(() =>
        {
            AudioManager.inst.Play_Ui_SFX(4, 0.8f);
            relicBuyBtn[2].interactable = false;
            GameStatus.inst.Shop_adView_GachaDateValue(1,DateTime.Now);     // ���ڱ��
            ADViewManager.inst.AdMob_ActiveAndFuntion(() => Play_RelicGacha(5, true));

        });

        //X ��ư
        xBtn[1].onClick.AddListener(() =>
        {
            xBtn[1].gameObject.SetActive(false);
            completePsRef.SetActive(false);
            gachaResultRef[1].SetActive(false);
            gachaPlayGroundRef.SetActive(false);
            rellicRowIMG.SetActive(false);
            WorldUI_Manager.inst.RawImagePlayAcitve(2, false);

            for (int index = 0; index < rawPrefabs.Length; index++)
            {
                if (rawPrefabs[index].gameObject.activeSelf)
                {
                    rawPrefabs[index].ReturnPrefabs();
                }
            }

            for (int index = 0; index < relicResultPrefabs.Length; index++)
            {
                if (relicResultPrefabs[index].gameObject.activeSelf)
                {
                    relicResultPrefabs[index].ReturnPrefabs();
                }
            }

        });

        // ����ŸƮ
        reStartBtn[1].onClick.AddListener(() =>
        {
            // ���� ȸ��
            for (int index = 0; index < rawPrefabs.Length; index++)
            {
                if (rawPrefabs[index].gameObject.activeSelf)
                {
                    rawPrefabs[index].ReturnPrefabs();
                }
            }

            for (int index = 0; index < relicResultPrefabs.Length; index++)
            {
                if (relicResultPrefabs[index].gameObject.activeSelf)
                {
                    relicResultPrefabs[index].ReturnPrefabs();
                }
            }

            completePsRef.SetActive(false);
            GameStatus.inst.Ruby -= arr.Count == 10 ? RubyPrice.inst.RelicGachaPrice(0) : RubyPrice.inst.RelicGachaPrice(1);
            Play_RelicGacha(arr.Count, false);
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

    //�ܺο��� �������ִ� �Լ�
    public void GachaMode_Changer(SelectType types)
    {
        if (isChange == true || curMode == types)
        {
            return;
        }

        isChange = true;

        // ���޵� �Ű����� types ���� ����Ͽ� curMode ����
        curMode = types;
        BtnMaskChanger((int)curMode);
        boxAnim.SetTrigger(curMode.ToString());
    }

    /////////////////////////////////////////// ���� //////////////////////////////////////////


    // ���̱� ����!
    private void Play_CrewMaterialGacha(int gachaCount, bool isAd)
    {
        reStartBtn[0].gameObject.SetActive(false);
        xBtn[0].gameObject.SetActive(false);

        gachaPlayGroundRef.SetActive(true);
        gachaResultRef[0].SetActive(true);
        curGachaCount = gachaCount;
        StartCoroutine(PlayMaterial(gachaCount, isAd));
    }


    WaitForSeconds startWaitTime = new WaitForSeconds(0.1f);
    WaitForSeconds endWaitTime = new WaitForSeconds(0.5f);
    WaitForSeconds intervalTime = new WaitForSeconds(0.14f);

    int type = 0;
    int count = 0;
    int criType = 0;
    IEnumerator PlayMaterial(int Count, bool isAd)
    {
        WorldUI_Manager.inst.Effect_WhiteCutton(1f); // �Ͼ�ȭ�� ����Ʈ
        yield return startWaitTime;

        for (int index = 0; index < Count; index++)
        {
            type = 0;
            count = 0;
            criType = 0;

            CrewMaterial_GachaPrefabs obj = crewMaterialQue.Dequeue();
            //���� ����

            //1. Ÿ�� (����, ȭ��, ���ҽ�)
            type = UnityEngine.Random.Range(0, 3);

            //2. ����
            

            //3. ũ��Ÿ�� ���� 80% = �Ϲ� / 15% ���� / 5% ����
            float Cri = UnityEngine.Random.Range(0f, 100f);
            if (Cri > 0 && Cri < 80f)
            {
                criType = 0;
                count = UnityEngine.Random.Range(5, 20);
            }
            else if (Cri >= 80 && Cri < 95)
            {
                criType = 1;
                count = UnityEngine.Random.Range(25, 40);
            }
            else
            {
                criType = 2;
                count = UnityEngine.Random.Range(41, 80);
            }

            GameStatus.inst.Set_crewMaterial(type, count); // ���� ��� �־���
            obj.Set_CrewMaterialGacha(type, count, criType); // �������

            yield return intervalTime;
        }

        yield return endWaitTime;

        if (isAd == false)
        {
            //�ѹ��� �ҷ� ��ư
            int curPrice = curGachaCount == 10 ? RubyPrice.inst.CrewMaterialGachaPrice(0) : RubyPrice.inst.CrewMaterialGachaPrice(1);
            if (GameStatus.inst.Ruby >= curPrice)
            {
                priceTextInRestarBtn[0].text = $"x {curPrice}";
                reStartBtn[0].gameObject.SetActive(true);
            }
        }

        xBtn[0].gameObject.SetActive(true);
    }


    // �����̱� ����

    List<int> arr = new List<int>();
    public void Play_RelicGacha(int count, bool isAd)
    {
        //�ʱ�ȭ
        arr.Clear();
        xBtn[1].gameObject.SetActive(false);
        reStartBtn[1].gameObject.SetActive(false);
        gachaResultRef[1].gameObject.SetActive(false);

        arr.AddRange(relicGachaSc.MakeRelicGacha(count));
        gachaPlayGroundRef.SetActive(true);

        StartCoroutine(PlayRelicAction(isAd));
    }

    WaitForSeconds relic_RawImg_intervalTime = new WaitForSeconds(0.22f);
    WaitForSeconds relic_RawImg_NextWaitTime = new WaitForSeconds(3f);
    WaitForSeconds relic_RawImg_FirstNextWaitTime = new WaitForSeconds(1f);
    IEnumerator PlayRelicAction(bool isAd)
    {
        WorldUI_Manager.inst.Effect_WhiteCutton(1.5f); // �Ͼ�ȭ�� ����Ʈ
        WorldUI_Manager.inst.RawImagePlayAcitve(2, true);
        rellicRowIMG.SetActive(true);
        yield return startWaitTime;

        // ���� ���� ����
        for (int index = 0; index < arr.Count; index++)
        {
            Raw_Prefabs obj = rawPrefabsQue.Dequeue();
            obj.Set_Prefabs(arr[index], index);
            yield return relic_RawImg_intervalTime;
        }

        yield return relic_RawImg_FirstNextWaitTime;

        completePsRef.SetActive(true); // ���� ��ƼŬ ���

        yield return relic_RawImg_NextWaitTime;

        gachaResultRef[1].SetActive(true);

        yield return startWaitTime;

        // Result ����
        for (int index = 0; index < arr.Count; index++)
        {
            Relic_Result_Prefabs obj = relicResultQue.Dequeue();
            obj.Set_RelicResultPrefabs(arr[index]);
            yield return intervalTime;
        }

        yield return endWaitTime;

        if (isAd == false)
        {
            //�ѹ��� �ҷ� ��ư
            int curPrice = arr.Count == 10 ? RubyPrice.inst.RelicGachaPrice(0) : RubyPrice.inst.RelicGachaPrice(1);
            if (GameStatus.inst.Ruby >= curPrice)
            {
                priceTextInRestarBtn[1].text = $"x {curPrice}";
                reStartBtn[1].gameObject.SetActive(true);
            }
        }
        
        xBtn[1].gameObject.SetActive(true);
    }


    // Ǯ������

    public void CrewMaterial_ReturnObj_ToPool(CrewMaterial_GachaPrefabs obj)
    {
        obj.gameObject.SetActive(false);
        crewMaterialQue.Enqueue(obj);
    }

    public void Relic_ReturnObj_ToPool(Raw_Prefabs obj)
    {
        obj.gameObject.SetActive(false);
        rawPrefabsQue.Enqueue(obj);
    }

    public void Relic_ReturnObj_ToPool(Relic_Result_Prefabs obj)
    {
        obj.gameObject.SetActive(false);
        relicResultQue.Enqueue(obj);
    }

    // ���� �ε�� ��ư �ʱ�ȭ
    public void ResetAdBtn(string[] dateValue)
    {
        if (dateValue[0] != string.Empty)
        {
            DateTime materialLastDate = DateTime.Parse(dateValue[0]);
            
            if (materialLastDate.Date < DateTime.Now.Date)
            {
                crewMaterialBuyBtn[2].interactable = true;
            }
            else if (materialLastDate.Date <= DateTime.Now.Date)
            {
                crewMaterialBuyBtn[2].interactable = false;
            }
        }
        else
        {
            crewMaterialBuyBtn[2].interactable = true;
        }

        if (dateValue[1] != string.Empty)
        {
            DateTime relicLastDate = DateTime.Parse(dateValue[1]);

            if (relicLastDate.Date < DateTime.Now.Date)
            {
                relicBuyBtn[2].interactable = true;
            }
            else if (relicLastDate.Date <= DateTime.Now.Date)
            {
                relicBuyBtn[2].interactable = false;
            }
        }
        else
        {
            relicBuyBtn[2].interactable = true;
        }
    }
}
