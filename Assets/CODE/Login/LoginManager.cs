using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public static LoginManager inst;

    //Ref

    GameObject worldRef, canvasRef;


    GameObject loginRef, taptoScrrenRef;

    
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

        worldRef = GameObject.Find("World").gameObject;
        canvasRef = GameObject.Find("Canvas").gameObject;

        loginRef = canvasRef.transform.Find("SignIn").gameObject;
        taptoScrrenRef = canvasRef.transform.Find("TapToScreen").gameObject;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStep()
    {
        if (taptoScrrenRef.gameObject.activeSelf)
        {
            if (DataManager.inst.IshaveJsonFile == false)
            {
                taptoScrrenRef.SetActive(false);
                loginRef.SetActive(true);
            }
            else if(DataManager.inst.IshaveJsonFile == true)
            {
                LoadingManager.LoadScene(2);
            }
        }

        
    }
}
