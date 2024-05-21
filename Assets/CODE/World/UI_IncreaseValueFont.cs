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

    // Font ���� �ö󰡸鼭 ���̵��
    private void MoveAndHideFont()
    {
        transform.localPosition += Vector3.up * Time.deltaTime * upSpeed;
        valueFont.color -= new Color(0, 0, 0, 0.35f) * Time.deltaTime * 2;

        if (valueFont.color.a <= 0.1f) // ���İ�0�Ǹ� Ǯ�� ����
        {
            returnObj();
        }
    }

    // ���� Init
    public void Set_PosAndColorInit(string value)
    {
        valueFont.color = Color.white;

        if (valueFont == null)
        {
            valueFont = GetComponent<TMP_Text>();
        }

        valueFont.text = "-" + CalCulator.inst.StringFourDigitAddFloatChanger(value);
        
        gameObject.SetActive(true);
    }

    private void returnObj()
    {
        WorldUI_Manager.inst.Return_WorldUIObjPoolingObj(gameObject,2);
    }

    


}
