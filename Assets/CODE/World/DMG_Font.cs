using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class DMG_Font : MonoBehaviour
{
    [SerializeField] float speed = 50f;
    [SerializeField] bool isCritical;
    float stayTime = 1.25f;
    float timeCount = 0;
    Vector2 originPos;

    TMP_Text Dmg_Text;
    Color originColor;
    void Start()
    {
        originColor = Dmg_Text.color;
        originPos = transform.position; 
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
            transform.position += (Vector3.up * 40) * Time.deltaTime * speed;
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
        Dmg_Text.text = text;
    
        if (cri)
        {
            isCritical = true;
            Dmg_Text.color = Color.red;
        }
    }
    
}
