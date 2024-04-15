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

    Button upBtn;
    Image relicImgae;
    TextMeshProUGUI NameText;
    TextMeshProUGUI LvText;
    TextMeshProUGUI ExText;
    TextMeshProUGUI PercentText;
    TextMeshProUGUI PriceText;


    protected virtual void Start()
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
        LvText.text = $"Lv = {GameStatus.inst.AryNormalRelicLv[index]}";
        ExText.text = Explane;
        PercentText.text = "공식 정해야됨";
        PriceText.text = "가격 정해야됨";
        upBtn.onClick.AddListener(ClickUp);
    }

    protected virtual void ClickUp()
    {
        GameStatus.inst.AryNormalRelicLv[index]++;
        LvText.text = $"Lv {GameStatus.inst.AryNormalRelicLv[index]}";
    }

    public Sprite GetSprite()
    {
        return m_Sprite;
    }

    public void SetLv(int num)
    {
        GameStatus.inst.AryNormalRelicLv[index] += num;
        LvText.text = $"Lv = {GameStatus.inst.AryNormalRelicLv[index]}";

    }
    public Vector2 GetMyType()
    {
        return new Vector2((int)rankNum, (int)itemNum);
    }
}
