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


    private void OnDisable()
    {
        boxFontAnim.gameObject.SetActive(false);
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
    ///  생성된 아이템메테리얼을 상자열었을떄 이미지, 숫자 출력
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
        thisBtn.interactable = true;
        materialIMG.sprite = typeIMG; // 출력되는 아이템 이미지 초기화
        countText.text = $"x {itemCount.ToString()}";  // 출력되는 아이템 갯수 초기화
    }
}
