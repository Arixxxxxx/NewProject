using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    //Ÿ ��ũ��Ʈ���� ��ã�Ƶ��ǰ� ã�Ƽ� ������
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
