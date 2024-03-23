using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UnityEvent OnBuyCountChanged;

    [SerializeField] List<GameObject> m_listBottomUI = new List<GameObject>();
    [SerializeField] private int buyCount = 1;
    public int BuyCount
    {
        get => buyCount;
        set
        {
            buyCount = value;
            OnBuyCountChanged?.Invoke();
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {

    }

    void Update()
    {

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

    public void ClickBuyCountBtn(int count)
    {
        BuyCount = count;
    }
}
