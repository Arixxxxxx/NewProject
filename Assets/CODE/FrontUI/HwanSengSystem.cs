using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HwanSengSystem : MonoBehaviour
{
    public static HwanSengSystem inst;

    int selectType = 0;
    int adStarIncrease = 50;
    int rubyStarMultiPle = 3;
    string[] alrimTypeText = { "ȯ�� Ÿ�� : �Ϲ� ȯ��", "ȯ�� Ÿ�� : ���� ȯ��", "ȯ�� Ÿ�� : ��� ȯ��" };
    // Ref
    GameObject frontUI, worldUI, hwansengRef, mainWindowRef, rawImage;

    // Title
    Button xBtn;

    // ��ü �ʿ��� ������
    GameObject[] activeHwanseng = new GameObject[2];

    //�� �ؽ�Ʈ
    TMP_Text textBox_FloorInfoText, textBox_StarValueText;

    //���� ȭ�� ��ư 3��
    Button[] mainBtn;
    GameObject btnMask;

    //�˸�
    GameObject alrimWindowRef;
    Button[] alrimBtn;
    TMP_Text feverTimeTextSec;
    TMP_Text alrim_HwansengTypeText;
    TMP_Text worldUICenterTopText;

    //�˸��ȿ� �ǹ� ��
    float defaultFeverTime = 30f;
    float relicAddTime = 0;
    float totalFeverTime = 0;

    // ���� ȭ�� �����ϴ� ȯ����ư Return Star�� Text
    TMP_Text hwansengIconReturnValueText;

    // �ǹ�Ÿ��
    Animator feverAnim;
    Image feverFrontImg;
    float feverCountTimer;
    Material feverBG;
    Material feverFrontBGMat;


   
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
        frontUI = GameManager.inst.FrontUiRef;
        worldUI = GameManager.inst.WorldUiRef;
        rawImage = GameManager.inst.RawImgRef;
        hwansengRef = frontUI.transform.Find("Hwnaseng").gameObject;
        mainWindowRef = hwansengRef.transform.Find("Window").gameObject;

        worldUICenterTopText = worldUI.transform.Find("HwansengCount").GetComponent<TMP_Text>();

        //Btn
        xBtn = mainWindowRef.transform.Find("Title/X_Btn").GetComponent<Button>();
        mainBtn = mainWindowRef.transform.Find("Btn").GetComponentsInChildren<Button>();
        btnMask = mainWindowRef.transform.Find("Mask").gameObject;

        // �߰� �ؽ�Ʈ
        activeHwanseng[0] = mainWindowRef.transform.Find("Bot_Box/Active_True").gameObject;
        activeHwanseng[1] = mainWindowRef.transform.Find("Bot_Box/Active_False").gameObject;

        textBox_FloorInfoText = mainWindowRef.transform.Find("Bot_Box/Active_True/ToTalFloorText").GetComponent<TMP_Text>();
        textBox_StarValueText = mainWindowRef.transform.Find("Bot_Box/Active_True/GetStarText").GetComponent<TMP_Text>();

        // �˸�â
        alrimWindowRef = hwansengRef.transform.Find("Alrim").gameObject;
        alrimBtn = alrimWindowRef.transform.Find("Box/Btns").GetComponentsInChildren<Button>();
        feverTimeTextSec = alrimWindowRef.transform.Find("Box/FeverTime/FeverTimeText").GetComponent<TMP_Text>();
        alrim_HwansengTypeText = alrimWindowRef.transform.Find("Box/HwansengTypeText").GetComponent<TMP_Text>();
        // WorldUI ȯ�������� �ؽ�Ʈ
        hwansengIconReturnValueText = worldUI.transform.Find("StageUI/HwanSeng/Box/CurStarText").GetComponent<TMP_Text>();

        // �ǹ�Ÿ��
        feverAnim = worldUI.transform.Find("Fever").GetComponent<Animator>();
        feverFrontImg = feverAnim.transform.Find("TimeBG/FRONT").GetComponent<Image>();
        feverBG = feverAnim.transform.Find("BG").GetComponent<Image>().material;
        feverFrontBGMat = feverAnim.transform.Find("FrontBG").GetComponent<Image>().material;
        BtnInIt();

    }

    private void Start()
    {

    }

    Vector3 tillingVec;
    Vector3 tillingFrontVec;
    float tillingSpeedMultipleyr = 1.8f;
    float tillingFrontSpeedMultipleyr = 0.25f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) { FeverTimeActive(30, 0); }

        // ȯ�� �ִϸ��̼� ����� ��׶��� 
        FeverTime_AnimationUpdate();
    }

    // ȯ�� �ִϸ��̼� ����� ��׶��� 
    private void FeverTime_AnimationUpdate()
    {

        if (feverAnim.gameObject.activeInHierarchy)
        {
            tillingVec.x += Time.deltaTime * tillingSpeedMultipleyr;
            tillingVec.x = Mathf.Repeat(tillingVec.x, 1);
            feverBG.mainTextureOffset = tillingVec;

            tillingFrontVec.x += Time.deltaTime * tillingFrontSpeedMultipleyr;
            tillingFrontVec.x = Mathf.Repeat(tillingVec.x, 1);
            feverFrontBGMat.mainTextureOffset = tillingVec;
        }
    }
    private void BtnInIt()
    {

        xBtn.onClick.AddListener(() =>
        {
            Set_HwansengUIActive(false);
        });

        mainBtn[0].onClick.AddListener(() => // �Ϲ� ȯ��
        {
            selectType = 0;
            Set_AlrimWindowActive(true);
        });

        mainBtn[1].onClick.AddListener(() => // ���� ȯ��
        {
            selectType = 1;
            Set_AlrimWindowActive(true);
        });

        mainBtn[2].onClick.AddListener(() => // ��� ȯ��
        {
            selectType = 2;
            RubyPayment.inst.RubyPaymentOnlyFuntion(RubyPrice.inst.HwansengPrice, () => Set_AlrimWindowActive(true));
        });


        // �˸�â ��ư��
        alrimBtn[0].onClick.AddListener(() => // ���ư���
        {
            selectType = 0;
            Set_AlrimWindowActive(false);
        });

        alrimBtn[1].onClick.AddListener(() => // ���� ���� ��ư
        {
            switch (selectType)
            {
                case 0: // �� ����
                    FeverTimeActive(totalFeverTime, selectType);
                    break;

                case 1: // ���� �����ְ� ����
                    ADViewManager.inst.SampleAD_Active_Funtion(() => FeverTimeActive(totalFeverTime, selectType));
                    break;

                case 2: // ����â ��� �� ����
                    RubyPayment.inst.RubyPaymentUiActive(RubyPrice.inst.HwansengPrice, () => FeverTimeActive(totalFeverTime, selectType));
                    break;
            }

        });

        //; // �ǹ�Ÿ��   �ǹ�����
    }


    ///////////////////////////////////////////// UI ���ۺ� //////////////////////////////////////////////////////////


    int openFloor = 150;
    
    /// <summary>
    /// ȯ��UI ȣ�� 
    /// </summary>
    /// <param name="active"></param>
    public void Set_HwansengUIActive(bool active)
    {
        if (active)
        {
            if(GameStatus.inst.AccumlateFloor <= openFloor)
            {
                activeHwanseng[0].SetActive(false);
                activeHwanseng[1].SetActive(true);
                btnMask.SetActive(true);
            }
            else if(GameStatus.inst.AccumlateFloor > openFloor)
            {
                activeHwanseng[0].SetActive(true);
                activeHwanseng[1].SetActive(false);
                btnMask.SetActive(false);
            }
        }
        hwansengRef.SetActive(active);
    }


    /// <summary>
    /// �˸�â ȣ��
    /// </summary>
    /// <param name="active"></param>
    private void Set_AlrimWindowActive(bool active)
    {
        if (active)
        {
            relicAddTime = GameStatus.inst.GetAryRelicLv(4);
            totalFeverTime = defaultFeverTime + relicAddTime;
            feverTimeTextSec.text = $"{totalFeverTime}��";
            alrim_HwansengTypeText.text = alrimTypeText[selectType];
        }
        alrimWindowRef.SetActive(active);
    }




    /// <summary>
    /// FeverTime Setting
    /// </summary>
    /// <param name="InputTime"> �� </param>
    /// <param name="hwansengType"> �Ϲ� / ���� / ���</param>
    /// <param name="isAd"></param>
    public void FeverTimeActive(float InputTime, int hwansengType)
    {
        //�ʱ�ȭ
        Set_HwansengUIActive(false);
        Set_AlrimWindowActive(false);
        GameStatus.inst.HWansengCount++;

        feverCountTimer = InputTime; // �߰��ð� ���ϱ� ����
        feverFrontImg.fillAmount = 1;

        // �� ����
        string originalStarCount = CalCulator.inst.CurHwansengPoint();

        switch (hwansengType)
        {
            case 1: // ���� ������ 50%
                originalStarCount = CalCulator.inst.DigitAndIntPercentMultiply(originalStarCount, adStarIncrease);
                break;

            case 2: // ��� ������
                originalStarCount = CalCulator.inst.StringAndIntMultiPly(originalStarCount, rubyStarMultiPle);
                break;
        }

        // ���� �ʱ�ȭ
        GameStatus.inst.HwansengPointReset();
        GameStatus.inst.PlusStar(originalStarCount);
        selectType = 0;

        // ���
        StartCoroutine(FeverPlay(InputTime));
    }


    IEnumerator FeverPlay(float InputTime)
    {
        //����
        if (feverAnim.gameObject.activeSelf == false)
        {
            WorldUI_Manager.inst.Effect_WhiteCutton(); // ȭ�� �Ͼ�� ����Ʈ
            feverAnim.gameObject.SetActive(true);
            WorldUI_Manager.inst.RawImagePlayAcitve(0, true);
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

        WorldUI_Manager.inst.RawImagePlayAcitve(false);
        feverAnim.gameObject.SetActive(false);

        feverAnim.SetTrigger("Exit");
    }







    StringBuilder floorInfo = new StringBuilder();
    StringBuilder starValueInfo = new StringBuilder();

    /// <summary>
    /// ȯ�� ���޺� ���� �ؽ�Ʈ ��������
    /// </summary>
    public void MainWindow_TextBox_Updater()
    {
        floorInfo.Clear();
        floorInfo.Append($"�� Ŭ���� ���� : {GameStatus.inst.AccumlateFloor} ��");
        textBox_FloorInfoText.text = floorInfo.ToString();

        starValueInfo.Clear();
        starValueInfo.Append($"{CalCulator.inst.StringFourDigitAddFloatChanger(CalCulator.inst.CurHwansengPoint())}");
        textBox_StarValueText.text = starValueInfo.ToString();
        // ��������� Value��
        hwansengIconReturnValueText.text = starValueInfo.ToString();
    }

    public void Set_WorldHwansengCount_Text_Init(int value)
    {
        worldUICenterTopText.text = $"{value}��° ������ �⵿ �̾߱�";
    }

}
