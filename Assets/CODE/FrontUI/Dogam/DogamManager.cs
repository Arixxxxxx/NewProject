using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class DogamManager : MonoBehaviour
{
    public static DogamManager inst;

    [Tooltip(" # 0 ���� ���� ������ \n # 1 ���� ���� ������ ")]
    [SerializeField] GameObject[] slotPrefabs;
    GameObject[] slotParentTrs = new GameObject[2];

    // ���� ������ �� ��ũ��Ʈ
    Sprite[] WeaponSprite;
    DogamWeaponSlot[] weaponSlotsSc;
    int curWeaponNumber;

    // ���� ������ ��ũ��Ʈ
    Sprite[] enemySprite;
    [SerializeField]
    DogamMonsterSlot[] dogamMonsterSlots;
    int curMonsterNumber;
    int totalMonsterCount;

    // �ƽ����� 
    [SerializeField]
    int maxSoulCount = 50;
    public int MaxSoulCount { get { return maxSoulCount; } }
    int[] monster_Soul_List;

    // ���� ���� ȹ��� ���ݷ� ���� ����
    int enemyMasterCount;
    int weaponMasterCount;
    public int Get_DogamATKBonus()
    {
        return enemyMasterCount + weaponMasterCount;
    }
    // ���ӷε�� ���� �ʱ�ȭ (Load Data)
    public void GameLoad_MousterList_Init(int[] list)
    {
        if (list.Length < monster_Soul_List.Length) { Debug.Log("ù ����"); return; }
        for (int index = 0; index < monster_Soul_List.Length; index++)
        {
            monster_Soul_List[index] = list[index];
            dogamMonsterSlots[index].Set_current_Soulcount_Update();
            
            if(index == monster_Soul_List.Length - 1)
            {
                MonsterSoulMasterCheker();
            }
        }
    }


    //�ҿ� ȹ��� 
    public void Set_Monster_Soul(int value)
    {
        if (monster_Soul_List[value] < maxSoulCount)
        {
            monster_Soul_List[value]++;
            dogamMonsterSlots[value].Set_current_Soulcount_Update();
            MonsterSoulMasterCheker();
        }

        // ���⿡ ��ġ
    }
    public int[] Get_monster_Soul() => monster_Soul_List;

    //���� ����
    GameObject worldUIRef, frontUiRef, dogamMainRef, windowRef;
    GameObject[] bottomViewr = new GameObject[2];
    Button xBtn;
    Button[] topArrayBtn = new Button[2];
    Image[] topButtonImg;
    TMP_Text[] topBtnText;

    // ���� ����
    Image[] viewrBox_WeaponIMG = new Image[2];
    TMP_Text[] weaponInfoText = new TMP_Text[2];
    //���� ����
    Image viewrBG;
    TMP_Text[] enemyInfoText = new TMP_Text[2];
    GameObject maskObj;
    TMP_Text[] bottomText = new TMP_Text[2];

    Image[] weaponeSelectCutton = new Image[2];



    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(inst);
        }

        AwakeInit();
        BtnInit();
        Slot_Prefabs_Init();
    }

    private void AwakeInit()
    {
        worldUIRef = GameManager.inst.WorldUiRef;
        frontUiRef = GameManager.inst.FrontUiRef;
        dogamMainRef = frontUiRef.transform.Find("Dogam").gameObject;
        windowRef = dogamMainRef.transform.Find("Window").gameObject;
        bottomViewr[0] = windowRef.transform.Find("Weapon").gameObject;
        bottomViewr[1] = windowRef.transform.Find("Monster").gameObject;


        xBtn = windowRef.transform.Find("X_Btn").GetComponent<Button>();
        topArrayBtn = windowRef.transform.Find("TopBtnArray").GetComponentsInChildren<Button>();
        topButtonImg = new Image[topArrayBtn.Length];

        //��� ����,���� ��ư
        topBtnText = new TMP_Text[topArrayBtn.Length];
        for (int index = 0; index < topArrayBtn.Length; index++)
        {
            topButtonImg[index] = topArrayBtn[index].GetComponent<Image>();
            topBtnText[index] = topArrayBtn[index].GetComponentInChildren<TMP_Text>();
        }

        slotParentTrs[0] = bottomViewr[0].transform.Find("WeaponList/Scroll View/Viewport/Content").gameObject;
        slotParentTrs[1] = bottomViewr[1].transform.Find("EnemyList/Scroll View/Viewport/Content").gameObject;


        // ����
        viewrBox_WeaponIMG[0] = bottomViewr[0].transform.Find("WeaponMainInfo/BG/Weapon").GetComponent<Image>();
        viewrBox_WeaponIMG[1] = bottomViewr[1].transform.Find("MainInfo/BG(Mask)/Char").GetComponent<Image>();

        weaponInfoText[0] = bottomViewr[0].transform.Find("WeaponMainInfo/Box/WeaponText").GetComponent<TMP_Text>();
        weaponInfoText[1] = bottomViewr[0].transform.Find("WeaponMainInfo/Box/WeaponInfoText").GetComponent<TMP_Text>();

        weaponeSelectCutton[0] = bottomViewr[0].transform.Find("WeaponMainInfo/BG/Cutton").GetComponent<Image>();
        weaponeSelectCutton[1] = bottomViewr[1].transform.Find("MainInfo/BG(Mask)/Cutton").GetComponent<Image>();

        bottomText[0] = bottomViewr[0].transform.Find("BottomInfo/BottomBox/BottomText").GetComponent<TMP_Text>();
        bottomText[1] = bottomViewr[1].transform.Find("BottomInfo/BottomBox/BottomText").GetComponent<TMP_Text>();

        //����
        enemyInfoText[0] = bottomViewr[1].transform.Find("MainInfo/Box/EnemyName").GetComponent<TMP_Text>();
        enemyInfoText[1] = bottomViewr[1].transform.Find("MainInfo/Box/EnemyInfo").GetComponent<TMP_Text>();
        viewrBG = bottomViewr[1].transform.Find("MainInfo/BG(Mask)/BackGround").GetComponent<Image>();

    }

    private void Start()
    {

    }
    private void BtnInit()
    {
        xBtn.onClick.AddListener(() => Active_DogamUI(false));
        topArrayBtn[0].onClick.AddListener(() => Acitve_Bottom_Viewr(0));
        topArrayBtn[1].onClick.AddListener(() => Acitve_Bottom_Viewr(1));
    }

    private void Slot_Prefabs_Init()
    {

        // ���� ���� �ʱ�ȭ
        WeaponSprite = SpriteResource.inst.Weapons;
        weaponSlotsSc = new DogamWeaponSlot[WeaponSprite.Length];

        GameObject obj = null;

        for (int index = 0; index < WeaponSprite.Length; index++)
        {
            obj = Instantiate(slotPrefabs[0], slotParentTrs[0].transform);
            weaponSlotsSc[index] = obj.GetComponent<DogamWeaponSlot>();
            weaponSlotsSc[index].Init_Prefabs(WeaponSprite[index], index);

        }

        // ���� ���� �ʱ�ȭ
        Sprite[] stage1MonsterSprite = SpriteResource.inst.enemySprite(1);
        Sprite[] stage2MonsterSprite = SpriteResource.inst.enemySprite(2);
        Sprite[] stage3MonsterSprite = SpriteResource.inst.enemySprite(3);

        totalMonsterCount = stage1MonsterSprite.Length + stage2MonsterSprite.Length + stage3MonsterSprite.Length;
        enemySprite = new Sprite[totalMonsterCount];
        monster_Soul_List = new int[totalMonsterCount];
        dogamMonsterSlots = new DogamMonsterSlot[totalMonsterCount];
        int forcount = 0;


        for (int index = 0; index < stage1MonsterSprite.Length; index++)
        {
            obj = Instantiate(slotPrefabs[1], slotParentTrs[1].transform);
            enemySprite[forcount] = stage1MonsterSprite[index];
            dogamMonsterSlots[forcount] = obj.GetComponent<DogamMonsterSlot>();
            dogamMonsterSlots[forcount].Init_Prefabs(enemySprite[forcount], forcount);
            forcount++;
        }

        for (int index = 0; index < stage2MonsterSprite.Length; index++)
        {
            obj = Instantiate(slotPrefabs[1], slotParentTrs[1].transform);
            enemySprite[forcount] = stage2MonsterSprite[index];
            dogamMonsterSlots[forcount] = obj.GetComponent<DogamMonsterSlot>();
            dogamMonsterSlots[forcount].Init_Prefabs(enemySprite[forcount], forcount);
            forcount++;
        }

        for (int index = 0; index < stage3MonsterSprite.Length; index++)
        {
            obj = Instantiate(slotPrefabs[1], slotParentTrs[1].transform);
            enemySprite[forcount] = stage3MonsterSprite[index];
            dogamMonsterSlots[forcount] = obj.GetComponent<DogamMonsterSlot>();
            dogamMonsterSlots[forcount].Init_Prefabs(enemySprite[forcount], forcount);
            forcount++;
        }

    }

    private void Monster_Soul_textInit()
    {
        for (int index = 0; index < dogamMonsterSlots.Length; index++)
        {
            dogamMonsterSlots[index].Set_current_Soulcount_Update();
        }
    }



    int[] curWeaponLv;

    /// <summary>
    /// ���� ����â Ȱ��ȭ
    /// </summary>
    /// <param name="value"></param>
    public void Active_DogamUI(bool value)
    {
        if (value == true)
        {
            Acitve_Bottom_Viewr(0); // �⺻���� �ʱ�ȭ
            InitBottomBtns(); // �⺻�ʱ�ȭ
        }
        else
        {
            WorldUI_Manager.inst.RawImagePlayAcitve(1, false);
        }

        dogamMainRef.SetActive(value);
    }

    // ���� ������ Ƚ�� üĿ
    
    public void MasterWeaponCheker()
    {
        curWeaponLv = GameStatus.inst.GetAryWeaponLv().ToArray();
        int masterWeaponCount = 0;
        for (int index = 0; index < curWeaponLv.Length; index++)
        {
            if (curWeaponLv[index] == 5)
            {
                masterWeaponCount++;
                weaponSlotsSc[index].MaskActiveFalse();
            }
            else if (curWeaponLv[index] < 5)
            {
                break;
            }
        }

        weaponMasterCount = masterWeaponCount;
        bottomText[0].text = $"< �߰� ���ݷ� {weaponMasterCount}%��ŭ ��� >   ���� ���� ���� ( {weaponMasterCount} / {curWeaponLv.Length} )";
    }

    public void MonsterSoulMasterCheker()
    {
        int masterCount = 0;
        for (int index = 0; index < monster_Soul_List.Length; index++)
        {
            if (monster_Soul_List[index] >= MaxSoulCount)
            {
                masterCount++;
            }
        }

        enemyMasterCount = masterCount;

        bottomText[1].text = $"< �߰� ���ݷ� {enemyMasterCount}%��ŭ ��� >   ���� ���� ���� ( {enemyMasterCount} / {monster_Soul_List.Length} )";
    }

    Color fadeColor = new Color(0.3f, 0.3f, 0.3f, 1);
    private void Acitve_Bottom_Viewr(int value)
    {
        for (int index = 0; index < topArrayBtn.Length; index++)
        {
            if (index == value)
            {
                bottomViewr[index].gameObject.SetActive(true);
                topButtonImg[index].color = Color.white;
                topBtnText[index].color = Color.white;
            }
            else
            {
                bottomViewr[index].gameObject.SetActive(false);
                topButtonImg[index].color = fadeColor;
                topBtnText[index].color = fadeColor;
            }
        }

        if (value == 0)
        {
            WorldUI_Manager.inst.RawImagePlayAcitve(1, true);
        }
        else
        {
            MonsterSoulMasterCheker();
            WorldUI_Manager.inst.RawImagePlayAcitve(1, false);
        }
    }

    public void InitBottomBtns()
    {
        //���� �ʱ�ȭ
        viewrBox_WeaponIMG[0].sprite = WeaponSprite[0];
        string temp = weaponNameAndInfo[0];
        curWeaponNumber = 0;
        if (weaponSlotsSc[0].master)
        {
            weaponInfoText[0].text = temp.Split('-')[0];
            weaponInfoText[1].text = temp.Split('-')[1];
        }
        else
        {
            weaponInfoText[0].text = "? ? ?";
            weaponInfoText[1].text = " ȹ���غ����ʾƼ�.. \n��������..";
        }

        //���� �ʱ�ȭ
        viewrBox_WeaponIMG[1].sprite = enemySprite[0];
        viewrBG.sprite = SpriteResource.inst.Map(1);
        temp = monsterNameAndInfo[0];
        curMonsterNumber = 0;

        if (dogamMonsterSlots[0].complete)
        {
            enemyInfoText[0].text = temp.Split('-')[0];
            enemyInfoText[1].text = temp.Split('-')[1];
        }
        else
        {
            enemyInfoText[0].text = "? ? ?";
            enemyInfoText[1].text = "������ ���� ��ƾ�.. �˰Ͱ���";
        }

    }


    // �ڽ� �������� ���õǾ����� ���̵� �ξƿ��Ǹ鼭 �̹��� ��ü
    bool once = false;
    public void Set_WeaponMainViewr(int childrenNumber)
    {
        if (once || curWeaponNumber == childrenNumber) { return; } // �ٲ���ִ����̰ų� ���� ��ȣ�̸� Return
        once = true;

        curWeaponNumber = childrenNumber;
        StartCoroutine(Change(childrenNumber, "Weapon"));
    }

    public void Set_MonsterMainViewr(int childrenNumber)
    {
        if (once || curMonsterNumber == childrenNumber) { return; } // �ٲ���ִ����̰ų� ���� ��ȣ�̸� Return
        once = true;

        curMonsterNumber = childrenNumber;
        StartCoroutine(Change(childrenNumber, "Monster"));
    }

    float duration = 0.25f;
    float cuttonTimer = 0f;
    WaitForSeconds changeWaitTime = new WaitForSeconds(0.1f);
    Color zeroColor = new Color(0, 0, 0, 0);
    IEnumerator Change(int chidrenNumber, string type)
    {
        cuttonTimer = 0;
        int currentType = 0;

        if (type == "Monster")
        {
            currentType = 1;
        }
        while (cuttonTimer < duration)
        {
            float alphaZ = Mathf.Lerp(0f, 1f, cuttonTimer / duration);
            weaponeSelectCutton[currentType].color = new Color(weaponeSelectCutton[currentType].color.r, weaponeSelectCutton[currentType].color.g, weaponeSelectCutton[currentType].color.b, alphaZ);
            cuttonTimer += Time.deltaTime;
            yield return null;
        }

        weaponeSelectCutton[currentType].color = Color.black;

        //���̵� �ƿ� �� ����Ǿ��� �κ�
        switch (currentType)
        {
            case 0: // ���� 

                viewrBox_WeaponIMG[currentType].sprite = WeaponSprite[chidrenNumber];
                string temp = weaponNameAndInfo[chidrenNumber];

                // ȹ���غþ�� ����!
                if (weaponSlotsSc[chidrenNumber].master)
                {
                    weaponInfoText[0].text = temp.Split('-')[0];
                    weaponInfoText[1].text = temp.Split('-')[1];
                }
                else
                {
                    weaponInfoText[0].text = "? ? ?";
                    weaponInfoText[1].text = " ȹ���غ����ʾƼ�.. \n��������..";
                }
                break;

            case 1: // ����
                viewrBox_WeaponIMG[currentType].sprite = enemySprite[chidrenNumber];
                string enemyName = monsterNameAndInfo[chidrenNumber];
                if (chidrenNumber < SpriteResource.inst.enemySprite(1).Length)
                {
                    viewrBG.sprite = SpriteResource.inst.Map(0);
                }
                else if (chidrenNumber >= SpriteResource.inst.enemySprite(1).Length && chidrenNumber < SpriteResource.inst.enemySprite(1).Length + SpriteResource.inst.enemySprite(2).Length)
                {
                    viewrBG.sprite = SpriteResource.inst.Map(1);
                }
                else
                {
                    viewrBG.sprite = SpriteResource.inst.Map(2);
                }
                // ȹ���غþ�� ����!
                if (dogamMonsterSlots[0].complete)
                {
                    enemyInfoText[0].text = enemyName.Split('-')[0];
                    enemyInfoText[1].text = enemyName.Split('-')[1];
                }
                else
                {
                    enemyInfoText[0].text = "? ? ?";
                    enemyInfoText[1].text = "������ ���� ��ƾ�.. �˰Ͱ���";
                }
                break;
        }


        yield return changeWaitTime;

        cuttonTimer = 0;

        while (cuttonTimer < duration)
        {
            float alphaZ = Mathf.Lerp(1f, 0f, cuttonTimer / duration);
            weaponeSelectCutton[currentType].color = new Color(weaponeSelectCutton[currentType].color.r, weaponeSelectCutton[currentType].color.g, weaponeSelectCutton[currentType].color.b, alphaZ);
            cuttonTimer += Time.deltaTime;
            yield return null;
        }
        weaponeSelectCutton[currentType].color = zeroColor;
        once = false;
    }


    float getChance = 10f; // <�� ���� óġ�� �������� ���� Ȯ��
    public void MosterDogamIndexValueUP(int stage, int enemyIndex)
    {
        int monsterIndex = (stage - 1) * 5 + enemyIndex;
        if (monster_Soul_List[monsterIndex] >= maxSoulCount) { return; }

        float randomValue = UnityEngine.Random.Range(0, 100f);
        if (randomValue > getChance) { return; };

        Sprite IMG = SpriteResource.inst.EnemySoul(stage - 1);

        string monsterName = monsterNameAndInfo[monsterIndex].Split('-')[0];
        string text = $"'{monsterName}'�� ����";
        WorldUI_Manager.inst.Get_ItemInfomation_UI_Active(IMG, text);

        // ���� �ݿ�
        Set_Monster_Soul(monsterIndex);
    }

    string[] weaponNameAndInfo =
        {
            "�׵�-�볪���� ���� ���̴� ������ ���� �ʺ����� ���̴�",    //0
             "��-�ճ��� ���ѳ����� ���� ��\n���� �̻����� �߰��ϴ�",     //1
             "�߽ĵ�-ȣ�ΰ� ��������� ���\n�ٴϴ� �߽ĵ�",    //2 
             "�յ���-�� ������ �� ������",         //3 
             "��-�� ����� ��°� �γ���\n������ ��ɰ� ���� �������.\nƯ�� ���� �������",       //4 
             "����-��� 3���̸� ������ �ٷ��",        //5 
             "�ڵ�-�߱� �ϼ۴���� ����������� ���� �ΰ��� ������ �ʰ� ����� �������� �߱���",       //6 
             "�߱�â-�� ������� ���� ����� ����\r\n��⿡ Ưȭ�Ǿ��ִ�",         //7
            "�����-��⺸�ٴ� ���⿡ ����\r\n�ϵ��� ������� ����..",       //8 
            "ȭ����-���� ��ϰ� �οﶧ ����\r\n��ä�� �� ��� �� ����.......",         //9
            "����-3�� ����� �ϳ���.\r\n�ٸ� 2�ڷ�� ���� �Ϻ��� \r\n�ѱ��� �ִ�",         // 10
            "ȣ�α�-��� �߱����� ������ ����..\r\n������ �� Į���̴�",        //11
            "���Ǻ�-��ó ������ �����̰�\r\n������ ��� ������",        //12
            "���Ȼ��-�к����� �������� ���Դٰ�\r\n�ΰ� �� â�̴�",         //13
            "û������-�� ���� �ı� ���� ���ƿ´�\r\n���ݾƿ�",     //14
            "Ź��ä-���� ü������ �Ҿƹ�����\r\n���� Ź��ä",       //15
            "���̺�ä-������� ���� ��ä�� ��ϸ�\r\n���� ���󸸵� ��ä",         //16
            "������-100% �� ���ڱ⿡ ġ��, \r\n��Ŭ, ����, �丶��, ���ĸ�\r\n���� ���� ���� �ž�... ",       //17
            "���ڷ�-���� û�Ҵ�� ������?",        //18
            "�汤��-���� �� ���� �Ĵ�",         //19
            "������-�츮 ���̰� �����ϴ� ������",        //20
            "�վ-�� �Ⱑ����",         //21
            "�и�ġ-�и�ġ�� ���⸻�� ����\r\n�־��ٴ°� ��� .. ����",       //22
            "����-�Ǵٰ� �־�� �� ���� ��Ÿ���� �ʴ´ٸ� �� ������ ��� �ǰڽ��ϱ�?",     //23
            "����Ǳ�-�Ǵٰ� ���� �뷡�ϰ� �־�!\r\n�� ������ ��ü ����?!",         //24
            "û��ġ-�ϳ� ���� û��ġ���ٿ�",           //25
            "¯��-�ƺ��� �� ��������",           //26
            "�������-��Ʈ��",            //27
            "���ķ�-ġ���ǻ���� ���� ����� \r\n���",            //28
            "�˸����-���� �������� ���ϳ�?\r\n�������� ������",         //29
        };

    string[] monsterNameAndInfo =
       {
            "���� ����-�� �¾ �ñ�����",    //0
             "�ñ� ������-�ñ��� ���� ����\r\n���ΰ� �ִ�",     //1
             "���� �ñ�����-��ũ���� �ñ⵹����",    //2 
             "���̽����-���������� ���̽��巡��",         //3 
             "�ñ��� ����-���� ������� �ñ���� ����\r\n��� ������ ���������",       //4 
             "�� ����-�� �¾ ȭ������ ����",        //5 
             "�� ����-1�� ��ȭ�� ������ ����\\n��� ���� �� ȭ����\r\n����ǰ� �ִ�.",       //6 
             "��� ����-��� �������� �����\r\n�귯������. ������ �����\r\n��Ƴ����͸� ����.",         //7
            "���� �������-���Ǳ����� ������ �ϼ���\r\n������ ������ ���� ������.",       //8 
            "���Ǳ���-������ó��, �ʸ��ڵ��\r\n�������ɰ��� ����",         //9
            "��ٸ�-��Ŀ�� ��ȭ�ϱ����� ����\r\n���� �Ϳ��� �����",         // 10
            "�׸� ��Ŀ-�������� �ܹ��� �������ְ�\r\n�׻� �غ�� �յ�����\r\n�ϰ��ִ�",        //11
            "��ũ ��Ŀ-��Ŀ���� �ϵ�źΰ�",        //12
            "���� ��Ŀ-��Ŀ���̰��� ����è�Ǿ�\r\n�Ѵ������ ������ ��������\r\n��������",         //13
            "��� ��Ŀ-������ ���� ������ ��ȭ����\r\n�����Ѵ�. �� ģ�� �׷��� \r\n���Ŵ�",     //14
        };

}
