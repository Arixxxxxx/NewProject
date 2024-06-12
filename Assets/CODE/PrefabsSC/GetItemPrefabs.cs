using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetItemPrefabs : MonoBehaviour
{
    Animator anim;
    Image itemIMG;
    TMP_Text itemText;
    RectTransform textRect;
    CanvasGroup canvasGroup;
    int solting;
    RectTransform[] bgRect = new RectTransform[3];
    Vector2 orijinDelta;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        itemIMG = transform.Find("itemIMG").GetComponent<Image>();
        itemText = transform.Find("ItemText").GetComponent<TMP_Text>();
        textRect = itemText.GetComponent<RectTransform>();
        bgRect[0] = transform.Find("BG0").GetComponent<RectTransform>();
        bgRect[1] = transform.Find("BG1").GetComponent<RectTransform>();
        bgRect[2] = transform.Find("BG2").GetComponent<RectTransform>();
        orijinDelta = bgRect[0].sizeDelta;
        canvasGroup = GetComponent<CanvasGroup>();
        
    }

    void Start()
    {
        solting = transform.parent.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set_GetItemSpriteAndText(Sprite img, string Text)
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
            itemIMG = transform.Find("itemIMG").GetComponent<Image>();
            itemText = transform.Find("ItemText").GetComponent<TMP_Text>();
        }

        itemIMG.sprite = img;
        itemText.text = Text;

        counter = 0;
        canvasGroup.alpha = 0;
        transform.SetAsLastSibling();

        if(!gameObject.activeInHierarchy) 
        {
            gameObject.SetActive(true);
        }

        StopCoroutine(Play());
        StartCoroutine(Play());
    }

    WaitForSeconds endTime = new WaitForSeconds(1.25f);
    float duration = 0.5f;
    float counter = 0;
    Vector2 rectVec;
    IEnumerator Play()
    {
        yield return null;
        for (int index = 0; index < bgRect.Length; index++)
        {
            rectVec.y = bgRect[index].sizeDelta.y;
            rectVec.x = bgRect[index].sizeDelta.x + textRect.sizeDelta.x;
            bgRect[index].sizeDelta = rectVec;
        }
        WorldUI_Manager.inst.GetTrs_VerticalLayOutActive(true);
        yield return null;
        WorldUI_Manager.inst.GetTrs_VerticalLayOutActive(false);
        anim.SetTrigger("Play");

        while(counter < duration)
        {
            float alpah = Mathf.Lerp(0, 1, counter / duration);
            canvasGroup.alpha = alpah;
            counter += Time.deltaTime;
            yield return null;
        }
        counter = 0;
        yield return endTime;

        while (counter < duration)
        {
            float alpah = Mathf.Lerp(1, 0, counter / duration);
            canvasGroup.alpha = alpah;
            counter += Time.deltaTime;
            yield return null;
        }

        A_ReturnObj();
    }

    public void A_ReturnObj()
    {
        for (int index = 0; index < bgRect.Length; index++)
        {
            bgRect[index].sizeDelta = orijinDelta;
        }

        WorldUI_Manager.inst.Return_GetItemText(this);
    }
}
