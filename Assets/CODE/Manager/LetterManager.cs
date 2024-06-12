using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterManager : MonoBehaviour
{
    public static LetterManager inst;

    [SerializeField] GameObject letter;
    Queue<GameObject> letterQue = new Queue<GameObject>();

    GameObject fontUIRef, worldUiRef;
    GameObject postOfficeRef;
    Button xBtn;

    ///// ���� ������ ��������
    GameObject letterViewr, letterBox, notthingLetter;

    //���� ���� �˸�â
    GameObject alrimWindow;
    Image alrimSprite;
    Button alrimDisableBtn;
    Transform letterPool;
    TMP_Text alrimCountText;

    //��μ�����
    Button everyAcceptBtn;
    List<LetterPrefab> letterList = new List<LetterPrefab>();



    GameObject EveryGetAlrimRef, EveryGetAlrimFreamlayOut;
    Button everyAcceptBackBtn;

    LetterBoxIcon[] letterbox;

    // �ɺ�
    GameObject simBall;

    // ���� ������ Ŭ���� �����Ͽ� ����
    List<SaveLetter> saveLetterList = new List<SaveLetter>();

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

        worldUiRef = GameManager.inst.WorldUiRef;
        fontUIRef = GameManager.inst.FrontUiRef;

        postOfficeRef = fontUIRef.transform.Find("PostOffice").gameObject;
        xBtn = postOfficeRef.transform.Find("Window/Title/X_Btn").GetComponent<Button>();
        letterViewr = postOfficeRef.transform.Find("Window/Scroll View").gameObject;
        letterBox = letterViewr.transform.Find("Viewport/Content").gameObject;
        notthingLetter = postOfficeRef.transform.Find("Window/NotthingLetter").gameObject;
        letterPool = postOfficeRef.transform.Find("Window/LetterPool").GetComponent<Transform>();
        everyAcceptBtn = postOfficeRef.transform.Find("Window/everyAcceptBtn").GetComponent<Button>();

        alrimWindow = postOfficeRef.transform.Find("Alrim").gameObject;
        alrimSprite = alrimWindow.transform.Find("Window/Frame_LayOut/IMG_Frame/IMG").GetComponent<Image>();
        alrimDisableBtn = alrimWindow.transform.Find("Window/Button").GetComponent<Button>();

        EveryGetAlrimRef = postOfficeRef.transform.Find("EveryGetAlrim").gameObject;
        EveryGetAlrimFreamlayOut = EveryGetAlrimRef.transform.Find("Window/Frame_LayOut").gameObject;
        letterbox = EveryGetAlrimFreamlayOut.GetComponentsInChildren<LetterBoxIcon>(true);
        everyAcceptBackBtn = EveryGetAlrimRef.transform.Find("Window/Button").GetComponent<Button>();
        alrimCountText = alrimWindow.transform.Find("Window/CountText").GetComponent<TMP_Text>();

        simBall = worldUiRef.transform.Find("StageUI/Letter/SimBall").gameObject;

        BtnInIt();
        PrefabsPoolingAwake();
    }

    private void Start()
    {

    }
    private void BtnInIt()
    {
        xBtn.onClick.AddListener(() => OpenPostOnOfficeAndInit(false));
        everyAcceptBtn.onClick.AddListener(() =>
        {
            GetEveryLetter();
        });

        //��� ����â Ȯ�ι�ư
        everyAcceptBackBtn.onClick.AddListener(() =>
        {
            AudioManager.inst.Play_Ui_SFX(3, 1);
            EveryGetAlrimRef.SetActive(false);

            ////����ߴ� �ڽ��� ����
            //for(int index=0; index < letterbox.Length; index++)
            //{
            //    letterbox[index].gameObject.SetActive(false);
            //}
        }
        );
    }

    //���� �� ���� ����
    private void PrefabsPoolingAwake()
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
        simBall.gameObject.SetActive(true);

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



        // ���
        saveLetterList.Add(new SaveLetter(ItemType, From, text, ItemCount, obj.GetComponent<LetterPrefab>()));



        LetterBoxOnlyInit(); // �ֽ�ȭ�ѹ���
    }

    public void ListMyIDDelete(LetterPrefab thisNumber)
    {
        for (int index = 0; index < saveLetterList.Count; index++)
        {
            if (saveLetterList[index].letterPrefab == thisNumber)
            {
                Debug.Log($"{saveLetterList[index].letterText} ����");
                saveLetterList.RemoveAt(index);
            }
        }
    }

    /// <summary> ����=> 2
    ///  �˸������� �˾��� �ش�â �ʱ�ȭ
    /// </summary>
    /// <param name="value"></param>
    public void alrimWindowAcitveTrueAndInit(Sprite itemSprite, int itemType, int itemCount, GameObject letterObj)
    {
        // �̹��� ��ü���ְ�
        alrimSprite.sprite = itemSprite;

        switch (itemType)
        {
            case 0:
                alrimCountText.text = $"��� +{itemCount.ToString("N0")}";
                break;

            case 1:
                alrimCountText.text = $"��� +{CalCulator.inst.StringFourDigitAddFloatChanger(itemCount.ToString())}";
                break;

            case 2:
                alrimCountText.text = $"�� +{CalCulator.inst.StringFourDigitAddFloatChanger(itemCount.ToString())}";
                break;
        }

        // �޾ƾ��� ���� ��ü

        alrimWindow.SetActive(true);
        alrimDisableBtn.onClick.RemoveAllListeners();
        alrimDisableBtn.onClick.AddListener(() =>
        {
            alrimWindowAcitveFalse(itemType, itemCount, letterObj);
        }); // ���� ����
    }



    /// <summary> ���� => 3
    /// ���� ���� ������ �ߴ� �˸�â => ���⼭ ���������� ������Ʈ ���Ͻ�Ű�� 
    /// </summary>
    /// <param name="itemType"> 0 = ��� / 1 = ��� / 2 = �� </param>
    /// <param name="itemCount"> ���� </param>
    /// <param name="letterObj"> ���Ͻ�ų ������Obj </param>
    public void alrimWindowAcitveFalse(int itemType, int itemCount, GameObject letterObj)
    {
        ReturnLetter(letterObj);

        int count = letterBox.transform.childCount;
        if (count == 0)
        {
            // Text Init Ǯ�� �ý��� ����
            simBall.gameObject.SetActive(false);
            LetterBoxOnlyInit(); // ��ڽ� ǥ��
        }

        alrimWindow.SetActive(false);
    }

    //��μ���
    private void GetEveryLetter()
    {
        //�ʱ�ȭ
        letterList.Clear();
        int[] saveLetterItemArr = new int[3];
        Array.Fill(saveLetterItemArr, 0);


        int childCount = letterBox.transform.childCount;
        if (childCount == 0) { AudioManager.inst.Play_Ui_SFX(4, 1); return; }

        AudioManager.inst.Play_Ui_SFX(9, 1);

        //��� ���� ���빰Ȯ��   
        for (int index = 0; index < childCount; index++)
        {
            LetterPrefab thisLetter = letterBox.transform.GetChild(index).GetComponent<LetterPrefab>();
            ListMyIDDelete(thisLetter); // ���̺� ����Ʈ���� ����
            letterList.Add(thisLetter);

            int[] check = thisLetter.ReturnThisLetterItemTypeAndCount(); // Ÿ�� & ����������
            saveLetterItemArr[check[0]] += check[1]; // �� ������Ÿ�� �ε����� ã�� �����÷���
        }


        // �ڿ� ȹ��
        for (int index = 0; index < saveLetterItemArr.Length; index++)
        {
            switch (index)
            {
                case 0: //���
                    if (saveLetterItemArr[index] != 0)
                    {
                        GameStatus.inst.PlusRuby(saveLetterItemArr[index]);
                    }
                    break;

                case 1: //���
                    if (saveLetterItemArr[index] != 0)
                    {
                        GameStatus.inst.PlusGold(saveLetterItemArr[index].ToString());
                    }
                    break;

                case 2: //��
                    if (saveLetterItemArr[index] != 0)
                    {
                        GameStatus.inst.PlusStar(saveLetterItemArr[index].ToString());
                    }
                    break;
            }

            // ���� ������ ������ ���â �ʱ�ȭ
            letterbox[index].SetIconAndValue(saveLetterItemArr[index]);
        }


        for (int index = 0; index < letterList.Count; index++)
        {
            letterList[index].ReturnObjPool();
        }

        //Ȯ��â �����ְ�
        EveryGetAlrimRef.SetActive(true);
        simBall.gameObject.SetActive(false);
        LetterBoxOnlyInit(); // ��ڽ� ǥ��

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
            AudioManager.inst.Play_Ui_SFX(4, 1);
            int childCount = letterBox.transform.childCount;

            if (childCount > 0) // ���ŵ� ������ ������ ��� ����
            {
                letterViewr.gameObject.SetActive(true);
                notthingLetter.gameObject.SetActive(false);
            }
            else if (childCount <= 0) // ���ŵ� ������ ���ٸ� �����̾��ٴ� �ؽ�Ʈ
            {
                letterViewr.gameObject.SetActive(false);
                notthingLetter.gameObject.SetActive(true);

            }
        }
        else
        {
            AudioManager.inst.Play_Ui_SFX(3, 1);
        }
        postOfficeRef.SetActive(value);
    }

    // �����̾������� ������ �����Գ� ǥ�� ex => '�����Կ� �����̾����ϴ�'
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



    //���� ���� �޾ư�
    public List<SaveLetter> GetLeftLetter => saveLetterList;

    //�����Ҷ� ���� ����
    public void LeftLetterMake(List<SaveLetter> list)
    {
        if (list.Count <= 0) { return; }

        for (int index = 0; index < list.Count; index++)
        {
            MakeLetter(list[index].itemtype, list[index].letterFrom, list[index].letterText, list[index].letterItemCount);
        }

    }




}

[Serializable]
public class SaveLetter
{
    public int itemtype;
    public string letterFrom;
    public string letterText;
    public int letterItemCount;
    public LetterPrefab letterPrefab;
    public SaveLetter(int itemtype, string letterFrom, string letterText, int letterItemCount, LetterPrefab letterPrefabs)
    {
        this.itemtype = itemtype;
        this.letterFrom = letterFrom;
        this.letterText = letterText;
        this.letterItemCount = letterItemCount;
        this.letterPrefab = letterPrefabs;
    }
}


