using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewGatchaContent : MonoBehaviour
{
    public static CrewGatchaContent inst;

    int[] materialCount = new int[3]; // ��ȥ , ��, å

    //Ref �������̹���
    [Header("# Input Material <Color=yellow>( Sprite File )</Color>")]
    [Space]
    [SerializeField] Sprite[] crewMaterialItemIMG;

    //Ref
    GameObject frontUi, crewGatchaRef, window;
    GameObject boxLayout; // ���� �ڽ���

    // �ڿ���Ȳâ
    TMP_Text[] materialCountText = new TMP_Text[3];

    //Btn
    Button xBtn;
    Button allOpenBtn;
    Button closeBtn;

    // ���Ź�ư
    GameObject gatchaBoxBg;

    // ��í �ڽ���
    
    Button[] gatchaBox;
    BoxPrefabs[] gatchaBoxSc;


    //1ȸ�̱�, 5ȸ 9ȸ ����
    int openCount; //���� �� Ƚ��
    int setCount;
    public int OpenCount
    {
        get { return openCount; }
        set
        {
            openCount = value;

            if (setCount == openCount && setCount > 0) // ���ڸ� ��Ƚ���� ��� Ƚ���� ���ٸ� Ȯ�� ��ư �˾�
            {
                StartCoroutine(PopupCloseWindow());
                
            }
        }
    }

    int createBoxCount;
    public int CreateBoxCount
    {
        get { return createBoxCount; }
        set
        {
            createBoxCount = value;
            if (createBoxCount == setCount)
            {
                allOpenBtn.gameObject.SetActive(true);
            }
        }
    }

    // ���Ź�ư
    Button[] buyBtn = new Button[4];
    GameObject noCoolBtnRef;
    GameObject coolBtnRef;
    TMP_Text adCoolTImeText;

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

        frontUi = GameManager.inst.FrontUiRef;
        crewGatchaRef = frontUi.transform.Find("CrewMaterialGatcha").gameObject;
        window = crewGatchaRef.transform.Find("Window").gameObject;
        boxLayout = window.transform.Find("Main/Box_LayOut").gameObject;

        gatchaBoxBg = window.transform.Find("Main/Don'tClick").gameObject;
        gatchaBox = boxLayout.transform.GetComponentsInChildren<Button>(true);
        gatchaBoxSc = boxLayout.transform.GetComponentsInChildren<BoxPrefabs>(true);

        //�ڿ���Ȳ
        for (int index = 0; index < materialCountText.Length; index++)
        {
            materialCountText[index] = window.transform.Find("Material").GetChild(index).GetChild(1).GetComponent<TMP_Text>();
        }

        // ��ư��
        xBtn = window.transform.Find("Title/X_Btn").GetComponent<Button>();
        allOpenBtn = window.transform.Find("Main/AllOpenBtn").GetComponent<Button>();
        closeBtn = window.transform.Find("Main/CloseBtn").GetComponent<Button>();

        //���Ź�ư
        for (int index = 0; index < buyBtn.Length; index++)
        {
            buyBtn[index] = window.transform.Find("Bottom_Btn").GetChild(index).GetComponent<Button>();
        }

        noCoolBtnRef = buyBtn[3].transform.Find("noCoolTime").gameObject;
        coolBtnRef = buyBtn[3].transform.Find("CoolTime").gameObject;
        adCoolTImeText = coolBtnRef.transform.GetChild(1).GetComponent<TMP_Text>();

        BtnInit();
        MaterialTextBarUpdate(); // ���� �޾ƿ°� �ʱ�ȭ
    }

    void Start()
    {

    }

    private void Update()
    {
        AdviewTimeChaker();
    }

    WaitForSeconds waitPopup = new WaitForSeconds(1.25f);
    IEnumerator PopupCloseWindow()
    {
        allOpenBtn.gameObject.SetActive(false);
        yield return waitPopup;
        closeBtn.gameObject.SetActive(true);
    }

    private void BtnInit()
    {
        xBtn.onClick.AddListener(() =>
        {
            ShopManager.Instance.SetShopActive(false);
            CrewMaterialGatchaActive(false);
        });

        // ��� ����
        allOpenBtn.onClick.AddListener(() =>
        {
            AllBoxOpen(setCount);
            StartCoroutine(PopupCloseWindow());
        });


        //  Ȯ�� (���⼭ �ʱ�ȭ �� Disable �ص��ɵ�)
        closeBtn.onClick.AddListener(() =>
        {
            setCount = 0;
            OpenCount = 0;
            createBoxCount = 0;
            AllBoxDisable(); // ������ ��� Disable ó��

            closeBtn.gameObject.SetActive(false);
        });

        // 1ȸ �̱� / 100��
        buyBtn[0].onClick.AddListener(() =>
        {
            RubyPayment.inst.RubyPaymentUiActive(100, () => ActiveGatchaBox(1));
        });

        // 5ȸ �̱� / 400��
        buyBtn[1].onClick.AddListener(() =>
        {
            RubyPayment.inst.RubyPaymentUiActive(400, () => ActiveGatchaBox(5));
        });

        // 9ȸ �̱� / 700��
        buyBtn[2].onClick.AddListener(() =>
        {
            RubyPayment.inst.RubyPaymentUiActive(700, () => ActiveGatchaBox(9));
        });

        // ���� ���� 3ȸ �̱� / 100�� // 15�� ��Ÿ�� �߰�
        buyBtn[3].onClick.AddListener(() =>
        {
            ADViewManager.inst.SampleAD_Active_Funtion(() => { ActiveGatchaBox(3); AdViewCoolTime(15); });
        });

    }


    /// <summary>
    /// ���� ��ȭ��� �̱� UI Active �Լ�
    /// </summary>
    /// <param name="value"> true / false </param>
    public void CrewMaterialGatchaActive(bool value)
    {
        if (value)
        {
            MaterialTextBarUpdate(); // ������ ���� �ֽ�ȭ
            crewGatchaRef.SetActive(true);
        }
        else
        {
            crewGatchaRef.SetActive(false);
        }
    }


    /// <summary>
    /// �ڿ��߰� �Լ� (�ڵ�������Ʈ�Լ� ����)
    /// </summary>
    /// <param name="index"> 0��ȥ / 1�� / 2 å</param>
    /// <param name="Value"> ������ ���� </param>
    public void MaterialCountEditor(int index, int Value)
    {
        materialCount[index] += Value;
        MaterialTextBarUpdate();
    }

    /// <summary>
    /// ���� �ڿ� �Ҹ� (for ����)
    /// </summary>
    /// <param name="index"> 0��ȥ / 1 �� / 2 å</param>
    /// <param name="Value"> �Ҹ� </param>
    public void Use_Crew_Material(int index, int Value)
    {
        if (materialCount[index] - Value < 0)
        {
            return;
        }
        materialCount[index] -= Value;
    }

    //�ڿ�â ������Ʈ
    public void MaterialTextBarUpdate()
    {
        materialCountText[0].text = materialCount[0].ToString();
        materialCountText[1].text = materialCount[1].ToString();
        materialCountText[2].text = materialCount[2].ToString();
    }

    /// <summary>
    /// Load�� SaveData Setting �� �Ű����� 3��
    /// </summary>
    /// <param name="soul"></param>
    /// <param name="bone"></param>
    /// <param name="book"></param>
    public void Set_CrewMeterialData(int soul, int bone, int book)
    {
        materialCount[0] = soul;
        materialCount[1] = bone;
        materialCount[2] = book;
        MaterialTextBarUpdate();
    }

    /// <summary>
    /// ���� ���ᰭȭ��� �޾ư��� �Լ�
    /// </summary>
    /// <returns></returns>
    public int[] Get_CurCrewUpgreadMaterial() => materialCount;


    // ���Ž� ���� Ȱ��ȭ

    private void ActiveGatchaBox(int count)
    {
        setCount = count;
        gatchaBoxBg.gameObject.SetActive(true); // ��׶��� ����
        StartCoroutine(BoxAction(count));
    }

    // ���� ���� ��Ÿ��
    WaitForSeconds boxincount = new WaitForSeconds(0.2f);
    IEnumerator BoxAction(int count)
    {
        for (int index = 0; index < count; index++)
        {
            //������ ����
            int ranType = Random.Range(0, 3);

            int criticalDice = Random.Range(0, 100);
            
            int ranitemCount = 0;

            if (criticalDice <= 5) // 5% Ȯ���� ��� ��Ƣ��
            {
                ranitemCount = Random.Range(150, 300);
            }
            else if(criticalDice > 5)
            {
                ranitemCount = Random.Range(20, 99);
            }


            //�� �ʱ�ȭ
            gatchaBoxSc[index].Set_MaterialCount(crewMaterialItemIMG[ranType], ranType, ranitemCount);
            gatchaBox[index].gameObject.SetActive(true);
            CreateBoxCount++;
            yield return boxincount;
        }

        yield return boxincount;

        allOpenBtn.gameObject.SetActive(true);
    }


    // ��� ����
    private void AllBoxOpen(int count)
    {
        for (int index = 0; index < count; index++)
        {
            gatchaBoxSc[index].OpenBox();
        }
    }

    private void AllBoxDisable()
    {
        if (gatchaBoxBg.gameObject.activeSelf)
        {
            gatchaBoxBg.gameObject.SetActive(false); // ��沨��
        }

        for (int index = 0; index < gatchaBoxSc.Length; index++)
        {
            gatchaBoxSc[index].gameObject.SetActive(false);
        }
    }
    // ��� Ȯ�ν� Ȯ�ι�ư Ȱ��ȭ & ��μ�����ư ���ֱ�

    float coolTime;
    /// <summary>
    /// ���� �̱� ��Ÿ�� �ð� ü���ִ� �Լ�
    /// </summary>
    /// <param name="Min"> Time = Min </param>
    private void AdViewCoolTime(float Min)
    {
        coolTime = Min * 60;
    }

    /// <summary>
    /// ���� ��Ÿ�� üĿ
    /// </summary>
    private void AdviewTimeChaker()
    {
        if (coolTime <= 0) // �⺻
        {
            if (noCoolBtnRef.activeSelf == false)
            {
                coolTime = 0;
                coolBtnRef.SetActive(false);
                noCoolBtnRef.SetActive(true);
                buyBtn[3].interactable = true;
            }
        }
        else if (coolTime > 0)  // �𵹶�
        {
            if (coolBtnRef.activeSelf == false)
            {
                coolBtnRef.SetActive(true);
                noCoolBtnRef.SetActive(false);
                buyBtn[3].interactable = false;
            }

            coolTime -= Time.deltaTime;
            int min = (int)coolTime / 60;
            int sec = (int)coolTime % 60;
            adCoolTImeText.text = $"{min} : {sec}";
        }
    }
}
