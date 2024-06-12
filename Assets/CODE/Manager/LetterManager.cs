using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterManager : MonoBehaviour
{
    public static LetterManager inst;

    [SerializeField] GameObject letter;
    Queue<GameObject> letterQue = new Queue<GameObject>();

    GameObject fontUIRef, worldUiRef;
    GameObject postOfficeRef;
    Button xBtn;

    ///// 메인 수신함 참조관련
    GameObject letterViewr, letterBox, notthingLetter;

    //수신 최종 알림창
    GameObject alrimWindow;
    Image alrimSprite;
    Button alrimDisableBtn;
    Transform letterPool;
    TMP_Text alrimCountText;

    //모두수락용
    Button everyAcceptBtn;
    List<LetterPrefab> letterList = new List<LetterPrefab>();



    GameObject EveryGetAlrimRef, EveryGetAlrimFreamlayOut;
    Button everyAcceptBackBtn;

    LetterBoxIcon[] letterbox;

    // 심볼
    GameObject simBall;

    // 우편 생성시 클래스 생성하여 저장
    List<SaveLetter> saveLetterList = new List<SaveLetter>();

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

        worldUiRef = GameManager.inst.WorldUiRef;
        fontUIRef = GameManager.inst.FrontUiRef;

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
        alrimCountText = alrimWindow.transform.Find("Window/CountText").GetComponent<TMP_Text>();

        simBall = worldUiRef.transform.Find("StageUI/Letter/SimBall").gameObject;

        BtnInIt();
        PrefabsPoolingAwake();
    }

    private void Start()
    {

    }
    private void BtnInIt()
    {
        xBtn.onClick.AddListener(() => OpenPostOnOfficeAndInit(false));
        everyAcceptBtn.onClick.AddListener(() =>
        {
            GetEveryLetter();
        });

        //모두 수락창 확인버튼
        everyAcceptBackBtn.onClick.AddListener(() =>
        {
            AudioManager.inst.Play_Ui_SFX(3, 1);
            EveryGetAlrimRef.SetActive(false);

            ////사용했던 박스들 종료
            //for(int index=0; index < letterbox.Length; index++)
            //{
            //    letterbox[index].gameObject.SetActive(false);
            //}
        }
        );
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
        simBall.gameObject.SetActive(true);

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



        // 기록
        saveLetterList.Add(new SaveLetter(ItemType, From, text, ItemCount, obj.GetComponent<LetterPrefab>()));



        LetterBoxOnlyInit(); // 최신화한번더
    }

    public void ListMyIDDelete(LetterPrefab thisNumber)
    {
        for (int index = 0; index < saveLetterList.Count; index++)
        {
            if (saveLetterList[index].letterPrefab == thisNumber)
            {
                Debug.Log($"{saveLetterList[index].letterText} 삭제");
                saveLetterList.RemoveAt(index);
            }
        }
    }

    /// <summary> 순서=> 2
    ///  알림윈도우 팝업시 해당창 초기화
    /// </summary>
    /// <param name="value"></param>
    public void alrimWindowAcitveTrueAndInit(Sprite itemSprite, int itemType, int itemCount, GameObject letterObj)
    {
        // 이미지 교체해주고
        alrimSprite.sprite = itemSprite;

        switch (itemType)
        {
            case 0:
                alrimCountText.text = $"루비 +{itemCount.ToString("N0")}";
                break;

            case 1:
                alrimCountText.text = $"골드 +{CalCulator.inst.StringFourDigitAddFloatChanger(itemCount.ToString())}";
                break;

            case 2:
                alrimCountText.text = $"별 +{CalCulator.inst.StringFourDigitAddFloatChanger(itemCount.ToString())}";
                break;
        }

        // 받아야할 갯수 교체

        alrimWindow.SetActive(true);
        alrimDisableBtn.onClick.RemoveAllListeners();
        alrimDisableBtn.onClick.AddListener(() =>
        {
            alrimWindowAcitveFalse(itemType, itemCount, letterObj);
        }); // 편지 리턴
    }



    /// <summary> 순서 => 3
    /// 편지 수락 누른후 뜨는 알림창 => 여기서 수락누르면 오브젝트 리턴시키고 
    /// </summary>
    /// <param name="itemType"> 0 = 루비 / 1 = 골드 / 2 = 별 </param>
    /// <param name="itemCount"> 갯수 </param>
    /// <param name="letterObj"> 리턴시킬 프리펩Obj </param>
    public void alrimWindowAcitveFalse(int itemType, int itemCount, GameObject letterObj)
    {
        ReturnLetter(letterObj);

        int count = letterBox.transform.childCount;
        if (count == 0)
        {
            // Text Init 풀링 시스템 리턴
            simBall.gameObject.SetActive(false);
            LetterBoxOnlyInit(); // 빈박스 표기
        }

        alrimWindow.SetActive(false);
    }

    //모두수락
    private void GetEveryLetter()
    {
        //초기화
        letterList.Clear();
        int[] saveLetterItemArr = new int[3];
        Array.Fill(saveLetterItemArr, 0);


        int childCount = letterBox.transform.childCount;
        if (childCount == 0) { AudioManager.inst.Play_Ui_SFX(4, 1); return; }

        AudioManager.inst.Play_Ui_SFX(9, 1);

        //모든 편지 내용물확인   
        for (int index = 0; index < childCount; index++)
        {
            LetterPrefab thisLetter = letterBox.transform.GetChild(index).GetComponent<LetterPrefab>();
            ListMyIDDelete(thisLetter); // 세이브 리스트에서 삭제
            letterList.Add(thisLetter);

            int[] check = thisLetter.ReturnThisLetterItemTypeAndCount(); // 타입 & 갯수가져옴
            saveLetterItemArr[check[0]] += check[1]; // 각 아이템타입 인덱스를 찾아 값을올려줌
        }


        // 자원 획득
        for (int index = 0; index < saveLetterItemArr.Length; index++)
        {
            switch (index)
            {
                case 0: //루비
                    if (saveLetterItemArr[index] != 0)
                    {
                        GameStatus.inst.PlusRuby(saveLetterItemArr[index]);
                    }
                    break;

                case 1: //골드
                    if (saveLetterItemArr[index] != 0)
                    {
                        GameStatus.inst.PlusGold(saveLetterItemArr[index].ToString());
                    }
                    break;

                case 2: //별
                    if (saveLetterItemArr[index] != 0)
                    {
                        GameStatus.inst.PlusStar(saveLetterItemArr[index].ToString());
                    }
                    break;
            }

            // 최종 수락한 아이템 결과창 초기화
            letterbox[index].SetIconAndValue(saveLetterItemArr[index]);
        }


        for (int index = 0; index < letterList.Count; index++)
        {
            letterList[index].ReturnObjPool();
        }

        //확인창 열어주고
        EveryGetAlrimRef.SetActive(true);
        simBall.gameObject.SetActive(false);
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
            AudioManager.inst.Play_Ui_SFX(4, 1);
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
        else
        {
            AudioManager.inst.Play_Ui_SFX(3, 1);
        }
        postOfficeRef.SetActive(value);
    }

    // 우편이없있을떄 없을때 우편함내 표기 ex => '수신함에 우편이없습니다'
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



    //남은 편지 받아가
    public List<SaveLetter> GetLeftLetter => saveLetterList;

    //시작할때 편지 생성
    public void LeftLetterMake(List<SaveLetter> list)
    {
        if (list.Count <= 0) { return; }

        for (int index = 0; index < list.Count; index++)
        {
            MakeLetter(list[index].itemtype, list[index].letterFrom, list[index].letterText, list[index].letterItemCount);
        }

    }




}

[Serializable]
public class SaveLetter
{
    public int itemtype;
    public string letterFrom;
    public string letterText;
    public int letterItemCount;
    public LetterPrefab letterPrefab;
    public SaveLetter(int itemtype, string letterFrom, string letterText, int letterItemCount, LetterPrefab letterPrefabs)
    {
        this.itemtype = itemtype;
        this.letterFrom = letterFrom;
        this.letterText = letterText;
        this.letterItemCount = letterItemCount;
        this.letterPrefab = letterPrefabs;
    }
}


