using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStatus : MonoBehaviour
{
    [SerializeField] int Number;
    [SerializeField] int Lv;
    [SerializeField] float baseCost;
    [SerializeField] float nextCost;
    [SerializeField] float growthRate;
    [SerializeField] float initialProd;
    [SerializeField] float powNum;
    [SerializeField] float totalProd;
    
    // Start is called before the first frame update
    void Start()
    {
        initValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initValue()
    {
        powNum = 0;
        for (int iNum = 0; iNum <= Number; iNum++)
        {
            powNum +=  0.5f * iNum;
        }
        initialProd = 1.67f * Mathf.Pow(10, powNum);
        baseCost = initialProd * 2.56f;
        nextCost = baseCost * Mathf.Pow(growthRate, Lv);
        totalProd = initialProd * Lv;
    }

    public void setCost()
    {
        Lv++;
        totalProd = initialProd * Lv;
        nextCost = baseCost * Mathf.Pow(growthRate, Lv);
    }
}
