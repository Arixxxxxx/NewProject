using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepMode : MonoBehaviour
{
    public static SleepMode inst;

    CanvasGroup sleepModeRef;
    GameObject sleepPanda, wakeupPanda;
    private void Awake()
    {
        //
        #region
        if (inst == null)
        {
            inst  = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion 

        sleepModeRef = GameManager.inst.FrontUiRef.transform.Find("SleepMode").GetComponent<CanvasGroup>();
        sleepPanda = sleepModeRef.transform.Find("BG/Panda/Sleep").gameObject;
        wakeupPanda = sleepModeRef.transform.Find("BG/Panda/WakeUp").gameObject;
         
    }
    void Start()
    {
        
    }
    public void Active_SleepMode(bool value)
    {
        if (value)
        {
            PandaIMGChanger(0);
            sleepModeRef.alpha = 1f;
            sleepModeRef.gameObject.SetActive(true);
        }
        else
        {
            sleepModeRef.gameObject.SetActive(false);
        }
    }


    public void Active_SleepMode(bool value, RectTransform rect)
    {
        if (value)
        {
            sleepModeRef.alpha = 1f;
            sleepModeRef.gameObject.SetActive(true);
        }
        else 
        {
  
            StartCoroutine(SleepModeEnd(rect)); 
        }
    }

    float duration = 0.75f;
    float timer = 0;
    IEnumerator SleepModeEnd(RectTransform rect)
    {
        timer = 0;

        while (timer < duration)
        {
            sleepModeRef.alpha = Mathf.Lerp(1f, 0f, timer/duration);
            timer += Time.deltaTime;
            yield return null;
        }

        sleepModeRef.alpha = 0f;
        rect.anchoredPosition = Vector2.zero; // 슬라이더 위치 초기화
        sleepModeRef.gameObject.SetActive(false);
        PandaIMGChanger(0);
    }

    public void PandaIMGChanger(int type)
    {
        switch (type) 
        {
            case 0:
                sleepPanda.SetActive(true);
                wakeupPanda.SetActive(false);
                break;

            case 1:
                sleepPanda.SetActive(false);
                wakeupPanda.SetActive(true);
                break;
        }

    }
}
