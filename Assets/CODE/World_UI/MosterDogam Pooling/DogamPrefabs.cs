using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogamPrefabs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.localPosition += Vector3.up * Time.deltaTime * 30f;
        }

    }

    private void A_ReturnObj()
    {
        DogamManager.inst.Return_DogamIcon(gameObject);
    }
}
