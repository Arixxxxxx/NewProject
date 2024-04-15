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
            RelicManager.instance.SetAryPercent((int)relicTag ,value);
            PercentText.text = ((int)(Percnetage * 100f)).ToString() + "%";
        }
    }

    protected virtual void Start()
    {
        NameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        LvText = transform.Find("LvText").GetComponent<TextMeshProUGUI>();
        ExText = transform.Find("ExplaneText").GetComponent<TextMeshProUGUI>();
        PercentText = transform.Find("PercentageText").GetComponent<TextMeshProUGUI>();
        PriceText = transform.Find("Button/PriceText").GetComponent<TextMeshProUGUI>();
        upBtn = transform.Find("Button").GetComponent<Button>();

        NameText.text = Name;
        ExText.text = Explane;
        LvText.text = $"Lv = {Lv}";
        Percnetage = Mathf.Pow(1.1f, Lv);

        PriceText.text = "���� ���ؾߵ�";
        upBtn.onClick.AddListener(ClickUp);
    }

    void ClickUp()
    {
        Lv++;
        LvText.text = $"Lv {Lv}";
        Percnetage = Mathf.Pow(1.1f, Lv);
    }
}
