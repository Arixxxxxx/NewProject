using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelicManager : MonoBehaviour
{
    Transform UiCanvas;
    Transform RelicParents;
    Button nomalRelicBtn;
    Button ancientRelicBtn;
    int RelicBtnIndex = 0;
    GameObject[] list_RelicWindow = new GameObject[2];


    void Start()
    {
        UiCanvas = GameObject.Find("---[UI Canvas]").transform;
        RelicParents = UiCanvas.Find("BackGround/Relic");
        nomalRelicBtn = RelicParents.Find("TopBtn/NormalRelicBtn").GetComponent<Button>();
        ancientRelicBtn = RelicParents.Find("TopBtn/AncientRelicBtn").GetComponent<Button>();
        list_RelicWindow[0] = RelicParents.Find("NormalScroll View").gameObject;
        list_RelicWindow[1] = RelicParents.Find("AncientScroll View").gameObject;

        initbutton();
    }

    void initbutton()
    {
        nomalRelicBtn.onClick.AddListener(() => { clickRelicBtn(0); });
        ancientRelicBtn.onClick.AddListener(() => { clickRelicBtn(1); });
    }

    void clickRelicBtn(int index)
    {
        list_RelicWindow[RelicBtnIndex].SetActive(false);
        RelicBtnIndex = index;
        list_RelicWindow[RelicBtnIndex].SetActive(true);
    }

}