using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdShopBtn : MonoBehaviour
{
    public enum PrefabsType
    {
        gold, ruby, star, soul, bone, book
    }
    public PrefabsType itemType;

    // Ref
    Button btn;
    GameObject CooltimeRef;
    TMP_Text cooltimeText, canBuyText;

    void Start()
    {
        btn = GetComponent<Button>();

        //버튼 클릭 함수
        btn.onClick.AddListener(() => AdMarket.inst.ClickBtn((int)itemType));

        CooltimeRef = transform.Find("Cooltime").gameObject;
        cooltimeText = CooltimeRef.transform.Find("Text (TMP) (2)").GetComponent<TMP_Text>();
        canBuyText = transform.Find("Mid/Text (TMP) (1)").GetComponent<TMP_Text>();

        
        TextInit(); ;
    }

    
    void Update()
    {
        CheckCooltimeAndTextUpdate();
    }


    /// <summary>
    /// 버튼 비활성화 / 활성화
    /// </summary>
    /// <param name="value"> true / false </param>
    public void ActiveBtn(bool value)
    {
        TextInit();

        if (value)
        {
            btn.interactable = value;
            CooltimeRef.SetActive(false);
        }
        else // 수량 모두 소진
        {
            btn.interactable = false;
            CooltimeRef.SetActive(true);
        }
    }

    /// <summary>
    /// 수량 모두 소진시 기능
    /// </summary>
    public void CheckCooltimeAndTextUpdate()
    {
        if (CooltimeRef.activeSelf == false) { return; }

        int timeInSeconds = (int)AdMarket.inst.Get_CoolTime((int)itemType);

        // 시간 계산
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)((timeInSeconds % 3600) / 60);
        int seconds = (int)(timeInSeconds % 60);

        // 시, 분, 초 포매팅
        string timeFormatted = string.Format("{0:00} : {1:00} : {2:00}", hours, minutes, seconds);

        // UI Text에 표시
        cooltimeText.text = timeFormatted;
    }

    public void TextInit()
    {
        canBuyText.text = $"구매가능 수량 {AdMarket.inst.CurItemCount((int)itemType)}/{AdMarket.inst.MaxItemCount((int)itemType)}";
    }
  
}

