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
        if(isCritical)
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
        }

        timeCount += Time.deltaTime;
        if(timeCount > stayTime)
        {
            timeCount = 0;
            ActionManager.inst.Set_Pooling_Prefabs(gameObject, 0);
            transform.position = originPos;
            isCritical = false;
        }
    }

    public void SetText(string text, bool cri)
    {
         if (cri)
        {
            Dmg_Text.text = $"Hit! {text}";
            transform.position += Vector3.up * 0.5f;
            isCritical = true;
            Dmg_Text.color = Color.red;
        }
        else
        {
            Debug.Log("≥Î≈©∏Æ");
            Dmg_Text.color = originColor;
            Dmg_Text.text = text;
        }
    }
    
}
