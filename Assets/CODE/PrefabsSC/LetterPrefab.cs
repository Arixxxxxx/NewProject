using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class LetterPrefab : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    Image mainIMG;

    GameObject textSpace;
    TMP_Text title;
    TMP_Text mainText;
    TMP_Text returnItemText;

    Button getBtn;

    //��������
    public int letterItemType;
    public string letterFrom;
    public string letterText;
    public int letterItemCount;

    // �������� Ȯ�� ������
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

        //�ؽ�Ʈ 3��
        title = textSpace.transform.Find("Title_Text").GetComponent<TMP_Text>(); // ���
        mainText = textSpace.transform.Find("Main_Text").GetComponent<TMP_Text>(); //�ߴ�
        returnItemText = textSpace.transform.Find("ReturnItem_Text").GetComponent<TMP_Text>(); //�ϴ�

        //��ư
        getBtn = transform.Find("MoveBtn").GetComponent<Button>();
    }


    /// <summary>
    ///  ���� ������
    /// </summary>
    /// <param name="ItemType"> 0���,1���,2��</param>
    /// <param name="From"> �߽��� (Ex : ����GM) </param>
    /// <param name="text"> �� ���� (Ex : Lv1 , ����Ʈ���� ��)</param>
    /// <param name="ItemCount"> ���� �Ǵ� �������� ���� </param>
    public void Set_Letter(int ItemType, string From, string text, int ItemCount)
    {
        if(mainIMG == null)
        {
            AwakeInit();
        }

        // ���κ��� �ʱ�ȭ (���Ͽ�)
        itemtypeAndCount[0] = ItemType;
        itemtypeAndCount[1] = ItemCount;

        // �����
        letterItemType = ItemType;
        letterFrom = From;
        letterText = text;
        letterItemCount = ItemCount;

        // �̹��������� �� �ؽ�Ʈ �ʱ�ȭ
        string itemTypetext = ItemType == 0 ? "���" : ItemType == 1 ? "���" : "��";
        mainIMG.sprite = sprites[ItemType];

        title.text = From;
        mainText.text = text;
        returnItemText.text = $"{itemTypetext}  +{ItemCount.ToString("N0")}";

        getBtn.onClick.RemoveAllListeners();
        getBtn.onClick.AddListener( ()=> 
        {
            // �������� �˸�â �ʱ�ȭ �� ���ֱ�
            LetterManager.inst.alrimWindowAcitveTrueAndInit(mainIMG.sprite, ItemType, ItemCount, gameObject);
            LetterManager.inst.RemoveLetter(this);
            switch (ItemType) // ���� �ڿ� �־���
            {
                case 0:
                    GameStatus.inst.Ruby += ItemCount;
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
    /// ������ ����ִ� ������Ÿ�԰� ������ ����
    /// </summary>
    /// <returns></returns>
    public int[] ReturnThisLetterItemTypeAndCount()
    {
        return itemtypeAndCount;
    }

    /// <summary>
    /// ���� ����� Ǯ�� ���ư�
    /// </summary>
    public void ReturnObjPool()
    {
        LetterManager.inst.ReturnLetter(gameObject);
    }

}
