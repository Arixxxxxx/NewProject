using System.Collections;
using UnityEngine;


public class MiNIGame_GameStart : MonoBehaviour
{

    void Start()
    {
      
    }


    public void A_StartFuntion()
    {
        StartCoroutine(actionFuntion());
    }
    IEnumerator actionFuntion()
    {
        MinigameManager.inst.InvokeStartCountAction();
        AudioManager.inst.SleepMode_SFX(16, 1f);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }


    public void A_invokeResultWindow()  
    {
        // °á°úÃ¢
        MinigameManager.inst.Set_ReSultValueAndActive();
        gameObject.SetActive(false);
    }

    public void A_CountSound_Number()
    {
        AudioManager.inst.SleepMode_SFX(15, 1f);
    }


}
