using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop_Gacha : MonoBehaviour
{

    public static Shop_Gacha inst;

    GameObject shopRef, gachaShopRef;
    [SerializeField]
    Gacha relicGachaSc;
    CrewGatchaContent crewGachaSc;
    GachaBox_Animator boxSc;

    //현재 상태 추적
    enum SelectType { MaterialGacha , RelicGacha}
    SelectType curMode;

    // 모드변경 버튼
    Animator boxAnim;
    Button[] modeSwapBtn;
    public bool isChange;
    GameObject[] maskIMG = new GameObject[2];
    GameObject[] buyBtnArr = new GameObject[2];

    Button[] crewMaterialBuyBtn;
    Button[] relicBuyBtn;
    
    private void Awake()
    {
        //싱글톤
        #region
        if (inst == null) 
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
        shopRef = ShopManager.inst.ShopRef;
        gachaShopRef = shopRef.transform.Find("Shop_List/Gacha_Shop").gameObject;

        relicGachaSc = UIManager.Instance.GetComponent<Gacha>();
        crewGachaSc = CrewGatchaContent.inst;

        boxAnim = gachaShopRef.transform.Find("MainViewr").GetComponent<Animator>();
        boxSc = boxAnim.GetComponent<GachaBox_Animator>();
        modeSwapBtn = gachaShopRef.transform.Find("Cacha_Type_Btn").GetComponentsInChildren<Button>();
        maskIMG[0] = modeSwapBtn[0].transform.Find("Mask").gameObject;
        maskIMG[1] = modeSwapBtn[1].transform.Find("Mask").gameObject;
        buyBtnArr[0] = gachaShopRef.transform.Find("buyBtnArr").GetChild(0).gameObject;
        crewMaterialBuyBtn = buyBtnArr[0].transform.GetComponentsInChildren<Button>();

        buyBtnArr[1] = gachaShopRef.transform.Find("buyBtnArr").GetChild(1).gameObject;
        relicBuyBtn = buyBtnArr[1].transform.GetComponentsInChildren<Button>();

        BtnInit();
    }
    void Start()
    {
        
    }

    private void BtnInit()
    {
        modeSwapBtn[0].onClick.AddListener(() =>
        {
            if(isChange == true || curMode == SelectType.MaterialGacha) { return; }
            isChange = true;
            
            curMode = SelectType.MaterialGacha;
            BtnMaskChanger((int)curMode);
            boxAnim.SetTrigger(curMode.ToString());
        });

        modeSwapBtn[1].onClick.AddListener(() =>
        {
            if (isChange == true || curMode == SelectType.RelicGacha) { return; }
            isChange = true;

            curMode = SelectType.RelicGacha;
            BtnMaskChanger((int)curMode);
            boxAnim.SetTrigger(curMode.ToString());
        });
    }

    // 상단 모드변경 버튼 마스크 OnOFF
    private void BtnMaskChanger(int value)
    {
        for (int index = 0; index < maskIMG.Length; index++) 
        {
            if(index == value)
            {
                maskIMG[index].gameObject.SetActive(false);
                buyBtnArr[index].gameObject.SetActive(true);
            }
            else
            {
                maskIMG[index].gameObject.SetActive(true);
                buyBtnArr[index].gameObject.SetActive(false);
            }
        }
    }

    // ShopManager에서 통제
    public void Init(bool active)
    {
        if (isChange == true)
        {
            isChange = false;
        }

        boxSc.AllParticleActiveFalse();

        if (active)
        {
            BtnMaskChanger(0);
            curMode = SelectType.MaterialGacha;
        }
    }
   


}
