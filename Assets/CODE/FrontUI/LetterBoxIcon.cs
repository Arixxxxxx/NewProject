using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterBoxIcon : MonoBehaviour
{
    TMP_Text valueText;

    private void Awake()
    {
         valueText = GetComponentInChildren<TMP_Text>(true);
    }
    private void Start()
    {
        
    }
    // 모두수락창 아이콘 초기화 및 켜주고 꺼주기    
    public void SetIconAndValue(int value)
    {
        if (value == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }
             
        if(valueText == null)
        {
            valueText = GetComponentInChildren<TMP_Text>(true);
        }
        valueText.text = $"+{value.ToString()}";
    }
     
}
