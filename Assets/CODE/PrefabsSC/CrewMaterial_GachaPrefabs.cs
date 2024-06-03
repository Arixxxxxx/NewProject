using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrewMaterial_GachaPrefabs : MonoBehaviour
{
    [SerializeField] Sprite[] itemCase;
    [SerializeField] Sprite[] maskSprite;
    [SerializeField] TMP_ColorGradient[] gradiantColor;
    // ũ��Ƽ�ý� ����Ʈ �κ�
    GameObject criticalRef;
    Image maskImg;
    GameObject[] bgEffect = new GameObject[2];
    TMP_Text criText;

    // ������ �̹���
    Image itemIMG, itemCaseIMG;

    TMP_Text criTopText;
    CanvasGroup bgGroup;


    TMP_Text countText;
    string[] topText = { "", "���!", "�ʴ��!" };

    private void Awake()
    {
        bgGroup = GetComponent<CanvasGroup>();
        criticalRef = transform.Find("Critical").gameObject;
        criText = criticalRef.transform.Find("TopText").GetComponent<TMP_Text>();
        maskImg = criticalRef.transform.Find("EffectMask").GetComponent<Image>();
        bgEffect[0] = maskImg.transform.GetChild(0).gameObject;
        bgEffect[1] = maskImg.transform.GetChild(1).gameObject;
        itemCaseIMG = transform.Find("Case").GetComponent<Image>();
        itemIMG = transform.Find("ItemIMG").GetComponent<Image>();
        countText = transform.Find("CountText").GetComponent<TMP_Text>();

    }

    private void AwakeInit()
    {
        bgGroup = GetComponent<CanvasGroup>();
        criticalRef = transform.Find("Critical").gameObject;
        criText = criticalRef.transform.Find("TopText").GetComponent<TMP_Text>();
        maskImg = criticalRef.transform.Find("EffectMask").GetComponent<Image>();
        bgEffect[0] = maskImg.transform.GetChild(0).gameObject;
        bgEffect[1] = maskImg.transform.GetChild(1).gameObject;
        itemCaseIMG = transform.Find("Case").GetComponent<Image>();
        itemIMG = transform.Find("ItemIMG").GetComponent<Image>();
        countText = transform.Find("CountText").GetComponent<TMP_Text>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        SpinEffect();
    }

    Vector3 rotVec;
    float spinSpeedMultipler = 10;
    private void SpinEffect() // ����Ʈ ǳ��������
    {
        if (bgEffect[0].gameObject.activeSelf)
        {
            rotVec.z += Time.deltaTime * spinSpeedMultipler;
            rotVec.z = Mathf.Repeat(rotVec.z, 360);
            bgEffect[0].transform.localEulerAngles = rotVec;
        }
        else if (bgEffect[1].gameObject.activeSelf)
        {
            rotVec.z += Time.deltaTime * spinSpeedMultipler;
            rotVec.z = Mathf.Repeat(rotVec.z, 360);
            bgEffect[1].transform.localEulerAngles = rotVec;
        }
    }

    /// <summary>
    /// �̱� ����� ������ �ʱ�ȭ
    /// </summary>
    /// <param name="type"> ����, ȭ��, ���ҽ�</param>
    /// <param name="count"> �����ۼ�������</param>
    /// <param name="CriType">�Ϲ�/���(2��)/�ʴ��(3��)</param>
    public void Set_CrewMaterialGacha(int type, int count, int CriType)
    {
        if (bgGroup == null)
        {
            AwakeInit();
        }

        bgGroup.alpha = 0f;
        
        // �⺻ �ʱ�ȭ
        itemCaseIMG.sprite = itemCase[CriType]; // �ܰ� ���̽�
        itemIMG.sprite = SpriteResource.inst.CrewMaterialIMG(type); // ������ �̹���
        countText.text = $"{count}��"; // ����
      
        // ũ��Ƽ�� ���� �ʱ�ȭ
        maskImg.sprite = maskSprite[CriType]; // ����ŷ ���� �̹���
        criText.text = topText[CriType]; // ��� ��� �ʴ��
        countText.colorGradientPreset = gradiantColor[CriType];
        switch (CriType) 
        {
            case 0:
                criticalRef.SetActive(false);
                break;

            case 1: // ���
                
                criticalRef.SetActive(true);
                bgEffect[0].SetActive(true);
                bgEffect[1].SetActive(false);
                break;

                case 2: // �ʴ��
                criticalRef.SetActive(true);
                bgEffect[0].SetActive(false);
                bgEffect[1].SetActive(true);
                WorldUI_Manager.inst.Effect_WhiteCutton(1.2f); // �Ͼ�ȭ�� ����Ʈ
                break;
        }

        // �� ������
        transform.SetAsLastSibling();

        gameObject.SetActive(true);
        AudioManager.inst.Play_Ui_SFX(7, 0.6f);
        StartCoroutine(PlayAction());
    }
    float duration = 0.2f;
    float counter = 0f;
    IEnumerator PlayAction()
    {
        counter = 0;

       while (counter < duration)
        {
            bgGroup.alpha = Mathf.Lerp(0,1 , counter / duration);
            counter += Time.deltaTime;
            yield return null;
        }

        bgGroup.alpha = 1f;
    }


    public void ReturnPrefabs()
    {
        Shop_Gacha.inst.CrewMaterial_ReturnObj_ToPool(this);
    }
}
