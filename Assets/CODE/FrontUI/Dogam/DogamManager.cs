using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class DogamManager : MonoBehaviour
{
    public static DogamManager inst;

    [Tooltip(" # 0 무기 슬롯 프리펩 \n # 1 몬스터 슬롯 프리펩 ")]
    [SerializeField] GameObject[] slotPrefabs;
    GameObject[] slotParentTrs = new GameObject[2];

    // 무기 프리펩 및 스크립트
    Sprite[] WeaponSprite;
    DogamWeaponSlot[] weaponSlotsSc;
    int curWeaponNumber;

    // 몬스터 프리펩 스크립트
    Sprite[] enemySprite;
    [SerializeField]
    DogamMonsterSlot[] dogamMonsterSlots;
    int curMonsterNumber;
    int totalMonsterCount;

    // 맥스정수 
    [SerializeField]
    int maxSoulCount = 50;
    public int MaxSoulCount { get { return maxSoulCount; } }
    int[] monster_Soul_List;

    // 도감 정수 획득시 공격력 증가 변수
    int enemyMasterCount;
    int weaponMasterCount;
    public int Get_DogamATKBonus()
    {
        return enemyMasterCount + weaponMasterCount;
    }
    // 게임로드시 도감 초기화 (Load Data)
    public void GameLoad_MousterList_Init(int[] list)
    {
        if (list.Length < monster_Soul_List.Length) { Debug.Log("첫 시작"); return; }
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


    //소울 획득시 
    public void Set_Monster_Soul(int value)
    {
        if (monster_Soul_List[value] < maxSoulCount)
        {
            monster_Soul_List[value]++;
            dogamMonsterSlots[value].Set_current_Soulcount_Update();
            MonsterSoulMasterCheker();
        }

        // 여기에 설치
    }
    public int[] Get_monster_Soul() => monster_Soul_List;

    //공용 변수
    GameObject worldUIRef, frontUiRef, dogamMainRef, windowRef;
    GameObject[] bottomViewr = new GameObject[2];
    Button xBtn;
    Button[] topArrayBtn = new Button[2];
    Image[] topButtonImg;
    TMP_Text[] topBtnText;

    // 무기 변수
    Image[] viewrBox_WeaponIMG = new Image[2];
    TMP_Text[] weaponInfoText = new TMP_Text[2];
    //몬스터 변수
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

        //상단 무기,몬스터 버튼
        topBtnText = new TMP_Text[topArrayBtn.Length];
        for (int index = 0; index < topArrayBtn.Length; index++)
        {
            topButtonImg[index] = topArrayBtn[index].GetComponent<Image>();
            topBtnText[index] = topArrayBtn[index].GetComponentInChildren<TMP_Text>();
        }

        slotParentTrs[0] = bottomViewr[0].transform.Find("WeaponList/Scroll View/Viewport/Content").gameObject;
        slotParentTrs[1] = bottomViewr[1].transform.Find("EnemyList/Scroll View/Viewport/Content").gameObject;


        // 무기
        viewrBox_WeaponIMG[0] = bottomViewr[0].transform.Find("WeaponMainInfo/BG/Weapon").GetComponent<Image>();
        viewrBox_WeaponIMG[1] = bottomViewr[1].transform.Find("MainInfo/BG(Mask)/Char").GetComponent<Image>();

        weaponInfoText[0] = bottomViewr[0].transform.Find("WeaponMainInfo/Box/WeaponText").GetComponent<TMP_Text>();
        weaponInfoText[1] = bottomViewr[0].transform.Find("WeaponMainInfo/Box/WeaponInfoText").GetComponent<TMP_Text>();

        weaponeSelectCutton[0] = bottomViewr[0].transform.Find("WeaponMainInfo/BG/Cutton").GetComponent<Image>();
        weaponeSelectCutton[1] = bottomViewr[1].transform.Find("MainInfo/BG(Mask)/Cutton").GetComponent<Image>();

        bottomText[0] = bottomViewr[0].transform.Find("BottomInfo/BottomBox/BottomText").GetComponent<TMP_Text>();
        bottomText[1] = bottomViewr[1].transform.Find("BottomInfo/BottomBox/BottomText").GetComponent<TMP_Text>();

        //몬스터
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

        // 무기 슬롯 초기화
        WeaponSprite = SpriteResource.inst.Weapons;
        weaponSlotsSc = new DogamWeaponSlot[WeaponSprite.Length];

        GameObject obj = null;

        for (int index = 0; index < WeaponSprite.Length; index++)
        {
            obj = Instantiate(slotPrefabs[0], slotParentTrs[0].transform);
            weaponSlotsSc[index] = obj.GetComponent<DogamWeaponSlot>();
            weaponSlotsSc[index].Init_Prefabs(WeaponSprite[index], index);

        }

        // 몬스터 슬롯 초기화
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
    /// 메인 도감창 활성화
    /// </summary>
    /// <param name="value"></param>
    public void Active_DogamUI(bool value)
    {
        if (value == true)
        {
            Acitve_Bottom_Viewr(0); // 기본으로 초기화
            InitBottomBtns(); // 기본초기화
        }
        else
        {
            WorldUI_Manager.inst.RawImagePlayAcitve(1, false);
        }

        dogamMainRef.SetActive(value);
    }

    // 무기 마스터 횟수 체커
    
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
        bottomText[0].text = $"< 추가 공격력 {weaponMasterCount}%만큼 상승 >   현재 수집 갯수 ( {weaponMasterCount} / {curWeaponLv.Length} )";
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

        bottomText[1].text = $"< 추가 공격력 {enemyMasterCount}%만큼 상승 >   현재 수집 갯수 ( {enemyMasterCount} / {monster_Soul_List.Length} )";
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
        //무기 초기화
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
            weaponInfoText[1].text = " 획득해보지않아서.. \n뭔지몰랑..";
        }

        //몬스터 초기화
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
            enemyInfoText[1].text = "정수를 전부 모아야.. 알것같다";
        }

    }


    // 자식 프리펩이 선택되었을떄 페이드 인아웃되면서 이미지 교체
    bool once = false;
    public void Set_WeaponMainViewr(int childrenNumber)
    {
        if (once || curWeaponNumber == childrenNumber) { return; } // 바뀌고있는중이거나 같은 번호이면 Return
        once = true;

        curWeaponNumber = childrenNumber;
        StartCoroutine(Change(childrenNumber, "Weapon"));
    }

    public void Set_MonsterMainViewr(int childrenNumber)
    {
        if (once || curMonsterNumber == childrenNumber) { return; } // 바뀌고있는중이거나 같은 번호이면 Return
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

        //페이드 아웃 후 변경되야할 부분
        switch (currentType)
        {
            case 0: // 무기 

                viewrBox_WeaponIMG[currentType].sprite = WeaponSprite[chidrenNumber];
                string temp = weaponNameAndInfo[chidrenNumber];

                // 획득해봤어야 알지!
                if (weaponSlotsSc[chidrenNumber].master)
                {
                    weaponInfoText[0].text = temp.Split('-')[0];
                    weaponInfoText[1].text = temp.Split('-')[1];
                }
                else
                {
                    weaponInfoText[0].text = "? ? ?";
                    weaponInfoText[1].text = " 획득해보지않아서.. \n뭔지몰랑..";
                }
                break;

            case 1: // 몬스터
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
                // 획득해봤어야 알지!
                if (dogamMonsterSlots[0].complete)
                {
                    enemyInfoText[0].text = enemyName.Split('-')[0];
                    enemyInfoText[1].text = enemyName.Split('-')[1];
                }
                else
                {
                    enemyInfoText[0].text = "? ? ?";
                    enemyInfoText[1].text = "정수를 전부 모아야.. 알것같다";
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


    float getChance = 10f; // <ㅡ 몬스터 처치시 도감조각 얻을 확률
    public void MosterDogamIndexValueUP(int stage, int enemyIndex)
    {
        int monsterIndex = (stage - 1) * 5 + enemyIndex;
        if (monster_Soul_List[monsterIndex] >= maxSoulCount) { return; }

        float randomValue = UnityEngine.Random.Range(0, 100f);
        if (randomValue > getChance) { return; };

        Sprite IMG = SpriteResource.inst.EnemySoul(stage - 1);

        string monsterName = monsterNameAndInfo[monsterIndex].Split('-')[0];
        string text = $"'{monsterName}'의 정수";
        WorldUI_Manager.inst.Get_ItemInfomation_UI_Active(IMG, text);

        // 정수 반영
        Set_Monster_Soul(monsterIndex);
    }

    string[] weaponNameAndInfo =
        {
            "죽도-대나무로 만든 검이다 가볍고 가장 초보적인 검이다",    //0
             "목도-먼나라 딱총나무로 만든 검\n생각 이상으로 견고하다",     //1
             "중식도-호두가 예비용으로 들고\n다니던 중식도",    //2 
             "손도끼-이 도끼가 니 도끼냐",         //3 
             "낫-한 농부의 노력과 인내와\n비전과 결심과 땀의 결실이죠.\n특히 땀의 결실이죠",       //4 
             "진검-목검 3년이면 진검을 다룬다",        //5 
             "박도-중국 북송대부터 중일전쟁까지 군과 민간을 가리지 않고 사용한 전형적인 중국도",       //6 
             "중국창-옛 병사들이 자주 사용한 무기\r\n찌르기에 특화되어있다",         //7
            "언월도-찌르기보다는 베기에 적합\r\n하도록 만들어진 무기..",       //8 
            "화접선-앞집 언니가 싸울때 쓰던\r\n부채다 그 언니 참 ㅋㅡ.......",         //9
            "보검-3대 명검중 하나다.\r\n다른 2자루는 각각 일본과 \r\n한국에 있다",         // 10
            "호두구-고대 중국에서 쓰였던 무기..\r\n갈고리가 주 칼날이다",        //11
            "여의봉-근처 동물원 원숭이가\r\n가지고 놀던 막대기다",        //12
            "장팔사모-털복숭이 아저씨가 술먹다가\r\n두고 간 창이다",         //13
            "청룡언월도-이 잔이 식기 전에 돌아온다\r\n했잖아요",     //14
            "탁구채-옆집 체육관장 할아버지가\r\n쓰던 탁구채",       //15
            "종이부채-어렸을때 앞집 부채든 언니를\r\n보고 따라만든 부채",         //16
            "뒤집개-100% 순 살코기에 치즈, \r\n피클, 상추, 토마토, 양파를\r\n절대 잊지 못할 거야... ",       //17
            "빗자루-오늘 청소당번 누구냐?",        //18
            "경광봉-아이 엠 유얼 파더",         //19
            "딸랑이-우리 아이가 좋아하는 딸랑이",        //20
            "뚫어뻥-변 기가막혀",         //21
            "뿅망치-뿅망치에 공기말고 돌을\r\n넣었다는건 비밀 .. 찡긋",       //22
            "빠루-판다가 있어야 할 곳에 나타나지 않는다면 이 세상은 어떻게 되겠습니까?",     //23
            "전통악기-판다가 내게 노래하고 있어!\r\n이 선율은 대체 뭐야?!",         //24
            "청새치-꽤나 좋은 청새치였다우",           //25
            "짱돌-아부지 돌 굴러와유",           //26
            "막대사탕-민트맛",            //27
            "탕후루-치과의사들의 연봉 상승의 \r\n비결",            //28
            "똥막대기-똥이 더러워서 피하나?\r\n무서워서 피하지",         //29
        };

    string[] monsterNameAndInfo =
       {
            "얼음 정령-갓 태어난 냉기정령",    //0
             "냉기 돌정령-냉기의 오라가 몸을\r\n감싸고 있다",     //1
             "돌격 냉기정령-벌크업한 냉기돌정령",    //2 
             "아이스디어-얼음군주의 아이스드래곤",         //3 
             "냉기의 군주-신의 모습을한 냉기계의 군주\r\n닿는 모든것을 얼려버린다",       //4 
             "돌 정령-갓 태어난 화염계의 정령",        //5 
             "불 정령-1차 진화에 성공한 정령\\n등에서 이제 막 화산이\r\n불출되고 있다.",       //6 
             "용암 정령-등에서 무지막한 용암이\r\n흘러내린다. 닿으면 모든지\r\n녹아내릴것만 같다.",         //7
            "돌격 용암정령-불의군주의 오른팔 하수인\r\n강력한 힘으로 적을 누른다.",       //8 
            "불의군주-몸서리처라, 필멸자들아\r\n불의정령계의 군주",         //9
            "라바몬-오커로 진화하기전의 형태\r\n나름 귀엽게 생겼다",         // 10
            "그린 오커-언제든지 꿀밤을 때릴수있게\r\n항상 준비된 손동작을\r\n하고있다",        //11
            "핑크 오커-오커계의 일등신부감",        //12
            "복싱 오커-오커복싱계의 세계챔피언\r\n한대맞으면 정신을 못차린다\r\n조심하자",         //13
            "블루 오커-길쭉한 혀로 굉장한 위화감을\r\n형성한다. 이 친구 그래도 \r\n볼매다",     //14
        };

}
