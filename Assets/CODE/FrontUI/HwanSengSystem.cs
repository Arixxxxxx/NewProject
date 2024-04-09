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

    // �� �����̹��� �ϴ� Text
    TMP_Text keyPassiveUpText;
    TMP_Text stageLvUpPassiveText;
    TMP_Text totalStarValueUpPassiveText;

    // UI�ϴ� ��ư3����
    Button maxHwansengStart;
    Button middelHwansengStart;
    Button normallHwansengStart;

    // ���� ȯ���ϱ� ��ưâ
    GameObject lastHwansengRef;
    Button hwanSengxBtn;
    Button hwanSengYesBtn;
    Button hwanSengAdYesBtn;
    TMP_Text hwansengWindowTitleText;

    // ���� ȭ�� �����ϴ� ȯ����ư Return Star�� Text
    TMP_Text hwansengIconReturnValueText;

    // ��� ���� �˸�â
    GameObject failBuyRubyRef;
    Button failRubyBackBtn;
    Button goRubyStroeBtn;

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

        // �⺻â �ϴ� �ʰ�ȭȯ��, ��ȭȯ��, �Ϲ�ȯ��
        maxHwansengStart = mainWindowRef.transform.Find("Btn/LeftBtn").GetComponent<Button>();
        middelHwansengStart = mainWindowRef.transform.Find("Btn/MiddleBtn").GetComponent<Button>();
        normallHwansengStart = mainWindowRef.transform.Find("Btn/RightBtn").GetComponent<Button>(); ;

        // ���ν��� Ȯ��â
        exitStartBusterBtn = starBusterRef.transform.Find("Box/ExitBtn").GetComponent<Button>();
        buyRubyStartBuster = starBusterRef.transform.Find("Box/BuyRuby").GetComponent<Button>();

        // ���� ȯ���ϱ� â
        lastHwansengRef = hwansengRef.transform.Find("LastCheck").gameObject;
        hwanSengxBtn = lastHwansengRef.transform.Find("Box/Title/X_Btn").GetComponent<Button>();
        hwansengWindowTitleText = hwanSengxBtn.transform.parent.Find("TitleText").GetComponent<TMP_Text>();

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
        keyPassiveUpText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_L/Text").GetComponent<TMP_Text>();
        stageLvUpPassiveText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_C/Text").GetComponent<TMP_Text>();
        totalStarValueUpPassiveText = mainWindowRef.transform.Find("BuffViewr/iConLayOut_R/Text").GetComponent<TMP_Text>();

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
        xBtn.onClick.AddListener(() =>
        {
            Set_HwansengUIActive(false);
        });

        starBusterBtn.onClick.AddListener(() =>  // ����� ��ư
        {
            StartBusterWindowActive(true);
        });

        exitStartBusterBtn.onClick.AddListener(() => // �� �ν��� �ڷΰ���
        {
            StartBusterWindowActive(false);
        });

        buyRubyStartBuster.onClick.AddListener(() =>
        {
            // ȯ�� �߰����� 300�� �߰� => ��� 300 ���� ��ư
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
            }
            else if (curRuby < needRuby)// false�� �������� �̵� �ʿ�
            {
                NoHaveRubyAlrimWindowActive(true);
                Debug.Log($"��� {needRuby - curRuby} ����");
            }


        });

        normallHwansengStart.onClick.AddListener(() =>
        {
            LastCheckWindowActive(true, 2);
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
            // ���Ŀ� �����̵� �Լ� �־����
        });
    }

    public void Set_HwansengUIActive(bool active)
    {
        hwansengRef.SetActive(active);
    }

    public void StartBusterWindowActive(bool active)
    {
        starBusterRef.SetActive(active);
    }

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
        lastHwansengRef.SetActive(active);
    }

    // ȯ�������� �� �ֽ�ȭ ��������
    public void WorldUIHwansengIconReturnStarUpdate()
    {
        string curReturnStarCount = CalCulator.inst.CurHwansengPoint(0);
        curReturnStarCount = CalCulator.inst.StringFourDigitAddFloatChanger(curReturnStarCount);
        hwansengIconReturnValueText.text = curReturnStarCount;
    }
}
