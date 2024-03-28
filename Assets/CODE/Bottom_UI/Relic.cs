using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Relic : MonoBehaviour, ITypeGetable
{
    [SerializeField] RankType rankNum;
    [SerializeField] ItemTag itemNum;
    [SerializeField] TextMeshProUGUI LvText;
    [SerializeField] Sprite m_Sprite;

    int lv = 0;
    public int Lv
    {
        get => lv;
        set
        {
            lv = value;
            LvText.text = $"{lv}";
        }
    }

    private void Start()
    {
        LvText.text = $"{Lv}";
    }

    public virtual void ClickUp()
    {
        Lv++;
    }

    public Vector2 GetMyType()
    {
        return new Vector2((int)rankNum, (int)itemNum);
    }

    public Sprite GetSprite()
    {
        return m_Sprite;
    }
}
