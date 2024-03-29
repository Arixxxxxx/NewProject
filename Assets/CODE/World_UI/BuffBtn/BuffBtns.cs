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
            case buffType.ATK: // ����â ���ݷ�����
                GameStatus.inst.BuffAddATK = CalCulator.inst.StringAndIntMultiPly(CalCulator.inst.Get_ATKtoString(), 4);
                break;

            case buffType.Gold: // ��� ȹ�淮����
                GameStatus.inst.BuffAddGold = CalCulator.inst.StringAndIntMultiPly(UIManager.Instance.GetTotalGold(), 2);
                break;

            case buffType.Speed: // �̵��ӵ� ����
                GameStatus.inst.BuffAddSpeed = 2f;
                break;

            case buffType.AD_ATK: //�ΰ��� ȭ�� ������ ���ݷ�����
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

                // ���ǵ� ���� �߰��ؾ���
                GameStatus.inst.BuffAddSpeed = 1.0f;
                break;
        }
    }



}
