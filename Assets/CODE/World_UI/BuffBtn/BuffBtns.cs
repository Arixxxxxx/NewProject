using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffBtns : MonoBehaviour
{
    public enum buffType
    {
        ATK, Gold, Speed, AD_ATK, NewBie
    }

    public buffType whichBuff;

    Button btn;
    
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    void Start()
    {
        switch (whichBuff)
        {
            case buffType.ATK:
            case buffType.Gold:
            case buffType.Speed:
            case buffType.AD_ATK:
                btn.onClick.AddListener(() =>
                {
                    BuffManager.inst.Buff_UI_Active(true);
                });
                break;

            case buffType.NewBie:
                btn.onClick.AddListener(() =>
                {
                    Newbie_Content.inst.NewBieBuffInfoWindowActive(true); // 뉴비 버프 정보창
                });
                break;
        }
    }



    //// Ad버프는 꺼지고 켜지는거라 여기서 값을 0으로 바꿔줘야함
    ///
    private void OnEnable()
    {
        switch (whichBuff)
        {
            case buffType.NewBie:

                break;
        }

    }
    private void OnDisable()
    {
        switch (whichBuff)
        {
            case buffType.NewBie:

                break;
        }
    }
}
