using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DogamMonsterSlot : MonoBehaviour
{
    Button mybtn;
    Image myItemIMG;
    [SerializeField]
    int myNumber;
    GameObject MaskIMG;
    TMP_Text myNumberText;
    TMP_Text mainText;
    [HideInInspector] public int curLv;
    // 정수 수집완료시
    public bool complete;

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
        mainText = transform.Find("NumberBox/MainText").GetComponent<TMP_Text>();
        myNumber = number + 1; // No1 으로 시작할수있게
        myNumberText.text = $"No.{myNumber}";

        // 내 버튼 초기화
        mybtn = GetComponent<Button>();
        mybtn.onClick.AddListener(() => DogamManager.inst.Set_MonsterMainViewr(number)); // 부모에서 컨트롤

        // Mask
        MaskIMG = transform.Find("NumberBox/Mask").gameObject;

         Set_current_Soulcount_Update();
    }

    public void Set_current_Soulcount_Update()
    {
        if (complete) { return; }

        curLv  = DogamManager.inst.Get_monster_Soul()[myNumber-1];
        
        if(curLv < DogamManager.inst.MaxSoulCount)
        {
            mainText.text = $"{curLv} / {DogamManager.inst.MaxSoulCount}";
            MaskIMG.SetActive(true);
        }
        else
        {
            curLv = DogamManager.inst.MaxSoulCount;
            mainText.text = "수집 완료";
            MaskIMG.SetActive(false);
            complete = true;
        }
    }

    public bool master = false;
    public void MaskActiveFalse()
    {
        if (MaskIMG.activeSelf == false) { return; }

        MaskIMG.SetActive(false);
        master = true;
    }


}
