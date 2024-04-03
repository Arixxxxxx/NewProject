using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager inst;

    

    private void Awake()
    {

        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(inst);
        }

        DontDestroyOnLoad(inst);

        Screen.SetResolution(Screen.width, Screen.width / 9 * 16, true);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
