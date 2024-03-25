using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldActionPrefabs : MonoBehaviour
{
    ParticleSystem Ps;

    private void Awake()
    {
        Ps = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * 6f; 
    }

    
}
