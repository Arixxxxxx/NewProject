using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    //타 스크립트에서 안찾아도되게 찾아서 나눠줌
    GameObject worldUiRef;
    public GameObject WorldUiRef { get { return worldUiRef; } }

    GameObject frontUiRef;
    public GameObject FrontUiRef { get { return frontUiRef; } }

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

        worldUiRef = GameObject.Find("---[World UI Canvas]").gameObject;
        frontUiRef = GameObject.Find("---[FrontUICanvas]").gameObject;
    
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
