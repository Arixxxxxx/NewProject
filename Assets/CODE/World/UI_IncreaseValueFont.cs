using TMPro;
using UnityEngine;

public class UI_IncreaseValueFont : MonoBehaviour
{
    TMP_Text valueFont;
    [SerializeField] Color[] fontColor;
    [SerializeField] Transform[] startPos;
    [SerializeField] float upSpeed;
    [SerializeField] float durationTime;
    RectTransform rectTransform;
    private void Awake()
    {
        valueFont = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.localPosition += Vector3.up * Time.deltaTime * upSpeed;
        valueFont.color -= new Color(0, 0, 0, 0.35f) * Time.deltaTime * 2;
        if (valueFont.color.a == 0)
        {
            gameObject.SetActive(false);
        }
        // 풀링 
    }
    private void returnObj()
    {
        // 리턴 
    }

    public void Set_PosAndColor(int index)
    {
        transform.position = startPos[index].position;
        valueFont.color = fontColor[index];
    }


}
