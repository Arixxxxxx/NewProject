using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bamboo : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

    }
    

    //드랍속도 조절
    public void RandomDropSpeed()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        rb.gravityScale = Random.Range(0.3f, 0.8f);
        StartCoroutine(AddTorques());
    }
    //회전
    IEnumerator AddTorques()
    {
        yield return null;
        rb.AddTorque(3f, ForceMode2D.Impulse);
    }


    //측면  던지기
    public void AddForceImpers(int leftorright)
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0.8f;
        StartCoroutine(him(leftorright));
    }

    IEnumerator him(int leftorright)
    {
        yield return null;

        rb.AddTorque(7f, ForceMode2D.Impulse);

        float force = Random.Range(2f, 4f);
        if(leftorright == 0)
        {
            rb.AddForce(new Vector2(1, 1.5f) * force, ForceMode2D.Impulse);
        }
        else if(leftorright == 1)
        {
            rb.AddForce(new Vector2(-1, 1.5f) * force, ForceMode2D.Impulse);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            rb.angularVelocity = 0;
            rb.velocity = Vector2.zero;

            MiniGame_0.inst.ReturnBambooObj(this);
        }
        else if (collision.CompareTag("Box"))
        {
            MiniGame_0.inst.bambooGetParticlePlay();
            rb.angularVelocity = 0;
            rb.velocity = Vector2.zero;

            if (MiniGame_0.inst.GameStart == true)
            {
                MiniGame_0.inst.F_bamboocountUP();
            }
            MiniGame_0.inst.ReturnBambooObj(this);
        }
    }
}
