using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pet : MonoBehaviour
{

    [SerializeField] int baseCost;
    [SerializeField] PetType type;
    int lv;
    int Lv
    {
        get => lv;
        set
        {
            lv = value;
            switch (type)
            {
                case PetType.AtkPet:
                    GameStatus.inst.Pet0_Lv = value;

                    break;

                case PetType.BuffPet:
                    GameStatus.inst.Pet1_Lv = value;

                    break;

                case PetType.GoldPet:
                    GameStatus.inst.Pet2_Lv = value;

                    break;
            }
            setNextCost();
        }
    }
    int nextSoul;
    int nextBorn;
    int nextBook;
    Button upBtn;
    Button DetailBtn;
    Button BuyBtn;
    TMP_Text BuyPriceText;
    TMP_Text SoulText;
    TMP_Text BornText;
    TMP_Text BookText;
    TMP_Text lvText;

    void Start()
    {

    }

    public void initPet()
    {
        upBtn = transform.Find("Button").GetComponent<Button>();
        DetailBtn = transform.Find("imageBtn").GetComponent<Button>();
        BuyBtn = transform.Find("BuyBtn").GetComponent<Button>();
        BuyPriceText = transform.Find("BuyBtn/TextBox/PriceText").GetComponent<TMP_Text>();
        SoulText = transform.Find("Button/Soul/PriceText").GetComponent<TMP_Text>();
        BornText = transform.Find("Button/Born/PriceText").GetComponent<TMP_Text>();
        BookText = transform.Find("Button/Book/PriceText").GetComponent<TMP_Text>();
        lvText = transform.Find("LvText").GetComponent<TMP_Text>();

        BuyPriceText.text = baseCost.ToString();
        setNextCost();
        GameStatus.inst.OnRubyChanged.AddListener(checkHaveRuby);
        upBtn.onClick.AddListener(ClickUp);
        BuyBtn.onClick.AddListener(() =>
        {
            RubyPayment.inst.RubyPaymentUiActive(baseCost, () =>
            {
                Lv++;
                PetContollerManager.inst.PetActive((int)type);
                PetContollerManager.inst.CrewUnlock_Action((int)type, true);
                upBtn.gameObject.SetActive(true);
                BuyBtn.gameObject.SetActive(false);
            });
            //GameStatus.inst.Ruby -= baseCost;

        });
        DetailBtn.onClick.AddListener(() => PetDetailViewr_UI.inst.TopArrayBtnActive(transform.GetSiblingIndex()));
    }

    public Button GetUpBtn()
    {
        return upBtn;
    }

    void checkHaveRuby()
    {
        if (BuyBtn.gameObject.activeSelf)
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

    void ClickUp()
    {
        int[] petMoney = CrewGatchaContent.inst.Get_CurCrewUpgreadMaterial();

        if (petMoney[0] >= nextSoul && petMoney[1] >= nextBorn && petMoney[2] >= nextBook)
        {
            CrewGatchaContent.inst.Use_Crew_Material(0, nextSoul);
            CrewGatchaContent.inst.Use_Crew_Material(1, nextBorn);
            CrewGatchaContent.inst.Use_Crew_Material(2, nextBook);
            Lv++;
        }
    }

    void setNextCost()
    {
        nextSoul = baseCost + Lv * 100;
        nextBorn = baseCost + Lv * 100;
        nextBook = baseCost + Lv * 100;

        lvText.text = $"Lv.{Lv}";
        SoulText.text = $"{nextSoul.ToString("N0")}";
        BornText.text = $"{nextBorn.ToString("N0")}";
        BookText.text = $"{nextBook.ToString("N0")}";
    }
}