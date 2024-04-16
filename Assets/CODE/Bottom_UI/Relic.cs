using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Relic : MonoBehaviour, ITypeGetable
{
    [SerializeField] int index;
    [SerializeField] RankType rankNum;
    [SerializeField] ItemTag itemNum;
    [SerializeField] string Name;
    [SerializeField] Sprite m_Sprite;
    [SerializeField] string Explane;
    int lv;
    public int Lv
    {
        get => lv;
        set
        {
            lv = value;
            LvText.text = $"Lv = {lv}";
        }
    }
    Button upBtn;
    Image relicImgae;
    TextMeshProUGUI NameText;
    TextMeshProUGUI LvText;
    TextMeshProUGUI ExText;
    TextMeshProUGUI PercentText;
    TextMeshProUGUI PriceText;


    protected virtual void Start()
    {

    }

    public void initRelic()
    {
        NameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        LvText = transform.Find("LvText").GetComponent<TextMeshProUGUI>();
        ExText = transform.Find("ExplaneText").GetComponent<TextMeshProUGUI>();
        PercentText = transform.Find("PercentageText").GetComponent<TextMeshProUGUI>();
        PriceText = transform.Find("Button/PriceText").GetComponent<TextMeshProUGUI>();
        upBtn = transform.Find("Button").GetComponent<Button>();
        relicImgae = transform.Find("Image").GetComponent<Image>();

        relicImgae.sprite = m_Sprite;
        NameText.text = Name;
        LvText.text = $"Lv = {lv}";
        ExText.text = Explane;
        PercentText.text = "���� ���ؾߵ�";
        PriceText.text = "���� ���ؾߵ�";
        upBtn.onClick.AddListener(ClickUp);
    }

    protected virtual void ClickUp()
    {
        Lv++;
    }

    public Sprite GetSprite()
    {
        return m_Sprite;
    }

    public Vector2 GetMyType()
    {
        return new Vector2((int)rankNum, (int)itemNum);
    }
}
