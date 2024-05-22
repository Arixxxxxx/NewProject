using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicDogam_AuraEffect : MonoBehaviour
{
     Vector3 rotVec;
    float spinSpeedMultiPlyer = 12;

    GameObject parent;

    private void Awake()
    {
        parent = transform.parent.gameObject;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (parent.activeInHierarchy)
        {
            rotVec.z = Time.deltaTime * spinSpeedMultiPlyer;
            rotVec.z = Mathf.Repeat(rotVec.z, 360);
            transform.Rotate(rotVec);
        }
    }
}
