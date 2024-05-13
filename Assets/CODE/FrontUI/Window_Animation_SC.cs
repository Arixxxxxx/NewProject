using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Animation_SC : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 버프 애니메이션용
    private void Buff_UI_Active_False() => BuffManager.inst.MainWindow_Active_False();
}
