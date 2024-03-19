using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] List<GameObject> m_listBottomUI = new List<GameObject>();
    [SerializeField] float m_base;
    [SerializeField] float m_Level;
    [SerializeField] float m_Bounus;
    [SerializeField] float m_frequency;
    [SerializeField] float Min_C { get; set; }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void testratio()
    {
        float ratio = Mathf.Log(m_Level,m_base) + Min_C;
        Debug.Log(ratio);
    }

    public void ClickBotBtn(float _num)
    {
        int count = m_listBottomUI.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (iNum == _num)
            {
                m_listBottomUI[iNum].SetActive(true);
            }
            else
            {
                m_listBottomUI[iNum].SetActive(false);
            }
        }
    }

    public void ClickOpenThisTab(GameObject _obj)
    {
        _obj.SetActive(true);
    }

    public void ClickCloseThisTab(GameObject _obj)
    {
        _obj.SetActive(false);
    }
}
