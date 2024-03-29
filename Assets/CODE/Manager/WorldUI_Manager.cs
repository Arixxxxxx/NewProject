using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
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

    //�ؽ�Ʈ�˸�
    Animator textAlrim;
    TMP_Text alrimText;

    //���� �����׽�Ʈ
    [SerializeField]
    GameObject adSample;
    [SerializeField]
    Button adXbtn;
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

        //���ñ���
        adSample = frontUICanvas.transform.Find("SampleAD").gameObject;
        adXbtn = adSample.transform.Find("X").GetComponent<Button>();

        //�ؽ�Ʈ �˸�
        textAlrim = worldUI.transform.Find("TextAlrim").GetComponent<Animator>();
        alrimText = textAlrim.GetComponentInChildren<TMP_Text>();

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
        for(int index=0; index < testBtn.Length; index++)
        {
            weapbtnText[index] = testBtn[index].GetComponentInChildren<TMP_Text>();
        }
        curMaterial[0] = worldUI.transform.Find("StageUI/Bottom/Gold/UI_Text").GetComponent<TMP_Text>();
        curMaterial[1] = worldUI.transform.Find("StageUI/Bottom/Star/UI_Text").GetComponent<TMP_Text>();
        curMaterial[2] = worldUI.transform.Find("StageUI/Bottom/Key/UI_Text").GetComponent<TMP_Text>();
        curMaterial[3] = worldUI.transform.Find("StageUI/Bottom/Ruby/UI_Text").GetComponent<TMP_Text>();



        questListBtn = worldUI.transform.Find("StageUI/Right/QeustList/Button").GetComponent<Button>();
        questListBtn.onClick.AddListener(() => { QuestListWindow.inst.F_QuestList_ActiveWindow(0); });
        Prefabs_Awake();
    }
    void Start()
    {
        //�׽�Ʈ�� ���߿� ��������
        testBtnInit();

        // ���� ������ȭ�� �ʱ�ȭ
        curMaterial[0].text = GameStatus.inst.PulsGold;
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
   
    private void testBtnInit()
    {
        testBtn[0].onClick.AddListener(() => 
        {
            //weaponNum++;
            //weapbtnText[0].text = $"���� ��ü {weaponNum}��";
             ActionManager.inst.TestBtnWeaponChange(); 
        });

        testBtn[1].onClick.AddListener(() => 
        {
            GameStatus.inst.AtkSpeedLv++;
            if(GameStatus.inst.AtkSpeedLv < 10)
            {
                weapbtnText[1].text = $"���� �ӵ� x {GameStatus.inst.AtkSpeedLv}";
            }
            else if(GameStatus.inst.AtkSpeedLv >= 10)
            {
                weapbtnText[1].text = $"����";
            }
        });

        //testBtn[2].onClick.AddListener(() => // ������� �׽�Ʈ ��ư
        //{
        //    Debug.Log("2");
        //    Get_Increase_GetGoldAndStar_Font(0, "912093203981029389");
        //});

        //testBtn[3].onClick.AddListener(() =>// ������ �׽�Ʈ ��ư
        //{
        //    Debug.Log("3");
        //    Get_Increase_GetGoldAndStar_Font(1, "42344263424346465443");
        //});

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
    /// Gold = 0 / Star = 1
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


    Color orijinColor;
    /// <summary>
    /// �ؽ�Ʈ �˸�â ȣ��
    /// </summary>
    /// <param name="data">�˸�â�� ��� �޼���</param>
    public void Set_TextAlrim(string data)
    {
        orijinColor = alrimText.color;

        alrimText.text = data;
        StopCoroutine(TextalrimStart());
        StartCoroutine(TextalrimStart());
    }

    IEnumerator TextalrimStart()
    {
        textAlrim.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        textAlrim.SetTrigger("Off");
        yield return new WaitForSecondsRealtime(1.5f);
        textAlrim.gameObject.SetActive(false);
        alrimText.color = orijinColor;
    }


  /// <summary>
  /// ������ ���� Ȱ��ȭ�����ִ� �Լ�
  /// </summary>
  /// <param name="witch"> buff ~~</param>
  /// <param name="value"></param>
    public void SampleAD(string witch, int value)
    {
        adXbtn.onClick.RemoveAllListeners();
        adXbtn.onClick.AddListener(() =>
        {
            if(witch == "buff" && value != 3)
            {
                BuffContoller.inst.ActiveBuff(value, BuffManager.inst.AdbuffTime(value)); //����Ȱ��ȭ
                BuffManager.inst.AddBuffCoolTime(value, (int)BuffManager.inst.AdbuffTime(value)); // ��Ÿ�� �ð��߰�
                Set_TextAlrim(BuffManager.inst.MakeAlrimMSG(value, (int)BuffManager.inst.AdbuffTime(value))); // �˸�����
                
            }
            else if(value == 3)
            {
                BuffContoller.inst.ActiveBuff(value, BuffManager.inst.AdbuffTime(value)); //����Ȱ��ȭ
                Set_TextAlrim(BuffManager.inst.MakeAlrimMSG(0, (int)BuffManager.inst.AdbuffTime(value))); // �˸�����
            }
           
            adXbtn.gameObject.SetActive(false);
            adSample.SetActive(false);
            buffSelectUIWindow.SetActive(false);
        });

        StopCoroutine(PlayAD());
        StartCoroutine(PlayAD());
    }

    IEnumerator PlayAD()
    {
        adSample.SetActive(true);
        yield return new WaitForSeconds(3);
        adXbtn.gameObject.SetActive(true);
    }
    /// <summary>
    /// ����UI �ڿ��� ������Ʈ �Լ� 
    /// </summary>
    /// <param name="index"> 0��� / 1�� / 2Ű / 3���</param>
    /// <param name="EA"> ���� �ڿ��� </param>
    public void CurMaterialUpdate(int index, string EA) => curMaterial[index].text = CalCulator.inst.StringFourDigitChanger(EA);
    
    /// <summary>
    /// ���� ����â ȣ��
    /// </summary>
    /// <param name="value"> true / false </param>
    public void buffSelectUIWindowAcitve(bool value) => buffSelectUIWindow.SetActive(value);


 
    
}
