using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventRewardContent : MonoBehaviour
{
    public static WorldEventRewardContent inst;


    // Ref
    GameObject worldUiRef, frontUIRef, eventBoxRef;
    Vector3 startPos, endPos;
    FlyEventPrefabs boxSc;
    [Header("# Check Box Spawn Time �� ( Read Only )")]
    [Space]
    [SerializeField] float reSpawnTimer;
    [Header("# Setup Respawn Range <Color=yellow>(ToolTip include)</color>")]
    [Tooltip("BoxPopupTimeRange for GameStart\nGameStart X = RangeMin Second Sec \nY = RangeMin Second")]
    [Space]
    [SerializeField] Vector2 startTimeRange;
    [Tooltip("BoxPopupTimeRange for Runtime\nGameStart X = RangeMin Second Sec \nY = RangeMin Second")]
    [Space]
    [SerializeField] Vector2 clickTimeRange;

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
        boxSc = eventBoxRef.GetComponent<FlyEventPrefabs>();
        reSpawnTimer = UnityEngine.Random.Range(startTimeRange.x, startTimeRange.y);

    }
    void Start()
    {

    }
    void Update()
    {
        // �ʹ����
        BoxPostionCheker();
        // ������
        reSpawnCheker();
    }

    public void EventActive(int type)
    {
        switch (type)
        {
            // ��� ���� 2��
            case 0:
                BuffManager.inst.ActiveBuff(type, 1, "���ݷ�x5 ���� 1��");
                break;

            case 1:
                BuffManager.inst.ActiveBuff(type, 1, "�̼����� ���� 1��");
                break;

            case 2:
                BuffManager.inst.ActiveBuff(type, 1, "���ȹ������ ���� 1��");
                break;

            //��� (������û��)
            case 3:
                //�����â ����
                ADViewManager.inst.ActiveQuestionWindow(true, 1, 0, "��� +100", () => {
                    //��������
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
                    //��������
                    ADViewManager.inst.SampleAD_Active_Funtion(() =>
                    {
                        GameStatus.inst.PlusGold(curgold);
                        curgold = CalCulator.inst.StringFourDigitAddFloatChanger(curgold);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(1), $"��� +{curgold}");
                    });
                });
                break;
        }

        eventBoxReset();

    }

    private void BoxPostionCheker()
    {
        if (eventBoxRef.transform.position.x > endPos.x && eventBoxRef.activeSelf)
        {
            eventBoxReset();
        }

        
    }

    private void reSpawnCheker()
    {
        if(reSpawnTimer > 0 && eventBoxRef.activeSelf == false)
        {
            reSpawnTimer -= Time.deltaTime;
        }
        else if(reSpawnTimer <= 0 && eventBoxRef.activeSelf == false)
        {
            reSpawnTimer = 0;
            eventBoxRef.SetActive(true);
        }
    }

    /// <summary>
    /// �ڽ� ��ġ �ʱ�ȭ �� ��Ƽ��False
    /// </summary>
    public void eventBoxReset()
    {
        reSpawnTimer += UnityEngine.Random.Range(clickTimeRange.x, clickTimeRange.y);
        eventBoxRef.gameObject.SetActive(false);
        boxSc.ResetVelocity(); // �ӵ��� �ʱ�ȭ
        eventBoxRef.transform.localPosition = startPos; // ��ġ�ʱ�ȭ
        
        //�ð��־��� �ð��ٵǸ� ����
    }

   
}