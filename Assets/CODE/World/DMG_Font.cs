using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class DMG_Font : MonoBehaviour
{
    float speed = 0.6f;
    [SerializeField] bool isCritical;
    float stayTime = 1.25f;
    float timeCount = 0;
    Vector2 originPos;
    [SerializeField] TMP_FontAsset[] fonts;
    [SerializeField] float hideSpeed;
    TMP_Text Dmg_Text;
    [SerializeField] Color originColor;

    [Header("#Input Gradients Preset")]
    [Space]
    [SerializeField][Tooltip("0����ĳ���� �⺻ / 1����ĳ���� ũ�� / 2��ź / 3��� ")] TMP_ColorGradient[] gradients;

    private void Awake()
    {
        originPos = transform.position;
    }
    void Start()
    {
        
    }

    private void OnEnable()
    {

        timeCount = 0;

        if (Dmg_Text == null)
        {
            Dmg_Text = GetComponent<TMP_Text>();
        }
    }

    // Update is called once per frame

    void Update()
    {
        if (isCritical)
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
        }
        else
        {
            Dmg_Text.color -= new Color(0, 0, 0, 0.25f) * Time.deltaTime * hideSpeed; ;
        }

        timeCount += Time.deltaTime;
        if (timeCount > stayTime)
        {
            timeCount = 0;
            Dmg_Text.colorGradientPreset = null;
            ActionManager.inst.Return_Pooling_Prefabs(gameObject, 0);
            transform.position = originPos;
            isCritical = false;
        }
    }

    float defaultFontSize = 26f;
    float critiCalFontSize = 32f;

    public void SetText(string text, bool cri, int colorType)
    {
        Dmg_Text.color = originColor;
        Dmg_Text.fontSize = defaultFontSize;

        if (cri)
        {
            Dmg_Text.colorGradientPreset = gradients[1];
            Dmg_Text.font = fonts[1];
            Dmg_Text.fontSize = critiCalFontSize;
            Dmg_Text.text = $"<b>H.I.T {text}</b>";
            transform.position += Vector3.up * 1.3f;
            isCritical = true;
        }
        else
        {
            Dmg_Text.colorGradientPreset = gradients[0];
            transform.position = new Vector3(transform.position.x + Random.Range(-0.8f, 0.8f), transform.position.y + Random.Range(-0.35f, 1.2f));
            Dmg_Text.font = fonts[1];
            Dmg_Text.fontSize = defaultFontSize;
            Dmg_Text.text = $"<b>{text}</b>";
        }

        if (colorType == 0) // ��ź��
        {
            Dmg_Text.colorGradientPreset = gradients[2];
        }
        else if (colorType == 1) //���
        {
            Dmg_Text.colorGradientPreset = gradients[3];
        }
    }

}
