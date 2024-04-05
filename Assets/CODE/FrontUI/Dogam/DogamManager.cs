using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class DogamManager : MonoBehaviour
{
    public static DogamManager inst;

    // Ref
    GameObject frontUI, mainMenuRef, windowRef, weaponInfoRef, monsterInfoRef;
    Button xBtn;


    ////////////////////// ���� ����

    // Input Sprite
    [SerializeField][Tooltip("0 = Ȱ��ȭ / 1 = ��Ȱ��ȭ")] Sprite[] topArrayBtnSpirte;
    [SerializeField][Tooltip("0 = ����Ʈ / 1 = ���� / 2 = �÷���")] Sprite[] weaponIcon;

    // Spirte Color
    [SerializeField][Tooltip("0 = Ȱ��ȭ / 1 = ��Ȱ��ȭ")] Color[] weaponColor;

    // Top Array Btn
    Button[] topArrayBtn;
    Image[] topArrayImg;

    // Main Info
    Image charactorWeaponIMG; // ���� �̹���
    TMP_Text[] weaponInfoText = new TMP_Text[2];

    // ViewrSlot
    TMP_Text viewrTitleText;
    DogamWeaponSlot[] weaponSlot;

    //Bottom Imfo
    TMP_Text bonusAttackDMGText;
    TMP_Text curGotCountText;

    RectTransform[] scrollViewRectTrs = new RectTransform[2];

    // ���� Ŭ�� Ȯ�ο�
    int beforeWeaponSelectNum = -1;
    public int BeforeWeaponSelectNum { get { return beforeWeaponSelectNum; } }

    int beforeMonsterSelectNum = -1;
    public int BeforeMonsterSelectNum { get { return beforeMonsterSelectNum; } }

    // ���� ��/�� ������Ȯ�ο�
    bool[] isGotThisWeapon;
    int gotCount; // => ������ ������
    public int weaponDogamGetCount
    {
        get { return gotCount; }
    }

    // ���� �÷��� ����
    public bool[] IsgotThisWeapon// <=== ���߿� �μ� �÷��ߵ� �ڡڡڡ�
    {
        get { return isGotThisWeapon; }
        set { IsgotThisWeapon = value; }
    }

    // ���� �÷��� ����
    int[] mosterCollectionConut;     // <=== ���߿� �μ� �÷��ߵʡڡڡڡ�
    public int[] MonsterColltionCount
    {
        get { return mosterCollectionConut; }
        set { mosterCollectionConut = value; }
    }



    // ��������
    string[,] weaponInfoTextLog;




    ////////////////////// ���� ����

    // Main Info
    Image monsterIMG;
    TMP_Text[] mosterInfoText = new TMP_Text[2];
    DogamMonsterSlot[] monsterSlot;

    // ��������
    string[,] mosterInfoTextLog;

    // Bottom Text
    TMP_Text bonusEnemyDogamAtkText;
    TMP_Text curEnemyDogamContText;

    int monsterCollectionCount;




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

        // Ref
        frontUI = GameObject.Find("---[FrontUICanvas]").gameObject;
        mainMenuRef = frontUI.transform.Find("Dogam").gameObject;
        windowRef = mainMenuRef.transform.Find("Window").gameObject;
        weaponInfoRef = windowRef.transform.Find("Weapon").gameObject;
        monsterInfoRef = windowRef.transform.Find("Monster").gameObject;


        // Title 
        xBtn = mainMenuRef.transform.Find("Window/Title/X_Btn").GetComponent<Button>();

        // TopSelectCartegory Btn
        topArrayBtn = windowRef.transform.Find("TopBtnArray").GetComponentsInChildren<Button>();
        topArrayImg = windowRef.transform.Find("TopBtnArray").GetComponentsInChildren<Image>();

        // MainInfo
        charactorWeaponIMG = weaponInfoRef.transform.Find("WeaponMainInfo/BG/Weapon").GetComponent<Image>();
        weaponInfoText[0] = weaponInfoRef.transform.Find("WeaponMainInfo/Box/WeaponText").GetComponent<TMP_Text>();
        weaponInfoText[1] = weaponInfoRef.transform.Find("WeaponMainInfo/Box/WeaponInfoText").GetComponent<TMP_Text>();
        viewrTitleText = weaponInfoRef.transform.Find("WeaponList/DogamTitle/TitleText").GetComponent<TMP_Text>();

        monsterIMG = monsterInfoRef.transform.Find("MainInfo/BG/IMG").GetComponent<Image>();
        mosterInfoText[0] = monsterInfoRef.transform.Find("MainInfo/Box/name").GetComponent<TMP_Text>();
        mosterInfoText[1] = monsterInfoRef.transform.Find("MainInfo/Box/Info").GetComponent<TMP_Text>();

        // Slot
        weaponSlot = weaponInfoRef.transform.Find("WeaponList/Scroll View/Viewport/Content").GetComponentsInChildren<DogamWeaponSlot>();
        scrollViewRectTrs[0] = weaponInfoRef.transform.Find("WeaponList/Scroll View/Viewport/Content").GetComponent<RectTransform>();

        weaponInfoTextLog = new string[weaponSlot.Length, 2];
        isGotThisWeapon = new bool[weaponSlot.Length];

        monsterSlot = monsterInfoRef.transform.Find("EnemyList/Scroll View/Viewport/Content").GetComponentsInChildren<DogamMonsterSlot>();
        scrollViewRectTrs[1] = monsterInfoRef.transform.Find("EnemyList/Scroll View/Viewport/Content").GetComponent<RectTransform>();

        mosterInfoTextLog = new string[monsterSlot.Length, 2];
        MonsterColltionCount = new int[monsterSlot.Length];
        
        // Bottom Text
        bonusAttackDMGText = weaponInfoRef.transform.Find("BottomInfo/Top").GetComponent<TMP_Text>();
        curGotCountText = weaponInfoRef.transform.Find("BottomInfo/MiddleRight").GetComponent<TMP_Text>();

        bonusEnemyDogamAtkText = monsterInfoRef.transform.Find("BottomInfo/Top").GetComponent<TMP_Text>();
        curEnemyDogamContText = monsterInfoRef.transform.Find("BottomInfo/MiddleRight").GetComponent<TMP_Text>();
        //Test
        GetWeaponCheck(0); // �⺻����� �׻� �÷���Ȱ��ȭ
        


        BtnInit();
        WeaponTextInit(); // ���� ��Ʈ��������
        MonsterTextInit();// ���� ��Ʈ��������
      
    }

    private void BtnInit()
    {
        xBtn.onClick.AddListener(() => Set_DogamListAcitve(0, false));

        topArrayBtn[0].onClick.AddListener(() =>
        {
            Set_WeaponViewrInit();
        });
        topArrayBtn[1].onClick.AddListener(() =>
        {
            Set_MosterViewrInit();
        });
    }

    private void Start()
    {
        EnemyDogam_TextAnd_Count_Init(); //���� �������� �ֽ�ȭ����
    }
    /// <summary>
    ///  ����â Active 
    /// </summary>
    /// <param name="indextype"> 0 = ���⵵�� / 1 = ���͵��� </param>
    /// <param name="value"> true / false </param>
    public void Set_DogamListAcitve(int indextype, bool value)
    {
        if (value == true && indextype == 0)
        {
            beforeMonsterSelectNum = 0;
            Set_WeaponViewrInit();
        }
        else if (value == true && indextype == 1)
        {
            beforeMonsterSelectNum = 0;
            Set_MosterViewrInit();
        }

        if (value == false)
        {
            //��ũ�ѹ� ��ġ �ʱ�ȭ �� ���� Ŭ����ȣ �ʱ�ȭ

            beforeWeaponSelectNum = -1;
            beforeMonsterSelectNum = -1;
            Vector3 reposition0 = new Vector3(scrollViewRectTrs[0].position.x , 0);
            Vector3 reposition1 = new Vector3(scrollViewRectTrs[1].position.x , 0);    

            scrollViewRectTrs[0].position = reposition0;
            scrollViewRectTrs[1].position = reposition1;
        }

        mainMenuRef.SetActive(value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {

        }
    }



    //////////////////////////////////////////////////////////////
    ///////////////////////  ���� ���� ���� ///////////////////////
    //////////////////////////////////////////////////////////////

    // ���� ���� Ȱ��ȭ ��  �ʱ�ȭ
    private void Set_WeaponViewrInit()
    {

        topArrayImg[0].sprite = topArrayBtnSpirte[0];
        topArrayImg[1].sprite = topArrayBtnSpirte[1];

        if (monsterInfoRef.activeSelf == true)
        {
            monsterInfoRef.SetActive(false);
        }

        if (beforeWeaponSelectNum == -1) // ���ʽ��ට��
        {
            for (int index = 0; index < weaponSlot.Length; index++)
            {
                weaponSlot[index].ResetIconBoxLayout();
            }
        }

        weaponInfoRef.SetActive(true);

    }




    /// <summary>
    /// ��ư���� ����â ĳ������ �����̹����� ��ü �� �ڱ� ��ȣ ����
    /// </summary>
    /// <param name="img"></param>
    public void CharactorWeaponImgChangerAndNumber(Sprite img, int Num)
    {
        charactorWeaponIMG.sprite = img; // ����ĳ���� ����ٲ���
        MainWeaponInfoChanger(Num); // ����â �������� �ٲ���

        if (beforeWeaponSelectNum != -1) //�����ܹڽ� �����������͸� �ٲ���
        {
            weaponSlot[beforeWeaponSelectNum].ResetIconBoxLayout();
        }

        beforeWeaponSelectNum = Num;
    }



    /// <summary>
    /// ����â �����̸� , ���� ����
    /// </summary>
    /// <param name="clickNum"></param>
    public void MainWeaponInfoChanger(int clickNum)
    {
        if (IsgotThisWeapon[clickNum] == true)
        {
            weaponInfoText[0].text = weaponInfoTextLog[clickNum, 0];
            weaponInfoText[1].text = weaponInfoTextLog[clickNum, 1];
        }
        else
        {
            weaponInfoText[0].text = " ??? ";
            weaponInfoText[1].text = "ȹ�������� ��� ������ ����.";
        }
    }



    /// <summary>
    /// ���� ���׷��̵�� ���� 1ȸ ȹ������ ���� (���� ����ߴ�)
    /// </summary>
    /// <param name="indexNum">�����ε��� ��ȣ</param>
    public void GetWeaponCheck(int indexNum)
    {
        if (IsgotThisWeapon[indexNum] == true) { return; }

        IsgotThisWeapon[indexNum] = true; // ȹ�� ���� ����

        gotCount = IsgotThisWeapon.Count(x => x== true); // �� ȹ�淮 Ȯ��

        bonusAttackDMGText.text = $"���ʽ� ���ݷ� {gotCount}%";
        curGotCountText.text = $"{gotCount} / {IsgotThisWeapon.Length}";
    }




    //////////////////////////////////////////////////////////////
    ///////////////////////  ���� ���� ���� ///////////////////////
    //////////////////////////////////////////////////////////////


    // ���� ���� Ȱ��ȭ �� �ʱ�ȭ
    private void Set_MosterViewrInit()
    {
        topArrayImg[0].sprite = topArrayBtnSpirte[1];
        topArrayImg[1].sprite = topArrayBtnSpirte[0];

        if (weaponInfoRef.activeSelf == true)
        {
            weaponInfoRef.SetActive(false);
        }


        for (int index = 0; index < monsterSlot.Length; index++)
        {
            monsterSlot[index].ResetIconBoxLayout(); //�����ܸ��� 
        }

        EnemyDogam_TextAnd_Count_Init(); // ���� �������� �ʱ�ȭ
        monsterInfoRef.SetActive(true);
    }

    public void MonsterIMGChanger(Sprite img, int myNum)
    {

        if (beforeMonsterSelectNum != myNum)
        {
            int temp = beforeMonsterSelectNum;
            beforeMonsterSelectNum = myNum;
            monsterSlot[temp].ResetIconBoxLayout(); // ���� Ŭ���ߴ��� �ƿ����� ����
        }

        monsterIMG.sprite = img; // ��������Ʈ ��ü
        MainMosterInfoChanger(myNum); // �ؽ�Ʈ ��ü

        beforeMonsterSelectNum = myNum;
    }


    /// <summary>
    /// ����â ���� �̸� , ���� ����
    /// </summary>
    /// <param name="clickNum"></param>
    public void MainMosterInfoChanger(int clickNum)
    {
        if (MonsterColltionCount[clickNum] == 50)
        {
            mosterInfoText[0].text = mosterInfoTextLog[clickNum, 0];
            mosterInfoText[1].text = mosterInfoTextLog[clickNum, 1];
        }
        else
        {
            mosterInfoText[0].text = " ??? ";
            mosterInfoText[1].text = "������ �Ϸ�Ǿ�� ��µ˴ϴ�.";
        }
    }

    /// <summary>
    /// ���� óġ�� �ž������
    /// </summary>
    /// <param name="indexNum"></param>
    public void Get_EnemyElement(int indexNum)
    {
        if (MonsterColltionCount[indexNum] == 50) { return; }

        MonsterColltionCount[indexNum]++;
        monsterSlot[indexNum].Set_CollectionCount(MonsterColltionCount[indexNum]); // ���������� ���� ������Ʈ
    }

    /// <summary>
    /// ���� ���� ���ϴ� Init  ���߿� ĳ�������� �ʱ�ȭ�� �Լ�������Ѽ� �ʱ�ȭ������ҵ�
    /// </summary>
    public void EnemyDogam_TextAnd_Count_Init()
    {
        monsterCollectionCount = MonsterColltionCount.Count(x => x == 50);
        int enmeyLength = MonsterColltionCount.Length;

        // ����â �ϴܺ�
        bonusEnemyDogamAtkText.text = $"���ʽ� ���ݷ� {monsterCollectionCount}%";
        curEnemyDogamContText.text = $"{monsterCollectionCount} / {enmeyLength}";

        //����â �� ĳ���ͺ� �ϴ� �ؽ�Ʈ �ʱ�ȭ
        for (int index = 0; index < enmeyLength; index++)
        {
            monsterSlot[index].Set_CollectionCount(MonsterColltionCount[index]);
        }
    }


    //////////////////////////////////////////////////////////////
    ///////////////////////  ��Ʈ�� ����        ///////////////////////
    //////////////////////////////////////////////////////////////

    // 1. ���⵵�� ���������� �ߴ� �ؽ�Ʈ���� 0 = �̸� 1 = ����
    private void WeaponTextInit()
    {
        weaponInfoTextLog[0, 0] = "�ʽ����� ��";
        weaponInfoTextLog[0, 1] = "�� �������ٰ� ����ǰ���� ���� ���� �������� ��";

        weaponInfoTextLog[1, 0] = "�����뿡�� ������";
        weaponInfoTextLog[1, 1] = "�츮�� �����뿡 ������ ������ \n <color=yellow><b>(��ü ���������ž�?)";

        weaponInfoTextLog[2, 0] = "���ķ�";
        weaponInfoTextLog[2, 1] = "�հ����ķ縦 ���� ���󸸵� ���ķ�";

        weaponInfoTextLog[3, 0] = "����� ��";
        weaponInfoTextLog[3, 1] = "����Ѵ������̰� ���� ����Ѱ�";

        weaponInfoTextLog[4, 0] = "�����";
        weaponInfoTextLog[4, 1] = "CU���������� �Ǹ��ϴ°�";

        weaponInfoTextLog[5, 0] = "������";
        weaponInfoTextLog[5, 1] = "SSS����� �ʷ���Į";

        weaponInfoTextLog[6, 0] = "����������";
        weaponInfoTextLog[6, 1] = "������ ���� ���̿��б�����";

    }

    // 2. ���͵��� ���������� �ߴ� �ؽ�Ʈ���� 0 = �̸� 1 = ����
    private void MonsterTextInit()
    {
        mosterInfoTextLog[0, 0] = "�㵹��";
        mosterInfoTextLog[0, 1] = "�������� ���� �����ִ� ��";

        mosterInfoTextLog[1, 0] = "����";
        mosterInfoTextLog[1, 1] = "�� ���� ���ڰ��� ���ܼ� ������";
    }

    
}
