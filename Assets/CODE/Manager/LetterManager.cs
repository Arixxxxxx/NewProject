using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterManager : MonoBehaviour
{
    public static LetterManager inst;

    [SerializeField] GameObject letter;
    Queue<GameObject> letterQue = new Queue<GameObject>();

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


    //모두수락용
    Button everyAcceptBtn;
    List<LetterPrefab> letterList = new List<LetterPrefab>();
    int[] saveLetterItemDic = new int[3]; // 현재 아이템종류 3가지

    [SerializeField]
    GameObject EveryGetAlrimRef, EveryGetAlrimFreamlayOut;
    [SerializeField]
    Button everyAcceptBackBtn;
    [SerializeField]
    LetterBoxIcon[] letterbox;

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
        everyAcceptBtn = postOfficeRef.transform.Find("Window/everyAcceptBtn").GetComponent<Button>();

        alrimWindow = postOfficeRef.transform.Find("Alrim").gameObject;
        alrimSprite = alrimWindow.transform.Find("Window/Frame_LayOut/IMG_Frame/IMG").GetComponent<Image>();
        alrimDisableBtn = alrimWindow.transform.Find("Window/Button").GetComponent<Button>();

        EveryGetAlrimRef = postOfficeRef.transform.Find("EveryGetAlrim").gameObject;
        EveryGetAlrimFreamlayOut = EveryGetAlrimRef.transform.Find("Window/Frame_LayOut").gameObject;
        letterbox = EveryGetAlrimFreamlayOut.GetComponentsInChildren<LetterBoxIcon>(true);
        everyAcceptBackBtn = EveryGetAlrimRef.transform.Find("Window/Button").GetComponent<Button>();

        BtnInIt();
        PrefabsPoolingAwake();
    }

    private void Start()
    {
        
    }
    private void BtnInIt()
    {
        xBtn.onClick.AddListener(() => postOfficeRef.SetActive(false));
        everyAcceptBtn.onClick.AddListener(() =>
        {
            GetEveryLetter();
        });
        everyAcceptBackBtn.onClick.AddListener(() => EveryGetAlrimRef.SetActive(false));
    }

    //최초 빈 편지 생성
    private void PrefabsPoolingAwake()
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
        

        LetterBoxOnlyInit(); // 최신화한번더
    }


    /// <summary> 순서 . 2
    ///  알림윈도우 팝업시 해당창 초기화
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


    /// <summary> 순서. 3
    /// 편지 수락 누른후 뜨는 알림창 => 여기서 수락누르면 오브젝트 리턴시키고 
    /// </summary>
    /// <param name="itemType"> 0 = 루비 / 1 = 골드 / 2 = 별 </param>
    /// <param name="itemCount"> 갯수 </param>
    /// <param name="letterObj"> 리턴시킬 프리펩Obj </param>
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
            OpenPostOnOfficeAndInit(true); //
        }

        alrimWindow.SetActive(false);
    }

    //모두수락
    private void GetEveryLetter()
    {
        //초기화
        letterList.Clear();
        for (int index = 0; index < saveLetterItemDic.Length; index++)
        {
            saveLetterItemDic[index] = 0;
        }

        int childCount = letterBox.transform.childCount;
        if (childCount == 0) { return; }


        //모든 편지 내용물확인
        for (int index = 0; index < childCount; index++)
        {
            letterList.Add(letterBox.transform.GetChild(index).GetComponent<LetterPrefab>());
            int[] check = letterList[index].ReturnThisLetterItemTypeAndCount();
            saveLetterItemDic[check[0]] += check[1]; // 각 아이템타입 인덱스를 찾아 값을올려줌
        }


        // 자원 획득
        for (int index = 0; index < saveLetterItemDic.Length; index++)
        {
            switch (index)
            {
                case 0: //루비
                    GameStatus.inst.Ruby += saveLetterItemDic[index];
                    break;

                case 1: //골드
                    GameStatus.inst.PlusGold(saveLetterItemDic[index].ToString());
                    break;

                case 2: //별
                    GameStatus.inst.PlusStar(saveLetterItemDic[index].ToString());
                    break;
            }

            letterbox[index].SetIconAndValue(saveLetterItemDic[index]);
        }


        for (int index = 0; index < letterList.Count; index++)
        {
            letterList[index].ReturnObjPool();
        }

        //확인창 열어주고
        EveryGetAlrimRef.SetActive(true);
        LetterBoxOnlyInit(); // 빈박스 표기
    
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

            if (childCount > 0) // 수신된 우편이 있을때 뷰어 켜줌
            {
                letterViewr.gameObject.SetActive(true);
                notthingLetter.gameObject.SetActive(false);
            }
            else if (childCount <= 0) // 수신된 우편이 없다면 우편이없다는 텍스트
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
}
