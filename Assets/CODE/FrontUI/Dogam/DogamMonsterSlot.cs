using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DogamMonsterSlot : MonoBehaviour
{
    [SerializeField][Tooltip("0 �÷��� / 1 ����Ʈ(����) / 2 �̼���")] Sprite[] iconLayoutSprite;
    [SerializeField] Sprite[] mosterSprite;
    [SerializeField][Tooltip("0 ��� / 1 ��ο�")] Color[] monsterColor;

    Image imgBox;
    Image mosterImg;
    Button thisBtn;

    // �����ϴ� ��ȣ����
    TMP_Text boxNum;
    // �ϴ� �÷��� ����
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

        //  ������ �ʱ�ȭ ( ���̶�Ű ������Ʈ ��������)
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
                DogamManager.inst.MonsterIMGChanger(mosterImg.sprite, myNum); // �̹��� ��ü���ְ� ������ư ��Ȱ��ȭ
                imgBox.sprite = iconLayoutSprite[1]; // �ڽ� Ȱ��ȭ
                BottomCollectTextActive(false); // �ϴ� �÷��� �ؽ�Ʈ ����
            }
        });

    }

    
    // ���� �����ܹڽ�
    public void ResetIconBoxLayout()
    {
        if (imgBox == null)
        {
            AwakeInit();
        }

        if (DogamManager.inst.MonsterColltionCount[myNum] == 50) // ������ �Ϸ�?
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
            DogamManager.inst.MonsterIMGChanger(mosterImg.sprite, myNum); // �̹��� ��ü���ְ� ������ư ��Ȱ��ȭ
            imgBox.sprite = iconLayoutSprite[1]; // �ڽ� Ȱ��ȭ
            BottomCollectTextActive(false); // �ϴ� �÷��� �ؽ�Ʈ ����
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
