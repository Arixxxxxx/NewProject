using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pet : MonoBehaviour
{

    [SerializeField] int baseCost;
    [SerializeField] PetType type;
    int nextSoul;
    int nextBorn;
    int nextBook;
    Button upBtn;
    Button DetailBtn;
    TMP_Text SoulText;
    TMP_Text BornText;
    TMP_Text BookText;
    TMP_Text lvText;

    void Start()
    {
        upBtn = transform.Find("Button").GetComponent<Button>();
        DetailBtn = transform.Find("imageBtn").GetComponent<Button>();
        SoulText = transform.Find("Button/Soul/PriceText").GetComponent<TMP_Text>();
        BornText = transform.Find("Button/Born/PriceText").GetComponent<TMP_Text>();
        BookText = transform.Find("Button/Book/PriceText").GetComponent<TMP_Text>();
        lvText = transform.Find("LvText").GetComponent<TMP_Text>();

        setNextCost();
        upBtn.onClick.AddListener(ClickUp);
        DetailBtn.onClick.AddListener(() => PetDetailViewr_UI.inst.TopArrayBtnActive(transform.GetSiblingIndex()));
    }

    public Button GetUpBtn()
    {
        return upBtn;
    }

    void ClickUp()
    {
        int[] petMoney = CrewGatchaContent.inst.Get_CurCrewUpgreadMaterial();

        if (petMoney[0] >= nextSoul && petMoney[1] >= nextBorn && petMoney[2] >= nextBook)
        {
            CrewGatchaContent.inst.Use_Crew_Material(0, nextSoul);
            CrewGatchaContent.inst.Use_Crew_Material(1, nextBorn);
            CrewGatchaContent.inst.Use_Crew_Material(2, nextBook);
            switch (type)
            {
                case PetType.AtkPet:
                    GameStatus.inst.Pet0_Lv++;
                    setNextCost();
                    break;

                case PetType.BuffPet:
                    GameStatus.inst.Pet1_Lv++;
                    setNextCost();
                    break;

                case PetType.GoldPet:
                    GameStatus.inst.Pet2_Lv++;
                    setNextCost();
                    break;
            }
        }
    }

    void setNextCost()
    {
        switch (type)
        {
            case PetType.AtkPet:
                nextSoul = baseCost + GameStatus.inst.Pet0_Lv * 100;
                nextBorn = baseCost + GameStatus.inst.Pet0_Lv * 100;
                nextBook = baseCost + GameStatus.inst.Pet0_Lv * 100;
                lvText.text = $"Lv.{GameStatus.inst.Pet0_Lv}";
                break;

            case PetType.BuffPet:
                nextSoul = baseCost + GameStatus.inst.Pet1_Lv * 100;
                nextBorn = baseCost + GameStatus.inst.Pet1_Lv * 100;
                nextBook = baseCost + GameStatus.inst.Pet1_Lv * 100;
                lvText.text = $"Lv.{GameStatus.inst.Pet1_Lv}";
                break;

            case PetType.GoldPet:
                nextSoul = baseCost + GameStatus.inst.Pet2_Lv * 100;
                nextBorn = baseCost + GameStatus.inst.Pet2_Lv * 100;
                nextBook = baseCost + GameStatus.inst.Pet2_Lv * 100;
                lvText.text = $"Lv.{GameStatus.inst.Pet2_Lv}";
                break;
        }
        SoulText.text = $"{nextSoul.ToString("N0")}";
        BornText.text = $"{nextBorn.ToString("N0")}";
        BookText.text = $"{nextBook.ToString("N0")}";
    }
}