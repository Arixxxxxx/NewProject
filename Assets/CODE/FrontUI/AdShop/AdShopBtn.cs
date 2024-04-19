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

        //��ư Ŭ�� �Լ�
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
    /// ��ư ��Ȱ��ȭ / Ȱ��ȭ
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
        else // ���� ��� ����
        {
            btn.interactable = false;
            CooltimeRef.SetActive(true);
        }
    }

    /// <summary>
    /// ���� ��� ������ ���
    /// </summary>
    public void CheckCooltimeAndTextUpdate()
    {
        if (CooltimeRef.activeSelf == false) { return; }

        int timeInSeconds = (int)AdMarket.inst.Get_CoolTime((int)itemType);

        // �ð� ���
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)((timeInSeconds % 3600) / 60);
        int seconds = (int)(timeInSeconds % 60);

        // ��, ��, �� ������
        string timeFormatted = string.Format("{0:00} : {1:00} : {2:00}", hours, minutes, seconds);

        // UI Text�� ǥ��
        cooltimeText.text = timeFormatted;
    }

    public void TextInit()
    {
        canBuyText.text = $"���Ű��� ���� {AdMarket.inst.CurItemCount((int)itemType)}/{AdMarket.inst.MaxItemCount((int)itemType)}";
    }
  
}

