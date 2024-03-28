using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffBtns : MonoBehaviour
{
    Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            WorldUI_Manager.inst.buffSelectUIWindowAcitve(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
