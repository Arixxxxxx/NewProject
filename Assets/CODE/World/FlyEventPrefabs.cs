using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEventPrefabs : MonoBehaviour
{
    // ¿Ãµø
    Transform lightEffect;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float frequency;
    [SerializeField] float waveheight;

    private void Awake()
    {
        lightEffect = transform.Find("wing/bone_1/bone_2/Light").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
     
    }

    Vector3 rotateVec;
    Vector3 waveVec;
    void Update()
    {
        if(gameObject.activeSelf)
        {
            rotateVec.z += Time.deltaTime * 1;
            rotateVec.z = Mathf.Repeat(rotateVec.z, 360);
            lightEffect.transform.Rotate(rotateVec);
            rb.velocity = new Vector3(1 * speed, 1 * Mathf.Sin(Time.time * frequency) * waveheight);
        }
    }

    private void OnMouseDown()
    {
        int randomType = Random.Range(0, 5);
        WorldEventRewardContent.inst.EventActive(randomType);
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
    }
}
