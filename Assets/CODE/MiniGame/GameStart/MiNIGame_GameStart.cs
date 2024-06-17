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
        AudioManager.inst.Play_Ui_SFX(16, 1f);
        AudioManager.inst.Play_Ui_SFX(20, 1f);
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
        AudioManager.inst.Play_Ui_SFX(15, 1f);
    }

    public void A_CountSound_Talk()
    {
        AudioManager.inst.Play_Ui_SFX(19, 1f);
    }


}
