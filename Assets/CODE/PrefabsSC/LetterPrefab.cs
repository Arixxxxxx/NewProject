using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class LetterPrefab : MonoBehaviour
{

    Image mainIMG;

    GameObject textSpace;
    TMP_Text title;
    TMP_Text mainText;
    TMP_Text returnItemText;

    Button getBtn;

    //편지내용
    int letterItemType;
    string letterFrom;
    string letterText;
    int letterItemCount;

    // 편지내용 확인 변수들
    int[] itemtypeAndCount = new int[2];

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

        // 내부변수 초기화 (리턴용)
        itemtypeAndCount[0] = ItemType;
        itemtypeAndCount[1] = ItemCount;

        // 저장용
        letterItemType = ItemType;
        letterFrom = From;
        letterText = text;
        letterItemCount = ItemCount;


        // 이미지아이콘 및 텍스트 초기화
        string itemTypetext = ItemType == 0 ? "루비" : ItemType == 1 ? "골드" : "별";
        mainIMG.sprite = SpriteResource.inst.CoinIMG(ItemType);

        title.text = From;
        mainText.text = text;

        if(ItemType == 0)
        {
            returnItemText.text = $"{itemTypetext}  +{ItemCount.ToString("N0")}";
        }
        else
        {
            returnItemText.text = $"{itemTypetext}  +{CalCulator.inst.StringFourDigitAddFloatChanger(ItemCount.ToString())}";
        }

        getBtn.onClick.RemoveAllListeners();
        getBtn.onClick.AddListener(() =>
        {
            // 편지수락 알림창 초기화 및 켜주기
            LetterManager.inst.alrimWindowAcitveTrueAndInit(mainIMG.sprite, ItemType, ItemCount, gameObject);
            
            switch (ItemType) // 최종 자원 넣어줌
            {
                case 0:
                    GameStatus.inst.PlusRuby(ItemCount);
                    break;

                case 1:
                    GameStatus.inst.GetGold(ItemCount.ToString());
                    break;

                case 2:
                    GameStatus.inst.PlusStar(ItemCount.ToString());
                    break;
            }


        });

    }

    /// <summary>
    /// 편지의 들어있는 아이템타입과 수량을 리턴
    /// </summary>
    /// <returns></returns>
    public int[] ReturnThisLetterItemTypeAndCount()
    {
        return itemtypeAndCount;
    }

    /// <summary>
    /// 편지 사용후 풀로 돌아감
    /// </summary>
    public void ReturnObjPool()
    {
        LetterManager.inst.ReturnLetter(gameObject);
    }

}
