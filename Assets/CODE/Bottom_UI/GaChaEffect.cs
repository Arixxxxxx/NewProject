using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaChaEffect : MonoBehaviour
{
    [SerializeField] Image resultImage;
    [SerializeField] Image Light;
    [SerializeField] GameObject Box;
    [SerializeField] Animator anim;
    Color LegendColor = new Vector4(1, 0.6409776f, 0, 1);
    Color EpicColor = new Vector4(0.8192263f, 0, 1, 1);
    Color RareColor = new Vector4(0, 0.6498866f, 1, 1);
    [SerializeField] Button Openbtn;

    void Start()
    {
        Openbtn.onClick.AddListener(() => SetTrriger());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        resultImage.gameObject.SetActive(false);
    }

    public void Setsprite(Sprite sprite, RankType rank)
    {
        resultImage.sprite = sprite;
        float baseY = resultImage.rectTransform.sizeDelta.y;
        resultImage.SetNativeSize();

        float ratio = resultImage.rectTransform.sizeDelta.x / resultImage.rectTransform.sizeDelta.y;

        resultImage.rectTransform.sizeDelta = new Vector2(baseY * ratio, baseY);

        switch (rank)
        {
            case RankType.Rare:
                Light.color = RareColor;
                break;
            case RankType.Epic:
                Light.color = EpicColor;
                break;
            case RankType.Legent:
                Light.color = LegendColor;
                break;
        }
    }

    public void SetTrriger()
    {
        anim.SetTrigger("Open");
    }

    public Button GetOpenBtn()
    {
        return Openbtn;
    }
}
