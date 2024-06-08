using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pet : MonoBehaviour, IClickLvUpAble
{

    [SerializeField] int baseCost;
    [SerializeField] int releaseStage;
    [SerializeField] PetType type;
    int lv;
    int myMatTypeIndex;
    int Lv
    {
        get => lv;
        set
        {
            lv = value;
            switch (type)
            {
                case PetType.Bomb:
                    GameStatus.inst.Pet0_Lv = value;

                    break;

                case PetType.Panda:
                    GameStatus.inst.Pet1_Lv = value;

                    break;

                case PetType.Necromancer:
                    GameStatus.inst.Pet2_Lv = value;

                    break;
            }

            if (ishave == false && lv >= 1)
            {
                ishave = true;
                PetContollerManager.inst.PetActive((int)type);
                upBtn.gameObject.SetActive(true);
                BuyBtn.gameObject.SetActive(false);
            }
            setNextCost();
        }
    }
    bool ishave = false;
    bool isRelease = false;
    int nextCost;

    GameObject buyBtnTextBox, mask;
    Button upBtn;
    Button DetailBtn;
    Button BuyBtn;
    TMP_Text BuyPriceText;
    TMP_Text CostText;
    TMP_Text lvText;

    void Start()
    {
        switch (type)
        {
            case PetType.Bomb:
                myMatTypeIndex = 1;
                break;

            case PetType.Panda:
                myMatTypeIndex = 2;
                break;

            case PetType.Necromancer:
                myMatTypeIndex = 0;
                break;

        }

        //upBtn.onClick.AddListener(() => { AudioManager.inst.Play_Ui_SFX(1, 0.8f); });
    }

    public void initPet()
    {
        upBtn = transform.Find("LvUpBtn").GetComponent<Button>();
        DetailBtn = transform.Find("imageBtn").GetComponent<Button>();
        BuyBtn = transform.Find("BuyBtn").GetComponent<Button>();
        BuyPriceText = transform.Find("BuyBtn/TextBox/PriceText").GetComponent<TMP_Text>();
        CostText = transform.Find("LvUpBtn/Soul/PriceText").GetComponent<TMP_Text>();
        lvText = transform.Find("LvText").GetComponent<TMP_Text>();

        buyBtnTextBox = transform.Find("BuyBtn/TextBox").gameObject;
        mask = transform.Find("BuyBtn/BtnMask").gameObject;

        switch (type)
        {
            case PetType.Bomb:
                Lv = GameStatus.inst.Pet0_Lv;

                break;

            case PetType.Panda:
                Lv = GameStatus.inst.Pet1_Lv;

                break;

            case PetType.Necromancer:
                Lv = GameStatus.inst.Pet2_Lv;

                break;
        }

        BuyPriceText.text = baseCost.ToString();
        setNextCost();
        releasePet();

        //GameStatus.inst.OnRubyChanged.AddListener(checkHaveRuby);
        // 상시 열려있는걸로 변경

        GameStatus.inst.OnStageChanged.AddListener(releasePet);
        upBtn.onClick.AddListener(ClickUp);
        BuyBtn.onClick.AddListener(() =>
        {

            RubyPayment.inst.RubyPaymentUiActive(baseCost, () =>
            {
                Lv++;
                ishave = true;
                PetContollerManager.inst.CrewUnlock_Action((int)type, true);
            });
        });
        DetailBtn.onClick.AddListener(() => { PetDetailViewr_UI.inst.PetDetialviewrUI_Active(transform.GetSiblingIndex()); AudioManager.inst.Play_Ui_SFX(1, 0.8f); });
    }

    void checkHaveRuby()
    {
        if (isRelease && BuyBtn.gameObject.activeSelf)
        {
            int haveRuby = GameStatus.inst.Ruby;
            if (haveRuby >= baseCost)
            {
                BuyBtn.interactable = true;
            }
            else if (BuyBtn.interactable == true)
            {
                BuyBtn.interactable = false;
            }
        }
    }

    void checkHavePetMat()
    {
        int[] petMoney = GameStatus.inst.CrewMaterial;

        if (upBtn.gameObject.activeSelf && petMoney[myMatTypeIndex] >= nextCost && upBtn.interactable == false)
        {
            upBtn.interactable = true;
        }
        else if (upBtn.gameObject.activeSelf && petMoney[myMatTypeIndex] >= nextCost && upBtn.interactable == true)
        {
            upBtn.interactable = false;
        }
    }

    public void ClickUp()
    {

        int[] petMoney = GameStatus.inst.CrewMaterial;

        if (petMoney[myMatTypeIndex] >= nextCost) // 재료있을때
        {
            RubyPayment.inst.CrewMatPaymentUiActive(myMatTypeIndex, nextCost, () =>
            {
                PetContollerManager.inst.PetLvUp_WorldText_Active((int)type);
                GameStatus.inst.Use_crewMaterial(myMatTypeIndex, nextCost);
                Lv++;
            });
        }
        else// 재료 없을때
        {
            RubyPayment.inst.CrewMatPaymentUiActive(myMatTypeIndex, nextCost, () =>
            {
                GameStatus.inst.Use_crewMaterial(myMatTypeIndex, nextCost);
                Lv++;
            });
        }


    }

    void setNextCost()
    {
        nextCost = baseCost + Lv * 100;

        lvText.text = $"현재 Lv.{Lv}";
        CostText.text = $"{nextCost.ToString("N0")}";
    }

    void releasePet()
    {
        if (GameStatus.inst.StageLv >= releaseStage)
        {
            isRelease = true;

            mask.SetActive(false);
            buyBtnTextBox.SetActive(true);
            BuyBtn.interactable = true;
        }
    }
}