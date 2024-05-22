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
    CanvasGroup canvasGroup;
    int solting;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        itemIMG = transform.Find("itemIMG").GetComponent<Image>();
        itemText = transform.Find("ItemText").GetComponent<TMP_Text>();
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

        canvasGroup.alpha = 0;
        transform.SetAsLastSibling();

        if(!gameObject.activeInHierarchy) 
        {
            gameObject.SetActive(true);
        }
        

        StartCoroutine(Play());
    }

    WaitForSeconds endTime = new WaitForSeconds(1.25f);
    float duration = 0.5f;
    float counter = 0;
    IEnumerator Play()
    {
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

    private void A_ReturnObj()
    {
        WorldUI_Manager.inst.Return_WorldUIObjPoolingObj(gameObject, 1);
    }
}
