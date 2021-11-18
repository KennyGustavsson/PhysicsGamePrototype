using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBounce : MonoBehaviour
{
    private Rigidbody2D rb;
    bool triggered;
    bool addforce;
    float Timer = 0.1f;
    public float DestroyTimer = 15.0f;
    public float Fireballdirection = -50f;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        triggered = true;
        Timer = 0.1f;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(DestroyTimer <= 0 && triggered == true)
        {
            Destroy(gameObject);
        }
        else
        {
            DestroyTimer -= Time.deltaTime;
        }
        if (Timer > 0 && triggered == true)
        {
            Timer -= Time.deltaTime;
            addforce = true;
        }
        else
        {
            addforce = false;
        }
        if (addforce)
        {
        rb.AddForce(new Vector2(Fireballdirection, 400), ForceMode2D.Force);
        }
    }
}