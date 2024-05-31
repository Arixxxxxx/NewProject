using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raw_Prefabs : MonoBehaviour
{
    [SerializeField] Sprite[] itemIMG;
    ParticleSystemRenderer[] psEffect = new ParticleSystemRenderer[2];
    
    SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        psEffect[0] = transform.Find("Effect/Epic").GetComponent<ParticleSystemRenderer>();
        psEffect[1] = transform.Find("Effect/Legend").GetComponent<ParticleSystemRenderer>();
    }
    void Start()
    {
        
    }
    [SerializeField]
    int itemtype = 0;
    [SerializeField]
    int getNumter = 0;
    public void Set_Prefabs(int relicNum, int MakeCount)
    {
        if(sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
            psEffect[0] = transform.Find("Effect/Epic").GetComponent<ParticleSystemRenderer>();
            psEffect[1] = transform.Find("Effect/Legend").GetComponent<ParticleSystemRenderer>();
        }

        itemtype = 0;
        getNumter = relicNum;

        sr.sortingOrder = 0;

        psEffect[0].gameObject.SetActive(false);
        psEffect[1].gameObject.SetActive(false);

        if (relicNum >= 0 && relicNum < 4)
        {
            itemtype = 0;
         
        }
        else if (relicNum >= 4 && relicNum < 7)
        {
            itemtype = 1;
            psEffect[0].gameObject.SetActive(true);
            psEffect[0].sortingOrder = MakeCount + 1;
        }
        else
        {
            itemtype = 2;
            psEffect[1].gameObject.SetActive(true);
            psEffect[1].sortingOrder = MakeCount + 1;
        }
        
        sr.sprite = itemIMG[itemtype];
        sr.sortingOrder = MakeCount;
        float randomX = Random.Range(-1.8f, 1.8f);
        float randomY = 1f;
        transform.localPosition = new Vector2(randomX, randomY);

        gameObject.SetActive(true);
    }

    public void ReturnPrefabs()
    {
        Shop_Gacha.inst.Relic_ReturnObj_ToPool(this);
    }
}
