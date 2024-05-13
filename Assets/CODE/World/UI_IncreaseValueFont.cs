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
        MoveAndHideFont();
    }

    // Font 위로 올라가면서 하이드됨
    private void MoveAndHideFont()
    {
        transform.localPosition += Vector3.up * Time.deltaTime * upSpeed;
        valueFont.color -= new Color(0, 0, 0, 0.35f) * Time.deltaTime * 2;

        if (valueFont.color.a <= 0.1f) // 알파값0되면 풀로 리턴
        {
            returnObj();
        }
    }

    // 최초 Init
    public void Set_PosAndColorInit(int index, string value)
    {
        if(valueFont == null)
        {
            valueFont = GetComponent<TMP_Text>();
        }

        valueFont.text = "+" + CalCulator.inst.StringFourDigitChanger(value);
        valueFont.color = fontColor[index];
    }

    private void returnObj()
    {
        WorldUI_Manager.inst.Return_WorldUIObjPoolingObj(gameObject,1);
    }

    


}
