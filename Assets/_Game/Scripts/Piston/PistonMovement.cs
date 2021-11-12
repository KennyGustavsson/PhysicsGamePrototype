using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonMovement : MonoBehaviour
{
    private Rigidbody2D pistonTopRB;
    private DistanceJoint2D distanceJoint;
    [SerializeField] private float force = 3000f;
    [SerializeField] private float interval = 4f;
    [SerializeField] private bool startActive = false;
    private float timer = 0;

    private void Awake()
    {
        pistonTopRB = GetComponentInChildren<Rigidbody2D>();
        distanceJoint = GetComponentInChildren<DistanceJoint2D>();
    }

    private void Start()
    {
        if (startActive)
        {
            timer = interval / 2;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0;
            StartCoroutine(Push());            
        }
    }

    private IEnumerator Push()
    {        
        distanceJoint.enabled = true;
        pistonTopRB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        distanceJoint.enabled = false;
    }
}
