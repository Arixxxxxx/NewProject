using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DogamMonsterSlot : MonoBehaviour
{
    [SerializeField][Tooltip("0 컬렉션 / 1 셀렉트(선택) / 2 미선택")] Sprite[] iconLayoutSprite;
    [SerializeField] Sprite[] mosterSprite;
    [SerializeField][Tooltip("0 흰색 / 1 어두운")] Color[] monsterColor;

    Image imgBox;
    Image mosterImg;
    Button thisBtn;

    // 좌측하단 번호숫자
    TMP_Text boxNum;
    // 하단 컬렉션 수집
    int myCollectionCount = 0;
    public int MyCollectionCount
    {
        get { return myCollectionCount; }
        set { MyCollectionCount = value; }
    }

    TMP_Text collectionText;

    int myNum;

    private void Awake()
    {
        if (imgBox == null) { AwakeInit(); }

    }
    private void AwakeInit()
    {
        imgBox = GetComponent<Image>();
        mosterImg = transform.GetChild(0).GetComponent<Image>();
        thisBtn = GetComponent<Button>();
        boxNum = transform.GetChild(1).GetComponent<TMP_Text>();
        collectionText = transform.GetChild(2).GetComponent<TMP_Text>();

        //  아이콘 초기화 ( 하이라키 오브젝트 순서기준)
        myNum = transform.GetSiblingIndex();
        mosterImg.sprite = mosterSprite[myNum];
        boxNum.text = (myNum + 1).ToString();
    }


    void Start()
    {

        thisBtn.onClick.AddListener(() =>
        {
            if(DogamManager.inst.BeforeMonsterSelectNum != myNum)
            {
                DogamManager.inst.MonsterIMGChanger(mosterImg.sprite, myNum); // 이미지 교체해주고 이전버튼 비활성화
                imgBox.sprite = iconLayoutSprite[1]; // 박스 활성화
                BottomCollectTextActive(false); // 하단 컬렉션 텍스트 꺼줌
            }
        });

    }

    
    // 리셋 아이콘박스
    public void ResetIconBoxLayout()
    {
        if (imgBox == null)
        {
            AwakeInit();
        }

        if (DogamManager.inst.MonsterColltionCount[myNum] == 50) // 도감작 완료?
        {
            imgBox.sprite = iconLayoutSprite[0];
            mosterImg.color = monsterColor[0];
            BottomCollectTextActive(false);
        }
        else
        {
            imgBox.sprite = iconLayoutSprite[2];
            mosterImg.color = monsterColor[1];
            BottomCollectTextActive(true);
        }

        if(DogamManager.inst.BeforeMonsterSelectNum == myNum)
        {
            DogamManager.inst.MonsterIMGChanger(mosterImg.sprite, myNum); // 이미지 교체해주고 이전버튼 비활성화
            imgBox.sprite = iconLayoutSprite[1]; // 박스 활성화
            BottomCollectTextActive(false); // 하단 컬렉션 텍스트 꺼줌
        }
    }

    public void Set_CollectionCount(int value)
    {
        if(collectionText == null)
        {
            collectionText = transform.GetChild(2).GetComponent<TMP_Text>();
        }

        collectionText.text = $"{value} / 50";
    }

    public void BottomCollectTextActive(bool value)
    {
        collectionText.gameObject.SetActive(value);
    }
}
