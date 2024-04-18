using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMarket : MonoBehaviour
{
    public static AdMarket inst;

    GameObject frontUIRef, adMarketRef;
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        frontUIRef = GameManager.inst.FrontUiRef;
        adMarketRef = frontUIRef.transform.Find("AdMarket").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveAdMarket(bool value)
    {
        if (value)
        {
            adMarketRef.SetActive(true);
        }
        else
        {
            adMarketRef.SetActive(false);
        }

    }
}
