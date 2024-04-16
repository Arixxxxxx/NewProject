using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxPrefabs : MonoBehaviour
{
    // 내부 정보 값
    int itemType;
    int itemCount;

    Button thisBtn;
    Animator boxAnim;
    Animator boxFontAnim;

    Image materialIMG;
    TMP_Text countText;

    ParticleSystem psEffect;
    [SerializeField]
    Image boxLightEffect;

    Color normalEffectColor = new Color(1, 0.9f, 0.3f, 1f); // 노란빛
    Color criEffectColor = new Color(1, 0.3f, 0.9f, 1f); // 보라빛
    Animator screenEffectAnim;


    private void OnDisable()
    {
        boxFontAnim.gameObject.SetActive(false);
    }
    private void Awake()
    {
        AwakeInit();
    }

    void Start()
    {
        // 상자 클릭시
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
        boxLightEffect = transform.Find("Light").GetComponent<Image>();
        screenEffectAnim = transform.parent.parent.parent.Find("OpenEffect").GetComponent<Animator>();

    }

    
    /// <summary>
    /// 상자 염
    /// </summary>
    public void OpenBox()
    {
        psEffect.gameObject.SetActive(true);
        thisBtn.interactable = false;
        boxAnim.SetTrigger("Open");

        //자원생성
        CrewGatchaContent.inst.OpenCount++; //상자연 횟수 저장
        CrewGatchaContent.inst.MaterialCountEditor(itemType, itemCount);
        // 폰트
        StartCoroutine(CreateMaterialValue());
    }

    IEnumerator CreateMaterialValue()
    {
        yield return new WaitForSeconds(0.3f);
        boxFontAnim.gameObject.SetActive(true);
        boxFontAnim.SetTrigger("Up");
    }

    /// <summary>
    ///  동료강화재료 뽑기상자 내부 초기화
    /// </summary>
    /// <param name="typeIMG"> 받는 Sprtie </param>
    /// <param name="type"> 0영혼/1뼈/2책</param>
    /// <param name="count"> 갯수 </param>
    public void Set_MaterialCount(Sprite typeIMG, int type, int count)
    {
        if (materialIMG == null)
        {
            AwakeInit();
        }

        itemType = type;
        itemCount = count;

        if(itemCount < 100)
        {
            boxLightEffect.color = normalEffectColor;
        }
        else if(itemCount > 100)
        {
            boxLightEffect.color = criEffectColor;
            screenEffectAnim.SetTrigger("Effect");
        }

        thisBtn.interactable = true;
        materialIMG.sprite = typeIMG; // 출력되는 아이템 이미지 초기화
        countText.text = $"x {itemCount.ToString()}";  // 출력되는 아이템 갯수 초기화
    }
}
