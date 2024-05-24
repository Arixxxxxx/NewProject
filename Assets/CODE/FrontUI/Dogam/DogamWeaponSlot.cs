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
        // �̹��� �ʱ�ȭ
        myItemIMG = transform.Find("Item").GetComponent<Image>();
        myItemIMG.sprite = itemIMG;
        
        // ���� NO.1 �ʱ�ȭ
        myNumberText = transform.Find("NumberBox/NumText").GetComponent<TMP_Text>();
        myNumber = number + 1; // No1 ���� �����Ҽ��ְ�
        myNumberText.text = $"No.{myNumber}";

        // �� ��ư �ʱ�ȭ
        mybtn = GetComponent<Button>();
        mybtn.onClick.AddListener(() => DogamManager.inst.Set_WeaponMainViewr(number)); // �θ𿡼� ��Ʈ��

        // Mask
        MaskIMG = transform.Find("Mask").gameObject;
    }

}
