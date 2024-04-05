using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterManager : MonoBehaviour
{
    public static LetterManager inst;

    [SerializeField] GameObject letter;
    [SerializeField] Queue<GameObject> letterQue = new Queue<GameObject>();

    GameObject fontUIRef;
    GameObject postOfficeRef;
    Button xBtn;
 

    ///// ���� ������ ��������
    GameObject letterViewr, letterBox, notthingLetter;

    //���� ���� �˸�â
    GameObject alrimWindow;
    Image alrimSprite;
    Button alrimDisableBtn;
    Transform letterPool;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        fontUIRef = GameObject.Find("---[FrontUICanvas]").gameObject;
        postOfficeRef = fontUIRef.transform.Find("PostOffice").gameObject;
        xBtn = postOfficeRef.transform.Find("Window/Title/X_Btn").GetComponent<Button>();
        letterViewr = postOfficeRef.transform.Find("Window/Scroll View").gameObject;
        letterBox = letterViewr.transform.Find("Viewport/Content").gameObject;
        notthingLetter = postOfficeRef.transform.Find("Window/NotthingLetter").gameObject;
        letterPool = postOfficeRef.transform.Find("Window/LetterPool").GetComponent<Transform>();

        alrimWindow = postOfficeRef.transform.Find("Alrim").gameObject;
        alrimSprite = alrimWindow.transform.Find("Window/Frame_LayOut/IMG_Frame/IMG").GetComponent<Image>();
        alrimDisableBtn = alrimWindow.transform.Find("Window/Button").GetComponent<Button>();


        BtnInIt();
        LetterPoolInit();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void BtnInIt()
    {
        xBtn.onClick.AddListener(() => postOfficeRef.SetActive(false));
    }

    //���� �� ���� ����
    private void LetterPoolInit()
    {
        GameObject obj;

        for (int count = 0; count < 10; count++)
        {
            obj = Instantiate(letter, letterPool);
            letterQue.Enqueue(obj);
            obj.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���� ������
    /// </summary>
    /// <param name="ItemType"> 0���,1���,2��</param>
    /// <param name="From"> �߽��� (Ex : ����GM) </param>
    /// <param name="text"> �� ���� (Ex : Lv1 , ����Ʈ���� ��)</param>
    /// <param name="ItemCount"> ���� �Ǵ� �������� ���� </param>
    public void MakeLetter(int ItemType, string From, string text, int ItemCount)
    {
        if (letterQue.Count <= 0)
        {
            GameObject objs = Instantiate(letter, letterPool);
            letterQue.Enqueue(objs);
            objs.gameObject.SetActive(false);
        }

        GameObject obj = letterQue.Dequeue();
        obj.GetComponent<LetterPrefab>().Set_Letter(ItemType, From, text, ItemCount);
        obj.transform.SetParent(letterBox.transform);
        obj.SetActive(true);
        WorldUI_Manager.inst.OnEnableRedSimball(0, true); // �ɺ� ����

        LetterBoxOnlyInit(); // �ֽ�ȭ�ѹ���
    }

    /// <summary>
    /// ���ſϷ�� ���� ȸ��
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnLetter(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(letterPool);
        letterQue.Enqueue(obj);
    }

    /// <summary>
    /// ������ ȣ��
    /// </summary>
    /// <param name="value"></param>
    public void OpenPostOnOfficeAndInit(bool value)
    {
        if (value == true)
        {
            int childCount = letterBox.transform.childCount;

            if (childCount > 0)
            {
                letterViewr.gameObject.SetActive(true);
                notthingLetter.gameObject.SetActive(false);
            }
            else if (childCount <= 0)
            {
                letterViewr.gameObject.SetActive(false);
                notthingLetter.gameObject.SetActive(true);
            }
        }

        postOfficeRef.SetActive(value);
    }

    public void LetterBoxOnlyInit()
    {
        int childCount = letterBox.transform.childCount;

        if (childCount > 0)
        {
            letterViewr.gameObject.SetActive(true);
            notthingLetter.gameObject.SetActive(false);
        }
        else if (childCount <= 0)
        {
            letterViewr.gameObject.SetActive(false);
            notthingLetter.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// ���� ����â => ���� ��ư ��������
    /// </summary>
    /// <param name="value"></param>
    public void alrimWindowAcitveTrueAndInit(Sprite itemSprite, int itemType, int itemCount, GameObject letterObj)
    {
        // �̹��� ��ü���ְ�
        alrimSprite.sprite = itemSprite;
        // �޾ƾ��� ���� ��ü


        alrimWindow.SetActive(true);
        alrimDisableBtn.onClick.RemoveAllListeners();
        alrimDisableBtn.onClick.AddListener(() => alrimWindowAcitveFalse(itemType, itemCount, letterObj)); // ���� ����

    }

    /// <summary>
    /// ���� ����â => ���� ��ư ����
    /// </summary>
    /// <param name="value"></param>
    public void alrimWindowAcitveFalse(int itemType, int itemCount, GameObject letterObj)
    {
        switch (itemType) // ���� �ڿ� �־���
        {
            case 0:
                GameStatus.inst.Ruby += itemCount;
                break;

            case 1:
                GameStatus.inst.GetGold(itemCount.ToString());
                break;

            case 2:
                GameStatus.inst.PlusStar(itemCount.ToString());
                break;
        }

        ReturnLetter(letterObj);

        int count = letterBox.transform.childCount;
        if (count == 0)
        {
            // Text Init Ǯ�� �ý��� ����
            WorldUI_Manager.inst.OnEnableRedSimball(0, false);
            OpenPostOnOfficeAndInit(true); //
        }

        alrimWindow.SetActive(false);
    }
}
