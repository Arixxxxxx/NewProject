using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NormalRelic : MonoBehaviour
{
    [SerializeField] string Name;
    [SerializeField] string Explane;
    [SerializeField] NormalRelicTag relicTag;

    Button upBtn;
    TextMeshProUGUI NameText;
    TextMeshProUGUI LvText;
    TextMeshProUGUI ExText;
    TextMeshProUGUI PercentText;
    TextMeshProUGUI PriceText;
    int lv;
    public int Lv
    {
        get => lv;
        set
        {
            lv = value;
            GameStatus.inst.AryNormalRelicLv[(int)relicTag] = value;
        }
    }
    float percentage;
    float Percnetage
    {
        get => percentage;
        set
        {
            percentage = value;
            GameStatus.inst.SetAryPercent((int)relicTag, value);
            PercentText.text = ((int)(Percnetage * 100f)).ToString() + "%";
        }
    }

    void Start()
    {
        NameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        ExText = transform.Find("TextBox/ExplaneText").GetComponent<TextMeshProUGUI>();
        PercentText = transform.Find("TextBox/PercentageText").GetComponent<TextMeshProUGUI>();
        PriceText = transform.Find("Button/PriceText").GetComponent<TextMeshProUGUI>();
        LvText = transform.Find("Button/LvText").GetComponent<TextMeshProUGUI>();
        upBtn = transform.Find("Button").GetComponent<Button>();
        NameText.text = $"{transform.GetSiblingIndex() + 1}. {Name}";
        ExText.text = Explane;
        LvText.text = $"Lv. {Lv}";
        setPercent();

        //PriceText.text = "가격 정해야됨";
        upBtn.onClick.AddListener(clickUp);
    }

    void clickUp()
    {
        switch (relicTag)
        {
            case NormalRelicTag.Atk:
                Lv++;
                break;
            case NormalRelicTag.AtkSpeed:
                Lv++;
                if (Lv >= 10)
                {
                    upBtn.interactable = false;
                    PriceText.text = "Max";
                }
                break;
            case NormalRelicTag.Critical:
                percentage = Lv;
                if (Lv >= 25)
                {
                    upBtn.interactable = false;
                    PriceText.text = "Max";
                }
                break;
        }
        Lv++;
        LvText.text = $"Lv {Lv}";
        setPercent();
    }

    void setPercent()
    {
        switch (relicTag)
        {
            case NormalRelicTag.Atk:
                percentage = 100 + Lv;
                break;
            case NormalRelicTag.AtkSpeed:
                percentage = 100 + 15 * Lv;
                break;
            case NormalRelicTag.Critical:
                percentage = Lv;
                break;
        }
    }
}
