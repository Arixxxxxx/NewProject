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


    // 부모번호
    [SerializeField]
    int parentNumber;
    [SerializeField]
    int myNumber;
    // 내번호

    //최종번호
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
        imgOutLine = transform.Find("Case").GetComponent<Image>(); //아이템 아웃라인
        itemIMG = imgOutLine.transform.Find("Image").GetComponent<Image>(); // 아이템 이미지
        numberText = imgOutLine.transform.Find("Number").GetComponent<TMP_Text>(); // No.1 번호
        ishaveText = transform.Find("HaveText").GetComponent<TMP_Text>(); // 미획득 , 획득 (하단)
        maskIMG = transform.Find("Mask").GetComponent<Image>(); // 미획득시 덮게

        parentNumber = transform.parent.parent.parent.transform.GetSiblingIndex();
        myNumber = transform.GetSiblingIndex();
       

        mybtn = GetComponent<Button>();
        mybtn.onClick.AddListener(() => RelicInfoManager.inst.Set_MainViewr(parentNumber, myNumber, totalMyNumber, myLv > 0 ? true : false));
    }

    private void MyIMGInit()
    {
        //아웃라인
        imgOutLine.sprite = RelicInfoManager.inst.Get_RelicIcon_OutLine_Sprite(parentNumber);
        //이미지
        itemIMG.sprite = SpriteResource.inst.Relic_Sprite_TypeAndNumber(parentNumber, myNumber);

    }
    

    private void InitMyNumber()
    {
        // 최종번호 확인
        totalMyNumber = myNumber;


        if (parentNumber == 1)
        {
            totalMyNumber += SpriteResource.inst.Normal_Relic.Length;
        }
        else if (parentNumber == 2)
        {
            totalMyNumber += SpriteResource.inst.Normal_Relic.Length + SpriteResource.inst.Epic_relic_IMG.Length;
        }

        //번호 부여
        numberText.text = $"NO.{totalMyNumber + 1}";

    }


    Color nohaveColor = new Color(0.5f, 0.5f, 0.5f, 1);
    /// <summary>
    /// 유물 획득 유무 리턴 
    /// </summary>
    
    string myname;
    [SerializeField]
    int myLv = 0;
    /// <summary>
    /// 도감창 열고 닫을때 호출해서 획득인지 미획득인지 구별해야함
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
            ishaveText.text = "미 획 득";
            itemIMG.color = nohaveColor;
        }
        else if (myLv > 0)
        {
            maskIMG.gameObject.SetActive(false);
            ishaveText.text = "획 득";
            ishaveText.enableVertexGradient = false;
            itemIMG.color = Color.white;
        }
    }

    public int MyLv() => myLv;

}
