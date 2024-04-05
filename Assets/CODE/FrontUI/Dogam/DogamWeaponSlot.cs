using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DogamWeaponSlot : MonoBehaviour
{
    [SerializeField][Tooltip("0 �÷��� / 1 ����Ʈ(����) / 2 �̼���")] Sprite[] iconLayoutSprite;
    [SerializeField] Sprite[] weaponSprite;
    [SerializeField][Tooltip("0 ��� / 1 ��ο�")] Color[] weaponColor;

    Image imgBox;
    Image weaponImg;
    Button thisBtn;

    // �����ϴ� ��ȣ����
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
            DogamManager.inst.CharactorWeaponImgChangerAndNumber(weaponImg.sprite, myNum); // ����ĳ���� ���ⱳü���ְ� ������ư ��Ȱ��ȭ
            imgBox.sprite = iconLayoutSprite[1]; // �ڽ� Ȱ��ȭ
        });

    }

    private void AwakeInit()
    {
        imgBox = GetComponent<Image>();
        weaponImg = transform.GetChild(0).GetComponent<Image>();
        thisBtn = GetComponent<Button>();
        boxNum = transform.GetChild(1).GetComponent<TMP_Text>();

        //  ������ �ʱ�ȭ ( ���̶�Ű ������Ʈ ��������)
        myNum = transform.GetSiblingIndex();
        weaponImg.sprite = weaponSprite[myNum];
        boxNum.text = (myNum + 1).ToString();
    }
    // ���� �����ܹڽ�
    public void ResetIconBoxLayout()
    {
        if (imgBox == null)
        {
            AwakeInit();
        }
    
        if (DogamManager.inst.IsgotThisWeapon[myNum] == true) // �ѹ��̶� ����������մ� ģ����?
        {
            imgBox.sprite = iconLayoutSprite[0];
            weaponImg.color = weaponColor[0];
        }
        else
        {
            imgBox.sprite = iconLayoutSprite[2];
            weaponImg.color = weaponColor[1];
        }

        // ���ʽ���� 2��° ĭ���� ����
        if (DogamManager.inst.BeforeWeaponSelectNum == -1 && myNum == 1)
        {
            DogamManager.inst.CharactorWeaponImgChangerAndNumber(weaponImg.sprite, myNum); // ����ĳ���� ���ⱳü���ְ� ������ư ��Ȱ��ȭ
            imgBox.sprite = iconLayoutSprite[1]; // �ڽ� Ȱ��ȭ
      
        }
    }

}
