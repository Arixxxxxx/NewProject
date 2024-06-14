using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyPlayCheckUIManager : MonoBehaviour
{
    public static DailyPlayCheckUIManager inst;
    [Header("# Input Ad Wide Btn <Color=yellow>( Sprite File )</Color>")]
    [Space]
    [SerializeField] Sprite[] adBtnSprite;

    GameObject worldFrontRef, dailyCheckObjRef, dailyWindowRef, ItemListRef, worldUiref;
    GameObject[] iconBG;
    int iconCount;

    Button xBtn;

    //�ޱ� ��ư Ȥ�� ����
    GameObject[] GetBtn = new GameObject[2];
    TMP_Text mainTaxt;

    // ������ �ʱ�ȭ��
    int itemCount;
    TMP_Text[] itemCountText;
    TMP_Text[] itemNumberText;
    GameObject[] SelectOutLine;
    GameObject[] gotItemCheck;


    Button adViewAndGetRubyBtn;

    // �˸� �ɺ�
    GameObject simBall;

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        // ���̶�Ű Ref
        worldUiref = GameManager.inst.WorldUiRef;
        worldFrontRef = GameManager.inst.FrontUiRef;

        dailyCheckObjRef = worldFrontRef.transform.Find("DailyCheck").gameObject;
        dailyWindowRef = dailyCheckObjRef.transform.Find("Window").gameObject;
        ItemListRef = dailyWindowRef.transform.Find("RubyList").gameObject;

        // Init �ʿ� ������Ʈ
        itemCount = ItemListRef.transform.childCount;
        itemCountText = new TMP_Text[itemCount];
        SelectOutLine = new GameObject[itemCount];
        gotItemCheck = new GameObject[itemCount];
        itemNumberText = new TMP_Text[itemCount];

        for (int index = 0; index < itemCount; index++)
        {
            itemCountText[index] = ItemListRef.transform.GetChild(index).Find("InBox/CountText").GetComponent<TMP_Text>();
            itemNumberText[index] = ItemListRef.transform.GetChild(index).Find("InBox/NumberText").GetComponent<TMP_Text>();
            SelectOutLine[index] = ItemListRef.transform.GetChild(index).Find("OutLine").gameObject;
            gotItemCheck[index] = ItemListRef.transform.GetChild(index).Find("Get_Active").gameObject;
        }



        // ��ư
        xBtn = dailyWindowRef.transform.Find("Title/X_Btn").GetComponent<Button>();
        adViewAndGetRubyBtn = dailyWindowRef.transform.Find("ShowADBtn").GetComponent<Button>();

        // ���� ��ư�κ� ����ġ
        GetBtn[0] = dailyWindowRef.transform.Find("TextLayOut/NoGet").gameObject;
        GetBtn[1] = dailyWindowRef.transform.Find("TextLayOut/Got").gameObject;
        mainTaxt = GetBtn[0].GetComponent<TMP_Text>();

        simBall = worldUiref.transform.Find("StageUI/DailyCheck/SimBall").gameObject;
        BtnInIt();
    }
    private void Start()
    {
        
    }
    private void Update()
    {

    }
    private void BtnInIt()
    {
        xBtn.onClick.AddListener(() => MainWindow_Acitve(false));


        adViewAndGetRubyBtn.onClick.AddListener(() => //���� ��ư
        {
            ADViewManager.inst.AdMob_ActiveAndFuntion(() =>
            {
                GameStatus.inst.PlusRuby(100);
                WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "��� +100");
                GameStatus.inst.DailyADRuby = true;
                adViewAndGetRubyBtn_Init(false);
            });
        });
    }

    
    /// <summary>
    /// �⼮üũ ȣ��
    /// </summary>
    /// <param name="value"></param>
    public void MainWindow_Acitve(bool value)
    {
        if (value)
        {
            AudioManager.inst.Play_Ui_SFX(4, 0.8f);
        }
        else
        {
            AudioManager.inst.Play_Ui_SFX(3, 0.8f);
        }

        dailyCheckObjRef.SetActive(value);
    }

    /// <summary>
    /// �⼮üũ ���� Ȱ��ȭ �� �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="Boolian"> true / false </param>
    public void DialyContent_Init(bool Boolian)
    {
        AutoDialyCheckReset();
        DailyCheck_Material_Init();

        GetBtnAcitve(!Boolian);
        simBall.SetActive(!Boolian);

        adViewAndGetRubyBtn_Init(!GameStatus.inst.DailyADRuby); // �ϴܺ� �����ư (1�Ͽ� �ѹ� ����) => �ٸ����̻�

        if (Boolian == false)
        {
            //��� ��� (������� �ؽ�Ʈ���� ����)
            int valueIndex = GameStatus.inst.GotDaily_Reward % 20;
            int value = int.Parse(ItemListRef.transform.GetChild(valueIndex).Find("InBox/CountText").GetComponent<TMP_Text>().text.Where(x => char.IsDigit(x)).ToArray());
            mainTaxt.text = $"  < {GameStatus.inst.GotDaily_Reward + 1}��° > �⼮üũ ����ޱ�\r\n - ������ <color=green>������</color>���� �߼۵˴ϴ�.";

            GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.RemoveAllListeners();
            GetBtn[0].transform.Find("GetGiftBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioManager.inst.Play_Ui_SFX(4, 1);
                LetterManager.inst.MakeLetter(0, "����GM", $"�⼮üũ {GameStatus.inst.GotDaily_Reward + 1}���� ����", value); // ���� ���� ȹ��

                GameStatus.inst.TotayGotDaily_Reward = true;
                simBall.SetActive(false);
                GameStatus.inst.GotDaily_Reward++; // ���� ī��Ʈ �÷���

                //��ư����
                DailyCheck_Material_Init();
                GetBtnAcitve(false); // ��ư ��Ȱ��ȭ
            });
        }
    }

    /// <summary>
    /// �⼮üũ ���� ���� ���� Init
    /// </summary>
    public void DailyCheck_Material_Init()
    {
        int count = GameStatus.inst.GotDaily_Reward % 20;
        if(count == 0)
        {
            AutoDialyCheckReset();

            //�޾����ű��� üũǥ��
            for (int index = 0; index < itemCount; index++)
            {
                gotItemCheck[index].SetActive(false);
            }

            //���� �׸� ǥ��
            for (int index = 0; index < itemCount; index++)
            {
                if (index == 0)
                {
                    SelectOutLine[index].SetActive(true);
                }
                else
                {
                    SelectOutLine[index].SetActive(false);
                }
            }
        }

        //�޾����ű��� üũǥ��
        for (int index = 0; index < count; index++)
        {
            gotItemCheck[index].SetActive(false);

            if (index < count)
            {
                gotItemCheck[index].SetActive(true);
            }
        }

        //���� �׸� ǥ��
        for (int index = 0; index < itemCount; index++)
        {
            if (index == count)
            {
                SelectOutLine[index].SetActive(true);
            }
            else
            {
                SelectOutLine[index].SetActive(false);
            }
        }
    }

    /// <summary>
    /// �⼮üũ �ϴ� �����ư Ȱ�� / ��Ȱ��
    /// </summary>
    /// <param name="value"> Active ? </param>
    private void adViewAndGetRubyBtn_Init(bool value)
    {
        if (value)
        {
            adViewAndGetRubyBtn.GetComponent<Image>().sprite = adBtnSprite[0]; //��������Ʈ ��ü
            adViewAndGetRubyBtn.onClick.RemoveAllListeners(); 
            adViewAndGetRubyBtn.onClick.AddListener(() => //���� ��ư
            {
                ADViewManager.inst.AdMob_ActiveAndFuntion(() =>
                {
                    GameStatus.inst.PlusRuby(100);
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "��� +100");
                    GameStatus.inst.DailyADRuby = true;
                    adViewAndGetRubyBtn_Init(false);
                });
            });
        }
        else
        {
            adViewAndGetRubyBtn.GetComponent<Image>().sprite = adBtnSprite[1]; //��������Ʈ ��ü
            adViewAndGetRubyBtn.onClick.RemoveAllListeners(); // ��ư��� -> â����� �ٲ�
            adViewAndGetRubyBtn.onClick.AddListener(() => MainWindow_Acitve(false));
        }
    }

    /// <summary>
    /// �⼮���� �����ܹڽ� ��ȣ �� ������ ���� �� ���� �ʱ�ȭ
    /// </summary>
    public void AutoDialyCheckReset()
    {
        if(GameStatus.inst.GotDaily_Reward % 20 == 0)
        {
            GameStatus.inst.MakeDailyRewardCount = GameStatus.inst.GotDaily_Reward;

        }

        int startCount = GameStatus.inst.MakeDailyRewardCount + 1;

        for(int index=0;  index< itemCount; index++) 
        {
            //�ڽ��� ��ȣ �ʱ�ȭ
            itemNumberText[index].text = startCount.ToString();

            //�ڽ��� ��� ���� �ʱ�ȭ
            itemCountText[index].text = $"��� +{startCount * 10}";
            startCount++;
        }
    }

    /// <summary>
    /// �ϴܺ� ��ư
    /// </summary>
    /// <param name="value"></param>
    public void GetBtnAcitve(bool value)
    {
        if (value == true)
        {
            GetBtn[0].gameObject.SetActive(true);
            GetBtn[1].gameObject.SetActive(false);
        }
        else
        {
            GetBtn[0].gameObject.SetActive(false);
            GetBtn[1].gameObject.SetActive(true);
        }
    }

}
