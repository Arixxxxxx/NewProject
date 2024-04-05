using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterManager : MonoBehaviour
{
    public static LetterManager inst;

    [SerializeField] GameObject letter;
    [SerializeField] Queue<GameObject> letterQue = new Queue<GameObject>();

    GameObject fontUIRef;
    GameObject postOfficeRef;
    Button xBtn;
 

    ///// 메인 수신함 참조관련
    GameObject letterViewr, letterBox, notthingLetter;

    //수신 최종 알림창
    GameObject alrimWindow;
    Image alrimSprite;
    Button alrimDisableBtn;
    Transform letterPool;

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
        fontUIRef = GameObject.Find("---[FrontUICanvas]").gameObject;
        postOfficeRef = fontUIRef.transform.Find("PostOffice").gameObject;
        xBtn = postOfficeRef.transform.Find("Window/Title/X_Btn").GetComponent<Button>();
        letterViewr = postOfficeRef.transform.Find("Window/Scroll View").gameObject;
        letterBox = letterViewr.transform.Find("Viewport/Content").gameObject;
        notthingLetter = postOfficeRef.transform.Find("Window/NotthingLetter").gameObject;
        letterPool = postOfficeRef.transform.Find("Window/LetterPool").GetComponent<Transform>();

        alrimWindow = postOfficeRef.transform.Find("Alrim").gameObject;
        alrimSprite = alrimWindow.transform.Find("Window/Frame_LayOut/IMG_Frame/IMG").GetComponent<Image>();
        alrimDisableBtn = alrimWindow.transform.Find("Window/Button").GetComponent<Button>();


        BtnInIt();
        LetterPoolInit();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void BtnInIt()
    {
        xBtn.onClick.AddListener(() => postOfficeRef.SetActive(false));
    }

    //최초 빈 편지 생성
    private void LetterPoolInit()
    {
        GameObject obj;

        for (int count = 0; count < 10; count++)
        {
            obj = Instantiate(letter, letterPool);
            letterQue.Enqueue(obj);
            obj.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 편지 생성기
    /// </summary>
    /// <param name="ItemType"> 0루비,1골드,2별</param>
    /// <param name="From"> 발신자 (Ex : 게임GM) </param>
    /// <param name="text"> 주 내용 (Ex : Lv1 , 퀘스트보상 등)</param>
    /// <param name="ItemCount"> 지급 되는 아이템의 갯수 </param>
    public void MakeLetter(int ItemType, string From, string text, int ItemCount)
    {
        if (letterQue.Count <= 0)
        {
            GameObject objs = Instantiate(letter, letterPool);
            letterQue.Enqueue(objs);
            objs.gameObject.SetActive(false);
        }

        GameObject obj = letterQue.Dequeue();
        obj.GetComponent<LetterPrefab>().Set_Letter(ItemType, From, text, ItemCount);
        obj.transform.SetParent(letterBox.transform);
        obj.SetActive(true);
        WorldUI_Manager.inst.OnEnableRedSimball(0, true); // 심볼 켜줌

        LetterBoxOnlyInit(); // 최신화한번더
    }

    /// <summary>
    /// 수신완료된 편지 회수
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnLetter(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(letterPool);
        letterQue.Enqueue(obj);
    }

    /// <summary>
    /// 수신함 호출
    /// </summary>
    /// <param name="value"></param>
    public void OpenPostOnOfficeAndInit(bool value)
    {
        if (value == true)
        {
            int childCount = letterBox.transform.childCount;

            if (childCount > 0)
            {
                letterViewr.gameObject.SetActive(true);
                notthingLetter.gameObject.SetActive(false);
            }
            else if (childCount <= 0)
            {
                letterViewr.gameObject.SetActive(false);
                notthingLetter.gameObject.SetActive(true);
            }
        }

        postOfficeRef.SetActive(value);
    }

    public void LetterBoxOnlyInit()
    {
        int childCount = letterBox.transform.childCount;

        if (childCount > 0)
        {
            letterViewr.gameObject.SetActive(true);
            notthingLetter.gameObject.SetActive(false);
        }
        else if (childCount <= 0)
        {
            letterViewr.gameObject.SetActive(false);
            notthingLetter.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 최종 승인창 => 우편 버튼 눌렀을때
    /// </summary>
    /// <param name="value"></param>
    public void alrimWindowAcitveTrueAndInit(Sprite itemSprite, int itemType, int itemCount, GameObject letterObj)
    {
        // 이미지 교체해주고
        alrimSprite.sprite = itemSprite;
        // 받아야할 갯수 교체


        alrimWindow.SetActive(true);
        alrimDisableBtn.onClick.RemoveAllListeners();
        alrimDisableBtn.onClick.AddListener(() => alrimWindowAcitveFalse(itemType, itemCount, letterObj)); // 편지 리턴

    }

    /// <summary>
    /// 최종 승인창 => 우편 버튼 끌때
    /// </summary>
    /// <param name="value"></param>
    public void alrimWindowAcitveFalse(int itemType, int itemCount, GameObject letterObj)
    {
        switch (itemType) // 최종 자원 넣어줌
        {
            case 0:
                GameStatus.inst.Ruby += itemCount;
                break;

            case 1:
                GameStatus.inst.GetGold(itemCount.ToString());
                break;

            case 2:
                GameStatus.inst.PlusStar(itemCount.ToString());
                break;
        }

        ReturnLetter(letterObj);

        int count = letterBox.transform.childCount;
        if (count == 0)
        {
            // Text Init 풀링 시스템 리턴
            WorldUI_Manager.inst.OnEnableRedSimball(0, false);
            OpenPostOnOfficeAndInit(true); //
        }

        alrimWindow.SetActive(false);
    }
}
