using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PadContoller : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum WhatInput
    {
        Up, Down, Left, Right, A, B, Select, Start
    }

    [Header("<color=yellow># Select EnumType !! ")]
    [Space]
    public WhatInput inputBtn;
    

    Color onclickBtn = new Color(0.78f, 0.78f, 0.78f, 1);
    Image thisIMG;
    private void Awake()
    {
        thisIMG = GetComponent<Image>();

        switch (inputBtn) 
        {
          case WhatInput.Up:
            case WhatInput.Down:
            case WhatInput.Right:
            case WhatInput.Left:
                thisIMG = transform.GetComponent<Image>();
                break;
        }

    }
    void Start()
    {

    }



    public void OnPointerDown(PointerEventData eventData)
    {
        thisIMG.color = onclickBtn;

        switch (inputBtn)
        {
            case WhatInput.Up:
                    MinigameController.inst.Up = true;
                break;
            case WhatInput.Down:
                MinigameController.inst.Down = true;
                break;
            case WhatInput.Left:
                MinigameController.inst.Left = true;
                break;
            case WhatInput.Right:
                MinigameController.inst.Right = true;
                break;
            case WhatInput.A:
                MinigameController.inst.Abtn = true;
                break;
            case WhatInput.B:
                MinigameController.inst.Bbtn = true;
                break;
            case WhatInput.Select:
                MinigameController.inst.SelectBtn = true;
                break;
            case WhatInput.Start:
                MinigameController.inst.StartBtn = true;
                break;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {

        thisIMG.color = Color.white;

        switch (inputBtn)
        {
            case WhatInput.Up:
                MinigameController.inst.Up = false;
                break;
            case WhatInput.Down:
                MinigameController.inst.Down = false;
                break;
            case WhatInput.Left:
                MinigameController.inst.Left = false;
                break;
            case WhatInput.Right:
                MinigameController.inst.Right = false;
                break;
            case WhatInput.A:
                MinigameController.inst.Abtn = false;
                break;
            case WhatInput.B:
                MinigameController.inst.Bbtn = false;
                break;
            case WhatInput.Select:
                MinigameController.inst.SelectBtn = false;
                break;
            case WhatInput.Start:
                MinigameController.inst.StartBtn = false;
                break;
        }
    }
}
