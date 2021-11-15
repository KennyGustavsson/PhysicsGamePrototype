using System;
using UnityEngine;

public class PlatformPhysics : MonoBehaviour
{
    public float PlatformTimer = 0.5f;

    private Rigidbody2D rigidbody;
    [NonSerialized] public bool StartTimer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (StartTimer == true)
        {
            PlatformTimer -= Time.deltaTime;
        }
        if (PlatformTimer <= 0)
        {
            rigidbody.freezeRotation = false;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        StartTimer = true;
    }
}