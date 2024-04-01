using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LetterPrefab : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    Image mainIMG;

    GameObject textSpace;
    TMP_Text title;
    TMP_Text mainText;
    TMP_Text returnItemText;

    Button getBtn;
    private void Awake()
    {
        AwakeInit();
    }
    void Start()
    {
        
    }

    private void AwakeInit()
    {
        mainIMG = transform.Find("IMG_Frame/IMG").GetComponent<Image>();

        textSpace = transform.Find("TextSpace").gameObject;

        //텍스트 3종
        title = textSpace.transform.Find("Title_Text").GetComponent<TMP_Text>(); // 상단
        mainText = textSpace.transform.Find("Main_Text").GetComponent<TMP_Text>(); //중단
        returnItemText = textSpace.transform.Find("ReturnItem_Text").GetComponent<TMP_Text>(); //하단

        //버튼
        getBtn = transform.Find("MoveBtn").GetComponent<Button>();
    }


    /// <summary>
    ///  편지 생성기
    /// </summary>
    /// <param name="ItemType"> 0루비,1골드,2별</param>
    /// <param name="From"> 발신자 (Ex : 게임GM) </param>
    /// <param name="text"> 주 내용 (Ex : Lv1 , 퀘스트보상 등)</param>
    /// <param name="ItemCount"> 지급 되는 아이템의 갯수 </param>
    public void Set_Letter(int ItemType, string From, string text, int ItemCount)
    {
        if (mainIMG == null)
        {
            AwakeInit();
        }

        string itemTypetext = ItemType == 0 ? "루비" : ItemType == 1 ? "골드" : "별";
       
        mainIMG.sprite = sprites[ItemType];

        title.text = From;
        mainText.text = text;
        returnItemText.text = $"{itemTypetext}  +{ItemCount}";

        getBtn.onClick.RemoveAllListeners();
        getBtn.onClick.AddListener( ()=> 
        {
            //알림창 초기화 및 켜주기
            LetterManager.inst.alrimWindowAcitveTrueAndInit(mainIMG.sprite, ItemType, ItemCount, gameObject);
        });

    }

}
