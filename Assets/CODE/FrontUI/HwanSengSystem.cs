using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HwanSengSystem : MonoBehaviour
{
    public static HwanSengSystem inst;

    // Ref
    GameObject frontUI, worldUI, hwansengRef, mainWindowRef;

    // Title
    Button xBtn;

    // ĳ���͵޸� ������
    Image charBg;
    float rotZ;

    // ���ν���â
    Button starBusterBtn;
    GameObject starBusterRef;
    Button exitStartBusterBtn;
    Button buyRubyStartBuster;
    bool activeStarbuster = false; // ���Ž� ���� ����
    GameObject alreadyBuyAlrimWindow;
    Button alreadyBuyAlrimWindowBackBtn;
    TMP_Text alreadyBuyWindowMainText;

    // �� �����̹��� �ϴ� Text
    Button goldKeyBtn;
    Button floorKeyBtn;

    TMP_Text goldkeyLvText;
    TMP_Text stageLvUpkeyText;
    TMP_Text starBusterValueText;

    GameObject btnInfoWindow;
    TMP_Text btnInfoText;
    Image passiveIconBg_L; // ���� ���� �̹���
    Image passiveIconBg_M;// ��� ������ ���� �̹���

    // UI�ϴ� ȯ�� ��ư3����
    Button maxHwansengStart;
    Button middelHwansengStart;
    Button normallHwansengStart;

    // ���� ȯ���ϱ� ��ưâ
    GameObject lastHwansengRef;
    Image[] gagebar;
    Button freeUpgradeHwansengBtn;
    int adViewrCount;

    Button hwanSengxBtn;

    //ȯ�� ���۹�ư , ������۹�ư
    Button hwanSengYesBtn;
    Button hwanSengAdYesBtn;
    TMP_Text hwansengWindowTitleText;

    // �߰� ���޿��� ���ؽ�Ʈ
    TMP_Text getStarCountViewrText;

    // ���� ȭ�� �����ϴ� ȯ����ư Return Star�� Text
    TMP_Text hwansengIconReturnValueText;

    // ��� ���� �˸�â
    GameObject failBuyRubyRef;
    Button failRubyBackBtn;
    Button goRubyStroeBtn;

    // �ǹ�Ÿ��
    Animator feverAnim;
    Image feverFrontImg;
    float feverCountTimer;

    int goldkeyLv;
    public int GoldKeyLv_Hwanseng
    {
        get { return goldkeyLv; }
        set { goldkeyLv = value; }
    }

    int stageLvUpkeyLv;
    public int StageLvUpkeyLv_Hwanseng
    {
        get { return stageLvUpkeyLv; }
        set { stageLvUpkeyLv = value; }
    }

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

        //Ref
        frontUI = GameObject.Find("---[FrontUICanvas]").gameObject;
        worldUI = GameObject.Find("---[World UI Canvas]").gameObject;

        hwansengRef = frontUI.transform.Find("Hwnaseng").gameObject;
        mainWindowRef = hwansengRef.transform.Find("Window").gameObject;
        starBusterRef = hwansengRef.transform.Find("StarBuster").gameObject;
        starBusterBtn = mainWindowRef.transform.Find("BuffViewr/iConLayOut_R").GetComponent<Button>();


        //Main Art
        charBg = mainWindowRef.transform.Find("MainArt/Bg").GetComponent<Image>();

        //Btn
        xBtn = mainWindowRef.transform.Find("Title/X_Btn").GetComponent<Button>();

        // �߰�ȹ�淮â ������â
        btnInfoWindow = mainWindowRef.transform.Find("BtnInfoWindow").gameObject;
        btnInfoText = btnInfoWindow.transform.Find("Text").GetComponent<TMP_Text>();
        goldKeyBtn = mainWindowRef.transform.Find("BuffViewr/iConLayOut_L").GetComponent<Button>();
        passiveIconBg_L = goldKeyBtn.transform.Find("Bg").GetComponent<Image>();

        floorKeyBtn = mainWindowRef.transform.Find("BuffViewr/iConLayOut_C").GetComponent<Button>();
        passiveIconBg_M = floorKeyBtn.transform.Find("Bg").GetComponent<Image>();

        getStarCountViewrText = mainWindowRef.transform.Find("Bot_Box/Text").GetComponent<TMP_Text>();

        // �⺻â �ϴ� �ʰ�ȭȯ��, ��ȭȯ��, �Ϲ�ȯ��
        maxHwansengStart = mainWindowRef.transform.Find("Btn/LeftBtn").GetComponent<Button>();
        middelHwansengStart = mainWindowRef.transform.Find("Btn/MiddleBtn").GetComponent<Button>();
        normallHwansengStart = mainWindowRef.transform.Find("Btn/RightBtn").GetComponent<Button>(); ;

        // ���ν��� Ȯ��â
        exitStartBusterBtn = starBusterRef.transform.Find("Box/ExitBtn").GetComponent<Button>();
        buyRubyStartBuster = starBusterRef.transform.Find("Box/BuyRuby").GetComponent<Button>();
        alreadyBuyAlrimWindow = hwansengRef.transform.Find("AlreadyBuyBuster").gameObject;
        alreadyBuyAlrimWindowBackBtn = alreadyBuyAlrimWindow.transform.Find("Box/Btns/BackBtn").GetComponent<Button>();
        alreadyBuyWindowMainText = alreadyBuyAlrimWindow.transform.Find("Box/Main/Option0/Text (TMP)").GetComponent<TMP_Text>();

        // ���� ȯ���ϱ� â
        lastHwansengRef = hwansengRef.transform.Find("LastCheck").gameObject;
        hwanSengxBtn = lastHwansengRef.transform.Find("Box/Title/X_Btn").GetComponent<Button>();
        hwansengWindowTitleText = hwanSengxBtn.transform.parent.Find("TitleText").GetComponent<TMP_Text>();

        gagebar = lastHwansengRef.transform.Find("Box/Main/Option1/Bar").GetComponentsInChildren<Image>(); // ����Ƚ��
        freeUpgradeHwansengBtn = lastHwansengRef.transform.Find("Box/Main/Option1/FreeHwansengBtn").GetComponent<Button>(); // ���ᰭȭ ��ư

        //ȯ�� ���۹�ư , ������۹�ư
        hwanSengYesBtn = lastHwansengRef.transform.Find("Box/Btns/Yes").GetComponent<Button>();
        hwanSengAdYesBtn = lastHwansengRef.transform.Find("Box/Btns/Ad_Yes").GetComponent<Button>();

        // WorldUI ȯ�������� �ؽ�Ʈ
        hwansengIconReturnValueText = worldUI.transform.Find("StageUI/HwanSeng/Box/CurStarText").GetComponent<TMP_Text>();
        WorldUIHwansengIconReturnStarUpdate();

        // failBuyRubyRef
        failBuyRubyRef = hwansengRef.transform.Find("FailBuyRuby").gameObject;
        failRubyBackBtn = failBuyRubyRef.transform.Find("Box/Btns/BackBtn").GetComponent<Button>();
        goRubyStroeBtn = failBuyRubyRef.transform.Find("Box/Btns/GoRubyStoreBtn").GetComponent<Button>();

        // �߰� ������ �ϴܿ� ��ġ �ؽ�Ʈ
        goldkeyLvText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_L/Text").GetComponent<TMP_Text>();
        stageLvUpkeyText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_C/Text").GetComponent<TMP_Text>();
        starBusterValueText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_R/Text").GetComponent<TMP_Text>();




        // �ǹ�Ÿ��
        feverAnim = worldUI.transform.Find("Fever").GetComponent<Animator>();
        feverFrontImg = feverAnim.transform.Find("TimeBG/FRONT").GetComponent<Image>();


        BtnInIt();
    }


    private void Update()
    {
        if (hwansengRef.activeSelf) // ��� ������ ȸ��
        {
            rotZ += Time.deltaTime * 10;
            rotZ = Mathf.Repeat(rotZ, 360);
            charBg.transform.localRotation = Quaternion.Euler(0, 0, rotZ);
        }
    }

    private void BtnInIt()
    {
        alreadyBuyAlrimWindowBackBtn.onClick.AddListener(() => alreadyBuyAlrimWindow.SetActive(false)); // �̹̱����޾�� �ڷΰ���â

        xBtn.onClick.AddListener(() =>
        {
            Set_HwansengUIActive(false);
        });

        starBusterBtn.onClick.AddListener(() =>  // ���ν��� ���Ź�ư
        {
            if (activeStarbuster == false)
            {
                StartBusterWindowActive(true);
            }
            else
            {
                AlreadyBuyActive(1, true); // �̹� ���ſϷ�
            }

        });

        exitStartBusterBtn.onClick.AddListener(() => // �� �ν��� �ڷΰ���
        {
            StartBusterWindowActive(false);
        });

        buyRubyStartBuster.onClick.AddListener(() => // ���ν��� ���� ��ư
        {
            int curRuby = GameStatus.inst.Ruby;
            int busterValue = 300;
            if (curRuby >= busterValue) // ���ſϷ�
            {
                GameStatus.inst.Ruby -= busterValue;
                activeStarbuster = true;
                StartBusterWindowActive(false);
                UIValueBuffUpdate();
                AlreadyBuyActive(0, true);
            }
            else
            {
                StartBusterWindowActive(false);
                NoHaveRubyAlrimWindowActive(true);
            }


        });

        // ����â �ϴ� ȯ���ϱ� ��ư 3��
        maxHwansengStart.onClick.AddListener(() =>
        {
            // �ʰ�ȭ ȯ��
            // ���� ��� - ���� ��� üũ
            int curRuby = GameStatus.inst.Ruby;
            int needRuby = 1000;

            if (curRuby >= needRuby)// true�� �Ѿ��
            {
                LastCheckWindowActive(true, 0);
                LastCheckBtnInit(30, 0); // ���� ȯ���ϱ� , ������ ȯ���ϱ� ��ư �ʱ�ȭ (�� ���޷� ����� �ٸ�)
            }
            else if (curRuby < needRuby)// false�� �������� �̵� �ʿ�
            {
                NoHaveRubyAlrimWindowActive(true);
                Debug.Log($"��� {needRuby - curRuby} ����");
            }


        });


        middelHwansengStart.onClick.AddListener(() =>
        {
            // ��ȭ ȯ��
            // ���� ��� - ���� ��� üũ
            int curRuby = GameStatus.inst.Ruby;
            int needRuby = 500;

            if (curRuby >= needRuby)// true�� �Ѿ��
            {
                LastCheckWindowActive(true, 1);
                LastCheckBtnInit(30, 1); // ���� ȯ���ϱ� , ������ ȯ���ϱ� ��ư �ʱ�ȭ (�� ���޷� ����� �ٸ�)
            }
            else if (curRuby < needRuby)// false�� �������� �̵� �ʿ�
            {
                NoHaveRubyAlrimWindowActive(true);
            }


        });

        normallHwansengStart.onClick.AddListener(() =>
        {
            LastCheckWindowActive(true, 2);
            LastCheckBtnInit(30, 2); // ���� ȯ���ϱ� , ������ ȯ���ϱ� ��ư �ʱ�ȭ (�� ���޷� ����� �ٸ�)
        });

        // ����ȯ���ϱ� ���� ��ư

        hwanSengxBtn.onClick.AddListener(() => LastCheckWindowActive(false, 0));

        // ��� �����˸�â �ڷΰ����ư
        failRubyBackBtn.onClick.AddListener(() =>
        {
            NoHaveRubyAlrimWindowActive(false);
        });

        // ��� �����˸�â �������� �̵� ��ư
        goRubyStroeBtn.onClick.AddListener(() =>
        {
            NoHaveRubyAlrimWindowActive(false);
            Set_HwansengUIActive(false);
            ShopManager.Instance.OpenRubyShop();
        });

        // ���� ��ȭ ȯ����ư 
        freeUpgradeHwansengBtn.onClick.AddListener(() =>
        {
            LastCheckWindowActive(false, 1); // ����Ȯ��â ����
            Set_HwansengUIActive(false); // ����â����
            LastCheckWindowAdViewrCountInit(false); // ����â�� ���ᱤ����� �׸�� ����
            FeverTimeActive(30, 1, false); // �ǹ�Ÿ��
        });

        // ȯ������Ʈ ������ ��ư
        goldKeyBtn.onClick.AddListener(() => BtnInfoWindowActive(0, true, goldKeyBtn.transform.localPosition));

        floorKeyBtn.onClick.AddListener(() => BtnInfoWindowActive(1, true, floorKeyBtn.transform.localPosition));

    }



    /// <summary>
    /// ȯ��UI ȣ�� 
    /// </summary>
    /// <param name="active"></param>
    public void Set_HwansengUIActive(bool active)
    {
        UIValueBuffUpdate(); // ����â �߰� �нú귮 �� �ؽ�Ʈ�� �ʱ�ȭ
        hwansengRef.SetActive(active);
    }




    /// <summary>
    /// �߰��� ���� �нú귮 UI Text ��������
    /// </summary>
    /// <param name="goldkeyValue"></param>
    /// <param name="stageLvUpValue"></param>
    public void UIValueBuffUpdate()
    {
        // ���Ű�� �ʱ�ȭ
        int goldKeyValue = GoldKeyLv_Hwanseng * 10; // �������� * 10%
        passiveIconBg_L.gameObject.SetActive(goldKeyValue == 0 ? true : false);

        // �߰� ���� �ʱ�ȭ
        int stageLvUpValue = StageLvUpkeyLv_Hwanseng * 5; // �������� * 5��
        passiveIconBg_M.gameObject.SetActive(stageLvUpValue == 0 ? true : false);

        // �������ִ� ���� üũ
        string curStar = CalCulator.inst.CurHwansengPoint(stageLvUpValue); // ���� ���޹������ִ� ȭ�� üũ
        goldkeyLvText.text = $"{goldKeyValue}% ����";
        stageLvUpkeyText.text = $"{stageLvUpValue}�� ����";

        if (activeStarbuster) // ���ν��ͽ� 30% ��ŭ ����
        {
            starBusterValueText.text = "30%";
            curStar = CalCulator.inst.DigitPercentMultiply(curStar, 30);
        }
        else
        {
            starBusterValueText.text = "0%";
        }

        getStarCountViewrText.text = $"{CalCulator.inst.StringFourDigitAddFloatChanger(curStar)} ( Stage{GameStatus.inst.StageLv} + ���� �߰� {stageLvUpValue}�� )";
    }



    /// <summary>
    /// ���ν��� ��ư â ȣ��
    /// </summary>
    /// <param name="active"></param>
    public void StartBusterWindowActive(bool active)
    {
        starBusterRef.SetActive(active);
    }




    /// <summary>
    /// ������â ȣ��
    /// </summary>
    /// <param name="active"></param>
    private void NoHaveRubyAlrimWindowActive(bool active)
    {
        failBuyRubyRef.SetActive(active);
    }




    /// <summary>
    /// ���� ȯ�� Ȯ��â
    /// </summary>
    /// <param name="active"> true / false </param>
    /// <param name="invokeType"> 0 : �ʰ�ȭ / 1 : ��ȭ / 2 : �Ϲ� </param>
    public void LastCheckWindowActive(bool active, int invokeType)
    {
        string titleTextData = string.Empty;
        int curRuby = GameStatus.inst.Ruby;

        switch (invokeType)
        {
            case 0:
                titleTextData = "�ʰ�ȭ ȯ���ϱ�";
                break;

            case 1:
                titleTextData = "��ȭ ȯ���ϱ�";
                break;

            case 2:
                titleTextData = "ȯ���ϱ�";
                break;
        }

        hwansengWindowTitleText.text = titleTextData;
        LastCheckWindowAdViewrCountInit(true); // ����Ƚ�� �� ��ư �ʱ�ȭ
        lastHwansengRef.SetActive(active);
    }





    Coroutine btnInfoCo;

    /// <summary>
    /// ȯ������Ʈ ������ Ŭ���� ����â ȣ��
    /// </summary>
    /// <param name="type"> 0 ���Ű / 1 ����Ű</param>
    /// <param name="active">true / false</param>
    /// <param name="InvokePos"> ȣ�� Ʈ������ ��ġ</param>
    private void BtnInfoWindowActive(int type, bool active, Vector3 InvokePos)
    {
        if (btnInfoWindow.activeSelf == true)
        {
            btnInfoWindow.SetActive(false);
        }

        string textInfo = string.Empty;

        switch (type)
        {
            case 0:
                textInfo = "���Ű\r\nȯ���� ����ȹ�淮 ����\r\n���� �޴����� ���Ű���";
                break;

            case 1:
                textInfo = "����Ű\r\nȯ���� Ŭ���� ���� ����\r\n���� �޴����� ���Ű���";
                break;
        }

        btnInfoText.text = textInfo;
        btnInfoWindow.transform.localPosition = InvokePos;
        btnInfoWindow.SetActive(active);


        if (btnInfoCo != null)
        {
            StopCoroutine(btnInfoCo);
        }
        btnInfoCo = StartCoroutine(BtnInfoActiveFalse());

    }
    IEnumerator BtnInfoActiveFalse()
    {
        yield return new WaitForSeconds(3);
        btnInfoWindow.SetActive(false);
    }




    // ȯ�������� �� �ֽ�ȭ ��������
    public void WorldUIHwansengIconReturnStarUpdate()
    {
        string curReturnStarCount = CalCulator.inst.CurHwansengPoint(0);
        curReturnStarCount = CalCulator.inst.StringFourDigitAddFloatChanger(curReturnStarCount);
        hwansengIconReturnValueText.text = curReturnStarCount;
    }




    /// <summary>
    /// FeverTime Setting
    /// </summary>
    /// <param name="InputTime"> �� </param>
    /// <param name="hwansengType"> �ʰ�ȭ, ��ȭ, �Ϲ�</param>
    /// <param name="isAd"></param>
    public void FeverTimeActive(float InputTime, int hwansengType, bool isAd)
    {
        //�ʱ�ȭ
        feverCountTimer = InputTime;
        feverFrontImg.fillAmount = 1;

        // �� ����
        string addStarValue = CalCulator.inst.DigidPlus(GameStatus.inst.Star, CalCulator.inst.CurHwansengPoint(0));

        // ���� �ʱ�ȭ
        GameStatus.inst.HwansengPointReset();

        switch (hwansengType)
        {
            case 0: // �ʰ�ȭ
                addStarValue = CalCulator.inst.StringAndIntMultiPly(addStarValue, 10);
                break;

            case 1: // ��ȭ
                addStarValue = CalCulator.inst.StringAndIntMultiPly(addStarValue, 5);
                break;
        }

        if (isAd) // ����� 10% �߰�����
        {
            addStarValue = CalCulator.inst.DigitPercentMultiply(addStarValue, 10);
        }

        GameStatus.inst.Star = addStarValue;
        // ���
        StartCoroutine(FeverPlay(InputTime));
    }

    IEnumerator FeverPlay(float InputTime)
    {
        //����
        if (feverAnim.gameObject.activeSelf == false)
        {
            feverAnim.gameObject.SetActive(true);
            ActionManager.inst.IsFever = true;
        }

        // ȯ���ð� �ǹ���
        while (feverCountTimer > 0)
        {
            feverCountTimer -= Time.deltaTime;
            feverFrontImg.fillAmount = feverCountTimer / InputTime;
            yield return null;
        }

        feverCountTimer = 0;
        feverAnim.SetTrigger("Hide");
        ActionManager.inst.IsFever = false;
        yield return new WaitForSeconds(1f);
        feverAnim.gameObject.SetActive(false);

        feverAnim.SetTrigger("Exit");
    }

    private void LastCheckBtnInit(int Time, int Type)
    {
        // ���� ȯ�� ���۹�ư �ʱ�ȭ
        hwanSengYesBtn.onClick.RemoveAllListeners();
        hwanSengYesBtn.onClick.AddListener(() =>
        {
            LastCheckWindowActive(false, 0);
            Set_HwansengUIActive(false);

            FeverTimeActive(Time, Type, false);
        });

        // ���� ȯ�� ���� �� ���۹�ư 
        hwanSengAdYesBtn.onClick.RemoveAllListeners();
        hwanSengAdYesBtn.onClick.AddListener(() =>
        {
            LastCheckWindowActive(false, 0);
            Set_HwansengUIActive(false);

            adViewrCount++; //����Ƚ��
            if(adViewrCount >= 5)
            {
                adViewrCount = 5;
            }

            WorldUI_Manager.inst.SampleAD_Ad_FeverTIme(Time, Type, true); // ���ñ���
        });
    }

    /// <summary>
    /// ��������â ����Ƚ�� �� ��ư �ʱ�ȭ �Լ� �ʱ�ȭ �� ����
    /// </summary>
    /// <param name="value"> true ���� // false ���� </param>
    private void LastCheckWindowAdViewrCountInit(bool value)
    {
        if(value == true)
        {
            for (int index = 0; index < adViewrCount; index++)
            {
                gagebar[index].color = Color.green;
            }

            if(adViewrCount == 5)
            {
                freeUpgradeHwansengBtn.interactable = true;
            }
            else if(adViewrCount < 5)
            {
                freeUpgradeHwansengBtn.interactable = false;
            }

        }
        else if(value == false)  // ���±��
        {
            for (int index = 0; index < 5; index++)
            {
                gagebar[index].color = new Color(0.3f,0.3f,0.3f,0.5f);
            }
            adViewrCount = 0;
            freeUpgradeHwansengBtn.interactable = false;
        }
    }

    /// <summary>
    ///  �ν��� ���� �޼��� ���
    /// </summary>
    /// <param name="type"> 0 ���ſϷ� / 1 �̹� ��������</param>
    private void AlreadyBuyActive(int type, bool value)
    {
        alreadyBuyWindowMainText.text = type == 0 ? "�ν��Ͱ� ����Ǿ����ϴ�." : "�̹� �ν��Ͱ� ����Ǿ� �ֽ��ϴ�.";
        alreadyBuyAlrimWindow.SetActive(value);
    }

}
