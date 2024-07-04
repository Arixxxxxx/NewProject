using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI_Manager : MonoBehaviour
{
    public static WorldUI_Manager inst;

    [SerializeField] Sprite[] stageSprite;
    [Header("# Reward �� ��ȭȹ�� Ǯ�� ")]
    [SerializeField] GameObject[] poolingObj;
    UI_IncreaseValueFont[] minusMaterialFont;
    Queue<GameObject>[] poolingQue;

    Transform fontDanymic;
    Transform[] fontPoint = new Transform[2]; // Ǯ��������Ʈ ��ŸƮ����Ʈ �ʱ�ȭ��

    GameObject worldUI;
    Image[] stageSlot = new Image[5];
    Image uiBossHead;
    TMP_Text stageText;


    Animator cuttonBlack;

    // �ϴ� �ڿ���Ȳ�� �ؽ�Ʈ
    TMP_Text[] curMaterial = new TMP_Text[4];

    //����Ʈ ��� ����
    Button questListBtn;
    TMP_Text questListSideText;
    GameObject frontUICanvas;
    GameObject buffSelectUIWindow;

    // ������UI ����
    GameObject rewardRef;
    Reward_Parts[] rewards;
    int rewardChildCount;

    //���� ������

    Button getLetterBtn;

    // �⼮üũ
    Button dailyPlayCheckBtn;
    // �����ư
    Button newBieBtn;

    // ���θ޴� ��ư
    Button mainMenuBtn;

    // �̴ϰ��� ��ư 
    Button minigameAlrimBtn;

    //// ���͵��� ��ư  ( ���⵵���� �պ� 24.05.16 )
    //Button mosterDogamBtn;

    //ȯ����ư
    Button hwanSengBtn;

    // �������� ��ư
    Button adDeleteBtn;

    // ���� ���캸�� ��ư
    Button crewViewrBtn;

    // ���⵵����ư
    Button weaponShopBtn;

    // �̺�Ʈ�� ��ư
    Button eventShopBtn;

    // ������� ��ư
    Button bingoBtn;

    // ������
    Button buffShopBtn;
    Animator buffWindowAnim;

    //����ɺ� ����
    List<GameObject> redSimBall_Icons = new List<GameObject>();

    // �ﰢ�� ��ư
    Button openMenuIcon;
    Animator menuAnim;
    Transform checkArrowScaleX;

    // ����ƮUI �����̵� Ŀư
    Action cuttonAction;
    Image frontUiCutton;

    // ��ܹ�
    TMP_Text nickNameText;

    Image fakeScreen;

    // �ο��̹��� ������
    [SerializeField]
    GameObject[] rawCam;
    GameObject[] rawImageRef;

    // ����Ʈ Ŀư(�Ͼ��)
    Image whiteCutton;

    //������ȹ�� �ִϸ��̼�
    VerticalLayoutGroup getItemLayOut;
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }

        worldUI = GameManager.inst.WorldUiRef;

        frontUICanvas = GameManager.inst.FrontUiRef;

        //�ο��̹���
        int rawcamcount = GameManager.inst.CamsRef.transform.Find("RanderCams").childCount;
        rawCam = new GameObject[rawcamcount];
        for (int i = 0; i < rawcamcount; i++)
        {
            rawCam[i] = GameManager.inst.CamsRef.transform.Find("RanderCams").GetChild(i).gameObject;
        }

        int rawIMGCount = GameManager.inst.RawImgRef.transform.childCount;
        rawImageRef = new GameObject[rawIMGCount];
        for (int index = 0; index < rawIMGCount; index++)
        {
            rawImageRef[index] = GameManager.inst.RawImgRef.transform.GetChild(index).gameObject;
        }

        //�ε�� ����ũ ��ũ��
        fakeScreen = frontUICanvas.transform.Find("FakeScreen").GetComponent<Image>();

        //Ȥ�� ī�޶� ������ �ٽ� ä����
        worldUI.GetComponent<Canvas>().worldCamera = Camera.main;
        GameManager.inst.UiCanvasRef.GetComponent<Canvas>().worldCamera = Camera.main;
        frontUICanvas.GetComponent<Canvas>().worldCamera = Camera.main;



        //���������
        rewardRef = frontUICanvas.transform.Find("ReWard").gameObject;
        rewards = rewardRef.GetComponentsInChildren<Reward_Parts>(true);
        rewardChildCount = rewardRef.transform.childCount;


        //����â
        buffSelectUIWindow = frontUICanvas.transform.Find("Buff_Window").gameObject;
        buffWindowAnim = buffSelectUIWindow.GetComponent<Animator>();

        cuttonBlack = worldUI.transform.Find("Cutton(B)").GetComponent<Animator>();
        stageText = worldUI.transform.Find("StageUI/TopLayOut/StageInfo/Text").GetComponent<TMP_Text>();
        uiBossHead = worldUI.transform.Find("StageUI/TopLayOut/StageInfo/Boss").GetComponent<Image>();
        fontDanymic = worldUI.transform.Find("StageUI/Dyanamic").GetComponent<Transform>();
        getItemLayOut = worldUI.transform.Find("StageUI/Get").GetComponent<VerticalLayoutGroup>();

        for (int index = 0; index < stageSlot.Length; index++)
        {
            stageSlot[index] = worldUI.transform.Find("StageUI/TopLayOut/StageInfo").GetChild(index).GetComponent<Image>();
        }


        curMaterial[0] = worldUI.transform.Find("StageUI/MaterialBox/Gold/Text").GetComponent<TMP_Text>();
        curMaterial[1] = worldUI.transform.Find("StageUI/MaterialBox/Star/Text").GetComponent<TMP_Text>();
        curMaterial[3] = worldUI.transform.Find("StageUI/MaterialBox/Ruby/Text").GetComponent<TMP_Text>();
        nickNameText = worldUI.transform.Find("StageUI/TopLayOut/Profile/NickName").GetComponent<TMP_Text>();


        questListBtn = worldUI.transform.Find("StageUI/QeustList/Button").GetComponent<Button>();
        mainMenuBtn = worldUI.transform.Find("StageUI/MainMenu").GetComponent<Button>();
        mainMenuBtn.onClick.AddListener(() => { AudioManager.inst.Play_Ui_SFX(4, 0.8f); MainMenuManager.inst.Active_MainMenu(true); });

        hwanSengBtn = worldUI.transform.Find("StageUI/HwanSeng").GetComponent<Button>();

        // �޴���ư��

        getLetterBtn = worldUI.transform.Find("StageUI/Letter").GetComponent<Button>(); // ������
        dailyPlayCheckBtn = worldUI.transform.Find("StageUI/DailyCheck").GetComponent<Button>(); //�⼮üũ
        newBieBtn = worldUI.transform.Find("StageUI/NewBie").GetComponent<Button>(); //����
        //mosterDogamBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/MosterDogam").GetComponent<Button>(); //���͵���
        minigameAlrimBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/MiniGame").GetComponent<Button>(); //�̴ϰ���
        adDeleteBtn = worldUI.transform.Find("StageUI/AdDelete").GetComponent<Button>(); // ��������
        openMenuIcon = worldUI.transform.Find("StageUI/MenuBox/MeneOpen/RealBtn").GetComponent<Button>(); // �޴� �ﰢ����ư
        checkArrowScaleX = openMenuIcon.transform.parent.GetComponent<Transform>();
        buffShopBtn = worldUI.transform.Find("StageUI/BuffShop").GetComponent<Button>(); // ��������
        crewViewrBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/CrewViewr").GetComponent<Button>();      // ���� ���캸�� ��ư
        weaponShopBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/WeaponDogam").GetComponent<Button>();        // ���⵵����ư
        eventShopBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/EventShop").GetComponent<Button>();        // �̺�Ʈ�� ��ư
        bingoBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/Bingo").GetComponent<Button>(); // ������� ��ư

        menuAnim = worldUI.transform.Find("StageUI/MenuBox").GetComponent<Animator>(); // �޴��� ���� �ִϸޤ��̼�

        // FrontUI
        frontUiCutton = frontUICanvas.transform.Find("Cutton").GetComponent<Image>();

        // ȭ��ƮĿư ����Ʈ��
        whiteCutton = frontUICanvas.transform.Find("Cutton(W)").GetComponent<Image>();

        Prefabs_Awake();

    }



    void Start()
    {
        BtnInIt();
        menuAnim.SetTrigger("Open"); // �÷��̹� ������� ����
    }


    /// <summary>
    /// �������� UI�� ��ĥ
    /// </summary>
    /// <param name="curFloorLv"></param>
    public void Set_StageUiBar(int curFloorLv)
    {

        int setupCount = curFloorLv - 1;
        stageSlot[setupCount].sprite = stageSprite[2];
        stageText.text = $"�������� {GameStatus.inst.StageLv} - {GameStatus.inst.FloorLv}";

        // ���� �� ���
        uiBossHead.gameObject.SetActive(setupCount == 4 ? true : false);
        for (int index = 0; index < setupCount; index++)
        {
            stageSlot[index].sprite = stageSprite[1];
        }
    }




    /// <summary>
    /// ���������� �ʱ�ȭ 
    /// </summary>
    public void Reset_StageUiBar()
    {
        stageSlot[0].sprite = stageSprite[2];
        stageText.text = $"�������� {GameStatus.inst.StageLv} - {GameStatus.inst.FloorLv}";
        uiBossHead.gameObject.SetActive(false);

        for (int index = 1; index < stageSlot.Length; index++)
        {
            stageSlot[index].sprite = stageSprite[0];
        }
    }

    /// <summary>
    /// World���� ��� UI ���̵� Ŀư
    /// </summary>
    /// <param name="Value"></param>
    public void Set_Menual_WorldBlackCottun(bool Value)
    {
        cuttonBlack.SetTrigger(Value == true ? "FadeOn" : "FadeOff");
    }

    /// <summary>
    /// ����ȭ�� ���̵� �ξƿ� �Լ� 
    /// </summary>
    /// <param name="durationTime">���ӵǴ� �ð�</param>
    public void Set_Auto_BlackCutton(float durationTime)
    {
        StopCoroutine(StartCutton(durationTime));
        StartCoroutine(StartCutton(durationTime));
    }
    IEnumerator StartCutton(float durationTime)
    {
        cuttonBlack.SetTrigger("FadeOn");
        yield return new WaitForSeconds(durationTime);
        cuttonBlack.SetTrigger("FadeOff");

    }

    int weaponNum;

    private void BtnInIt()
    {

        // ���� ��ư �ʱ�ȭ
        hwanSengBtn.onClick.AddListener(() => { HwanSengSystem.inst.Set_HwansengUIActive(true); }); //ȯ����ư
        getLetterBtn.onClick.AddListener(() => { LetterManager.inst.OpenPostOnOfficeAndInit(true); });
        dailyPlayCheckBtn.onClick.AddListener(() => { DailyPlayCheckUIManager.inst.MainWindow_Acitve(true); });
        newBieBtn.onClick.AddListener(() => { Newbie_Content.inst.Set_NewbieWindowActive(true); });
        adDeleteBtn.onClick.AddListener(() => { AdDelete.inst.ActiveAdDeleteWindow(true); });
        buffShopBtn.onClick.AddListener(() => { BuffManager.inst.Buff_UI_Active(true); });
        crewViewrBtn.onClick.AddListener(() =>
        {
            PetDetailViewr_UI.inst.PetDetialviewrUI_Active(0);
        });

        weaponShopBtn.onClick.AddListener(() =>
        {
            DogamManager.inst.Active_DogamUI(true);
        });

        eventShopBtn.onClick.AddListener(() =>
        {
            EventShop_RulletManager.inst.Active_RulletEventShop(true);
        });

        // ���� ���� ���߿� ����
        bingoBtn.onClick.AddListener(() =>
        {

        });

        minigameAlrimBtn.onClick.AddListener(() =>
        {
            MinigameManager.inst.Active_minigameEntrance(true);
        });

        openMenuIcon.onClick.AddListener(() =>
        {
            if (checkArrowScaleX.localScale.x == 1)
            {
                AudioManager.inst.Play_Ui_SFX(2, 0.8f);
                menuAnim.SetTrigger("Open");
            }
            else if (checkArrowScaleX.localScale.x == -1)
            {
                AudioManager.inst.Play_Ui_SFX(3, 0.8f);
                menuAnim.SetTrigger("Close");
            }
        });
    }

    private void Prefabs_Awake()
    {
        int poolingCount = poolingObj.Length;
        minusMaterialFont = new UI_IncreaseValueFont[poolingCount];

        // Que �ʱ�ȭ        
        poolingQue = new Queue<GameObject>[poolingCount];
        for (int index = 0; index < poolingCount; index++)
        {
            poolingQue[index] = new Queue<GameObject>();
        }
        int count = 10;



        for (int forCount = 0; forCount < poolingCount; forCount++)
        {
            for (int index = 0; index < count; index++)
            {
                InstantiatePrefabs(forCount);
            }
        }

        fontPoint[0] = worldUI.transform.Find("StageUI/Dyanamic/0").GetComponent<Transform>();
        fontPoint[1] = worldUI.transform.Find("StageUI/Dyanamic/1").GetComponent<Transform>();


    }

    Queue<GetItemPrefabs> getItemTextQue = new Queue<GetItemPrefabs>();
    List<GetItemPrefabs> getItemTextList = new List<GetItemPrefabs>();
    /// <summary>
    /// ������Ʈ ����
    /// </summary>
    /// <param name="value"></param>
    private void InstantiatePrefabs(int value)
    {
        GameObject obj = null;

        switch (value)
        {
            case 0: // Reward
                obj = Instantiate(poolingObj[value], rewardRef.transform);
                poolingQue[value].Enqueue(obj);
                break;

            case 1: //���ϴ�
                obj = Instantiate(poolingObj[value], getItemLayOut.gameObject.transform);
                GetItemPrefabs textobj = obj.GetComponent<GetItemPrefabs>();

                getItemTextList.Add(textobj); // ���࿩�� üũ��
                getItemTextQue.Enqueue(textobj); // Ǯ����
                textobj.gameObject.SetActive(false);

                break;

            case 2: // ���Ҹ� ��Ʈ
                obj = Instantiate(poolingObj[value], fontDanymic);
                minusMaterialFont[value] = obj.GetComponent<UI_IncreaseValueFont>();
                obj.transform.position = fontDanymic.transform.position;
                poolingQue[value].Enqueue(obj);
                break;
        }

        obj.SetActive(false);
    }



    /// <summary>
    ///  WorldUI_Prefabs
    /// </summary>
    /// <param name="index"> 0Reward ��ܹ� / 1 �����ϴ� �پ����� </param>
    /// <returns></returns>
    public GameObject Get_WorldUIPooling_Prefabs_Object(int index)
    {
        GameObject obj;

        if (poolingQue[index].Count <= 0)
        {
            InstantiatePrefabs(index);
        }

        obj = poolingQue[index].Dequeue();

        return obj;
    }

    /// <summary>
    /// ��� �� �� ��� UI���� ���� �ö�
    /// </summary>
    /// <param name="type"> 0���, 1 ��</param>
    /// <param name="Matrielvalue"></param>
    public void Use_GoldOrStarMetrialFontPooling(int type, string Matrielvalue)
    {
        GameObject obj;

        if (poolingQue[2].Count <= 0)
        {
            InstantiatePrefabs(2);
        }

        obj = poolingQue[2].Dequeue();
        obj.transform.localPosition = fontPoint[type].localPosition;
        obj.GetComponent<UI_IncreaseValueFont>().Set_PosAndColorInit(Matrielvalue);
    }
    /// <summary>
    /// World UI �����ϴ� �˸�
    /// </summary>
    /// <param name="index"></param>
    /// <param name="img"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public void Get_ItemInfomation_UI_Active(Sprite img, string Value)
    {
        if (!worldUI.gameObject.activeInHierarchy) { return; }

        GetItemPrefabs obj;

        if (poolingQue[1].Count <= 0)
        {
            InstantiatePrefabs(1);
        }

        obj = getItemTextQue.Dequeue();
        obj.gameObject.SetActive(true);

        if (!AudioManager.inst.noSound)
        {
            AudioManager.inst.Play_Ui_SFX(18, 0.35f);
        }

        // ������ȹ�� ����
        if (worldUI.gameObject.activeInHierarchy)
        {
            obj.Set_GetItemSpriteAndText(img, Value);
        }

    }

    /// <summary>
    /// WorldUI Prefabs Return
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"> 0 Reward / 1 UI Material</param>
    public void Return_WorldUIObjPoolingObj(GameObject obj, int value)
    {
        obj.SetActive(false);
        poolingQue[value].Enqueue(obj);
    }

    public void Return_GetItemText(GetItemPrefabs obj)
    {
        if (getItemTextList.Contains(obj))
        {
            getItemTextList.Remove(obj);
        }
        obj.gameObject.SetActive(false);
        getItemTextQue.Enqueue(obj);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    GetItemTextAllReturn();
        //}
    }
    public void GetItemTextAllReturn()
    {
        for (int index = 0; index < getItemTextList.Count; index++)
        {
            if (getItemTextList[index].gameObject.activeInHierarchy)
            {
                getItemTextList[index].A_ReturnObj();
            }
        }
    }

    Coroutine cuttonCor;
    Color colorFadeValue = new Color(0, 0, 0, 0.2f);
    float fadeSpeed = 20;
    /// <summary>
    /// ����ƮUI ���̵��� �Լ�����
    /// </summary>
    /// <param name="funtion"></param>
    public void FrontUICuttonAction(Action funtion)
    {
        if (cuttonCor != null)
        {
            StopCoroutine(cuttonCor);
        }

        cuttonCor = StartCoroutine(PlayCutton(funtion));
    }
    IEnumerator PlayCutton(Action funtion)
    {
        if (frontUiCutton.gameObject.activeSelf == false)
        {
            frontUiCutton.gameObject.SetActive(true);
        }

        while (frontUiCutton.color.a < 1)
        {
            frontUiCutton.color += colorFadeValue * Time.deltaTime * fadeSpeed;
            yield return null;
        }
        cuttonAction = null;
        cuttonAction += funtion;
        cuttonAction?.Invoke();

        while (frontUiCutton.color.a > 0)
        {
            frontUiCutton.color -= colorFadeValue * Time.deltaTime * fadeSpeed;
            yield return null;
        }

        if (frontUiCutton.gameObject.activeSelf == true)
        {
            frontUiCutton.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// ����UI �ڿ��� ������Ʈ �Լ� 
    /// </summary>
    /// <param name="index"> 0��� / 1�� / 2Ű / 3���</param>
    /// <param name="EA"> ���� �ڿ��� </param>
    public void CurMaterialUpdate(int index, string EA)
    {
        curMaterial[index].text = CalCulator.inst.StringFourDigitAddFloatChanger(EA);
    }


    /// <summary>
    /// ���� �߾� ��� Rewardâ ȣ��
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="text"> ������ ������</param>
    public void Set_RewardUI_Invoke(Sprite sprite, string text)
    {
        if (!worldUI.gameObject.activeInHierarchy) { return; }
        AudioManager.inst.Play_Ui_SFX(8, 0.5f);
        GameObject obj = Get_WorldUIPooling_Prefabs_Object(0);
        obj.GetComponent<Reward_Parts>().Set_Reward(sprite, text);
    }

    /// <summary>
    /// ���� �߾� ��� Reward + Action ���� ȣ��
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="text"> ������ ������</param>
    public void Set_Reward_InclueAction(Sprite sprite, string text, Action funtion)
    {
        if (!worldUI.gameObject.activeInHierarchy) { return; }
        AudioManager.inst.Play_Ui_SFX(8, 0.5f);
        GameObject obj = Get_WorldUIPooling_Prefabs_Object(0);
        obj.GetComponent<Reward_Parts>().Set_RewardIncludeAction(sprite, text, funtion);
    }



    Color fadeOutColor = new Color(0, 0, 0, 0.05f);
    float FadeOutSpeedMultiFlyer = 12f;

    /// <summary>
    /// �α��ξ����� ���ξ� �ε�Ϸ�� ����ũ ���̵�ƿ� ����
    /// </summary>
    public void LoadScene_FakeScreen_Active()
    {
        StartCoroutine(FakeScreenStart());
    }

    IEnumerator FakeScreenStart()
    {
        if (!fakeScreen.gameObject.activeSelf)
        {
            fakeScreen.gameObject.SetActive(true);
        }

        while (fakeScreen.color.a > 0f)
        {
            fakeScreen.color -= fadeOutColor * Time.deltaTime * FadeOutSpeedMultiFlyer;
            yield return null;
        }

        fakeScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// �ο��̹��� Ű�� �ѱ�
    /// </summary>
    /// <param name="indexNum"> 0 �ǹ��� �� / 1 ���� ��ƼŬ // �����̱�</param>
    /// <param name="Active"></param>
    public void RawImagePlayAcitve(int indexNum, bool Active)
    {
        rawImageRef[indexNum].SetActive(Active);
        rawCam[indexNum].SetActive(Active);
    }

    //public void RawImagePlayAcitve(bool Active)
    //{
    //    for (int index = 0; index < rawImageRef.Length; index++)
    //    {
    //        rawImageRef[index].SetActive(false);
    //        rawCam[index].SetActive(false);
    //    }
    //}

    /// <summary>
    /// ȭ�� ���̵� �Ͼ����ٰ� ���� ����
    /// </summary>
    /// 
    Coroutine fadeCr;
    public void Effect_WhiteCutton(float Duration)
    {
        if (fadeCr != null)
        {
            StopCoroutine(fadeCr);
        }
        fadeCr = StartCoroutine(PlayWhiteEffect(Duration));
    }

    Color fadeStartColor = new Color(1, 1, 1, 0.9f);
    float duration = 2f;
    float fadeTimer = 0f;

    IEnumerator PlayWhiteEffect(float Duration)
    {
        fadeTimer = 0;
        duration = Duration;
        whiteCutton.color = fadeStartColor;
        whiteCutton.gameObject.SetActive(true);

        while (fadeTimer < duration)
        {
            float alpha = Mathf.Lerp(fadeStartColor.a, 0f, fadeTimer / duration);
            whiteCutton.color = new Color(1, 1, 1, alpha);
            fadeTimer += Time.deltaTime;
            yield return null;
        }

        whiteCutton.color = new Color(1, 1, 1, 0);
        whiteCutton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Load�� �г��� �ʱ�ȭ
    /// </summary>
    /// <param name="nickname"></param>
    public void Set_Nickname(string nickname) => nickNameText.text = nickname;


    public void NewbieBtnAcitveFalse() => newBieBtn.gameObject.SetActive(false);

    /// <summary>
    /// ������ȹ�� ���̾ƿ� �����ִ�
    /// </summary>
    /// <param name="value"></param>
    public void GetTrs_VerticalLayOutActive(bool value) => getItemLayOut.enabled = value;



}
