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
    GameObject activeCheck;
    bool isok;
    private void Awake()
    {
        btn = GetComponent<Button>();
        activeCheck = transform.GetChild(0).gameObject; // ���������� �� �ڽ� ������Ʈ 'Active'�� Enable �������� ����Ȱ��ȭ üũ
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
                    WorldUI_Manager.inst.buffSelectUIWindowAcitve(true);
                });
                break;



            case buffType.NewBie:

                btn.onClick.AddListener(() =>
                {
                    Newbie_Content.inst.NewBieBuffInfoWindowActive(true); // ���� ���� ����â
                });
                //if (GameStatus.inst.IsNewBie == false)
                //{
                //    activeCheck.gameObject.SetActive(false);
                //    gameObject.SetActive(false);
                //}
                
                break;

        }

       
    }

    private void Update()
    {
        switch (whichBuff) 
        {
            case buffType.ATK:
            case buffType.Speed:
            case buffType.Gold:

                if (activeCheck.activeSelf == false && isok == true)
                {
                    isok = false;
                    BuffStatsReset();
                }
                else if (activeCheck.activeSelf == true && isok == false)
                {
                    isok = true;
                    BuffStatsAdd();
                }

                break;
        }
    }


    //// Ad������ ������ �����°Ŷ� ���⼭ ���� 0���� �ٲ������
    ///
    private void OnEnable()
    {
        switch (whichBuff) 
        {
            case buffType.AD_ATK: //�ΰ��� ȭ�� ������ ���ݷ�����
                GameStatus.inst.BuffAddAdATK = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.TotalAtk.ToString(), 8); ;
                break;

            case buffType.NewBie:
                GameStatus.inst.NewbieATKBuffValue = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.TotalAtk.ToString(), 4);
                GameStatus.inst.NewbieAttackSpeed = 0.2f;
                GameStatus.inst.NewbieGoldBuffValue = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.GetTotalGold(), 4);
                GameStatus.inst.NewbieMoveSpeedBuffValue = 0.5f;
                break;
        }
       
    }
    private void OnDisable()
    {
        switch (whichBuff)
        {
            case buffType.AD_ATK: //�ΰ��� ȭ�� ������ ���ݷ�����
                GameStatus.inst.BuffAddAdATK = "0";
                break;

            case buffType.NewBie:
                GameStatus.inst.NewbieATKBuffValue = "0";
                GameStatus.inst.NewbieAttackSpeed = 0f;
                GameStatus.inst.NewbieGoldBuffValue = "0";
                GameStatus.inst.NewbieMoveSpeedBuffValue = 0f;
                break;
        }

        if (whichBuff == buffType.AD_ATK)
        {
          
        }
    }

    private void BuffStatsAdd()
    {
        switch (whichBuff)
        {
            case buffType.ATK: // ����â ���ݷ�����
                GameStatus.inst.BuffAddATK = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.TotalAtk.ToString(), 4);
                break;

            case buffType.Gold: // ��� ȹ�淮����
                GameStatus.inst.BuffAddGold = CalCulator.inst.StringAndIntMultiPly(GameStatus.inst.GetTotalGold(), 2);
                break;

            case buffType.Speed: // �̵��ӵ� ����
                GameStatus.inst.BuffAddSpeed = 1f;
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
                GameStatus.inst.BuffAddSpeed = 0f;
                break;

        }
    }



}
