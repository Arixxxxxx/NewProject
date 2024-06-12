using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterBoxIcon : MonoBehaviour
{
    public enum ItemType { Ruby, Gold, Star }

    public ItemType Type;

    TMP_Text valueText;
    
    private void Awake()
    {
        valueText = GetComponentInChildren<TMP_Text>(true);
    }
    private void Start()
    {
        Type = (ItemType)transform.GetSiblingIndex();
    }

    
    // 모두수락창 아이콘 초기화 및 켜주고 꺼주기    
    public void SetIconAndValue(int value)
    {
        if (value <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        if (valueText == null)
        {
            valueText = GetComponentInChildren<TMP_Text>(true);
        }

        switch (Type)
        {
            case ItemType.Ruby:
                //valueText.text = $"+ {value.ToString("N0")}";
                //break;

            case ItemType.Gold:
            case ItemType.Star:
                valueText.text = $"+ {CalCulator.inst.StringFourDigitAddFloatChanger(value.ToString())}";
                break;

        }

       
    }

}
