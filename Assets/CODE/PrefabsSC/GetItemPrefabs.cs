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
    int solting;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        itemIMG = transform.Find("itemIMG").GetComponent<Image>();
        itemText = transform.Find("ItemText").GetComponent<TMP_Text>();

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

        StartCoroutine(Play());
    }

    WaitForSeconds endTime = new WaitForSeconds(2f);
    IEnumerator Play()
    {
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
        WorldUI_Manager.inst.GetTrs_VerticalLayOutActive(true);
        yield return null;
        WorldUI_Manager.inst.GetTrs_VerticalLayOutActive(false);
        anim.SetTrigger("Play");
        yield return endTime;
        anim.SetTrigger("End");
    }

    private void A_ReturnObj()
    {
        WorldUI_Manager.inst.Return_WorldUIObjPoolingObj(gameObject, 1);
    }
}
