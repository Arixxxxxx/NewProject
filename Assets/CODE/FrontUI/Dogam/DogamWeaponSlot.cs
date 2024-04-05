using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DogamWeaponSlot : MonoBehaviour
{
    [SerializeField][Tooltip("0 컬렉션 / 1 셀렉트(선택) / 2 미선택")] Sprite[] iconLayoutSprite;
    [SerializeField] Sprite[] weaponSprite;
    [SerializeField][Tooltip("0 흰색 / 1 어두운")] Color[] weaponColor;

    Image imgBox;
    Image weaponImg;
    Button thisBtn;

    // 좌측하단 번호숫자
    TMP_Text boxNum;

    int myNum;

    private void Awake()
    {
        if(imgBox == null)   { AwakeInit();  }
        
    }

    void Start()
    {

        thisBtn.onClick.AddListener(() =>
        {
            DogamManager.inst.CharactorWeaponImgChangerAndNumber(weaponImg.sprite, myNum); // 메인캐릭터 무기교체해주고 이전버튼 비활성화
            imgBox.sprite = iconLayoutSprite[1]; // 박스 활성화
        });

    }

    private void AwakeInit()
    {
        imgBox = GetComponent<Image>();
        weaponImg = transform.GetChild(0).GetComponent<Image>();
        thisBtn = GetComponent<Button>();
        boxNum = transform.GetChild(1).GetComponent<TMP_Text>();

        //  아이콘 초기화 ( 하이라키 오브젝트 순서기준)
        myNum = transform.GetSiblingIndex();
        weaponImg.sprite = weaponSprite[myNum];
        boxNum.text = (myNum + 1).ToString();
    }
    // 리셋 아이콘박스
    public void ResetIconBoxLayout()
    {
        if (imgBox == null)
        {
            AwakeInit();
        }
    
        if (DogamManager.inst.IsgotThisWeapon[myNum] == true) // 한번이라도 얻엇던적이잇는 친구니?
        {
            imgBox.sprite = iconLayoutSprite[0];
            weaponImg.color = weaponColor[0];
        }
        else
        {
            imgBox.sprite = iconLayoutSprite[2];
            weaponImg.color = weaponColor[1];
        }

        // 최초실행시 2번째 칸으로 고정
        if (DogamManager.inst.BeforeWeaponSelectNum == -1 && myNum == 1)
        {
            DogamManager.inst.CharactorWeaponImgChangerAndNumber(weaponImg.sprite, myNum); // 메인캐릭터 무기교체해주고 이전버튼 비활성화
            imgBox.sprite = iconLayoutSprite[1]; // 박스 활성화
      
        }
    }

}
