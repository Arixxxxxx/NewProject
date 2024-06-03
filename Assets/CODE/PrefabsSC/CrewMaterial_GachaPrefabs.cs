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
    // 크리티컬시 이펙트 부분
    GameObject criticalRef;
    Image maskImg;
    GameObject[] bgEffect = new GameObject[2];
    TMP_Text criText;

    // 아이템 이미지
    Image itemIMG, itemCaseIMG;

    TMP_Text criTopText;
    CanvasGroup bgGroup;


    TMP_Text countText;
    string[] topText = { "", "대박!", "초대박!" };

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
    private void SpinEffect() // 이펙트 풍차돌리기
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
    /// 뽑기 연출용 프리펩 초기화
    /// </summary>
    /// <param name="type"> 부적, 화약, 굴소스</param>
    /// <param name="count"> 아이템수량갯수</param>
    /// <param name="CriType">일반/대박(2배)/초대박(3배)</param>
    public void Set_CrewMaterialGacha(int type, int count, int CriType)
    {
        if (bgGroup == null)
        {
            AwakeInit();
        }

        bgGroup.alpha = 0f;
        
        // 기본 초기화
        itemCaseIMG.sprite = itemCase[CriType]; // 외각 케이스
        itemIMG.sprite = SpriteResource.inst.CrewMaterialIMG(type); // 아이템 이미지
        countText.text = $"{count}개"; // 갯수
      
        // 크리티컬 여부 초기화
        maskImg.sprite = maskSprite[CriType]; // 마스킹 내부 이미지
        criText.text = topText[CriType]; // 상단 대박 초대박
        countText.colorGradientPreset = gradiantColor[CriType];
        switch (CriType) 
        {
            case 0:
                criticalRef.SetActive(false);
                break;

            case 1: // 대박
                
                criticalRef.SetActive(true);
                bgEffect[0].SetActive(true);
                bgEffect[1].SetActive(false);
                break;

                case 2: // 초대박
                criticalRef.SetActive(true);
                bgEffect[0].SetActive(false);
                bgEffect[1].SetActive(true);
                WorldUI_Manager.inst.Effect_WhiteCutton(1.2f); // 하얀화면 이펙트
                break;
        }

        // 젤 하위로
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
