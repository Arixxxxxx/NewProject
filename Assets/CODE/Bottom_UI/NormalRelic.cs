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
    float percentage;
    float Percnetage 
    { 
        get => percentage; 
        set 
        { 
            percentage = value;
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
        LvText.text = $"Lv = {GameStatus.inst.AryNormalRelicLv[(int)relicTag]}";
        ExText.text = Explane;
        Percnetage = Mathf.Pow(1.1f, GameStatus.inst.AryNormalRelicLv[(int)relicTag]);
        
        PriceText.text = "가격 정해야됨";
        upBtn.onClick.AddListener(ClickUp);
    }

     void ClickUp()
    {
        GameStatus.inst.AryNormalRelicLv[(int)relicTag]++;
        LvText.text = $"Lv {GameStatus.inst.AryNormalRelicLv[(int)relicTag]}";
        Percnetage = Mathf.Pow(1.1f, GameStatus.inst.AryNormalRelicLv[(int)relicTag]);
    }
}
