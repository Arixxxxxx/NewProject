using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    public static ShopManager inst;

    //////////////////// < 인스펙터 참조 > ////////////////////////

    [Header("# BotArrayBtn Image <color=yellow>(Sprite)</color>")]
    [Space]
    [SerializeField]
    Sprite[] botArr_NonClickImage;
    [SerializeField]
    Sprite[] botArr_ClickImage;

    //////////////////////////////////////////////////////////////


    GameObject shopRef;
    // 동은 작업용
    public GameObject ShopRef => shopRef;

    Button[] botArrBtn; // 상점 하단 버튼
    Image[] botArrImage; // 상점 하단 이동 버튼 이미지
    TMP_Text[] botArrText;


    [Tooltip("0갓챠/1골드상점/2루비상점/3광고상점")] GameObject[] shopListRef;

    // 현재 눌려있는 번호
    int curSelectMenu = -1;

    private void Awake()
    {
        //싱글톤
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


        // 상점 리스트 초기화
        shopListRef = new GameObject[shopRef.transform.Find("Shop_List").childCount];
        for (int index = 0; index < shopListRef.Length; index++)
        {
            shopListRef[index] = shopRef.transform.Find("Shop_List").GetChild(index).gameObject;
        }


        //상점 하단 버튼초기화
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
        // 하단 상점이동 버튼부 초기화
        for (int index = 0;index < botArrBtn.Length; index++)
        {
            int curIndex = index;
            botArrBtn[curIndex].onClick.AddListener(() => Active_Shop(curIndex, true));
        }

        // 
    }



    /// <summary>
    /// 상점호출
    /// </summary>
    /// <param name="ShopTypeNumber">0뽑기/1골드/2루비/3광고 </param>
    public void Active_Shop(int ShopTypeNumber, bool active)
    {
        if (active) // 해당 상점 호출
        {
            
            // 최초 넘어온게 아니라면 버튼음 재생
            if(curSelectMenu != -1 && curSelectMenu != ShopTypeNumber)
            {
                AudioManager.inst.PlaySFX(4);
            }

            //동일 버튼 또 클릭시 리턴
            if(curSelectMenu == ShopTypeNumber) { return; }

            //현재 선택되어있는 상점 추적
            curSelectMenu = ShopTypeNumber;

            // 창 열어주기
            for (int index = 0; index < shopListRef.Length; index++)
            {
                if (index == ShopTypeNumber)
                {
                    shopListRef[index].SetActive(true);
                }
                else
                {
                    shopListRef[index].SetActive(false);
                }
            }

            // 이미지 변경
            BotArrBtn_ImageChanger(ShopTypeNumber);
        }
        else // 상점 종료
        {
            curSelectMenu = -1;
            shopRef.SetActive(false);
        }
    }


        

    float clickFontsize = 11.5f;
    float nonclickFontsize = 10f;
    // 하단버튼 이미지 변경
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
}

