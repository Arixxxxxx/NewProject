using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaBox_Animator : MonoBehaviour
{
    [SerializeField] GameObject Dust;
    Queue<ParticleSystem> dustQUE = new Queue<ParticleSystem>();
    int awakeCount = 5;
    Transform psParent;

    private void Awake()
    {
        psParent = transform.Find("Ps");

        for (int index=0; index<awakeCount; index++)
        {
            MakeDustParticle();
        }

     
    }

    private void MakeDustParticle()
    {
        ParticleSystem obj = Instantiate(Dust, psParent).GetComponent<ParticleSystem>();
        obj.gameObject.SetActive(false);
        dustQUE.Enqueue(obj);
    }
    void Start()
    {
        
    }

    public void A_StartSound()
    {
        AudioManager.inst.Play_Ui_SFX(5, 1f);
    }
    public void A_PlayDustPS()
    {
        if (dustQUE.Count <= 0)
        {
            MakeDustParticle();
        }

        StartCoroutine(Play());
    }


    WaitForSeconds times = new WaitForSeconds(2.5f);
    IEnumerator Play()
    {
        ParticleSystem dustPsObj = dustQUE.Dequeue();
        dustPsObj.gameObject.SetActive(true);
        AudioManager.inst.Play_Ui_SFX(6, 1f);
        yield return times;
        dustPsObj.gameObject.SetActive(false);
        dustQUE.Enqueue(dustPsObj);
    }
    
    public void A_ChangeComplete()
    {
        Shop_Gacha.inst.isChange = false;
    }

    public void AllParticleActiveFalse()
    {
        for (int index = 0; index < psParent.childCount; index++)
        {
            if (psParent.GetChild(index).gameObject.activeSelf)
            {
                ParticleSystem obj = psParent.GetChild(index).gameObject.GetComponent<ParticleSystem>();
                obj.gameObject.SetActive(false);
                dustQUE.Enqueue(obj);
            }
        }
    }
}
