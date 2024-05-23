using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicInfo_Prefbas : MonoBehaviour
{
    // Start is called before the first frame update

    Button mybtn;
    Image imgOutLine;
    Image itemIMG, maskIMG;
    TMP_Text numberText, ishaveText;


    // �θ��ȣ
    [SerializeField]
    int parentNumber;
    [SerializeField]
    int myNumber;
    // ����ȣ

    //������ȣ
    [SerializeField]
    int totalMyNumber;

    private void Awake()
    {
        AwakeInit();
    }

    void Start()
    {
        MyIMGInit();
    }

    private void AwakeInit()
    {
        imgOutLine = transform.Find("Case").GetComponent<Image>(); //������ �ƿ�����
        itemIMG = imgOutLine.transform.Find("Image").GetComponent<Image>(); // ������ �̹���
        numberText = imgOutLine.transform.Find("Number").GetComponent<TMP_Text>(); // No.1 ��ȣ
        ishaveText = transform.Find("HaveText").GetComponent<TMP_Text>(); // ��ȹ�� , ȹ�� (�ϴ�)
        maskIMG = transform.Find("Mask").GetComponent<Image>(); // ��ȹ��� ����

        parentNumber = transform.parent.parent.parent.transform.GetSiblingIndex();
        myNumber = transform.GetSiblingIndex();
       

        mybtn = GetComponent<Button>();
        mybtn.onClick.AddListener(() => RelicInfoManager.inst.Set_MainViewr(parentNumber, myNumber, totalMyNumber, myLv > 0 ? true : false));
    }

    private void MyIMGInit()
    {
        //�ƿ�����
        imgOutLine.sprite = RelicInfoManager.inst.Get_RelicIcon_OutLine_Sprite(parentNumber);
        //�̹���
        itemIMG.sprite = SpriteResource.inst.Relic_Sprite_TypeAndNumber(parentNumber, myNumber);

    }
    

    private void InitMyNumber()
    {
        // ������ȣ Ȯ��
        totalMyNumber = myNumber;


        if (parentNumber == 1)
        {
            totalMyNumber += SpriteResource.inst.Normal_Relic.Length;
        }
        else if (parentNumber == 2)
        {
            totalMyNumber += SpriteResource.inst.Normal_Relic.Length + SpriteResource.inst.Epic_relic_IMG.Length;
        }

        //��ȣ �ο�
        numberText.text = $"NO.{totalMyNumber + 1}";

    }


    Color nohaveColor = new Color(0.5f, 0.5f, 0.5f, 1);
    /// <summary>
    /// ���� ȹ�� ���� ���� 
    /// </summary>
    
    string myname;
    [SerializeField]
    int myLv = 0;
    /// <summary>
    /// ����â ���� ������ ȣ���ؼ� ȹ������ ��ȹ������ �����ؾ���
    /// </summary>
    public void Update_Current_Lv()
    {
        if (maskIMG == null)
        {
            AwakeInit();
        }

        InitMyNumber();

        myLv = GameStatus.inst.GetAryRelicLv(totalMyNumber);

        if (myLv == 0)
        {
            maskIMG.gameObject.SetActive(true);
            ishaveText.text = "�� ȹ ��";
            itemIMG.color = nohaveColor;
        }
        else if (myLv > 0)
        {
            maskIMG.gameObject.SetActive(false);
            ishaveText.text = "ȹ ��";
            ishaveText.enableVertexGradient = false;
            itemIMG.color = Color.white;
        }
    }

    public int MyLv() => myLv;

}
