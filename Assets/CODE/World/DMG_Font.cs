using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
            ActionManager.inst.Return_Pooling_Prefabs(gameObject, 0);
            transform.position = originPos;
            isCritical = false;
        }
    }

    public void SetText(string text, bool cri, int colorType)
    {
        if (cri)
        {
            Dmg_Text.font = fonts[1];
            Dmg_Text.text = $"<b>H.I.T {text}</b>";
            transform.position += Vector3.up * 1.3f;
            isCritical = true;
            Dmg_Text.color = Color.red;
        }
        else
        {
            transform.position = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.3f, 0.9f));
            Dmg_Text.font = fonts[0];
            Dmg_Text.color = originColor;
            Dmg_Text.text = $"<b>{text}</b>";
        }
        
        if(colorType == 0)
        {
            Dmg_Text.color = Color.red;
        }
        if(colorType == 1)
        {
            Dmg_Text.color = Color.cyan;
        }
    }

}
