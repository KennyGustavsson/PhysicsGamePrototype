using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPhysics : MonoBehaviour
{
    public float PlatformTimer = 0.5f;

    public Rigidbody2D PlatformRef;

    bool StartTimer;
  
    void Update()
    {
        if (StartTimer == true)
        {
            PlatformTimer -= Time.deltaTime;
        }
        if (PlatformTimer <= 0)
        {
            PlatformRef.freezeRotation = false;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        StartTimer = true;
    }
}