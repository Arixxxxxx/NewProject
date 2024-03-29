using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffBtns : MonoBehaviour
{
    public enum buffType
    {
        ATK, Gold, Speed, AD_ATK
    }

    public buffType whichBuff;

    Button btn;
    GameObject activeCheck;
    bool isok;
    private void Awake()
    {
        btn = GetComponent<Button>();
        activeCheck = transform.GetChild(0).gameObject;
    }
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            WorldUI_Manager.inst.buffSelectUIWindowAcitve(true);
        });
    }

    private void Update()
    {
        if(activeCheck.activeSelf == false  && isok ==true)
        {
            isok = false;
            BuffStatsReset();
        }
        else if(activeCheck.activeSelf == true && isok == false)
        {
            isok = true;
            BuffStatsAdd();
        }
    }


    private void BuffStatsAdd()
    {
        switch (whichBuff)
        {
            case buffType.ATK: // 버프창 공격력증가
                GameStatus.inst.BuffAddATK = CalCulator.inst.StringAndIntMultiPly(CalCulator.inst.Get_ATKtoString(), 4);
                break;

            case buffType.Gold: // 골드 획득량증가
                GameStatus.inst.BuffAddGold = CalCulator.inst.StringAndIntMultiPly(UIManager.Instance.GetTotalGold(), 2);
                break;

            case buffType.Speed: // 이동속도 증가
                GameStatus.inst.BuffAddSpeed = 2f;
                break;

            case buffType.AD_ATK: //인게임 화면 광고보고 공격력증가
                GameStatus.inst.BuffAddAdATK = CalCulator.inst.StringAndIntMultiPly(CalCulator.inst.Get_ATKtoString(), 8);
                break;
        }
    }

    private void BuffStatsReset()
    {
        switch (whichBuff)
        {
            case buffType.ATK:

                GameStatus.inst.BuffAddATK = "0";

                break;

            case buffType.Gold:
                GameStatus.inst.BuffAddGold = "0";
                break;

            case buffType.Speed:

                // 스피드 버프 추가해야함
                GameStatus.inst.BuffAddSpeed = 1.0f;
                break;
        }
    }



}
