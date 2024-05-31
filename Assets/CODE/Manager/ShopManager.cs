using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    public static ShopManager inst;

    //////////////////// < �ν����� ���� > ////////////////////////

    [Header("# BotArrayBtn Image <color=yellow>(Sprite)</color>")]
    [Space]
    [SerializeField]
    Sprite[] botArr_NonClickImage;
    [SerializeField]
    Sprite[] botArr_ClickImage;

    //////////////////////////////////////////////////////////////


    GameObject shopRef;
    Transform shopListRef;
    
    public GameObject ShopRef => shopRef; // ���� �۾� �����

    Button[] botArrBtn; // ���� �ϴ� ��ư
    Image[] botArrImage; // ���� �ϴ� �̵� ��ư �̹���
    TMP_Text[] botArrText;
    TMP_Text curRubyText;

    [Tooltip("0��í/1������/2������/3�������")] GameObject[] shopListChildRef;

    // ���� �����ִ� ��ȣ
    int curSelectMenu = -1;

    private void Awake()
    {
        //�̱���
        #region
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion 

        shopRef = transform.parent.Find("ScreenArea/BackGround/Shop").gameObject;
        shopListRef = shopRef.transform.Find("Shop_List");

        // ���� ����Ʈ �ʱ�ȭ
        shopListChildRef = new GameObject[shopListRef.childCount-1]; // CurRuby�� ����
        for (int index = 0; index < shopListChildRef.Length; index++)
        {
            shopListChildRef[index] = shopListRef.GetChild(index).gameObject;
        }

        // ���� ������� ���� �����Ȳ �ؽ�Ʈ
        curRubyText = shopListRef.Find("CurRuby/Box/Text (TMP)").GetComponent<TMP_Text>();

        //���� �ϴ� ��ư�ʱ�ȭ
        botArrBtn = shopRef.transform.Find("ShopBottomBtn").GetComponentsInChildren<Button>();
        botArrImage = new Image[botArrBtn.Length];
        botArrText = new TMP_Text[botArrBtn.Length];
        for (int index = 0; index < botArrBtn.Length; index++)
        {
            botArrImage[index] = botArrBtn[index].GetComponent<Image>();
            botArrText[index] = botArrImage[index].GetComponentInChildren<TMP_Text>();
        }


        Btn_Init();
    }

    private void Start()
    {

    }

    private void Btn_Init()
    {
        // �ϴ� �����̵� ��ư�� �ʱ�ȭ
        for (int index = 0;index < botArrBtn.Length; index++)
        {
            int curIndex = index;
            botArrBtn[curIndex].onClick.AddListener(() => Active_Shop(curIndex, true));
        }

        // 
    }



    /// <summary>
    /// ����ȣ��
    /// </summary>
    /// <param name="ShopTypeNumber">0�̱�/1���/2���/3���� </param>
    public void Active_Shop(int ShopTypeNumber, bool active)
    {
        if (active) // �ش� ���� ȣ��
        {
            
            // ���� �Ѿ�°� �ƴ϶�� ��ư�� ���
            if(curSelectMenu != -1 && curSelectMenu != ShopTypeNumber)
            {
                AudioManager.inst.PlaySFX(4, 0.8f);
            }

            //���� ��ư �� Ŭ���� ����
            if(curSelectMenu == ShopTypeNumber) { return; }

            //���� ���õǾ��ִ� ���� ����
            curSelectMenu = ShopTypeNumber;

            //�������� startInit
            switch (curSelectMenu)
            {
                //�̱�����̶��
                case 0:
                    Shop_Gacha.inst.Init(true);
                    break;
            }

            // �Һ��� �ʱ�ȭ
            curRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Ruby.ToString());

            // â �����ֱ�
            for (int index = 0; index < shopListChildRef.Length; index++)
            {
                if (index == ShopTypeNumber)
                {
                    shopListChildRef[index].SetActive(true);
                }
                else
                {
                    shopListChildRef[index].SetActive(false);
                }
            }

            // �̹��� ����
            BotArrBtn_ImageChanger(ShopTypeNumber);
        }
        else // ���� ����
        {
            switch (curSelectMenu) 
            {
                //�̱�����̶��
                case 0:
                    Shop_Gacha.inst.Init(false);
                    break;
            }

            curSelectMenu = -1;
            shopRef.SetActive(false);
        }
    }


        

    float clickFontsize = 11.5f;
    float nonclickFontsize = 10f;
    // �ϴܹ�ư �̹��� ����
    private void BotArrBtn_ImageChanger(int selectBtn)
    {
        for(int index=0; index < botArrImage.Length; index++)
        {
            if(index == selectBtn)
            {
                botArrImage[index].sprite = botArr_ClickImage[index];
                botArrText[index].fontSize = clickFontsize;
            }
            else
            {
                botArrImage[index].sprite = botArr_NonClickImage[index];
                botArrText[index].fontSize = nonclickFontsize;
            }
        }
    }

    public void ShopRubyTextInit()
    {
        curRubyText.text = CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Ruby.ToString());
    }
}

