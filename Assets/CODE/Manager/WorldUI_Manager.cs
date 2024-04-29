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
    [SerializeField] GameObject getGoldAndStar_Text;
    Queue<GameObject> getGoldAndStar_TextQue = new Queue<GameObject>();
    Transform fontDanymic;
    Transform[] fontPoint = new Transform[2]; // Ǯ��������Ʈ ��ŸƮ����Ʈ �ʱ�ȭ��

    GameObject worldUI;
    Image[] stageSlot = new Image[5];
    Image uiBossHead;
    TMP_Text stageText;

    Button[] testBtn;
    TMP_Text[] weapbtnText;
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
    [SerializeField]
    GameObject stageUI;

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

    // ���͵��� ��ư
    Button mosterDogamBtn;

    //ȯ����ư
    Button hwanSengBtn;

    // �������� ��ư
    Button adDeleteBtn;

    //����ɺ� ����
    List<GameObject> redSimBall_Icons = new List<GameObject>();

    // �ﰢ�� ��ư
    Button openMenuIcon;
    Animator menuAnim;
    Transform checkArrowScaleX;

    // ����ƮUI �����̵� Ŀư
    Action cuttonAction;
    Image frontUiCutton;


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

        worldUI = GameObject.Find("---[World UI Canvas]").gameObject;
        frontUICanvas = GameObject.Find("---[FrontUICanvas]").gameObject;

        //���������
        rewardRef = frontUICanvas.transform.Find("ReWard").gameObject;
        rewards = rewardRef.GetComponentsInChildren<Reward_Parts>(true);
        rewardChildCount = rewardRef.transform.childCount;


        //����â
        buffSelectUIWindow = frontUICanvas.transform.Find("Buff_Window").gameObject;

        cuttonBlack = worldUI.transform.Find("Cutton(B)").GetComponent<Animator>();
        stageText = worldUI.transform.Find("StageUI/StageInfo/Text").GetComponent<TMP_Text>();
        uiBossHead = worldUI.transform.Find("StageUI/StageInfo/Boss").GetComponent<Image>();
        fontDanymic = worldUI.transform.Find("StageUI/Dyanamic").GetComponent<Transform>();

        for (int index = 0; index < stageSlot.Length; index++)
        {
            stageSlot[index] = worldUI.transform.Find("StageUI/StageInfo").GetChild(index).GetComponent<Image>();
        }

        //�׽�Ʈ ��ư
        testBtn = worldUI.transform.Find("TestBtn").GetComponentsInChildren<Button>();


        weapbtnText = new TMP_Text[testBtn.Length];
        for (int index = 0; index < testBtn.Length; index++)
        {
            weapbtnText[index] = testBtn[index].GetComponentInChildren<TMP_Text>();
        }
        curMaterial[0] = worldUI.transform.Find("StageUI/Bottom/Gold/UI_Text").GetComponent<TMP_Text>();
        curMaterial[1] = worldUI.transform.Find("StageUI/Bottom/Star/UI_Text").GetComponent<TMP_Text>();
        curMaterial[2] = worldUI.transform.Find("StageUI/Bottom/Key/UI_Text").GetComponent<TMP_Text>();
        curMaterial[3] = worldUI.transform.Find("StageUI/Bottom/Ruby/UI_Text").GetComponent<TMP_Text>();



        questListBtn = worldUI.transform.Find("StageUI/QeustList/Button").GetComponent<Button>();
        mainMenuBtn = worldUI.transform.Find("StageUI/MainMenu").GetComponent<Button>();
        mainMenuBtn.onClick.AddListener(() => MainMenuManager.inst.Set_MainMenuActive(true));

        hwanSengBtn = worldUI.transform.Find("StageUI/HwanSeng").GetComponent<Button>();

        // �޴���ư��
        
        getLetterBtn = worldUI.transform.Find("StageUI/Letter").GetComponent<Button>(); // ������
        dailyPlayCheckBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/DailyCheck").GetComponent<Button>(); //�⼮üũ
        newBieBtn = worldUI.transform.Find("StageUI/NewBie").GetComponent<Button>(); //����
        mosterDogamBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/MosterDogam").GetComponent<Button>(); //���͵���
        minigameAlrimBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/MiniGame").GetComponent<Button>(); //�̴ϰ���
        adDeleteBtn = worldUI.transform.Find("StageUI/MenuBox/Btns/AdDelete").GetComponent<Button>(); // ��������
        openMenuIcon = worldUI.transform.Find("StageUI/MenuBox/MeneOpen/RealBtn").GetComponent<Button>(); // �޴� �ﰢ����ư
        checkArrowScaleX = openMenuIcon.transform.parent.GetComponent<Transform>(); 

        menuAnim = worldUI.transform.Find("StageUI/MenuBox").GetComponent<Animator>(); // �޴��� ���� �ִϸޤ��̼�

        // FrontUI
        frontUiCutton = frontUICanvas.transform.Find("Cutton").GetComponent<Image>();

        Prefabs_Awake();
        
    }



    void Start()
    {
        //�׽�Ʈ�� ���߿� ��������
        BtnInIt();

        // ���� ������ȭ�� �ʱ�ȭ
        curMaterial[0].text = GameStatus.inst.Gold;
        curMaterial[1].text = GameStatus.inst.Star;
        curMaterial[2].text = GameStatus.inst.Key;
        curMaterial[3].text = GameStatus.inst.Ruby.ToString();
    }


    /// <summary>
    /// �������� UI�� ��ĥ
    /// </summary>
    /// <param name="curFloorLv"></param>
    public void Set_StageUiBar(int curFloorLv)
    {
        stageSlot[curFloorLv].sprite = stageSprite[2];
        stageText.text = $"Stage {GameStatus.inst.StageLv} - {GameStatus.inst.FloorLv}";
        uiBossHead.gameObject.SetActive(curFloorLv == 4 ? true : false);
        for (int index = 0; index < curFloorLv; index++)
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
        stageText.text = $"Stage {GameStatus.inst.StageLv} - {GameStatus.inst.FloorLv}";
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

        //testBtn[0].onClick.AddListener(() =>
        //{
        //    GameStatus.inst.AtkSpeedLv++;
        //    if (GameStatus.inst.AtkSpeedLv < 10)
        //    {
        //        weapbtnText[0].text = $"���� �ӵ� x {GameStatus.inst.AtkSpeedLv}";
        //    }
        //    else if (GameStatus.inst.AtkSpeedLv >= 10)
        //    {
        //        weapbtnText[0].text = $"����";
        //    }
        //});


        // ���� ��ư �ʱ�ȭ

        hwanSengBtn.onClick.AddListener(() => HwanSengSystem.inst.Set_HwansengUIActive(true)); //ȯ����ư
        getLetterBtn.onClick.AddListener(() => { LetterManager.inst.OpenPostOnOfficeAndInit(true); });
        dailyPlayCheckBtn.onClick.AddListener(() => { DailyPlayCheckUIManager.inst.MainWindow_Acitve(true); });
        newBieBtn.onClick.AddListener(() => { Newbie_Content.inst.Set_NewbieWindowActive(true); });
        mosterDogamBtn.onClick.AddListener(() => { DogamManager.inst.Set_DogamListAcitve(1, true); });
        adDeleteBtn.onClick.AddListener(() => AdDelete.inst.ActiveAdDeleteWindow());
        minigameAlrimBtn.onClick.AddListener(() => 
        {
            MinigameManager.inst.Active_minigameEntrance(true);
        });

        openMenuIcon.onClick.AddListener(() => 
        {
          if(checkArrowScaleX.localScale.x == 1)
            {
                menuAnim.SetTrigger("Open");
            }
          else if(checkArrowScaleX.localScale.x == -1)
            {
                menuAnim.SetTrigger("Close");
            }
        });
    }

    private void Prefabs_Awake()
    {
        int count = 10;

        fontPoint[0] = worldUI.transform.Find("StageUI/Dyanamic/0").GetComponent<Transform>();
        fontPoint[1] = worldUI.transform.Find("StageUI/Dyanamic/1").GetComponent<Transform>();

        for (int index = 0; index < count; index++)
        {
            GameObject obj = Instantiate(getGoldAndStar_Text, fontDanymic);
            getGoldAndStar_TextQue.Enqueue(obj);
            obj.transform.position = fontDanymic.transform.position;
            obj.SetActive(false);
        }
    }
    /// <summary>
    /// ȭ�� �ڿ��� ȹ���� �ڿ��� ���ڿö󰡴� ���� ( Gold = 0 / Star = 1 )
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public void Get_Increase_GetGoldAndStar_Font(int index, string textvalue)
    {
        if (getGoldAndStar_TextQue.Count <= 1)
        {
            GameObject obj = Instantiate(getGoldAndStar_Text, fontDanymic);
            getGoldAndStar_TextQue.Enqueue(obj);
            obj.transform.position = fontDanymic.transform.position;
            obj.SetActive(false);
        }


        GameObject objs = getGoldAndStar_TextQue.Dequeue();
        objs.transform.localPosition = fontPoint[index].localPosition;
        objs.GetComponent<UI_IncreaseValueFont>().Set_PosAndColorInit(index, textvalue);
        objs.gameObject.SetActive(true);
    }

    public void Return_GoldAndStarFontPrefabs(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }

        getGoldAndStar_TextQue.Enqueue(obj);

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
        if(cuttonCor != null)
        {
            StopCoroutine(cuttonCor);
        }

        cuttonCor = StartCoroutine(PlayCutton(funtion));
    }
    IEnumerator PlayCutton(Action funtion)
    {
        if(frontUiCutton.gameObject.activeSelf == false)
        {
            frontUiCutton.gameObject.SetActive(true);
        }

        while(frontUiCutton.color.a < 1)
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
        if (index != 3) // ��� �ƴϸ� (���,��,Ű)
        {
            curMaterial[index].text = CalCulator.inst.StringFourDigitAddFloatChanger(EA);
        }
        else if (index == 3)
        {
            curMaterial[index].text = EA;
        }

        
    }

    int curIndex = 0;
    /// <summary>
    /// ���� �߾� ��� Rewardâ ȣ��
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="text"> ������ ������</param>
   public void Set_RewardUI_Invoke(Sprite sprite, string text)
    {
        curIndex = (int)Mathf.Repeat(curIndex, rewardChildCount);
        rewards[curIndex].Set_Reward(sprite, text);
        curIndex++;
    }

    /// <summary>
    /// ���� �߾� ��� Reward + Action ���� ȣ��
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="text"> ������ ������</param>
    public void Set_Reward_InclueAction(Sprite sprite, string text,Action funtion)
    {
        curIndex = (int)Mathf.Repeat(curIndex, rewardChildCount);
        rewards[curIndex].Set_RewardIncludeAction(sprite, text, funtion);
        curIndex++;
    }


    /// <summary>
    /// ���� ����â ȣ��
    /// </summary>
    /// <param name="value"> true / false </param>
    public void buffSelectUIWindowAcitve(bool value) => buffSelectUIWindow.SetActive(value);

    public void NewbieBtnAcitveFalse() => newBieBtn.gameObject.SetActive(false);



}
