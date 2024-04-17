using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEventPrefabs : MonoBehaviour
{
    // 이동
    Transform lightEffect;

    private void Awake()
    {
        lightEffect = transform.Find("wing/bone_1/bone_2/Light").GetComponent<Transform>();
    }
    void Start()
    {
        
    }

    Vector3 rotateVec;
    void Update()
    {
        if(gameObject.activeSelf)
        {
            rotateVec.z += Time.deltaTime * 1;
            rotateVec.z = Mathf.Repeat(rotateVec.z, 360);
            lightEffect.transform.Rotate(rotateVec);
        }
    }

    private void OnMouseDown()
    {
        int randomType = Random.Range(0, 5);
        WorldEventRewardContent.inst.EventActive(randomType);
        // 종료 => 위치초기화


    }
}
