using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DogamWeaponSlot : MonoBehaviour
{
    Button mybtn;
    Image myItemIMG;
    int myNumber;
    GameObject MaskIMG;
    TMP_Text myNumberText;

    void Start()
    {


    }

    public void Init_Prefabs(Sprite itemIMG, int number)
    {
        // 이미지 초기화
        myItemIMG = transform.Find("Item").GetComponent<Image>();
        myItemIMG.sprite = itemIMG;
        
        // 무기 NO.1 초기화
        myNumberText = transform.Find("NumberBox/NumText").GetComponent<TMP_Text>();
        myNumber = number + 1; // No1 으로 시작할수있게
        myNumberText.text = $"No.{myNumber}";

        // 내 버튼 초기화
        mybtn = GetComponent<Button>();
        mybtn.onClick.AddListener(() => DogamManager.inst.Set_WeaponMainViewr(number)); // 부모에서 컨트롤

        // Mask
        MaskIMG = transform.Find("Mask").gameObject;
    }

}
