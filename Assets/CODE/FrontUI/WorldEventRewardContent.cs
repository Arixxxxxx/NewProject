using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldEventRewardContent : MonoBehaviour
{
    public static WorldEventRewardContent inst;
    

    // Ref
    GameObject worldUiRef, frontUIRef, eventBoxRef;
    Vector3 startPos, endPos;





    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        worldUiRef = GameManager.inst.WorldSpaceRef;
        frontUIRef = GameManager.inst.FrontUiRef;
        eventBoxRef = worldUiRef.transform.Find("EventPresent/ClickPresent").gameObject;
        startPos = worldUiRef.transform.Find("EventPresent/Trs/Start").transform.localPosition;
        endPos = worldUiRef.transform.Find("EventPresent/Trs/End").transform.localPosition;

  
    }
    void Start()
    {

    }
    void Update()
    {
        // �ð�
        // �ʱ�ȭ
        // �˾�
    }

    public void EventActive(int type)
    {
        switch (type)
        {
            // ��� ���� 3��
            case 0:
                BuffManager.inst.ActiveBuff(type, 3, "���ݷ�x5 ���� 3��");
                break;

            case 1:
                BuffManager.inst.ActiveBuff(type, 3, "�̼����� ���� 3��");
                break;

            case 2:
                BuffManager.inst.ActiveBuff(type, 3, "���ȹ������ ���� 3��");
                break;

            //��� (�����û��)
            case 3:
                //�����â ����
                ADViewManager.inst.ActiveQuestionWindow(true, 1, 0, "��� +100", () => {
                    //������
                    ADViewManager.inst.SampleAD_Active_Funtion(() =>
                    {
                        GameStatus.inst.Ruby += 100; //����
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " ��� +100");
                    });
                });
                break;

            //��� (���� ��û��)
            case 4:

                string curgold = ActionManager.inst.Get_EnemyDeadGold();
                
                //�����â ����
                ADViewManager.inst.ActiveQuestionWindow(true, 1, 1, $"��� +{curgold}", () => {
                    //������
                    ADViewManager.inst.SampleAD_Active_Funtion(() =>
                    {
                        GameStatus.inst.PlusGold(curgold);
                        curgold = CalCulator.inst.StringFourDigitAddFloatChanger(curgold);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(1), $"��� +{curgold}");
                    });
                });
                break;
        }

        //eventBoxReset();

    }


    /// <summary>
    /// �ڽ� ��ġ �ʱ�ȭ �� ��Ƽ��False
    /// </summary>
    public void eventBoxReset()
    {
        eventBoxRef.gameObject.SetActive(false);
        eventBoxRef.transform.localPosition = startPos;
    }
}
