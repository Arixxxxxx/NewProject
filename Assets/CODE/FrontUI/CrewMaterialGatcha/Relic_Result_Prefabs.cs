using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Relic_Result_Prefabs : MonoBehaviour
{
    [Header("# Input Spirte !!")]
    [Space]
    [SerializeField] Sprite[] caseSprite;
    [SerializeField] Sprite[] maskSprite;
    [SerializeField] TMP_ColorGradient[] textGradientAssets;

    CanvasGroup canvasGroup;
    Image caseIMG, maskIMG, itemIMG;
    GameObject[] effectRef = new GameObject[2];
    TMP_Text top_Text, number_text;

    private void Awake()
    {
        AwakeInit();
    }

    private void AwakeInit()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        caseIMG = transform.Find("Case").GetComponent<Image>();
        maskIMG = caseIMG.transform.Find("Mask").GetComponent<Image>();
        itemIMG = caseIMG.transform.Find("ItemIMG").GetComponent<Image>();
        effectRef[0] = maskIMG.transform.GetChild(0).gameObject;
        effectRef[1] = maskIMG.transform.GetChild(1).gameObject;
        top_Text = transform.Find("TopText").GetComponent<TMP_Text>();
        number_text = transform.Find("NumberText").GetComponent<TMP_Text>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpinEffect();
    }

    Vector3 rotVec;
    float spinSpeedMultipler = 10;
    private void SpinEffect() // 이펙트 풍차돌리기
    {
        if (effectRef[0].gameObject.activeSelf)
        {
            rotVec.z += Time.deltaTime * spinSpeedMultipler;
            rotVec.z = Mathf.Repeat(rotVec.z, 360);
            effectRef[0].transform.localEulerAngles = rotVec;
        }
        else if (effectRef[1].gameObject.activeSelf)
        {
            rotVec.z += Time.deltaTime * spinSpeedMultipler;
            rotVec.z = Mathf.Repeat(rotVec.z, 360);
            effectRef[1].transform.localEulerAngles = rotVec;
        }
    }

    int itemtype = 0;
    string[] topText = { "일반","에픽", "전설" };
    public void Set_RelicResultPrefabs(int relicNum)
    {
        //초기화
        if(canvasGroup == null)
        {
            AwakeInit();
        }

        canvasGroup.alpha = 0f;
        effectRef[0].gameObject.SetActive(false);
        effectRef[1].gameObject.SetActive(false);

        //유물번호로 타입구분
        if (relicNum >= 0 && relicNum < 4)
        {
            itemtype = 0;

        }
        else if (relicNum >= 4 && relicNum < 7)
        {
            itemtype = 1;
            effectRef[0].gameObject.SetActive(true);
         }
        else
        {
            itemtype = 2;
            effectRef[1].gameObject.SetActive(true);
         }

        caseIMG.sprite = caseSprite[itemtype];
        maskIMG.sprite = maskSprite[itemtype];
        itemIMG.sprite = SpriteResource.inst.Relic_SpriteNumber(relicNum);
        
        top_Text.colorGradientPreset = textGradientAssets[itemtype];
        top_Text.text = topText[itemtype];
        number_text.text = $"No.{relicNum+1:D2}";
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
        AudioManager.inst.Play_Ui_SFX(7,0.6f);
        StartCoroutine(PlayAction());
    }

    float duration = 0.2f;
    float counter = 0f;
    IEnumerator PlayAction()
    {
        counter = 0;

        while (counter < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, counter / duration);
            counter += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public void ReturnPrefabs()
    {
        Shop_Gacha.inst.Relic_ReturnObj_ToPool(this);
    }
}
