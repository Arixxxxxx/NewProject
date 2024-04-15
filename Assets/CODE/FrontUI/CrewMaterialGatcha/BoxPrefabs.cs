using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxPrefabs : MonoBehaviour
{
    // ���� ���� ��
    int itemType;
    int itemCount;

    Button thisBtn;
    Animator boxAnim;
    Animator boxFontAnim;

    Image materialIMG;
    TMP_Text countText;

    ParticleSystem psEffect;


    private void OnDisable()
    {
        boxFontAnim.gameObject.SetActive(false);
    }

    
    void Start()
    {
        // ���� Ŭ����
        thisBtn.onClick.AddListener(() => 
        {
            OpenBox();
        });

       
    }

    private void AwakeInit()
    {
        thisBtn = GetComponent<Button>();
        boxAnim = GetComponent<Animator>();
        boxFontAnim = transform.Find("GetItemIMG").GetComponent<Animator>();
        materialIMG = boxFontAnim.GetComponent<Image>();
        countText = boxFontAnim.GetComponentInChildren<TMP_Text>();
        psEffect = transform.Find("Ps").GetComponent<ParticleSystem>();
        
    }
    
    /// <summary>
    /// ���� ��
    /// </summary>
    public void OpenBox()
    {
        psEffect.gameObject.SetActive(true);
        thisBtn.interactable = false;
        boxAnim.SetTrigger("Open");
        
        

        //�ڿ�����
        CrewGatchaContent.inst.OpenCount++; //���ڿ� Ƚ�� ����
        CrewGatchaContent.inst.MaterialCountEditor(itemType, itemCount);
        // ��Ʈ
        StartCoroutine(CreateMaterialValue());
    }

    IEnumerator CreateMaterialValue()
    {
        yield return new WaitForSeconds(0.3f);
        boxFontAnim.gameObject.SetActive(true);
        boxFontAnim.SetTrigger("Up");
    }

    /// <summary>
    ///  ������ �����۸��׸����� ���ڿ������� �̹���, ���� ���
    /// </summary>
    /// <param name="typeIMG"> �޴� Sprtie </param>
    /// <param name="type"> 0��ȥ/1��/2å</param>
    /// <param name="count"> ���� </param>
    public void Set_MaterialCount(Sprite typeIMG, int type, int count)
    {
        if (materialIMG == null)
        {
            AwakeInit();
        }

        itemType = type;
        itemCount = count;
        thisBtn.interactable = true;
        materialIMG.sprite = typeIMG; // ��µǴ� ������ �̹��� �ʱ�ȭ
        countText.text = $"x {itemCount.ToString()}";  // ��µǴ� ������ ���� �ʱ�ȭ
    }
}
