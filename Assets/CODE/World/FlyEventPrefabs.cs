using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEventPrefabs : MonoBehaviour
{
  

    Transform lightEffect;
    Rigidbody2D rb;

    [Header("# Input Box WaveSpeed <Color=yellow>( Float Data )</Color>")]
    [Tooltip("속도, 속도주기, 높이")]
    [Space]
    [SerializeField] float speed;
    [SerializeField] float frequency;
    [SerializeField] float waveheight;
    [Header("# Input Light Effect Value <Color=yellow>( Float Data )</Color>")]
    [Tooltip("속도, 속도주기, 높이")]
    [Space]
    [SerializeField] float speeds = 1;
    [SerializeField] float height = 0.8f;
    // 이동 벡터
    Vector3 rotateVec;
    Vector3 waveVec;
    Vector3 scaleVec;
    float scaleValue;




    private void Awake()
    {
        lightEffect = transform.Find("Body/Light").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

    }



    void Update()
    {
        if (gameObject.activeSelf)
        {
            // 박스 뒷 라이트
            rotateVec.z = Time.deltaTime * 200f;
            rotateVec.z = Mathf.Repeat(rotateVec.z, 360);
            scaleValue = Mathf.PingPong(Time.time * speeds, height);
            lightEffect.transform.Rotate(rotateVec);

            scaleVec.x = scaleValue + 2.5f;
            scaleVec.y = scaleValue + 2.5f;
            lightEffect.localScale = scaleVec;

            // 박스 본체 움직임
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
